using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.BCMProcessor.Interfaces;

namespace Trados.GenAI.BCMProcessor.Model
{
    public class BcmTranslated : IBCMResponse
    {
        public bool Success { get; set; }

        public Document TranslatedDocument { get; set; } = new Document();
    }
}
