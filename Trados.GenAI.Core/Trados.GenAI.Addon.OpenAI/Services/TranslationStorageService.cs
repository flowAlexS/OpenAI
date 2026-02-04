using System.Threading.Tasks;
using Trados.GenAI.Addon.OpenAI.DAL.Entities;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.LLMCoordinator.Interfaces;

namespace Trados.GenAI.Addon.OpenAI.Services
{
    public class TranslationStorageService : ITranslationStorageService
    {
        private readonly ITranslationRepository _translationRepository;
        private readonly string _tenantId;

        public TranslationStorageService(
            ITranslationRepository translationRepository,
            string tenantId)
        {
            _translationRepository = translationRepository;
            _tenantId = tenantId;
        }

        public async Task<string> RetrieveTranslationAsync(IAIModel aiModel)
        {
            var translationEntity =
                new TranslationEntity
                {
                    TenantId = _tenantId,
                    Model = aiModel.Model,
                    SystemInstructions = aiModel.SystemInstructions,
                    UserPrompt = aiModel.UserPrompt,
                    ContextImage = aiModel.ContextUri,
                    SourceText = aiModel.Source
                }; ;

            var existingTranslation = await _translationRepository.GetTranslationAsync(translationEntity);
            if (!string.IsNullOrEmpty(existingTranslation))
            {
                return existingTranslation;
            }
            return await _translationRepository.GetTranslationAsync(translationEntity);
        }

        public async Task SaveTranslationAsync(IAIModel aiModel, string translation)
        {
            var translationEntity =
                new TranslationEntity
                {
                    TenantId = _tenantId,
                    Model = aiModel.Model,
                    SystemInstructions = aiModel.SystemInstructions,
                    UserPrompt = aiModel.UserPrompt,
                    ContextImage = aiModel.ContextUri,
                    SourceText = aiModel.Source,
                    TargetText = translation
                };

            await _translationRepository.SaveTranslation(translationEntity);
        }
    }
}
