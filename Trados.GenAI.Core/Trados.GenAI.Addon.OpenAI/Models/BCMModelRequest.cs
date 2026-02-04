using Newtonsoft.Json;
using Sdl.Core.Bcm.BcmModel;
using System.Collections.Generic;

namespace Trados.GenAI.Addon.OpenAI.Models
{
    public class BCMModelRequest
    {
        [JsonProperty("engineId")]
        public string EngineId { get; set; }

        [JsonProperty("contents")]
        public Document Contents { get; set; }
    }
}
