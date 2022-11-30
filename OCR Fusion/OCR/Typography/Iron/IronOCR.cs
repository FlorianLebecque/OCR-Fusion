namespace OCR_Fusion.OCR.Typography.Iron {
    using IronOcr;
    public class IronOCR : IOCRManager {
        public OutputDefinition GetText(InputDefinition input) {
            IronText();
            throw new NotImplementedException();
        }

        private List<string> IronText()
        {
            List<string> text = new List<string>();
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput())
            {
                Input.AddImage(@"OCR_Test.png");
                var Result = Ocr.Read(Input);
                Console.WriteLine(Result.Text);
                text.Add(Result.Text);
            }
            return text;
        }
    }
}
