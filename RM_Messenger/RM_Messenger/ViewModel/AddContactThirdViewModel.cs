using RM_Messenger.Command;
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
  class AddContactThirdViewModel: INotifyPropertyChanged
  {
    public ICommand FinishCommand { get; set; }
    private string newContact;
    private string _message;

    public event PropertyChangedEventHandler PropertyChanged;

    public Action CloseAction { get; set; }
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
    public AddContactThirdViewModel(Window window, string newContact, string message)
    {
      _message = message;
      FinishCommand = new RelayCommand(FinishCommandExecute);
      this.newContact = newContact;
    }

    private void FinishCommandExecute()
    {
      this.CloseAction();
    }
  }
}
