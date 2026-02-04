namespace Trados.GenAI.Core.Models
{
    public enum StepStatus
    {
        NotStarted, // Step has not been executed yet
        Running,    // Step is currently executing
        Passed,     // Step executed successfully
        Failed,     // Step execution failed
        Skipped     // Step was intentionally skipped
    }
}
