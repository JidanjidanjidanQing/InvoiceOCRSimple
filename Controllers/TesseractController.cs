using Microsoft.AspNetCore.Mvc;
using Tesseract;
using OCRSimple.Utility;
using System.Text.RegularExpressions;
using System.Text.Json;


namespace OCRSimple.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesseractController : ControllerBase
    {

        private readonly ILogger<TesseractController> _logger;

        public TesseractController(ILogger<TesseractController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PostReadImage")]

        public async Task<IActionResult> PostReadImageTesseract(string filepath)
        {
            //调用图片裁剪方法
            string datetime = ImageCut.CutImage(filepath);

            string name = "0";
            string tax_code = "0";
            string invoiceCode;
            string invoiceDate;
            string invoiceValue;

            var tess = new TesseractEngine(@"O:/software/tesseract-ocr/tessdata", "chi_sim", EngineMode.Default);//构建对象并加载训练好的数据
            //tess.DefaultPageSegMode = PageSegMode.SingleLine;//设为单行识别
            {//识别区域1：发票号码、开票日期
                var page = tess.Process(Pix.LoadFromFile("cutimage/" + datetime + "area1.png"));//处理图片
                var text = page.GetText();
                page.Dispose();

                text = Regex.Replace(text, @"\s", "");
                text = Regex.Replace(text, @"[a-zA-Z]", "");
                Console.WriteLine(text);

                int startIndex = text.IndexOf("发票号码");
                if (startIndex < 0)
                {
                    invoiceCode = "0";
                }
                else
                {
                    invoiceCode = InvoiceCode.readInvoiceCode(text.Substring(startIndex, text.Length - 1 - startIndex));
                }

                startIndex = text.IndexOf("开票日期");
                if (startIndex < 0)
                {
                    invoiceDate = "0";
                }
                else 
                {
                    invoiceDate = InvoiceDate.readInvoiceDate(text.Substring(startIndex, text.Length - 1 - startIndex));
                }

                Console.WriteLine("发票号=" + invoiceCode);
                Console.WriteLine("开票日期=" + invoiceDate);

            }
            {//识别区域2：名称、纳税人识别号
                var page = tess.Process(Pix.LoadFromFile("cutimage/" + datetime + "area2.png"));//处理图片
                var text = page.GetText();
                page.Dispose();

                text = Regex.Replace(text, @"\s", "");
                text = Regex.Replace(text, @"[a-zA-Z]", "");
                Console.WriteLine(text);

                name = "0";
                int startIndex = text.IndexOf("瑞阳制药股份有限公司");
                if (startIndex > -1)
                {
                    name = "1";
                }
                Console.WriteLine("name=" + name);


                tax_code = "0";
                startIndex = text.IndexOf("913703001686121827");
                if (startIndex > -1)
                {
                    tax_code = "1";
                }
                Console.WriteLine("tax_code=" + tax_code);
            }
            {//识别区域3：金额
                var page = tess.Process(Pix.LoadFromFile("cutimage/" + datetime + "area3.png"));//处理图片 
                Console.WriteLine(page.GetText());
                invoiceValue = InvoiceValue.readInvoiceValue(page.GetText());
                page.Dispose();

                Console.WriteLine("金额=" + invoiceValue);//输出识别内容
            }

            var invoicedata = new
            {
                name = name,
                tax_code = tax_code,
                invoiceNumber = invoiceCode,
                invoiceDate = invoiceDate,
                smallAmount = invoiceValue
            };

            //序列化
            String json = JsonSerializer.Serialize(invoicedata);

            //释放Tesseract引擎
            tess.Dispose();

            //删除临时裁剪图片
            await DeleteCuttedPic.DeleteImageAsync("cutimage/" + datetime + "area1.png");
            await DeleteCuttedPic.DeleteImageAsync("cutimage/" + datetime + "area2.png");
            await DeleteCuttedPic.DeleteImageAsync("cutimage/" + datetime + "area3.png");

            return Ok(json);
        }
    }
}