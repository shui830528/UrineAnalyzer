using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
  public class CommandObject : ICommand
  {
    private readonly Action<object> executeAction;
    private readonly Func<object, bool> canExecuteFunc;
    public event EventHandler CanExecuteChanged;

    public CommandObject(Action<object> execute)
        : this(execute, null)
    { }

    public CommandObject(Action<object> execute, Func<object, bool> canExecute)
    {
      if (execute == null)
      {
        return;
      }
      executeAction = execute;
      canExecuteFunc = canExecute;
    }
    public bool CanExecute(object parameter)
    {
      if (canExecuteFunc == null)
      {
        return true;
      }
      return canExecuteFunc(parameter);
    }
    public void Execute(object parameter)
    {
      if (executeAction == null)
      {
        return;
      }
      executeAction(parameter);
    }
    public void OnCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
