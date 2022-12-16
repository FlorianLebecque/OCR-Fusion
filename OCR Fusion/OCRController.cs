namespace OCR_Fusion {
    public class OCRController {

        private IOCRManager ocrManager;        

        public OCRController(IOCRManager oCRManager ) { 
            this.ocrManager = oCRManager;
        }

        public OutputDefinition GetText(InputDefinition input) {

            OutputDefinition output = ocrManager.GetText(input);

            output.session = input.session;
            output.algorithm = input.algo;
            output.parameters = input.parameters;

            Utils.CheckImage(input.imageName);

            Utils.Insert<OutputDefinition>("outputs", output);

            return output;
        }

        public static string UploadImage(IFormFile image) {

            Utils.CheckUploadDir();

            if (image.Length == 0) {
                return "0";
            }

            string extention = Utils.GetExtention(image.FileName);
            if (!Utils.allowedExtention.Contains(extention)) {
                return "not supported extention";
            }

            Utils.SaveImage(image);
            
            return image.FileName;
        }
    }
}
