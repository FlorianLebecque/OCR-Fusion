using MongoDB.Bson;
using MongoDB.Driver;

namespace OCR_Fusion.Database {
    public class MangoCRUD : IDBCrud {
        private IMongoDatabase db;

        public MangoCRUD(string database) {

            //Create the database if it don't exist

            //initiate database connection
            var client = new MongoClient();
            db = client.GetDatabase(database);

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

        public void Update<T>(string table, T value) {
            throw new NotImplementedException();
        }
    }
}
