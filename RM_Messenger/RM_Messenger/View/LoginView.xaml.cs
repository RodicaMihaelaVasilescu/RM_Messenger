using RM_Messenger.Helpers;
using RM_Messenger.ViewModel;
using System;
using System.Windows;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for LoginView.xaml
  /// </summary>
  public partial class LoginView : Window
  {
    public LoginView()
    {
      InitializeComponent();
      var viewModel = new LoginViewModel(this);
      DataContext = viewModel;
      WindowManager.ResizeWindow(this);

      bool signInAutomatically = Convert.ToBoolean(AppConfigManager.Get(Properties.Resources.SignInAutomatically));
      bool hasPreviousCredentials = AppConfigManager.Get(Properties.Resources.Username) != string.Empty
        && AppConfigManager.Get(Properties.Resources.EncryptedPassword) != string.Empty;

      if (signInAutomatically && hasPreviousCredentials)
      {
        viewModel.LoginCommandExecute();
      }
    }
  }
}
