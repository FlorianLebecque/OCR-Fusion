using Microsoft.AspNetCore.Mvc;


namespace OCR_Fusion.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {
        [HttpGet(Name = "{filename}")]
        public IActionResult GetImg(string filename) {

            filename = "Uploads/" + filename;

            if (!System.IO.File.Exists(filename)) {
                throw new Exception("File not found");
            }

            return File(System.IO.File.ReadAllBytes(filename), "image/"+Utils.GetExtention(filename));
        }
    }
}