using QRCoder;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Convience.Util.Helpers
{
    public static class QRCoderHelper
    {
        public static string GetPTQRCode(string url, int pixel, string logoPath = null)
        {
            QRCodeGenerator generator = new QRCodeGenerator();

            // QRCodeGenerator.ECCLevel:纠错能力
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            QRCode qrcode = new QRCode(codeData);
            Bitmap logoIcon = logoPath == null ? null : new Bitmap(logoPath);

            // pixelsPerModule  // 生成二维码图片的像素大小
            // darkColor        // 暗色   一般设置为Color.Black 
            // lightColor       // 亮色   一般设置为Color.White
            // icon             // 二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
            // iconSizePercent  // 水印图标的大小比例 ，可根据自己的喜好设置
            // iconBorderWidth  // 水印图标的边框
            // drawQuietZones   // 静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
            Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, icon: logoIcon, drawQuietZones: true);

            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Jpeg);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
