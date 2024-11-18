using MongoDB.Bson;
using MongoDB.Driver;

namespace DPSIW.Common.Services
{
    public class MongoService
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoService(string connStr, string databaseName, string collectionName)
        {
            _mongoClient = new MongoClient(connStr);
            _database = _mongoClient.GetDatabase(databaseName);
            _collection = _database.GetCollection<BsonDocument>(collectionName);
            var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("updated");
            var indexModel = new CreateIndexModel<BsonDocument>(indexKeys)!;
            //await _collection.Indexes.CreateOneAsync(indexModel);
        }

        //public async Task CreateCollectionWithIndexAsync(BsonDocument doc)
        //{
        //    // Create index
        //    //var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("id");
        //    //var indexOptions = new CreateIndexOptions { Unique = true };
        //    //await _collection.Indexes.CreateOneAsync(indexKeys, indexOptions);
        //    // Alternatively, create collection with index
        //    // var collectionSettings = new MongoCollectionSettings { };
        //    // collectionSettings.Indexes.Add(new CreateIndexModel<BsonDocument>(indexKeys, indexOptions));
        //    // _database.CreateCollection(collectionName, collectionSettings);

        //}

        public async Task UpsertDataAsync(BsonDocument document)
        {
            //var filter = Builders<BsonDocument>.Filter.Eq("id", document["id"]);
            await _collection.InsertOneAsync(document);
        }
    }
}
