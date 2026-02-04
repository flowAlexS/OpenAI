using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Services
{
    public class Pipeline : IPipeline
    {
        private readonly LinkedList<IPipelineStep> _pipelineSteps = new LinkedList<IPipelineStep>();
        private readonly IPipelineObject _pipelineObject;

        public Pipeline(IPipelineObject pipelineObject)
        {
            _pipelineObject = pipelineObject;
        }

        public IReadOnlyCollection<IPipelineStep> Steps => _pipelineSteps.ToList().AsReadOnly();

        public StepStatus Status
        {
            get
            {
                if (!_pipelineSteps.Any()) return StepStatus.NotStarted;

                if (_pipelineSteps.Any(s => s.Status == StepStatus.Failed))
                    return StepStatus.Failed;

                if (_pipelineSteps.All(s => s.Status == StepStatus.Passed))
                    return StepStatus.Passed;

                if (_pipelineSteps.Any(s => s.Status == StepStatus.Running))
                    return StepStatus.Running;

                return StepStatus.NotStarted;
            }
        }

        public void AddFirst(IPipelineStep step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            _pipelineSteps.AddFirst(step);
        }

        public void AddLast(IPipelineStep step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            _pipelineSteps.AddLast(step);
        }

        public void Dispose()
        {
            foreach (var step in _pipelineSteps)
            {
                step.Dispose();
            }
        }

        public void InsertAfter(IPipelineStep existing, IPipelineStep toInsert)
        {
            if (existing == null) throw new ArgumentNullException(nameof(existing));
            if (toInsert == null) throw new ArgumentNullException(nameof(toInsert));

            var node = _pipelineSteps.Find(existing) ?? throw new InvalidOperationException("Existing step not found");
            _pipelineSteps.AddAfter(node, toInsert);
        }

        public void InsertBefore(IPipelineStep existing, IPipelineStep toInsert)
        {
            if (existing == null) throw new ArgumentNullException(nameof(existing));
            if (toInsert == null) throw new ArgumentNullException(nameof(toInsert));

            var node = _pipelineSteps.Find(existing) ?? throw new InvalidOperationException("Existing step not found");
            _pipelineSteps.AddBefore(node, toInsert);
        }

        public void Remove(IPipelineStep step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (!_pipelineSteps.Remove(step))
                throw new InvalidOperationException("Step not part of the pipeline");
        }

        public async Task<ITranslationResponse> ExecuteAsync()
        {
            foreach (var step in _pipelineSteps)
            {
                await step.ExecuteAsync();
            }
            
            if (_pipelineObject.TranslationResponse == null)
            {
                throw new InvalidOperationException();
            }

            return _pipelineObject.TranslationResponse;
        }
    }
}
