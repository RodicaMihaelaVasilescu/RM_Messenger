using RM_Messenger.Model;
using RM_Messenger.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for HomepageControl.xaml
  /// </summary>
  public partial class HomepageControl : UserControl
  {
    ChangeProfilePictureViewModel changeProfilePictureViewModel;
    public HomepageControl()
    {
      InitializeComponent();
      changeProfilePictureViewModel = new ChangeProfilePictureViewModel();
      changeProfilePictureViewModel.popup = ProfilePicturePopupTooltip;
      ChangeProfilePictureControl.DataContext = changeProfilePictureViewModel;
      ContactListsControl.DataContext = new ContactListsViewModel();
    }

    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
      ProfilePicturePopupTooltip.IsOpen = true;
      changeProfilePictureViewModel.ProfilePicture =
        Converters.GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
    }
    private void Tooltip_MouseLeave(object sender, MouseEventArgs e)
    {
      ProfilePicturePopupTooltip.IsOpen = false;
    }

    private void Button_MouseLeave(object sender, MouseEventArgs e)
    {
      if (!ProfilePicturePopupTooltip.IsMouseOver)
      {
        ProfilePicturePopupTooltip.IsOpen = false;
      }
    }
  }
}
