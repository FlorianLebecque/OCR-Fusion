namespace OCR_Fusion {
    public class OCRController {

        private IOCRManager ocrManager;

        private const string uploadPath = "Uploads/";
        private static string[] allowedExtention {
            get {
                string[] ae = { "png", "jepg", "jpg" };
                return ae;
            }
        }

        public OCRController(IOCRManager oCRManager){ 
            this.ocrManager = oCRManager;
        }


        public OutputDefinition GetText(InputDefinition input) {

            OutputDefinition output = ocrManager.GetText(input);

            if (!File.Exists(uploadPath + input.imageLink)) {
                throw new Exception("File not found");
            }

            return output;
        }

        public static string UploadImage(IFormFile file) {
            if (!Directory.Exists(uploadPath)) {
                Directory.CreateDirectory(uploadPath);
            }

            if (file.Length == 0) {
                return "0";
            }

            string extention = Utils.GetExtention(file.FileName);
            if (!allowedExtention.Contains(extention)) {
                return "not supported extention";
            }

            string filename = Path.Combine(uploadPath, file.FileName);

            using (var fileStream = new FileStream(filename, FileMode.Create)) {
                file.CopyToAsync(fileStream);
            }
            return file.FileName;

        }

    }
}
