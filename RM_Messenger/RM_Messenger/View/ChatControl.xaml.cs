using RM_Messenger.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;


namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ChatControl.xaml
  /// </summary>
  public partial class ChatControl : UserControl
  {
    public ChatControl()
    {
      InitializeComponent();
      EmoticonsControl.DataContext = new EmoticonsViewModel();
      MessageBox.Focus();
    }
    private void TS_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      ScrollViewer scrollviewer = sender as ScrollViewer;
      if (e.Delta > 0)
      {
        scrollviewer.LineUp();
      }
      else
      {
        scrollviewer.LineDown();
      }
      e.Handled = true;
    }

    private void EmoticonsButton_MouseEnter(object sender, MouseEventArgs e)
    {
      EmoticonsPopupTooltip.IsOpen = true;
    }
    private void EmoticonsTooltip_MouseLeave(object sender, MouseEventArgs e)
    {
      EmoticonsPopupTooltip.IsOpen = false;
    }

    private void EmoticonsButton_MouseLeave(object sender, MouseEventArgs e)
    {
      if (!EmoticonsPopupTooltip.IsMouseOver)
      {
        EmoticonsPopupTooltip.IsOpen = false;
      }
    }
  }
}
