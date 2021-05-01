using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Convience.Util.Helpers
{
    public static class CaptchaHelper
    {
        private static char[] _characters = { '0', '2', '3', '4', '5', '6', '7', '8', '9',
            'a','b','c','d','e','f','j','h','i','g','k','m','n','o','p','q','r','s','t','u','v',
            'w','x','y','z'};

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns>随机字符串</returns>
        public static string GetValidateCode(int length = 4)
        {
            // 产生的随机字符串
            var result = string.Empty;

            // 字符集 todo配置到文件
            char[] chars = _characters;

            // 生成字节数组,利用BitConvert方法把字节数组转换为整数
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var iRoot = BitConverter.ToInt32(buffer, 0);
            var random = new Random(iRoot);
            for (int i = 0; i < length; i++)
            {
                var index = random.Next(0, chars.Length);
                result += chars[index];
            }
            return result;
        }


        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="randomCode">随机字符串</param>
        /// <returns>图片流</returns>
        public static string CreateBase64Image(string randomCode)
        {
            // 生成的字符串长度
            var codeCharCount = randomCode.Length;

            // 需要在nuget引入System.Drawing.Common包
            var image = new Bitmap(codeCharCount * 30, 30);
            var graph = Graphics.FromImage(image);

            // 字体颜色和背景颜色合集
            Color[] fontColors = { Color.White };
            Color[] backgroundColors = {
                Color.FromArgb(245, 34, 45),
                Color.FromArgb(250,84,28),
                Color.FromArgb(250,140,22),
                Color.FromArgb(250,173,20),
                Color.FromArgb(212,177,6),
                Color.FromArgb(124,179,5),
                Color.FromArgb(82,196,26),
                Color.FromArgb(19,194,194),
                Color.FromArgb(24,144,255),
                Color.FromArgb(124,179,5),
                Color.FromArgb(47,84,235),
                Color.FromArgb(114,46,209),
                Color.FromArgb(235,47,150),
            };
            string[] fontFamilies = { "Times New Roman" };

            Random random = new Random();

            // 绘制背景色
            graph.Clear(backgroundColors[random.Next(0, backgroundColors.Length)]);

            // 绘制文字
            for (int i = 0; i < codeCharCount; i++)
            {
                // 生成随机颜色和随机字体
                var fontColor = fontColors[random.Next(0, fontColors.Length)];
                var font = new Font(fontFamilies[random.Next(0, fontFamilies.Length)], random.Next(18, 22));

                // 生成随机角度
                int x = 30 * i + random.Next(0, 15);
                int y = random.Next(0, 10);

                // 图片所占空间
                var sf = graph.MeasureString(randomCode[i].ToString(), font);
                var angle = random.Next(-30, 30);

                // 以文字中心点进行旋转画板的角度
                Matrix matrix = graph.Transform;
                matrix.RotateAt(angle, new PointF(30 * i + 15, 15));
                graph.Transform = matrix;

                // 绘制
                graph.DrawString(randomCode[i].ToString(), font, new SolidBrush(fontColor),
                    new PointF(30 * i + 15 - sf.Width / 2, 15 - sf.Height / 2));

                // 恢复画板角度
                matrix.RotateAt(-angle, new PointF(30 * i + 15, 15));
                graph.Transform = matrix;
            }

            // 绘制混淆内容
            for (int i = 0; i < 2; i++)
            {
                // 线段随机颜色和宽度
                var fontColor = fontColors[random.Next(0, fontColors.Length)];
                var width = random.Next(1, 3);
                var pen = new Pen(fontColor, width);

                // 线段随机起点和终点
                var p1 = new Point(random.Next(0, codeCharCount * 30), random.Next(0, 30));
                var p2 = new Point(random.Next(0, codeCharCount * 30), random.Next(0, 30));
                graph.DrawLine(pen, p1, p2);
            }

            // 保存文件到流中
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}
