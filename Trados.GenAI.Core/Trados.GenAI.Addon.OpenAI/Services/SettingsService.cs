using Trados.GenAI.Addon.OpenAI.Enums;
using Trados.GenAI.Addon.OpenAI.Helpers;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.Addon.OpenAI.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Trados.GenAI.Addon.OpenAI.Services
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

        public async Task<OpenAIModel> GetSettings(string tenandId)
        {
            if (string.IsNullOrWhiteSpace(tenandId)) return null;

            var configurations = await _accountService.GetConfigurationSettings(tenandId, CancellationToken.None);

            return new OpenAIModel()
            {
                ApiKey = GetSettingAsString(Constants.OpenAIApiKey, configurations),
                Model = GetSettingAsString(Constants.OpenAIModel, configurations),
                ModelType = GetSettingAsString(Constants.OpenAIModelType, configurations),
                SystemInstructions = GetSettingAsString(Constants.OpenAISystemInstructions, configurations),
                UserPrompt = GetSettingAsString(Constants.OpenAIUserPrompt, configurations),
                IncludeTags = GetSettingsAsBoolean(Constants.OpenAIIncludeTags, configurations),
                UseContextImage = GetSettingsAsBoolean(Constants.OpenAIInContextImage, configurations),
                UseInFileSystemInstructions = GetSettingsAsBoolean(Constants.OpenAIDynamicSystemInstructions, configurations),
                UseInFileUserPrompt = GetSettingsAsBoolean(Constants.OpenAIDynamicUserPrompt, configurations),
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
