namespace OCR_Fusion.OCR.Iron
{
    using IronOcr;
    using MongoDB.Bson;
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
            IronText(input.imageName);
            output.imageName = input.imageName;
            return output;
        }

        private void IronText(string imageName)
        {
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput())
            {
                Input.AddImage(@"Uploads\" + imageName);
                var Result = Ocr.Read(Input);
                output.words.Add(Result.Text);
            }
        }
    }
}
