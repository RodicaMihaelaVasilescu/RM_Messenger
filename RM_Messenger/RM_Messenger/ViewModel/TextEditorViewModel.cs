using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using RM_Messenger.Command;
using RM_Messenger.Properties;

namespace RM_Messenger.ViewModel
{
  class TextEditorViewModel : INotifyPropertyChanged
  {
    #region Private Properties

    private readonly string textPath = @"..\..\TextEditorFiles\text.txt";
    private readonly string expressionPath = @"..\..\TextEditorFiles\pattern.txt";

    private int currentIndex;
    private string _expression;
    private string _text;
    private string _selectedResult;
    private string _resultsText;
    private string _numberOfWordsAndCharacters;
    private bool _isCaseSensitive;
    private bool _isWholeWordMatched;
    private bool _areWhiteSpacesCounted;
    private bool _arePrevAndNextButtonsEnabled;
    private ObservableCollection<string> _resultList;
    private string _totalResults;
    private string openedFile;
    private bool _isSaveDocumentEnabled;

    #endregion

    #region Public Properties

    public TextBox MyTextBox { get; }

    public TextBox MySearchTextBox { get; }

    public DataGrid MyDataGrid { get; }

    public ICommand FindCommand { get; set; }

    public ICommand FindPreviousWordCommand { get; set; }

    public ICommand FindNextWordCommand { get; set; }

    public ICommand ImportDocumentCommand { get; set; }

    public ICommand SaveDocumentCommand { get; set; }

    public ICommand CancelDocumentCommand { get; set; }


    public string Expression
    {
      get { return _expression; }
      set
      {
        if (value == _expression) return;
        _expression = value;
        FindCommandExecute();
        MySearchTextBox.Focus();
        OnPropertyChanged(nameof(Expression));
      }
    }

    public string Text
    {
      get { return _text; }
      set
      {
        if (value == _text) return;
        _text = value;
        CountNumberOfCharactersAndWords();
        OnPropertyChanged(nameof(Text));
      }
    }

    public string SelectedResult
    {
      get { return _selectedResult; }
      set
      {
        if (value == _selectedResult) return;
        _selectedResult = value;
        if (value != null)
        {
          MyTextBox.Focus();
          MyTextBox.Select(Int32.Parse(value.Split(' ').Last()), Expression.Length);
          currentIndex = ResultList.IndexOf(value);
          ResultsText = String.Format("Results: {0}/ {1}", currentIndex + 1, ResultList.Count);
        }
        OnPropertyChanged(nameof(SelectedResult));
      }
    }

    public string ResultsText
    {
      get { return _resultsText; }
      set
      {
        if (value == _resultsText) return;
        _resultsText = value;
        OnPropertyChanged(nameof(ResultsText));
      }
    }

    public string TotalResults
    {
      get { return _totalResults; }
      set
      {
        if (value == _totalResults) return;
        _totalResults = value;
        OnPropertyChanged(nameof(TotalResults));
      }
    }

    public string NumberOfWordsAndCharacters
    {
      get { return _numberOfWordsAndCharacters; }
      set
      {
        if (value == _numberOfWordsAndCharacters) return;
        _numberOfWordsAndCharacters = value;
        OnPropertyChanged(nameof(NumberOfWordsAndCharacters));
      }
    }

    public bool IsCaseSensitive
    {
      get { return _isCaseSensitive; }
      set
      {
        if (value == _isCaseSensitive) return;
        _isCaseSensitive = value;
        FindCommandExecute();
        OnPropertyChanged(nameof(IsCaseSensitive));
      }
    }

    public bool IsWholeWordMatched
    {
      get { return _isWholeWordMatched; }
      set
      {
        if (value == _isWholeWordMatched) return;
        _isWholeWordMatched = value;
        FindCommandExecute();
        OnPropertyChanged(nameof(IsWholeWordMatched));
      }
    }

    public bool AreWhiteSpacesCounted
    {
      get { return _areWhiteSpacesCounted; }
      set
      {
        if (value == _areWhiteSpacesCounted) return;
        _areWhiteSpacesCounted = value;
        CountNumberOfCharactersAndWords();
        FindCommandExecute();
        OnPropertyChanged(nameof(AreWhiteSpacesCounted));
      }
    }

    public bool IsSaveDocumentEnabled
    {
      get { return _isSaveDocumentEnabled; }
      set
      {
        if (value == _isSaveDocumentEnabled) return;
        _isSaveDocumentEnabled = value;
        OnPropertyChanged(nameof(IsSaveDocumentEnabled));
      }
    }

    public bool ArePrevAndNextButtonsEnabled
    {
      get { return _arePrevAndNextButtonsEnabled; }
      set
      {
        if (value == _arePrevAndNextButtonsEnabled) return;
        _arePrevAndNextButtonsEnabled = value;
        OnPropertyChanged(nameof(ArePrevAndNextButtonsEnabled));
      }
    }

