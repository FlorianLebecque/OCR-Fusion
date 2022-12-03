using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace OCR_Fusion.Controllers {


    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase {
        private IConfiguration configuration;
        public OcrController(IConfiguration configuration) {
            this.configuration = configuration;
        }

        [HttpGet]
        public string test() {
            return "hello world";
        }


        [HttpPost]
        public OutputDefinition Get(InputDefinition input) {

            OCRController ocrController = new OCRController(Multiplex.GetOCR(input,configuration));

            return ocrController.GetText(input);
        }

        [HttpPut]
        public string PostImage(IFormFile file) {

            return OCRController.UploadImage(file);

        }
    }
}
