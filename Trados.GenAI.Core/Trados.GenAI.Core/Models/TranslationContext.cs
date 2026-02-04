using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.Core.Models
{
    public class TranslationContext : ITranslationContext
    {
        public string ApiKey { get; set; } = string.Empty;

        public string SystemInstructions { get; set; } = string.Empty;

        public string UserPrompt { get; set; } = string.Empty;

        public string ContextUri { get; set; } = string.Empty; 

        public string Model { get; set; } = string.Empty;

        public ModelType ModelType { get; set; }

        public string BaseUri { get; set; } = string.Empty;
    }

}
