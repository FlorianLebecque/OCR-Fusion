using System.Numerics;

namespace OCR_Fusion {
    public class InputDefinition {
        string imageLink { get; set; }
        bool IsHandWritten{ get; set; }
        List<Vector2[]> regions { get; set; }
    }
}
