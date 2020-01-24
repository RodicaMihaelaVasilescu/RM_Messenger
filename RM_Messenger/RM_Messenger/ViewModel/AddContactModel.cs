﻿using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class AddContactModel : INotifyPropertyChanged
  {
    private string _email;
    public ICommand NextCommand { get; set; }
    public string Email
    {
      get { return _email; }
      set
      {
        if (_email == value) return;
        _email = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
      }
    }
    public Action CloseAction { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public AddContactModel(string email = "")
    {
      Email = email;
      NextCommand = new RelayCommand(LoginCommandExecute);
    }

    private void LoginCommandExecute()
    {
      var context = new RMMessengerEntities();
      if (context.Users.Any(u => u.User_ID == Email))
      {
        if (!context.AddressBooks.Any(a => a.User_ID == UserModel.Instance.Username && a.Friend_User_ID == Email))
        {
          context.AddressBooks.Add(new AddressBook
          {
            User_ID = UserModel.Instance.Username,
            Friend_User_ID = Email
          });
          context.SaveChanges();
        }
      }
    }
  }
}
