using APIObject;
using System.Text.Json.Nodes;

namespace OCR_Fusion.OCR.Placeholder

{
    //[Register("placeholder", "Just a dumb thing", "Give a placeholder response, it does nothing")]
    public class PlaceholderOCR : IOCRManager
    {
        public JsonObject GetParameters() {

            JsonObject parameters = new();

            JsonObject return_text = new();
            return_text.Add("type", "text");
            return_text.Add("title", "Returned value");
            return_text.Add("description", "Output text returned by the OCR");
            return_text.Add("default", "Hello World!");

            parameters.Add("return", return_text);

            return parameters;
        }

        public OutputDefinition GetText(InputDefinition input)
        {

            List<string> text = new List<string> {
                (input.parameters["return"] != null)? input.parameters["return"]: "Hello world !"
            };

            Dictionary<string, Vector[]> regions = new Dictionary<string, Vector[]>();

            for (int i = 0; i < text.Count; i++)
            {
                regions.Add(i.ToString(), new Vector[] { new Vector(10, 10 + i * 10), new Vector(10, 10 + i * 10) });
            }


            OutputDefinition output = new OutputDefinition();

            output.imageName = input.imageName;
            output.words = text;
            output.regions = regions;


            return output;
        }
    }
}
