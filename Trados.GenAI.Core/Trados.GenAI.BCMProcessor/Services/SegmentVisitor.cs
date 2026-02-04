using Sdl.Core.Bcm.BcmModel.Annotations;
using Sdl.Core.Bcm.BcmModel.Common;
using Sdl.Core.Bcm.BcmModel;
using System.Text.RegularExpressions;
using System.Xml;
using Trados.GenAI.BCMProcessor.Models;

namespace Trados.GenAI.BCMProcessor.Services
{
    public class SegmentVisitor : BcmVisitor
    {
        private readonly Regex _regexNewline = new Regex(@"[\r\n]+", RegexOptions.None);
        private readonly Regex _regexPlaceable = new Regex(@"{\d+}", RegexOptions.None);

        private Stack<MarkupDataContainer> _tagPairStack;
        private Stack<Element> _elementTagPairStack;

        private List<TagMapping> _tagMappings;

        private int _index;
        private int _anchor;

        public List<TagMapping> TagMappings => _tagMappings;

        public Segment? Segment { get; set; }

        public List<Element> Elements { get; private set; }

        public string Text { get; private set; }

        public Segment UpdateSegment(Segment segment, string xml, List<TagMapping> tagMappings)
        {
            if (!(segment?.Clone() is Segment updatedSegment))
            {
                return null;
            }

            updatedSegment.Clear();

            var containers = new Stack<MarkupDataContainer>();
            containers.Push(updatedSegment);

            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;

            try
            {
                xmlDoc.LoadXml("<root>" + xml + "</root>");
            }
            catch (Exception ex)
            {
                try
                {

                    // Attempt to remove the tag pairs and continue processing the xml
                    xml = RemoveTags(xml, "TagPair");
                    xml = RemoveTags(xml, "Locked");

                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml("<root>" + xml + "</root>");
                }
                catch (Exception ex2)
                {
                    // Simply return the updated segment with invalid xml
                    //updatedSegment.Add(_segmentBuilder.Text(xml));
                    return updatedSegment;
                }
            }

            var rootNode = xmlDoc.FirstChild;
            foreach (XmlNode childNode in rootNode.ChildNodes)
            {
                GetItems(childNode, containers, tagMappings);
            }

            return updatedSegment;

        }

        private void GetItems(
            XmlNode node,
            Stack<MarkupDataContainer> containers,
            List<TagMapping> tagMappings)
        {
            if (containers == null || containers.Count <= 0)
            {
                return;
            }

            var container = containers.Peek();

            if (node.NodeType == XmlNodeType.Text || node.NodeType == XmlNodeType.Whitespace)
            {
                container.Add(new TextMarkup() { Text = node.Value });
            }
            else if (node.NodeType == XmlNodeType.Element)
            {
                var element = tagMappings.FirstOrDefault(a =>
                    string.Compare(a.Id, node.Name, StringComparison.CurrentCultureIgnoreCase) == 0);

                if (element != null)
                {
                    // TagPair
                    if (node.Name.StartsWith("TagPair", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (element.MarkupData is TagPair tp)
                        {
                            var newTp = new TagPair();
                            newTp.TagPairDefinitionId = tp.TagPairDefinitionId;
                            newTp.TagNumber = tp.TagNumber;
                            container.Add(newTp);
                            containers.Push(newTp);

                            if (node.HasChildNodes)
                            {
                                foreach (XmlNode childNode in node.ChildNodes)
                                {
                                    GetItems(childNode, containers, tagMappings);
                                }
                            }

                            containers.Pop();
                        }


                    }

                    // PlaceholderTag
                    else if (node.Name.StartsWith("Placeholder", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (element.MarkupData is PlaceholderTag pt)
                        {
                            container.Add(pt);
                        }
                    }

                    // LockedContentContainer
                    else if (node.Name.StartsWith("Locked", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (element.MarkupData is LockedContentContainer lc)
                        {
                            container.Add(lc);
                        }
                    }
                }
            }
            else if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    GetItems(childNode, containers, tagMappings);
                }
            }
        }

        private string RemoveTags(string result, string name)
        {
            var regex = new Regex(
                @"(?<lessThan>&lt;|<)\s*(?<backSlashClosing>/|)\s*(?<tagName>" + name + @"\d+)\s*(?<backSlashPlaceholder>/|)\s*(?<greaterThan>(&gt;|>))",
                RegexOptions.IgnoreCase);
            var regexMatches = regex.Matches(result);
            if (regexMatches.Count > 0)
            {
                result = regex.Replace(result, string.Empty);
            }

            return result;
        }
        public string GetXml(bool includeTags)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = true
            };

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    foreach (var element in Elements)
                    {
                        WriteSegmentXml(writer, element, includeTags);
                    }
                }

