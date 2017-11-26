namespace devdeer.DoctorFlox.Logic.Wpf.Messages
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using Enumerations;

    using Helpers;

    using Interfaces;

    /// <summary>
    /// Basic messenger implementation.
    /// </summary>
    public class Messenger : IMessenger
    {
        #region member vars

        private readonly ConcurrentDictionary<Type, List<WeakTokenedAction>> _derivedRegistrations = new ConcurrentDictionary<Type, List<WeakTokenedAction>>();

        private readonly ConcurrentDictionary<Type, List<WeakTokenedAction>> _onlyDirectRegistrations = new ConcurrentDictionary<Type, List<WeakTokenedAction>>();

        private bool _isCleanupRegistered;

        #endregion

        #region constants

        private static IMessenger _defaultInstance;

        private static SynchronizationContext _synchronizationContext;

        private static readonly object MessengerLock = new object();

        #endregion

        #region constructors and destructors

        public Messenger(SynchronizationContext context)
        {
            _synchronizationContext = context;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            Register(recipient, null, false, action);
        }

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, ThreadCallbackOption threadCallbackOption, Action<TMessage> action)
        {
            Register(recipient, threadCallbackOption, null, false, action);
        }

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            Register(recipient, null, receiveDerivedMessagesToo, action);
        }

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            Register(recipient, token, false, action);
        }

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            Register(recipient, ThreadCallbackOption.Sender, token, false, action);
        }

        /// <inheritdoc />
        public virtual void Register<TMessage>(object recipient, ThreadCallbackOption threadCallbackOption, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            var recipients = receiveDerivedMessagesToo ? _derivedRegistrations : _onlyDirectRegistrations;
            List<WeakTokenedAction> list;
            if (!recipients.ContainsKey(messageType))
            {
                list = new List<WeakTokenedAction>();
                recipients.TryAdd(messageType, list);
            }
            else
            {
                list = recipients[messageType];
            }
            var weakAction = new WeakAction<TMessage>(recipient, action);
            var item = new WeakTokenedAction(weakAction, token, threadCallbackOption);
            list.Add(item);
            RequestCleanup();
        }

        /// <inheritdoc />
        public virtual void Send<TMessage>(TMessage message)
        {
            SendToTargetOrType(message, null, null);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void Send<TMessage, TTarget>(TMessage message)
        {
            SendToTargetOrType(message, typeof(TTarget), null);
        }

        /// <inheritdoc />
        public virtual void Send<TMessage>(TMessage message, object token)
        {
            SendToTargetOrType(message, null, token);
        }

        /// <inheritdoc />
        public virtual void Unregister(object recipient)
        {
            UnregisterFromLists(recipient, _derivedRegistrations);
            UnregisterFromLists(recipient, _derivedRegistrations);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void Unregister<TMessage>(object recipient)
        {
            Unregister<TMessage>(recipient, null, null);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This syntax is more convenient than other alternatives.")]
        public virtual void Unregister<TMessage>(object recipient, object token)
        {
            Unregister<TMessage>(recipient, token, null);
        }

        /// <inheritdoc />
        public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            Unregister(recipient, null, action);
        }

        /// <inheritdoc />
        public virtual void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            UnregisterFromLists(recipient, token, action, _onlyDirectRegistrations);
            UnregisterFromLists(recipient, token, action, _derivedRegistrations);
            RequestCleanup();
        }

        #endregion

        #region methods

        public void Cleanup()
        {
            CleanupList(_derivedRegistrations);
            CleanupList(_onlyDirectRegistrations);
            _isCleanupRegistered = false;
        }

        /// <summary>
        /// Provides a way to override the Messenger.Default instance with a custom instance, for example for unit testing
        /// purposes.
        /// </summary>
        /// <param name="newMessenger">The instance that will be used as Messenger.Default.</param>
        public static void OverrideDefault(IMessenger newMessenger)
        {
            lock (MessengerLock)
            {
                _defaultInstance = newMessenger;
            }
        }

        /// <summary>
        /// Notifies the Messenger that the lists of recipients should  be scanned and cleaned up.
        /// </summary>
        /// <remarks>
        /// Since recipients are stored as <see cref="WeakReference" />, recipients can be garbage collected even though the
        /// Messenger keeps
        /// them in a list. During the cleanup operation, all "dead" recipients are removed from the lists. Since this operation
        /// can take a moment, it is only executed when the application is idle. For this reason, a user of the Messenger class
        /// should use
        /// <see cref="RequestCleanup" /> instead of forcing one with the <see cref="Cleanup" /> method.
        /// </remarks>
        public void RequestCleanup()
        {
            if (_isCleanupRegistered)
            {
                return;
            }
            Action cleanupAction = Cleanup;
            DispatcherHelper.BeginInvoke(cleanupAction, DispatcherPriority.ApplicationIdle);
            _isCleanupRegistered = true;
        }

        /// <summary>
        /// Sets the Messenger's default (static) instance to null.
        /// </summary>
        public static void Reset()
        {
            lock (MessengerLock)
            {
                _defaultInstance = null;
            }
        }

        /// <summary>
        /// Provides a non-static access to the static <see cref="Reset" /> method.
        /// </summary>
        /// <remarks>
        /// Sets the Messenger's default (static) instance to null.
        /// </remarks>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void ResetAll()
        {
            Reset();
        }

        /// <summary>
        /// Removes all dead entries from a given dictionary of <paramref name="lists" />.
        /// </summary>
        /// <remarks>
        /// An entry is considered "dead" if the underlaying <see cref="WeakAction" /> is <c>null</c> or not alive.
        /// </remarks>
        /// <param name="lists">The reference to the list to clean up.</param>
        private static void CleanupList(ConcurrentDictionary<Type, List<WeakTokenedAction>> lists)
        {
            if (lists == null)
            {
                return;
            }
            var listsToRemove = new List<Type>();
            foreach (var list in lists)
            {
                var recipientsToRemove = list.Value.Where(item => item.Action == null || !item.Action.IsAlive).ToList();
                foreach (var recipient in recipientsToRemove)
                {
                    list.Value.Remove(recipient);
                }
                if (list.Value.Count == 0)
                {
                    listsToRemove.Add(list.Key);
                }
            }
            foreach (var key in listsToRemove)
            {
                lists.TryRemove(key, out var ignore);
            }
        }

        /// <summary>
        /// Sends a <paramref name="message" /> of the given <typeparamref name="TMessage" /> to all registered listeners.
        /// </summary>
        /// <typeparam name="TMessage">The type of the <paramref name="message" />.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="weakActionsAndTokens">The enumerable of weak action references including optional tokens.</param>
        /// <param name="messageTargetType">The type of the target.</param>
        /// <param name="token">The token for matching purposes.</param>
        private static void SendToList<TMessage>(TMessage message, IEnumerable<WeakTokenedAction> weakActionsAndTokens, Type messageTargetType, object token)
        {
            if (weakActionsAndTokens == null)
            {
                return;
            }
            var list = weakActionsAndTokens.ToList();
            foreach (var item in list)
            {
                if (item.Action is IExecuteWithObject executeAction && item.Action.IsAlive && item.Action.Target != null
                    && (messageTargetType == null || item.Action.Target.GetType() == messageTargetType || messageTargetType.IsInstanceOfType(item.Action.Target))
                    && (item.Token == null && token == null || item.Token != null && item.Token.Equals(token)))
                {
                    switch (item.ThreadCallbackOption)
                    {
                        case ThreadCallbackOption.Sender:
                            executeAction.ExecuteWithObject(message);
                            break;
                        case ThreadCallbackOption.ThreadPool:
                            Task.Run(() => executeAction.ExecuteWithObject(message));
                            break;
                        case ThreadCallbackOption.UiThread:
                            _synchronizationContext.Post(o => executeAction.ExecuteWithObject(message), null);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sends a <paramref name="message" /> of the given <typeparamref name="TMessage" /> to all registered listeners.
        /// </summary>
        /// <typeparam name="TMessage">The type of the <paramref name="message" />.</typeparam>
        /// <param name="message">The message instance.</param>
        /// <param name="messageTargetType">The type of the target.</param>
        /// <param name="token">A token to append to the message.</param>
        private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            var messageType = typeof(TMessage);
            if (_derivedRegistrations != null)
            {
                // take care of registrations with sub-classing
                var keys = _derivedRegistrations.Keys.ToList();
                foreach (var type in keys)
                {
                    List<WeakTokenedAction> list = null;
                    if (messageType == type || messageType.IsSubclassOf(type) || type.IsAssignableFrom(messageType))
                    {
                        list = _derivedRegistrations[type].ToList();
                    }
                    SendToList(message, list, messageTargetType, token);
                }
            }
            if (_onlyDirectRegistrations != null)
            {
                // take care of registrations without sub-classing                
                List<WeakTokenedAction> list = null;
                if (_onlyDirectRegistrations.ContainsKey(messageType))
                {
                    list = _onlyDirectRegistrations[messageType].ToList();
                }
                if (list != null)
                {
                    SendToList(message, list, messageTargetType, token);
                }
            }
            RequestCleanup();
        }

        /// <summary>
        /// Removes a single <paramref name="recipient" /> from all <paramref name="lists" />.
        /// </summary>
        /// <param name="recipient">The receipient to remove.</param>
        /// <param name="lists">The dictionary of lists to remove the <paramref name="recipient" /> from.</param>
        private static void UnregisterFromLists(object recipient, IDictionary<Type, List<WeakTokenedAction>> lists)
        {
            if (recipient == null || lists == null || lists.Count == 0)
            {
                return;
            }
            foreach (var messageType in lists.Keys)
            {
                foreach (var item in lists[messageType])
                {
                    var weakAction = (IExecuteWithObject)item.Action;
                    if (weakAction != null && recipient == weakAction.Target)
                    {
                        weakAction.MarkForDeletion();
                    }
                }
            }
        }

        /// <summary>
        /// Removes a single <paramref name="recipient" /> from all <paramref name="lists" /> including check on
        /// <paramref name="token" /> and <paramref name="action" />.s
        /// </summary>
        /// <typeparam name="TMessage">The type of the messages the registration was done for.</typeparam>
        /// <param name="recipient">The receipient to search for.</param>
        /// <param name="token">The token for matching purposes.</param>
        /// <param name="action">The action for matching purposes.</param>
        /// <param name="lists">The dictionary of lists from which to remove the <paramref name="recipient" />.</param>
        private static void UnregisterFromLists<TMessage>(object recipient, object token, Action<TMessage> action, ConcurrentDictionary<Type, List<WeakTokenedAction>> lists)
        {
            var messageType = typeof(TMessage);
            if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(messageType))
            {
                return;
            }
            foreach (var item in lists[messageType])
            {
                if (item.Action is WeakAction<TMessage> weakActionCasted && recipient == weakActionCasted.Target && (action == null || action.Method.Name == weakActionCasted.MethodName)
                    && (token == null || token.Equals(item.Token)))
                {
                    item.Action.MarkForDeletion();
                }
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Retrieves the default messenger instance as a singleton.
        /// </summary>
        public static IMessenger Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    // we have to create the instance once
                    lock (MessengerLock)
                    {
                        // double check if the instance wasn't set after the lock
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new Messenger(SynchronizationContext.Current);
                        }
                    }
                }
                return _defaultInstance;
            }
        }

        #endregion
    }
}