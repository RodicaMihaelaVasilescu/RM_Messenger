using System;
using System.Collections.Generic;
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
      Emoticons = new EmoticonCollection();
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/1.gif", Text = ":)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/2.gif", Text = ":(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/3.gif", Text = ";)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/4.gif", Text = ":D" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/5.gif", Text = ";;)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/6.gif", Text = ">:D<" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/7.gif", Text = ":-/" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/7.gif", Text = ":/" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/8.gif", Text = ":x" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/8.gif", Text = ":X" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/9.gif", Text = ":>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":-p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/10.gif", Text = ":-P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/11.gif", Text = ":*" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/12.gif", Text = "=((" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/13.gif", Text = ":o" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/14.gif", Text = "x(" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/15.gif", Text = ":>" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/16.gif", Text = "b-)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/17.gif", Text = ":s" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/18.gif", Text = "#:-S" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/19.gif", Text = ">:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/20.gif", Text = ":((" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/21.gif", Text = ":))" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/22.gif", Text = ":|" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/23.gif", Text = "/:)" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/24.gif", Text = "=))" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/36.gif", Text = "<:-P" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/36.gif", Text = "<:-p" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/65.gif", Text = ":-\"" });
      Emoticons.Add(new EmoticonMapper { Icon = "pack://application:,,,/RM_Messenger;component/Resources/Emoticons/102.gif", Text = "~x(" });
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
      var tp = Document.ContentStart;
      var word = WordBreaker.GetWordRange(tp);

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
          image.UriSource = new Uri(emoticon.Icon);//("pack://application:,,,/RM_Messenger;component/Resources/Emoticons/8.gif");
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
    public EmoticonCollection Emoticons { get; set; }
  }

}

