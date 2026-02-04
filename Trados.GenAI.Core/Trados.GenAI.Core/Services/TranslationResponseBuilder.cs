using System.Text.Json;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Services
{
    public class TranslationResponseBuilder : IPipelineStep
    {
        private readonly IPipelineObject _pipelineObject;
        public StepStatus Status { get; private set; } = StepStatus.NotStarted;

        public TranslationResponseBuilder(IPipelineObject pipelineObject)
        {
            _pipelineObject = pipelineObject ?? throw new ArgumentNullException(nameof(pipelineObject));
        }

        public async Task ExecuteAsync()
        {
            if (Status == StepStatus.Failed || Status == StepStatus.Skipped)
                throw new InvalidOperationException("Cannot execute a step that has already failed or was skipped.");

            Status = StepStatus.Running;

            try
            {
                var response = _pipelineObject.TranslationResponse;

                if (response == null || string.IsNullOrEmpty(response.BaseResponse))
                {
                    Status = StepStatus.Failed;
                    throw new InvalidOperationException("No AI response found to process.");
                }

                // Deserialize response
                try
                {
                    // Try multiple units first
                    var multiUnits = JsonSerializer.Deserialize<TranslationUnitContainer>(
                        response.BaseResponse,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (multiUnits?.TransUnits != null && multiUnits.TransUnits.Count > 0)
                    {
                        response.TranslationUnits = multiUnits.TransUnits;
                    }
                    else
                    {
                        // Try single unit
                        var singleUnit = JsonSerializer.Deserialize<TranslationUnit>(
                            response.BaseResponse,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (singleUnit != null)
                        {
                            response.TranslationUnits = new List<TranslationUnit> { singleUnit };
                        }
                        else
                        {
                            // Fallback to raw string
                            response.TranslationUnits = new List<TranslationUnit>
                            {
                                new TranslationUnit { Translation = response.BaseResponse, Comment = null }
                            };
                        }
                    }
                }
                catch
                {
                    // Fallback to raw string if any deserialization error occurs
                    response.TranslationUnits = new List<TranslationUnit>
                    {
                        new TranslationUnit { Translation = response.BaseResponse, Comment = null }
                    };
                }

                Status = StepStatus.Passed;
            }
            catch
            {
                Status = StepStatus.Failed;
                throw;
            }

            await Task.CompletedTask;
        }

        public void Dispose() { }
    }

}
