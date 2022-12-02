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
        public OutputDefinition Get(InputDefinition input) {

            OCRController ocrController = new OCRController(Multiplex.GetOCR(input,configuration));

            return ocrController.GetText(input);
        }

        [HttpPost]
        public string PostImage(IFormFile file) {

            return OCRController.UploadImage(file);

        }
    }
}
