using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.Addon.OpenAI.Models;
using System.Collections.Generic;
using Trados.GenAI.Addon.OpenAI.Extensions;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.BCMProcessor.Interfaces;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.LLMCoordinator.Interfaces;

namespace Trados.GenAI.Addon.OpenAI.Services
{
    public class AIModelBuilder : IAIModelBuilder
    {
        private readonly OpenAIModel _openAIModel;

        public AIModelBuilder(OpenAIModel openAIModel)
        {
            _openAIModel = openAIModel;
        }

        public IAIModel BuildAIModel(IBcmData bcmData)
        {
            var baseModel = _openAIModel.ToBaseAIModel();
            if (_openAIModel.UseContextImage && !string.IsNullOrEmpty(bcmData.ContextUri))
                baseModel.ContextUri = bcmData.ContextUri;

            if (_openAIModel.UseInFileUserPrompt && !string.IsNullOrEmpty(bcmData.UserPrompt))
                baseModel.UserPrompt = bcmData.UserPrompt;

            if (_openAIModel.UseInFileSystemInstructions && !string.IsNullOrEmpty(bcmData.SystemPrompt))
                baseModel.SystemInstructions = bcmData.SystemPrompt;

            baseModel.SourceLanguage = bcmData.SourceLanguage;
            baseModel.TargetLanguage = bcmData.TargetLanguage;
            baseModel.Source = bcmData.TranslatableSource;
            baseModel.Target = bcmData.TranslatableTarget;
            return baseModel;
        }
    }
}
