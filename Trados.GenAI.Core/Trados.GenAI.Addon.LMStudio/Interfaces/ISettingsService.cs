using Trados.GenAI.Addon.LMStudio.Models;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.LMStudio.Interfaces
{
    public interface ISettingsService
    {
        Task<string> GetApiKey(string tenandId);

        Task<LMStudioModel> GetSettings(string tenandId);
    }
}
