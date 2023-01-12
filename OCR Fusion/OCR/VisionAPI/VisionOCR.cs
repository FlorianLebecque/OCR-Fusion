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

    [Register("VisionOCR", "Vision", "Great recognition of printed and handwritten caracters")]
        public class VisionOCR : IOCRManager {
        OutputDefinition output = new();

        public JsonObject GetParameters()
        {

            JsonObject parameters = new();

            JsonObject scale = new() {
                { "type"        , "text" },
                { "title"       , "Scale" },
                { "description" , "Let defined witch lang the algorithm will use" },
                { "default"     , "1" }
            };

            JsonObject document = new() {
                { "type"        , "check" },
                { "title"       , "Document" },
                { "description" , "Let defined witch lang the algorithm will use" }
            };

            JsonObject language = new();
            Dictionary<string, string> options = new(){
                {"en", "English"},
                {"fr", "French"}
            };

            language.Add("type", "select");
            language.Add("title", "Language");
            language.Add("description", "Let defined witch lang the algorithm will use");
            language.Add("options", options.ToJson());

            parameters.Add("scale", scale);
            parameters.Add("document", document);
            parameters.Add("language", language);

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
            Google.Cloud.Vision.V1.Image image1 = Google.Cloud.Vision.V1.Image.FromFile(pathfile);
            if (input.regions.Count != 0)
            {
                Byte[] cropImage = Utils.CropImage(pathfile, input.regions[0]);
                image1 = Google.Cloud.Vision.V1.Image.FromBytes(cropImage);
            }
            if (input.parameters["document"] == "on")
            {
                var response = client.DetectDocumentText(image1);
                foreach (var page in response.Pages)
                {
                    foreach (var block in page.Blocks)
                    {
                        string box = string.Join(" - ", block.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                        Console.WriteLine($"Block {block.BlockType} at {box}");
                        foreach (var paragraph in block.Paragraphs)
                        {
                            box = string.Join(" - ", paragraph.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                            Console.WriteLine($"  Paragraph at {box}");
                            foreach (var word in paragraph.Words)
                            {
                                Console.WriteLine($"    Word: {string.Join("", word.Symbols.Select(s => s.Text))}");
                            }
                        }
                    }
                }

            }
            else
            {
                var response = client.DetectText(image1);
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
