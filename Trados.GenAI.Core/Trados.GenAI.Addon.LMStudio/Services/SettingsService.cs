using Trados.GenAI.Addon.LMStudio.Enums;
using Trados.GenAI.Addon.LMStudio.Helpers;
using Trados.GenAI.Addon.LMStudio.Interfaces;
using Trados.GenAI.Addon.LMStudio.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.LMStudio.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IAccountService _accountService;

        public SettingsService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // We'll be needed for validation..
        public Task<string> GetApiKey(string tenandId)
        {
            throw new System.Exception();
        }

        public async Task<LMStudioModel> GetSettings(string tenandId)
        {
            if (string.IsNullOrWhiteSpace(tenandId)) return null;

            var configurations = await _accountService.GetConfigurationSettings(tenandId, CancellationToken.None);

            return new LMStudioModel()
            {
                BaseUri = GetSettingAsString(Constants.LMStudioBaseURi, configurations),
                Model = GetSettingAsString(Constants.LMStudioModel, configurations),
                ModelType = GetSettingAsString(Constants.LMStudioModelType, configurations),
                SystemInstructions = GetSettingAsString(Constants.LMStudioSystemInstructions, configurations),
                UserPrompt = GetSettingAsString(Constants.LMStudioUserPrompt, configurations),
                IncludeTags = GetSettingsAsBoolean(Constants.LMStudioIncludeTags, configurations),
                UseCached = GetSettingsAsBoolean(Constants.LMStudioUseCache, configurations),
                UseContextImage = GetSettingsAsBoolean(Constants.LMStudioUseContextImage, configurations),
                UseInFileSystemInstructions = GetSettingsAsBoolean(Constants.LMStudioUseDynamicSystemInstructions, configurations),
                UseInFileUserPrompt = GetSettingsAsBoolean(Constants.LMStudioUseDynamicUserPrompt, configurations),
            };
        }

        private string GetSettingAsString(string propertyValue, ConfigurationSettingsResult configurations)
        {
            var settingAsString = configurations?.Items?.FirstOrDefault(c => c.Id.ToLower().Equals(propertyValue.ToLower()))?.Value?
                .ToString();
            return settingAsString;
        }

        private bool GetSettingsAsBoolean(string propertyValue, ConfigurationSettingsResult configurations)
        {
            var str = GetSettingAsString(propertyValue, configurations);
            var result = bool.TryParse(str, out var value);
            return value;
        }
    }
}
