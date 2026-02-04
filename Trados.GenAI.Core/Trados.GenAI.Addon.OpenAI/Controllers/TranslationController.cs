using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trados.GenAI.Addon.OpenAI.Infrastructure;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.Addon.OpenAI.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Trados.GenAI.Addon.OpenAI.Services;
using Trados.GenAI.LLMCoordinator;
using Trados.GenAI.LLMCoordinator.Model;
using Trados.GenAI.Storage;

namespace Trados.GenAI.Addon.OpenAI.Controllers
{
    [Route("v1")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;
        private readonly ISettingsService _settingsService;
        
        private readonly ITranslationRepository _translationRepository;
        private readonly ITranslationEngineBuilder
            _translationEngineBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TranslationController(
            ILogger<TranslationController> logger,
            ITranslationRepository translationRepository,
            ITranslationEngineBuilder translationEngineBuilder,
            ISettingsService settingsService)
        {
            _logger = logger;
            _translationRepository = translationRepository;
            _translationEngineBuilder = translationEngineBuilder;
            _settingsService = settingsService;
        }

        /// <summary>
        /// Receive content to be tranlated.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromHeader(Name = "TR_ID")] string traceId)
        {
            _logger.LogInformation("Gettings translations");
            var tenantId = HttpContext.User?.GetTenantId();

            _logger.LogError($"Trace ID: {traceId}");

            var openAISettings = await _settingsService.GetSettings(tenantId);

            var translationCoordinator =
                TranslationCoordinatorFactory.BuildCoordinator(
                    new AIModelBuilder(openAISettings),
                    new TranslationProvider());

            string content = string.Empty;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            var bcmRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<BCMModelRequest>(content);

            var translationRequest = new TranslationRequest()
            {
                Owner = tenantId,
                BcmDocument = bcmRequest.Contents,
                IncludeTags = openAISettings.IncludeTags,
                UseCache = false
            };
            var result = await translationCoordinator.ExecuteAsync(translationRequest);

            if (result.Success)
            {
                var translationResponse = new BCMModelResponse()
                {
                    Translation = result.TranslatedDocument
                };

                var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(translationResponse);
                return Ok(serialized);
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Returns the available transation engines
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("translation-engines")]
        public async Task<IActionResult> GetTranslationEngines([FromQuery] TranslationEnginesRequestModel translationEnginesRequest)
        {
            _logger.LogInformation("Getting translation engines");

            var translationEngine = await _translationEngineBuilder.Build(translationEnginesRequest);
            return Ok(translationEngine);
        }
    }
}
