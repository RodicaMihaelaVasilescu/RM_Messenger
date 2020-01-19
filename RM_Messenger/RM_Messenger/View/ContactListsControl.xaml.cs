using RM_Messenger.ViewModel;
using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for ContactListsControl.xaml
  /// </summary>
  public partial class ContactListsControl : UserControl
  {
    public ContactListsControl()
    {
      InitializeComponent();
      DataContext = new ContactListsViewModel();
    }
  }
}
