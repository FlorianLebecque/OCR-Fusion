namespace OCR_Fusion.OCR.Iron
{
    using IronOcr;
    using System.Text.Json.Nodes;

    [Register("IronOCR","Iron","Great recognition of printed caracters")]
    public class IronOCR : IOCRManager {
        OutputDefinition output = new();

        public JsonObject GetParameters() {

            JsonObject parameters = new();

            parameters.Add("test","gee");
            
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
