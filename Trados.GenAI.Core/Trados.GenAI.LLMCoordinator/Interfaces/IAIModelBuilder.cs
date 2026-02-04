using Trados.GenAI.BCMProcessor.Interfaces;
using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Interfaces
{
    public interface IAIModelBuilder
    {
        IAIModel BuildAIModel(IBcmData bcmData);
    }
}
