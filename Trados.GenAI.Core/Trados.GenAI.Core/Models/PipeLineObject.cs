using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.Core.Models
{
    public class PipelineObject : IPipelineObject
    {
        public IAIModel? AIModel { get; set; }

        public ITranslationContext? TranslationContext { get; set; }

        public ITranslationResponse? TranslationResponse { get; set; }
    }
}
