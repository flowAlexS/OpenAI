using Trados.GenAI.Storage.Entity;
using Trados.GenAI.Storage.Model;

namespace Trados.GenAI.Storage.Extensions
{
    public static class PromptModelExtensions
    {
        public static IEnumerable<PromptItem> ToPromptItemModels(this IEnumerable<PromptItemEntity> entities)
        {
            foreach (var entity in entities)
            {
                yield return new PromptItem
                {
                    Provider = entity.Provider,
                    Model = entity.Model,
                    SystemInstructions = entity.SystemInstructions,
                    UserPrompt = entity.UserPrompt,
                    ContextUri = entity.ContextUri,
                    Translation = entity.Translation,
                    CreatedAt = entity.CreatedAt
                };
            }
        }
    }
}
