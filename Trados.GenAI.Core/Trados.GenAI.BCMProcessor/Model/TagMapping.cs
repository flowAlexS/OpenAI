using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.BCMProcessor.Models
{
    public class TagMapping
    {
        public MarkupData MarkupData { get; set; }

        public string TextEquivalent { get; set; }

        public string Id { get; set; }

        public string TagId { get; set; }

        public int Index { get; set; }
    }
}
