using Microsoft.AspNetCore.Mvc;

namespace OCRSimple.Controllers
{
    public class PaddleController
    {
        [ApiController]
        [Route("[controller]")]
        public class ReadPDFController : ControllerBase
        {

            private readonly ILogger<ReadPDFController> _logger;

            public ReadPDFController(ILogger<ReadPDFController> logger)
            {
                _logger = logger;
            }


            //[HttpPost(Name = "PostReadPDF")]
            //public Task<IActionResult> PaddleOCR(string filepath, string filetype)
            //{
            //    ret;

            //}
        }
    }
}
