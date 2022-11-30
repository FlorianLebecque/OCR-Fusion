using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace OCR_Fusion.Controllers {


    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase {

        private string uploadPath;
        private string[] allowedExtention { get {
                string[] ae = { "png", "jepg", "jpg" };
                return ae;
            } 
        }

        public OcrController() {

            uploadPath = "Uploads/";

        }


        [HttpGet(Name = "GetText")]
        public OutputDefinition Get(InputDefinition input) {

            OCRController ocrController = new OCRController(Multiplex.GetOCR(input.IsHandWritten));

            if (!System.IO.File.Exists(uploadPath + input.imageLink)) {
                throw new Exception("File not found");
            }


            return ocrController.GetText(input);
        }


        [HttpPost]
        public string PostImage(IFormFile file) {

            if (!Directory.Exists(uploadPath)) {
                Directory.CreateDirectory(uploadPath);
            }

            if(file.Length == 0) {
                return "0";
            }

            string extention = Utils.GetExtention(file.FileName);
            if(!allowedExtention.Contains(extention)){
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
