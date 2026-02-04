using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;
using Trados.GenAI.Core.Services;
using Trados.GenAI.Core.Tests.Helpers;

namespace Trados.GenAI.Core.Tests
{
    public class TranslationContextBuilderTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldBuildTranslationContextAndUpdateStepStatusCorrectly()
        {
            // Arrange: create a fake AIModel with placeholders
            IAIModel model = new AIModelMock
            {
                SystemInstructions = "Always translate [SOURCE] to [TARGETLANG]",
                UserPrompt = "Translate this: [SOURCE]",
                Source = "Hello world",
                Target = "",
                SourceLanguage = "en",
                TargetLanguage = "de",
                ContextUri = "http://example.com/context.png",
                Model = "GPT-5",
                ModelType = ModelType.ChatCompletion,
                BaseUri = "http://localhost:5000"
            };

            // Arrange: create a pipeline object
            IPipelineObject pipelineObject = new PipelineObjectMock
            {
                AIModel = model
            };

            // Act: create the builder and execute
            var builder = new TranslationContextBuilder(pipelineObject);
            await builder.ExecuteAsync();

            var context = pipelineObject.TranslationContext;

            // Assert: placeholders replaced correctly
            Assert.NotNull(context);
            Assert.Equal("Always translate Hello world to de", context.SystemInstructions);
            Assert.Equal("Translate this: Hello world", context.UserPrompt);
            Assert.Equal(model.ContextUri, context.ContextUri);
            Assert.Equal(model.Model, context.Model);
            Assert.Equal(model.ModelType, context.ModelType);
            Assert.Equal(model.BaseUri, context.BaseUri);

            // Assert: step status updated correctly
            Assert.Equal(StepStatus.Passed, builder.Status);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldSkipWhenAIModelIsNull()
        {
            // Arrange: pipeline object with no AIModel
            IPipelineObject pipelineObject = new PipelineObjectMock
            {
                AIModel = null
            };

            // Act
            var builder = new TranslationContextBuilder(pipelineObject);
            await builder.ExecuteAsync();

            // Assert
            Assert.Null(pipelineObject.TranslationContext);
            Assert.Equal(StepStatus.Skipped, builder.Status);
        }
    }

}