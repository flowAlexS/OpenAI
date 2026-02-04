using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Services;

namespace Trados.GenAI.Core
{
    public class PipelineFactory
    {
        /// <summary>
        /// Creates an empty pipeline with a given pipeline object.
        /// </summary>
        public static IPipeline CreateEmptyPipeline(IPipelineObject pipelineObject)
        {
            if (pipelineObject == null) throw new ArgumentNullException(nameof(pipelineObject));

            return new Pipeline(pipelineObject);
        }

        /// <summary>
        /// Creates a default pipeline blueprint with:
        /// 1. TranslationContextBuilder
        /// 2. AIProcessor (LLM processing step)
        /// 3. TranslationResponseBuilder
        /// </summary>
        public static IPipeline CreateDefaultPipeline(IPipelineObject pipelineObject, IPipelineStep aiProcessorStep)
        {
            if (pipelineObject == null) throw new ArgumentNullException(nameof(pipelineObject));
            if (aiProcessorStep == null) throw new ArgumentNullException(nameof(aiProcessorStep));

            var pipeline = new Pipeline(pipelineObject);

            // Step 1: Build context
            var contextBuilder = new TranslationContextBuilder(pipelineObject);
            pipeline.AddLast(contextBuilder);

            // Step 2: AI processing step
            pipeline.AddLast(aiProcessorStep);

            // Step 3: Build response
            var responseBuilder = new TranslationResponseBuilder(pipelineObject);
            pipeline.AddLast(responseBuilder);

            return pipeline;
        }
    }
}
