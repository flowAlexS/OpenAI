using Trados.GenAI.Core.Interfaces;
using Trados.GenAI.Core.Models;

namespace Trados.GenAI.LMStudio.Models
{
    public class TranslationResponse : ITranslationResponse
    {
        public string BaseResponse { get; set; } = string.Empty;

        public List<TranslationUnit> TranslationUnits { get; set; } = new();
    }
}
