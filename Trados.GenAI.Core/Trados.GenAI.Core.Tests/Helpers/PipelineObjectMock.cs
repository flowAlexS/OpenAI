using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.Core.Tests.Helpers
{
    internal class PipelineObjectMock : IPipelineObject
    {
        public IAIModel AIModel { get; set; }
        public ITranslationContext TranslationContext { get; set; }
        public ITranslationResponse TranslationResponse { get; set; }
    }
}
