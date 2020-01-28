using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
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
  class AddToMessengerRequestSecondViewModel: INotifyPropertyChanged
  {
    public Action CloseAction { get; set; }

    private AddRequest request;
    private Window window;
    private string _dispplayedMessage;
    private string _name;

    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand BackCommand { get; set; }
    public ICommand FinishCommand { get; set; }
    

    public string DisplayedMessage
    {
      get { return _dispplayedMessage; }
      set
      {
        if (_dispplayedMessage == value) return;
        _dispplayedMessage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayedMessage"));
      }
    }

    public string Name
    {
      get { return _name; }
      set
      {
        if (_name == value) return;
        _name = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
      }
    }

    public AddToMessengerRequestSecondViewModel(Window window, AddRequest request)
    {
      this.request = request;
      this.window = window;
      _name = UserModel.Instance.Username;
      DisplayedMessage = string.Format( "A message will be sent asking {0} to approve your request to add him or her.", request.SentBy_User_ID);
      BackCommand = new RelayCommand(BackCommandExecute);
      FinishCommand = new RelayCommand(FinishCommandExecute);
    }

    private void FinishCommandExecute()
    {
      CloseAction();
    }

    private void BackCommandExecute()
    {
      var addRequestViewModel = new AddToMessengerRequestFirstViewModel(window, request);
      WindowManager.ChangeWindowContent(window, addRequestViewModel, Resources.AddToMessengerRequestWindowTitle, Resources.AddToMessengerRequestFirstControlPath);

      if (addRequestViewModel.CloseAction == null)
      {
        addRequestViewModel.CloseAction = () => window.Close();
      }

    }
  }
}
