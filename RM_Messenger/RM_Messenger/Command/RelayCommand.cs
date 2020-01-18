using System;
using System.Windows.Input;

namespace RM_Messenger.Command
{
  public class RelayCommand : ICommand
  {
    #region Private Properties

    private readonly Action _action;
    private Func<bool> _canExecute;

    #endregion

    #region Constructor

    public RelayCommand(Action action, Func<bool> canExecute = null)
    {
      _action = action;
      _canExecute = canExecute;
    }

    #endregion

    #region Public Methods

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute.Invoke();
    }

    public void Execute(object parameter)
    {
      _action.Invoke();
    }

    #endregion

  }
}
