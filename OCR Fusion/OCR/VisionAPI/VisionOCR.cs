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

    [Register("VisionOCR", "Vision", "Great recognition of printed and handwritten caracters")]
        public class VisionOCR : IOCRManager {
        OutputDefinition output = new();

        public JsonObject GetParameters() {
            return new JsonObject();
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
