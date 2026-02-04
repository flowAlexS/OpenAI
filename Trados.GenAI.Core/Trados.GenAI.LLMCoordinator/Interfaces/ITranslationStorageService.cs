using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface ITranslationStorageService
    {
        Task<string?> RetrieveTranslationAsync(IAIModel aiModel);

        Task SaveTranslationAsync(IAIModel aiModel, string translation);
    }
}
