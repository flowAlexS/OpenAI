using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.LLMCoordinator.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Model
{
    public class TranslationRequest : ITranslationRequest
    {
        public string Owner { get; set; } = string.Empty;

        public bool UseCache { get; set; }

        public bool IncludeTags { get; set; }

        public Document? BcmDocument { get; set; }
    }
}
