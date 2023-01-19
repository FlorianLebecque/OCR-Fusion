using Microsoft.AspNetCore.Mvc;
using APIObject;

namespace OCR_Fusion.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AlgorithmsController : ControllerBase {

        [HttpGet]
        public List<Algorithms> Get() {

            return Multiplex.GetAlgos();
        }
    }
}
