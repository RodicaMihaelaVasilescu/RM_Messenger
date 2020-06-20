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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class CreateNewAccountNextViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private string _username;
    private Window window;

    public UserModel NewUser { get; }

    private RMMessengerEntities _context;

    public RelayCommand CreateAccountCommand { get; }

    private List<string> _idSuggestionsList;
    private string _selectedIdSuggestion;
    private bool _isFinishButtonEnabled;
    private string _passwordValidationMessage;
    private string _email;
    private string _finishVerificationEmailButton = Resources.Next;

    public ICommand BackCommand { get; set; }
    public ICommand VerificationEmailCommand { get; set; }
    public RelayCommand CancelCommand { get; }

    public string Username
    {
      get { return _username; }
      set
      {
        if (_username == value) return;
        _username = value.ToLower();
        if (!IdSuggestionsList.Where(id => id == value).Any())
        {
          SelectedIdSuggestion = IdSuggestionsList.LastOrDefault();
        }
        IsFinishButtonEnabled = !string.IsNullOrEmpty(value) && !String.IsNullOrEmpty(UserModel.Instance.EncryptedPassword);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
      }
    }

    public List<string> IdSuggestionsList
    {
      get { return _idSuggestionsList; }
      set
      {
        if (_idSuggestionsList == value) return;
        _idSuggestionsList = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IdSuggestions"));
      }
    }

    public string SelectedIdSuggestion
    {
      get { return _selectedIdSuggestion; }
      set
      {
        if (_selectedIdSuggestion == value) return;
        _selectedIdSuggestion = value;
        if (value != IdSuggestionsList.LastOrDefault())
        {
          Username = value;
        }
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedIdSuggestion"));
      }
    }

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

    public bool IsFinishButtonEnabled
    {
      get { return _isFinishButtonEnabled; }
      set
      {
        if (_isFinishButtonEnabled == value) return;
        _isFinishButtonEnabled = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFinishButtonEnabled"));
      }
    }

    private bool accountSaved = false;
    private string _verificationCode;
    private string _verificationCodeMessage;

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
    public string FinishVerificationEmailButton
    {
      get { return _finishVerificationEmailButton; }
      set
      {
        if (_finishVerificationEmailButton == value) return;
        _finishVerificationEmailButton = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FinishVerificationEmailButton"));
      }
    }
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

    public string Email
    {
      get { return _email; }
      set
      {
        if (_email == value) return;
        _email = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }
    public Action CloseAction { get; set; }


    #region Constructor
    public CreateNewAccountNextViewModel(Window window, UserModel user)
    {
      this.window = window;
      NewUser = user;
      _context = new RMMessengerEntities();
      CreateAccountCommand = new RelayCommand(CreateAccountCommandExecute);
      BackCommand = new RelayCommand(BackCommandExecute);
      VerificationEmailCommand = new RelayCommand(VerificationEmailCommandExecute);

      CancelCommand = new RelayCommand(CancelCommandExecute);
      InitializeIdSuggestions();
      //NextCommand = new RelayCommand(NextCommandCommandExecute);
      //ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
      //CancelCommand = new RelayCommand(CancelCommandExecute);
    }

    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void InitializeIdSuggestions()
    {
      IdSuggestionsList = new List<string>();
      while (IdSuggestionsList.Count < 3)
      {
        // generate random username
        var idSuggestion = string.Format("{0}_{1}{2}",
          NewUser.FirstName.ToLower(), NewUser.LastName.ToLower(), new Random().Next(10, 1000));

        //check if the username is not already used and has not been previuously generated
        if (!_context.Users.Where(u => u.User_ID == idSuggestion).Any() &&
          !IdSuggestionsList.Where(id => id == idSuggestion).Any())
        {
          IdSuggestionsList.Add(idSuggestion);
        }
      }

      // Last option from the radio buttons list
      IdSuggestionsList.Add("Create my own: ");

      // Last radio button option is selected by default
      SelectedIdSuggestion = IdSuggestionsList.LastOrDefault();
    }

    private void BackCommandExecute()
    {
      var createNewAccountViewModel = new CreateNewAccountViewModel(window, NewUser);
      WindowManager.CreateGeneralWindow(window, createNewAccountViewModel, Resources.CreateNewAccountWindowTitle, Resources.CreateNewAccountControlPath);

      if (createNewAccountViewModel.CloseAction == null)
      {
        createNewAccountViewModel.CloseAction = () => window.Close();
      }
    }

    private void VerificationEmailCommandExecute()
    {
      if (accountSaved == true)
      {
        CloseAction?.Invoke();
        return;
      }
      var confirmation = _context.EmailConfirmations.Where(e => e.User_ID == UserModel.Instance.Username).FirstOrDefault();
      if (VerificationCode == confirmation.Code.ToString())
      {
        confirmation.IsConfirmed = true;
        SaveAccountInDatabase();
        VerificationCodeMessage = "The account has been successfully saved!";
        accountSaved = true;
        FinishVerificationEmailButton = Resources.Finish;
      }
      else
      {
        VerificationCodeMessage = "The email verification failed. Please make sure you typed the right address.";
      }
    }



    private void CreateAccountCommandExecute()
    {
      UserModel.Instance.Username = Username;
      if (_context.Users.Any(u => u.User_ID == UserModel.Instance.Username))
      {
        WindowManager.OpenLoginErrorWindow(window, Resources.IDNotAvailableError);
        return;
      }

      string validationMessage = Validator.ValidateUsername(UserModel.Instance.Username);
      validationMessage += PasswordValidationMessage;

      if (!string.IsNullOrEmpty(validationMessage))
      {
        WindowManager.OpenLoginErrorWindow(window, validationMessage);
        return;
      }

      if (!string.IsNullOrEmpty(Email))
      {
        var confirmationCode = new Random().Next(1000, 9999);
        if (!SendEmail(Email, Resources.CreateNewAccountMailSubject, string.Format("We received your request to create an RM! ID account.\n\nThe validation code is {0}.\n\nIf it wasn't you, please disregard this email.", confirmationCode)))
        {
          WindowManager.OpenLoginErrorWindow(window, "The email is not valid.");
          return;
        }
        else
        {
          _context.EmailConfirmations.Add(new EmailConfirmation
          {
            User_ID = UserModel.Instance.Username,
            Code = confirmationCode,
            IsConfirmed = false
          });
          _context.SaveChanges();
          WindowManager.ChangeWindowContent(window, this, Resources.EmailConfirmationCodeControlTitle, Resources.EmailConfirmationCodeControlPath);
        }
      }

      //SaveAccountInDatabase();


    }

    private void SaveAccountInDatabase()
    {
      string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Resources\\OfflineProfilePicture.jpg";
      byte[] data = File.ReadAllBytes(path);
      UserModel.Instance.ProfilePicture = data;

      var user = new User
      {
        User_ID = UserModel.Instance.Username,
        Password = UserModel.Instance.EncryptedPassword,
      };
      _context.Users.Add(user);
      var account = new Account
      {
        First_Name = NewUser.FirstName,
        Last_Name = NewUser.LastName,
        Email = NewUser.Email,
        User_ID = UserModel.Instance.Username,
        Profile_Picture = UserModel.Instance.ProfilePicture
      };
      _context.Accounts.Add(account);
      _context.SaveChanges();

      // WindowManager.OpenLoginErrorWindow(window, "Account successfully created");
      // var loginViewModel = new LoginViewModel(window);
      //WindowManager.ChangeWindowContent(window, loginViewModel, Resources.CreateNewAccountWindowTitle, Resources.LoginControlPath);
      // this.CloseAction?.Invoke();
    }

    public bool SendEmail(string email, string emailSubject, string emailContent)
    {
      if (email == null)
      {
        return false;
      }
      try
      {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("RM.Messenger.App@gmail.com");
        mail.To.Add(email);
        mail.Subject = emailSubject;
        mail.Body = emailContent;
        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential("RM.Messenger.App@gmail.com", "RM_Messenger_App2@gmail.com");
        SmtpServer.EnableSsl = true;
        SmtpServer.Send(mail);
        return true;
      }
      catch (Exception e)
      {
        return false;
        // ValidationMessage = "The email you entered is not valid. Please re-enter your email.";
      }
    }
    #endregion

  }
}
