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

        [HttpPatch]
        public void Patch(OutputDefinition output) {
            Utils.Update<OutputDefinition>("outputs", output);
        }


        [HttpGet]
        public List<OutputDefinition> Get(string session) {

            List<OutputDefinition> result = Utils.Gets<OutputDefinition>("outputs", session);

            return result.FindAll(x => x.session == session);
        }

        [HttpPost]
        public OutputDefinition Post(InputDefinition input) {

            OCRController ocrController = new OCRController(Multiplex.GetOCR(input,configuration));
            return ocrController.GetText(input);
        }

        [HttpPut]
        public string Put(IFormFile file) {

            return OCRController.UploadImage(file);

        }
    }
}
