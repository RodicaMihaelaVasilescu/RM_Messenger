using RM_Messenger.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RM_Messenger.ViewModel
{
  class ContactListsViewModel:INotifyPropertyChanged
  {
    private List<ContactListsModel> _contactsLists;

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
      ContactsLists = new List<ContactListsModel>();
      AddRecentContactsFakeList();
      AddAdressBookFakeList();
    }

    private void AddRecentContactsFakeList()
    {
      var recent = new ContactListsModel
      {
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/RecentContact_Icon.png",
        ContactsList = new List<DisplayedContactModel>()
      };
      recent.ContactsList.Add(new DisplayedContactModel
      {
        Name = "test",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      });

      recent.ListName = string.Format("Recent Contacts (0/{0})", recent.ContactsList.Count);
      ContactsLists.Add(recent);
    }

    private void AddAdressBookFakeList()
    {
      var addressBook = new ContactListsModel
      {
        ContactsList = new List<DisplayedContactModel>()
      };
      addressBook.ContactsList.Add(new DisplayedContactModel
      {
        Name = "test",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      });

      addressBook.ContactsList.Add(new DisplayedContactModel
      {
        Name = "ankii29.an",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture2.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Online.ico"

      });

      addressBook.ContactsList.Add(new DisplayedContactModel
      {
        Name = "iasmi31_iasmi",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture4.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      });
      addressBook.ContactsList.Add(new DisplayedContactModel
      {
        Name = "madalina.mada",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture3.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Online.ico"
      });
      addressBook.ContactsList.Add(new DisplayedContactModel
      {
        Name = "mihaela1234",
        ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
        OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Busy.ico"
      });

      addressBook.ListName = string.Format("Address Book ({0}/{1})", addressBook.ContactsList.Where(c => c.OnlineIcoPath.Contains("Online")).Count(), addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
