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
  class ForgotPasswordViewModel : INotifyPropertyChanged
  {
    public Action CloseAction { get; set; }

    private Window window;
    private RMMessengerEntities _context;
    private string _username;

    public ICommand CancelCommand { get; set; }
    public ICommand NextCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

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
    public ForgotPasswordViewModel(Window window, string username = null)
    {
      this.window = window;
      Username = username;
      CancelCommand = new RelayCommand(CancelCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      _context = new RMMessengerEntities();
    }

    private void NextCommandExecute()
    {
      var user = _context.Accounts.Where(u => u.User_ID == Username).FirstOrDefault();
      if (user == null)
      {
        WindowManager.OpenLoginErrorWindow(window, "The user you have entered does not exist.", false);
        return;
      }
      var userModel = new UserModel
      {
        Username = user.User_ID,
        Email = user.Email
      };
      if (!string.IsNullOrEmpty(user.Email))
      {
        CheckEmail(userModel);
      }
      else
      {
        WindowManager.OpenLoginErrorWindow(window, "Sorry, you don't have an email in order to recover your password.", false);
      }
    }


    private void CheckEmail(UserModel user)
    {
      var confirmationCode = new Random().Next(1000, 9999);

      if (!SendEmail.SendEmailExecute(user.Email, "RM! Messenger - Password Recovery", string.Format("We received your request to change your password.\n\nThe validation code is {0}.\n\nIf it wasn't you, please disregard this email.", confirmationCode)))
      {
        WindowManager.OpenLoginErrorWindow(window, "The email is not valid.", false);
        return;
      }
      else
      {
        var newEmailConfirmation = new EmailConfirmation
        {
          User_ID = user.Username,
          Code = confirmationCode,
          IsConfirmed = false
        };
        var emailConfirmation = _context.EmailConfirmations.FirstOrDefault(u => u.User_ID == user.Username);
        if (emailConfirmation == null)
        {
          _context.EmailConfirmations.Add(newEmailConfirmation);
        }
        else
        {
          emailConfirmation.Code = confirmationCode;
          emailConfirmation.IsConfirmed = false;
        }
        _context.SaveChanges();
        var emailConfirmationCodeViewModel = new EmailConfirmationCodeViewModel(window, user);
        WindowManager.ChangeWindowContent(window, emailConfirmationCodeViewModel, Resources.ForgotPasswordControlTitle, Resources.EmailConfirmationCodeControlPath);
        if (emailConfirmationCodeViewModel.CloseAction == null)
        {
          emailConfirmationCodeViewModel.CloseAction = () => window.Close();
        }
      }
    }


    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }
  }
}
