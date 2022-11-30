using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace OCR_Fusion.Controllers {


    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase {


        private IWebHostEnvironment Environment;
        private string uploadPath;

        public OcrController(IWebHostEnvironment _environment) {
            Environment = _environment;

            //uploadPath = Path.Combine(Environment.WebRootPath, "Uploads/");
            uploadPath = "Uploads/";

        }


        [HttpGet(Name = "GetText")]
        public OutputDefinition Get(InputDefinition input) {

            

            return new OutputDefinition();
        }


        [HttpPost]
        public string PostImage(IFormFile file) {

            if (!Directory.Exists(uploadPath)) {
                Directory.CreateDirectory(uploadPath);
            }

            if (file.Length > 0) {

                string filename = Path.Combine(uploadPath, file.FileName);

                using (var fileStream = new FileStream(filename, FileMode.Create)) {
                    file.CopyToAsync(fileStream);
                }
                return filename;
            }

            return "0";
        }
    }
}
