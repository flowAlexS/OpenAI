using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.BCMProcessor.Interfaces
{
    public interface IBcmData
    {
        string TranslatableSource { get; }

        string TranslatableTarget { get; }

        ParagraphUnit? ParagraphUnit { get; }

        Paragraph? SourceParagraph { get; }

        string SourceLanguage { get; }  

        string TargetLanguage { get; }

        string ContextUri { get; }

        string SystemPrompt { get; }
        
        string UserPrompt { get; }
    }
}
