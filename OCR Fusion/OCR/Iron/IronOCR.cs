namespace OCR_Fusion.OCR.Iron
{
    using IronOcr;
    using MongoDB.Bson;
    using OCR_Fusion.API_Object;
    using System.Text.Json.Nodes;

    [Register("IronOCR","Iron","Great recognition of printed caracters")]
    public class IronOCR : IOCRManager {
        OutputDefinition output = new();

        public JsonObject GetParameters() {

            JsonObject parameters = new();

            JsonObject lang = new() {
                { "type"        , "text" },
                { "title"       , "Lang" },
                { "description" , "Let defined witch lang the algorithm will use" },
                { "default"     , "fr" }
            };

            JsonObject fine = new() {
                { "type"        , "check" },
                { "title"       , "Fine" },
                { "description" , "Let defined witch lang the algorithm will use" }
            };

            JsonObject country = new();

            Dictionary<string,string> options = new(){
                {"fr", "French"},
                {"nl", "Dutch"},
                {"en", "English"} 
            };

            country.Add("type"          , "select");
            country.Add("title"         , "Country");
            country.Add("description"   , "Let defined witch lang the algorithm will use");
            country.Add("options"       , options.ToJson());

            parameters.Add("lang"   ,lang);
            parameters.Add("fine"   ,fine);
            parameters.Add("country", country);

            return parameters;
        }

        public OutputDefinition GetText(InputDefinition input)
        {
            IronText(input);
            output.imageName = input.imageName;
            return output;
        }

        private void IronText(InputDefinition input)
        {
            var Ocr = new IronTesseract();
            
                
            using (var ironInput = new OcrInput())
            {
                if (input.regions.Count != 0)
                {
                    Vector xy1 = input.regions[0][0];
                    Vector xy2 = input.regions[0][1];
                    float inputWidth = xy2.x - xy1.x;
                    float inputHeight = xy2.y - xy1.y;
                    var ContentArea = new CropRectangle(x: xy1.x, y: xy1.y, width: inputWidth, height: inputHeight);
                    ironInput.AddImage(@"Uploads\" + input.imageName, ContentArea);
                }
                else
                {
                    ironInput.AddImage(@"Uploads\" + input.imageName);
                }
                var Result = Ocr.Read(ironInput);
                output.words.Add(Result.Text);
            }
        }
    }
}
