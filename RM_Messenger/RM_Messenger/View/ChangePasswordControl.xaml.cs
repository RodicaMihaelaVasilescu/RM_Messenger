using RM_Messenger.Helpers;
using RM_Messenger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
  /// Interaction logic for ForgotPasswordNextControl.xaml
  /// </summary>
  public partial class ChangePasswordControl : UserControl
  {
    public ChangePasswordControl()
    {
      InitializeComponent();
      NextButton.IsEnabled = false;
    }
    private void RetypePassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
      NextButton.IsEnabled = IsFinishButtonEnabled();
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
      NextButton.IsEnabled = IsFinishButtonEnabled();
      UserModel.Instance.NewEncryptedPassword = Encryption.Sha256(Password.Password);
      PasswordValidationMessage.Text = Validator.ValidatePassword(Password.Password);
    }

    private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      NextButton.Content = string.IsNullOrEmpty(Username.Text) ?
        Properties.Resources.Finish : Properties.Resources.Next + " >";
    }

    private bool IsFinishButtonEnabled()
    {
      return !string.IsNullOrEmpty(Password.Password) &&
        Password.Password == RetypePassword.Password;
    }
  }
}