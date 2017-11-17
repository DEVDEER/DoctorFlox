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

        /// <inheritdoc />
        public CollectionViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            var data = new List<ChildDataModel>();
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < 1000; i++)
            {
                data.Add(
                    new ChildDataModel
                    {
                        Firstname = Guid.NewGuid().ToString("N").Substring(0, 20),
                        Lastname = Guid.NewGuid().ToString("N").Substring(0, 20),
                        Age = random.Next(2, 90)
                    });
            }
            InitItems(data);
        }

        #endregion
    }
}