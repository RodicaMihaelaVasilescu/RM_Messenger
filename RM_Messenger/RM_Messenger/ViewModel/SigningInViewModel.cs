using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class SigningInViewModel : INotifyPropertyChanged
  {
    public Action CloseAction { get; set; }
    public ICommand CancelCommand { get; set; }
    Window window;
    private bool cancelButtonPressed;
    private string messageOnSingingIn;

    public event PropertyChangedEventHandler PropertyChanged;

    public string MessageOnSingingIn
    {
      get { return messageOnSingingIn; }
      set
      {
        if (messageOnSingingIn == value) return;
        messageOnSingingIn = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageOnSingingIn"));
      }
    }

    public SigningInViewModel(Window window)
    {
      var email = string.IsNullOrEmpty(UserModel.Instance.Username) ? string.Empty : UserModel.Instance.Username.Split('@')[0];
      MessageOnSingingIn = "Signing in as " + email ;
      this.window = window;
      cancelButtonPressed = false;
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }

    private void CancelCommandExecute()
    {
      cancelButtonPressed = true;
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    }

    public async void LoadWindow()
    {
      await Task.Delay(5000);
      //SwitchToLoginindow();
      if (cancelButtonPressed)
      {
        //SwitchToLoginindow();
        return;
      }
      var _context = new RMMessengerEntities();
      if (!_context.Users.Any(u => u.Username == UserModel.Instance.Username &&
   u.Password == UserModel.Instance.EncryptedPassword))
      {
        CancelCommandExecute();
       WindowManager.OpenLoginErrorWindow(window,Resources.IncorrectIDAndPassword);
        return;
      }

      var homepageViewModel = new HomepageViewModel(window);
      WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);

      if (homepageViewModel.CloseAction == null)
      {
        homepageViewModel.CloseAction = () => window.Close();
      }

      WindowManager.ResizeWindow(window);
      window.ResizeMode = ResizeMode.NoResize;
    }
  }
}
