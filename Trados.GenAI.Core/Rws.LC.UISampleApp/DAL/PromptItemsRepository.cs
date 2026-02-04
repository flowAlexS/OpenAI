using MongoDB.Driver;
using Rws.LC.UISampleApp.DAL.Entities;
using Rws.LC.UISampleApp.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rws.LC.UISampleApp.DAL
{
    public class PromptItemsRepository : IPromptItemsRepository
    {
        private readonly IMongoCollection<PromptItemEntity> _promptItems;

        private readonly IDatabaseContext _databaseContext;

        public PromptItemsRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _promptItems = _databaseContext.Mongo.GetCollection<PromptItemEntity>("PromptItems");
        }

        public async Task<IEnumerable<PromptItemEntity>> ListPromptItems(string tenantId)
        {
            using var cursor = await _promptItems.FindAsync(pi => pi.TenantId == tenantId).ConfigureAwait(false);
            var list = await cursor.ToListAsync().ConfigureAwait(false);
            return list.OrderByDescending(a => a.CreatedAt);
        }

        public async Task AddPromptItem(PromptItemEntity entity)
        {
            var existingPromptCursor = await _promptItems
                .FindAsync(p =>
                    p.TenantId == entity.TenantId &&
                    p.Model == entity.Model &&
                    p.Provider == entity.Provider &&
                    p.SystemInstructions == entity.SystemInstructions &&
                    p.UserPrompt == entity.UserPrompt &&
                    p.ContextUri == entity.ContextUri)
                .ConfigureAwait(false);

            // Convert cursor to a list (or use FirstOrDefaultAsync for a single match)
            var existingPrompt = await existingPromptCursor.FirstOrDefaultAsync().ConfigureAwait(false);

            if (existingPrompt != null)
            {
                await _promptItems.InsertOneAsync(entity).ConfigureAwait(false);
                return;
            }

            if (existingPrompt.Translation == entity.Translation)
                return;

            existingPrompt.Translation = entity.Translation;
        }
    }
}
