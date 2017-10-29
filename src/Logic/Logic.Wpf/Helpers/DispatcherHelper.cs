namespace devdeer.DoctorFlox.Logic.Wpf.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// Helper class for dispatcher operations on the UI thread.
    /// </summary>
    public static class DispatcherHelper
    {
        #region methods

        /// <summary>
        /// Invokes an action asynchronously on the UI thread.
        /// </summary>
        /// <param name="action">The action that must be executed.</param>
        /// <returns>
        /// An object, which is returned immediately after BeginInvoke is called, that can be used to interact
        /// with the delegate as it is pending execution in the event queue.
        /// </returns>
        public static DispatcherOperation BeginInvoke(Action action)
        {
            CheckDispatcher();
            return UiDispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Executes an action on the UI thread. If this method is called
        /// from the UI thread, the action is executed immendiately. If the
        /// method is called from another thread, the action will be enqueued
        /// on the UI thread's dispatcher and executed asynchronously.
        /// <para>
        /// For additional operations on the UI thread, you can get a
        /// reference to the UI thread's dispatcher thanks to the property
        /// <see cref="UiDispatcher" />
        /// </para>
        /// .
        /// </summary>
        /// <param name="action">
        /// The action that will be executed on the UI
        /// thread.
        /// </param>
        public static void CheckBeginInvokeOnUi(Action action)
        {
            if (action == null)
            {
                return;
            }
            CheckDispatcher();
            if (UiDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                BeginInvoke(action);
            }
        }

        /// <summary>
        /// This method should be called once on the UI thread to ensure that
        /// the <see cref="UiDispatcher" /> property is initialized.
        /// <para>
        /// In a Silverlight application, call this method in the
        /// Application_Startup event handler, after the MainPage is constructed.
        /// </para>
        /// <para>In WPF, call this method on the static App() constructor.</para>
        /// </summary>
        public static void Initialize()
        {
            if (UiDispatcher != null && UiDispatcher.Thread.IsAlive)
            {
                return;
            }
            UiDispatcher = Dispatcher.CurrentDispatcher;
        }

        public static Task InvokeAsync(Action action)
        {
            CheckDispatcher();
            var operation = BeginInvoke(action);
            return operation.Task;
        }

        /// <summary>
        /// Resets the class by deleting the <see cref="UiDispatcher" />
        /// </summary>
        public static void Reset()
        {
            UiDispatcher = null;
        }

        private static void CheckDispatcher()
        {
            if (UiDispatcher == null)
            {
                throw new InvalidOperationException("The DispatcherHelper is not initialized.\nCall DispatcherHelper.Initialize() in the static App constructor.");
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets a reference to the UI thread's dispatcher, after the
        /// <see cref="Initialize" /> method has been called on the UI thread.
        /// </summary>
        public static Dispatcher UiDispatcher { get; private set; }

        #endregion
    }
}