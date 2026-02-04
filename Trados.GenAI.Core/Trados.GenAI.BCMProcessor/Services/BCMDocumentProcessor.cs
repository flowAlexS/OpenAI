using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.BCMProcessor.Interfaces;
using Trados.GenAI.BCMProcessor.Model;

namespace Trados.GenAI.BCMProcessor.Services
{
    public class BCMDocumentProcessor : IBCMProcessor
    {
        private readonly SegmentVisitor _segmentVisitor = new SegmentVisitor();

        private Document? _document;
        private Sdl.Core.Bcm.BcmModel.File? _file;
        private ParagraphUnit? _paragraphUnit;

        public bool IsValidBCM { get; private set; } = true;

        public void Initialize(Document? document)
        {
            _document = document;
            if (_document == null)
            {
                IsValidBCM = false;
                return;
            }


            _file = document?.Files.FirstOrDefault();
            if (_file == null)
            {
                IsValidBCM = false;
                return;
            }

            _paragraphUnit = _file.ParagraphUnits.FirstOrDefault();
            if (_paragraphUnit == null || _paragraphUnit.Source == null)
            {
                IsValidBCM = false;
                return;
            }
        }

        public List<IBcmData> GetTranslatableContents(bool includeTarget)
        {
            var result = new List<IBcmData>();

            if (_document == null || _file == null)
                throw new InvalidOperationException();

            foreach (var paragraphUnit in _file.ParagraphUnits)
            {
                var source = paragraphUnit?.Source;
                var target = paragraphUnit?.Target;

                if (paragraphUnit == null || source == null)
                    continue;

                var bcmData = new BcmData()
                {
                    SourceLanguage = _document.SourceLanguageCode,
                    TargetLanguage = _document.TargetLanguageCode,
                    ParagraphUnit = paragraphUnit
                };

                _segmentVisitor.VisitParagraph(source);

                bcmData.TranslatableSource = _segmentVisitor.GetXml(includeTarget);
                if (target != null)
                {
                    _segmentVisitor.VisitParagraph(target);
                    bcmData.TranslatableTarget = _segmentVisitor.GetXml(includeTarget);
                }

                var contextDefinitions = _file?.Skeleton?.ContextDefinitions;
                var contexts = _file?.Skeleton?.Contexts;
                var contextLists = paragraphUnit.ContextList;

                if (contexts is null)
                    continue;



                var applicableContextDefinitionIds =
                    contexts
                        .Where(c => contextLists.Contains(c.Id))
                        .Select(c => c.ContextDefinitionId)
                        .Distinct()
                        .ToHashSet();

                // Step 2: filter context definitions
                var filteredContextDefinitions =
                    contextDefinitions
                        ?.Where(cd => applicableContextDefinitionIds.Contains(cd.Id))
                        .ToList();

                if (filteredContextDefinitions != null && filteredContextDefinitions.Any())
                {
                    bcmData.SystemPrompt = filteredContextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "SYS")?.Description ?? string.Empty;
                    bcmData.UserPrompt = filteredContextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "SPMT")?.Description ?? string.Empty;
                    bcmData.ContextUri = filteredContextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "INC")?.Description ?? string.Empty;
                }

