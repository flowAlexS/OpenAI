using MongoDB.Driver;

namespace Trados.GenAI.Storage.Interfaces
{
    public interface IDatabaseContext
    {
        IMongoDatabase Mongo { get; }
    }
}
