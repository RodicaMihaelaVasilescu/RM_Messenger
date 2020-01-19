using RM_Messenger.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for HomepageControl.xaml
  /// </summary>
  public partial class HomepageControl : UserControl
  {
    public HomepageControl()
    {
      InitializeComponent();
      var viewModel = new ChangeProfilePictureViewModel();
      viewModel.popup = ProfilePicturePopupTooltip;
      ChangeProfilePictureControl.DataContext = viewModel;
    }

    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
      ProfilePicturePopupTooltip.IsOpen = true;
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
