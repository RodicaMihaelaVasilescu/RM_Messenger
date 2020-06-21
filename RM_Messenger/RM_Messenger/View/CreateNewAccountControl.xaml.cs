using RM_Messenger.Helpers;
using RM_Messenger.Model;
using System;
using System.Diagnostics;
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
      FirstNameTextBox.Focus();
      //PasswordValidationMessage.Text = Validator.ValidatePassword(Password.Password);
    }

    private void FirstName_PreviewKeyDown(object sender, KeyEventArgs e)
    {

      if (e.Key == Key.Enter)
      {
        LastNameTextBox.Focus();
      }
      if (!Char.IsLetter((char)e.Key)) e.Handled = false;
      //}

    }
    private void LastName_PreviewKeyDown(object sender, KeyEventArgs e)
    {

      if (e.Key == Key.Enter)
      {
        PostalCodeTextBox.Focus();
      }
      if (!Char.IsLetter((char)e.Key)) e.Handled = false;
      //}

    }

    private void ViewPolicyButton_Click(object sender, EventArgs e)
    {
      Process.Start("https://github.com/RodicaMihaelaVasilescu/RM_Messenger");
    }

  }
}
