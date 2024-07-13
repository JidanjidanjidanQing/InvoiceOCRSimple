using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OCRSimple.Controllers;
using OpenCvSharp;
using System;

namespace OCRSimple.Utility
{
    public class ImageCut
    {

        public static string CutImage(string path)
        {
            DateTime now = DateTime.Now;

            string datetime = now.ToString("yyyy-MM-dd-") + now.ToString("HH-mm-ss");

            // 载入图片
            Mat image = Cv2.ImRead(path);


            //获取图片宽度和高度
            int width = image.Cols;
            int height = image.Rows;

            // 检查图片是否被成功加载
            if (image.Empty())
            {
                Console.WriteLine(path + "图片加载失败。");
                return null;
            }

            // 指定裁剪区域1
            // 参数分别为：起始点的x坐标，起始点的y坐标，裁剪区域的宽度，裁剪区域的高度
            Rect cropArea = new Rect((int)(width * 0.655), 0, width - (int)(width * 0.655), (int)(height * 0.16));
            // 裁剪图片
            Mat croppedImage1 = new Mat(image, cropArea);
            // 显示裁剪后的图片
            //Cv2.ImShow("Cropped Image", croppedImage);
            // 保存裁剪后的图片
            Cv2.ImWrite("cutimage/" + datetime + "area1.png", croppedImage1);

            // 指定裁剪区域2
            // 参数分别为：起点的x坐标，起始点的y坐标，裁剪区域的宽度，裁剪区域的高度
            cropArea = new Rect(0, (int)(height * 0.206), (int)(width * 0.496), (int)(height * 0.369) - (int)(height * 0.206));
            // 裁剪图片
            Mat croppedImage2 = new Mat(image, cropArea);
            // 显示裁剪后的图片
            //Cv2.ImShow("Cropped Image", croppedImage);
            // 保存裁剪后的图片
            Cv2.ImWrite("cutimage/" + datetime + "area2.png", croppedImage2);


            // 指定裁剪区域3
            // 参数分别为：起始点的x坐标，起始点的y坐标，裁剪区域的宽度，裁剪区域的高度
            cropArea = new Rect((int)(width * 0.694), (int)(height * 0.687), width - (int)(width * 0.694), (int)(height * 0.733) - (int)(height * 0.679));
            // 裁剪图片
            Mat croppedImage3 = new Mat(image, cropArea);
            // 显示裁剪后的图片
            //Cv2.ImShow("Cropped Image", croppedImage);
            // 保存裁剪后的图片
            Cv2.ImWrite("cutimage/" + datetime + "area3.png", croppedImage3);




            // 创建一个Mat数组，用于存储要合并的图片
            Mat[] images = { croppedImage1, croppedImage2, croppedImage3 };

            //Mat concatenatedImage = new Mat();
            //Cv2.ImreadVconcat(images, concatenatedImage);



            // 释放资源
            croppedImage1.Dispose();
            croppedImage2.Dispose();
            croppedImage3.Dispose();
            image.Dispose();

            return datetime;

        }


    }
}
