namespace devdeer.DoctorFlox.Ui.WpfSample
{
    using System;
    using System.Linq;
    using System.Windows;

    using Enumerations;

    using Logic.Wpf;
    using Logic.Wpf.Messages;

    using Models.Messages;

    /// <summary>
    /// Central logic that hooks into messenger messages for the UI.
    /// </summary>
    public class MessageListener
    {
        #region constructors and destructors

        public MessageListener()
        {
            if (BaseViewModel.IsInDesignModeStatic)
            {
                return;
            }
            RegisterMessages();
        }

        #endregion

        #region methods

        /// <summary>
        /// Handles all messenger messages related to <see cref="WindowOpenRequestMessage" />.
        /// </summary>
        /// <param name="message">The message containing informations about the window to open.</param>
        private static void HandleWindowOpenRequestMessage(WindowOpenRequestMessage message)
        {
            Window window = null;
            switch (message.Data)
            {
                case WindowType.ChildWindow:
                    window = new ChildWindow();
                    break;
                case WindowType.CollectionWindow:
                    window = new ChildWindow();
                    break;
            }
            if (window == null)
            {
                // we cannot open the type of window the caller intented to
                throw new ApplicationException("Invalid window type.");
            }
            // set the owner to the main window and open the window
            window.Owner = Application.Current.MainWindow;
            try
            {
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not open the window!", ex);
            }
        }

        /// <summary>
        /// Is called by the ctor to register all kinds of UI related messages on the <see cref="Messenger" />.
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<WindowOpenRequestMessage>(this, HandleWindowOpenRequestMessage);
        }

        #endregion

        #region properties

        /// <summary>
        /// Is used for binding purposes.
        /// </summary>
        /// <example>
        /// Bind this to your main view using <WpfSample:MessageListener x:Key="MessageListener" x:Shared="True" /> in
        /// App.xaml and something like IsEnabled="{Binding IsValid, Source={StaticResource MessageListener}}" in MainWindow.
        /// </example>
        public bool IsValid => true;

        #endregion
    }
}