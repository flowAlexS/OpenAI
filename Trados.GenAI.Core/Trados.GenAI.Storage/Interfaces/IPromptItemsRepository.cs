using Trados.GenAI.Storage.Entity;

namespace Trados.GenAI.Storage.Interfaces
{
    public interface IPromptItemsRepository
    {
        Task<IEnumerable<PromptItemEntity>> ListPromptItems(string tenantId);

        Task<PromptItemEntity?> GetPromptItem(PromptItemEntity entity);

        Task AddPromptItem(PromptItemEntity entity);
    }
}
