namespace Trados.GenAI.BCMProcessor.Models
{
    public abstract class Element : ICloneable
    {
        public enum TagType
        {
            TagOpen,
            TagClose,
        }

        public int Anchor { get; set; }

        public string TagId { get; set; }

        public string TagMappingId { get; set; }

        public object Clone()
        {
            TagId = TagId;
            TagMappingId = TagMappingId;

            if (this is ElementComment comment)
            {
                return new ElementComment
                {
                    Id = comment.Id,
                    Type = comment.Type
                };
            }

            if (this is ElementLocked locked)
            {
                return new ElementLocked
                {
                    Type = locked.Type
                };
            }

            if (this is ElementPlaceholder placeholder)
            {
                return new ElementPlaceholder
                {
                    DisplayText = placeholder.DisplayText,
                    TagContent = placeholder.TagContent,
                    TextEquivalent = placeholder.TextEquivalent
                };
            }

            if (this is ElementGenericPlaceholder elementGenericPlaceholder)
            {
                return new ElementGenericPlaceholder
                {
                    CType = elementGenericPlaceholder.CType,
                    Name = elementGenericPlaceholder.Name,
                    TextEquivalent = elementGenericPlaceholder.TextEquivalent
                };
            }

            if (this is ElementSegment segment)
            {
                return new ElementSegment
                {
                    Id = segment.Id,
                    Type = segment.Type
                };
            }

            if (this is ElementTagPair tagPair)
            {
                return new ElementTagPair
                {
                    DisplayText = tagPair.DisplayText,
                    TagContent = tagPair.TagContent,
                    Type = tagPair.Type
                };
            }

            if (this is ElementText text)
            {
                return new ElementText
                {
                    Text = text.Text
                };
            }

            return MemberwiseClone();
        }
    }

}
