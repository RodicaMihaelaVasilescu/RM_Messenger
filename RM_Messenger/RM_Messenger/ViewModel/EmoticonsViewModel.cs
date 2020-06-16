using RM_Messenger.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace RM_Messenger.ViewModel
{
  class EmoticonsViewModel : INotifyPropertyChanged
  {
    #region Private Properties

    private string _selectedEmoticon;
    private List<List<string>> _emoticonsMatrix;

    #endregion

    #region Public Properties
    public Action CloseAction { get; set; }
    public ICommand CancelCommand { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public string TextEmoticon { get; set; }

    public List<List<string>> EmoticonsMatrix
    {
      get { return _emoticonsMatrix; }
      set
      {
        if (_emoticonsMatrix == value) return;
        _emoticonsMatrix = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmoticonsMatrix"));
      }
    }

    public string SelectedEmoticon
    {
      get { return _selectedEmoticon; }
      set
      {
        if (_selectedEmoticon == value) return;
        _selectedEmoticon = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedEmoticon"));
      }
    }

    #endregion

    #region Constructor

    public EmoticonsViewModel()
    {
      InitializeEmoticonList();
      CancelCommand = new RelayCommand(CancelCommandExecute);
    }

    #endregion

    #region Private Methods

    private void CancelCommandExecute()
    {
      //todo
    }

    private void InitializeEmoticonList()
    {
      EmoticonsMatrix = new List<List<string>>();
      int emoticonId = 1;
      for (int row = 0; row < 8; row++)
      {
        var emoticonsList = new List<string>();
        for (int column = 0; column < 6; column++)
        {
          emoticonsList.Add(string.Format("pack://application:,,,/RM_Messenger;component/Resources/Emoticons/{0}.gif", emoticonId++));
        }

        EmoticonsMatrix.Add(emoticonsList);
      }
      SelectedEmoticon = EmoticonsMatrix.FirstOrDefault().FirstOrDefault();
    }

    #endregion
  }
}
