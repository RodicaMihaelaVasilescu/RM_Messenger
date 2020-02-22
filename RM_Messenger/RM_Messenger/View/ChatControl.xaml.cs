using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ChatControl.xaml
  /// </summary>
  /// 
  public partial class ChatControl : UserControl
  {
    public ChatControl()
    {
      InitializeComponent();
      ChatTextBoxControl.SendButton = SendCommandButton;
      ChatTextBoxControl.TextBox.Focus();
    }

    private void OnForceUpdateClick(object sender, RoutedEventArgs e)
    {
      this.ChatTextBoxControl.UpdateDocumentBindings();
      ChatTextBoxControl.TextBox.Focus();
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
  }
}