                result.Add(bcmData);
            }

            return result;
        }

        public IBcmData GetTranslatableContent(bool includeTarget)
        {
            var source = _paragraphUnit?.Source;
            var target = _paragraphUnit?.Target;

            if (_document == null || source == null)
                throw new InvalidOperationException();

            var bcmData = new BcmData()
            {
                SourceLanguage = _document.SourceLanguageCode,
                TargetLanguage = _document.TargetLanguageCode,
            };

            _segmentVisitor.VisitParagraph(source);

            bcmData.TranslatableSource = _segmentVisitor.GetXml(includeTarget);
            if (target != null)
            {
                _segmentVisitor.VisitParagraph(target);
                bcmData.TranslatableTarget = _segmentVisitor.GetXml(includeTarget);
            }

            var contextDefinitions = _file?.Skeleton?.ContextDefinitions;
            if (contextDefinitions != null && contextDefinitions.Any())
            {
                bcmData.SystemPrompt = contextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "SYS")?.Description ?? string.Empty;
                bcmData.UserPrompt = contextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "SPMT")?.Description ?? string.Empty;
                bcmData.ContextUri = contextDefinitions.FirstOrDefault(cd => cd.DisplayCode == "INC")?.Description ?? string.Empty;
            }

            return bcmData;
        }

        public IBCMResponse BuildDocument()
        {
            if (_document == null)
                return new BcmTranslated()
                {
                    Success = false
                };

            return new BcmTranslated()
            {
                Success = true,
                TranslatedDocument = _document

            };
        }

        public IBCMResponse TranslateDocument(List<string> translations)
        {
            if (_document == null || _file == null)
                return new BcmTranslated()
                {
                    Success = false,
                };

            var translatedSegments = BuildTranslatedSegments(translations);
            var sourceParagraph = _paragraphUnit?.Source.Clone() as Paragraph;

            _file.ParagraphUnits.Clear();
            if (sourceParagraph == null)
                return new BcmTranslated()
                {
                    Success = false,
                };

            var index = 0;
            foreach (var segment in translatedSegments )
            {
                var pu = new ParagraphUnit();
                pu.Index = index;
                pu.Id = null;
                pu.Source = sourceParagraph;
                pu.Target = new Paragraph(segment);
                _file.ParagraphUnits.Add(pu);
                index++;
            }

            return new BcmTranslated()
            {
                Success = true,
                TranslatedDocument = _document
            };
        }

        public void UpdateParagraphUnit(ParagraphUnit paragraphUnit, string translation)
        {
            if (_file?.ParagraphUnits.Count == 1)
            {
                paragraphUnit.Id = null;
            }

            var sourceParagraph = paragraphUnit.Source as Paragraph;

            if (sourceParagraph.Children.Count == 0)
                return;

            var targetParagraph = paragraphUnit.Target as Paragraph;

            Segment sourceSegment = sourceParagraph?.Children?.FirstOrDefault() as Segment ?? new Segment();
            var targetSegment = targetParagraph?.Children?.FirstOrDefault() as Segment ?? new Segment(sourceSegment.SegmentNumber);

            _segmentVisitor.VisitParagraph(paragraphUnit.Source);
            var tagMappings = _segmentVisitor.TagMappings;

            _segmentVisitor.VisitParagraph(paragraphUnit.Target);
            var targetTagMappings = _segmentVisitor.TagMappings;
            var originalElements = _segmentVisitor.Elements;

            foreach (var tagMapping in from tagMapping in targetTagMappings
                                       let item = tagMappings.FirstOrDefault(a => a.Id == tagMapping.Id)
                                       where item == null
                                       select tagMapping)
            {
                tagMappings.Add(tagMapping);
            }

            var updatedSegment = _segmentVisitor.UpdateSegment(targetSegment, translation, tagMappings);
            targetParagraph?.Clear();
            targetParagraph?.Add(updatedSegment);
        }

        private List<Segment> BuildTranslatedSegments(List<string> translations)
        {
            var segments = new List<Segment>();
            if (_paragraphUnit?.Source == null)
                return segments;

            foreach (var translation in translations)
            {
                var sourceParagraph = _paragraphUnit.Source;
                var targetParagraph = _paragraphUnit.Target;

                Segment sourceSegment = sourceParagraph?.Children?.FirstOrDefault() as Segment ?? new Segment();
                var targetSegment = targetParagraph?.Children?.FirstOrDefault() as Segment ?? new Segment(sourceSegment.SegmentNumber);

                _segmentVisitor.VisitParagraph(_paragraphUnit.Source);
                var tagMappings = _segmentVisitor.TagMappings;

                _segmentVisitor.VisitParagraph(_paragraphUnit.Target);
                var targetTagMappings = _segmentVisitor.TagMappings;
                var originalElements = _segmentVisitor.Elements;

                foreach (var tagMapping in from tagMapping in targetTagMappings
                                           let item = tagMappings.FirstOrDefault(a => a.Id == tagMapping.Id)
                                           where item == null
                                           select tagMapping)
                {
                    tagMappings.Add(tagMapping);
                }

                var updatedSegment = _segmentVisitor.UpdateSegment(targetSegment, translation, tagMappings);
                segments.Add(updatedSegment);
            }

            return segments;
        }
    }
}
