namespace OCR_Fusion.OCR.VisionAPI
{
    using Google.Cloud.Vision.V1;
    using System;
    using Grpc.Auth;
    using Google.Apis.Auth.OAuth2;
    using System.Buffers.Text;
    using System.Text.Json.Nodes;
    using IronOcr;
    using ZstdSharp.Unsafe;
    using SharpCompress.Common;
    using System.Drawing;
    using System.Drawing.Imaging;
    using MongoDB.Bson;
    using static System.Net.Mime.MediaTypeNames;
    using static Google.Protobuf.Reflection.GeneratedCodeInfo.Types;
    using Google.Protobuf.Collections;

    [Register("VisionOCR", "Vision", "Great recognition of printed and handwritten caracters")]
        public class VisionOCR : IOCRManager {
        OutputDefinition output = new();

        public JsonObject GetParameters()
        {

            JsonObject parameters = new();

            JsonObject scale = new() {
                { "type"        , "text" },
                { "title"       , "Scale" },
                { "description" , "Scale factor" },
                { "default"     , "1" }
            };

            JsonObject document = new() {
                { "type"        , "check" },
                { "title"       , "Document" },
                { "description" , "Better results for dense text" }
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

            parameters.Add("language", language);
            parameters.Add("scale", scale);
            parameters.Add("document", document);
            

            return parameters;
        }

        public OutputDefinition GetText(InputDefinition input)
        {
            VisionText(input);
            output.imageName = input.imageName;
            return output;
        }
        private void VisionText(InputDefinition input)
        {
            string credential_path = @"ocrproject-371113-b08aeae56092.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
            var client = ImageAnnotatorClient.Create();
            string pathfile = @"Uploads\" + input.imageName;
            ImageContext context = new ImageContext();
            
            Google.Cloud.Vision.V1.Image image1 = Google.Cloud.Vision.V1.Image.FromFile(pathfile);
            switch (input.parameters["language"])
            {
                case "en":
                    context.LanguageHints.Add("en");
                    break;
                case "fr":
                    context.LanguageHints.Add("fr");
                    break;
                case "nl":
                    context.LanguageHints.Add("nl");
                    break;
                case "de":
                    context.LanguageHints.Add("de");
                    break;
                case "it":
                    context.LanguageHints.Add("it");
                    break;
                case "pt":
                    context.LanguageHints.Add("pt");
                    break;
                case "es":
                    context.LanguageHints.Add("es");
                    break;
                case "none":
                    break;
                default:
                    break;
            }
            if (input.regions.Count != 0)
            {
                Byte[] cropImage = Utils.CropImage(pathfile, input.regions[0]);
                image1 = Google.Cloud.Vision.V1.Image.FromBytes(cropImage);
            }
            if (input.parameters["document"] == "true")
            {
                var response = client.DetectDocumentText(image1, context);
                output.words.Add(response.Text);
            }
            else
            {
                var response = client.DetectText(image1, context);
                foreach (var annotation in response)
                {
                    if (annotation.Description != null)
                    {
                        output.words.Add(annotation.Description);
                    }
                }
            }

        }
    }
}
