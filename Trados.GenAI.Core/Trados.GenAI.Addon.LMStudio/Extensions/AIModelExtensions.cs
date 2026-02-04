using Trados.GenAI.Addon.LMStudio.Models;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Addon.LMStudio.Extensions
{
    public static class AIModelExtensions
    {
        public static IAIModel ToBaseAIModel(this LMStudioModel model)
        {
            return new AIModel()
            {
                BaseUri = model.BaseUri,
                SystemInstructions = model.SystemInstructions,
                UserPrompt = model.UserPrompt,
                ModelType = model.IsChatCompletion ? ModelType.ChatCompletion : ModelType.Completion,
                Model = model.Model
            };
        }
    }
}
