using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Storage.Entity;

namespace Trados.GenAI.Storage.Extensions
{
    public static class AIModelExtensions
    {
        public static PromptItemEntity ToPromptItemEntity(this IAIModel model, string provider, string tenant)
        {
            return new PromptItemEntity
            {
                TenantId = tenant,
                Provider = provider,
                Model = model.Model,
                SystemInstructions = model.SystemInstructions,
                UserPrompt = model.UserPrompt,
                ContextUri = model.ContextUri,
            };
        }
    }
}
