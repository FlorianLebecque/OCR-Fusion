using Microsoft.AspNetCore.Mvc;
using APIObject;

namespace OCR_Fusion.Controllers {


    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase {
        private IConfiguration configuration;
        public OcrController(IConfiguration configuration) {
            this.configuration = configuration;
        }

        [HttpPatch]
        public void Patch(UpdateDefinition input) {

            Guid target_id = new Guid(input.Id);
            List<OutputDefinition> result = Utils.Gets<OutputDefinition>("outputs", "");
            OutputDefinition upDate = result.Find(x => x.Id == target_id);

            upDate.words = new List<string> { input.Words };

            //OutputDefinition upDate = new OutputDefinition(new Guid(input.Id),input.Words);

            Utils.Update<OutputDefinition>("outputs",upDate.Id ,upDate);
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
