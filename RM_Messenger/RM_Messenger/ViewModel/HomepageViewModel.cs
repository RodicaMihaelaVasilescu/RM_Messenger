using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.Generic;
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
    public string OnOffImage { get; set; } = UserModel.Instance.IsOnline ? "pack://application:,,,/RM_Messenger;component/Resources/Online.ico" : "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";

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
    private Window window;
    private BitmapImage profilePicture;
    private List<ContactListsModel> _contactsLists;
    private RMMessengerEntities _context;
    private string _status;
    private string _searchedUser;

    #endregion

    #region Constructor

    public HomepageViewModel(Window window)
    {
      this.window = window;
      window.Activated += new EventHandler(RefreshProfilePicture);
      LogoutCommand = new RelayCommand(LogoutCommandExecute);
      ChangeStatusCommand = new RelayCommand(ChangeStatusCommandExecute);

      InitializeUserProfile();
      InitializeContactList();
    }

    private void ChangeStatusCommandExecute()
    {

    }

    private void InitializeContactList()
    {
      _context = new RMMessengerEntities();
      ContactsLists = new List<ContactListsModel>();
      AddFriendCommand = new RelayCommand(AddFriendCommandExecute);
      LoadContactLists();
    }

    private void LoadContactLists()
    {
      ContactsLists = new List<ContactListsModel>();
      LoadRecentList();
      LoadFriendList();
      LoadAddressBook();
    }

    private void LoadFriendList()
    {
      var currentUser = UserModel.Instance.Username;
      var friendList = new ContactListsModel
      {
        ContactsList = new List<DisplayedContactModel>(_context.Friendships.
        Where(a => a.User_ID == UserModel.Instance.Username).ToList().OrderByDescending(a => a.Date).Select(a => new DisplayedContactModel { UserId = a.Friend_ID }))
      };

      foreach (var friend in friendList.ContactsList)
      {
        var friendAccount = _context.Accounts.Where(a => a.User_ID == friend.UserId).FirstOrDefault();
        friend.ImagePath = Converters.GeneralConverters.ConvertToBitmapImage(friendAccount.Profile_Picture);
        friend.OnOffImage = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
        friend.Status = _context.AddRequests.Any(f => f.SentTo_User_ID == friend.UserId &&
           f.SentBy_User_ID == currentUser && f.Status == Resources.AcceptedStatus) ? friendAccount.Status : Resources.AddRequestPendingStatus;
      }

      friendList.IsExpanded = true;
      friendList.ListName = string.Format("Friends ({0}/{1})", friendList.ContactsList.Where(c => c.OnOffImage.Contains("Online")).Count(), friendList.ContactsList.Count);
      ContactsLists.Add(friendList);
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
        ContactsList = new List<DisplayedContactModel>(_context.AddressBooks.
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

      addressBook.ListName = string.Format("Address Book ({0})", addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);

    }

    private void LoadRecentList()
    {
      var currentUser = UserModel.Instance.Username;
      var recentList = new ContactListsModel();
      // to do
      recentList.ContactsList = new List<DisplayedContactModel>();
      recentList.ImagePath = "pack://application:,,,/RM_Messenger;component/Resources/RecentContact_Icon.png";
      recentList.ListName = string.Format("Recent ({0}/{1})", recentList.ContactsList.Where(c => c.OnOffImage.Contains("Online")).Count(), recentList.ContactsList.Count);
      ContactsLists.Add(recentList);
    }

    private void RefreshProfilePicture(object sender, EventArgs e)
    {
      ProfilePicture = Converters.GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
    }
    public void ReloadContactLists(object sender, EventArgs e)
    {
      LoadContactLists();
    }
    private void InitializeUserProfile()
    {
      Email = UserModel.Instance.Username;
      ProfilePicture = GetProfilePicture(UserModel.Instance.ProfilePicture);
      Status = UserModel.Instance.Status;
    }
    #endregion

    #region Private Methods

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
