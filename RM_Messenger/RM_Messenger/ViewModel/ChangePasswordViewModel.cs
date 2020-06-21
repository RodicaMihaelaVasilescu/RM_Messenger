using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
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
  class ChangePasswordViewModel : INotifyPropertyChanged
  {
    private string _username;
    private Window window;
    private UserModel user;
    private RMMessengerEntities _context;
    private string _passwordValidationMessage;

    public string Username
    {
      get { return _username; }
      set
      {
        if (_username == value) return;
        _username = value.ToLower();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private string _successfulConfirmationMessage = Resources.PasswordSuccessfullyChanged;
    public Action CloseAction { get; set; }
    public ICommand BackCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand NextCommand { get; set; }

    public string PasswordValidationMessage
    {
      get { return _passwordValidationMessage; }
      set
      {
        if (_passwordValidationMessage == value) return;
        _passwordValidationMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PasswordValidationMessage"));
      }
    }

    public string SuccessfulConfirmationMessage
    {
      get { return _successfulConfirmationMessage; }
      set
      {
        if (_successfulConfirmationMessage == value) return;
        _successfulConfirmationMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SuccessfulConfirmationMessage"));
      }
    }

    public ChangePasswordViewModel(Window window, UserModel user)
    {
      this.window = window;
      this.user = user;
      _context = new RMMessengerEntities();
      BackCommand = new RelayCommand(BackCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      Username = user.Username;

    }

    private void NextCommandExecute()
    {

      if (!string.IsNullOrEmpty(PasswordValidationMessage))
      {
        WindowManager.OpenLoginErrorWindow(window, PasswordValidationMessage, true);
        return;
      }

      var databaseUser = _context.Users.Where(u => u.User_ID == user.Username).FirstOrDefault();
      databaseUser.Password = UserModel.Instance.NewEncryptedPassword;
      _context.SaveChanges();
      WindowManager.ChangeWindowContent(window, this, Resources.EmailConfirmationCodeFinishedControlTitle, Resources.EmailConfirmationCodeFinishedControlPath);

    }

    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void BackCommandExecute()
    {
    }
  }
}
