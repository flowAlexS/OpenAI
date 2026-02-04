using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.LLMCoordinator.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Model
{
    public class TranslationResult : ITranslationResult
    {
        public bool Success { get; set; }

        public Document TranslatedDocument { get; set; } = new Document();

        public string Error { get; set; } = string.Empty;
    }
}
