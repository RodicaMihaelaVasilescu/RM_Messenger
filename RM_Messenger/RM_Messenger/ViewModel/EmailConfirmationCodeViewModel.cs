using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class EmailConfirmationCodeViewModel : INotifyPropertyChanged
  {
    #region Private Properties
    private string _verificationCode;
    private string _verificationCodeMessage;
    private Window window;
    private UserModel user;
    private RMMessengerEntities _context;
    private string _displayedMailMessage;
    #endregion

    #region Properties

    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    private string _successfulConfirmationMessage = Resources.AccountSuccessfullyCreated;

    public ICommand BackCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand SendAnotherVerificationCodeCommand { get; set; }

    public string VerificationCode
    {
      get { return _verificationCode; }
      set
      {
        if (_verificationCode == value) return;
        _verificationCode = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VerificationCode"));
      }
    }

    public string VerificationCodeMessage
    {
      get { return _verificationCodeMessage; }
      set
      {
        if (_verificationCodeMessage == value) return;
        _verificationCodeMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VerificationCodeMessage"));
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

    public string DisplayedMailMessage
    {
      get { return _displayedMailMessage; }
      set
      {
        if (_displayedMailMessage == value) return;
        _displayedMailMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayedMailMessage"));
      }
    }


    #endregion

    #region Constructor
    public EmailConfirmationCodeViewModel(Window window, UserModel user = null)
    {
      this.window = window;
      this.user = user;
      _context = new RMMessengerEntities();
      BackCommand = new RelayCommand(BackCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CloseCommandExecute);
      SendAnotherVerificationCodeCommand = new RelayCommand(SendAnotherVerificationCodeExecute);
      _displayedMailMessage = string.Format("We've sent you a verification code on {0}. Please type the code you received:", user.Email);
    }

    private void BackCommandExecute()
    {
      if (window.Title == Resources.CreateNewAccountWindowTitle)
      {
        var createNewAccountNextViewModel = new CreateNewAccountNextViewModel(window, user);
        WindowManager.CreateGeneralWindow(window, createNewAccountNextViewModel, Resources.CreateNewAccountNextWindowTitle, Resources.CreateNewAccountNextControlPath);

        if (createNewAccountNextViewModel.CloseAction == null)
        {
          createNewAccountNextViewModel.CloseAction = () => window.Close();
        }
        return;
      }
      if (window.Title == Resources.ForgotPasswordControlTitle)
      {
        var forgotPasswordViewModel = new ForgotPasswordViewModel(window, user.Username);
        WindowManager.ChangeWindowContent(window, forgotPasswordViewModel, Resources.ForgotPasswordControlTitle, Resources.ForgotPasswordControlPath);

        if (forgotPasswordViewModel.CloseAction == null)
        {
          forgotPasswordViewModel.CloseAction = () => window.Close();
        }
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.Show();
      }
    }

    private void CloseCommandExecute()
    {
      CloseAction?.Invoke();
    }
    #endregion


    private void NextCommandExecute()
    {
      if (window.Title == Resources.ForgotPasswordControlTitle)
      {
        var confirmation = _context.EmailConfirmations.Where(e => e.User_ID == user.Username).FirstOrDefault();
        if (VerificationCode == confirmation.Code.ToString())
        {
          confirmation.IsConfirmed = true;
          var changePasswordViewModel = new ChangePasswordViewModel(window, user);
          WindowManager.ChangeWindowContent(window, changePasswordViewModel, Resources.ChangePasswordControlTitle, Resources.ChangePasswordControlPath);
          if (changePasswordViewModel.CloseAction == null)
          {
            changePasswordViewModel.CloseAction = () => window.Close();
          }
        }
        else
        {
          VerificationCodeMessage =Resources.EmailVerificationFailed;
        }
        return;
      }

      if (window.Title == Resources.CreateNewAccountWindowTitle)
      {
        var confirmation = _context.EmailConfirmations.Where(e => e.User_ID == user.Username).FirstOrDefault();
        if (VerificationCode == confirmation.Code.ToString())
        {
          confirmation.IsConfirmed = true;
          SaveAccountInDatabase();
          WindowManager.ChangeWindowContent(window, this, Resources.EmailConfirmationCodeFinishedControlTitle, Resources.EmailConfirmationCodeFinishedControlPath);
        }
        else
        {
          VerificationCodeMessage = Resources.EmailVerificationFailed;
        }
        return;
      }
    }

    private void SendAnotherVerificationCodeExecute()
    {
      var confirmationCode = new Random().Next(1000, 9999);

      if (!SendEmail.SendEmailExecute(user.Email, Resources.CreateNewAccountMailSubject, string.Format(Resources.CreateAccountMailBodyMessage, confirmationCode)))
      {
        WindowManager.OpenLoginErrorWindow(window, "The email is not valid.", false);
        return;
      }
      else
      {
        var newEmailConfirmation = new EmailConfirmation
        {
          User_ID = UserModel.Instance.Username,
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
      }
    }


    private void SaveAccountInDatabase()
    {
      string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Resources\\OfflineProfilePicture.jpg";
      byte[] data = File.ReadAllBytes(path);
      this.user.ProfilePicture = data;
      this.user.NewEncryptedPassword = UserModel.Instance.NewEncryptedPassword;

      var user = new User
      {
        User_ID = this.user.Username,
        Password = this.user.NewEncryptedPassword,
      };
      _context.Users.Add(user);

      var account = new Account
      {
        First_Name = this.user.FirstName,
        Last_Name = this.user.LastName,
        Email = this.user.Email,
        User_ID = this.user.Username,
        Profile_Picture = this.user.ProfilePicture,
        PostalCode = this.user.PostalCode
      };
      _context.Accounts.Add(account);
      _context.SaveChanges();
    }
  }
}
