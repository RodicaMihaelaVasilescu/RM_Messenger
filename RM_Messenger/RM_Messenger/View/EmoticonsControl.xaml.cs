using RM_Messenger.ViewModel;
using System.Linq;
using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for EmoticonsControl.xaml
  /// </summary>
  public partial class EmoticonsControl : UserControl
  {
    public string TextEmoticon;
    public EmoticonsControl()
    {
      InitializeComponent();
    }
    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var listViewItem = sender as ListViewItem;
      var emoticon = EmoticonCollection.Instance.Emoticons.Where(em => em.Icon == listViewItem.Content.ToString()).FirstOrDefault();
      if (emoticon != null)
      {
        TextEmoticon = emoticon.Text;
        EmoticonsViewModel viewModel = DataContext as EmoticonsViewModel;
        viewModel.TextEmoticon = emoticon.Text;
        viewModel.CloseAction();
      }
    }
  }
}
