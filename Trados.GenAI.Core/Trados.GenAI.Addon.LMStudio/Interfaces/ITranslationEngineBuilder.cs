using Trados.GenAI.Addon.LMStudio.Models;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.LMStudio.Interfaces
{
    public interface ITranslationEngineBuilder
    {
        Task<TranslationEnginesResult> Build(TranslationEnginesRequestModel input);
    }
}
