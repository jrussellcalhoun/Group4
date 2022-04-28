using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Word_Game.Utilities
{
    /// <summary>
    /// This is a command which routes command execution to other objects.
    /// For instance, a command attached to a button click of type DelegateCommand
    /// can route it's command execution to a delegate method within another class object.
    /// This is useful for the purposes of decoupling our Logic from our UI code while
    /// still receiving notice from some important game events, 
    /// such as when a letter has been clicked.
    /// </summary>
    public class RelayCommand : ObservableObject, ICommand
    {
        // The readonly keyword denotes that these fields can only be assigned to during class construction.
        // In other words, these are essentially treated const references after instantiation.
        // Action and Predicate are wrapper objects available in C# system which encapsulate function delegates.
        // These are what we will use to route our ICommand Execute and CanExecute methods to some other method
        // e.g. a method from our logic class to handle a specific command on a control.
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constructors
        public RelayCommand(Action<object> execute) : this(execute, null) {}

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            // C# 6.0 static method null check this replaces if (execute == null) throw new ArgumentNullException();
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));

            _execute = execute;
            OnPropertyChanged(nameof(_execute));
            _canExecute = canExecute;
            OnPropertyChanged(nameof(_canExecute));
        }
        #endregion

        #region ICommand Members
        // ICommand interface method that determines conditions under which the command can execute.
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        // This is an event that fires if CanExecute status changes (i.e. whether it can or can't execute).
        // This uses the add and remove keywords to define custom event accessors.
        // RequerySuggested fires when the CommandManager detects conditions that might change the ability of the command to execute.
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // ICommand interface method that actually executes logic for the command. In this case we are delegating this to the _execute method wrapper.
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
