using RM_Messenger.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
      var x = Mouse.GetPosition(ProfilePicturePopupTooltip);
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
