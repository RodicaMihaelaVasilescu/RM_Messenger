using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddToMessengerRequestSecondViewModel : INotifyPropertyChanged
  {
    #region Private fields

    private AddRequest request;
    private Window window;
    private string _dispplayedMessage;
    private string _name;
    private List<string> contactLists;
    private string selectedContactList;

    #endregion

    #region Public fields
    public Action CloseAction { get; set; }
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
    public List<string> ContactLists
    {
      get { return contactLists; }
      set
      {
        if (contactLists == value) return;
        contactLists = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ContatcLists"));
      }
    }

    public string SelectedContactList
    {
      get { return selectedContactList; }
      set
      {
        if (selectedContactList == value) return;
        selectedContactList = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedContactList"));
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

    #endregion

    #region Contructor
    public AddToMessengerRequestSecondViewModel(Window window, AddRequest request)
    {
      this.request = request;
      this.window = window;
      _name = UserModel.Instance.Username;
      DisplayedMessage = string.Format("A message will be sent asking {0} to approve your request to add him or her.", request.SentBy_User_ID);
      BackCommand = new RelayCommand(BackCommandExecute);
      FinishCommand = new RelayCommand(FinishCommandExecute);
      LoadContactLists();
    }
    #endregion

    #region Private methods
    private void LoadContactLists()
    {
      //todo : database contact lists
      ContactLists = new List<string>();
      ContactLists.Add(Resources.FriendListName);
      ContactLists.Add("Address Book");
      selectedContactList = ContactLists.FirstOrDefault();
    }

    private void FinishCommandExecute()
    {
      var context = new RMMessengerEntities();
      var addRequest = context.AddRequests.FirstOrDefault(r => r.AddRequest_ID == request.AddRequest_ID);
      addRequest.Status = Resources.AcceptedStatus;

      context.AddRequests.Add(new AddRequest
      {
        SentBy_User_ID = UserModel.Instance.Username,
        SentTo_User_ID = request.SentBy_User_ID,
        Status = Resources.NoResponseStatus
      });

      if (!context.AddressBooks.Any(a => a.User_ID == UserModel.Instance.Username && a.Friend_User_ID == request.SentBy_User_ID))
      {
        context.AddressBooks.Add(new AddressBook
        {
          User_ID = UserModel.Instance.Username,
          Friend_User_ID = request.SentBy_User_ID,
          Date = DateTime.Now
        });
      }

      if (selectedContactList == Resources.FriendListName && !context.Friendships.Any(f => f.User_ID == UserModel.Instance.Username && f.Friend_ID == request.SentBy_User_ID))
      {
        context.Friendships.Add(new Friendship
        {
          User_ID = UserModel.Instance.Username,
          Friend_ID = request.SentBy_User_ID,
          Date = DateTime.Now
        });
      }

      context.SaveChanges();
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

    #endregion
  }
}
