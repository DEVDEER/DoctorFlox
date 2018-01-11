namespace deveer.DoctorFlox.Tests.Logic.Core.Tests.BaseClasses
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using devdeer.DoctorFlox;
    using devdeer.DoctorFlox.Messages;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestModels;

    /// <summary>
    /// Contains unit tests for the type <see cref="BaseViewModel" />.
    /// </summary>
    [TestClass]
    public class BaseViewModelTests
    {
        #region methods

        /// <summary>
        /// Tests if the <see cref="BaseViewModel.AssociatedView" /> property is <c>null</c> by default and
        /// does not throw any unexpected exceptions.
        /// </summary>
        [TestMethod]
        public void BaseViewAssociatedViewIsNullTest()
        {
            // arrange && act
            var viewModel = new TestViewModel();
            // assert
            Assert.IsNull(viewModel.AssociatedView);
        }

        /// <summary>
        /// Tests if the values passed to the ctor are present after the instantiation.
        /// </summary>
        [TestMethod]
        public void BaseViewModelConstructorBasicParameterTest()
        {
            TestHandler.RunInWpfSyncContext(
                async () =>
                {
                    // arrange
                    await Task.Yield();
                    // act
                    var viewModel = new TestViewModel(Messenger.Default, SynchronizationContext.Current);
                    // assert
                    TestHandler.AssertNonPublicProperty(viewModel, "SyncContext", SynchronizationContext.Current, "Synchronization context not set as expected.");
                    TestHandler.AssertNonPublicProperty(viewModel, "MessengerInstance", Messenger.Default, "Messenger context not set as expected.");
                });
        }

        /// <summary>
        /// Tests if the basic parameters are set correctly after instantiation.
        /// </summary>
        [TestMethod]
        public void BaseViewModelConstructorDefaultPropertiesTest()
        {
            // arrange && act
            var viewModel = new TestViewModel();
            // assert
            Assert.IsNotNull(viewModel.Id);
        }

        /// <summary>
        /// Tests if <see cref="BaseViewModel.CreateWindowInstance(Type,bool,Window)" /> will return <c>null</c> without
        /// throwing any exceptions.
        /// </summary>
        [TestMethod]
        public void BaseViewModelCreateWindowTest()
        {
            // arrange && act
            var viewModel = new TestViewModel();
            // assert
            var type = viewModel.GetType();
            var method = type.GetMethod("CreateWindowInstance", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Type), typeof(bool), typeof(Window) }, null);
            if (method == null)
            {
                throw new InvalidOperationException("Could not find method for test on target type.");
            }
            var result = method.Invoke(viewModel, new object[] { typeof(object), false, null });
            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests if the <see cref="BaseViewModel.InitCommands" /> method gets called.
        /// </summary>
        [TestMethod]
        public void BaseViewModelInitCommandsTest()
        {
            // arrange && act
            var viewModel = new TestViewModel();
            // assert
            Assert.IsNotNull(viewModel.TestCommand);
        }

        /// <summary>
        /// Tests if the <see cref="BaseViewModel.InitData" /> method gets called.
        /// </summary>
        [TestMethod]
        public void BaseViewModelInitDataTest()
        {
            // arrange && act
            var viewModel = new TestViewModel();
            // assert
            Assert.IsTrue(viewModel.PropertySetByInitData);
        }

        /// <summary>
        /// Tests if the <see cref="BaseViewModel.IsInDesignMode" /> property returns <c>false</c>.
        /// </summary>
        [TestMethod]
        public void BaseViewModelIsInDesignModeTest()
        {
            // arrange
            var viewModel = new TestViewModel();
            // act
            var actual = viewModel.IsInDesignMode;
            // assert
            Assert.IsFalse(actual);
        }

        #endregion
    }
}