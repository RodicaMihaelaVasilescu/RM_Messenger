using RM_Messenger.Model;
using RM_Messenger.Properties;
using RM_Messenger.View;
using RM_Messenger.ViewModel;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RM_Messenger.Helpers
{
  public class WindowManager
  {
    public static void ChangeWindowContent(Window window, object viewModel, string title, string controlPath)
    {
      window.Title = title;
      var controlAssembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
      var controlType = controlAssembly.GetType(controlPath);
      var newControl = Activator.CreateInstance(controlType) as UserControl;
      newControl.DataContext = viewModel;
      window.Content = newControl;
    }
    public static void ResizeWindow(Window window)
    {
      var desktopWorkingArea = SystemParameters.WorkArea;
      window.Width = desktopWorkingArea.Right / 4.5;
      window.Top = desktopWorkingArea.Top;
      window.Height = desktopWorkingArea.Bottom + 7;
      window.Left = desktopWorkingArea.Right - window.Width;
    }
    public static void OpenLoginErrorWindow(Window OwnerWindow, string errorMessage)
    {
      if (!OwnerWindow.IsVisible)
      {
        return;
      }
      var errorMessageViewModel = new ErrorMessageViewModel(errorMessage);
      var errorWindow = new Window();
      WindowManager.CreateGeneralWindow(errorWindow, errorMessageViewModel, Resources.SignInErrorWindowTitle, Resources.ErrorMessageControlPath);

      if (errorMessageViewModel.CloseAction == null)
      {
        errorMessageViewModel.CloseAction = () => errorWindow.Close();
      }

      errorWindow.Owner = OwnerWindow;
      errorWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      errorWindow.ShowDialog();
    }

    public static void CreateGeneralWindow(Window window, object viewModel, string title, string controlPath)
    {
      window.Title = title;
      var controlAssembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
      var controlType = controlAssembly.GetType(controlPath);
      var newControl = Activator.CreateInstance(controlType) as UserControl;
      newControl.DataContext = viewModel;
      window.Content = newControl;
      window.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Empty.png", UriKind.RelativeOrAbsolute));
      window.SizeToContent = SizeToContent.WidthAndHeight;
      window.ResizeMode = ResizeMode.NoResize;
    }


    public static ChatControl OpenChatWindow(DisplayedContactModel selectedContact)
    {
      if (selectedContact == null)
      {
        return null;
      }
      UserModel user = new UserModel
      {
        Username = selectedContact.UserId,
        FirstName = selectedContact.UserId,
        LastName = selectedContact.UserId,
        IsOnline = false
      };
      foreach (Window win in App.Current.Windows)
      {
        if (win != null)
          if (win.Tag != null && win.Tag.ToString() == user.Username + "Child")
          {
            win.Focus();
            return null;
          }
      }
      Window child = new Window
      {
        Tag = user.Username + "Child",
        Title = Resources.ChatWindowTitle,
        MaxHeight = 540,
        MaxWidth = 620
    };
      var chatControl = new ChatControl();
      var chatViewModel = new ChatViewModel(child, selectedContact, chatControl.AutoScrollViewer);
      chatControl.DisplayedUser_ID = selectedContact.UserId;
      chatControl.DataContext = chatViewModel;
      child.Content = chatControl;
      child.SizeToContent = SizeToContent.WidthAndHeight;
      child.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Resources/Chat.ico", UriKind.RelativeOrAbsolute));
      child.Show();

      return chatControl;
    }
  }
}
