using Microsoft.Extensions.Options;
using OCRSimple.Models;

namespace OCRSimple.Utils
{
    /// <summary>
    /// 文件上传工具类
    /// </summary>
    public class FileUtil {


        //文件上传
        public static string UploadFile(string directoryName,IFormFile file) 
        {

            string baseDirectory = AppContext.BaseDirectory + "image\\"+directoryName+"\\";
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
            string extension = Path.GetExtension(file.FileName);

            //string fileName = DateTime.Now.ToString("yyyyMMdd") + directoryName + Guid.NewGuid().ToString().Substring(0, 8) +extension;
            string fileName = "result" + extension;

            string filePath = baseDirectory + fileName;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    file.CopyTo(fs);
                    byte[] buffer = new byte[fs.Length];      //把要写入的东西转换成byte数组
                    fs.Write(buffer, 0, buffer.Length);          //写入
                    fs.Flush();
                }
                if (File.Exists(filePath))
                {
                    return filePath;
                }
                else
                {
                    return "not exist";
                }
            }
            catch (Exception e)
            {
                return "not exist";
            }
        }
    }
}
