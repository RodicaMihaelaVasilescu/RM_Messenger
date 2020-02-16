using RM_Messenger.Command;
using RM_Messenger.Database;
using RM_Messenger.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RM_Messenger.ViewModel
{
  class ChatViewModel : INotifyPropertyChanged
  {

    #region Private Properties

    private RMMessengerEntities _context;
    private BitmapImage _profilePicture;
    private BitmapImage _personalProfilePicture;
    private string messageBoxContent;
    private DisplayedContactModel _displayedUser;
    private ObservableCollection<MessageModel> messagesList = new ObservableCollection<MessageModel>();

    #endregion

    #region Public Properties

    public ICommand SendCommand { get; set; }
    public Action CloseAction { get; set; }
    public ScrollViewer AutoScroll;
    public event PropertyChangedEventHandler PropertyChanged;

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

    public BitmapImage PersonalProfilePicture
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

    #region Constructor

    public ChatViewModel(Window window, DisplayedContactModel displayedUser, ScrollViewer scroll)
    {
      _context = new RMMessengerEntities();
      DisplayedUser = displayedUser;
      AutoScroll = scroll;
      SendCommand = new RelayCommand(SendCommandExecute);
      ProfilePicture = displayedUser.ImagePath;
      PersonalProfilePicture = Converters.GeneralConverters.ConvertToBitmapImage(UserModel.Instance.ProfilePicture);
      LoadMessages();
    }

    #endregion

    #region Private Methods

    private void LoadMessages()
    {
      MessagesList = new ObservableCollection<MessageModel>(_context.Messages.OrderBy(m => m.Date).Where(u => u.SentTo_User_ID == DisplayedUser.UserId && u.SentBy_User_ID == UserModel.Instance.Username ||
       u.SentBy_User_ID == DisplayedUser.UserId && u.SentTo_User_ID == UserModel.Instance.Username).ToList().Select(m =>
       new MessageModel
       {
         SentBy = string.Format("{0} ({1}): ", m.SentBy_User_ID, m.Date.ToString("dd/MM/yyyy HH:mm:ss")),
         SentTo = m.SentTo_User_ID,
         ToolTip = string.Format("{0} ({1}): ", m.SentBy_User_ID, m.Date.ToString("dd/MM/yyyy HH:mm:ss")),
         Content = GetDocument(m.SentBy_User_ID, m.Message_Content),
         HorizontalAlignment = m.SentTo_User_ID == DisplayedUser.UserId ? HorizontalAlignment.Right : HorizontalAlignment.Left
       }));
    }

    private FlowDocument GetDocument(string sentBy, string message)
    {
      var flowDocument = new FlowDocument();
      if (!string.IsNullOrEmpty(sentBy as string))
      {
        var paragraph = new Paragraph();
        if (sentBy == DisplayedUser.UserId)
        {
          Bold myBold = new Bold(new Run(sentBy + ": "));
          myBold.Foreground = Brushes.DarkBlue;
          paragraph.Inlines.Add(myBold);
        }
        else
        {
          Bold myBold = new Bold(new Run(sentBy + ": "));
          myBold.Foreground = Brushes.Gray;
          paragraph.Inlines.Add(myBold);
        }
        paragraph.Inlines.Add(message);
        flowDocument.Blocks.Add(paragraph);
      }
      return flowDocument;
    }

    private void SendCommandExecute()
    {
      if (string.IsNullOrEmpty(messageBoxContent) )
      {
        return;
      }
      var flowDocument = new FlowDocument();
      if (messageBoxContent != null)
      {
        var xamlText = (string)messageBoxContent;
        flowDocument = (FlowDocument)XamlReader.Parse(xamlText);
      }

      messageBoxContent = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Text;
      if (messageBoxContent.Length < 4)
      {
        return;
      }
      //remove new line \r\n
      messageBoxContent = messageBoxContent.Remove(messageBoxContent.Length - 2);
      var message = new Message
      {
        Date = DateTime.Now,
        SentTo_User_ID = DisplayedUser.UserId,
        SentBy_User_ID = UserModel.Instance.Username,
        Message_Content = MessageBoxContent
      };
      _context.Messages.Add(message); ;
      _context.SaveChanges();


      MessagesList.Add(new MessageModel
      {
        SentBy = string.Format("{0} ({1}): ", message.SentBy_User_ID, message.Date.ToString("dd/MM/yyyy HH:mm:ss")),
        SentTo = message.SentTo_User_ID,
        ToolTip = string.Format("{0} ({1}): ", message.SentBy_User_ID, message.Date.ToString("dd/MM/yyyy HH:mm:ss")),
        Content = GetDocument(message.SentBy_User_ID, MessageBoxContent),
        HorizontalAlignment = HorizontalAlignment.Right
      });

      MessageBoxContent = string.Empty;
      AutoScroll.ScrollToEnd();
    }

    #endregion

  }
}
