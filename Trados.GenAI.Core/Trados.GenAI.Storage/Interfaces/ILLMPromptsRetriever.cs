using Trados.GenAI.Storage.Model;

namespace Trados.GenAI.Storage.Interfaces
{
    public interface ILLMPromptsRetriever
    {
        Task<IEnumerable<PromptItem>> RetrieveAlPromptItems(string tenantId);
    }
}
