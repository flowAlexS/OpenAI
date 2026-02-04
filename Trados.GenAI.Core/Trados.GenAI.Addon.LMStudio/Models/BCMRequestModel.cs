using Newtonsoft.Json;
using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.Addon.LMStudio.Models
{
    public class BCMModelRequest
    {
        [JsonProperty("engineId")]
        public string EngineId { get; set; }

        [JsonProperty("contents")]
        public Document Contents { get; set; }
    }
}
