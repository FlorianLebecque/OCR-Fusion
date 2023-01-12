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

    [Register("AzureOCR", "Azure", "Great recognition of printed and handwritten caracters")]
        public class AzureOCR : IOCRManager
    {
        OutputDefinition output = new();

        static string subscriptionKey = ""; // Include key
        static string endpoint = "https://azurevisionecam.cognitiveservices.azure.com/";
        static string uriBase = endpoint + "vision/v3.2/read/analyze"; // v2.1/ocr

        //ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

        public OutputDefinition GetText(InputDefinition input)
        {
            //Task task = VisionTextAsync(input.imageName);
            
            output.imageName = input.imageName;
            return output;
        }
        public JsonObject GetParameters()
        {

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

            Dictionary<string, string> options = new(){
                {"fr", "French"},
                {"nl", "Dutch"},
                {"en", "English"}
            };

            country.Add("type", "select");
            country.Add("title", "Country");
            country.Add("description", "Let defined witch lang the algorithm will use");
            country.Add("options", options.ToJson());

            parameters.Add("lang", lang);
            parameters.Add("fine", fine);
            parameters.Add("country", country);

            return parameters;
        }

        //private async Task VisionTextAsync(string imageName)
        //{
        //    string imageFilePath = @"Uploads\" + imageName;

        //    if (File.Exists(imageFilePath))
        //    {
        //        await MakeOCRRequest(imageFilePath, output);
        //    }
        //        //ReadFileUrl(client, imagepath, output).Wait();
        //}

        //static async Task MakeOCRRequest(string imageFilePath, OutputDefinition output)
        //{
        //    try
        //    {
        //        HttpClient client = new HttpClient();

        //        // Request headers.
        //        client.DefaultRequestHeaders.Add(
        //            "Ocp-Apim-Subscription-Key", subscriptionKey);

        //        // Request parameters. 
        //        // The language parameter doesn't specify a language, so the 
        //        // method detects it automatically.
        //        // The detectOrientation parameter is set to true, so the method detects and
        //        // and corrects text orientation before detecting text.
        //        string requestParameters = "language=en"; // params : &detectOrientation=true

        //        // Assemble the URI for the REST API method.
        //        string uri = uriBase + "?" + requestParameters;

        //        HttpResponseMessage response;

        //        // Read the contents of the specified local image
        //        // into a byte array.
        //        byte[] byteData = GetImageAsByteArray(imageFilePath);

        //        // Add the byte array as an octet stream to the request body.
        //        using (ByteArrayContent content = new ByteArrayContent(byteData))
        //        {
        //            // This example uses the "application/octet-stream" content type.
        //            // The other content types you can use are "application/json"
        //            // and "multipart/form-data".
        //            content.Headers.ContentType =
        //                new MediaTypeHeaderValue("application/octet-stream");

        //            // Asynchronously call the REST API method.
        //            response = await client.PostAsync(uri, content);
        //        }

        //        // Asynchronously get the JSON response.
        //        // string contentString = await response.Content.ReadAsStringAsync();

        //        // Display the JSON response.
        //        // Console.WriteLine("\nResponse:\n\n{0}\n",
        //        //JToken.Parse(contentString).ToString());

        //        //output.words.Add()
               
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("\n" + e.Message);
        //    }
        //}

        //static byte[] GetImageAsByteArray(string imageFilePath)
        //{
        //    // Open a read-only file stream for the specified file.
        //    using (FileStream fileStream =
        //        new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        // Read the file's contents into a byte array.
        //        BinaryReader binaryReader = new BinaryReader(fileStream);
        //        return binaryReader.ReadBytes((int)fileStream.Length);
        //    }
        //}

        public JsonObject GetParameters() {
            return new JsonObject();
        }

        //public static ComputerVisionClient Authenticate(string endpoint, string key)
        //{
        //    ComputerVisionClient client =
        //      new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
        //      { Endpoint = endpoint };
        //    return client;
        //}

        //public static async Task ReadFileUrl(ComputerVisionClient client, string urlFile, OutputDefinition output)
        //{
        //    // Read text from URL
        //    var textHeaders = await client.ReadAsync(urlFile);
        //    // After the request, get the operation location (operation ID)
        //    string operationLocation = textHeaders.OperationLocation;
        //    Thread.Sleep(2000);

        //    // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        //    // We only need the ID and not the full URL
        //    const int numberOfCharsInOperationId = 36;
        //    string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

        //    // Extract the text
        //    ReadOperationResult results;
        //    Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
        //    Console.WriteLine();
        //    do
        //    {
        //        results = await client.GetReadResultAsync(Guid.Parse(operationId));
        //    }
        //    while ((results.Status == OperationStatusCodes.Running ||
        //        results.Status == OperationStatusCodes.NotStarted));

        //    // Display the found text.
        //    Console.WriteLine();
        //    var textUrlFileResults = results.AnalyzeResult.ReadResults;
        //    foreach (ReadResult page in textUrlFileResults)
        //    {
        //        foreach (Line line in page.Lines)
        //        {
        //            Console.WriteLine(line.Text);
        //            output.words.Add(line.Text);
        //        }
        //    }
        //    Console.WriteLine();
        //}

    }
}
