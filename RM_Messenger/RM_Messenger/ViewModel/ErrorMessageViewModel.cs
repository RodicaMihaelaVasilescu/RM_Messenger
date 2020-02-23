using RM_Messenger.Command;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class ErrorMessageViewModel : INotifyPropertyChanged
  {
    #region Private properties

    private string _errorMessage;
    private string _imagePath;

    #endregion

    #region Public properties

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

    #endregion

    #region Constructor
    public ErrorMessageViewModel(string errorMessage)
    {
      CloseCommand = new RelayCommand(LoginCommandExecute);
      if (errorMessage == Properties.Resources.YouMustEnterAnIDAndPasswordError)
      {
        _imagePath = "pack://application:,,,/RM_Messenger;component/Resources/SadEmoticon.png";
      }
      else
      {
        _imagePath = "pack://application:,,,/RM_Messenger;component/Resources/YahooMessengerEmoticon.png";
      }
      _errorMessage = errorMessage;
    }
    #endregion

    #region Private region
    private void LoginCommandExecute()
    {
      this.CloseAction();
    }
    #endregion
  }
}
