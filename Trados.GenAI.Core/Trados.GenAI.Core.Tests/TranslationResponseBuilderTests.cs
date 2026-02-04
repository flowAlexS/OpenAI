using System.Text.Json;
using Trados.GenAI.Core.Models;
using Trados.GenAI.Core.Services;
using Trados.GenAI.Core.Tests.Helpers;

namespace Trados.GenAI.Core.Tests
{
    public class TranslationResponseBuilderTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldFail_WhenBaseResponseIsNull()
        {
            // Arrange
            var pipelineObject = new PipelineObjectMock
            {
                TranslationResponse = new TranslationResponse { BaseResponse = null }
            };
            var builder = new TranslationResponseBuilder(pipelineObject);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => builder.ExecuteAsync());
            Assert.Equal(StepStatus.Failed, builder.Status);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldDeserializeMultipleUnits()
        {
            // Arrange
            var multiJson = JsonSerializer.Serialize(new TranslationUnitContainer
            {
                TransUnits = new List<TranslationUnit>
                {
                    new TranslationUnit { Translation = "Hello", Comment = "c1" },
                    new TranslationUnit { Translation = "World", Comment = "c2" }
                }
            });

            var pipelineObject = new PipelineObjectMock
            {
                TranslationResponse = new TranslationResponse { BaseResponse = multiJson }
            };
            var builder = new TranslationResponseBuilder(pipelineObject);

            // Act
            await builder.ExecuteAsync();

            // Assert
            var units = pipelineObject.TranslationResponse.TranslationUnits;
            Assert.Equal(2, units.Count);
            Assert.Equal("Hello", units[0].Translation);
            Assert.Equal("c1", units[0].Comment);
            Assert.Equal(StepStatus.Passed, builder.Status);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldDeserializeSingleUnit()
        {
            // Arrange
            var singleJson = JsonSerializer.Serialize(new TranslationUnit
            {
                Translation = "Hello Single",
                Comment = "single comment"
            });

            var pipelineObject = new PipelineObjectMock
            {
                TranslationResponse = new TranslationResponse { BaseResponse = singleJson }
            };
            var builder = new TranslationResponseBuilder(pipelineObject);

            // Act
            await builder.ExecuteAsync();

            // Assert
            var units = pipelineObject.TranslationResponse.TranslationUnits;
            Assert.Single(units);
            Assert.Equal("Hello Single", units[0].Translation);
            Assert.Equal("single comment", units[0].Comment);
            Assert.Equal(StepStatus.Passed, builder.Status);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldFallbackToRawString()
        {
            // Arrange
            var rawString = "Raw AI response text";
            var pipelineObject = new PipelineObjectMock
            {
                TranslationResponse = new TranslationResponse { BaseResponse = rawString }
            };
            var builder = new TranslationResponseBuilder(pipelineObject);

            // Act
            await builder.ExecuteAsync();

            // Assert
            var units = pipelineObject.TranslationResponse.TranslationUnits;
            Assert.Single(units);
            Assert.Equal(rawString, units[0].Translation);
            Assert.Null(units[0].Comment);
            Assert.Equal(StepStatus.Passed, builder.Status);
        }
    }

}
