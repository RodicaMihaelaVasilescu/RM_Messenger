using RM_Messenger.Command;
using RM_Messenger.Helper;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddContactThirdViewModel: INotifyPropertyChanged
  {
    #region Properties

    private string _message;
    private Window window;
    private string contact;

    public RelayCommand SendYourDetailsCommand { get; }
    public RelayCommand RequestContactDetailsCommand { get; }
    public RelayCommand AssociateThisIDCommand { get; }
    public RelayCommand AddAnotherContactCommand { get; }
    public ICommand FinishCommand { get; set; }
    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public string Message
    {
      get { return _message; }
      set
      {
        if (_message == value) return;
        _message = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
      }
    }

    #endregion

    #region Constructor

    public AddContactThirdViewModel(Window window, string newContact, string message)
    {
      this.window = window;
      contact = newContact;
      _message = message;

      SendYourDetailsCommand = new RelayCommand(SendYourDetailsCommandExecute);
      RequestContactDetailsCommand = new RelayCommand(RequestContactDetailsCommandExecute);
      AssociateThisIDCommand = new RelayCommand(AssociateThisIDCommandExecute);
      AddAnotherContactCommand = new RelayCommand(AddAnotherContactCommandExecute);
      FinishCommand = new RelayCommand(FinishCommandExecute);
    }

    #endregion

    #region Private Methods

    private void SendYourDetailsCommandExecute()
    {
      // todo
      CloseAction();
    }
    private void RequestContactDetailsCommandExecute()
    {
      //todo
      CloseAction();
    }

    private void AssociateThisIDCommandExecute()
    {
      //todo
      CloseAction();
    }

    private void AddAnotherContactCommandExecute()
    {
      var addContactFirstViewModel = new AddContactFirstViewModel(window);
      WindowManager.ChangeWindowContent(window, addContactFirstViewModel, Resources.AddContactWindowTitle, Resources.AddContactFirstControlPath);
      if (addContactFirstViewModel.CloseAction == null)
      {
        addContactFirstViewModel.CloseAction = () => window.Close();
      }
    }

    private void FinishCommandExecute()
    {
      this.CloseAction();
    }

    #endregion
  }
}
