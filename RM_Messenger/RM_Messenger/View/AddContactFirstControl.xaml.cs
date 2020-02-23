using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for AddContactFirstControl.xaml
  /// </summary>
  public partial class AddContactFirstControl : UserControl
  {
    public AddContactFirstControl()
    {
      InitializeComponent();
      EmailTextBox.Focus();
    }
  }
}
