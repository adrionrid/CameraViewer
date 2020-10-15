using System.IO;
using System.Windows.Media.Imaging;

namespace CameraViewer
{
    public static class ImageConverter
    {
        public static BitmapImage GetBitmapImage(byte[] rawImage)
        {
            using (var stream = new MemoryStream(rawImage))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
