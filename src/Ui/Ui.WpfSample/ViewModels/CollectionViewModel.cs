namespace devdeer.DoctorFlox.Ui.WpfSample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Logic.Wpf;
    using Logic.Wpf.Interfaces;

    using Models;

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
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
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