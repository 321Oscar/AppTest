using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AppTest.UI
{
    public static class ImageEx
    {
        /// <summary>
        /// 获取打开文件对话框所有的图片类型过滤条件
        /// ------
        /// All Images|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.TIF;*.TIFF;*.PNG|
        /// BMP Files: (*.BMP;*.DIB;*.RLE)|*.BMP;*.DIB;*.RLE|
        /// JPEG Files: (*.JPG;*.JPEG;*.JPE;*.JFIF)|*.JPG;*.JPEG;*.JPE;*.JFIF|
        /// GIF Files: (*.GIF)|*.GIF|
        /// TIFF Files: (*.TIF;*.TIFF)|*.TIF;*.TIFF|
        /// PNG Files: (*.PNG)|*.PNG|
        /// All Files|*.*
        /// ------
        /// </summary>
        /// <returns></returns>
        public static string GetImageFilter()
        {
            StringBuilder allImageExtensions = new StringBuilder();
            string separator = "";
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            Dictionary<string, string> images = new Dictionary<string, string>();
            foreach (ImageCodecInfo codec in codecs)
            {
                allImageExtensions.Append(separator);
                allImageExtensions.Append(codec.FilenameExtension);
                separator = ";";
                images.Add(string.Format("{0} Files: ({1})", codec.FormatDescription, codec.FilenameExtension), codec.FilenameExtension);
            }

            StringBuilder sb = new StringBuilder();
            if (allImageExtensions.Length > 0)
            {
                sb.AppendFormat("{0}|{1}", "All Images", allImageExtensions.ToString());
            }

            images.Add("All Files", "*.*");
            foreach (KeyValuePair<string, string> image in images)
            {
                sb.AppendFormat("|{0}|{1}", image.Key, image.Value);
            }

            return sb.ToString();
        }

        public static Image FromFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    return System.Drawing.Image.FromStream(new MemoryStream(bytes));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static Bitmap ChangeOpacity(Image img, float opacity)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = bmp.Graphics();
            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = opacity;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        public static ImageList GetToolbarImageList(Type type, Image bitmap, Size imageSize, Color transparentColor)
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = imageSize;
            imageList.TransparentColor = transparentColor;
            imageList.Images.AddStrip(bitmap);
            imageList.ColorDepth = ColorDepth.Depth24Bit;
            return imageList;
        }

        //public static Bitmap Split(this Image image, int size, UIShape shape)
        //{
        //    //截图画板
        //    Bitmap result = new Bitmap(size, size);
        //    Graphics g = System.Drawing.Graphics.FromImage(result);
        //    //创建截图路径（类似Ps里的路径）
        //    GraphicsPath path = new GraphicsPath();

        //    if (shape == UIShape.Circle)
        //    {
        //        path.AddEllipse(0, 0, size, size);//圆形
        //    }

        //    if (shape == UIShape.Square)
        //    {
        //        path.Dispose();
        //        path = new Rectangle(0, 0, size, size).CreateRoundedRectanglePath(5);//圆形
        //    }

        //    g.SetHighQuality();
        //    //设置画板的截图路径
        //    g.SetClip(path);
        //    //对图片进行截图
        //    g.DrawImage(image, 0, 0);
        //    //保存截好的图
        //    g.Dispose();
        //    path.Dispose();

        //    return result;
        //}

        public static Bitmap Split(this Image image, GraphicsPath path)
        {
            //截图画板
            Bitmap result = new Bitmap(image.Width, image.Height);
            Graphics g = System.Drawing.Graphics.FromImage(result);
            g.SetHighQuality();
            //设置画板的截图路径
            g.SetClip(path);
            //对图片进行截图
            g.DrawImage(image, 0, 0);
            //保存截好的图
            g.Dispose();
            path.Dispose();

            return result;
        }

        public static Graphics Graphics(this Image image)
        {
            return System.Drawing.Graphics.FromImage(image);
        }

        /// <summary>
        /// 图像水平翻转
        /// </summary>
        /// <param name="image">原来图像</param>
        /// <returns></returns>
        public static Bitmap HorizontalFlip(this Bitmap image)
        {
            try
            {
                var width = image.Width;
                var height = image.Height;
                Graphics g = System.Drawing.Graphics.FromImage(image);
                Rectangle rect = new Rectangle(0, 0, width, height);
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                g.DrawImage(image, rect);
                return image;
            }
            catch (Exception)
            {
                return image;
            }
        }

        /// <summary>
        /// 图像垂直翻转
        /// </summary>
        /// <param name="image">原来图像</param>
        /// <returns></returns>
        public static Bitmap VerticalFlip(this Bitmap image)
        {
            try
            {
                var width = image.Width;
                var height = image.Height;
                Graphics g = System.Drawing.Graphics.FromImage(image);
                Rectangle rect = new Rectangle(0, 0, width, height);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                g.DrawImage(image, rect);
                return image;
            }
            catch (Exception)
            {
                return image;
            }
        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="bmp">图片</param>
        /// <param name="angle">角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>图片</returns>
        public static Bitmap Rotate(this Image bmp, float angle, Color bkColor)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            PixelFormat pf = bkColor == Color.Transparent ? PixelFormat.Format32bppArgb : bmp.PixelFormat;

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = System.Drawing.Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 0, 0);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix matrix = new Matrix();
            matrix.Rotate(angle);
            RectangleF rct = path.GetBounds(matrix);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = System.Drawing.Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        /// <summary>
        /// 缩放图像
        /// </summary>
        /// <param name="bmp">原图片</param>
        /// <param name="newW">宽度</param>
        /// <param name="newH">高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ResizeImage(this Image bmp, int newW, int newH)
        {
            if (bmp == null)
            {
                return null;
            }

            Bitmap b = new Bitmap(newW, newH);
            using (Graphics g = System.Drawing.Graphics.FromImage(b))
            {
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            }

            return b;
        }

        /// <summary>
        /// Serializes the image in an byte array
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <param name="format">Specifies the format of the image.</param>
        /// <returns>The image serialized as byte array.</returns>
        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, format ?? image.RawFormat);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Converts to image.
        /// </summary>
        /// <param name="bytes">The byte array in.</param>
        /// <returns>结果</returns>
        public static Image ToImage(this byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Gets the bounds of the image in pixels
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <returns>A rectangle that has the same height and width as given image.</returns>
        public static Rectangle Bounds(this Image image)
        {
            return new Rectangle(0, 0, image.Width, image.Height);
        }

        /// <summary>
        /// Gets the rectangle that surrounds the given point by a specified distance.
        /// </summary>
        /// <param name="p">Instance value.</param>
        /// <param name="distance">Distance that will be used to surround the point.</param>
        /// <returns>Rectangle that surrounds the given point by a specified distance.</returns>
        public static Rectangle Surround(this Point p, int distance)
        {
            return new Rectangle(p.X - distance, p.Y - distance, distance * 2, distance * 2);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "size">The target size.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSize(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "width">The target width.</param>
        /// <param name = "height">The target height.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSize(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (Graphics g = System.Drawing.Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// 从URL获取图像
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Image</returns>
        public static Image GetImageFromUrl(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
            req.Method = "GET";
            req.UserAgent = " Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Accept = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            req.Headers.Add("X-HttpWatch-RID", " 46990-10314");
            req.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3");

            Image image = null;

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                if (stream != null)
                {
                    image = Image.FromStream(stream);
                    stream.Dispose();
                }

                resp.Close();
                return image;
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    return null;
                }

                return null;
            }
        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="angle">角度</param>
        /// <returns>图片</returns>
        public static Bitmap RotateAngle(this Image image, float angle)
        {
            if (image == null)
            {
                return null;
            }

            const double Pi2 = Math.PI / 2.0;

            double oldWidth = image.Width;
            double oldHeight = image.Height;

            double theta = angle * Math.PI / 180.0;
            double locked_theta = theta;

            while (locked_theta < 0.0)
            {
                locked_theta += 2 * Math.PI;
            }

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            if ((locked_theta >= 0.0 && locked_theta < Pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + Pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            double newWidth = adjacentTop + oppositeBottom;
            double newHeight = adjacentBottom + oppositeTop;

            int nWidth = (int)Math.Ceiling(newWidth);
            int nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = System.Drawing.Graphics.FromImage(rotatedBmp))
            {
                Point[] points;

                if (locked_theta >= 0.0 && locked_theta < Pi2)
                {
                    points = new[]
                    {
                        new Point((int) oppositeBottom, 0),
                        new Point(nWidth, (int) oppositeTop),
                        new Point(0, (int) adjacentBottom)
                    };
                }
                else if (locked_theta >= Pi2 && locked_theta < Math.PI)
                {
                    points = new[]
                    {
                        new Point(nWidth, (int) oppositeTop),
                        new Point((int) adjacentTop, nHeight),
                        new Point((int) oppositeBottom, 0)
                    };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + Pi2))
                {
                    points = new[]
                    {
                        new Point((int) adjacentTop, nHeight),
                        new Point(0, (int) adjacentBottom),
                        new Point(nWidth, (int) oppositeTop)
                    };
                }
                else
                {
                    points = new[]
                    {
                        new Point(0, (int) adjacentBottom),
                        new Point((int) oppositeBottom, 0),
                        new Point((int) adjacentTop, nHeight)
                    };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
        }

        /// <summary>
        /// 转换Image为Icon
        /// https://www.cnblogs.com/ahdung/p/ConvertToIcon.html
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public static Icon ToIcon(this Image image)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
            {
                image.Save(msImg, ImageFormat.Png);

                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);           //0-1保留
                    bin.Write((short)1);           //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);           //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)image.Width);  //6图标宽度
                    bin.Write((byte)image.Height); //7图标高度
                    bin.Write((byte)0);            //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);            //9保留。必须为0
                    bin.Write((short)0);           //10-11调色板
                    bin.Write((short)32);          //12-13位深
                    bin.Write((int)msImg.Length);  //14-17位图数据大小
                    bin.Write(22);                 //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());

                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    return new Icon(msIco);
                }
            }
        }

        public static void DrawImageFromBase64(this Graphics g, string base64Image, Rectangle rect)
        {
            using (var ms = new System.IO.MemoryStream(Convert.FromBase64String(base64Image)))
            using (var image = Image.FromStream(ms))
            {
                g.DrawImage(image, rect);
                image.Dispose();
            }
        }

        public static void DrawTransparentImage(this Graphics g, float alpha, Image image, Rectangle rect)
        {
            var colorMatrix = new ColorMatrix { Matrix33 = alpha };
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix);
            g.DrawImage(image, new Rectangle(rect.X, rect.Y, image.Width, image.Height), rect.X, rect.Y, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
            imageAttributes.Dispose();
        }

        public static void DrawStrokedRectangle(this Graphics g, Rectangle rect, Color bodyColor, Color strokeColor, int strokeThickness = 1)
        {
            using (var bodyBrush = new SolidBrush(bodyColor))
            {
                var x = strokeThickness == 1 ? 0 : strokeThickness;
                var y = strokeThickness == 1 ? 0 : strokeThickness;
                var h = strokeThickness == 1 ? 1 : strokeThickness + 1;
                var w = strokeThickness == 1 ? 1 : strokeThickness + 1;
                var newRect = new Rectangle(rect.X + x, rect.Y + y, rect.Width - w, rect.Height - h);
                using (var strokePen = new Pen(strokeColor, strokeThickness))
                {
                    g.FillRectangle(bodyBrush, newRect);
                    g.DrawRectangle(strokePen, newRect);
                }
            }
        }

        public static void DrawStrokedEllipse(this Graphics g, Rectangle rect, Color bodyColor, Color strokeColor, int strokeThickness = 1)
        {
            using (var bodyBrush = new SolidBrush(bodyColor))
            {
                var x = strokeThickness == 1 ? 0 : strokeThickness;
                var y = strokeThickness == 1 ? 0 : strokeThickness;
                var h = strokeThickness == 1 ? 1 : strokeThickness + 1;
                var w = strokeThickness == 1 ? 1 : strokeThickness + 1;
                var newRect = new Rectangle(rect.X + x, rect.Y + y, rect.Width - w, rect.Height - h);
                using (var strokePen = new Pen(strokeColor, strokeThickness))
                {
                    g.FillEllipse(bodyBrush, newRect);
                    g.DrawEllipse(strokePen, newRect);
                }
            }
        }

        public static string ToBase64(this Image image)
        {
            using (var toBase64 = new MemoryStream())
            {
                image.Save(toBase64, image.RawFormat);
                image.Dispose();
                return Convert.ToBase64String(toBase64.ToArray());
            }
        }

        public static Image ToImage(string base64Image)
        {
            using (var toImage = new System.IO.MemoryStream(Convert.FromBase64String(base64Image)))
            {
                return Image.FromStream(toImage);
            }
        }

        public static Icon SaveToIcon(this Image img, int size = 16)
        {
            byte[] pngiconheader = new byte[] { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            using (Bitmap bmp = new Bitmap(img, new Size(size, size)))
            {
                byte[] png;
                using (System.IO.MemoryStream fs = new System.IO.MemoryStream())
                {
                    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                    fs.Position = 0;
                    png = fs.ToArray();
                }

                using (System.IO.MemoryStream fs = new System.IO.MemoryStream())
                {
                    if (size >= 256) size = 0;
                    pngiconheader[6] = (byte)size;
                    pngiconheader[7] = (byte)size;
                    pngiconheader[14] = (byte)(png.Length & 255);
                    pngiconheader[15] = (byte)(png.Length / 256);
                    pngiconheader[18] = (byte)(pngiconheader.Length);

                    fs.Write(pngiconheader, 0, pngiconheader.Length);
                    fs.Write(png, 0, png.Length);
                    fs.Position = 0;
                    return new Icon(fs);
                }
            }
        }
    }

    public unsafe class FastBitmap : IDisposable
    {
        /// <summary>
        /// Specifies the number of bytes available per pixel of the bitmap object being manipulated
        /// </summary>
        public const int BytesPerPixel = 4;

        /// <summary>
        /// The Bitmap object encapsulated on this FastBitmap
        /// </summary>
        private readonly Bitmap _bitmap;

        /// <summary>
        /// The BitmapData resulted from the lock operation
        /// </summary>
        private BitmapData _bitmapData;

        /// <summary>
        /// The first pixel of the bitmap
        /// </summary>
        private int* _scan0;

        /// <summary>
        /// Gets the width of this FastBitmap object
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of this FastBitmap object
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the pointer to the first pixel of the bitmap
        /// </summary>
        public IntPtr Scan0 => _bitmapData.Scan0;

        /// <summary>
        /// Gets the stride width (in int32-sized values) of the bitmap
        /// </summary>
        public int Stride { get; private set; }

        /// <summary>
        /// Gets the stride width (in bytes) of the bitmap
        /// </summary>
        public int StrideInBytes { get; private set; }

        /// <summary>
        /// Gets a boolean value that states whether this FastBitmap is currently locked in memory
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// Gets an array of 32-bit color pixel values that represent this FastBitmap
        /// </summary>
        /// <exception cref="Exception">The locking operation required to extract the values off from the underlying bitmap failed</exception>
        /// <exception cref="InvalidOperationException">The bitmap is already locked outside this fast bitmap</exception>
        public int[] DataArray
        {
            get
            {
                bool unlockAfter = false;
                if (!Locked)
                {
                    Lock();
                    unlockAfter = true;
                }

                // Declare an array to hold the bytes of the bitmap
                int bytes = Math.Abs(_bitmapData.Stride) * _bitmap.Height;
                int[] argbValues = new int[bytes / BytesPerPixel];

                // Copy the RGB values into the array
                Marshal.Copy(_bitmapData.Scan0, argbValues, 0, bytes / BytesPerPixel);

                if (unlockAfter)
                {
                    Unlock();
                }

                return argbValues;
            }
        }

        /// <summary>
        /// Creates a new instance of the FastBitmap class with a specified Bitmap.
        /// The bitmap provided must have a 32bpp depth
        /// </summary>
        /// <param name="bitmap">The Bitmap object to encapsulate on this FastBitmap object</param>
        /// <exception cref="ArgumentException">The bitmap provided does not have a 32bpp pixel format</exception>
        public FastBitmap(Bitmap bitmap)
        {
            if (Image.GetPixelFormatSize(bitmap.PixelFormat) != 32)
            {
                throw new ArgumentException(@"The provided bitmap must have a 32bpp depth", nameof(bitmap));
            }

            _bitmap = bitmap;

            Width = bitmap.Width;
            Height = bitmap.Height;
        }

        /// <summary>
        /// Disposes of this fast bitmap object and releases any pending resources.
        /// The underlying bitmap is not disposes, and is unlocked, if currently locked
        /// </summary>
        public void Dispose()
        {
            if (Locked)
            {
                Unlock();
            }
        }

        /// <summary>
        /// Locks the bitmap to start the bitmap operations. If the bitmap is already locked,
        /// an exception is thrown
        /// </summary>
        /// <returns>A fast bitmap locked struct that will unlock the underlying bitmap after disposal</returns>
        /// <exception cref="InvalidOperationException">The bitmap is already locked</exception>
        /// <exception cref="System.Exception">The locking operation in the underlying bitmap failed</exception>
        /// <exception cref="InvalidOperationException">The bitmap is already locked outside this fast bitmap</exception>
        public FastBitmapLocker Lock()
        {
            return Lock((FastBitmapLockFormat)_bitmap.PixelFormat);
        }

        /// <summary>
        /// Locks the bitmap to start the bitmap operations. If the bitmap is already locked,
        /// an exception is thrown.
        ///
        /// The provided pixel format should be a 32bpp format.
        /// </summary>
        /// <param name="pixelFormat">A pixel format to use when locking the underlying bitmap</param>
        /// <returns>A fast bitmap locked struct that will unlock the underlying bitmap after disposal</returns>
        /// <exception cref="InvalidOperationException">The bitmap is already locked</exception>
        /// <exception cref="Exception">The locking operation in the underlying bitmap failed</exception>
        /// <exception cref="InvalidOperationException">The bitmap is already locked outside this fast bitmap</exception>
        public FastBitmapLocker Lock(FastBitmapLockFormat pixelFormat)
        {
            if (Locked)
            {
                throw new InvalidOperationException("Unlock must be called before a Lock operation");
            }

            return Lock(ImageLockMode.ReadWrite, (PixelFormat)pixelFormat);
        }

        /// <summary>
        /// Locks the bitmap to start the bitmap operations
        /// </summary>
        /// <param name="lockMode">The lock mode to use on the bitmap</param>
        /// <param name="pixelFormat">A pixel format to use when locking the underlying bitmap</param>
        /// <returns>A fast bitmap locked struct that will unlock the underlying bitmap after disposal</returns>
        /// <exception cref="System.Exception">The locking operation in the underlying bitmap failed</exception>
        /// <exception cref="InvalidOperationException">The bitmap is already locked outside this fast bitmap</exception>
        /// <exception cref="ArgumentException"><see cref="!:pixelFormat"/> is not a 32bpp format</exception>
        private FastBitmapLocker Lock(ImageLockMode lockMode, PixelFormat pixelFormat)
        {
            var rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);

            return Lock(lockMode, rect, pixelFormat);
        }

        /// <summary>
        /// Locks the bitmap to start the bitmap operations
        /// </summary>
        /// <param name="lockMode">The lock mode to use on the bitmap</param>
        /// <param name="rect">The rectangle to lock</param>
        /// <param name="pixelFormat">A pixel format to use when locking the underlying bitmap</param>
        /// <returns>A fast bitmap locked struct that will unlock the underlying bitmap after disposal</returns>
        /// <exception cref="System.ArgumentException">The provided region is invalid</exception>
        /// <exception cref="System.Exception">The locking operation in the underlying bitmap failed</exception>
        /// <exception cref="InvalidOperationException">The bitmap region is already locked</exception>
        /// <exception cref="ArgumentException"><see cref="!:pixelFormat"/> is not a 32bpp format</exception>
        private FastBitmapLocker Lock(ImageLockMode lockMode, Rectangle rect, PixelFormat pixelFormat)
        {
            // Lock the bitmap's bits
            _bitmapData = _bitmap.LockBits(rect, lockMode, pixelFormat);

            _scan0 = (int*)_bitmapData.Scan0;
            Stride = _bitmapData.Stride / BytesPerPixel;
            StrideInBytes = _bitmapData.Stride;

            Locked = true;

            return new FastBitmapLocker(this);
        }

        /// <summary>
        /// Unlocks the bitmap and applies the changes made to it. If the bitmap was not locked
        /// beforehand, an exception is thrown
        /// </summary>
        /// <exception cref="InvalidOperationException">The bitmap is already unlocked</exception>
        /// <exception cref="System.Exception">The unlocking operation in the underlying bitmap failed</exception>
        public void Unlock()
        {
            if (!Locked)
            {
                throw new InvalidOperationException("Lock must be called before an Unlock operation");
            }

            _bitmap.UnlockBits(_bitmapData);

            Locked = false;
        }

        /// <summary>
        /// Sets the pixel color at the given coordinates. If the bitmap was not locked beforehands,
        /// an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to set</param>
        /// <param name="y">The Y coordinate of the pixel to set</param>
        /// <param name="color">The new color of the pixel to set</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.ToArgb());
        }

        /// <summary>
        /// Sets the pixel color at the given coordinates. If the bitmap was not locked beforehands,
        /// an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to set</param>
        /// <param name="y">The Y coordinate of the pixel to set</param>
        /// <param name="color">The new color of the pixel to set</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public void SetPixel(int x, int y, int color)
        {
            SetPixel(x, y, unchecked((uint)color));
        }

        /// <summary>
        /// Sets the pixel color at the given coordinates. If the bitmap was not locked beforehands,
        /// an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to set</param>
        /// <param name="y">The Y coordinate of the pixel to set</param>
        /// <param name="color">The new color of the pixel to set</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public void SetPixel(int x, int y, uint color)
        {
            if (!Locked)
            {
                throw new InvalidOperationException("The FastBitmap must be locked before any pixel operations are made");
            }

            if (x < 0 || x >= Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x), @"The X component must be >= 0 and < width");
            }
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y), @"The Y component must be >= 0 and < height");
            }

            *(uint*)(_scan0 + x + y * Stride) = color;
        }

        /// <summary>
        /// Gets the pixel color at the given coordinates. If the bitmap was not locked beforehands,
        /// an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to get</param>
        /// <param name="y">The Y coordinate of the pixel to get</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public Color GetPixel(int x, int y)
        {
            return Color.FromArgb(GetPixelInt(x, y));
        }

        /// <summary>
        /// Gets the pixel color at the given coordinates as an integer value. If the bitmap
        /// was not locked beforehands, an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to get</param>
        /// <param name="y">The Y coordinate of the pixel to get</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public int GetPixelInt(int x, int y)
        {
            if (!Locked)
            {
                throw new InvalidOperationException("The FastBitmap must be locked before any pixel operations are made");
            }

            if (x < 0 || x >= Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x), @"The X component must be >= 0 and < width");
            }
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y), @"The Y component must be >= 0 and < height");
            }

            return *(_scan0 + x + y * Stride);
        }

        /// <summary>
        /// Gets the pixel color at the given coordinates as an unsigned integer value.
        /// If the bitmap was not locked beforehands, an exception is thrown
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to get</param>
        /// <param name="y">The Y coordinate of the pixel to get</param>
        /// <exception cref="InvalidOperationException">The fast bitmap is not locked</exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided coordinates are out of bounds of the bitmap</exception>
        public uint GetPixelUInt(int x, int y)
        {
            if (!Locked)
            {
                throw new InvalidOperationException("The FastBitmap must be locked before any pixel operations are made");
            }

            if (x < 0 || x >= Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x), @"The X component must be >= 0 and < width");
            }
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y), @"The Y component must be >= 0 and < height");
            }

            return *((uint*)_scan0 + x + y * Stride);
        }

        /// <summary>
        /// Copies the contents of the given array of colors into this FastBitmap.
        /// Throws an ArgumentException if the count of colors on the array mismatches the pixel count from this FastBitmap
        /// </summary>
        /// <param name="colors">The array of colors to copy</param>
        /// <param name="ignoreZeroes">Whether to ignore zeroes when copying the data</param>
        public void CopyFromArray(int[] colors, bool ignoreZeroes = false)
        {
            if (colors.Length != Width * Height)
            {
                throw new ArgumentException(@"The number of colors of the given array mismatch the pixel count of the bitmap", nameof(colors));
            }

            // Simply copy the argb values array
            // ReSharper disable once InconsistentNaming
            int* s0t = _scan0;

            fixed (int* source = colors)
            {
                // ReSharper disable once InconsistentNaming
                int* s0s = source;

                int count = Width * Height;

                if (!ignoreZeroes)
                {
                    // Unfold the loop
                    const int sizeBlock = 8;
                    int rem = count % sizeBlock;

                    count /= sizeBlock;

                    while (count-- > 0)
                    {
                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);

                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);
                        *(s0t++) = *(s0s++);
                    }

                    while (rem-- > 0)
                    {
                        *(s0t++) = *(s0s++);
                    }
                }
                else
                {
                    while (count-- > 0)
                    {
                        if (*(s0s) == 0) { s0t++; s0s++; continue; }
                        *(s0t++) = *(s0s++);
                    }
                }
            }
        }

        /// <summary>
        /// Clears the bitmap with the given color
        /// </summary>
        /// <param name="color">The color to clear the bitmap with</param>
        public void Clear(Color color)
        {
            Clear(color.ToArgb());
        }

        /// <summary>
        /// Clears the bitmap with the given color
        /// </summary>
        /// <param name="color">The color to clear the bitmap with</param>
        public void Clear(int color)
        {
            bool unlockAfter = false;
            if (!Locked)
            {
                Lock();
                unlockAfter = true;
            }

            // Clear all the pixels
            int count = Width * Height;
            int* curScan = _scan0;

            // Uniform color pixel values can be mem-set straight away
            int component = (color & 0xFF);
            if (component == ((color >> 8) & 0xFF) && component == ((color >> 16) & 0xFF) && component == ((color >> 24) & 0xFF))
            {
                memset(_scan0, component, (ulong)(Height * Stride * BytesPerPixel));
            }
            else
            {
                // Defines the ammount of assignments that the main while() loop is performing per loop.
                // The value specified here must match the number of assignment statements inside that loop
                const int assignsPerLoop = 8;

                int rem = count % assignsPerLoop;
                count /= assignsPerLoop;

                while (count-- > 0)
                {
                    *(curScan++) = color;
                    *(curScan++) = color;
                    *(curScan++) = color;
                    *(curScan++) = color;

                    *(curScan++) = color;
                    *(curScan++) = color;
                    *(curScan++) = color;
                    *(curScan++) = color;
                }
                while (rem-- > 0)
                {
                    *(curScan++) = color;
                }

                if (unlockAfter)
                {
                    Unlock();
                }
            }
        }

        /// <summary>
        /// Clears a square region of this image w/ a given color
        /// </summary>
        /// <param name="region"></param>
        /// <param name="color"></param>
        public void ClearRegion(Rectangle region, Color color)
        {
            ClearRegion(region, color.ToArgb());
        }

        /// <summary>
        /// Clears a square region of this image w/ a given color
        /// </summary>
        /// <param name="region"></param>
        /// <param name="color"></param>
        public void ClearRegion(Rectangle region, int color)
        {
            var thisReg = new Rectangle(0, 0, Width, Height);
            if (!region.IntersectsWith(thisReg))
                return;

            // If the region covers the entire image, use faster Clear().
            if (region == thisReg)
            {
                Clear(color);
                return;
            }

            int minX = region.X;
            int maxX = region.X + region.Width;

            int minY = region.Y;
            int maxY = region.Y + region.Height;

            // Bail out of optimization if there's too few rows to make this worth it
            if (maxY - minY < 16)
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int x = minX; x < maxX; x++)
                    {
                        *(_scan0 + x + y * Stride) = color;
                    }
                }
                return;
            }

            ulong strideWidth = (ulong)region.Width * BytesPerPixel;

            // Uniform color pixel values can be mem-set straight away
            int component = (color & 0xFF);
            if (component == ((color >> 8) & 0xFF) && component == ((color >> 16) & 0xFF) &&
                component == ((color >> 24) & 0xFF))
            {
                for (int y = minY; y < maxY; y++)
                {
                    memset(_scan0 + minX + y * Stride, component, strideWidth);
                }
            }
            else
            {
                // Prepare a horizontal slice of pixels that will be copied over each horizontal row down.
                var row = new int[region.Width];

                fixed (int* pRow = row)
                {
                    int count = region.Width;
                    int rem = count % 8;
                    count /= 8;
                    int* pSrc = pRow;
                    while (count-- > 0)
                    {
                        *pSrc++ = color;
                        *pSrc++ = color;
                        *pSrc++ = color;
                        *pSrc++ = color;

                        *pSrc++ = color;
                        *pSrc++ = color;
                        *pSrc++ = color;
                        *pSrc++ = color;
                    }
                    while (rem-- > 0)
                    {
                        *pSrc++ = color;
                    }

                    var sx = _scan0 + minX;
                    for (int y = minY; y < maxY; y++)
                    {
                        memcpy(sx + y * Stride, pRow, strideWidth);
                    }
                }
            }
        }

        /// <summary>
        /// Copies a region of the source bitmap into this fast bitmap
        /// </summary>
        /// <param name="source">The source image to copy</param>
        /// <param name="srcRect">The region on the source bitmap that will be copied over</param>
        /// <param name="destRect">The region on this fast bitmap that will be changed</param>
        /// <exception cref="ArgumentException">The provided source bitmap is the same bitmap locked in this FastBitmap</exception>
        public void CopyRegion(Bitmap source, Rectangle srcRect, Rectangle destRect)
        {
            // Throw exception when trying to copy same bitmap over
            if (source == _bitmap)
            {
                throw new ArgumentException(@"Copying regions across the same bitmap is not supported", nameof(source));
            }

            var srcBitmapRect = new Rectangle(0, 0, source.Width, source.Height);
            var destBitmapRect = new Rectangle(0, 0, Width, Height);

            // Check if the rectangle configuration doesn't generate invalid states or does not affect the target image
            if (srcRect.Width <= 0 || srcRect.Height <= 0 || destRect.Width <= 0 || destRect.Height <= 0 ||
                !srcBitmapRect.IntersectsWith(srcRect) || !destRect.IntersectsWith(destBitmapRect))
                return;

            // Find the areas of the first and second bitmaps that are going to be affected
            srcBitmapRect = Rectangle.Intersect(srcRect, srcBitmapRect);

            // Clip the source rectangle on top of the destination rectangle in a way that clips out the regions of the original bitmap
            // that will not be drawn on the destination bitmap for being out of bounds
            srcBitmapRect = Rectangle.Intersect(srcBitmapRect, new Rectangle(srcRect.X, srcRect.Y, destRect.Width, destRect.Height));

            destBitmapRect = Rectangle.Intersect(destRect, destBitmapRect);

            // Clip the source bitmap region yet again here
            srcBitmapRect = Rectangle.Intersect(srcBitmapRect, new Rectangle(-destRect.X + srcRect.X, -destRect.Y + srcRect.Y, Width, Height));

            // Calculate the rectangle containing the maximum possible area that is supposed to be affected by the copy region operation
            int copyWidth = Math.Min(srcBitmapRect.Width, destBitmapRect.Width);
            int copyHeight = Math.Min(srcBitmapRect.Height, destBitmapRect.Height);

            if (copyWidth == 0 || copyHeight == 0)
                return;

            int srcStartX = srcBitmapRect.Left;
            int srcStartY = srcBitmapRect.Top;

            int destStartX = destBitmapRect.Left;
            int destStartY = destBitmapRect.Top;

            using (var fastSource = source.FastLock())
            {
                ulong strideWidth = (ulong)copyWidth * BytesPerPixel;

                // Perform copies of whole pixel rows
                for (int y = 0; y < copyHeight; y++)
                {
                    int destX = destStartX;
                    int destY = destStartY + y;

                    int srcX = srcStartX;
                    int srcY = srcStartY + y;

                    long offsetSrc = (srcX + srcY * fastSource.Stride);
                    long offsetDest = (destX + destY * Stride);

                    memcpy(_scan0 + offsetDest, fastSource._scan0 + offsetSrc, strideWidth);
                }
            }
        }

        /// <summary>
        /// Performs a copy operation of the pixels from the Source bitmap to the Target bitmap.
        /// If the dimensions or pixel depths of both images don't match, the copy is not performed
        /// </summary>
        /// <param name="source">The bitmap to copy the pixels from</param>
        /// <param name="target">The bitmap to copy the pixels to</param>
        /// <returns>Whether the copy proceedure was successful</returns>
        /// <exception cref="ArgumentException">The provided source and target bitmaps are the same</exception>
        public static bool CopyPixels(Bitmap source, Bitmap target)
        {
            if (source == target)
            {
                throw new ArgumentException(@"Copying pixels across the same bitmap is not supported", nameof(source));
            }

            if (source.Width != target.Width || source.Height != target.Height || source.PixelFormat != target.PixelFormat)
                return false;

            using (FastBitmap fastSource = source.FastLock(),
                fastTarget = target.FastLock())
            {
                memcpy(fastTarget.Scan0, fastSource.Scan0, (ulong)(fastSource.Height * fastSource.Stride * BytesPerPixel));
            }

            return true;
        }

        /// <summary>
        /// Clears the given bitmap with the given color
        /// </summary>
        /// <param name="bitmap">The bitmap to clear</param>
        /// <param name="color">The color to clear the bitmap with</param>
        public static void ClearBitmap(Bitmap bitmap, Color color)
        {
            ClearBitmap(bitmap, color.ToArgb());
        }

        /// <summary>
        /// Clears the given bitmap with the given color
        /// </summary>
        /// <param name="bitmap">The bitmap to clear</param>
        /// <param name="color">The color to clear the bitmap with</param>
        public static void ClearBitmap(Bitmap bitmap, int color)
        {
            using (var fb = bitmap.FastLock())
            {
                fb.Clear(color);
            }
        }

        /// <summary>
        /// Copies a region of the source bitmap to a target bitmap
        /// </summary>
        /// <param name="source">The source image to copy</param>
        /// <param name="target">The target image to be altered</param>
        /// <param name="srcRect">The region on the source bitmap that will be copied over</param>
        /// <param name="destRect">The region on the target bitmap that will be changed</param>
        /// <exception cref="ArgumentException">The provided source and target bitmaps are the same bitmap</exception>
        public static void CopyRegion(Bitmap source, Bitmap target, Rectangle srcRect, Rectangle destRect)
        {
            var srcBitmapRect = new Rectangle(0, 0, source.Width, source.Height);
            var destBitmapRect = new Rectangle(0, 0, target.Width, target.Height);

            // If the copy operation results in an entire copy, use CopyPixels instead
            if (srcBitmapRect == srcRect && destBitmapRect == destRect && srcBitmapRect == destBitmapRect)
            {
                CopyPixels(source, target);
                return;
            }

            using (var fastTarget = target.FastLock())
            {
                fastTarget.CopyRegion(source, srcRect, destRect);
            }
        }

        /// <summary>
        /// Returns a bitmap that is a slice of the original provided 32bpp Bitmap.
        /// The region must have a width and a height > 0, and must lie inside the source bitmap's area
        /// </summary>
        /// <param name="source">The source bitmap to slice</param>
        /// <param name="region">The region of the source bitmap to slice</param>
        /// <returns>A Bitmap that represents the rectangle region slice of the source bitmap</returns>
        /// <exception cref="ArgumentException">The provided bimap is not 32bpp</exception>
        /// <exception cref="ArgumentException">The provided region is invalid</exception>
        public static Bitmap SliceBitmap(Bitmap source, Rectangle region)
        {
            if (region.Width <= 0 || region.Height <= 0)
            {
                throw new ArgumentException(@"The provided region must have a width and a height > 0", nameof(region));
            }

            var sliceRectangle = Rectangle.Intersect(new Rectangle(Point.Empty, source.Size), region);

            if (sliceRectangle.IsEmpty)
            {
                throw new ArgumentException(@"The provided region must not lie outside of the bitmap's region completely", nameof(region));
            }

            var slicedBitmap = new Bitmap(sliceRectangle.Width, sliceRectangle.Height);
            CopyRegion(source, slicedBitmap, sliceRectangle, new Rectangle(0, 0, sliceRectangle.Width, sliceRectangle.Height));

            return slicedBitmap;
        }

