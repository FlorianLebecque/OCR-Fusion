namespace OCR_Fusion.OCR.VisionAPI
{
    using Google.Cloud.Vision.V1;
    using System;
    using Grpc.Auth;
    using Google.Apis.Auth.OAuth2;
    using System.Buffers.Text;

    [Register("VisionOCR", "Vision", "Recognise printed caractere")]
        public class VisionOCR : IOCRManager
    {
        OutputDefinition output = new();
        public OutputDefinition GetText(InputDefinition input)
        {
            VisionText(input.imageName);
            output.imageName = input.imageName;
            return output;
        }
        private void VisionText(string imageName)
        {
            string credential_path = @"ocrproject-371113-b08aeae56092.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
            var client = ImageAnnotatorClient.Create();
            Image image1 = Image.FromFile(@"Uploads\" + imageName);

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
