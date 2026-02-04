using MongoDB.Driver;
using Trados.GenAI.Storage.Interfaces;

namespace Trados.GenAI.Storage.Services
{
    public class DatabaseContext : IDatabaseContext
    {
        public IMongoDatabase Mongo { get; }

        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "GenAIStorage";

        public DatabaseContext()
        {
            var connection = new MongoClient(ConnectionString);
            Mongo = connection.GetDatabase(DatabaseName);
        }
    }
}
