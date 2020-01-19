using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RM_Messenger.Converters
{
  public class NameConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      string name;

      switch ((string)parameter)
      {
        case "FormatFirstLast":
        name = values[0] + " " + values[1];
          break;
        case "FormatLastFirst":
          name = values[1] + " " + values[0];
          break;
        case "FormatNormal":
        default:
          name = values[0] + " " + values[1];
          break;
      }

      return name;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      string[] splitValues = ((string)value).Split(' ');
      return splitValues;
    }
  }
}
