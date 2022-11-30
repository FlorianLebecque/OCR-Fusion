using System.Numerics;

namespace OCR_Fusion {
    public class InputDefinition {
        public string imageLink { get; set; }
        public bool IsHandWritten { get; set; }
        public List<Vector2[]> regions { get; set; }

    }
}
