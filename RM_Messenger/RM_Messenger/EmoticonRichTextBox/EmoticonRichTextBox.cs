using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace RM_Messenger
{
  public class EmoticonRichTextBox : RichTextBox
  {
    public EmoticonRichTextBox()
    {
      Emoticons = EmoticonCollection.Instance.Emoticons;
    }

    private DispatcherTimer _timer;

    /// <summary>
    /// Override to trigger the look up.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
      //Looking for an idle time to start look up..
      if (_timer == null)
      {
        _timer = new DispatcherTimer(DispatcherPriority.Background);
        _timer.Interval = TimeSpan.FromSeconds(0.25);
        _timer.Tick += LookUp;
      }

      //Restart timer here...
      _timer.Stop();
      _timer.Start();

      base.OnTextChanged(e);
    }

    private void LookUp(object sender, EventArgs e)
    {
      Dispatcher.InvokeAsync(UpdateSmileys);
      _timer.Stop();
    }

    /// <summary>
    /// Iterate through words and get the text range of smiley text. Replace it with corresponding icon.
    /// </summary>
    private void UpdateSmileys()
    {
      var textPointer = Document.ContentStart;
      var textRange = new TextRange(Document.ContentStart, Document.ContentEnd).Text;
      if (string.IsNullOrEmpty(textRange))
      {
        textPointer.InsertTextInRun(" ");
        CaretPosition = Document.ContentEnd;
      }
      var word = WordBreaker.GetWordRange(textPointer);

      while (word.End.GetNextInsertionPosition(LogicalDirection.Forward) != null)
      {
        word = WordBreaker.GetWordRange(word.End.GetNextInsertionPosition(LogicalDirection.Forward));
        var smileys = from smiley in Emoticons
                      where smiley.Text == word.Text
                      select smiley;

        var emoticonMappers = smileys as IList<EmoticonMapper> ?? smileys.ToList();
        if (emoticonMappers.Any())
        {
          var emoticon = emoticonMappers.FirstOrDefault();

          var img = new Image() { Stretch = Stretch.None };
          ReplaceTextRangeWithImage(word, img);
          var image = new BitmapImage();
          image.BeginInit();
          image.UriSource = new Uri(emoticon.Icon);
          image.EndInit();
          ImageBehavior.SetAnimatedSource(img, image);
        }
      }
    }

    /// <summary>
    /// Replacing the text range with image.
    /// </summary>
    /// <param name="textRange">The smiley text range.</param>
    /// <param name="image">The smiley icon</param>
    public void ReplaceTextRangeWithImage(TextRange textRange, Image image)
    {
      if (textRange.Start.Parent is Run)
      {
        var run = textRange.Start.Parent as Run;

        var runBefore =
            new Run(new TextRange(run.ContentStart, textRange.Start).Text);
        var runAfter =
            new Run(new TextRange(textRange.End, run.ContentEnd).Text);


        if (textRange.Start.Paragraph != null)
        {
          textRange.Start.Paragraph.Inlines.Add(runBefore);
          textRange.Start.Paragraph.Inlines.Add(image);
          textRange.Start.Paragraph.Inlines.Add(runAfter);
          textRange.Start.Paragraph.Inlines.Remove(run);
        }

        CaretPosition = runAfter.ContentEnd;

      }
    }

    /// <summary>
    /// The collection of Emoticon mappers
    /// </summary>
    public Collection<EmoticonMapper> Emoticons { get; set; }
  }

}

