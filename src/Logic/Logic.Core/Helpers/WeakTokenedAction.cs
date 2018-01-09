namespace devdeer.DoctorFlox.Helpers
{
    using System;
    using System.Linq;

    using Enumerations;

    /// <summary>
    /// Brings an action together with a token.
    /// </summary>
    public struct WeakTokenedAction
    {
        #region constructors and destructors

        public WeakTokenedAction(WeakAction action, object token, ThreadCallbackOption threadCallback = ThreadCallbackOption.Sender)
        {
            Action = action;
            Token = token;
            ThreadCallbackOption = threadCallback;
        }

        #endregion

        #region properties

        /// <summary>
        /// The action to execute.
        /// </summary>
        public WeakAction Action { get; }

        /// <summary>
        /// Defines on which thread the <see cref="Action" /> should be executed.
        /// </summary>
        public ThreadCallbackOption ThreadCallbackOption { get; }

        /// <summary>
        /// An optional token to specify the action more closely or to pass some extra data.
        /// </summary>
        public object Token { get; }

        #endregion
    }
}