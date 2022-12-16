using OCR_Fusion.API_Object;
using System.Text.Json.Nodes;

namespace OCR_Fusion.OCR.Placeholder

{
    [Register("placeholder", "Just a dumb thing", "Give a placeholder response, it does nothing")]
    public class PlaceholderOCR : IOCRManager
    {
        public JsonObject GetParameters() {
            return new();
        }

        public OutputDefinition GetText(InputDefinition input)
        {

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

            Dictionary<string, Vector[]> regions = new Dictionary<string, Vector[]>();

            for (int i = 0; i < text.Count; i++)
            {
                regions.Add(i.ToString(), new Vector[] { new Vector(10, 10 + i * 10), new Vector(10, 10 + i * 10) });
            }


            OutputDefinition output = new OutputDefinition();

            output.imageName = "test.png";
            output.words = text;
            output.regions = regions;


            return output;
        }
    }
}
