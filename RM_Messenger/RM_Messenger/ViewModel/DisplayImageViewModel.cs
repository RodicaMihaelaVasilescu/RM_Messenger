using RM_Messenger.Command;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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

    public BitmapImage ProfilePicturePath
    {
      get { return profilePicturePath; }
      set
      {
        profilePicturePath = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePicturePath"));
      }
    }

    #endregion

    #region Private Properties

    private BitmapImage profilePicturePath;

    #endregion

    #region Constructor

    public DisplayImageViewModel()
    {
      CloseCommand = new RelayCommand(CloseCommandExecute);
      SelectImageCommand = new RelayCommand(SelectImageCommandExecute);
      ProfilePicturePath = new BitmapImage(new Uri(@"pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture.jpg"));
      ProfilePicturePath.CacheOption = BitmapCacheOption.None;
      Image selectedImage = Image.FromFile(Path.GetDirectoryName( Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) +"\\Resources\\ProfilePicture.jpg");
      selectedImage.Dispose();
    }

    #endregion

    #region Private Methods

    private void CloseCommandExecute()
    {
      CloseAction?.Invoke();
    }

    private void SelectImageCommandExecute()
    {
      Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
      dialog.Filter = "All files (*.*)|*.*|PNG files (*.png)|*.png*|JPG files (*.jpg)|*.jpg*";
      dialog.ShowDialog();
      var newFile = dialog.FileName;
      if (string.IsNullOrEmpty(newFile) || ProfilePicturePath == null)
      {
        return;
      }

      try
      {
        string path = Path.GetDirectoryName( Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) +  "\\Resources\\ProfilePicture.jpg"; ;
        Image selectedImage = Image.FromFile(path);
        selectedImage.Dispose();
        File.Delete(path); 
        File.Copy(newFile, path);
        ProfilePicturePath = new BitmapImage(new Uri(Path.GetDirectoryName( Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) +"\\Resources\\ProfilePicture.jpg"));
        ProfilePicturePath.CacheOption = BitmapCacheOption.None;
        Image image = Image.FromFile(Path.GetDirectoryName(  Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\Resources\\ProfilePicture.jpg");
        image.Dispose();
      }
      catch (Exception e)
      {
      }

    }
    #endregion
  }
}
