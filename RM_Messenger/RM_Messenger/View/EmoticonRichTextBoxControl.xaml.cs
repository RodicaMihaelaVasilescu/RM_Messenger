using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for EmoticonRichTextBoxControl.xaml
  /// </summary>
  public partial class EmoticonRichTextBoxControl : UserControl
  {
    public static readonly DependencyProperty DocumentProperty =
     DependencyProperty.Register("Document", typeof(FlowDocument),
     typeof(EmoticonRichTextBoxControl), new PropertyMetadata(OnDocumentChanged));

    public FlowDocument Document
    {
      get { return (FlowDocument)GetValue(DocumentProperty); }
      set { SetValue(DocumentProperty, value); }
    }
    public Button SendButton { get; set; }

    private int m_InternalUpdatePending;
    private bool m_TextHasChanged;

    public EmoticonRichTextBoxControl()
    {
      InitializeComponent();
      TextBox.Focus();
    }
    public void UpdateDocumentBindings()
    {
      // Exit if text hasn't changed
      //if (!m_TextHasChanged) return;

      // Set 'Internal Update Pending' flag
      m_InternalUpdatePending = 2;

      // Set Document property
      var FlowDoc = new TextRange(this.TextBox.Document.ContentStart, this.TextBox.Document.ContentEnd).Text;
      SetValue(DocumentProperty, this.TextBox.Document);
    }

    public void FocusTextBox()
    {
      TextBox.Focus();
    }
    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisControl = (EmoticonRichTextBoxControl)d;
      if (thisControl.m_InternalUpdatePending > 0)
      {
        thisControl.m_InternalUpdatePending--;
        return;
      }
      thisControl.TextBox.Document = (e.NewValue == null) ? new FlowDocument() : (FlowDocument)e.NewValue;
      thisControl.m_TextHasChanged = false;
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        SendButton.Focus();
        e.Handled = false;
      }
      base.OnPreviewKeyDown(e);
    }
  }
}
