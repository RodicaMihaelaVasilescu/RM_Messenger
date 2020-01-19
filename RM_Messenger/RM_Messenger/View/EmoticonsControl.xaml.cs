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
  /// Interaction logic for EmoticonsControl.xaml
  /// </summary>
  public partial class EmoticonsControl : UserControl
  {
    public EmoticonsControl()
    {
      InitializeComponent();
    }
    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
    }
  }
}
