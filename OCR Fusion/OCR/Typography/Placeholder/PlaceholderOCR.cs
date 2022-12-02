using System.Numerics;

namespace OCR_Fusion.OCR.Typography.Placeholder {
    public class PlaceholderOCR : IOCRManager {
        public OutputDefinition GetText(InputDefinition input) {

            List<string> text = new List<string> {
                "hello",
                "World",
                "!",
                "How",
                "is",
                "your",
                "day",
                "going",
                "?"
            };

            Dictionary<string, Vector2[]> regions = new Dictionary<string, Vector2[]>();

            for(int i = 0; i < text.Count; i++) {
                regions.Add(i.ToString(), new Vector2[] { new Vector2(10, 10 + (i*10)), new Vector2(10, 10 + (i * 10)) });
            }


            OutputDefinition output= new OutputDefinition();

            output.imageName = "test.png";
            output.words = text;
            output.regions = regions;


            return output;
        }
    }
}
