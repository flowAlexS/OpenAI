using Newtonsoft.Json;
using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.Addon.LMStudio.Models
{
    public class BCMModelResponse
    {
        [JsonProperty("translations")]
        public Document Translation { get; set; }
    }
}
