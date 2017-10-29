namespace devdeer.DoctorFlox.Logic.Wpf.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Abstract base class for all relay commadns.
    /// </summary>
    public abstract class BaseRelayCommand : ICommand
    {
        #region events

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public abstract event EventHandler CanExecuteChanged;

        #endregion

        #region explicit interfaces

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data
        /// to be passed, this object can be set to a null reference
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data
        /// to be passed, this object can be set to a null reference
        /// </param>
        public abstract void Execute(object parameter);

        #endregion

        #region methods

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This cannot be an event")]
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}