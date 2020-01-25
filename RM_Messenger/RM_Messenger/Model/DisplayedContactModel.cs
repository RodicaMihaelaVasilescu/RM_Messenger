using System.Windows.Media.Imaging;

namespace RM_Messenger.Model
{
  public class DisplayedContactModel
  {
    public string UserId { get; set; }
    public string Status { get; set; }
    public BitmapImage ImagePath { get; set; }
    public string OnOffImage { get; set; }
  }
}
