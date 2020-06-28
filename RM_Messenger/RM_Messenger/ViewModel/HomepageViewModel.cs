using RM_Messenger.Command;
using RM_Messenger.Constants;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using RM_Messenger.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
    public ICommand SearchFriendCommand { get; set; }
    public ICommand ChangeStatusCommand { get; set; }
    public ICommand RefreshCommand { get; set; }
    public ICommand TextEditorCommand { get; set; }
    public ICommand ChangeColorCommand { get; set; }
    public string OnOffImage { get; set; } = UserModel.Instance.IsOnline ? "pack://application:,,,/RM_Messenger;component/Resources/Online.ico" : "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";

    private ObservableCollection<DisplayedContactModel> searchResults;

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

    public Brush BackgroundColor
    {
      get { return _backgroundColor; }
      set
      {
        if (_backgroundColor == value) return;
        _backgroundColor = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackgroundColor"));
      }
    }

    public Brush DarkBackgroundColor
    {
      get { return _darkBackgroundColor; }
      set
      {
        if (_darkBackgroundColor == value) return;
        _darkBackgroundColor = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DarkBackgroundColor"));
      }
    }


    public Color DockpanelGradientColor1
    {
      get { return _dockpanelGradientColor1; }
      set
      {
        if (_dockpanelGradientColor1 == value) return;
        _dockpanelGradientColor1 = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DockpanelGradientColor1"));
      }
    }

    public Color DockpanelGradientColor2
    {
      get { return _dockpanelGradientColor2; }
      set
      {
        if (_dockpanelGradientColor2 == value) return;
        _dockpanelGradientColor2 = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DockpanelGradientColor2"));
      }
    }
    public Color DarkColorGradient
    {
      get { return _darkColorGradient; }
      set
      {
        if (_darkColorGradient == value) return;
        _darkColorGradient = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DarkColorGradient"));
      }
    }

    public Color LightColorGradient
    {
      get { return _lightColorGradient; }
      set
      {
        if (_lightColorGradient == value) return;
        _lightColorGradient = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LightColorGradient"));
      }
    }


    public ObservableCollection<String> ThemeColors
    {
      get { return _themeColors; }
      set
      {
        if (_themeColors == value) return;
        _themeColors = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ThemeColors"));
      }
    }

    public String SelectedThemeColor
    {
      get { return _selectedThemeColor; }
      set
      {
        if (_selectedThemeColor == value) return;
        _selectedThemeColor = value;
        ChangeColorCommandExecute();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedThemeColor"));
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
        if (string.IsNullOrEmpty(value))
        {
          ContactsLists = AllContactsLists;
        }
        else
        {
          searchResults = new ObservableCollection<DisplayedContactModel>
            (AddressBookList.Where(a => a.UserId.ToLower().StartsWith(value.ToLower())).OrderBy(u => u.UserId));
          ContactsLists = new ObservableCollection<ContactListsModel>();
          var searchListModel = new ContactListsModel
          {
            IsExpanded = true,
            ContactsList = searchResults,
            ArrowVisibility = Visibility.Collapsed
          };
          ContactsLists.Add(searchListModel);
        }
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

    public ObservableCollection<DisplayedContactModel> AddressBookList { get; private set; }
    public ObservableCollection<ContactListsModel> AllContactsLists { get; private set; }

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
    private ObservableCollection<string> _themeColors;
    private string _selectedThemeColor;
    private Brush _backgroundColor = ThemeColorsHelper.GetThemeColors().BackgroundColor;
    private Color _dockpanelGradientColor1 = ThemeColorsHelper.GetThemeColors().DockpanelGradientColor1;
    private Color _dockpanelGradientColor2 = ThemeColorsHelper.GetThemeColors().DockpanelGradientColor2;
    private Color _darkColorGradient = ThemeColorsHelper.GetThemeColors().DarkColorGradient;
    private Color _lightColorGradient = ThemeColorsHelper.GetThemeColors().LightColorGradient;
    private Brush _darkBackgroundColor;

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
      SearchFriendCommand = new RelayCommand(SearchFriendCommandExecute);
      RefreshCommand = new RelayCommand(RefreshCommandExecute);
      TextEditorCommand = new RelayCommand(TextEditorCommandExecute);
      ChangeColorCommand = new RelayCommand(ChangeColorCommandExecute);

      InitializeUserProfile();
      InitializeContactList();
      InitializeColors();
      RegisterSqlDependency();
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

    void TextEditorCommandExecute()
    {
      var textEditorWindow = new TextEditorView();
      textEditorWindow.Show();
    }

    void InitializeColors()
    {
      ThemeColors = new ObservableCollection<string>(Constants.Colors.ColorsList);
      string themeColor = AppConfigManager.Get(Resources.ThemeColorProperty);
      SelectedThemeColor = ThemeColors.FirstOrDefault(c => c == themeColor);
      if (string.IsNullOrEmpty(SelectedThemeColor))
      {
        SelectedThemeColor = ThemeColors.FirstOrDefault();
      }
    }

    void RegisterSqlDependency()
    {
      string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
      SqlConnection connection = new SqlConnection(SqlConnectionString);

      SqlDependency.Start(SqlConnectionString);
      connection.Open();
      var query = string.Format("SELECT SentBy_User_ID, SentTo_User_ID, Status, Date FROM dbo.AddRequests where SentTo_User_ID = '{0}'", UserModel.Instance.Username);
      // Create a new SqlCommand object.
      using (SqlCommand command = new SqlCommand(query
         , connection))
      {
        // Create a dependency and associate it with the SqlCommand.
        SqlDependency dependency = new SqlDependency(command);
        // Maintain the reference in a class member.

        // Subscribe to the SqlDependency event.
        dependency.OnChange += new OnChangeEventHandler(OnAddRequestDependencyChange);

        // Execute the command.

        using (SqlDataReader reader = command.ExecuteReader())
        {
          // Process the DataReader.
        }
      }
      connection.Close();
    }

    // Handler method
    void OnAddRequestDependencyChange(object sender,
       SqlNotificationEventArgs e)
    {
      RegisterSqlDependency();

      var currentUser = UserModel.Instance.Username;
      App.Current.Dispatcher.Invoke(() =>
      {
        OpenAddRequests();
      });
    }

    private void ChangeColorCommandExecute()
    {
      var colors = ThemeColorsHelper.GetThemeColors(SelectedThemeColor);
      BackgroundColor = colors.BackgroundColor;
      DockpanelGradientColor1 = colors.DockpanelGradientColor1;
      DockpanelGradientColor2 = colors.DockpanelGradientColor2;
      DarkColorGradient = colors.DarkColorGradient;
      LightColorGradient = colors.LightColorGradient;
      DarkBackgroundColor = colors.DarkBackgroundColor;
      AppConfigManager.Set(Resources.ThemeColorProperty, SelectedThemeColor);
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
        if (addRequestWindow.Left < 0)
        {
          addRequestWindow.Left = 0;
        }
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
        friend.Status = friendAccount.Status;
      }

      friendList.IsExpanded = friendList.ContactsList.Any();
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
      AllContactsLists = new ObservableCollection<ContactListsModel>();
      AllContactsLists = ContactsLists;
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

    private void SearchFriendCommandExecute()
    {
      if (!searchResults.Any())
      {
        AddFriendCommandExecute();
      }
      else
      {
        var chatControl = WindowManager.OpenChatWindow(searchResults.FirstOrDefault());
        ContactsLists = AllContactsLists;
      }
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
        address.Status = friendAccount.Status;
      }

      addressBook.IsExpanded = false;
      addressBook.DisplayedName = string.Format("Address Book ({0})", addressBook.ContactsList.Count);
      ContactsLists.Add(addressBook);
      AddressBookList = addressBook.ContactsList;
    }

    private void LoadRecentList()
    {
      var currentUser = UserModel.Instance.Username;
      var recentList = new ContactListsModel();
      recentList.ContactsList = new ObservableCollection<DisplayedContactModel>();
      var contacts = _context.RecentLists.Where(c => c.Sent_By == currentUser).OrderByDescending(c => c.Date).Select(s => s.Sent_To).ToList();
      contacts = contacts.Distinct().ToList();
      contacts.Reverse();
      foreach (var contact in contacts)
      {
        var sent_to = _context.Accounts.Where(a => a.User_ID == contact).FirstOrDefault();
        var status = sent_to.Status;

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
