using RM_Messenger.Database;
using RM_Messenger.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RM_Messenger.ViewModel
{
  class ContactListsViewModel:INotifyPropertyChanged
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
      AddRecentContactsFakeList();
      AddAdressBookFakeList();
    }

    private void AddRecentContactsFakeList()
    {
      //var recent = new ContactListsModel
      //{
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/RecentContact_Icon.png",
      //  ContactsList = new List<DisplayedContactModel>()
      //};
      //recent.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "test",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      //});

      //recent.ListName = string.Format("Recent Contacts (0/{0})", recent.ContactsList.Count);
      //ContactsLists.Add(recent);
    }

    private void AddAdressBookFakeList()
    {
      var addressBook = new ContactListsModel
      {
        ContactsList = new List<DisplayedContactModel>(_context.AddressBooks.
        Where(a => a.User_ID == UserModel.Instance.Username).ToList().Select(a => new DisplayedContactModel{Name= a.Friend_User_ID}))
      };

      //addressBook.ContactsList = new List< DisplayedContactModel>(from addess in _context.AddressBooks
      //              join user in _context.Users on addess.Friend_User_ID equals user.User_ID
      //              select new DisplayedContactModel { Name = addess.Friend_User_ID });

      foreach(var address in addressBook.ContactsList)
      {
        address.ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(
          _context.Users.Where(u => u.User_ID == address.Name).Select(u => u.ProfilePicture).FirstOrDefault());
        address.OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
      }
      //   addressBook.ContactsList =  _context.Users    // your starting point - table in the "from" statement
      //.Join(_context.AddressBooks, // the source table of the inner join
      //   address => address.User_ID,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
      //   user => user,   // Select the foreign key (the second part of the "on" clause)
      //   (post, meta) => new { Post = post, Meta = meta }) // selection
      //.Where(addressAndUser => addressAndUser.Post.ID == id);    // where statement
      //     MessagesList = new ObservableCollection<MessageModel>(_context.Messages.OrderBy(m => m.Date).Where(u => u.SentTo_User_ID == DisplayedUser.Name && u.SentBy_User_ID == UserModel.Instance.Username ||
      //u.SentBy_User_ID == DisplayedUser.Name && u.SentTo_User_ID == UserModel.Instance.Username).ToList().Select(m =>
      //new MessageModel
      //{
      //  SentBy = string.Format("{0}\n{1}", m.SentBy_User_ID, m.Date.ToString("dd/MM/yyyy HH:mm")),
      //  SentTo = m.SentTo_User_ID,
      //  Content = m.Message_Content,
      //  HorizontalAlignment = m.SentTo_User_ID == DisplayedUser.Name ? HorizontalAlignment.Right : HorizontalAlignment.Left
      //}));
      //addressBook.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "test",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      //});

      //addressBook.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "ankii29.an",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture2.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Online.ico"

      //});

      //addressBook.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "iasmi31_iasmi",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture4.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico"
      //});
      //addressBook.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "madalina.mada",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture3.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Online.ico"
      //});
      //addressBook.ContactsList.Add(new DisplayedContactModel
      //{
      //  Name = "mihaela1234",
      //  ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/OfflineProfilePicture.jpg",
      //  OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Busy.ico"
      //});

      addressBook.ListName = string.Format("Address Book ({0}/{1})", addressBook.ContactsList.Where(c => c.OnlineIcoPath.Contains("Online")).Count(), addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
