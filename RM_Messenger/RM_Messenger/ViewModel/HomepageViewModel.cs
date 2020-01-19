using RM_Messenger.Command;
using RM_Messenger.Helper;
using RM_Messenger.Model;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class HomepageViewModel : INotifyPropertyChanged
  {
    #region Public Properties

    public Action CloseAction { get; set; }
    public ICommand LogoutCommand { get; set; }
    public ICommand SendCommand { get; set; }
    public string OnlineIcoPath { get; set; } = UserModel.Instance.IsOnline ? "pack://application:,,,/RM_Messenger;component/Resources/Online.ico" : "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
 
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

    public BitmapImage ProfilePicture
    {
      get { return profilePicture; }
      set
      {
        profilePicture = value;
        if (value != null) profilePicture.CacheOption = BitmapCacheOption.None;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePicture"));
      }
    }

    #endregion

    #region Private Properties

    private string _email;
    private Window window;
    private BitmapImage profilePicture;

    #endregion

    #region Constructor

    public HomepageViewModel(Window window)
    {
      this.window = window;
      window.Activated += new EventHandler(windowActivated);
      LogoutCommand = new RelayCommand(LogoutCommandExecute);
      InitializeUserProfile();
    }

    private void windowActivated(object sender, EventArgs e)
    {
      ProfilePicture =Converters.GeneralConverters.ConvertToBitmapImage( UserModel.Instance.ProfilePicture);
    }

    private void InitializeUserProfile()
    {
      Email = UserModel.Instance.Username;
      ProfilePicture = GetProfilePicture(UserModel.Instance.ProfilePicture);
    }
    #endregion

    #region Private Methods

    private void LogoutCommandExecute()
    {
      var loginViewModel = new LoginViewModel(window);
      WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);

      window.VerticalContentAlignment = VerticalAlignment.Top;
      foreach (Window win in App.Current.Windows)
      {
        if (win.Tag != null && win.Tag.ToString().EndsWith("Child"))
        {
          win.Close();
        }
      }
    }

    private BitmapImage GetProfilePicture(byte[] data)
    {
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.CacheOption = BitmapCacheOption.None;
        imageSource.StreamSource = memoryStream;
        imageSource.CacheOption = BitmapCacheOption.OnLoad;
        imageSource.EndInit();
        return imageSource;
      }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
