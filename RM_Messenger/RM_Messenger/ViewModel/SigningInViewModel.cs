﻿using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RM_Messenger.ViewModel
{
  class SigningInViewModel : INotifyPropertyChanged
  {
    public Action CloseAction { get; set; }
    public ICommand CancelCommand { get; set; }

    readonly Window window;
    private bool cancelButtonPressed;
    private string messageOnSingingIn;
    private RMMessengerEntities _context;
    private HomepageViewModel homepageViewModel;

    public event PropertyChangedEventHandler PropertyChanged;

    public string MessageOnSingingIn
    {
      get { return messageOnSingingIn; }
      set
      {
        if (messageOnSingingIn == value) return;
        messageOnSingingIn = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageOnSingingIn"));
      }
    }

    public SigningInViewModel(Window window)
    {
      _context = new RMMessengerEntities();
      var email = string.IsNullOrEmpty(UserModel.Instance.Username) ? string.Empty : UserModel.Instance.Username.Split('@')[0];
      MessageOnSingingIn = "Signing in as " + email;
      this.window = window;
      cancelButtonPressed = false;
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }

    private void CancelCommandExecute()
    {
      cancelButtonPressed = true;
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
    }

    public async void ValidateLogin()
    {
      await Task.Delay(5000);
      //SwitchToLoginindow();
      if (cancelButtonPressed)
      {
        //SwitchToLoginindow();
        return;
      }

      var user = _context.Users.FirstOrDefault(u => u.User_ID == UserModel.Instance.Username &&
   u.Password == UserModel.Instance.EncryptedPassword);
      if (user == null)
      {
        CancelCommandExecute();
        WindowManager.OpenLoginErrorWindow(window, Resources.IncorrectIDAndPassword);
        return;
      }
      var account = _context.Accounts.Where(a => a.User_ID == user.User_ID).FirstOrDefault();
      UserModel.Instance.ProfilePicture = account.Profile_Picture;
      UserModel.Instance.Status = account.Status;
      OpenHomepageWindow();
    }

    private void OpenHomepageWindow()
    {
      homepageViewModel = new HomepageViewModel(window);
      WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);

      if (homepageViewModel.CloseAction == null)
      {
        homepageViewModel.CloseAction = () => window.Close();
      }

      WindowManager.ResizeWindow(window);
      window.ResizeMode = ResizeMode.NoResize;
      window.Show();

      OpenAddRequests();
    }

    private void OpenAddRequests()
    {
      var addRequests = _context.AddRequests.Where(a => a.SentTo_User_ID == UserModel.Instance.Username && a.Status == Resources.NoResponseStatus).ToList();

      int offset = 0;
      foreach (var request in addRequests)
      {
        Window addRequestWindow = new Window();
        var addRequestViewModel = new AddToMessengerRequestFirstViewModel(addRequestWindow, request);
        WindowManager.CreateGeneralWindow(addRequestWindow, addRequestViewModel, Resources.AddToMessengerRequestWindowTitle, Resources.AddToMessengerRequestFirstControlPath);

        if (addRequestViewModel.CloseAction == null)
        {
          addRequestViewModel.CloseAction = () => addRequestWindow.Close();
        addRequestWindow.Closed += new EventHandler(homepageViewModel.ReloadContactLists);
        }
        addRequestWindow.Owner = window;
        addRequestWindow.Left = window.Left - 400 + offset;
        addRequestWindow.Top = window.Top + 150 + offset;
        offset += 60;
        addRequestWindow.Tag = "Child";
        addRequestWindow.Show();
      }
    }

  }
}
