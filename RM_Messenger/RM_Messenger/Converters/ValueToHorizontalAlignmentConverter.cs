using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RM_Messenger.Converters
{
  public class ValueToHorizontalAlignmentConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      HorizontalAlignment horizontalAlignment = (HorizontalAlignment) value;
      return horizontalAlignment;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      HorizontalAlignment horizontalAlignment;

      // All I'm doing here is simply getting the integer value of the Enumeration.
      switch ((int)value)
      {
        case 0:
          // Left to Left
          horizontalAlignment = HorizontalAlignment.Left;
          break;
        case 1:
          // Right to Right
          horizontalAlignment = HorizontalAlignment.Right;
          break;
        case 2:
          // Center to Center
          horizontalAlignment = HorizontalAlignment.Center;
          break;
        default:
          // Justify to Stretch
          horizontalAlignment = HorizontalAlignment.Stretch;
          break;
      }

      return horizontalAlignment;
    }
  }
}
