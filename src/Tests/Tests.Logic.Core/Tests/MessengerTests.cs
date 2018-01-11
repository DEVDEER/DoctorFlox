namespace deveer.DoctorFlox.Tests.Logic.Core.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using devdeer.DoctorFlox.Enumerations;
    using devdeer.DoctorFlox.Messages;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the type <see cref="Messenger" />.
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MessengerTests
    {
        #region methods

        /// <summary>
        /// Tests the sending and receiving of <see cref="DataMessage{TData}" /> elements using the <see cref="Messenger" />.
        /// </summary>
        [TestMethod]
        public void MessengerDataMessageTest()
        {
            // arrange
            var messageWithValue = new DataMessage<object, object, int>(10);
            var messageWithValeAndSender = new DataMessage<object, object, string>(this, "10");
            var messageWithAll = new DataMessage<object, object, double>(this, this, 10);
            var messageWithValueHandled = false;
            var messageWithValeAndSenderHandled = false;
            var messageWithAllHandled = false;
            var handled = 0;
            // act
            TestHandler.RunInWpfSyncContext(
                async () =>
                {
                    Messenger.Default.Register<DataMessage<object, object, int>>(
                        this,
                        ThreadCallbackOption.ThreadPool,
                        msg =>
                        {
                            if (msg.Data == 10 && msg.Sender == null && msg.Target == null)
                            {
                                messageWithValueHandled = true;
                            }
                            Interlocked.Increment(ref handled);
                        });
                    Messenger.Default.Register<DataMessage<object, object, string>>(
                        this,
                        ThreadCallbackOption.ThreadPool,
                        msg =>
                        {
                            if (msg.Sender == this && msg.Target == null && (msg.Data?.Equals("10") ?? false))
                            {
                                messageWithValeAndSenderHandled = true;
                            }
                            Interlocked.Increment(ref handled);
                        });
                    Messenger.Default.Register<DataMessage<object, object, double>>(
                        this,
                        ThreadCallbackOption.ThreadPool,
                        msg =>
                        {
                            if (msg.Sender == this && msg.Target == this && msg.Data.Equals(10))
                            {
                                messageWithAllHandled = true;
                            }
                            Interlocked.Increment(ref handled);
                        });
                    Messenger.Default.Send(messageWithValue);
                    Messenger.Default.Send(messageWithValeAndSender);
                    Messenger.Default.Send(messageWithAll);
                    while (handled < 3)
                    {
                        await Task.Delay(10);
                    }
                });
            // assert
            Assert.IsTrue(messageWithValueHandled, "Message with int value only not handled correctly.");
            Assert.IsTrue(messageWithValeAndSenderHandled, "Message string value and sender not handled correctly.");
            Assert.IsTrue(messageWithAllHandled, "Message with double value, sender and target not handled correctly.");
        }

        /// <summary>
        /// Tests if the <see cref="Messenger.ResetAll" /> method will force the messenger to be re-instantiated.
        /// </summary>
        [TestMethod]
        public void MessengerResetAllTest()
        {
            // arrange
            var firstMessenger = Messenger.Default;
            var secondMessenger = Messenger.Default;
            // act
            (Messenger.Default as Messenger)?.ResetAll();
            var thirdMessenger = Messenger.Default;
            // assert
            Assert.AreEqual(firstMessenger, secondMessenger);
            Assert.AreNotEqual(firstMessenger, thirdMessenger);
        }

        /// <summary>
        /// Tests if the <see cref="Messenger.Reset" /> method will force the messenger to be re-instantiated.
        /// </summary>
        [TestMethod]
        public void MessengerResetTest()
        {
            // arrange
            var firstMessenger = Messenger.Default;
            var secondMessenger = Messenger.Default;
            // act
            Messenger.Reset();
            var thirdMessenger = Messenger.Default;
            // assert
            Assert.AreEqual(firstMessenger, secondMessenger);
            Assert.AreNotEqual(firstMessenger, thirdMessenger);
        }

        /// <summary>
        /// Tests if the
        /// <see cref="Messenger.Register{TMessage}(object,ThreadCallbackOption,Action{TMessage})" /> throws an
        /// <see cref="InvalidOperationException" /> if
        /// no synchronization context is provided.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task MessengerThreadCallbackExceptionTest()
        {
            SynchronizationContext.SetSynchronizationContext(null);
            Messenger.Reset();
            // act        
            Messenger.Default.Register<Message>(
                this,
                ThreadCallbackOption.UiThread,
                msg =>
                {
                });
            await Task.Run(
                () =>
                {
                    Messenger.Default.Send(new Message(this));
                });
        }

        /// <summary>
        /// Tests if the
        /// <see cref="Messenger.Register{TMessage}(object,ThreadCallbackOption,Action{TMessage})" />
        /// operates on the expected thread.
        /// </summary>
        [TestMethod]
        public void MessengerThreadCallbackTest()
        {
            // arrange
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            var senderThreadId = 0;
            var handled = 0;
            var senderThreadOk = false;
            var threadPoolThreadOk = false;
            var uiThreadOk = false;
            // act            
            TestHandler.RunInWpfSyncContext(
                async () =>
                {
                    Messenger.Reset();
                    Messenger.Default.Register<Message>(
                        this,
                        ThreadCallbackOption.Sender,
                        msg =>
                        {
                            senderThreadOk = Thread.CurrentThread.ManagedThreadId == senderThreadId;
                            Interlocked.Increment(ref handled);
                        });
                    Messenger.Default.Register<Message>(
                        this,
                        ThreadCallbackOption.ThreadPool,
                        msg =>
                        {
                            threadPoolThreadOk = Thread.CurrentThread.IsThreadPoolThread;
                            Interlocked.Increment(ref handled);
                        });
                    Messenger.Default.Register<Message>(
                        this,
                        ThreadCallbackOption.UiThread,
                        msg =>
                        {
                            uiThreadOk = Thread.CurrentThread.ManagedThreadId == currentThreadId;
                            Interlocked.Increment(ref handled);
                        });
                    await Task.Run(
                        () =>
                        {
                            senderThreadId = Thread.CurrentThread.ManagedThreadId;
                            Messenger.Default.Send(new Message(this));
                        });
                    while (handled < 3)
                    {
                        await Task.Delay(10);
                    }
                });
            // assert
            Assert.IsTrue(senderThreadOk, "Message is not consumed on sender thread.");
            Assert.IsTrue(threadPoolThreadOk, "Message is not consumed on thread-pool-thread.");
            Assert.IsTrue(uiThreadOk, "Message is not consumed on UI thread.");
        }

        #endregion
    }
}