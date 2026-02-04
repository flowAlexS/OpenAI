using Trados.GenAI.Addon.OpenAI.Models;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Addon.OpenAI.Extensions
{
    public static class AIModelExtensions
    {
        public static IAIModel ToBaseAIModel(this OpenAIModel model)
        {
            return new AIModel()
            {
                ApiKey = model.ApiKey,
                SystemInstructions = model.SystemInstructions,
                UserPrompt = model.UserPrompt,
                ModelType = model.IsChatCompletion ? ModelType.ChatCompletion : ModelType.Completion,
                Model = model.Model
            };
        }
    }
}
