using System.Numerics;

namespace OCR_Fusion {
    public class OutputDefinition {
        string imageLink { get; set; }
        List<string> words { get; set; }
        Dictionary<string, Vector2[]> regions { get; set; }
    }
}
