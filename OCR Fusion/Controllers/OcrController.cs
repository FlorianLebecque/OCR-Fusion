using Microsoft.AspNetCore.Mvc;

namespace OCR_Fusion.Controllers {


    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase {

        [HttpGet(Name = "GetText")]
        public OutputDefinition Get(InputDefinition input) {

                

            return new OutputDefinition();
        }
    }
}
