using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AccommodationApplication.Annotations;

namespace AccommodationApplication.Commands
{
    /// <summary>
    /// Klasa reprezentująca komendę, implementująca ICommand
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Tworzy nową instancję klasy DelegateCommand z odpowiednimi delegacjami na metody Execute i CanExecute z interfejsu.
        /// </summary>
        /// <param name="executeAction">Delegacja wywwoływana w implementacji metody Execute</param>
        /// <param name="canExecutePredicate">Predykat sprawdzany jako metoda CanExecute</param>
        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction), @"Action to execute cannot be null.");
            if (canExecutePredicate == null) throw new ArgumentNullException(nameof(canExecutePredicate), @"Predicate CanExecute cannot be null.");
            ExecuteAction = executeAction;
            CanExecutePredicate = canExecutePredicate;
        }
        /// <summary>
        /// Tworzy nową instancję klasy DelegateCommand z odpowiednimi delegacjami na metody Execute i CanExecute z interfejsu.
        /// </summary>
        /// <param name="executeAction">Delegacja wywwoływana w implementacji metody Execute</param>
        public DelegateCommand(Action<object> executeAction) : this(executeAction, x => true) { }

        /// <summary>
        /// Delegacja wywwoływana w implementacji metody Execute
        /// </summary>
        public Action<object> ExecuteAction { get; }

        /// <summary>
        /// Predykat sprawdzany jako metoda CanExecute
        /// </summary>
        public Predicate<object> CanExecutePredicate { get; }

        /// <summary>
        /// Sprawdza czy komenda może być wywołana
        /// </summary>
        /// <param name="parameter">Parametr komendy</param>
        /// <returns>Prawda, jeśli komenda może byc wykonana, fałsz w przeciwnym razie.</returns>
        public bool CanExecute(object parameter)
        {
            return CanExecutePredicate(parameter);
        }

        /// <summary>
        /// Wykonuje akcję określoną dla komendy
        /// </summary>
        /// <param name="parameter">Parametr komendy</param>
        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }

        /// <summary>
        /// Zdarzenie informujące o zmianie statusu możliwości wykonania komendy
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Podnosi zdarzenie CanExecuteChanged.
        /// </summary>
        public virtual void RaiseCanExecuteChangedEvent()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
