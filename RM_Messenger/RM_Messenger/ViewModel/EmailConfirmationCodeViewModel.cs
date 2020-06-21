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
    #endregion

    #region Properties

    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    private Window window;
    private UserModel newUser;
    private RMMessengerEntities _context;
    private string _displayedMailMessage;

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
    public EmailConfirmationCodeViewModel(Window window, UserModel newUser)
    {
      this.window = window;
      this.newUser = newUser;
      _context = new RMMessengerEntities();
      BackCommand = new RelayCommand(BackCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CloseCommandExecute);
      SendAnotherVerificationCodeCommand = new RelayCommand(SendAnotherVerificationCodeExecute);
      _displayedMailMessage = string.Format("We've sent you a verification code on {0}. Please type the code you received:", newUser.Email);
    }

    private void BackCommandExecute()
    {
      var createNewAccountNextViewModel = new CreateNewAccountNextViewModel(window, newUser);
      WindowManager.CreateGeneralWindow(window, createNewAccountNextViewModel, Resources.CreateNewAccountNextWindowTitle, Resources.CreateNewAccountNextControlPath);

      if (createNewAccountNextViewModel.CloseAction == null)
      {
        createNewAccountNextViewModel.CloseAction = () => window.Close();
      }
    }

    private void CloseCommandExecute()
    {
      CloseAction?.Invoke();
    }
    #endregion


    private void NextCommandExecute()
    {

      var confirmation = _context.EmailConfirmations.Where(e => e.User_ID == UserModel.Instance.Username).FirstOrDefault();
      if (VerificationCode == confirmation.Code.ToString())
      {
        confirmation.IsConfirmed = true;
        SaveAccountInDatabase();
        WindowManager.ChangeWindowContent(window, this, Resources.EmailConfirmationCodeFinishedControlTitle, Resources.EmailConfirmationCodeFinishedControlPath);
      }
      else
      {
        VerificationCodeMessage = "The email verification failed. Please make sure you typed the right address.";
      }
    }

    private void SendAnotherVerificationCodeExecute()
    {
      var confirmationCode = new Random().Next(1000, 9999);

      if (!SendEmail.SendEmailExecute(newUser.Email, Resources.CreateNewAccountMailSubject, string.Format("We received your request to create an RM! ID account.\n\nThe validation code is {0}.\n\nIf it wasn't you, please disregard this email.", confirmationCode)))
      {
        WindowManager.OpenLoginErrorWindow(window, "The email is not valid.");
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
        var emailConfirmation = _context.EmailConfirmations.FirstOrDefault(u => u.User_ID == newUser.Username);
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
      newUser.ProfilePicture = data;
      newUser.EncryptedPassword = UserModel.Instance.EncryptedPassword;

      var user = new User
      {
        User_ID = newUser.Username,
        Password = newUser.EncryptedPassword,
      };
      _context.Users.Add(user);

      var account = new Account
      {
        First_Name = newUser.FirstName,
        Last_Name = newUser.LastName,
        Email = newUser.Email,
        User_ID = newUser.Username,
        Profile_Picture = newUser.ProfilePicture,
        PostalCode = newUser.PostalCode
      };
      _context.Accounts.Add(account);
      _context.SaveChanges();
    }
  }
}
