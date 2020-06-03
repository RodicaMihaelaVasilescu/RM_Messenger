using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helpers;
using RM_Messenger.Model;
using RM_Messenger.Properties;

namespace RM_Messenger.ViewModel
{
  class AddToMessengerRequestFirstViewModel : INotifyPropertyChanged
  {
    #region Private fields
    private RMMessengerEntities _context;
    private Window window;
    private AddRequest request;
    private string _dispplayedMessage;
    private bool _allowChecked;
    private bool _doNotAllowCheck;
    private bool _addToMessengerEnabled;
    private bool _addToMessengerChecked;
    private string _nextButtonText;
    private Visibility _addToMessengerVisibility;
    #endregion

    #region Public fields
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
        NextButtonText = value ? Resources.Next : Resources.Finish;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddToMessengerChecked"));
      }
    }

    public Visibility AddToMessengerVisibility
    {
      get { return _addToMessengerVisibility; }
      set
      {
        if (_addToMessengerVisibility == value) return;
        _addToMessengerVisibility = value;
        NextButtonText = value == Visibility.Visible ? Resources.Next : Resources.Finish;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddToMessengerVisibility"));
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

    #endregion


    #region Constructor
    public AddToMessengerRequestFirstViewModel(Window window, AddRequest request)
    {
      _context = new RMMessengerEntities();
      this.window = window;
      this.request = request;
      _addToMessengerVisibility = _context.Friendships.Any(f => f.User_ID == UserModel.Instance.Username && f.Friend_ID == request.SentBy_User_ID) ? Visibility.Hidden : Visibility.Visible;
      _dispplayedMessage = string.Format("{0} would like to add you as his or her Messenger List ", request.SentBy_User_ID);
      AllowChecked = true;
      AddToMessengerEnabled = _addToMessengerVisibility == Visibility.Visible;
      AddToMessengerChecked = _addToMessengerVisibility == Visibility.Visible;
      NextButtonText = AddToMessengerChecked ? Resources.Next : Resources.Finish;
      NextCommand = new RelayCommand(NextCommandExecute);
    }
    #endregion

    #region Private methods
    private void NextCommandExecute()
    {
      if (NextButtonText == Resources.Next)
      {
        OpenAddToMessengerRequestNextWindow();
      }
      else
      {
        var addRequest = _context.AddRequests.FirstOrDefault(a => a.AddRequest_ID == request.AddRequest_ID);
        addRequest.Status = AllowChecked ? Resources.AcceptedStatus : Resources.DeclinedStatus;
        _context.SaveChanges();

        CloseAction();
      }
    }

    private void OpenAddToMessengerRequestNextWindow()
    {
      var addRequestViewModel = new AddToMessengerRequestSecondViewModel(window, request);
      WindowManager.ChangeWindowContent(window, addRequestViewModel, Resources.AddToMessengerRequestWindowTitle, Resources.AddToMessengerRequestSecondControlPath);

      if (addRequestViewModel.CloseAction == null)
      {
        addRequestViewModel.CloseAction = () => window.Close();
      }
    }

    #endregion
  }
}
