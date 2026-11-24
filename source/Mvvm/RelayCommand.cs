using System;
using System.Windows.Input;

namespace BIMPlugins.Windows.Mvvm
{
    /// <summary>A command that runs an action; it can always run.</summary>
    internal sealed class RelayCommand(Action execute) : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => execute();
    }

    /// <summary>
    /// A command that runs an action with its parameter. A parameter of another type is ignored; null is passed on
    /// when <typeparamref name="T"/> takes it.
    /// </summary>
    internal sealed class RelayCommand<T>(Action<T> execute) : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is T value)
                execute(value);
            else if (parameter == null && default(T) == null)
                execute(default);
        }
    }
}
