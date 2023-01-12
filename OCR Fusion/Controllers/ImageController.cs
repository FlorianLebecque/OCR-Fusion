using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;


namespace OCR_Fusion.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {


        [HttpGet("{filename}")]
        public IActionResult Get(string filename) {

            filename = "Uploads/" + filename;

            if (!System.IO.File.Exists(filename)) {
                return NotFound();
            }

            return File(System.IO.File.ReadAllBytes(filename), "image/"+Utils.GetExtention(filename));
        }
    }
}