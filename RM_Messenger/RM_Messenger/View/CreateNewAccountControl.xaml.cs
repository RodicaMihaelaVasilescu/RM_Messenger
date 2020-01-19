using RM_Messenger.Helper;
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
