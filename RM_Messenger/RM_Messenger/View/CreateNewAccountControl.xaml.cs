using RM_Messenger.Helpers;
using RM_Messenger.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for CreateNewAccountControl.xaml
  /// </summary>
  public partial class CreateNewAccountControl : UserControl
  {
    public CreateNewAccountControl()
    {
      InitializeComponent();
      PasswordValidationMessage.Text = Validator.ValidatePassword(Password.Password);
    }
    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
      UserModel.Instance.EncryptedPassword = Encryption.Sha256(Password.Password);
      PasswordValidationMessage.Text = Validator.ValidatePassword(Password.Password);
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
      CreateAccountButton.IsDefault = true;
    }
  }
}
