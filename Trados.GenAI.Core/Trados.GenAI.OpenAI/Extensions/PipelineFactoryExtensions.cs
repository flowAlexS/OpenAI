using Trados.GenAI.Core;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.OpenAI.Extensions
{
    public static class PipelineFactoryExtensions
    {
        public static IPipeline WithOpenAI(this PipelineFactory _, IPipelineObject pipelineObject)
        {
            if (pipelineObject.TranslationContext == null)
            {
                pipelineObject.TranslationContext = new TranslationContext();
            }
            var openAIProcessor = new OpenAIProcessor(pipelineObject);
            return PipelineFactory.CreateDefaultPipeline(pipelineObject, openAIProcessor);
        }
    }
}
