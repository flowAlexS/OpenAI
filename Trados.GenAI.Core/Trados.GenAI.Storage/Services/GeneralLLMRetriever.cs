using Trados.GenAI.Storage.Interfaces;
using Trados.GenAI.Storage.Model;
using Trados.GenAI.Storage.Extensions;

namespace Trados.GenAI.Storage.Services
{
    public class GeneralLLMRetriever : ILLMPromptsRetriever
    {
        private readonly IPromptItemsRepository _repository;

        public GeneralLLMRetriever(
            IPromptItemsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PromptItem>> RetrieveAlPromptItems(string tenantId)
        {
            var entities = await _repository.ListPromptItems(tenantId);
            return entities.ToPromptItemModels();
        }
    }
}
