using RM_Messenger.Model;
using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ChangeProfilePictureControl.xaml
  /// </summary>
  public partial class ChangeProfilePictureControl : UserControl
  {
    public ChangeProfilePictureControl()
    {
      InitializeComponent();
      ProfilePicture.Source = Converters.GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
    }
  }
}
