namespace deveer.DoctorFlox.Tests.Logic.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
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
        /// Can be used to perform assertion on a property that is not public.
        /// </summary>
        /// <typeparam name="TItem">The type of the <paramref name="instance" />.</typeparam>
        /// <typeparam name="TValue">The type of the <paramref name="expectedValue" />.</typeparam>
        /// <param name="instance">The instance to test.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="expectedValue">The expected value in the <paramref name="instance" />.</param>
        /// <param name="assertErrorMessage">An optional error message for assertion-fail.</param>
        public static void AssertNonPublicProperty<TItem, TValue>(TItem instance, string propertyName, TValue expectedValue, string assertErrorMessage = "Expected value not present.")
        {
            var type = typeof(TItem);
            var property = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null)
            {
                throw new InvalidOperationException($"Cannot find property {propertyName} on type {type.Name}");
            }
            var actualValue = property.GetValue(instance);
            Assert.AreEqual(expectedValue, actualValue, assertErrorMessage);
        }
        
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