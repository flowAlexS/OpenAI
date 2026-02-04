using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface ITranslationProvider
    {
        Task<ITranslationResponse> TranslateAsync(IAIModel model);
    }
}
