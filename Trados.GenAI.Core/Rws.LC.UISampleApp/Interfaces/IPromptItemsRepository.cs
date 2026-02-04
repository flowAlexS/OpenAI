using Rws.LC.UISampleApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rws.LC.UISampleApp.Interfaces
{
    public interface IPromptItemsRepository
    {
        Task<IEnumerable<PromptItemEntity>> ListPromptItems(string tenantId);

        Task AddPromptItem(PromptItemEntity entity);
    }
}
