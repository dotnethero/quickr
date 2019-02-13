using System;
using System.Windows.Input;

namespace Quickr.Utils
{
    public class ParameterCommand : ICommand
    {
        private readonly Action<object> _execute;

        public ParameterCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}