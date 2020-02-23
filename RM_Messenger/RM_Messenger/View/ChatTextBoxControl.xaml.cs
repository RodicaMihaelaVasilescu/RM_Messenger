using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ChatTextBoxControl.xaml
  /// </summary>
  public partial class ChatTextBoxControl : UserControl
  {

    #region Dependency Property Declarations

    // CodeControlsVisibility property
    public static readonly DependencyProperty CodeControlsVisibilityProperty =
        DependencyProperty.Register("CodeControlsVisibility", typeof(Visibility),
        typeof(ChatTextBoxControl));

    // Document property
    public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.Register("Document", typeof(FlowDocument),
        typeof(ChatTextBoxControl), new PropertyMetadata(OnDocumentChanged));

    // ToolbarBackground property
    public static readonly DependencyProperty ToolbarBackgroundProperty =
        DependencyProperty.Register("ToolbarBackground", typeof(Brush),
        typeof(ChatTextBoxControl));

    // ToolbarBorderBrush property
    public static readonly DependencyProperty ToolbarBorderBrushProperty =
        DependencyProperty.Register("ToolbarBorderBrush", typeof(Brush),
        typeof(ChatTextBoxControl));

    // ToolbarBorderThickness property
    public static readonly DependencyProperty ToolbarBorderThicknessProperty =
        DependencyProperty.Register("ToolbarBorderThickness", typeof(Thickness),
        typeof(ChatTextBoxControl));

    #endregion

    #region Fields

    private int internalUpdatePending;

    public Button SendMessageButton { get; set; }

    public FlowDocument Document
    {
      get { return (FlowDocument)GetValue(DocumentProperty); }
      set { SetValue(DocumentProperty, value); }
    }

    #endregion

    #region Constructor
    public ChatTextBoxControl()
    {
      InitializeComponent();
      Initialize();
      TextBox.Focus();
    }
    #endregion

    private void Initialize()
    {
      FontFamilyCombo.ItemsSource = Fonts.SystemFontFamilies;
      FontSizeCombo.Items.Add("10");
      FontSizeCombo.Items.Add("12");
      FontSizeCombo.Items.Add("14");
      FontSizeCombo.Items.Add("18");
      FontSizeCombo.Items.Add("24");
      FontSizeCombo.Items.Add("36");
    }

    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      /* For unknown reasons, this method gets called twice when the 
       * Document property is set. Until we figure out why, we initialize
       * the flag to 2 and decrement it each time through this method. */

      // Initialize
      var thisControl = (ChatTextBoxControl)d;

      // Exit if this update was internally generated
      if (thisControl.internalUpdatePending > 0)
      {

        // Decrement flags and exit
        thisControl.internalUpdatePending--;
        return;
      }

      // Set Document property on RichTextBox
      thisControl.TextBox.Document = (e.NewValue == null) ? new FlowDocument() : (FlowDocument)e.NewValue;
    }



    public void UpdateDocumentBindings()
    {
      SetValue(DocumentProperty, this.TextBox.Document);
      SendButton.Focus();
    }

    /// <summary>
    /// Changes the font family of selected text.
    /// </summary>
    private void OnFontFamilyComboSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (FontFamilyCombo.SelectedItem == null) return;
      var fontFamily = FontFamilyCombo.SelectedItem.ToString();
      var textRange = new TextRange(TextBox.Selection.Start, TextBox.Selection.End);
      textRange.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
    }

    /// <summary>
    /// Changes the font size of selected text.
    /// </summary>
    private void OnFontSizeComboSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // Exit if no selection
      if (FontSizeCombo.SelectedItem == null) return;

      // clear selection if value unset
      if (FontSizeCombo.SelectedItem.ToString() == "{DependencyProperty.UnsetValue}")
      {
        FontSizeCombo.SelectedItem = null;
        return;
      }

      // Process selection
      var pointSize = FontSizeCombo.SelectedItem.ToString();
      var pixelSize = Convert.ToDouble(pointSize) * (96 / 72);
      var textRange = new TextRange(TextBox.Selection.Start, TextBox.Selection.End);
      textRange.ApplyPropertyValue(TextElement.FontSizeProperty, pixelSize);
    }

    /// <summary>
    /// Formats regular text
    /// </summary>
    private void OnNormalTextClick(object sender, RoutedEventArgs e)
    {
      var textRange = new TextRange(TextBox.Selection.Start, TextBox.Selection.End);
      textRange.ApplyPropertyValue(TextElement.FontFamilyProperty, FontFamily);
      textRange.ApplyPropertyValue(TextElement.FontSizeProperty, FontSize);
      textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Foreground);
      textRange.ApplyPropertyValue(Block.MarginProperty, new Thickness(Double.NaN));
    }

    /// <summary>
    /// Sets the toolbar.
    /// </summary>
    private void SetToolbar()
    {
      // Set font family combo
      var textRange = new TextRange(TextBox.Selection.Start, TextBox.Selection.End);
      var fontFamily = textRange.GetPropertyValue(TextElement.FontFamilyProperty);
      FontFamilyCombo.SelectedItem = fontFamily;

      // Set font size combo
      var fontSize = textRange.GetPropertyValue(TextElement.FontSizeProperty);
      FontSizeCombo.Text = fontSize.ToString();

      // Set Font buttons
      if (!String.IsNullOrEmpty(textRange.Text))
      {
        BoldButton.IsChecked = textRange.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold);
        ItalicButton.IsChecked = textRange.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic);
        UnderlineButton.IsChecked = textRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline);
      }
    }
    /// <summary>
    /// Updates the toolbar when the text selection changes.
    /// </summary>
    private void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
    {
      this.SetToolbar();
    }

    private void EmoticonsButton_MouseEnter(object sender, MouseEventArgs e)
    {
      EmoticonsPopupTooltip.IsOpen = true;
    }

    private void EmoticonsButton_MouseLeave(object sender, MouseEventArgs e)
    {
      EmoticonsPopupTooltip.IsOpen = false;
    }

    private void EmoticonsPopupTooltip_MouseLeave(object sender, MouseEventArgs e)
    {
      if (!EmoticonsPopupTooltip.IsMouseOver)
      {
        EmoticonsPopupTooltip.IsOpen = false;
      }
    }

    private void TextFormat_Click(object sender, RoutedEventArgs e)
    {
      if(TextFormatPanel.Visibility == Visibility.Visible)
      {
        TextFormatPanel.Visibility = Visibility.Collapsed;
      }
      else
      {
        TextFormatPanel.Visibility = Visibility.Visible;
      }

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
