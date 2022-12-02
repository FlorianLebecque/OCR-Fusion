using System.Numerics;

namespace OCR_Fusion {
    public class OutputDefinition {
        public string imageName { get; set; }
        public List<string> words { get; set; }
        public Dictionary<string, Vector2[]>? regions { get; set; }
    }
}
