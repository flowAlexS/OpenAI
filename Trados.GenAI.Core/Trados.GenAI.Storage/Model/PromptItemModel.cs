using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trados.GenAI.Storage.Model
{
    public class PromptItem
    {
        public string Provider { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string SystemInstructions { get; set; } = string.Empty;

        public string UserPrompt { get; set; } = string.Empty;

        public string? ContextUri { get; set; }

        public string Translation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
