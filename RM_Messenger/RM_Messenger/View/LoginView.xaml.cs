using RM_Messenger.Helper;
using RM_Messenger.Helpers;
using RM_Messenger.ViewModel;
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
using System.Windows.Shapes;

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
