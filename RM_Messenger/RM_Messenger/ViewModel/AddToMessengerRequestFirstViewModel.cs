using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Properties;

namespace RM_Messenger.ViewModel
{
  class AddToMessengerRequestFirstViewModel : INotifyPropertyChanged
  {
    private Window window;
    private AddRequest request;
    private string _dispplayedMessage;
    private bool _allowChecked;
    private bool _doNotAllowCheck;
    private bool _addToMessengerEnabled;
    private bool _addToMessengerChecked;
    private string _nextButtonText;

    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand NextCommand { get; set; }
    public Action CloseAction { get; set; }

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

    public bool AllowChecked
    {
      get { return _allowChecked; }
      set
      {
        if (_allowChecked == value) return;
        _allowChecked = value;
        AddToMessengerEnabled = value;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllowChecked"));
      }
    }

    public bool DoNotAllowChecked
    {
      get { return _doNotAllowCheck; }
      set
      {
        if (_doNotAllowCheck == value) return;
        _doNotAllowCheck = value;
        AddToMessengerEnabled = !value;
        NextButtonText = value ? "Finish" : "Next";
        AddToMessengerChecked = false;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DoNotAllowCheck"));
      }
    }

    public bool AddToMessengerChecked
    {
      get { return _addToMessengerChecked; }
      set
      {
        if (_addToMessengerChecked == value) return;
        _addToMessengerChecked = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddToMessengerChecked"));
      }
    }

    public bool AddToMessengerEnabled
    {
      get { return _addToMessengerEnabled; }
      set
      {
        if (_addToMessengerEnabled == value) return;
        _addToMessengerEnabled = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddToMessengerEnabled"));
      }
    }

    public string NextButtonText
    {
      get { return _nextButtonText; }
      set
      {
        if (_nextButtonText == value) return;
        _nextButtonText = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NextButtonText"));
      }
    }

    public AddToMessengerRequestFirstViewModel(Window window, AddRequest request)
    {
      this.window = window;
      this.request = request;

      NextButtonText = "Next";
      _dispplayedMessage = string.Format("{0} would like to add you as his or her Messenger List ", request.SentBy_User_ID);
      AllowChecked = true;
      AddToMessengerEnabled = true;
      AddToMessengerChecked = true;
      NextCommand = new RelayCommand(NextCommandExecute);
    }

    private void NextCommandExecute()
    {
      if (AllowChecked)
      {
        OpenAddToMessengerRequestWindow();
      }
      else
      {
        var _context = new RMMessengerEntities();
        var addRequest = _context.AddRequests.FirstOrDefault(a => a.AddRequest_ID == request.AddRequest_ID);
        addRequest.Status = Resources.DeclinedStatus;
        _context.SaveChanges();

        CloseAction();
      }
    }

    private void OpenAddToMessengerRequestWindow()
    {
      var addRequestViewModel = new AddToMessengerRequestSecondViewModel(window, request);
      WindowManager.ChangeWindowContent(window, addRequestViewModel, Resources.AddToMessengerRequestWindowTitle, Resources.AddToMessengerRequestSecondControlPath);

      if (addRequestViewModel.CloseAction == null)
      {
        addRequestViewModel.CloseAction = () => window.Close();
      }
    }
  }
}
