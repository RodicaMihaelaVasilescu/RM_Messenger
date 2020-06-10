using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for AddToMessengerRequestFirstControl.xaml
  /// </summary>
  public partial class AddToMessengerRequestFirstControl : UserControl
  {
    public AddToMessengerRequestFirstControl()
    {
      InitializeComponent();
      var themeColors = Helpers.ThemeColorsHelper.GetThemeColors();
      Background = themeColors.BackgroundColor;
      Gradient1.Color = themeColors.DarkColorGradient;
      Gradient2.Color = themeColors.MediumColorGradient;
      Gradient3.Color = themeColors.MediumColorGradient;
    }
  }
}
