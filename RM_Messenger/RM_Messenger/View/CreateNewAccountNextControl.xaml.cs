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
      FinishButton.IsEnabled = !string.IsNullOrEmpty(Password.Password) && Password.Password == RetypePassword.Password && !string.IsNullOrEmpty(UsernameTextBox.Text);

    }

    private void RetypePassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
      FinishButton.IsEnabled = !string.IsNullOrEmpty(Password.Password) && Password.Password == RetypePassword.Password && !string.IsNullOrEmpty(UsernameTextBox.Text);

    }

    private void Password_PasswordChanged(object sender, RoutedEventArgs e)
    {
      FinishButton.IsEnabled = !string.IsNullOrEmpty(Password.Password) && Password.Password == RetypePassword.Password && !string.IsNullOrEmpty(UsernameTextBox.Text);

    }

    private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if(string.IsNullOrEmpty(EmailTextBox.Text))
      {
        FinishButton.Content = "Finish";
      }
      else
      {
        FinishButton.Content = "Next";
      }
    }
  }
}
