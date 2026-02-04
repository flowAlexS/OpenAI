namespace Trados.GenAI.Addon.LMStudio.Models
{
    public class LMStudioModel
    {
        public string BaseUri { get; set; }

        public string Model { get; set; }

        public string ModelType { get; set; }

        public bool IsChatCompletion => ModelType.Trim() != "Completion";

        public string SystemInstructions { get; set; }

        public string UserPrompt { get; set; }

        // We will be able to work with these once we understand the bcm document
        // And have access to more details...
        public bool IgnoreTranslated { get; set; }

        public bool IncludeTarget { get; set; }

        public bool IncludeTags { get; set; }

        public bool UseCached { get; set; }

        public bool UseContextImage { get; set; }

        public bool UseInFileSystemInstructions { get; set; }

        public bool UseInFileUserPrompt { get; set; }
    }
}
