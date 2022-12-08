using MongoDB.Bson.Serialization.Attributes;
using OCR_Fusion.API_Object;

namespace OCR_Fusion {
    public class OutputDefinition {

        public OutputDefinition() {
            words = new();
            regions = new();
        }

        [BsonId]
        public string session { get; set; }
        public string imageName { get; set; }
        public List<string> words { get; set; }
        public Dictionary<string, Vector[]>? regions { get; set; }
    }
}
