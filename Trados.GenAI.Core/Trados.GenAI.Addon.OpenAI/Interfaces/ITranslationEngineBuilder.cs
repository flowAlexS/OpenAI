using Trados.GenAI.Addon.OpenAI.Models;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.OpenAI.Interfaces
{
    public interface ITranslationEngineBuilder
    {
        Task<TranslationEnginesResult> Build(TranslationEnginesRequestModel input);
    }
}
