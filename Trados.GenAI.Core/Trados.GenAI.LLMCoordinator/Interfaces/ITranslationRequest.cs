using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface ITranslationRequest
    {
        string Owner { get; }

        bool UseCache { get; }

        bool IncludeTags { get; }

        Document? BcmDocument { get; }
    }
}
