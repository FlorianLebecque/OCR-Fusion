namespace OCR_Fusion.OCR.Typography.Iron {
    using IronOcr;
    public class IronOCR : IOCRManager {
        OutputDefinition output = new();
        public OutputDefinition GetText(InputDefinition input) {
            IronText(input.imageName);
            output.imageName = input.imageName;
            return output;
        }

        private void IronText(String imageName)
        {
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput())
            {
                Input.AddImage(@"Uploads\"+imageName);
                var Result = Ocr.Read(Input);
                output.words.Add(Result.Text);
            }
        }
    }
}
