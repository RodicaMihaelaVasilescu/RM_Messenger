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
          colorModel.BackgroundColor = new SolidColorBrush(Color.FromRgb(0xFF, 0X66, 0X99));
          colorModel.DarkBackgroundColor = new SolidColorBrush(Color.FromRgb(0xFF, 0X33, 0X77));
          colorModel.DockpanelGradientColor1 = Colors.LightPink;
          colorModel.DockpanelGradientColor2 = Color.FromRgb(0xFF, 0X66, 0X99);
          colorModel.DockpanelGradientColor3 = Color.FromRgb(0xFF, 0X66, 0X99);
          colorModel.MediumColorGradient = Color.FromRgb(0xFF, 0X33, 0X77);
          colorModel.DarkColorGradient = Color.FromRgb(0xFF, 0X33, 0X77);
          colorModel.LightColorGradient = Colors.LightPink;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = Brushes.LightPink;
          break;

        case Constants.Colors.RedColor:
          colorModel.BackgroundColor = Brushes.Red;
          colorModel.DarkBackgroundColor = Brushes.Crimson;
          colorModel.DockpanelGradientColor1 = Colors.MistyRose;
          colorModel.DockpanelGradientColor2 = Colors.Red;
          colorModel.DockpanelGradientColor3 = Colors.OrangeRed;
          colorModel.LightColorGradient = Colors.MistyRose;
          colorModel.MediumColorGradient = Colors.Crimson;
          colorModel.DarkColorGradient = Colors.Crimson;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = Brushes.MistyRose;
          break;

        case Constants.Colors.OrangeColor:
          colorModel.BackgroundColor = Brushes.Orange;
          colorModel.DarkBackgroundColor = Brushes.DarkOrange;
          colorModel.DockpanelGradientColor1 = Colors.BlanchedAlmond;
          colorModel.DockpanelGradientColor2 = Colors.Orange;
          colorModel.DockpanelGradientColor3 = Colors.Orange;
          colorModel.Foreground = Brushes.White;
          colorModel.LightColorGradient = Colors.BlanchedAlmond;
          colorModel.MediumColorGradient = Colors.Orange;
          colorModel.DarkColorGradient = Colors.DarkOrange;
          colorModel.ChatPanelBackgroundColor = Brushes.BlanchedAlmond;
          break;
        case Constants.Colors.YellowColor:

          AppConfigManager.Set(Resources.ThemeColorProperty, Constants.Colors.YellowColor);
          colorModel.BackgroundColor = Brushes.Goldenrod;
          colorModel.DarkBackgroundColor = Brushes.Goldenrod;
          colorModel.DockpanelGradientColor1 = Colors.Yellow;
          colorModel.DockpanelGradientColor2 = Colors.Goldenrod;
          colorModel.DockpanelGradientColor3 = Colors.Gold;
          colorModel.LightColorGradient = Colors.Yellow;
          colorModel.MediumColorGradient = Colors.Gold;
          colorModel.DarkColorGradient = Colors.Goldenrod;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = Brushes.Gold;
          break;

        case Constants.Colors.GreenColor:
          colorModel.BackgroundColor = Brushes.Green;
          colorModel.DarkBackgroundColor = Brushes.DarkGreen;
          colorModel.DockpanelGradientColor1 = Colors.LightGreen;
          colorModel.DockpanelGradientColor2 = Colors.Green;
          colorModel.DockpanelGradientColor3 = Colors.Green;
          colorModel.LightColorGradient = Colors.LightGreen;
          colorModel.MediumColorGradient = Colors.Green;
          colorModel.DarkColorGradient = Colors.DarkGreen;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = Brushes.LightGreen;

          break;

        case Constants.Colors.BlueColor:
          colorModel.BackgroundColor = Brushes.CornflowerBlue;
          colorModel.DarkBackgroundColor = Brushes.DarkBlue;
          colorModel.DockpanelGradientColor1 = Color.FromRgb(0xbb, 0xd0, 0xf7);
          colorModel.DockpanelGradientColor2 = Colors.CornflowerBlue;
          colorModel.DockpanelGradientColor3 = Colors.CornflowerBlue;
          colorModel.LightColorGradient = Colors.CornflowerBlue;
          colorModel.MediumColorGradient = Colors.CornflowerBlue;
          colorModel.DarkColorGradient = Colors.DarkBlue;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = new SolidColorBrush(Color.FromRgb(0xbb, 0xd0, 0xf7));
          break;

        case Constants.Colors.IndigoColor:
          colorModel.BackgroundColor = Brushes.Indigo;
          colorModel.DarkBackgroundColor = Brushes.Navy;
          colorModel.DockpanelGradientColor1 = Colors.Lavender;
          colorModel.DockpanelGradientColor2 = Colors.Indigo;
          colorModel.DockpanelGradientColor3 = Colors.Indigo;
          colorModel.LightColorGradient = Colors.MediumPurple;
          colorModel.MediumColorGradient = Colors.Indigo;
          colorModel.DarkColorGradient = Colors.Navy;
          colorModel.Foreground = Brushes.White;
          colorModel.ChatPanelBackgroundColor = Brushes.MediumPurple;
          break;

        case Constants.Colors.BlackColor:
          colorModel.BackgroundColor = Brushes.Black;
          colorModel.DarkBackgroundColor = Brushes.Black;
          colorModel.DockpanelGradientColor1 = Colors.Gray;
          colorModel.DockpanelGradientColor2 = Colors.Black;
          colorModel.DockpanelGradientColor3 = Colors.Black;
          colorModel.LightColorGradient = Colors.Gray;
          colorModel.MediumColorGradient = Colors.Gray;
          colorModel.ChatPanelBackgroundColor = Brushes.Gray;
          colorModel.DarkColorGradient = Colors.Black;
          break;

        default:
          //Purple
          colorModel.BackgroundColor = new SolidColorBrush(Color.FromRgb(0x76, 0x24, 0x6E));
          colorModel.DockpanelGradientColor1 = Color.FromRgb(0x48, 0x10, 0x41);
          colorModel.DockpanelGradientColor2 = Color.FromRgb(0x76, 0x24, 0x6E);
          colorModel.DockpanelGradientColor3 = Color.FromRgb(0x69, 0x30, 0x60);
          colorModel.DarkColorGradient = Color.FromRgb(0x48, 0x10, 0x41);
          colorModel.LightColorGradient = Color.FromRgb(0x69, 0x30, 0x60);
          colorModel.MediumColorGradient = Color.FromRgb(0x7B, 0x41, 0x71);
          colorModel.ChatPanelBackgroundColor = new SolidColorBrush(Color.FromRgb(0x94, 0x5F, 0x8D));
          colorModel.DarkBackgroundColor = new SolidColorBrush(Color.FromRgb(0x48, 0x10, 0x41));
          break;
      }
      return colorModel;
    }
  }
}
