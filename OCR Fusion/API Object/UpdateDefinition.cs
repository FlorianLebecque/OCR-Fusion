using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace OCR_Fusion.API_Object {
    public class UpdateDefinition {

        [BsonId]
        public string Id { get; set; }
        public string Words { get; set; }
    }
}
