using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.ViewModel;
using System.Windows.Controls;
using System.Windows.Media;

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

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      var themeColor = ThemeColorsHelper.GetThemeColors();
      BackgroundColor1.Color = themeColor.DockpanelGradientColor1;
      BackgroundColor2.Color = themeColor.DockpanelGradientColor2;
      BackgroundColor3.Color = themeColor.DockpanelGradientColor3;
      DisplayImage.Foreground = themeColor.Foreground ?? Brushes.DodgerBlue;
      ContactDetailsButton.Foreground = themeColor.Foreground ?? Brushes.DodgerBlue;
      AccountInfoButton.Foreground = themeColor.Foreground ?? Brushes.DodgerBlue;
      MyProfileButton.Foreground = themeColor.Foreground ?? Brushes.DodgerBlue;
    }
  }
}
