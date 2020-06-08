using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.ViewModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
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
    private ChatControl chatControl;

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
      chatControl = WindowManager.OpenChatWindow(selectedContact);
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      ((Expander)sender).BringIntoView();
    }

    private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (sender is ListBox && !e.Handled)
      {
        e.Handled = true;
        var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
        eventArg.Source = sender;
        var parent = ((Control)sender).Parent as UIElement;
        parent.RaiseEvent(eventArg);
      }
    }

    private void ListView_Selected(object sender, RoutedEventArgs e)
    {
      chatControl?.SetBackgroundColors();
    }
  }
}
