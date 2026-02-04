using Newtonsoft.Json;
using System.Text;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;
using Trados.GenAI.LMStudio.Extensions;
using Trados.GenAI.LMStudio.Models;
using Trados.GenAI.LMStudio.Services;

namespace Trados.GenAI.LMStudio
{
    public class LMStudioProcessor : IPipelineStep
    {
        private readonly IPipelineObject _pipelineObject;
        private readonly HttpClient _httpClient;
        private readonly SnapshotService _snapshotService;

        public LMStudioProcessor(IPipelineObject pipelineObject)
        {
            _pipelineObject = pipelineObject;
            _httpClient = new HttpClient();
            _snapshotService = new SnapshotService();
        }

        public StepStatus Status { get; private set; } = StepStatus.NotStarted;

        public void Dispose()
        {
        }

        public async Task ExecuteAsync()
        {
            Status = StepStatus.Running;
            if (_pipelineObject.TranslationContext == null)
            {
                Status = StepStatus.Failed;
                return;
            }

            if (_pipelineObject.TranslationContext.ModelType == ModelType.Completion)
            {
                var completionRequest = _pipelineObject.TranslationContext.ToCompletionRequest();
               

                var uri = _pipelineObject.TranslationContext.BaseUri + "/v1/completions";
                var response = await SendRequestAsync<CompletionRequest, CompletionResponse>(uri, completionRequest);
                _pipelineObject.TranslationResponse = new Trados.GenAI.Core.Models.TranslationResponse()
                {
                    BaseResponse = response?.Choices.FirstOrDefault()?.Text ?? string.Empty
                };

                Status = StepStatus.Passed;
            }
            else
            {
                var chatCompletionRequest = _pipelineObject.TranslationContext.ToChatCompletionRequest();
                if (!string.IsNullOrEmpty(_pipelineObject.TranslationContext.ContextUri))
                {
                    var base64 = await _snapshotService.GenerateSnapshotAsync(_pipelineObject.TranslationContext.ContextUri);
                    if (!string.IsNullOrEmpty(base64))
                    {
                        chatCompletionRequest.AddBase64Image(base64);
                    }
                }
                var uri = _pipelineObject.TranslationContext.BaseUri + "/v1/chat/completions";
                var response = await SendRequestAsync<ChatCompletionRequest, ChatCompletionResponse>(uri, chatCompletionRequest);
                _pipelineObject.TranslationResponse = new Trados.GenAI.Core.Models.TranslationResponse()
                {
                    BaseResponse = response?.Choices.FirstOrDefault()?.Message.ContentMessage ?? string.Empty
                };

                Status = StepStatus.Passed;
            }
        }

        private async Task<TResponse?> SendRequestAsync<TRequest, TResponse>(
            string requestUri,
            TRequest requestContent)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                var jsonContent = JsonConvert.SerializeObject(requestContent);
                requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                try
                {
                    var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return default;
                    }

                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }
                catch (Exception ex)
                {
                    return default;
                }
            }
        }
    }
}
