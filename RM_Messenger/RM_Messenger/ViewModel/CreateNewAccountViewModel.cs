using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class CreateNewAccountViewModel : INotifyPropertyChanged
  {

    #region Private Fields
    private string _email;
    private Window window;
    private RMMessengerEntities _context;
    private string _passwordValidationMessage;
    #endregion

    #region Public Fields
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand CreateAccountCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand CreateNewAccountCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public Action CloseAction { get; set; }

    public string Email
    {
      get { return _email; }
      set
      {
        if (_email == value) return;
        _email = value;
        UserModel.Instance.Username = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
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
    public CreateNewAccountViewModel(Window window)
    {
      this.window = window;
      _context = new RMMessengerEntities();
      CreateAccountCommand = new RelayCommand(CreateAccountCommandExecute);
      ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }
    #endregion

    #region Private methods
    private void CancelCommandExecute()
    {
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    }

    private void ForgotPasswordCommandExecute()
    {
      // to do
    }

    private void CreateAccountCommandExecute()
    {
      if (_context.Users.Any(u => u.User_ID == UserModel.Instance.Username))
      {
        WindowManager.OpenLoginErrorWindow(window, "This ID is not available.\n");
        return;
      }

      string validationMessage = Validator.ValidateEmail(UserModel.Instance.Username);
      validationMessage += PasswordValidationMessage;

      if (!string.IsNullOrEmpty(validationMessage))
      {
        WindowManager.OpenLoginErrorWindow(window, validationMessage);
        return;
      }

      string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Resources\\OfflineProfilePicture.jpg";
      byte[] data = File.ReadAllBytes(path);
      UserModel.Instance.ProfilePicture = data;

      var user = new User
      {
        User_ID = UserModel.Instance.Username,
        Password = UserModel.Instance.EncryptedPassword
      };
      _context.Users.Add(user);
      var account = new Account
      {
        User_ID = UserModel.Instance.Username,
        Profile_Picture = UserModel.Instance.ProfilePicture
      };
      _context.Accounts.Add(account);
      _context.SaveChanges();

      WindowManager.OpenLoginErrorWindow(window, "Account successfully created");
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    }
    #endregion
  }
}
