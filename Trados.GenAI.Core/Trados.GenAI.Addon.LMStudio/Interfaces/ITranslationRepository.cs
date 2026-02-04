using System.Threading.Tasks;
using Trados.GenAI.Addon.LMStudio.DAL.Entities;

namespace Trados.GenAI.Addon.LMStudio.Interfaces
{
    public interface ITranslationRepository
    {
        Task SaveTranslation(TranslationEntity translationEntity);

        Task<string> GetTranslationAsync(TranslationEntity translationEntity);
    }
}
