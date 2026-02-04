using Standalone_Testing.Models;
using Trados.GenAI.Core;
using Trados.GenAI.Core.Models;
using Trados.GenAI.LMStudio.Extensions;

var aiModel = new Trados.GenAI.Core.Models.AIModel()
{
    ApiKey = "",
    Model = "",
    UserPrompt = "",
    ContextUri = @"",
    BaseUri = "",
    //ModelType = Trados.GenAI.Core.Models.ModelType.ChatCompletion
};

var pipelineObject = new PipelineObject()
{ AIModel = aiModel };
//var openAIPipeline = new PipelineFactory().WithOpenAI(pipelineObject);
//var response = await openAIPipeline.ExecuteAsync();

var lmStudioPipeline = new PipelineFactory().WithLMStudio(pipelineObject);
var response = await lmStudioPipeline.ExecuteAsync();

Console.WriteLine(response.BaseResponse);