                return sw.ToString();
            }
        }

        private void WriteSegmentXml(XmlWriter writer, Element element, bool includeTags)
        {
            if (element is ElementText text)
            {
                writer.WriteString(text.Text);
            }

            if (element is ElementGenericPlaceholder)
            {
                writer.WriteStartElement(element.TagMappingId);
                writer.WriteEndElement();
            }

            if (includeTags)
            {
                if (element is ElementTagPair tag)
                {
                    switch (tag.Type)
                    {
                        case Element.TagType.TagOpen:
                            writer.WriteStartElement(element.TagMappingId);
                            break;
                        case Element.TagType.TagClose:
                            writer.WriteEndElement();
                            break;
                    }
                }

                if (element is ElementPlaceholder)
                {
                    writer.WriteStartElement(element.TagMappingId);
                    writer.WriteEndElement();
                }

                if (element is ElementLocked locked)
                {
                    switch (locked.Type)
                    {
                        case Element.TagType.TagOpen:
                            writer.WriteStartElement(element.TagMappingId);
                            break;
                        case Element.TagType.TagClose:
                            writer.WriteEndElement();
                            break;
                    }
                }
            }
        }

        public override void VisitCommentContainer(CommentContainer commentContainer)
        {
        }

        public override void VisitFeedbackContainer(FeedbackContainer feedbackContainer)
        {
        }

        public override void VisitLockedContentContainer(LockedContentContainer lockedContentContainer)
        {
            _anchor++;
            _index++;
            var tagMapping = new TagMapping
            {
                Index = _index,
                Id = "Locked" + _index,
                MarkupData = lockedContentContainer.Clone() as LockedContentContainer
            };
            _tagMappings.Add(tagMapping);

            var element = new ElementLocked
            {
                Anchor = _anchor,
                TagId = _index.ToString(),
                TagMappingId = tagMapping.Id,
                Type = Element.TagType.TagOpen
            };

            Elements.Add(element);
            _tagPairStack.Push(lockedContentContainer);
            _elementTagPairStack.Push(element);

            VisitChilderen(lockedContentContainer);

            var currentTag = _tagPairStack.Pop();
            var currentelElement = _elementTagPairStack.Pop();

            element = new ElementLocked
            {
                Anchor = currentelElement.Anchor,
                TagId = currentelElement.TagId,
                TagMappingId = currentelElement.TagMappingId,
                Type = Element.TagType.TagClose
            };

            Elements.Add(element);
        }

        public override void VisitParagraph(Paragraph paragraph)
        {
            VisitChilderen(paragraph);
        }

        public override void VisitPlaceholderTag(PlaceholderTag tag)
        {
            _anchor++;
            _index++;
            var tagMapping = new TagMapping
            {
                Index = _index,
                Id = "Placeholder" + _index,
                MarkupData = tag.Clone() as PlaceholderTag,
                TagId = tag.Id
            };
            ;
            _tagMappings.Add(tagMapping);

            var element = new ElementPlaceholder
            {
                Anchor = _anchor,
                TagMappingId = tagMapping.Id,
                TagId = tag.Id,
            };

            Elements.Add(element);
        }

        public override void VisitRevisionContainer(RevisionContainer revisionContainer)
        {
            if (revisionContainer.RevisionType == RevisionType.Inserted)
            {
                VisitChilderen(revisionContainer);
            }
        }

        public override void VisitSegment(Segment segment)
        {
            Segment = segment;
            InitializeComponents();
            VisitChilderen(segment);
        }

        public override void VisitStructure(StructureTag structureTag)
        {
        }

        public override void VisitTagPair(TagPair tagPair)
        {
            _anchor++;
            _index++;
            var tagMapping = new TagMapping
            {
                Index = _index,
                Id = "TagPair" + _index,
                MarkupData = tagPair.Clone() as TagPair,
                TagId = tagPair.Id
            };

            _tagMappings.Add(tagMapping);

            tagMapping.MarkupData = tagPair;
            var element1 = new ElementTagPair
            {
                Anchor = _anchor,
                TagMappingId = tagMapping.Id,
                Type = Element.TagType.TagOpen,
                TagId = tagPair.Id,
            };


            Elements.Add(element1);

            //Text += tagPair.StartTagProperties.TagContent;
            _tagPairStack.Push(tagPair);
            _elementTagPairStack.Push(element1);

            VisitChilderen(tagPair);

            var currentTag = _tagPairStack.Pop() as TagPair;
            var currentelElement = _elementTagPairStack.Pop();

            var element2 = new ElementTagPair
            {
                Anchor = currentelElement.Anchor,
                TagMappingId = currentelElement.TagMappingId,
                Type = Element.TagType.TagClose,
                TagId = currentelElement.TagId,
            };

            Elements.Add(element2);
        }


        //TBI
        public override void VisitTerminologyContainer(TerminologyAnnotationContainer terminologyAnnotation)
        {
            VisitChilderen(terminologyAnnotation);
        }

        public override void VisitText(TextMarkup text)
        {
            var elements = GetInlineText(text.Text);
            if (elements?.Count > 0)
            {
                Elements.AddRange(elements);
            }

            Text += text.Text;
        }

        private List<Element> GetInlineText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var elements = new List<Element>
            {
                new ElementText { Text = text }
            };

            elements = GetElements(elements, _regexNewline, "LineBreak");
            elements = GetElements(elements, _regexPlaceable, "Placeable");

            return elements;
        }

        private List<Element> GetElements(List<Element> elements, Regex regex, string type)
        {
            var elementsResult = new List<Element>();
            foreach (var element in elements)
            {
                if (element is ElementText elementText)
                {
                    var matches = regex.Matches(elementText.Text);
                    if (matches.Count > 0)
                    {
                        var startIndex = 0;
                        foreach (Match match in matches)
                        {
                            var prefix = elementText.Text.Substring(startIndex, match.Index - startIndex);
                            if (!string.IsNullOrEmpty(prefix))
                            {
                                elementsResult.Add(new ElementText { Text = prefix });
                            }

                            var matchText = elementText.Text.Substring(match.Index, match.Length);
                            //matchText = matchText.Replace("\n", "\\n").Replace("\r", "\\r");

                            _index++;
                            var tagMapping = new TagMapping
                            {
                                Index = _index,
                                Id = type + _index,
                                MarkupData = null,
                                TextEquivalent = matchText
                            };
                            _tagMappings.Add(tagMapping);

                            elementsResult.Add(new ElementGenericPlaceholder
                            {
                                Name = type,
                                TextEquivalent = matchText,
                                TagMappingId = tagMapping.Id,
                                CType = type,
                            });

                            startIndex = match.Index + match.Length;
                        }

                        if (startIndex < elementText.Text.Length)
                        {
                            var suffixText = elementText.Text.Substring(startIndex);
                            elementsResult.Add(new ElementText { Text = suffixText });
                        }
                    }
                    else
                    {
                        elementsResult.Add(element);
                    }
                }
                else
                {
                    elementsResult.Add(element);
                }
            }

            return elementsResult;
        }

        private void VisitChilderen(MarkupDataContainer container)
        {
            if (container == null)
            {
                return;
            }
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }

        private void InitializeComponents()
        {
            _index = 1;
            _anchor = 1;
            _tagMappings = new List<TagMapping>();
            _tagPairStack = new Stack<MarkupDataContainer>();
            _elementTagPairStack = new Stack<Element>();
            Elements = new List<Element>();
            Text = string.Empty;
        }
    }

}
