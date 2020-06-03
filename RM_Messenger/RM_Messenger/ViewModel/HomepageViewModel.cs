using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class HomepageViewModel : INotifyPropertyChanged
  {

    #region Public Properties

    public Action CloseAction { get; set; }
    public ICommand LogoutCommand { get; set; }
    public ICommand SendCommand { get; set; }
    public ICommand AddFriendCommand { get; set; }
    public ICommand ChangeStatusCommand { get; set; }
    public RelayCommand RefreshCommand { get; set; }
    public string OnOffImage { get; set; } = UserModel.Instance.IsOnline ? "pack://application:,,,/RM_Messenger;component/Resources/Online.ico" : "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";

    public ObservableCollection<ContactListsModel> ContactsLists
    {
      get { return _contactsLists; }
      set
      {
        if (_contactsLists == value) return;
        _contactsLists = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ContactsLists"));
      }
    }

    public string Email
    {
      get { return _email; }
      set
      {
        if (_email == value) return;
        _email = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }
    public string Status
    {
      get { return _status; }
      set
      {
        if (_status == value) return;
        _status = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
      }
    }
    public string SearchedUser
    {
      get { return _searchedUser; }
      set
      {
        if (_searchedUser == value) return;
        _searchedUser = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchedUser"));
      }
    }

    public DisplayedContactModel SelectedContact
    {
      get { return _selectedContact; }
      set
      {
        if (_selectedContact == value) return;
        _selectedContact = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedContact"));
      }
    }

    public BitmapImage ProfilePicture
    {
      get { return profilePicture; }
      set
      {
        profilePicture = value;
        if (value != null) profilePicture.CacheOption = BitmapCacheOption.None;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePicture"));
      }
    }

    #endregion

    #region Private Properties

    private string _email;
    private string _status;
    private string _searchedUser;
    private Window window;
    private BitmapImage profilePicture;
    private ObservableCollection<ContactListsModel> _contactsLists;
    private RMMessengerEntities _context;
    private DisplayedContactModel _selectedContact;

    #endregion

    #region Constructor

    public HomepageViewModel(Window window)
    {
      this.window = window;
      _context = new RMMessengerEntities();
      window.Activated += new EventHandler(RefreshProfilePicture);
      LogoutCommand = new RelayCommand(LogoutCommandExecute);
      ChangeStatusCommand = new RelayCommand(ChangeStatusCommandExecute);
      AddFriendCommand = new RelayCommand(AddFriendCommandExecute);
      RefreshCommand = new RelayCommand(RefreshCommandExecute);

      InitializeUserProfile();
      InitializeContactList();
    }

    #endregion

    #region Public methods
    public void ReloadContactLists(object sender, EventArgs e)
    {
      InitializeContactList();
    }

    #endregion

    #region Private Methods
    void RefreshCommandExecute()
    {
      InitializeUserProfile();
      InitializeContactList();
      OpenAddRequests();
    }

    private void OpenAddRequests()
    {
      var request = _context.AddRequests.Where(a => a.SentTo_User_ID == UserModel.Instance.Username && a.Status == Resources.NoResponseStatus).FirstOrDefault();
      if (request == null)
      {
        return;
      }
      request.Status = Resources.SentStatus;
      _context.SaveChanges();
      int offset = 0;
      Application.Current.Dispatcher.Invoke((Action)delegate
      {
        Window addRequestWindow = new Window();
        var addRequestViewModel = new AddToMessengerRequestFirstViewModel(addRequestWindow, request);
        WindowManager.CreateGeneralWindow(addRequestWindow, addRequestViewModel, Resources.AddToMessengerRequestWindowTitle, Resources.AddToMessengerRequestFirstControlPath);

        if (addRequestViewModel.CloseAction == null)
        {
          addRequestViewModel.CloseAction = () => addRequestWindow.Close();
          addRequestWindow.Closed += new EventHandler(this.ReloadContactLists);
        }
        addRequestWindow.Owner = window;
        addRequestWindow.Left = window.Left - 400 + offset;
        addRequestWindow.Top = window.Top + 150 + offset;
        offset += 60;
        addRequestWindow.Tag = "Child";
        addRequestWindow.Show();
      });

    }

    private void InitializeUserProfile()
    {
      Email = UserModel.Instance.Username;
      ProfilePicture = GetProfilePicture(UserModel.Instance.ProfilePicture);
      Status = UserModel.Instance.Status;
    }

    private void LoadFriendList()
    {
      var currentUser = UserModel.Instance.Username;
      var friendList = new ContactListsModel
      {
        ContactsList = new ObservableCollection<DisplayedContactModel>(_context.Friendships.
        Where(a => a.User_ID == UserModel.Instance.Username).ToList().OrderByDescending(a => a.Date).Select(a => new DisplayedContactModel { UserId = a.Friend_ID }))
      };

      foreach (var friend in friendList.ContactsList)
      {
        friend.ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(
         _context.Accounts.Where(a => a.User_ID == friend.UserId).Select(u => u.Profile_Picture).FirstOrDefault());
        friend.OnOffImage = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
        var friendAccount = _context.Accounts.Where(a => a.User_ID == friend.UserId).FirstOrDefault();
        friend.Status = _context.AddRequests.Any(f => f.SentTo_User_ID == friend.UserId &&
           f.SentBy_User_ID == currentUser && f.Status == Resources.AcceptedStatus) ? friendAccount.Status : Resources.AddRequestPendingStatus;
      }

      friendList.IsExpanded = true;
      friendList.DisplayedName = string.Format("Friends ({0}/{1})", friendList.ContactsList.Where(c => c.OnOffImage.Contains("Online")).Count(), friendList.ContactsList.Count);
      ContactsLists.Add(friendList);
    }

    private void ChangeStatusCommandExecute()
    {
      //todo
    }

    private void InitializeContactList()
    {
      ContactsLists = new ObservableCollection<ContactListsModel>();
      LoadRecentList();
      LoadFriendList();
      LoadAddressBook();
    }

    private void AddFriendCommandExecute()
    {
      var addContactWindow = new Window();
      var addContactViewModel = new AddContactFirstViewModel(addContactWindow, SearchedUser);
      addContactWindow.Closed += new EventHandler(ReloadContactLists);
      WindowManager.CreateGeneralWindow(addContactWindow, addContactViewModel, Resources.AddContactWindowTitle, Resources.AddContactFirstControlPath);

      if (addContactViewModel.CloseAction == null)
      {
        addContactViewModel.CloseAction = () => addContactWindow.Close();
      }

      addContactWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
      addContactWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      addContactWindow.ShowDialog();
    }

    private void LoadAddressBook()
    {

      var currentUser = UserModel.Instance.Username;
      var addressBook = new ContactListsModel
      {
        ContactsList = new ObservableCollection<DisplayedContactModel>(_context.AddressBooks.
        Where(a => a.User_ID == UserModel.Instance.Username).ToList().OrderByDescending(a => a.Date).Select(a => new DisplayedContactModel { UserId = a.Friend_User_ID }))
      };

      foreach (var address in addressBook.ContactsList)
      {
        address.ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(
          _context.Accounts.Where(a => a.User_ID == address.UserId).Select(u => u.Profile_Picture).FirstOrDefault());
        address.OnOffImage = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
        var friendAccount = _context.Accounts.Where(a => a.User_ID == address.UserId).FirstOrDefault();
        address.Status = _context.AddRequests.Any(f => f.SentTo_User_ID == address.UserId &&
            f.SentBy_User_ID == currentUser && f.Status == Resources.AcceptedStatus) ? friendAccount.Status : Resources.AddRequestPendingStatus;
      }

      addressBook.IsExpanded = false;
      addressBook.DisplayedName = string.Format("Address Book ({0})", addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);
    }

    private void LoadRecentList()
    {
      var currentUser = UserModel.Instance.Username;
      var recentList = new ContactListsModel();
      recentList.ContactsList = new ObservableCollection<DisplayedContactModel>();
      var contacts = _context.RecentLists.Where(c => c.Sent_By == currentUser).ToList();
      contacts.Reverse();
      foreach (var contact in contacts)
      {
        var sent_to = _context.Accounts.Where(a => a.User_ID == contact.Sent_To).FirstOrDefault();
        var status = _context.AddRequests.Any(f => f.SentTo_User_ID == sent_to.User_ID &&
           f.SentBy_User_ID == currentUser && f.Status == Resources.AcceptedStatus) ? sent_to.Status : Resources.AddRequestPendingStatus;

        recentList.ContactsList.Add(new DisplayedContactModel
        {
          UserId = sent_to.User_ID,
          ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(sent_to.Profile_Picture),
          OnOffImage = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico",
          Status = status
        });
        recentList.DisplayedName = string.Format("Recent ({0}/{1})", recentList.ContactsList.Where(c => c.OnOffImage.Contains("Online")).Count(), recentList.ContactsList.Count);
      }
      recentList.Name = Resources.RecentListName;
      recentList.ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/RecentContact_Icon.png";
      recentList.DisplayedName = string.Format("Recent ({0}/{1})", recentList.ContactsList.Where(c => c.OnOffImage.Contains("Online")).Count(), recentList.ContactsList.Count);
      ContactsLists.Add(recentList);
    }

    private void RefreshProfilePicture(object sender, EventArgs e)
    {
      ProfilePicture = Converters.GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
    }

    private void LogoutCommandExecute()
    {
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);

      window.VerticalContentAlignment = VerticalAlignment.Top;
      foreach (Window win in App.Current.Windows)
      {
        if (win.Tag != null && win.Tag.ToString().EndsWith("Child"))
        {
          win.Close();
        }
      }
    }

    private BitmapImage GetProfilePicture(byte[] data)
    {
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.CacheOption = BitmapCacheOption.None;
        imageSource.StreamSource = memoryStream;
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.EndInit();
        return imageSource;
      }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
