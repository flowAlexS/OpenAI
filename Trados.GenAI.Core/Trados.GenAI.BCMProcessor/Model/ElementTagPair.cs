namespace Trados.GenAI.BCMProcessor.Models
{
    public class ElementTagPair : Element
    {
        public TagType Type { get; set; }

        public string TagContent { get; set; }

        public string DisplayText { get; set; }
    }
}
