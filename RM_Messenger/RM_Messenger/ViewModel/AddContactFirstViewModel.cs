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
  class AddContactFirstViewModel : INotifyPropertyChanged
  {
    private string _email;

    public ICommand NextCommand { get; set; }
    public ICommand CancelCommand { get; set; }
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
      get { return isNextButtonEnabled; }
      set
      {
        if (isNextButtonEnabled == value) return;
        isNextButtonEnabled = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNextButtonEnabled"));
      }
    }

    public Action CloseAction { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    private Window window;
    private bool isNextButtonEnabled;

    public AddContactFirstViewModel(Window window, string email = "")
    {
      this.window = window;
      Email = email;
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }
    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }
    private void NextCommandExecute()
    {

      //context = new RMMessengerEntities();
      //var newContact = context.Users.Where(u => u.User_ID == Email).FirstOrDefault();
      var addContactSecondViewModel = new AddContactSecondViewModel(window, Email);

      WindowManager.ChangeWindowContent(window, addContactSecondViewModel, Resources.AddContactWindowTitle, Resources.AddContactSecondControlPath);
      if (addContactSecondViewModel.CloseAction == null)
      {
        addContactSecondViewModel.CloseAction = () => window.Close();
      }
      //window.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
 

    }
  }
}
