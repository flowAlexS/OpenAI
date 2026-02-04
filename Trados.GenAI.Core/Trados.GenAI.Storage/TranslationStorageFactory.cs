using Trados.GenAI.LLMCoordinator.Interfaces;
using Trados.GenAI.Storage.Repository;
using Trados.GenAI.Storage.Services;

namespace Trados.GenAI.Storage
{
    public class TranslationStorageFactory
    {
        public static ITranslationStorageService CreateTranslationStorageService(
            string provider,
            string tenantId)
        {
            var databaseContext = new DatabaseContext();
            var promptItemsRepository = new PromptItemsRepository(databaseContext);
            return new TranslationStorageService(
                promptItemsRepository,
                provider,
                tenantId);
        }
    }
}
