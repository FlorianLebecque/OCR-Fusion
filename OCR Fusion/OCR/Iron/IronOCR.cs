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

            JsonObject scale = new() {
                { "type"        , "text" },
                { "title"       , "Scale" },
                { "description" , "Scale factor" },
                { "default"     , "1" }
            };
            JsonObject rotate = new() {
                { "type"        , "text" },
                { "title"       , "Rotate" },
                { "description" , "Spefic rotation degrees" },
                { "default"     , "0" }
            };

            JsonObject lowqual = new() {
                { "type"        , "check" },
                { "title"       , "Low quality scan" },
                { "description" , "Straighten the image" }
            };
            JsonObject fastMod = new() {
                { "type"        , "check" },
                { "title"       , "Fast scan" },
                { "description" , "Performance Tuning for Speed" }
            };
            JsonObject denoise = new() {
                { "type"        , "check" },
                { "title"       , "Denoise" },
                { "description" , "Remove digital noise" }
            };
            JsonObject binarize = new() {
                { "type"        , "check" },
                { "title"       , "Binarize" },
                { "description" , "Turn all pixels black and white" }
            };
            JsonObject invert = new() {
                { "type"        , "check" },
                { "title"       , "Invert" },
                { "description" , "Invert all the colors" }
            };

            JsonObject language = new();
            Dictionary<string, string> options = new(){
                {"none", "None"},
                {"en", "English"},
                {"fr", "French"},
                {"nl", "Dutch"},
                {"de", "German"},
                {"it", "Italian"},
                {"pt", "Portuguese"},
                {"es", "Spanish"},
            };

            language.Add("type", "select");
            language.Add("title", "Language");
            language.Add("description", "Language used for text recognition");
            language.Add("options", options.ToJson());

            parameters.Add("lang" , language);
            parameters.Add("scale", scale);
            parameters.Add("rotate", rotate);
            parameters.Add("lowqual", lowqual);
            parameters.Add("denoise", denoise);
            parameters.Add("binarize", binarize);
            parameters.Add("invert", invert);
            parameters.Add("fastMod", fastMod);

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
