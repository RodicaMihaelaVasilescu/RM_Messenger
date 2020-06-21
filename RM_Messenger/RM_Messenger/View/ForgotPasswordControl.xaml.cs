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
  /// Interaction logic for PasswordRecoveryControl.xaml
  /// </summary>
  public partial class ForgotPasswordControl : UserControl
  {
    public ForgotPasswordControl()
    {
      InitializeComponent();
      FinishButton.IsEnabled = false;
    }

    private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      FinishButton.IsEnabled = IsFinishButtonEnabled();
    }

    private bool IsFinishButtonEnabled()
    {
      return !string.IsNullOrEmpty(Username.Text);
    }
  }
}