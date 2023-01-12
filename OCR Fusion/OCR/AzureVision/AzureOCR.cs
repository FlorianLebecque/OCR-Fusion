namespace OCR_Fusion.OCR.AzureOCR
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
    using System.Threading.Tasks;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Threading;
    using System.Linq;

    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Text.Json.Nodes;
    using MongoDB.Bson;
    using System.Drawing.Imaging;
    using static IronOcr.OcrResult;

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
            //Task task = VisionTextAsync(input.imageName);
            ReadFileUrl(client, imageFilePath, output).Wait();
            output.imageName = input.imageName;
            return output;
        }

        private async Task VisionTextAsync(string imageFilePath)
        {
            if (File.Exists(imageFilePath))
            {
                await MakeOCRRequest(imageFilePath, output);
            }
            
        }

        static async Task MakeOCRRequest(string imageFilePath, OutputDefinition output)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=en"; // params : &detectOrientation=true

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    ReadOperationResult results;
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    //do
                    //    {
                    //        results = await client.GetReadResultAsync(uri,Guid.Parse(content));
                    //    }
                    //while ((results.Status == OperationStatusCodes.Running ||
                    //    results.Status == OperationStatusCodes.NotStarted));

                    //response = await client.PostAsync(uri, content);
                    //response.Content.ReadAsStringAsync();
                }

                // Asynchronously get the JSON response.
                // string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                // Console.WriteLine("\nResponse:\n\n{0}\n",
                //JToken.Parse(contentString).ToString());

                //output.words.Add()

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
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

        public static async Task ReadFileUrl(ComputerVisionClient client, string imagepath, OutputDefinition output)
        {
            // Read text from URL
            Stream imageStream = ToStream(imagepath);
            var textHeaders = await client.ReadInStreamAsync(imageStream);
            
            //var textHeaders = await client.ReadAsync(imagepath);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);
            var text = textHeaders.OperationLocation;

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
                foreach (Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models.Line line in page.Lines)
                {
                    outputString+= line.Text;
                    outputString += "\n";
                }
            }
            output.words.Add(outputString);
        }
        public static Stream ToStream(string imagePath)
        {
            Stream stream = new FileStream(imagePath, FileMode.Open);
            return stream;
        }

    }
}
