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
  class AddContactSecondViewModel : INotifyPropertyChanged
  {
    #region Private properties

    private Window window;
    private RMMessengerEntities _context;
    private string newContact;
    private List<string> contactLists;
    private string selectedContactList;

    #endregion

    #region Public properties

    public ICommand BackCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public Action CloseAction { get; set; }

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

    #endregion

    #region Constructor

    public AddContactSecondViewModel(Window window, string contact)
    {
      _context = new RMMessengerEntities();
      this.window = window;
      newContact = contact;
      BackCommand = new RelayCommand(BackCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
      CancelCommand = new RelayCommand(CancelCommandExecute);
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

    private void BackCommandExecute()
    {
      var addContactFirstViewModel = new AddContactFirstViewModel(window, newContact);
      WindowManager.ChangeWindowContent(window, addContactFirstViewModel, Resources.AddContactWindowTitle, Resources.AddContactFirstControlPath);

      if (addContactFirstViewModel.CloseAction == null)
      {
        addContactFirstViewModel.CloseAction = () => window.Close();
      }
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

    }
    private void NextCommandExecute()
    {
      var currentUserID = UserModel.Instance.Username;
      string message = string.Format(Resources.ContactHasBeenAddedMessage, newContact);

      if (_context.Users.Any(u => u.User_ID == newContact))
      {
        if(!_context.AddRequests.Any(a=> a.SentBy_User_ID == currentUserID && a.SentTo_User_ID == newContact))
        {
          _context.AddRequests.Add(new AddRequest
          {
            SentBy_User_ID = UserModel.Instance.Username,
            SentTo_User_ID = newContact,
            Status = Resources.NoResponseStatus
          }) ;
        }
        if (!_context.AddressBooks.Any(a => a.User_ID == currentUserID && a.Friend_User_ID == newContact) && currentUserID != newContact)
        {
          _context.AddressBooks.Add(new AddressBook
          {
            User_ID = UserModel.Instance.Username,
            Friend_User_ID = newContact,
            Date = DateTime.Now
          });
        }

        if (selectedContactList == Resources.FriendListName)
        {
          if (!_context.Friendships.Any(f => f.User_ID == UserModel.Instance.Username && f.Friend_ID == newContact))
          {
            _context.Friendships.Add(new Friendship
            {
              User_ID = UserModel.Instance.Username,
              Friend_ID = newContact,
              Date = DateTime.Now
            });
          }
          else
          {
            message = string.Format( Resources.ContactAlreadyExistsMessage, newContact);

          }
        }
      }
      else
      {
        message = string.Format(Resources.ContactDoesNotExistsMessage, newContact);
      }
      _context.SaveChanges();

      var addContactThirdViewModel = new AddContactThirdViewModel(window, newContact, message);
      WindowManager.ChangeWindowContent(window, addContactThirdViewModel, Resources.AddContactWindowTitle, Resources.AddContactThirdControlPath);
      if (addContactThirdViewModel.CloseAction == null)
      {
        addContactThirdViewModel.CloseAction = () => window.Close();
      }
    }

    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    #endregion
  }
}
