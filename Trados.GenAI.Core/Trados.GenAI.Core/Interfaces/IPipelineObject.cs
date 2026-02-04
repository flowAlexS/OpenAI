namespace Trados.GenAI.Core.Interfaces
{
    public interface IPipelineObject
    {
        IAIModel? AIModel { get; }

        ITranslationContext? TranslationContext { get; set; }

        ITranslationResponse? TranslationResponse { get; set; }
    }
}
