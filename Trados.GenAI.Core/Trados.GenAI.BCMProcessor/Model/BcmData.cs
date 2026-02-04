using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.BCMProcessor.Interfaces;

namespace Trados.GenAI.BCMProcessor.Model
{
    public class BcmData : IBcmData
    {
        public string TranslatableSource { get; set; } = string.Empty;

        public Paragraph? SourceParagraph { get; set; }

        public ParagraphUnit? ParagraphUnit { get; set; }

        public string TranslatableTarget { get; set; } = string.Empty;

        public string SourceLanguage { get; set; } = string.Empty;

        public string TargetLanguage { get; set; } = string.Empty;

        public string ContextUri { get; set; } = string.Empty;

        public string SystemPrompt { get; set; } = string.Empty;

        public string UserPrompt { get; set; } = string.Empty;
    }
}
