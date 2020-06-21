using System;
using System.ComponentModel;
using RM_Messenger.Model;
using System.Windows;
using System.Windows.Input;
using RM_Messenger.Helpers;
using RM_Messenger.Properties;
using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.View;

namespace RM_Messenger.ViewModel
{
  class LoginViewModel : INotifyPropertyChanged
  {
    #region Public Properties

    public ICommand LoginCommand { get; set; }
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

    public bool RememberMyIDPassword
    {
      get { return _rememberMyIDPassword; }
      set
      {
        if (_rememberMyIDPassword == value) return;
        _rememberMyIDPassword = value;
        AppConfigManager.Set(Resources.RememberMyIDPassword, value.ToString());
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
        AppConfigManager.Set(Resources.SignInAutomatically, value.ToString());
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
        AppConfigManager.Set(Resources.SignInAsInvisible, value.ToString());
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SignInAsInvisible"));
      }
    }

    #endregion

    #region Private Properties

    private string _email;
    private readonly RMMessengerEntities _context;
    private Window window;
    private bool _rememberMyIDPassword = Convert.ToBoolean(AppConfigManager.Get(Resources.RememberMyIDPassword));
    private bool _signInAutomatically = Convert.ToBoolean(AppConfigManager.Get(Resources.SignInAutomatically));
    private bool _signInAsInvisible = Convert.ToBoolean(AppConfigManager.Get(Resources.SignInAsInvisible));

    #endregion

    #region Constructor
    public LoginViewModel(Window window)
    {
      _context = new RMMessengerEntities();
      this.window = window;
      LoginCommand = new RelayCommand(LoginCommandExecute);
      CreateNewAccountCommand = new RelayCommand(CreateNewAccountCommandExecute);
      ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
    }

    #endregion

    #region Private Methods

    public void LoginCommandExecute()
    {

      if (RememberMyIDPassword)
      {
        AppConfigManager.Set(Resources.Username, UserModel.Instance.Username);
        AppConfigManager.Set(Resources.EncryptedPassword, UserModel.Instance.EncryptedPassword);
      }

      if (String.IsNullOrEmpty(UserModel.Instance.EncryptedPassword) || String.IsNullOrEmpty(UserModel.Instance.EncryptedPassword))
      {
        WindowManager.OpenLoginErrorWindow(window, Resources.YouMustEnterAnIDAndPasswordError);
        return;
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

      signingInViewModel.ValidateLogin();
    }

    private void CreateNewAccountCommandExecute()
    {
      Window createNewAccountWindow = new Window();
      var createNewAccountViewModel = new CreateNewAccountViewModel(createNewAccountWindow);
      WindowManager.CreateGeneralWindow(createNewAccountWindow, createNewAccountViewModel, Resources.CreateNewAccountWindowTitle, Resources.CreateNewAccountControlPath);

      if (createNewAccountViewModel.CloseAction == null)
      {
        createNewAccountViewModel.CloseAction = () => createNewAccountWindow.Close();
      }
      createNewAccountWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      createNewAccountWindow.Show();
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
