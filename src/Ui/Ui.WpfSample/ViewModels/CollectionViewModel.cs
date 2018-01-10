namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Attributes;

    using Interfaces;

    using Models;

    /// <summary>
    /// The view model for the collection window.
    /// </summary>
    [AssociatedView("devdeer.DoctorFlox.Ui.WpfSample.CollectionWindow")]
    public class CollectionViewModel : BaseCollectionViewModel<ChildDataModel>
    {
        #region constructors and destructors

        public CollectionViewModel()
        {
        }

        public CollectionViewModel(IMessenger messenger) : base(messenger)
        {
        }

        public CollectionViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            InitItems(GetSampleItems(1000));
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            base.InitDesignTimeData();
            InitItemsDesignTime(GetSampleItems(10));
        }

        /// <summary>
        /// Retrieves a list of <paramref name="amount" /> items with random values.
        /// </summary>
        /// <remarks>
        /// Uses yield to return the items on demand.
        /// </remarks>
        /// <param name="amount">The amount of items to retrieve.</param>
        /// <returns>The items in a yielded manner.</returns>
        private IEnumerable<ChildDataModel> GetSampleItems(int amount)
        {
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < amount; i++)
            {
                yield return new ChildDataModel
                {
                    Firstname = Guid.NewGuid().ToString("N").Substring(0, 20),
                    Lastname = Guid.NewGuid().ToString("N").Substring(0, 20),
                    Age = random.Next(2, 90)
                };
            }
        }

        #endregion
    }
}