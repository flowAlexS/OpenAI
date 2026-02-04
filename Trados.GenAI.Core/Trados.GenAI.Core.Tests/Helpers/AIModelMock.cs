using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Tests.Helpers
{
    public class AIModelMock : IAIModel
    {
        public string ApiKey { get; set; } = string.Empty;

        public string SystemInstructions { get; set; } = string.Empty;

        public string UserPrompt { get; set; } = string.Empty;


        public string ContextUri { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Target { get; set; } = string.Empty;

        public string SourceLanguage { get; set; } = string.Empty;

        public string TargetLanguage { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public ModelType ModelType { get; set; }

        public string BaseUri { get; set; } = string.Empty;
    }
}