#if NETSTANDARD
        public static void memcpy(IntPtr dest, IntPtr src, ulong count)
        {
            Buffer.MemoryCopy(src.ToPointer(), dest.ToPointer(), count, count);
        }

        public static void memcpy(void* dest, void* src, ulong count)
        {
            Buffer.MemoryCopy(src, dest, count, count);
        }

        public static void memset(void* dest, int value, ulong count)
        {
            Unsafe.InitBlock(dest, (byte)value, (uint)count);
        }
#else
        /// <summary>
        /// .NET wrapper to native call of 'memcpy'. Requires Microsoft Visual C++ Runtime installed
        /// </summary>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, ulong count);

        /// <summary>
        /// .NET wrapper to native call of 'memcpy'. Requires Microsoft Visual C++ Runtime installed
        /// </summary>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(void* dest, void* src, ulong count);

        /// <summary>
        /// .NET wrapper to native call of 'memset'. Requires Microsoft Visual C++ Runtime installed
        /// </summary>
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memset(void* dest, int value, ulong count);
#endif

        /// <summary>
        /// Represents a disposable structure that is returned during Lock() calls, and unlocks the bitmap on Dispose calls
        /// </summary>
        public struct FastBitmapLocker : IDisposable
        {
            /// <summary>
            /// Gets the fast bitmap instance attached to this locker
            /// </summary>
            public FastBitmap FastBitmap { get; }

            /// <summary>
            /// Initializes a new instance of the FastBitmapLocker struct with an initial fast bitmap object.
            /// The fast bitmap object passed will be unlocked after calling Dispose() on this struct
            /// </summary>
            /// <param name="fastBitmap">A fast bitmap to attach to this locker which will be released after a call to Dispose</param>
            public FastBitmapLocker(FastBitmap fastBitmap)
            {
                FastBitmap = fastBitmap;
            }

            /// <summary>
            /// Disposes of this FastBitmapLocker, essentially unlocking the underlying fast bitmap
            /// </summary>
            public void Dispose()
            {
                if (FastBitmap.Locked)
                    FastBitmap.Unlock();
            }
        }
    }

    /// <summary>
    /// Describes a pixel format to use when locking a bitmap using <see cref="FastBitmap"/>.
    /// </summary>
    public enum FastBitmapLockFormat
    {
        /// <summary>Specifies that the format is 32 bits per pixel; 8 bits each are used for the red, green, and blue components. The remaining 8 bits are not used.</summary>
        Format32bppRgb = 139273,
        /// <summary>Specifies that the format is 32 bits per pixel; 8 bits each are used for the alpha, red, green, and blue components. The red, green, and blue components are premultiplied, according to the alpha component.</summary>
        Format32bppPArgb = 925707,
        /// <summary>Specifies that the format is 32 bits per pixel; 8 bits each are used for the alpha, red, green, and blue components.</summary>
        Format32bppArgb = 2498570,
    }

    /// <summary>
    /// Static class that contains fast bitmap extension methdos for the Bitmap class
    /// </summary>
    public static class FastBitmapExtensions
    {
        /// <summary>
        /// Locks this bitmap into memory and returns a FastBitmap that can be used to manipulate its pixels
        /// </summary>
        /// <param name="bitmap">The bitmap to lock</param>
        /// <returns>A locked FastBitmap</returns>
        public static FastBitmap FastLock(this Bitmap bitmap)
        {
            var fast = new FastBitmap(bitmap);
            fast.Lock();

            return fast;
        }

        /// <summary>
        /// Locks this bitmap into memory and returns a FastBitmap that can be used to manipulate its pixels
        /// </summary>
        /// <param name="bitmap">The bitmap to lock</param>
        /// <param name="lockFormat">The underlying pixel format to use when locking the bitmap</param>
        /// <returns>A locked FastBitmap</returns>
        public static FastBitmap FastLock(this Bitmap bitmap, FastBitmapLockFormat lockFormat)
        {
            var fast = new FastBitmap(bitmap);
            fast.Lock(lockFormat);

            return fast;
        }

        /// <summary>
        /// Returns a deep clone of this Bitmap object, with all the data copied over.
        /// After a deep clone, the new bitmap is completely independent from the original
        /// </summary>
        /// <param name="bitmap">The bitmap to clone</param>
        /// <returns>A deep clone of this Bitmap object, with all the data copied over</returns>
        public static Bitmap DeepClone(this Bitmap bitmap)
        {
            return bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
        }
    }
}
