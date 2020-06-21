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
    #region Private Properties

    private Window window;
    public UserModel newUser { get; set; }
    private RMMessengerEntities _context;
    private List<string> _idSuggestionsList;
    private string _selectedIdSuggestion;
    private string _username;
    private string _email;
    private string _finishVerificationEmailButton = Resources.Next;
    private bool _isFinishButtonEnabled;
    private string _passwordValidationMessage;
    #endregion

    #region Public Properties

    public event PropertyChangedEventHandler PropertyChanged;
    public Action CloseAction { get; set; }
    public ICommand BackCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand CreateAccountCommand { get; set; }
    public string _successfulConfirmationMessage = Resources.AccountSuccessfullyCreated;

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
        IsFinishButtonEnabled = !string.IsNullOrEmpty(value) && !String.IsNullOrEmpty(UserModel.Instance.NewEncryptedPassword);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Username"));
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

    #endregion

    #region Constructor
    public CreateNewAccountNextViewModel(Window window, UserModel user)
    {
      this.window = window;
      newUser = user;
      _context = new RMMessengerEntities();
      BackCommand = new RelayCommand(BackCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
      CreateAccountCommand = new RelayCommand(CreateAccountCommandExecute);
      InitializeIdSuggestions();
      if (newUser != null)
      {
        Username = newUser.Username;
        Email = newUser.Email;
      }
    }

    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void BackCommandExecute()
    {
      var createNewAccountViewModel = new CreateNewAccountViewModel(window, newUser);
      WindowManager.CreateGeneralWindow(window, createNewAccountViewModel, Resources.CreateNewAccountWindowTitle, Resources.CreateNewAccountControlPath);

      if (createNewAccountViewModel.CloseAction == null)
      {
        createNewAccountViewModel.CloseAction = () => window.Close();
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


    private void CreateAccountCommandExecute()
    {
      string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Resources\\OfflineProfilePicture.jpg";
      byte[] data = File.ReadAllBytes(path);
      newUser.ProfilePicture = data;
      newUser.Username = Username;
      newUser.Email = Email;
      newUser.NewEncryptedPassword = UserModel.Instance.NewEncryptedPassword;
      UserModel.Instance.Username = Username;

      if (_context.Users.Any(u => u.User_ID == UserModel.Instance.Username))
      {
        WindowManager.OpenLoginErrorWindow(window, Resources.IDNotAvailableError, false);
        return;
      }

      string validationMessage = Validator.ValidateUsername(UserModel.Instance.Username);
      validationMessage += PasswordValidationMessage;

      if (!string.IsNullOrEmpty(validationMessage))
      {
        WindowManager.OpenLoginErrorWindow(window, validationMessage, true);
        return;
      }

      if (!string.IsNullOrEmpty(validationMessage))
      {
        WindowManager.OpenLoginErrorWindow(window, validationMessage, true);
        return;
      }

      if (!string.IsNullOrEmpty(Email))
      {
        CheckEmail();
        return;
      }
      SaveAccountInDatabase();
      WindowManager.ChangeWindowContent(window, this, Resources.EmailConfirmationCodeFinishedControlTitle, Resources.EmailConfirmationCodeFinishedControlPath);
    }

    private void CheckEmail()
    {
      var confirmationCode = new Random().Next(1000, 9999);

      if (!SendEmail.SendEmailExecute(Email, Resources.CreateNewAccountMailSubject, string.Format("We received your request to create an RM! ID account.\n\nThe validation code is {0}.\n\nIf it wasn't you, please disregard this email.", confirmationCode)))
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
        var emailConfirmationCodeViewModel = new EmailConfirmationCodeViewModel(window, newUser);
        WindowManager.ChangeWindowContent(window, emailConfirmationCodeViewModel, Resources.CreateNewAccountWindowTitle, Resources.EmailConfirmationCodeControlPath);
        if (emailConfirmationCodeViewModel.CloseAction == null)
        {
          emailConfirmationCodeViewModel.CloseAction = () => window.Close();
        }
      }
    }

    private void SaveAccountInDatabase()
    {
      var user = new User
      {
        User_ID = Username,
        Password = newUser.NewEncryptedPassword,
      };
      _context.Users.Add(user);

      var account = new Account
      {
        First_Name = newUser.FirstName,
        Last_Name = newUser.LastName,
        Email = newUser.Email,
        User_ID = Username,
        Profile_Picture = newUser.ProfilePicture,
        PostalCode = newUser.PostalCode
      };
      _context.Accounts.Add(account);
      _context.SaveChanges();
    }

    private void InitializeIdSuggestions()
    {
      IdSuggestionsList = new List<string>();
      while (IdSuggestionsList.Count < 3)
      {
        // generate random username
        var idSuggestion = string.Format("{0}_{1}{2}",
          newUser.FirstName.ToLower(), newUser.LastName.ToLower(), new Random().Next(10, 1000));

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

    #endregion

  }
}
