using Trados.GenAI.Addon.OpenAI.Models;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.OpenAI.Interfaces
{
    public interface ISettingsService
    {
        Task<string> GetApiKey(string tenandId);

        Task<OpenAIModel> GetSettings(string tenandId);
    }
}
