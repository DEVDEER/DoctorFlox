namespace devdeer.DoctorFlox.Interfaces
{
    using System;
    using System.Linq;

    using Helpers;

    /// <summary>
    /// This interface is meant for the <see cref="WeakAction{T}" /> class and can be
    /// useful if you store multiple WeakAction{T} instances but don't know in advance
    /// what type T represents.
    /// </summary>
    public interface IExecuteWithObject
    {
        #region methods

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="parameter">A parameter passed as an object, to be casted to the appropriate type.</param>
        void ExecuteWithObject(object parameter);

        /// <summary>
        /// Deletes all references, which notifies the cleanup method that this entry must be deleted.
        /// </summary>
        void MarkForDeletion();

        #endregion

        #region properties

        /// <summary>
        /// The target of the <see cref="WeakAction" />.
        /// </summary>
        object Target { get; }

        #endregion
    }
}