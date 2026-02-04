using System.Threading.Tasks;
using Trados.GenAI.Core;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;
using Trados.GenAI.LLMCoordinator.Interfaces;
using Trados.GenAI.LMStudio.Extensions;

namespace Trados.GenAI.Addon.OpenAI.Services
{
    public class TranslationProvider : ITranslationProvider
    {
        public async Task<ITranslationResponse> TranslateAsync(IAIModel aiModel)
        {
            var translationPipeline = new PipelineFactory().WithLMStudio(new PipelineObject() { AIModel = aiModel });
            return await translationPipeline.ExecuteAsync();
        }
    }
}
