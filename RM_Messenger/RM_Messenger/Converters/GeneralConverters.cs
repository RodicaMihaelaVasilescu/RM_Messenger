using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RM_Messenger.Converters
{
  public static class GeneralConverters
  {
    public static byte[] ConvertToByteArray(string path)
    {
      return File.ReadAllBytes(path);
    }
    public static BitmapImage ConvertToBitmapImage(string path)
    {
      byte[] byteArray = File.ReadAllBytes(path);
      return ConvertFromByteArrayToBitmapImage(byteArray);
    }
    public static BitmapImage ConvertToBitmapImage(byte[] byteArray)
    {
      return ConvertFromByteArrayToBitmapImage(byteArray);
    }
    static BitmapImage ConvertFromByteArrayToBitmapImage(byte[] byteArray)
    {
      using (MemoryStream memoryStream = new MemoryStream(byteArray))
      {
        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.CacheOption = BitmapCacheOption.None;
        imageSource.StreamSource = memoryStream;
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.EndInit();
        return imageSource;
      }
    }
  }
}
