using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for EmoticonRichTextBoxMessagesControl.xaml
  /// </summary>
  public partial class EmoticonRichTextBoxMessagesControl : UserControl
  {
    public static readonly DependencyProperty DocumentProperty =
 DependencyProperty.Register("Document", typeof(FlowDocument),
 typeof(EmoticonRichTextBoxMessagesControl), new PropertyMetadata(OnDocumentChanged));

    public FlowDocument Document
    {
      get { return (FlowDocument)GetValue(DocumentProperty); }
      set { SetValue(DocumentProperty, value); }
    }


    private int messageContent;
    public EmoticonRichTextBoxMessagesControl()
    {
      InitializeComponent();
    }
    public void UpdateDocumentBindings()
    {
      SetValue(DocumentProperty, this.TextBox.Document);
    }
    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisControl = (EmoticonRichTextBoxMessagesControl)d;
      if (thisControl.messageContent > 0)
      {
        thisControl.messageContent--;
        return;
      }
      thisControl.TextBox.Document = (e.NewValue == null) ? new FlowDocument() : (FlowDocument)e.NewValue;
    }

  }
}
