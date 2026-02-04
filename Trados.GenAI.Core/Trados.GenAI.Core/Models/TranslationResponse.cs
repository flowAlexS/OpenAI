using Trados.GenAI.Core.Interfaces;

namespace Trados.GenAI.Core.Models
{
    public class TranslationResponse : ITranslationResponse
    {
        public string BaseResponse { get; set; } = string.Empty;

        public List<TranslationUnit> TranslationUnits { get; set; } = new List<TranslationUnit>();
    }
}
