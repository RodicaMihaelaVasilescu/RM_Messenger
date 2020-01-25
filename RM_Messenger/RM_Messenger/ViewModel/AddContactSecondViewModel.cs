using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddContactSecondViewModel
  {

    public ICommand BackCommand { get; set; }
    public ICommand NextCommand { get; set; }
    private Window window;
    private string newContact;
    public Action CloseAction { get; set; }

    public AddContactSecondViewModel(Window window, string contact)
    {
      this.window = window;
      newContact = contact;
      BackCommand = new RelayCommand(BackCommandExecute);
      NextCommand = new RelayCommand(NextCommandExecute);
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
      var context = new RMMessengerEntities();
      var currentUserID = UserModel.Instance.Username;
      string message;
      if (context.Users.Any(u => u.User_ID == newContact))
      {
        if (!context.AddressBooks.Any(a => a.User_ID == currentUserID && a.Friend_User_ID == newContact) && currentUserID != newContact)
        {
          context.AddressBooks.Add(new AddressBook
          {
            User_ID = UserModel.Instance.Username,
            Friend_User_ID = newContact,
            Date = DateTime.Now
          });
          context.SaveChanges();
          message = newContact + " has been added to your Messenger List and Address Book, pending his or her response to your request.";
        }
        else
        {
          message = newContact + " already exists in your Messenger List and Address Book.";
        }
      }
      else
      {
        message = newContact + " does not exist. Make sure you have typed the correct Messenger ID or address.";
      }

      var addContactThirdViewModel = new AddContactThirdViewModel(window, newContact, message);
      WindowManager.ChangeWindowContent(window, addContactThirdViewModel, Resources.AddContactWindowTitle, Resources.AddContactThirdControlPath);
      if (addContactThirdViewModel.CloseAction == null)
      {
        addContactThirdViewModel.CloseAction = () => window.Close();
      }

    }
  }
}
