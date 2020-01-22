using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class CreateNewAccountViewModel : INotifyPropertyChanged
  {
    private string _email;

    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand CreateAccountCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand CreateNewAccountCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public Action CloseAction { get; set; }
    private Window window;
    private RMMessengerEntities _context;
    private string _passwordValidationMessage;

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

    public CreateNewAccountViewModel(Window window)
    {
      this.window = window;
      _context = new RMMessengerEntities();
      CreateAccountCommand = new RelayCommand(CreateAccountCommandExecute);
      ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }

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
        User_ID =  UserModel.Instance.Username,
        Password = UserModel.Instance.EncryptedPassword,
        ProfilePicture = UserModel.Instance.ProfilePicture,
      };
      _context.Users.Add(user);
      _context.SaveChanges();

      WindowManager.OpenLoginErrorWindow(window, "Account successfully created");
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    }
  }
}
