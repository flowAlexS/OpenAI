using System.Threading.Tasks;
using Trados.GenAI.Addon.OpenAI.DAL.Entities;

namespace Trados.GenAI.Addon.OpenAI.Interfaces
{
    public interface ITranslationRepository
    {
        Task SaveTranslation(TranslationEntity translationEntity);

        Task<string> GetTranslationAsync(TranslationEntity translationEntity);
    }
}
