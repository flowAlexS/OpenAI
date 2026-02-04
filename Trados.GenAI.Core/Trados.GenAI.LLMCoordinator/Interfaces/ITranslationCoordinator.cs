namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface ITranslationCoordinator
    {
        Task<ITranslationResult> ExecuteAsync(ITranslationRequest translationRequest);
    }
}
