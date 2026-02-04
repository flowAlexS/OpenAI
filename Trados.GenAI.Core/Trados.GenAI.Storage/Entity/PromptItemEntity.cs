namespace Trados.GenAI.Storage.Entity
{
    public class PromptItemEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string TenantId { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string SystemInstructions { get; set; } = string.Empty;

        public string UserPrompt { get; set; } = string.Empty;

        public string? ContextUri { get; set; }

        public string Translation { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
