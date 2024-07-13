using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace OCRSimple.Utility
{
    public class PDF2PNG
    {
        public static bool Convert(string filepath)
        {
            //创建一个PdfDocument实例
            PdfDocument pdf = new PdfDocument();

            //加载需要转换的PDF文档
            pdf.LoadFromFile(filepath);

            //加载完成后删除原始文件
            FileInfo fileInfo = new FileInfo(filepath);
            fileInfo.Delete();

            //将所有页面转换为图像并设置图像Dpi
            Image image = pdf.SaveAsImage(0, PdfImageType.Bitmap, 260, 260);

            //更新文件后缀名
            //filepath = Regex.Replace(filepath, @".pdf", ".png");

            //设置图像格式
            String file = String.Format(filepath, 0);

            image.Save(file, ImageFormat.Png);

            return true;    
        }
    }
}