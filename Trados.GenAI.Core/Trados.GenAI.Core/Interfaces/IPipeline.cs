namespace Trados.GenAI.Core.Interfaces
{
    public interface IPipeline : IDisposable
    {
        IReadOnlyCollection<IPipelineStep> Steps { get; } 

        void AddFirst(IPipelineStep step);
        void AddLast(IPipelineStep step);
        void InsertAfter(IPipelineStep existing, IPipelineStep toInsert);
        void InsertBefore(IPipelineStep existing, IPipelineStep toInsert);
        void Remove(IPipelineStep step);

        Task<ITranslationResponse> ExecuteAsync();
    }
}
