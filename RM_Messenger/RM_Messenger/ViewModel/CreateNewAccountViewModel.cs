using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class CreateNewAccountViewModel : INotifyPropertyChanged
  {

    #region Private Fields
    private string _firstName;
    private Window window;

    public UserModel NewUser;

    private RMMessengerEntities _context;
    private string _passwordValidationMessage;
    private string _lastName;
    private string _postalCode;
    private string _gender;
    private bool _isNextButtonEnabled;
    #endregion

    #region Public Fields
    public event PropertyChangedEventHandler PropertyChanged;

    //public ICommand CreateAccountCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand CreateNewAccountCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public Action CloseAction { get; set; }

    public string FirstName
    {
      get { return _firstName; }
      set
      {
        if (_firstName == value) return;
        _firstName = new String(value.Where(Char.IsLetter).ToArray());
        IsNextButtonEnabled = !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstName"));
      }
    }
    public string LastName
    {
      get { return _lastName; }
      set
      {
        if (_lastName == value) return;
        _lastName = new String(value.Where(Char.IsLetter).ToArray());
        IsNextButtonEnabled = !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastName"));
      }
    }

    public string PostalCode
    {
      get { return _postalCode; }
      set
      {
        if (_postalCode == value) return;
        _postalCode = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }
    public string Gender
    {
      get { return _gender; }
      set
      {
        if (_gender == value) return;
        _gender = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gender"));
      }
    }

    public bool IsNextButtonEnabled
    {
      get { return _isNextButtonEnabled; }
      set
      {
        if (_isNextButtonEnabled == value) return;
        _isNextButtonEnabled = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNextButtonEnabled"));
      }
    }

    public string PasswordValidationMessage
    {
      get { return _passwordValidationMessage; }
      set
      {
        if (_passwordValidationMessage == value) return;
        _passwordValidationMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PasswordValidationMessage"));
      }
    }

    #endregion

    #region Constructor
    public CreateNewAccountViewModel(Window window, UserModel newUser = null)
    {
      this.window = window;
      NewUser = newUser;
      if (NewUser != null)
      {
        FirstName = newUser.FirstName;
        LastName = NewUser.LastName;
        PostalCode = newUser.PostalCode;
      }
      _context = new RMMessengerEntities();
      //CreateAccountCommand = new RelayCommand(CreateAccountCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }
    #endregion

    #region Private methods
    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void ForgotPasswordCommandExecute()
    {
      // to do
    }

    private void NextCommandExecute()
    {
      NewUser = new UserModel
      {
        FirstName = FirstName,
        LastName = LastName,
        PostalCode = PostalCode
      };

      var createNewAccountNextViewModel = new CreateNewAccountNextViewModel(window, NewUser);
      WindowManager.ChangeWindowContent(window, createNewAccountNextViewModel, Resources.CreateNewAccountWindowTitle, Resources.CreateNewAccountNextControl);
      if (createNewAccountNextViewModel.CloseAction == null)
      {
        createNewAccountNextViewModel.CloseAction = () => window.Close();
      }

    }

    //private void CreateAccountCommandExecute()
    //{
    //  if (_context.Users.Any(u => u.User_ID == UserModel.Instance.Username))
    //  {
    //    WindowManager.OpenLoginErrorWindow(window, "This ID is not available.\n");
    //    return;
    //  }

    //  string validationMessage = Validator.ValidateUsername(UserModel.Instance.Username);
    //  validationMessage += PasswordValidationMessage;

    //  if (!string.IsNullOrEmpty(validationMessage))
    //  {
    //    WindowManager.OpenLoginErrorWindow(window, validationMessage);
    //    return;
    //  }

    //  string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Resources\\OfflineProfilePicture.jpg";
    //  byte[] data = File.ReadAllBytes(path);
    //  UserModel.Instance.ProfilePicture = data;

    //  var user = new User
    //  {
    //    User_ID = UserModel.Instance.Username,
    //    Password = UserModel.Instance.EncryptedPassword
    //  };
    //  _context.Users.Add(user);
    //  var account = new Account
    //  {
    //    User_ID = UserModel.Instance.Username,
    //    Profile_Picture = UserModel.Instance.ProfilePicture
    //  };
    //  _context.Accounts.Add(account);
    //  _context.SaveChanges();

    //  WindowManager.OpenLoginErrorWindow(window, "Account successfully created");
    //  var loginViewModel = new LoginViewModel(window);
    //  WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    //}
    #endregion
  }
}
