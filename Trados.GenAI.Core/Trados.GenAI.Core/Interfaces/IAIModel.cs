using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Interfaces
{
    public interface IAIModel
    {
        string ApiKey { get; set; }

        string SystemInstructions { get; set; }

        string UserPrompt { get; set; }

        string ContextUri { get; set; }

        string Source { get; set; }

        string Target { get; set; }

        string SourceLanguage { get; set; }

        string TargetLanguage { get; set; }

        string Model { get; set; }

        ModelType ModelType { get; set; }

        string BaseUri { get; set; }
    }

}
