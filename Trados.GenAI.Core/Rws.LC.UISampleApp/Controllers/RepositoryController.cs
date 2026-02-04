using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rws.LC.UISampleApp.Models.Extensions;
using Rws.LC.UISampleApp.Infrastructure;
using Rws.LC.UISampleApp.Interfaces;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Rws.LC.UISampleApp.Models;
using Trados.GenAI.Storage;
using System.Linq;

namespace Rws.LC.UISampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<RepositoryController> logger;

        public RepositoryController(
            ILogger<RepositoryController> logger)
        {
            this.logger = logger;
        }
       
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> ListPromptItems()
        {
            var tenantId = HttpContext.User?.GetTenantId();
            var llmPromptRetriever = LLMPromptFactory.CreatePromptRetriever();
            var result = await llmPromptRetriever.RetrieveAlPromptItems(tenantId).ConfigureAwait(false);
            if (result?.Any() == false)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
