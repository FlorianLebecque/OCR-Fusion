namespace OCR_Fusion.OCR.Iron
{
    using Google.Api;
    using IronOcr;
    using MongoDB.Bson;
    using OCR_Fusion.API_Object;
    using System.Text.Json.Nodes;

    [Register("IronOCR","IronOCR (Tesseract)","Great for printed text")]
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
                { "title"       , "Low quality image" },
                { "description" , "Straighten the image" }
            };
            JsonObject denoise = new() {
                { "type"        , "check" },
                { "title"       , "Denoise" },
                { "description" , "Remove digital noise" }
            };
            JsonObject sharpen = new() {
                { "type"        , "check" },
                { "title"       , "Sharpen" },
                { "description" , "Sharpen blurred image" }
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
            Dictionary<string, string> languages = new(){
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
            language.Add("options", languages.ToJson());

            JsonObject scanMode = new();
            Dictionary<string, string> options = new(){
                {"default", "Default"},
                {"fast", "Fast"},
                {"accurate", "Accurate"}
            };

            scanMode.Add("type", "select");
            scanMode.Add("title", "Scan mode");
            scanMode.Add("description", "Type of scan strategy");
            scanMode.Add("options", options.ToJson());

            parameters.Add("language" , language);
            parameters.Add("scale", scale);
            parameters.Add("rotate", rotate);
            parameters.Add("lowqual", lowqual);
            parameters.Add("denoise", denoise);
            parameters.Add("sharpen", sharpen);
            parameters.Add("binarize", binarize);
            parameters.Add("invert", invert);
            parameters.Add("scanMode", scanMode);

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

            switch (input.parameters["language"])
            {
                case "en":
                    Ocr.Language = OcrLanguage.English;
                    break;
                case "fr":
                    Ocr.Language = OcrLanguage.French;
                    break;
                case "nl":
                    Ocr.Language = OcrLanguage.Dutch;
                    break;
                case "de":
                    Ocr.Language = OcrLanguage.German;
                    break;
                case "it":
                    Ocr.Language = OcrLanguage.Italian;
                    break;
                case "pt":
                    Ocr.Language = OcrLanguage.Portuguese;
                    break;
                case "es":
                    Ocr.Language = OcrLanguage.Spanish;
                    break;
                case "none":
                    break;
                default:
                    break;
            }
            if (input.parameters["scanMode"] == "fast")
            {
                Ocr.Configuration.BlackListCharacters = "~`$#^*_}{][|\\@¢©«»°±·×‑–—‘’“”•…′″€™←↑→↓↔⇄⇒∅∼≅≈≠≤≥≪≫⌁⌘○◔◑◕●☐☑☒☕☮☯☺♡⚓✓✰";
                Ocr.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.Auto;
                Ocr.Configuration.TesseractVersion = TesseractVersion.Tesseract5;
                Ocr.Configuration.EngineMode = TesseractEngineMode.LstmOnly;
                Ocr.Configuration.ReadBarCodes = false;
                if (input.parameters["language"] == "en")
                {
                    Ocr.Language = OcrLanguage.EnglishFast;
                }
                if (input.parameters["language"] == "fr")
                {
                    Ocr.Language = OcrLanguage.FrenchFast;
                }
            }
            if (input.parameters["scanMode"] == "accurate")
            {
                if (input.parameters["language"] == "en")
                {
                    Ocr.Language = OcrLanguage.EnglishBest;
                }
                if (input.parameters["language"] == "fr")
                {
                    Ocr.Language = OcrLanguage.FrenchBest;
                }
            }

            using (var ironInput = new OcrInput())
            {
                if (input.parameters["lowqual"] == "true")
                {
                    ironInput.Deskew();
                }
                if (input.parameters["rotate"] != "0")
                {
                    ironInput.Rotate(int.Parse(input.parameters["rotate"]));
                }
                if (input.parameters["scale"] != "1")
                {
                    ironInput.Scale(int.Parse(input.parameters["rotate"]));
                }
                if (input.parameters["binarize"] == "true")
                {
                    ironInput.Binarize();
                }
                if (input.parameters["invert"] == "true")
                {
                    ironInput.Invert();
                }
                if (input.parameters["denoise"] == "true")
                {
                    ironInput.DeNoise();
                }

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
                    img.Dispose();
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
