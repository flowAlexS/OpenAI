using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.LLMCoordinator.Utils
{
    public static class VariableHelper
    {
        public static string ReplaceVariables(string original, IAIModel model)
        {
            if (string.IsNullOrEmpty(original)) return original;

            var containsSource = original.Contains("{source}");

            var updated = original.Replace("{source}", model.Source)
                .Replace("{target}", model.Target)
                .Replace("{sourceLanguage}", model.SourceLanguage)
                .Replace("{targetLanguage}", model.TargetLanguage)
                .Replace("{{SRC}}", model.SourceLanguage)
                .Replace("{{TARGET}}", model.TargetLanguage);

            if (!containsSource)
                updated += $"\n[SOURCE]\n{model.Source}";

            return updated;
        }
    }
}
