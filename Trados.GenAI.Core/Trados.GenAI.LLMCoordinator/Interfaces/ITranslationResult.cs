using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface ITranslationResult
    {
        bool Success { get; }

        Document TranslatedDocument { get; }

        string Error { get; }   
    }
}
