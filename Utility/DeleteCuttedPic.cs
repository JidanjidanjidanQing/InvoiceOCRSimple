using System;
using System.IO;
using System.Threading.Tasks;

public class DeleteCuttedPic
{
    // 异步方法，用于删除图片文件
    public static async Task DeleteImageAsync(string imagePath)
    {
        await Task.Run(() =>
        {
            try
            {
                // 检查文件是否存在
                if (File.Exists(imagePath))
                {
                    // 删除文件
                    File.Delete(imagePath);
                }
                else
                {
                    Console.WriteLine($"图片文件 {imagePath} 不存在。");
                }
            }
            catch (Exception ex)
            {
                // 处理异常，例如记录日志或通知用户
                Console.WriteLine($"删除图片 {imagePath} 时发生错误: {ex.Message}");
            }
        });
    }
}