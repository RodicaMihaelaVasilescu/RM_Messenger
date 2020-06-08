using RM_Messenger.Model;
using RM_Messenger.Properties;
using System.Windows.Media;

namespace RM_Messenger.Helpers
{
  class ThemeColorsHelper
  {
    public static ThemeColorModel GetThemeColors(string SelectedThemeColor = null)
    {
      if (SelectedThemeColor == null)
      {
        SelectedThemeColor = AppConfigManager.Get(Resources.ThemeColorProperty);
      }
      ThemeColorModel colorModel = new ThemeColorModel();
      switch (SelectedThemeColor)
      {
        case Constants.Colors.PinkColor:
          colorModel.BackgroundColor = Brushes.DeepPink;
          colorModel.DarkBackgroundColor = Brushes.MediumVioletRed;
          colorModel.DockpanelGradientColor1 = Colors.LightPink;
          colorModel.DockpanelGradientColor2 = Colors.DeepPink;
          colorModel.DockpanelGradientColor3 = Colors.Pink;
          colorModel.DarkColorGradient = Colors.MediumVioletRed;
          colorModel.LightColorGradient = Colors.LightPink;
          colorModel.Foreground = Brushes.Black;
          break;

        case Constants.Colors.RedColor:
          colorModel.BackgroundColor = Brushes.Red;
          colorModel.DarkBackgroundColor = Brushes.Crimson;
          colorModel.DockpanelGradientColor1 = Colors.MistyRose;
          colorModel.DockpanelGradientColor2 = Colors.Red;
          colorModel.DockpanelGradientColor3 = Colors.OrangeRed;
          colorModel.DarkColorGradient = Colors.Crimson;
          colorModel.LightColorGradient = Colors.MistyRose;
          colorModel.Foreground = Brushes.Black;
          break;

        case Constants.Colors.OrangeColor:
          colorModel.BackgroundColor = Brushes.Orange;
          colorModel.DarkBackgroundColor = Brushes.DarkOrange;
          colorModel.DockpanelGradientColor1 = Colors.BlanchedAlmond;
          colorModel.DockpanelGradientColor2 = Colors.Orange;
          colorModel.DockpanelGradientColor3 = Colors.Orange;
          colorModel.Foreground = Brushes.Black;
          colorModel.DarkColorGradient = Colors.DarkOrange;
          colorModel.LightColorGradient = Colors.BlanchedAlmond;
          break;
        case Constants.Colors.YellowColor:

          AppConfigManager.Set(Resources.ThemeColorProperty, Constants.Colors.YellowColor);
          colorModel.BackgroundColor = Brushes.Goldenrod;
          colorModel.DarkBackgroundColor = Brushes.Goldenrod;
          colorModel.DockpanelGradientColor1 = Colors.Yellow;
          colorModel.DockpanelGradientColor2 = Colors.Goldenrod;
          colorModel.DockpanelGradientColor3 = Colors.Gold;
          colorModel.DarkColorGradient = Colors.Goldenrod;
          colorModel.LightColorGradient = Colors.Yellow;
          colorModel.Foreground = Brushes.Black;
          break;

        case Constants.Colors.GreenColor:
          colorModel.BackgroundColor = Brushes.Green;
          colorModel.DarkBackgroundColor = Brushes.DarkGreen;
          colorModel.DockpanelGradientColor1 = Colors.LightGreen;
          colorModel.DockpanelGradientColor2 = Colors.Green;
          colorModel.DockpanelGradientColor3 = Colors.Green;
          colorModel.DarkColorGradient = Colors.DarkGreen;
          colorModel.LightColorGradient = Colors.LightGreen;
          colorModel.Foreground = Brushes.Black;

          break;

        case Constants.Colors.BlueColor:
          colorModel.BackgroundColor = Brushes.CornflowerBlue;
          colorModel.DarkBackgroundColor = Brushes.DarkBlue;
          colorModel.DockpanelGradientColor1 = Colors.LightBlue;
          colorModel.DockpanelGradientColor2 = Colors.CornflowerBlue;
          colorModel.DockpanelGradientColor3 = Colors.CornflowerBlue;
          colorModel.DarkColorGradient = Colors.DarkBlue;
          colorModel.LightColorGradient = Colors.CornflowerBlue;
          colorModel.Foreground = Brushes.Black;
          break;

        case Constants.Colors.IndigoColor:
          colorModel.BackgroundColor = Brushes.Indigo;
          colorModel.DarkBackgroundColor = Brushes.Navy;
          colorModel.DockpanelGradientColor1 = Colors.Lavender;
          colorModel.DockpanelGradientColor2 = Colors.Indigo;
          colorModel.DockpanelGradientColor3 = Colors.Indigo;
          colorModel.DarkColorGradient = Colors.Navy;
          colorModel.LightColorGradient = Colors.MediumPurple;
          colorModel.Foreground = Brushes.Black;
          break;

        case Constants.Colors.BlackColor:
          colorModel.BackgroundColor = Brushes.Black;
          colorModel.DarkBackgroundColor = Brushes.Black;
          colorModel.DockpanelGradientColor1 = Colors.Gray;
          colorModel.DockpanelGradientColor2 = Colors.Black;
          colorModel.DockpanelGradientColor3 = Colors.Black;
          colorModel.DarkColorGradient = Colors.Black;
          colorModel.LightColorGradient = Colors.Gray;
          break;

        default:
          //Purple
          colorModel.BackgroundColor = new SolidColorBrush(Color.FromRgb(0x76, 0x24, 0x6E));
          colorModel.DockpanelGradientColor1 = Color.FromRgb(0x48, 0x10, 0x41);
          colorModel.DockpanelGradientColor2 = Color.FromRgb(0x76, 0x24, 0x6E);
          colorModel.DockpanelGradientColor3 = Color.FromRgb(0x69, 0x30, 0x60);
          colorModel.DarkColorGradient = Color.FromRgb(0x48, 0x10, 0x41);
          colorModel.LightColorGradient = Color.FromRgb(0x76, 0x24, 0x6E);
          colorModel.DarkBackgroundColor = new SolidColorBrush(Color.FromRgb(0x48, 0x10, 0x41));
          break;
      }
      return colorModel;
    }
  }
}
