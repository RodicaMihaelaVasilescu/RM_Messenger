using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for AddToMessengerRequestSecondControl.xaml
  /// </summary>
  public partial class AddToMessengerRequestSecondControl : UserControl
  {
    public AddToMessengerRequestSecondControl()
    {
      InitializeComponent();
      var themeColors = Helpers.ThemeColorsHelper.GetThemeColors();
      Background = themeColors.BackgroundColor;
      Gradient1.Color = themeColors.DarkColorGradient;
      Gradient2.Color = themeColors.MediumColorGradient;
      Gradient3.Color = themeColors.MediumColorGradient;
      LinearGradient2.GradientStops = LinearGradient.GradientStops;
      LinearGradient3.GradientStops = LinearGradient.GradientStops;

    }
  }
}
