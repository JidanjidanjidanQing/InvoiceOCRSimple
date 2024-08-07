using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using OCRSimple.Models;
using OCRSimple.Utils;



namespace OCRSimple.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OcrImgController : ControllerBase
    {

        private readonly ILogger<OcrImgController> _logger;

        public OcrImgController(ILogger<OcrImgController> logger)
        {
            _logger = logger;
        }


        [HttpPost(Name = "PostReadImagePaddle")]

        public async Task<string> PostReadImagePaddle([FromForm] Ocr ocr)
        {
            var strResult = string.Empty;

            string filepath = FileUtil.UploadFile("test",ocr.file);

            if (filepath == "not exist")
            {
                return await Task.FromResult("file not exist");
            }    

            FullOcrModel model = LocalFullModels.ChineseV3;

            using (PaddleOcrAll all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                AllowRotateDetection = true, /* 允许识别有角度的文字 */
                Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
            })

            {
                using (Mat src = Cv2.ImRead(filepath))
                {
                    PaddleOcrResult result = all.Run(src);
                    Console.WriteLine("Detected all texts: \n" + result.Text);
                    strResult = result.Text;
                    foreach (PaddleOcrResultRegion region in result.Regions)
                    {
                        Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize:    {region.Rect.Size}, Angle: {region.Rect.Angle}");
                    }
                }
            }

            return await Task.FromResult(strResult);
            //return strResult;
        }
    }
}
