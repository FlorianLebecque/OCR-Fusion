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
            string pathfile = @"Uploads\" + input.imageName;

            using (var ironInput = new OcrInput())
            {
                if (input.regions.Count != 0)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(pathfile);
                    int x1 = (int)(input.regions[0][0].x * img.Width);
                    int y1 = (int)(input.regions[0][0].y * img.Height);
                    int x2 = (int)(input.regions[0][1].x * img.Width);
                    int y2 = (int)(input.regions[0][1].y * img.Height);
                    int inputWidth = x2 - x1;
                    int inputHeight = y2 - y1;
                    var ContentArea = new CropRectangle(x: x1, y: y1, width: inputWidth, height: inputHeight);
                    ironInput.AddImage(pathfile, ContentArea);
                }
                else
                {
                    ironInput.AddImage(pathfile);
                }
                var Result = Ocr.Read(ironInput);
                output.words.Add(Result.Text);
            }
        }
    }
}
