using MongoDB.Bson.Serialization.Attributes;

namespace APIObject {
    public class UpdateDefinition {

        [BsonId]
        public string Id { get; set; }
        public string Words { get; set; }
    }
}
