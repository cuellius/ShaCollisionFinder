using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ShaCollisionFinder
{
    public static class JpegUtils
    {
        // ReSharper disable once InconsistentNaming
        private const int JPEG_SIZE_MAX = 65000;

        private static ImageCodecInfo GetEncoderInfo(string mimeType) => ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);

        private static Image Compress(this Image image, int quality)
        {
            var encoderParameters = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, quality) }
            };
            var imageCodecInfo = GetEncoderInfo("image/jpeg");
            var stream = new MemoryStream();
            image.Save(stream, imageCodecInfo, encoderParameters);
            return Image.FromStream(stream);
        }

        private static int GetCompressedImageSize(this Image image, int quality)
        {
            var encoderParameters = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, quality) }
            };
            var imageCodecInfo = GetEncoderInfo("image/jpeg");
            var stream = new MemoryStream();
            image.Save(stream, imageCodecInfo, encoderParameters);
            return (int)stream.Length;
        }

        public static Image CompressToSize(this Image image, int maxSize)
        {
            int l = 1, r = 100;
            while (r - l > 1)
            {
                var m = (r + l) >> 1;
                if (image.GetCompressedImageSize(m) <= maxSize) l = m; else r = m;
            }

            return image.Compress(l);
        }

        public static Image ResizeImage(this Image image, int w, int h)
        {
            if (image.Width == w && image.Height == h) return image;

            Image resizedImage = new Bitmap(w, h);

            using (var g = Graphics.FromImage(resizedImage))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, w, h);
            }
            return resizedImage;
        }

        public static byte[] ReadImageAsJpegWithSize(string fileName, int w, int h)
        {
            var image = Image.FromFile(fileName, true);
            image = image.ResizeImage(w, h);
            image = image.CompressToSize(JPEG_SIZE_MAX); //image.CompressToSize(65528);
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static byte[] ReadImageAsJpegAndCompress(string fileName)
        {
            var image = Image.FromFile(fileName, true);
            image = image.CompressToSize(JPEG_SIZE_MAX); //image.CompressToSize(65528);
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static byte[] MergeImages(string fileName1, string fileName2) =>
            MergeJpegs(ReadImageAsJpegAndCompress(fileName1), ReadImageAsJpegAndCompress(fileName2));

        public static byte[] MergeJpegs(byte[] content1, byte[] content2) => content1.Skip(2).Concat(content2.Skip(2)).ToArray();

        public static byte[] MergeJpegs(string fileName1, string fileName2) => File.ReadAllBytes(fileName1).Skip(2).Concat(File.ReadAllBytes(fileName2).Skip(2)).ToArray();
    }
}
