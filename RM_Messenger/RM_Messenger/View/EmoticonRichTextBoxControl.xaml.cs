using System.Collections.ObjectModel;
using System.Linq;
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
    private Collection<EmoticonMapper> emoticonsList;

    public EmoticonRichTextBoxControl()
    {
      InitializeComponent();
      emoticonsList = EmoticonCollection.Instance.Emoticons;
      TextBox.Focus();
    }
    public void UpdateDocumentBindings()
    {
      m_InternalUpdatePending = 2;

      var originalInput = GetOriginalInput();
      FlowDocument document = new FlowDocument();
      Paragraph paragraph = new Paragraph(new Run(originalInput));
      document.Blocks.Add(paragraph);
      SetValue(DocumentProperty, document);
    }

    private string GetOriginalInput()
    {
      string input = string.Empty;
      foreach (Block bk in TextBox.Document.Blocks)
      {
        if (bk is Paragraph)
        {
          Paragraph par = (Paragraph)bk;
          foreach (Inline inline in par.Inlines)
          {
            // if item is a ui control, like an image
            if (inline is InlineUIContainer)
            {
              InlineUIContainer ui = (InlineUIContainer)inline;
              if (ui.Child is Image)
              {
                Image img = (Image)ui.Child;
                string source = img.Source.ToString();
                var emoticon = emoticonsList.FirstOrDefault(e => e.Icon == source);
                if (emoticon != null)
                {
                  input += emoticon.Text + " ";
                }
              }
            }
            else
            {
              var inl = inline as Run;
              input += inl.Text;
            }
          }
        }
      }

      return input;
    }

    public void InitializeAndFocusTextBox()
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
