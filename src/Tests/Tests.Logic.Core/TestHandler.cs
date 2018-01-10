namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using devdeer.DoctorFlox.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains helper and lifetime logic for all unit tests in this assembly.
    /// </summary>
    [TestClass]
    public class TestHandler
    {
        #region methods

        /// <summary>
        /// Is called before any of the tests in this assembly will execute.
        /// </summary>
        /// <param name="context">The context for the test run passed by the test runner.</param>
        [AssemblyInitialize]
        public static void OnAssemblyInit(TestContext context)
        {
            DispatcherHelper.Initialize();
            Context = context;
        }

        /// <summary>
        /// Executes the given <paramref name="function" /> inside of a <see cref="DispatcherSynchronizationContext" /> mimicing
        /// WPF sync.
        /// </summary>
        /// <remarks>
        /// Idea taken from answer in
        /// https://stackoverflow.com/questions/14087257/how-to-add-synchronization-context-to-async-test-method.
        /// </remarks>
        /// <param name="function">The logic to execute while using a WPF synchronization context.</param>
        public static void RunInWpfSyncContext(Func<Task> function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            var oldContext = SynchronizationContext.Current;
            try
            {
                var dispatcherContext = new DispatcherSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(dispatcherContext);
                var task = function();
                if (task == null)
                {
                    throw new InvalidOperationException();
                }
                var frame = new DispatcherFrame();
                task.ContinueWith(
                    x =>
                    {
                        frame.Continue = false;
                    },
                    TaskScheduler.Default);
                Dispatcher.PushFrame(frame);
                task.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(oldContext);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The context for the test run.
        /// </summary>
        public static TestContext Context { get; private set; }

        #endregion
    }
}