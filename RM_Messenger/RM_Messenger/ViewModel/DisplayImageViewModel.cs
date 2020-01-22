using RM_Messenger.Command;
using RM_Messenger.Converters;
using RM_Messenger.Database;
using RM_Messenger.Model;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class DisplayImageViewModel : INotifyPropertyChanged
  {
    #region Public Properties

    public Action CloseAction { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand SelectImageCommand { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    private Window window;
    public BitmapImage ProfilePicture
    {
      get { return _profilePicture; }
      set
      {
        _profilePicture = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePicture"));
      }
    }

    #endregion

    #region Private Properties

    private BitmapImage _profilePicture;

    #endregion

    #region Constructor

    public DisplayImageViewModel(Window window)
    {
      this.window = window;
      CloseCommand = new RelayCommand(CloseCommandExecute);
      SelectImageCommand = new RelayCommand(SelectImageCommandExecute);
      ProfilePicture = GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
    }

    #endregion

    #region Private Methods

    private void CloseCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void SelectImageCommandExecute()
    {
      Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
      {
        Filter = "All files (*.*)|*.*|PNG files (*.png)|*.png*|JPG files (*.jpg)|*.jpg*"
      };
      dialog.ShowDialog();

      var newFile = dialog.FileName;
      if (string.IsNullOrEmpty(newFile) || ProfilePicture == null)
      {
        return;
      }

      UserModel.Instance.ProfilePicture = File.ReadAllBytes(newFile);
      ProfilePicture = GeneralConverters.ConvertToBitmapImage(newFile);
      window.Close();
      SetProfilePicture(newFile);
    }

    private void SetProfilePicture(string newFile)
    {
      var context = new RMMessengerEntities();
      var user = context.Users.FirstOrDefault(u=>u.User_ID == UserModel.Instance.Username);
      user.ProfilePicture = GeneralConverters.ConvertToByteArray(newFile);
      context.SaveChanges();
    }


    #endregion
  }
}
