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

    public ICommand BackCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    private RMMessengerEntities _context;
    private Window window;
    private string newContact;
    private List<string> contactLists;
    private string selectedContactList;

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

    private void CancelCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void LoadContactLists()
    {
      ContactLists = new List<string>();
      ContactLists.Add("Friends");
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
      //window.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
      window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

    }
    private void NextCommandExecute()
    {
      var currentUserID = UserModel.Instance.Username;
      string message = newContact + " has been added to your Messenger List and Address Book, pending his or her response to your request."; ;
      if (_context.Users.Any(u => u.User_ID == newContact))
      {
        if (!_context.AddressBooks.Any(a => a.User_ID == currentUserID && a.Friend_User_ID == newContact) && currentUserID != newContact)
        {
          _context.AddressBooks.Add(new AddressBook
          {
            User_ID = UserModel.Instance.Username,
            Friend_User_ID = newContact,
            Date = DateTime.Now
          });
        }

        if (selectedContactList == "Friends")
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
            message = newContact + " already exists in your Messenger List and Address Book.";

          }
        }
      }
      else
      {
        message = newContact + " does not exist. Make sure you have typed the correct Messenger ID or address.";
      }
      _context.SaveChanges();

      var addContactThirdViewModel = new AddContactThirdViewModel(message);
      WindowManager.ChangeWindowContent(window, addContactThirdViewModel, Resources.AddContactWindowTitle, Resources.AddContactThirdControlPath);
      if (addContactThirdViewModel.CloseAction == null)
      {
        addContactThirdViewModel.CloseAction = () => window.Close();
      }

    }
  }
}
