using Trados.GenAI.Addon.OpenAI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trados.GenAI.Addon.OpenAI.Interfaces;

namespace Trados.GenAI.Addon.OpenAI.Services
{
    public class TranslationEngineBuilder : ITranslationEngineBuilder
    {
        private const string EngineModel = "nmt";

        public Task<TranslationEnginesResult> Build(TranslationEnginesRequestModel input)
        {
            var translationEngineResponse = new TranslationEnginesResult(new List<TranslationEngineModel>());
            foreach (var item in input.TargetLanguage)
            {
                var engineId = string.Join("-", new[]
                {
                    input.SourceLanguage,
                    item,
                    "OPENAI",
                    EngineModel
                });

                var translationEngineItem = new TranslationEngineModel()
                {
                    Id = engineId,
                    Model = EngineModel,
                    EngineSourceLanguage = input.SourceLanguage,
                    EngineTargetLanguage = item,
                    MatchingSourceLanguage = input.SourceLanguage,
                    MatchingTargetLanguages = [item]
                };

                translationEngineResponse.Items.Add(translationEngineItem);
                translationEngineResponse.ItemCount++;
            }

            return Task.FromResult(translationEngineResponse);
        }
    }
}
