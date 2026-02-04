using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Services
{
    public class TranslationContextBuilder : IPipelineStep
    {
        private readonly IPipelineObject _pipelineObject;

        public StepStatus Status { get; private set; } = StepStatus.NotStarted;

        public TranslationContextBuilder(IPipelineObject pipelineObject)
        {
            _pipelineObject = pipelineObject;
        }

        public async Task ExecuteAsync()
        {
            if (_pipelineObject.AIModel == null)
            {
                Status = StepStatus.Skipped;
                return;
            }

            Status = StepStatus.Running;

            try
            {
                var model = _pipelineObject.AIModel;

                ITranslationContext context = _pipelineObject.TranslationContext ?? new TranslationContext();

                context.ApiKey = model.ApiKey;
                context.SystemInstructions = model.SystemInstructions;
                context.UserPrompt = model.UserPrompt;
                context.ContextUri = model.ContextUri;
                context.Model = model.Model;
                context.ModelType = model.ModelType;
                context.BaseUri = model.BaseUri;

                _pipelineObject.TranslationContext = context;
                Status = StepStatus.Passed;
            }
            catch
            {
                Status = StepStatus.Failed;
                throw;
            }

            await Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }

}
