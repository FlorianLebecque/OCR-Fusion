using MongoDB.Bson;
using MongoDB.Driver;

namespace OCR_Fusion.Database {
    public class MangoCRUD : IDBCrud {
        private IMongoDatabase db;

        public MangoCRUD(string database_name) {


            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://test:123@cluster0.syhqt6b.mongodb.net/?retryWrites=true&w=majority");


            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);
            db = client.GetDatabase(database_name);

        }

        public void Delete<T>(string table, string session) {
            throw new NotImplementedException();
        }

        public List<T> Gets<T>(string table, string session) {

            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList<T>();

        }

        public void Insert<T>(string table, T record) {

            var collection = db.GetCollection<T>(table);

            collection.InsertOne(record);

        }

        public void Update<T>(string table,Guid id, T value) {

            var collection = db.GetCollection<T>(table);

            var result = collection.ReplaceOne(new BsonDocument("_id", id),value,new ReplaceOptions { IsUpsert = true});

        }
    }
}
