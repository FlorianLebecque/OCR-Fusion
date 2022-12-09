
using OCR_Fusion.API_Object;

namespace OCR_Fusion {
    public class InputDefinition {

        public string session { get; set; }
        public string imageName { get; set; }
        public string algo { get; set; }
        public List<Vector[]> regions { get; set; }

    }
}
