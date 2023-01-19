namespace OCR_Fusion.OCR.AzureOCR
{
    using System;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
    using System.Threading.Tasks;
    using System.IO;
    using System.Threading;
    using System.Text.Json.Nodes;

    [Register("AzureOCR", "Azure Computer Vision", "Great for printed and handwritten text")]
        public class AzureOCR : IOCRManager
    {
        OutputDefinition output = new();
        

        static string subscriptionKey = "2869f8d23c7a49749844c75a9515d9ff"; // Include key
        static string endpoint = "https://azurevisionecam.cognitiveservices.azure.com/";
        static string uriBase = endpoint + "vision/v3.2/read/analyze"; // v2.1/ocr

        ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

        public OutputDefinition GetText(InputDefinition input)
        {
            string imageFilePath = @"Uploads\" + input.imageName;
            ReadFileUrl(client, imageFilePath, input, output).Wait();
            output.imageName = input.imageName;
            return output;
        }

        public JsonObject GetParameters() {
            return new JsonObject();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task ReadFileUrl(ComputerVisionClient client, string imagepath, InputDefinition input, OutputDefinition output)
        {
            Stream imageStream = Utils.CropOrNotImageStream(imagepath, input);
            var textHeaders = await client.ReadInStreamAsync(imageStream);

            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            string outputString = "";
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    outputString+= line.Text;
                    outputString += "\n";
                }
            }
            output.words.Add(outputString);
            imageStream.Dispose();
        }

    }
}
