using System;

namespace Rws.LC.UISampleApp.DAL.Entities
{
    public class PromptItemEntity
    {
        public string Id { get; set; }

        public string TenantId { get; set; }

        public string Provider { get; set; }

        public string Model { get; set; }

        public string SystemInstructions { get; set; }

        public string UserPrompt { get; set; }

        public string ContextUri { get; set; }

        public string Translation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
