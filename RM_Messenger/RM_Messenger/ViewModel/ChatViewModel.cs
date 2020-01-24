using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class ChatViewModel : INotifyPropertyChanged
  {
    #region Public Properties

    public ICommand SendCommand { get; set; }
    public Action CloseAction { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public ScrollViewer AutoScroll;
    public bool IsLogsChangedPropertyInViewModel { get; set; } = true;

    private RMMessengerEntities _context;

    public DisplayedContactModel DisplayedUser
    {
      get { return _displayedUser; }
      set
      {
        if (_displayedUser == value) return;
        _displayedUser = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayedUser"));
      }
    }

    public BitmapImage ProfilePicture
    {
      get { return _profilePicture; }
      set
      {
        if (_profilePicture == value) return;
        _profilePicture = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProfilePicture"));
      }
    }

    public string PersonalProfilePicture
    {
      get { return _personalProfilePicture; }
      set
      {
        if (_personalProfilePicture == value) return;
        _personalProfilePicture = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PersonalProfilePicture"));
      }
    }
    public string MessageBoxContent
    {
      get { return messageBoxContent; }
      set
      {
        if (messageBoxContent == value) return;
        messageBoxContent = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageBoxContent"));
      }
    }
    public ObservableCollection<MessageModel> MessagesList
    {
      get { return messagesList; }
      set
      {
        if (messagesList == value) return;
        messagesList = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessagesList"));
      }
    }

    #endregion

    #region Private Properties

    private BitmapImage _profilePicture;
    private string _personalProfilePicture;
    private string messageBoxContent;
    private DisplayedContactModel _displayedUser;
    private ObservableCollection<MessageModel> messagesList = new ObservableCollection<MessageModel>();

    #endregion

    #region Constructor
    public ChatViewModel(Window window, DisplayedContactModel displayedUser, ScrollViewer scroll)
    {
      _context = new RMMessengerEntities();
      DisplayedUser = displayedUser;
      displayedUser.OnlineIcoPath = "pack://application:,,,/RM_Messenger;component/Resources/Offline.ico";
      AutoScroll = scroll;
      SendCommand = new RelayCommand(SendCommandExecute);
      ProfilePicture = displayedUser.ImagePath;
      PersonalProfilePicture = "pack://application:,,,/RM_Messenger;component/Resources/ProfilePicture.jpg";
      LoadMessages();
    }

    #endregion

    #region Private Methods

    private void LoadMessages()
    {
      MessagesList = new ObservableCollection<MessageModel>( _context.Messages.OrderBy(m => m.Date).Where(u => u.SentTo_User_ID == DisplayedUser.Name && u.SentBy_User_ID == UserModel.Instance.Username ||
        u.SentBy_User_ID == DisplayedUser.Name && u.SentTo_User_ID == UserModel.Instance.Username).ToList().Select(m =>
        new MessageModel
        {
          SentBy = string.Format("{0}\n{1}",m.SentBy_User_ID , m.Date.ToString("dd/MM/yyyy HH:mm")),
          SentTo = m.SentTo_User_ID,
          Content = m.Message_Content,
          HorizontalAlignment = m.SentTo_User_ID == DisplayedUser.Name ? HorizontalAlignment.Right : HorizontalAlignment.Left
        }));
    }

    private void SendCommandExecute()
    {
      if (string.IsNullOrEmpty(messageBoxContent))
      {
        return;
      }

      var message = new Message
      {
        Date = DateTime.Now,
        SentTo_User_ID = DisplayedUser.Name,
        SentBy_User_ID = UserModel.Instance.Username,
        Message_Content = MessageBoxContent
      };
      _context.Messages.Add(message); ;
      _context.SaveChanges();


      MessagesList.Add(new MessageModel { SentBy = message.SentBy_User_ID + "\n" + message.Date.ToString("dd/MM/yyyy HH:mm"), SentTo = message.SentTo_User_ID, Content = MessageBoxContent, HorizontalAlignment = HorizontalAlignment.Right });

      MessageBoxContent = string.Empty;
      AutoScroll.ScrollToEnd();
    }

    #endregion

  }
}
