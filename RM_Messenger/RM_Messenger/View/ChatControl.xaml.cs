using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.ViewModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ChatControl.xaml
  /// </summary>
  /// 
  public partial class ChatControl : System.Windows.Controls.UserControl
  {
    public string DisplayedUser_ID;
    public ChatControl()
    {
      InitializeComponent();
      ChatTextBoxControl.SendButton = SendCommandButton;
      SetBackgroundColors();
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

    public void SetBackgroundColors()
    {
      var themeColor = ThemeColorsHelper.GetThemeColors();
      GridChat.Background = themeColor.BackgroundColor;
      DockPanelChat.Background = themeColor.BackgroundColor;
      DockpanelGradientColor1.Color = themeColor.DockpanelGradientColor1;
      DockpanelGradientColor2.Color = themeColor.DockpanelGradientColor2;
      ChatTextBoxControl.GradientColor1.Color = themeColor.DockpanelGradientColor1;
      ChatTextBoxControl.GradientColor2.Color = themeColor.DockpanelGradientColor2;
      ChatTextBoxControl.GradientColor3.Color = themeColor.DockpanelGradientColor3;
      DockPanelChat.Background = themeColor.ChatPanelBackgroundColor;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      ChatTextBoxControl.DisplayedContact_ID = DisplayedUser_ID;
    }

    protected void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
    {
      System.Windows.Controls.ListViewItem item = (System.Windows.Controls.ListViewItem)sender;
      item.IsSelected = true;
    }
  }
}
