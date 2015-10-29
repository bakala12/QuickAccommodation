using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Annotations;

namespace AccommodationApplication.Commands
{
    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction), @"Action to execute cannot be null.");
            if (canExecutePredicate == null) throw new ArgumentNullException(nameof(canExecutePredicate), @"Predicate CanExecute cannot be null.");
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate;
        }

        public DelegateCommand(Action<object> executeAction) : this(executeAction, x => true) { }

        public Action<object> ExecuteAction { get; }

        public Predicate<object> CanExecutePredicate { get; }

        public bool CanExecute(object parameter)
        {
            return CanExecutePredicate(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public virtual void RaiseCanExecuteChangedEvent()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
