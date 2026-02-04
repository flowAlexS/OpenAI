using Sdl.Core.Bcm.BcmModel;
using Trados.GenAI.BCMProcessor.Model;

namespace Trados.GenAI.BCMProcessor.Interfaces
{
    public interface IBCMProcessor
    {
        bool IsValidBCM { get; }

        void Initialize(Document? document);

        void UpdateParagraphUnit(ParagraphUnit paragraphUnit, string translation);

        List<IBcmData> GetTranslatableContents(bool includeTarget);

        IBcmData GetTranslatableContent(bool includeTarget);

        IBCMResponse TranslateDocument(List<string> translations);

        IBCMResponse BuildDocument();
    }
}
