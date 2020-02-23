using RM_Messenger.Command;
using RM_Messenger.Helper;
using RM_Messenger.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class ChangeProfilePictureViewModel : INotifyPropertyChanged
  {
    #region Private Properties

    private BitmapImage _profilePicture;

    #endregion

    #region Public Properties

    public Action CloseAction { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand DisplayImageCommand { get; set; }
    public Popup popup;
    public event PropertyChangedEventHandler PropertyChanged;

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

    #region Constructor

    public ChangeProfilePictureViewModel()
    {
      CloseCommand = new RelayCommand(CloseCommandExecute);
      DisplayImageCommand = new RelayCommand(DisplayImageCommandExecute);
    }

    #endregion

    #region Private Methods

    private void DisplayImageCommandExecute()
    {
      var window = new Window();
      var displayImageViewModel = new DisplayImageViewModel(window);
      WindowManager.ChangeWindowContent(window, displayImageViewModel, Resources.DisplayImageTitle, Resources.DisplayImagePath);

      window.Width = 400;
      var padding = 20;
      window.Left = Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - window.Width - padding;
      window.Top = SystemParameters.WorkArea.Top + padding;

      window.ShowDialog();
    }

    private void CloseCommandExecute()
    {
      popup.IsOpen = false;
    }

    #endregion
  }
}
