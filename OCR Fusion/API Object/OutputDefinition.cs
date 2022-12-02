using OCR_Fusion.API_Object;

namespace OCR_Fusion {
    public class OutputDefinition {

        public OutputDefinition() {
            words = new();
            regions = new();
        }

        public string imageName { get; set; }
        public List<string> words { get; set; }
        public Dictionary<string, Vector[]>? regions { get; set; }
    }
}
