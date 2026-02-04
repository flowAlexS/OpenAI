using Sdl.Core.Bcm.BcmModel;

namespace Trados.GenAI.BCMProcessor.Interfaces
{
    public interface IBCMResponse
    {
       bool Success { get; }

       Document TranslatedDocument { get; }
    }
}
