using Trados.GenAI.Core.Models;

namespace Trados.GenAI.Core.Interfaces
{
    public interface ITranslationResponse
    {
        string BaseResponse { get; set; }

        List<TranslationUnit> TranslationUnits { get; set; }
    }
}
