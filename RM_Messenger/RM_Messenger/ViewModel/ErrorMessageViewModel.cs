using RM_Messenger.Command;
using RM_Messenger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class ErrorMessageViewModel : INotifyPropertyChanged
  {
    private string _errorMessage;
    private string _imagePath;
    public ICommand CloseCommand { get; set; }
    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public string ErrorMessage
    {
      get { return _errorMessage; }
      set
      {
        if (_errorMessage == value) return;
        _errorMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorMessage"));
      }
    }

    public string ImagePath
    {
      get { return _imagePath; }
      set
      {
        if (_imagePath == value) return;
        _imagePath = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImagePath"));
      }
    }
    public ErrorMessageViewModel(string errorMessage)
    {
    CloseCommand = new RelayCommand(LoginCommandExecute);
      if (errorMessage == Properties.Resources.YouMustEnterAnIDAndPasswordError)
      {
        _imagePath = "pack://application:,,,/RM_Messenger;component/Resources/YahooMessengerEmoticon.png";
      }
      else
      {
        _imagePath = "pack://application:,,,/RM_Messenger;component/Resources/SadEmoticon.png";
      }
      _errorMessage = errorMessage;
    }

    private void LoginCommandExecute()
    {
      this.CloseAction();
    }
  }
}
