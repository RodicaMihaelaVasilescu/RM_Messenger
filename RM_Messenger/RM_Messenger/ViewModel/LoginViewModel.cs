using System;
using System.ComponentModel;
using RM_Messenger.Model;
using System.Windows;
using System.Windows.Input;
using RM_Messenger.Helper;
using RM_Messenger.Properties;
using RM_Messenger.Command;
using RM_Messenger.Helpers;
using RM_Messenger.Database;
using System.Linq;
using RM_Messenger.Constants;

namespace RM_Messenger.ViewModel
{
  class LoginViewModel : INotifyPropertyChanged
  {
    #region Public Properties

    public ICommand LoginCommand { get; set; }
    public ICommand RegisterCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public Action CloseAction { get; set; }

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

    public bool RememberMyIDPassword
    {
      get { return _rememberMyIDPassword; }
      set
      {
        if (_rememberMyIDPassword == value) return;
        _rememberMyIDPassword = value;
        AppConfigManager.Set(LoginConstants.RememberMyIDPassword, value.ToString());
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RememberMyIDPassword"));
      }
    }
    public bool SignInAutomatically
    {
      get { return _signInAutomatically; }
      set
      {
        if (_signInAutomatically == value) return;
        _signInAutomatically = value;
        AppConfigManager.Set(LoginConstants.SignInAutomatically, value.ToString());
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SignInAutomatically"));
      }
    }

    public bool SignInAsInvisible
    {
      get { return _signInAsInvisible; }
      set
      {
        _signInAsInvisible = value;
        UserModel.Instance.IsOnline = !value;
        AppConfigManager.Set(LoginConstants.SignInAsInvisible, value.ToString());
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SignInAsInvisible"));
      }
    }

    #endregion

    #region Private Properties

    private string _email;
    private RMMessengerEntities _context;
    private Window window;

    private bool _rememberMyIDPassword = Convert.ToBoolean(AppConfigManager.Get(LoginConstants.RememberMyIDPassword));
    private bool _signInAutomatically = Convert.ToBoolean(AppConfigManager.Get(LoginConstants.SignInAutomatically));
    private bool _signInAsInvisible = Convert.ToBoolean(AppConfigManager.Get(LoginConstants.SignInAsInvisible));

    #endregion

    #region Constructor
    public LoginViewModel(Window window)
    {
      _context = new RMMessengerEntities();
      this.window = window;
      UserModel.Instance.Username = string.IsNullOrEmpty(Email) ? string.Empty : Email.Split('@')[0]; ;

      LoginCommand = new RelayCommand(LoginCommandExecute);
      RegisterCommand = new RelayCommand(RegisterCommandExecute);
      ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
      bool rememberMyIDPassword = Convert.ToBoolean(AppConfigManager.Get(Properties.Resources.RememberMyIDPassword));
      if (rememberMyIDPassword)
      {
        Email = AppConfigManager.Get(Properties.Resources.Username);
        UserModel.Instance.Username = AppConfigManager.Get(Properties.Resources.Username);
        UserModel.Instance.EncryptedPassword = AppConfigManager.Get(Properties.Resources.EncryptedPassword);
      }
    }

    #endregion

    #region Private Methods

    public void LoginCommandExecute()
    {
      if (Email == string.Empty || UserModel.Instance.EncryptedPassword == string.Empty)
      {
        WindowManager.OpenLoginErrorWindow(window, Resources.YouMustEnterAnIDAndPasswordError);
        return;
      }

      UserModel.Instance.Username = Email;
      if (RememberMyIDPassword)
      {
        AppConfigManager.Set(LoginConstants.Username, UserModel.Instance.Username);
        //UserModel.Instance.EncryptedPassword = Email;
        AppConfigManager.Set(LoginConstants.EncryptedPassword, UserModel.Instance.EncryptedPassword);
      }

      OpenSigningInWindow();
    }

    private void OpenSigningInWindow()
    {
      var signingInViewModel = new SigningInViewModel(window);
      WindowManager.ChangeWindowContent(window, signingInViewModel, Resources.SigningInWindowTitle, Resources.SigningInControlPath);

      if (signingInViewModel.CloseAction == null)
      {
        signingInViewModel.CloseAction = () => window.Close();
      }

      WindowManager.ResizeWindow(window);
      window.ResizeMode = ResizeMode.NoResize;
      signingInViewModel.LoadWindow();
    }

    private void RegisterCommandExecute()
    {
      //to do
    }

    public void ForgotPasswordCommandExecute()
    {
      //to do
    }
    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
