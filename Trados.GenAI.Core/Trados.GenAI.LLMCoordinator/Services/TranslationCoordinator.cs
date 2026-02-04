using Sdl.Core.Bcm.BcmModel;
using Sdl.Core.Bcm.BcmModel.Annotations;
using Sdl.Core.Bcm.BcmModel.Skeleton;
using Trados.GenAI.BCMProcessor.Interfaces;
using Trados.GenAI.BCMProcessor.Model;
using Trados.GenAI.BCMProcessor.Services;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.LLMCoordinator.Interfaces;
using Trados.GenAI.LLMCoordinator.Model;
using Trados.GenAI.LLMCoordinator.Utils;

namespace Trados.GenAI.LLMCoordinator.Services
{
    public class TranslationCoordinator : ITranslationCoordinator
    {
        private readonly IBCMProcessor _bcmProcessor;
        private readonly IAIModelBuilder _aiModelBuilder;
        private readonly ITranslationStorageService? _translationStorageService;
        private readonly ITranslationProvider _translationProvider;

        public TranslationCoordinator(
            IBCMProcessor bcmDocumentProcessor,
            IAIModelBuilder aIModelBuilder,
            ITranslationProvider translationProvider)
        {
            _bcmProcessor = bcmDocumentProcessor;
            _aiModelBuilder = aIModelBuilder;
            _translationProvider = translationProvider;
        }
        public TranslationCoordinator(
            IBCMProcessor bcmDocumentProcessor,
            IAIModelBuilder aIModelBuilder,
            ITranslationStorageService translationStorageService,
            ITranslationProvider translationProvider)
        {
            _bcmProcessor = bcmDocumentProcessor;
            _aiModelBuilder = aIModelBuilder;
            _translationStorageService = translationStorageService;
            _translationProvider = translationProvider;
        }

        public async Task<ITranslationResult> ExecuteAsync(
    ITranslationRequest translationRequest)
        {
            var translationResult = new TranslationResult();

            _bcmProcessor.Initialize(translationRequest.BcmDocument);
            if (!_bcmProcessor.IsValidBCM)
            {
                translationResult.Error = "Invalid document";
                return translationResult;
            }

            var bcmDataCollection =
                _bcmProcessor.GetTranslatableContents(
                    translationRequest.IncludeTags);

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
                // or a fixed number like 4–8 if API-limited
            };

            await Parallel.ForEachAsync(
                bcmDataCollection,
                options,
                async (bcm, cancellationToken) =>
                {
                    var aiModel = _aiModelBuilder.BuildAIModel(bcm);
                    aiModel.SystemInstructions =
                        VariableHelper.ReplaceVariables(
                            aiModel.SystemInstructions, aiModel);
                    aiModel.UserPrompt =
                        VariableHelper.ReplaceVariables(
                            aiModel.UserPrompt, aiModel);

                    string? translation = null;

                    if (translationRequest.UseCache && _translationStorageService != null)
                    {
                        translation =
                            await _translationStorageService
                                .RetrieveTranslationAsync(aiModel);
                    }

                    if (string.IsNullOrEmpty(translation))
                    {
                        var response =
                            await _translationProvider
                                .TranslateAsync(aiModel);

                        translation =
                            response?.TranslationUnits?
                                .FirstOrDefault()?.Translation;

                        if (!string.IsNullOrEmpty(translation) && _translationStorageService != null)
                        {
                            _ = _translationStorageService
                                .SaveTranslationAsync(aiModel, translation);
                        }
                    }

                    if (!string.IsNullOrEmpty(translation) &&
                        bcm.ParagraphUnit != null)
                    {
                        _bcmProcessor.UpdateParagraphUnit(
                            bcm.ParagraphUnit,
                            translation);
                    }
                });

            var bcmResponse = _bcmProcessor.BuildDocument();

            return bcmResponse.Success
                ? new TranslationResult
                {
                    Success = true,
                    TranslatedDocument = bcmResponse.TranslatedDocument
                }
                : new TranslationResult
                {
                    Success = false,
                    Error = "An error occurred"
                };
        }

    }
}
