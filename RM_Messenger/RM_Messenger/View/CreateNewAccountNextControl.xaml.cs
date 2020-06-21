using RM_Messenger.Helpers;
using RM_Messenger.Model;
using System.Windows;
using System.Windows.Controls;

namespace RM_Messenger.View
{
  /// <summary>
  /// Interaction logic for CreateNewAccountNextControl.xaml
  /// </summary>
  public partial class CreateNewAccountNextControl : UserControl
  {
    public CreateNewAccountNextControl()
    {
      InitializeComponent();
      FinishButton.IsEnabled = false;
      UsernameTextBox.Focus();
    }

    private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      FinishButton.IsEnabled = IsFinishButtonEnabled();
    }

    private void RetypePassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
      FinishButton.IsEnabled = IsFinishButtonEnabled();
    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
      FinishButton.IsEnabled = IsFinishButtonEnabled();
      UserModel.Instance.EncryptedPassword = Encryption.Sha256(Password.Password);
      PasswordValidationMessage.Text = Validator.ValidatePassword(Password.Password);
    }

    private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      FinishButton.Content = string.IsNullOrEmpty(EmailTextBox.Text) ?
        Properties.Resources.Finish : Properties.Resources.Next + " >";
    }

    private bool IsFinishButtonEnabled()
    {
      return !string.IsNullOrEmpty(Password.Password) &&
        Password.Password == RetypePassword.Password &&
        !string.IsNullOrEmpty(UsernameTextBox.Text);
    }
  }
}
