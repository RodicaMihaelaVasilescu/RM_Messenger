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
    public string OnlineIcoPath { get; set; } = UserModel.Instance.IsOnline ? "pack://application:,,,/RM_Messenger;component/Resources/Online.ico" : "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";

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

    #endregion

    #region Constructor

    public HomepageViewModel(Window window)
    {
      this.window = window;
      window.Activated += new EventHandler(windowActivated);
      LogoutCommand = new RelayCommand(LogoutCommandExecute);
      InitializeUserProfile();
      InitializeContactList();
    }

    private void InitializeContactList()
    {
      _context = new RMMessengerEntities();
      ContactsLists = new List<ContactListsModel>();
      AddFriendCommand = new RelayCommand(AddFriendCommandExecute);
      LoadRecentList();
      LoadFriendList();
      LoadAddressBook();
    }

    private void LoadFriendList()
    {
      var currentUser = UserModel.Instance.Username;
      var friendsList = new ContactListsModel();
      // to do
      friendsList.ContactsList = new List<DisplayedContactModel>();
      friendsList.ListName = string.Format("Friends ({0}/{1})", friendsList.ContactsList.Where(c => c.OnlineIcoPath.Contains("Online")).Count(), friendsList.ContactsList.Count);
      ContactsLists.Add(friendsList);
    }

    private void AddFriendCommandExecute()
    {
      var addContactViewModel = new AddContactModel();
      var errorWindow = new Window();
      WindowManager.CreateErrorWindow(errorWindow, addContactViewModel, Resources.AddContactWindowTitle, Resources.AddContactControlPath);

      if (addContactViewModel.CloseAction == null)
      {
        addContactViewModel.CloseAction = () => errorWindow.Close();
      }

      errorWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
      errorWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      errorWindow.ShowDialog();

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

    private void windowActivated(object sender, EventArgs e)
    {
      ProfilePicture =Converters.GeneralConverters.ConvertToBitmapImage( UserModel.Instance.ProfilePicture);
    }

    private void InitializeUserProfile()
    {
      Email = UserModel.Instance.Username;
      ProfilePicture = GetProfilePicture(UserModel.Instance.ProfilePicture);
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
