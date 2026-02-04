using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;
using Trados.GenAI.OpenAI.Services;

namespace Trados.GenAI.OpenAI
{
    public class OpenAIProcessor : IPipelineStep
    {
        private readonly IPipelineObject _pipelineObject;
        private readonly ITranslationContext _translationContext;
        private readonly SnapshotService _snapshotService;

        public OpenAIProcessor(IPipelineObject pipelineObject)
        {
            _pipelineObject = pipelineObject;
            _translationContext = pipelineObject.TranslationContext ?? throw new ArgumentNullException();
            _snapshotService = new();
        }

        public StepStatus Status { get; private set; } = StepStatus.NotStarted;

        public void Dispose()
        {
        }

        public async Task ExecuteAsync()
        {
            Status = StepStatus.Running;
            var client = new OpenAIResponseClient(_translationContext.Model, _translationContext.ApiKey);
            var inputItems = new List<ResponseItem>();

            // Add system instructions if present
            if (!string.IsNullOrEmpty(_translationContext.SystemInstructions)
                && _translationContext.ModelType == ModelType.ChatCompletion)
            {
                var systemResponseItem =
                ResponseItem.CreateSystemMessageItem(_translationContext.SystemInstructions);

                inputItems.Add(systemResponseItem);
            }

            // Handle snapshot of image / web page
            if (!string.IsNullOrEmpty(_translationContext.ContextUri))
            {
                var (ImageBytes, MediaType, Caption) = await _snapshotService.GenerateSnapshotAsync(_translationContext.ContextUri);
                BinaryData imageBytes = ImageBytes;
                string imageMediaType = MediaType;
                string caption = Caption;

                var imageItem = ResponseItem.CreateUserMessageItem(
                    [
                    ResponseContentPart.CreateInputImagePart(imageBytes, imageMediaType)
                    ]);

                inputItems.Add(imageItem);
            }

            // Add user prompt if present
            if (!string.IsNullOrEmpty(_translationContext.UserPrompt))
            {
                inputItems.Add(
                    ResponseItem.CreateUserMessageItem(_translationContext.UserPrompt));
            }

            try
            {
                // Send to OpenAI client
                OpenAIResponse response = await client.CreateResponseAsync(inputItems);
                Status = StepStatus.Passed;

                _pipelineObject.TranslationResponse = new TranslationResponse()
                {
                    BaseResponse = response.GetOutputText()
                };
            }
            catch (Exception ex)
            {
                Status = StepStatus.Failed;
            }
        }
    }

}
