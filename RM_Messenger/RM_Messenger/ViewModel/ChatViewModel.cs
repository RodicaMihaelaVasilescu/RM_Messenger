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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
    private string documentXaml;

    public event PropertyChangedEventHandler PropertyChanged;
    public string DocumentXaml
    {
      get { return documentXaml; }

      set
      {
        documentXaml = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentXaml"));
      }
    }
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
      RegisterSqlDependency();
    }

    #endregion


    void RegisterSqlDependency()
    {
      string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
      SqlConnection connection = new SqlConnection(SqlConnectionString);

      SqlDependency.Start(SqlConnectionString);
      connection.Open();

      // Create a new SqlCommand object.
      using (SqlCommand command = new SqlCommand(
          "SELECT SentBy_User_ID, SentTo_User_ID, Text, Date FROM dbo.Messages", connection))
      {
        // Create a dependency and associate it with the SqlCommand.
        SqlDependency dependency = new SqlDependency(command);
        // Maintain the reference in a class member.

        // Subscribe to the SqlDependency event.
        dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

        // Execute the command.

        using (SqlDataReader reader = command.ExecuteReader())
        {
          // Process the DataReader.
        }
      }
      connection.Close();
    }

    // Handler method
    void OnDependencyChange(object sender,
       SqlNotificationEventArgs e)
    {
      RegisterSqlDependency();
      var chatUser = DisplayedUser.UserId;
      var currentUser = UserModel.Instance.Username;

      if (!_context.RecentLists.Where(r => r.Sent_By == currentUser && r.Sent_To == DisplayedUser.UserId).Any())
      {
        _context.RecentLists.Add(new RecentList { Sent_To = currentUser, Sent_By = DisplayedUser.UserId });
        _context.RecentLists.Add(new RecentList { Sent_To = DisplayedUser.UserId, Sent_By = currentUser });
        _context.SaveChanges();
      }

      DateTime dateOfLastDisplayedMessage;
      if (MessagesList.Any())
      {
        dateOfLastDisplayedMessage = MessagesList.LastOrDefault().Date;
      }
      else
      {
        dateOfLastDisplayedMessage = new DateTime();
      }

      Message lastMessageFromDb = _context.Messages.Where(m => m.SentBy_User_ID == chatUser &&
      m.SentTo_User_ID == currentUser).OrderByDescending(m => m.Date).FirstOrDefault();
      if (lastMessageFromDb == null && MessagesList.Any() ||
        DateTime.Compare(lastMessageFromDb.Date, dateOfLastDisplayedMessage) < 0)
      {
        return;
      }

      var message = new Message
      {
        Date = lastMessageFromDb.Date,
        SentTo_User_ID = currentUser,
        SentBy_User_ID = chatUser,
        Text = lastMessageFromDb.Text
      };

      App.Current.Dispatcher.Invoke((Action)delegate
      {
        MessagesList.Add(new MessageModel
        {
          SentBy = message.SentBy_User_ID,
          SentTo = message.SentTo_User_ID,
          Date = message.Date,
          ToolTip = string.Format("{0} ({1})", message.SentBy_User_ID, message.Date.ToString("dd/MM/yyyy HH:mm:ss")),
          Content = GetDocument(message.SentBy_User_ID, message.Text)
        });
      });

      App.Current.Dispatcher.Invoke(() =>
      {
        AutoScroll.ScrollToEnd();
      });
    }

    #region Private Methods

    private void LoadMessages()
    {
      MessagesList = new ObservableCollection<MessageModel>(_context.Messages.OrderBy(m => m.Date).Where(u => u.SentTo_User_ID == DisplayedUser.UserId && u.SentBy_User_ID == UserModel.Instance.Username ||
       u.SentBy_User_ID == DisplayedUser.UserId && u.SentTo_User_ID == UserModel.Instance.Username).ToList().Select(m =>
       new MessageModel
       {
         SentBy = m.SentBy_User_ID,
         SentTo = m.SentTo_User_ID,
         Date = m.Date,
         ToolTip = string.Format("{0} ({1})", m.SentBy_User_ID, m.Date.ToString("dd/MM/yyyy HH:mm:ss")),
         Content = GetDocument(m.SentBy_User_ID, m.Text),
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
    public void SendMessage(object sender, EventArgs e)
    {
      SendCommandExecute();
    }
    public void SendCommandExecute()
    {
      if (this.messageBoxContent == null)
      {
        return;
      }

      var flowDocument = (FlowDocument)XamlReader.Parse(messageBoxContent);
      string text = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Text;
      if (text.Length < 3)
      {
        return;
      }
      // remove new line \r\n
      text = text.Remove(text.Length - 2);
      var message = new Message
      {
        Date = DateTime.Now,
        SentTo_User_ID = DisplayedUser.UserId,
        SentBy_User_ID = UserModel.Instance.Username,
        Text = text
      };
      _context.Messages.Add(message); ;
      _context.SaveChanges();


      MessagesList.Add(new MessageModel
      {
        SentBy = message.SentBy_User_ID,
        SentTo = message.SentTo_User_ID,
        Date = message.Date,
        ToolTip = string.Format("{0} ({1})", message.SentBy_User_ID, message.Date.ToString("dd/MM/yyyy HH:mm:ss")),
        Content = GetDocument(message.SentBy_User_ID, text)
      });

      MessageBoxContent = null;
      AutoScroll.ScrollToEnd();
    }

    #endregion

  }
}
