using RM_Messenger.Helpers;
using RM_Messenger.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for LoginControl.xaml
  /// </summary>
  public partial class LoginControl : UserControl
  {
    bool predefinedChecks = false;
    public LoginControl()
    {
      InitializeComponent();
      VerticalAlignment = VerticalAlignment.Top;
      ExecutePredifinedChecks();
      Email.Focus();
    }

    private void ExecutePredifinedChecks()
    {
      bool rememberMyIDPassword = Convert.ToBoolean(AppConfigManager.Get(Properties.Resources.RememberMyIDPassword));
      if (rememberMyIDPassword && string.IsNullOrEmpty(Email.Text) && string.IsNullOrEmpty(Password.Password))
      {
        predefinedChecks = true;

        UserModel.Instance.Username = AppConfigManager.Get(Properties.Resources.Username);
        UserModel.Instance.EncryptedPassword = AppConfigManager.Get(Properties.Resources.EncryptedPassword);

        Email.Text = AppConfigManager.Get(Properties.Resources.Username);
        Password.Password = AppConfigManager.Get(Properties.Resources.EncryptedPassword);
      }
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
      if (!predefinedChecks)
      {
        UserModel.Instance.EncryptedPassword = Encryption.Sha256(Password.Password);
      }
      else
      {
        predefinedChecks = false;
      }
    }

    private void Click(object sender, RoutedEventArgs e)
    {
    }

    private void Email_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        Password.Focus();
      }
    }

    private void Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {

      SignInButton.IsDefault = true;
    }
  }
}
