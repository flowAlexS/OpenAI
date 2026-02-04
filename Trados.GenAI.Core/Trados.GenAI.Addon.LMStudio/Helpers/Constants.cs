namespace Trados.GenAI.Addon.LMStudio.Helpers
{
    public class Constants
    {
        # region descriptor configuration keys
        public const string GoogleServiceKey = "SAMPLE_ACCOUNT_SECRET";
        public const string GoogleProjectId = "SAMPLE_PROJECT_ID";
        public const string GoogleLocation = "SAMPLE_LOCATION";
        #endregion

        #region descriptor openai configuration keys

        public const string LMStudioBaseURi = "baseUri";
        public const string LMStudioModel = "model";
        public const string LMStudioModelType = "modelType";
        public const string LMStudioSystemInstructions = "systemInstructions";
        public const string LMStudioUserPrompt = "userPrompt";
        public const string LMStudioIncludeTags = "includeTags";
        public const string LMStudioUseCache = "useCache";
        public const string LMStudioUseContextImage = "useContextImage";
        public const string LMStudioUseDynamicSystemInstructions = "useDynamicSystemInstructions";
        public const string LMStudioUseDynamicUserPrompt = "useDynamicUserPrompt";
        #endregion
    }
}
