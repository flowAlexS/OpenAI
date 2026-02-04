using MongoDB.Driver;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using System.Threading.Tasks;
using Trados.GenAI.Addon.OpenAI.DAL.Entities;

namespace Trados.GenAI.Addon.OpenAI.DAL
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly IMongoCollection<TranslationEntity> _translations;

        private readonly IDatabaseContext _databaseContext;

        public TranslationRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _translations = _databaseContext.Mongo.GetCollection<TranslationEntity>("TranslationRegistration");
        }

        public async Task SaveTranslation(TranslationEntity translationEntity)
        {
            await _translations.InsertOneAsync(translationEntity).ConfigureAwait(false);
        }

        public async Task<string> GetTranslationAsync(TranslationEntity translationEntity)
        {
            var entity = await _translations.FindAsync(te => 
                te.TenantId == translationEntity.TenantId &&
                te.SystemInstructions == translationEntity.SystemInstructions &&
                te.UserPrompt == translationEntity.UserPrompt &&
                te.ContextImage == translationEntity.ContextImage &&
                te.SourceText == translationEntity.SourceText);

            var response = await entity.SingleOrDefaultAsync();
            return response?.TargetText;
        }
    }
}
