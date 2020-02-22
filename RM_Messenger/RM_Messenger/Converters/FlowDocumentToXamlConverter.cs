using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
namespace RM_Messenger.Converters
{
  [ValueConversion(typeof(string), typeof(FlowDocument))]
  public class FlowDocumentToXamlConverter : IValueConverter
  {
    #region IValueConverter Members

    /// <summary>
    /// Converts from XAML markup to a WPF FlowDocument.
    /// </summary>
    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

      var flowDocument = new FlowDocument();

      if (value != null)
      {
        var xamlText = (string)value;
        flowDocument = (FlowDocument)XamlReader.Parse(xamlText);
      }

      // Set return value
      return flowDocument;
    }

    /// <summary>
    /// Converts from a WPF FlowDocument to a XAML markup string.
    /// </summary>
    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

      // Exit if FlowDocument is null
      if (value == null) return string.Empty;

      // Get flow document from value passed in
      var flowDocument = (FlowDocument)value;

      // Convert to XAML and return
      return XamlWriter.Save(flowDocument);
    }

    #endregion
  }
}
