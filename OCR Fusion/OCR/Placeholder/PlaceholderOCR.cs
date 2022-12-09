using OCR_Fusion.API_Object;

namespace OCR_Fusion.OCR.Placeholder
{
    public class PlaceholderOCR : IOCRManager
    {
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
