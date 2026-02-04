using MongoDB.Driver;
using SharpCompress.Common;
using Trados.GenAI.Storage.Entity;
using Trados.GenAI.Storage.Interfaces;

namespace Trados.GenAI.Storage.Repository
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
            var existingPrompt = await GetPromptItem(entity).ConfigureAwait(false);

            // Insert if it does not exist
            if (existingPrompt == null)
            {
                await _promptItems.InsertOneAsync(entity).ConfigureAwait(false);
                return;
            }

            // Do nothing if translation is the same
            if (existingPrompt.Translation == entity.Translation)
                return;

            // Update translation in MongoDB
            await _promptItems.UpdateOneAsync(
                p => p.Id == existingPrompt.Id,
                Builders<PromptItemEntity>.Update
                    .Set(p => p.Translation, entity.Translation)
                    .Set(p => p.CreatedAt, DateTime.Now)
            ).ConfigureAwait(false);
        }

        public async Task<PromptItemEntity?> GetPromptItem(PromptItemEntity entity)
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

            return await existingPromptCursor.FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }

}
