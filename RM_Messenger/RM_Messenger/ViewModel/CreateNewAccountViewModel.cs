using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class CreateNewAccountViewModel : INotifyPropertyChanged
  {

    #region Private Fields

    private Window window;
    private UserModel newUser;
    private string _firstName;
    private string _lastName;
    private string _postalCode;
    private string _gender;
    private bool _isNextButtonEnabled;

    #endregion

    #region Public Fields
    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand CancelCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public Action CloseAction { get; set; }

    public string FirstName
    {
      get { return _firstName; }
      set
      {
        if (_firstName == value) return;
        _firstName = new String(value.Where(Char.IsLetter).ToArray());
        IsNextButtonEnabled = !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstName"));
      }
    }
    public string LastName
    {
      get { return _lastName; }
      set
      {
        if (_lastName == value) return;
        _lastName = new String(value.Where(Char.IsLetter).ToArray());
        IsNextButtonEnabled = !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastName"));
      }
    }

    public string PostalCode
    {
      get { return _postalCode; }
      set
      {
        if (_postalCode == value) return;
        _postalCode = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }
    public string Gender
    {
      get { return _gender; }
      set
      {
        if (_gender == value) return;
        _gender = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Gender"));
      }
    }

    public bool IsNextButtonEnabled
    {
      get { return _isNextButtonEnabled; }
      set
      {
        if (_isNextButtonEnabled == value) return;
        _isNextButtonEnabled = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNextButtonEnabled"));
      }
    }


    #endregion

    #region Constructor
    public CreateNewAccountViewModel(Window window, UserModel newUser = null)
    {
      this.window = window;
      this.newUser = newUser;
      if (newUser != null)
      {
        FirstName = newUser.FirstName;
        LastName = this.newUser.LastName;
        PostalCode = newUser.PostalCode; 
      }
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }
    #endregion

    #region Private methods
    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void NextCommandExecute()
    {
      newUser = new UserModel
      {
        FirstName = FirstName,
        LastName = LastName,
        PostalCode = PostalCode
      };

      var createNewAccountNextViewModel = new CreateNewAccountNextViewModel(window, newUser);
      WindowManager.ChangeWindowContent(window, createNewAccountNextViewModel, Resources.CreateNewAccountWindowTitle, Resources.CreateNewAccountNextControl);
      if (createNewAccountNextViewModel.CloseAction == null)
      {
        createNewAccountNextViewModel.CloseAction = () => window.Close();
      }
    }
    #endregion
  }
}
