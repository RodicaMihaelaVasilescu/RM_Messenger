using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.ViewModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

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

    private void StatusTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        var _context = new RMMessengerEntities();
        var account = _context.Accounts.Where(a => a.User_ID == UserModel.Instance.Username).FirstOrDefault();
        account.Status = StatusTextBox.Text;
        _context.SaveChanges();
        ChangeStatusButton.Focus();
      }
    }

    private void InnerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var value = sender as ListView;
      var selectedContact = value.SelectedItem as DisplayedContactModel;
      WindowManager.OpenChatWindow(selectedContact);
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }

  }
}
