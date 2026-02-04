using Trados.GenAI.Storage.Interfaces;
using Trados.GenAI.Storage.Repository;
using Trados.GenAI.Storage.Services;

namespace Trados.GenAI.Storage
{
    public class LLMPromptFactory
    {
        public static ILLMPromptsRetriever CreatePromptRetriever()
        {
            var databaseContext = new DatabaseContext();
            var promptItemsRepository = new PromptItemsRepository(databaseContext);
            return new GeneralLLMRetriever(
                promptItemsRepository);
        }

    }
}
