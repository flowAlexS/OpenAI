using System;

namespace Rws.LC.UISampleApp.Models
{
    public class PromptItem
    {
        public string Provider { get; set; }

        public string Model { get; set; }

        public string SystemInstructions { get; set; }

        public string UserPrompt { get; set; }

        public string ContextUri { get; set; }

        public string Translation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
