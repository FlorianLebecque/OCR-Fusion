using Microsoft.AspNetCore.Mvc;


namespace OCR_Fusion.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        [HttpGet(Name = "{filename}")]
        public IActionResult GetImg(string filename) {

            return File(System.IO.File.ReadAllBytes("Uploads/" + filename), "image/jpeg");
        }
    }
}