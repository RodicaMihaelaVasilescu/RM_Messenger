using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
      messageContent = 2;
      var FlowDoc = new TextRange(this.TextBox.Document.ContentStart, this.TextBox.Document.ContentEnd).Text;
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
