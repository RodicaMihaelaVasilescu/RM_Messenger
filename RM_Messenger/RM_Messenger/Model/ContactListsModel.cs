using RM_Messenger.Helper;
using System.Collections.Generic;
using System.ComponentModel;
namespace RM_Messenger.Model
{
  class ContactListsModel: INotifyPropertyChanged
  {
    private DisplayedContactModel _selectedContact;

    public string ListName { get; set; }
    public string ImagePath { get; set; }
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
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedContact"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
