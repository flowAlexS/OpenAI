using Trados.GenAI.BCMProcessor.Services;
using Trados.GenAI.LLMCoordinator.Interfaces;
using Trados.GenAI.LLMCoordinator.Services;

namespace Trados.GenAI.LLMCoordinator
{
    public class TranslationCoordinatorFactory
    {
        public static ITranslationCoordinator BuildCoordinator(
            IAIModelBuilder aiModelBuilder,
            ITranslationStorageService translationStorageService,
            ITranslationProvider translationProvider)
        {
            return new TranslationCoordinator(
                new BCMDocumentProcessor(),
                aiModelBuilder,
                translationStorageService,
                translationProvider);
        }

        public static ITranslationCoordinator BuildCoordinator(
            IAIModelBuilder aiModelBuilder,
            ITranslationProvider translationProvider)
        {
            return new TranslationCoordinator(
                new BCMDocumentProcessor(),
                aiModelBuilder,
                translationProvider);
        }
    }
}
