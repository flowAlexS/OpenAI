using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;
using Trados.GenAI.Core.Services;
using Xunit;

namespace Trados.GenAI.Core.Tests
{
    public class PipelineTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldRunStepsInOrder_AndReturnTranslationResponse()
        {
            // Arrange
            var executionOrder = new List<int>();
            var pipelineObject = new PipelineObject
            {
                TranslationResponse = new TranslationResponse()
            };

            var step1 = new MockStep(() => executionOrder.Add(1));
            var step2 = new MockStep(() => executionOrder.Add(2));
            var step3 = new MockStep(() => executionOrder.Add(3));

            var pipeline = new Pipeline(pipelineObject);
            pipeline.AddLast(step1);
            pipeline.AddLast(step2);
            pipeline.AddLast(step3);

            // Act
            var response = await pipeline.ExecuteAsync();

            // Assert
            Assert.Equal(new List<int> { 1, 2, 3 }, executionOrder);
            Assert.Equal(StepStatus.Passed, step1.Status);
            Assert.Equal(StepStatus.Passed, step2.Status);
            Assert.Equal(StepStatus.Passed, step3.Status);
            Assert.NotNull(response);
            Assert.Same(pipelineObject.TranslationResponse, response);
        }

        [Fact]
        public void AddInsertRemove_ShouldManipulateStepsCorrectly()
        {
            // Arrange
            var step1 = new MockStep();
            var step2 = new MockStep();
            var step3 = new MockStep();
            var step4 = new MockStep();

            var pipeline = new Pipeline(new PipelineObject());
            pipeline.AddLast(step1);
            pipeline.AddLast(step2);

            // Act
            pipeline.AddFirst(step3);
            pipeline.InsertAfter(step1, step4);
            pipeline.Remove(step2);

            // Assert
            var steps = pipeline.Steps;
            Assert.Contains(step3, steps); // Added first
            Assert.Contains(step1, steps);
            Assert.Contains(step4, steps); // Inserted after step1
            Assert.DoesNotContain(step2, steps); // Removed
        }

        [Fact]
        public async Task ExecuteAsync_ShouldStopOnFailedStep_AndReturnFailedStatus()
        {
            // Arrange
            var executed = new List<string>();
            var pipelineObject = new PipelineObject
            {
                TranslationResponse = new TranslationResponse()
            };

            var step1 = new MockStep(() => executed.Add("step1"));
            var stepFail = new MockStep(throwOnExecute: true);
            var step3 = new MockStep(() => executed.Add("step3"));

            var pipeline = new Pipeline(pipelineObject);
            pipeline.AddLast(step1);
            pipeline.AddLast(stepFail);
            pipeline.AddLast(step3);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.ExecuteAsync());

            Assert.Contains("step1", executed);
            Assert.DoesNotContain("step3", executed); // Should not execute after failure
            Assert.Equal(StepStatus.Passed, step1.Status);
            Assert.Equal(StepStatus.Failed, stepFail.Status);
            Assert.Equal(StepStatus.NotStarted, step3.Status);
        }

        // MockStep implementing IPipelineStep with optional action or failure
        private class MockStep : IPipelineStep
        {
            private readonly Action _action;
            private readonly bool _throwOnExecute;

            public StepStatus Status { get; private set; } = StepStatus.NotStarted;

            public MockStep(Action action = null, bool throwOnExecute = false)
            {
                _action = action;
                _throwOnExecute = throwOnExecute;
            }

            public Task ExecuteAsync()
            {
                Status = StepStatus.Running;

                if (_throwOnExecute)
                {
                    Status = StepStatus.Failed;
                    throw new InvalidOperationException("Step failed");
                }

                _action?.Invoke();
                Status = StepStatus.Passed;

                return Task.CompletedTask;
            }

            public void Dispose() { }
        }
    }
}