    public ObservableCollection<string> ResultList
    {
      get { return _resultList; }
      set
      {
        if (_resultList == value) return;
        _resultList = value;
        OnPropertyChanged(nameof(ResultList));
      }
    }

    #endregion

    #region Constructor

    public TextEditorViewModel(TextBox mySearchTextBox, TextBox myTextBox, DataGrid myDataGrid)
    {
      MySearchTextBox = mySearchTextBox;
      MyTextBox = myTextBox;
      MyDataGrid = myDataGrid;
    }
    #endregion

    #region Public Methods

    public void Load()
    {
      ResultList = new ObservableCollection<string>();
      FindCommand = new RelayCommand(FindCommandExecute);
      FindPreviousWordCommand = new RelayCommand(FindPreviousWordCommandExecute);
      FindNextWordCommand = new RelayCommand(FindNextWordCommandExecute);
      ImportDocumentCommand = new RelayCommand(ImportDocumentCommandExecute);
      SaveDocumentCommand = new RelayCommand(SaveDocumentCommandExecute);
      CancelDocumentCommand = new RelayCommand(CancelDocumentCommandExecute);
      Text = File.ReadAllText(textPath);
      Expression = File.ReadAllText(expressionPath);
    }

    public void ImportDocumentCommandExecute()
    {
      var previousFile = openedFile;
      Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
      {
        Filter = "TXT (*.txt)|*.txt|All files (*.*)|*.*|Doc files (*.doc)|*.doc*|JPG files (*.jpg)|*.jpg*"
      };

      dialog.ShowDialog();
      openedFile = dialog.FileName;
      if (string.IsNullOrEmpty(openedFile))
        openedFile = previousFile;

      IsSaveDocumentEnabled = !string.IsNullOrEmpty(openedFile);
      if (string.IsNullOrEmpty(openedFile))
      {
        return;
      }
      Text = File.ReadAllText(openedFile);
    }

    public void SaveDocumentCommandExecute()
    {
      File.WriteAllText(openedFile, Text);
    }

    public void CancelDocumentCommandExecute()
    {
      openedFile = textPath;
      Text = string.Empty;
    }

    public void FindNextWordCommandExecute()
    {
      currentIndex++;
      currentIndex %= ResultList.Count;
      SelectedResult = ResultList.ElementAt(currentIndex);
      MyDataGrid.UpdateLayout();
      MyDataGrid.ScrollIntoView(MyDataGrid.SelectedItem);
    }

    public void FindPreviousWordCommandExecute()
    {
      currentIndex--;
      if (currentIndex < 0)
      {
        currentIndex = ResultList.Count - 1;
      }
      SelectedResult = ResultList.ElementAt(currentIndex);
      MyDataGrid.UpdateLayout();
      MyDataGrid.ScrollIntoView(MyDataGrid.SelectedItem);
    }

    public void FindCommandExecute()
    {
      if (Text == null || Expression == string.Empty)
      {
        ClearFields();
        return;
      }

      File.WriteAllText(textPath, Text);
      File.WriteAllText(expressionPath, Expression);
      ResultList = new ObservableCollection<string>();

      MatchCollection matches = null;
      try
      {
        matches = Regex.Matches(Text, IsWholeWordMatched ? string.Format(@"\b{0}\b", Expression) : Expression, IsCaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
      }
      catch
      {
        ClearFields();
        ResultsText = Resources.InvalidRegexError;
        return;
      }

      if (matches != null)
      {
        foreach (Match m in matches)
        {
          ResultList.Add(String.Format("Found '{0}' at position {1}", m.Value, m.Index));
        }
      }

      TotalResults = string.Format("Total results: {0}", ResultList != null ? ResultList.Count : 0);
      if (ResultList.Any())
      {
        ResultsText = String.Format("Results: {0}/ {1}", currentIndex, ResultList.Count);
        SelectedResult = ResultList.First();
        currentIndex = 0;
        ArePrevAndNextButtonsEnabled = true;
      }
      else
      {
        ClearFields();
        ResultsText = Resources.TextNotFoundError;
      }
    }

    #endregion

    #region Private Methods

    private void ClearFields()
    {
      ResultList.Clear();
      ResultsText = string.Empty;
      TotalResults = string.Format("Total results: {0}", ResultList != null ? ResultList.Count : 0);
      ArePrevAndNextButtonsEnabled = false;
      currentIndex = 0;
    }

    private void CountNumberOfCharactersAndWords()
    {
      NumberOfWordsAndCharacters = string.Format("Number of words: {0}\n", Regex.Matches(Text, @"[\w]+",
            RegexOptions.IgnoreCase).Count);
      NumberOfWordsAndCharacters += string.Format("Number of characters: {0}", AreWhiteSpacesCounted ? Text.Count(c => c != '\n') : Regex.Matches(Text, @"[a-zA-Z]").Count);
    }

    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
