using RM_Messenger.Command;
using System.Linq;
using System.Windows.Input;
using RM_Messenger.Database;
using System.Collections.ObjectModel;

namespace RM_Messenger.Model
{
  class ContactListsModel
  {
    public string DisplayedName { get; set; }
    public string ImagePath { get; set; }
    public bool IsExpanded { get; set; } = false;
    public ObservableCollection<DisplayedContactModel> ContactsList { get; set; }
    public DisplayedContactModel SelectedContact { get; set; }

    private ICommand deleteCommand;
    public ICommand DeleteCommand
    {
      get
      {
        return deleteCommand ?? (deleteCommand = new RelayCommand(DeleteCommandExecute));
      }
    }

    public void DeleteCommandExecute()
    {
      var context = new RMMessengerEntities();
      var itemToRemoveFromDb = context.Friendships.FirstOrDefault(c => c.User_ID == UserModel.Instance.Username && c.Friend_ID == SelectedContact.UserId);
      if (itemToRemoveFromDb == null)
      {
        return;
      }
      context.Friendships.Remove(itemToRemoveFromDb);
      context.SaveChanges();
      var itemToRemove = ContactsList.FirstOrDefault(c => c.UserId == SelectedContact.UserId);
      ContactsList.Remove(itemToRemove);
    }

  }
}
