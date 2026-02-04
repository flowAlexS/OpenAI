using Trados.GenAI.Core;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.LMStudio.Extensions
{
    public static class PipelineFactoryExtensions
    {
        public static IPipeline WithLMStudio(this PipelineFactory _, IPipelineObject pipelineObject)
        {
            if (pipelineObject.TranslationContext == null)
            {
                pipelineObject.TranslationContext = new TranslationContext();
            }
            var lmStudioProcessor = new LMStudioProcessor(pipelineObject);
            return PipelineFactory.CreateDefaultPipeline(pipelineObject, lmStudioProcessor);
        }
    }
}
