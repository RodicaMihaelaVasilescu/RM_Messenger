using RM_Messenger.Command;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddContactThirdViewModel: INotifyPropertyChanged
  {
    private string _message;

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

    public AddContactThirdViewModel( string message)
    {
      _message = message;
      FinishCommand = new RelayCommand(FinishCommandExecute);
    }

    private void FinishCommandExecute()
    {
      this.CloseAction();
    }
  }
}
