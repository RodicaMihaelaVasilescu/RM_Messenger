using RM_Messenger.Helper;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace RM_Messenger.Model
{
  class ContactListsModel: INotifyPropertyChanged
  {
    private DisplayedContactModel _selectedContact;
    public string ListName { get; set; }
    public BitmapImage ImagePath { get; set; }
    public bool IsExpanded { get; set; } = false;
    public List<DisplayedContactModel> ContactsList { get; set; }
    public DisplayedContactModel SelectedContact
    {
      get { return _selectedContact; }
      set
      {
        if (_selectedContact == value) return;
        _selectedContact = value;
        if(value != null)
        {
          WindowManager.OpenChatWindow(value);
          SelectedContact = null;
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedContact"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
