namespace devdeer.DoctorFlox.Logic.Wpf.Interfaces
{
    using System;
    using System.Linq;

    using Helpers;

    /// <summary>
    /// This interface is meant for the <see cref="WeakFunc{TResult}" /> class and can be
    /// useful if you store multiple WeakFunc{T} instances but don't know in advance
    /// what type T represents.
    /// </summary>
    public interface IExecuteWithObjectAndResult
    {
        #region methods

        /// <summary>
        /// Executes a Func and returns the result.
        /// </summary>
        /// <param name="parameter">A parameter passed as an object, to be casted to the appropriate type.</param>
        /// <returns>The result of the operation.</returns>
        object ExecuteWithObject(object parameter);

        #endregion
    }
}