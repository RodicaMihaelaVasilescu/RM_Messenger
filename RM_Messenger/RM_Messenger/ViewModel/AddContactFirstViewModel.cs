using RM_Messenger.Command;
using RM_Messenger.Helper;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddContactFirstViewModel : INotifyPropertyChanged
  {
    #region Private properties

    private Window window;
    private string _email;
    private bool _isNextButtonEnabled;

    #endregion

    #region Public properties
    public ICommand NextCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public string Email
    {
      get { return _email; }
      set
      {
        if (_email == value) return;
        _email = value;
        IsNextButtonEnabled = !string.IsNullOrEmpty(Email);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
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

    public AddContactFirstViewModel(Window window, string email = null)
    {
      this.window = window;
      Email = email;
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }
    #endregion

    #region Private Methods
    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void NextCommandExecute()
    {
      // open AddContactSecond window
      var addContactSecondViewModel = new AddContactSecondViewModel(window, Email);
      WindowManager.ChangeWindowContent(window, addContactSecondViewModel, Resources.AddContactWindowTitle, Resources.AddContactSecondControlPath);
      if (addContactSecondViewModel.CloseAction == null)
      {
        addContactSecondViewModel.CloseAction = () => window.Close();
      }
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    #endregion
  }
}
