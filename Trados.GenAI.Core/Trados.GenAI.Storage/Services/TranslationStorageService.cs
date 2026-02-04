using System.Security.Cryptography.X509Certificates;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.LLMCoordinator.Interfaces;
using Trados.GenAI.Storage.Extensions;
using Trados.GenAI.Storage.Interfaces;

namespace Trados.GenAI.Storage.Services
{
    public class TranslationStorageService : ITranslationStorageService
    {
        private readonly IPromptItemsRepository _repository;
        private readonly string _provider;
        private readonly string _tenantId;

        public TranslationStorageService(
            IPromptItemsRepository repository,
            string provider,
            string tenantId)
        {
            _repository = repository;
            _provider = provider;
            _tenantId = tenantId;
        }


        public async Task<string?> RetrieveTranslationAsync(IAIModel aiModel)
        {
            var entityToSearch = aiModel.ToPromptItemEntity(_provider, _tenantId);
            var entityFound = await _repository.GetPromptItem(entityToSearch);
            return entityFound?.Translation;
        }

        public async Task SaveTranslationAsync(IAIModel aiModel, string translation)
        {
            var entityToAdd = aiModel.ToPromptItemEntity(_provider, _tenantId);
            entityToAdd.Translation = translation;
            await _repository.AddPromptItem(entityToAdd);
        }
    }
}
