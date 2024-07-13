using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using OCRSimple.Utility;

namespace OCRSimple.Controllers
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


        [HttpPost(Name = "PostReadPDF")]
        public async Task<IActionResult> PostReadPDF(string filepath, string filetype)
        {
            //解压文件到同级目录
            UnZip unZip = new UnZip();

            if (unZip.UnZipPDF(filepath) == false)
            {
                return BadRequest();
            }

            //更新文件名及路径
            filepath = Regex.Replace(filepath, @".zip", "");


            if (filetype.ToLower() != "pdf")
            {
                var OCRimg = new TesseractController(null); // 传入ILogger<TesseractController>的null对象或使用Mock
                var result = await OCRimg.PostReadImageTesseract(filepath);

                //删除原始文件
                FileInfo fileInfo = new FileInfo(filepath);
                fileInfo.Delete();

                return result;
            }



            //读取PDF文件内容
            string text = string.Empty;

            string pdffilename = filepath;
            StringBuilder buffer = new StringBuilder();

            //PdfDocument变量
            using (Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument())
            {
                //加载pdf文件
                doc.LoadFromFile(pdffilename);

                foreach (Spire.Pdf.PdfPageBase page in doc.Pages)
                {
                    buffer.Append(page.ExtractText());
                }
                doc.Close();
            }

            //保存识别内容
            text = buffer.ToString();

            Console.WriteLine(text.Length);
            //如果文本长度很小，则这个PDF有可能是图片强转的，需要转换为图片进行识别
            if (text.Length <= 100)
            {
                //pdf转图片
                Console.WriteLine("--------------------pdf转图片:");
                PDF2PNG.Convert(filepath);

                Console.WriteLine("--------------------" + filepath);

                var OCRimg = new TesseractController(null); // 传入ILogger<TesseractController>的null对象或使用Mock
                var result = await OCRimg.PostReadImageTesseract(filepath);

                //删除原始文件
                FileInfo fileInfo = new FileInfo(filepath);
                fileInfo.Delete();

                return result;
            }

            //去除空格
            text = Regex.Replace(text, @"\s", "");
            //去除英文字母
            text = Regex.Replace(text, @"[a-zA-Z]", "");
            //去除购|买|方|销|售
            text = Regex.Replace(text, @"购|买|方|销|售", "");


            ////-----------------------------------------
            //匹配名称
            string name = "0";
            int startIndex = text.IndexOf("瑞阳制药股份有限公司");
            if (startIndex > -1)
                name = "1";
            Console.WriteLine("name=" + name);


            //匹配纳税人识别号
            string tax_code = "0";
            startIndex = text.IndexOf("913703001686121827");
            if (startIndex > -1)
                tax_code = "1";
            Console.WriteLine("tax_code=" + tax_code);


            // 匹配发票号码
            
            startIndex = text.IndexOf("发票号码");
            string invoiceCode = "";
            //startIndex = 0;
            if (startIndex > -1)
            {
                invoiceCode = InvoiceCode.readInvoiceCode(text.Substring(startIndex, 30));
            }
            Console.WriteLine("invoiceCode = " + invoiceCode);
            Console.WriteLine("invoiceCodePos = " + startIndex);

            // 匹配开票日期
            startIndex = 0;
            string invoiceDate = "";
            startIndex = text.IndexOf("开票日期");
            if (startIndex > -1)
            {
                invoiceDate = InvoiceDate.readInvoiceDate(text.Substring(startIndex, 20));
            }
            Console.WriteLine("invoiceDate = " + invoiceDate);


            // 匹配小写金额
            // 查找“小写”的位置
            startIndex = 0;
            string smallAmount = "";
            startIndex = text.IndexOf("小写");
            if (startIndex > -1)
            {
                smallAmount = InvoiceValue.readInvoiceValue(
                    text.Substring(
                        startIndex,
                        startIndex + 20 > text.Length - 1
                            ? text.Length - 1 - startIndex : 20));
            }
            Console.WriteLine(smallAmount);


            //如果PDF读取失败，转为OCR识别
            if (name == "0" || tax_code == "0" || invoiceCode == "" || invoiceDate == "" ||  smallAmount == "")
            {
                //pdf转图片
                Console.WriteLine("--------------------pdf转图片:");
                PDF2PNG.Convert(filepath);

                Console.WriteLine("--------------------" + filepath);

                var OCRimg = new TesseractController(null); // 传入ILogger<TesseractController>的null对象或使用Mock
                var result = await OCRimg.PostReadImageTesseract(filepath);

                //删除原始文件
                FileInfo fileInfo = new FileInfo(filepath);
                fileInfo.Delete();

                return result;
            }




            var invoicedata = new
            {
                name = name,
                tax_code = tax_code,
                invoiceNumber = invoiceCode,
                invoiceDate = invoiceDate,
                smallAmount = smallAmount
            };

            String json = JsonSerializer.Serialize(invoicedata);

            unZip.DeleteUnZipedPDF(filepath);

            return Ok(json);
        }
    }
}
