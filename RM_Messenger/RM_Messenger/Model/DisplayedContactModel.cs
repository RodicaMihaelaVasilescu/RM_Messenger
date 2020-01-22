using System.Windows.Media.Imaging;

namespace RM_Messenger.Model
{
  public class DisplayedContactModel
  {
    public string Name { get; set; }
    public string Image { get; set; }
    public BitmapImage ImagePath { get; set; }
    public string OnlineIcoPath { get; set; }
  }
}
