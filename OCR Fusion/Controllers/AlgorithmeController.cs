using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR_Fusion.API_Object;

namespace OCR_Fusion.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AlgorithmeController : ControllerBase {

        [HttpGet]
        public List<Algorithme> Index() {

            return Multiplex.GetAlgos();
        }
    }
}
