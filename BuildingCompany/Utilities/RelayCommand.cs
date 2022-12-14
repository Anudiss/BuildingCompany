using System;
using System.Windows.Input;

namespace BuildingCompany.Utilities
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private Action<object> _execute;
        private Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter = null) => _canExecute == null || _canExecute?.Invoke(parameter) != false;

        public void Execute(object parameter = null) => _execute?.Invoke(parameter);
    }
}
