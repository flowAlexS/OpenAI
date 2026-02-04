using Rws.LC.UISampleApp.DAL.Entities;
using System.Collections.Generic;

namespace Rws.LC.UISampleApp.Models.Extensions
{
    public static class ToResponsePromptItemExtension
    {
        public static PromptItemEntity ToPromptItemEntity(this PromptItem promptItem, string tenantId)
        {
            return new PromptItemEntity()
            {
                TenantId = tenantId,
                Provider = promptItem.Provider,
                Model = promptItem.Model,
                SystemInstructions = promptItem.SystemInstructions,
                UserPrompt = promptItem.UserPrompt,
                ContextUri = promptItem.ContextUri,
                Translation = promptItem.Translation,
                CreatedAt = promptItem.CreatedAt
            };
        }

        public static IEnumerable<PromptItem> ToPromptItems(this IEnumerable<PromptItemEntity> entities)
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
