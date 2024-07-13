using Microsoft.AspNetCore.Mvc;
using SharpCompress.Common;
using System;
using System.IO;
using System.IO.Compression;

namespace OCRSimple.Utility
{
    public class UnZip
    {
        public UnZip()
        {
        }

        public bool UnZipPDF(string filepath)
        {
            bool rtvalue = false;
            string directoryPath = Path.GetDirectoryName(filepath);
            string filename = Path.GetFileName(filepath);


            // 确保解压目录存在
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }


            // 使用ZipArchive类来打开ZIP文件
            using (FileStream zipFileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
            {
                // 解压缩文件到与ZIP文件同级的目录
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // 构造解压文件的完整路径
                    string completeFilePath = Path.Combine(directoryPath, entry.FullName);

                    // 确保路径中的目录存在
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFilePath));

                    // 解压缩文件
                    entry.ExtractToFile(completeFilePath, true);
                }
                rtvalue = true;
            }


            //删除文件

            //Console.WriteLine("解压完成！");
            return rtvalue;
        }

        public bool DeleteUnZipedPDF(string filepath)
        {
            bool rtvalue = false;

            try
            {
                // 创建FileInfo对象
                FileInfo fileInfo = new FileInfo(filepath);

                // 检查文件是否存在
                if (fileInfo.Exists)
                {
                    // 删除文件
                    fileInfo.Delete();
                    //Console.WriteLine("文件删除成功！");
                    rtvalue = true;
                }
                else
                {
                    //Console.WriteLine("文件不存在，无法删除。");
                }
            }
            catch (Exception ex)
            {
                // 处理可能发生的异常
                Console.WriteLine("删除文件时发生错误：" + ex.Message);
            }


            return rtvalue;
        }
    }

}