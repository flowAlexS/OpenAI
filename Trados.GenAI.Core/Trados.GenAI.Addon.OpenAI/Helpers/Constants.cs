namespace Trados.GenAI.Addon.OpenAI.Helpers
{
    public class Constants
    {
        # region descriptor configuration keys
        public const string GoogleServiceKey = "SAMPLE_ACCOUNT_SECRET";
        public const string GoogleProjectId = "SAMPLE_PROJECT_ID";
        public const string GoogleLocation = "SAMPLE_LOCATION";
        #endregion

        #region descriptor openai configuration keys

        public const string OpenAIApiKey = "apiKey";
        public const string OpenAIModel = "model";
        public const string OpenAIModelType = "modelType";
        public const string OpenAISystemInstructions = "systemInstructions";
        public const string OpenAIUserPrompt = "userPrompt";
        public const string OpenAIIncludeTags = "includeTags";
        public const string OpenAIInContextImage = "useContextImage";
        public const string OpenAIDynamicSystemInstructions = "useDynamicSystemInstructions";
        public const string OpenAIDynamicUserPrompt = "useDynamicUserPrompt";
        public const string OpenAICustomPromptTemplate = "useCustomTemplates";
        public const string PromptTemplate = "promptTemplate";
        #endregion
    }
}
