using RM_Messenger.Database;
using RM_Messenger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RM_Messenger.ViewModel
{
  class ContactListsViewModel : INotifyPropertyChanged
  {
    private List<ContactListsModel> _contactsLists;
    private RMMessengerEntities _context;

    public List<ContactListsModel> ContactsLists
    {
      get { return _contactsLists; }
      set
      {
        if (_contactsLists == value) return;
        _contactsLists = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ContactsLists"));
      }
    }

    public ContactListsViewModel()
    {
      InitializeContactList();
    }

    private void InitializeContactList()
    {
      _context = new RMMessengerEntities();
      ContactsLists = new List<ContactListsModel>();

      LoadRecentList();
      LoadAddressBook();
    }

    private void LoadAddressBook()
    {

      var currentUser = UserModel.Instance.Username;
      var addressBook = new ContactListsModel
      {
        ContactsList = new List<DisplayedContactModel>(_context.AddressBooks.
        Where(a => a.User_ID == UserModel.Instance.Username).ToList().Select(a => new DisplayedContactModel { Name = a.Friend_User_ID }))
      };

      foreach (var address in addressBook.ContactsList)
      {
        address.ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(
          _context.Users.Where(u => u.User_ID == address.Name).Select(u => u.ProfilePicture).FirstOrDefault());
        address.OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
      }

      addressBook.ListName = string.Format("Address Book ({0}/{1})", addressBook.ContactsList.Where(c => c.OnlineIcoPath.Contains("Online")).Count(), addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);

    }

    private void LoadRecentList()
    {
      var currentUser = UserModel.Instance.Username;
      var recentList = new ContactListsModel();
      // to do
      recentList.ContactsList = new List<DisplayedContactModel>();
      recentList.ListName = string.Format("Recent ({0}/{1})", recentList.ContactsList.Where(c => c.OnlineIcoPath.Contains("Online")).Count(), recentList.ContactsList.Count);
      ContactsLists.Add(recentList);
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
