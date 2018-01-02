namespace devdeer.DoctorFlox.Logic.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Data;

    using Helpers;

    using Interfaces;

    /// <summary>
    /// Abstract base class for all view models which want to display a bunch of items of the
    /// type <typeparamref name="TItem" />.
    /// </summary>
    /// <typeparam name="TItem">The type of the items to display.</typeparam>
    public abstract class BaseCollectionViewModel<TItem> : BaseViewModel
        where TItem : INotifyPropertyChanged
    {
        #region member vars

        /// <summary>
        /// Is used by the synchronization logic for multithreading.
        /// </summary>
        private readonly object _listLock = new object();

        private ListCollectionView _fullView;

        private bool _isInPreview;

        #endregion

        #region events

        /// <summary>
        /// Occurs after the <see cref="ItemsView" /> has go a new value.
        /// </summary>
        public event EventHandler ItemsViewChanged;

        #endregion

        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseCollectionViewModel()
        {
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        public BaseCollectionViewModel(IMessenger messenger) : base(messenger)
        {
        }

        /// <summary>
        /// Constructor for passing <paramref name="messenger" /> and <paramref name="synchronizationContext" /> from outside.
        /// </summary>
        /// <param name="messenger">The messenger to use.</param>
        /// <param name="synchronizationContext">The sync context to use.</param>
        public BaseCollectionViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region methods

        /// <summary>
        /// Adds a single <paramref name="item" /> to the internal data collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(TItem item)
        {
            if (Items == null)
            {
                InitItems(Enumerable.Empty<TItem>());
            }
            item.PropertyChanged += OnItemPropertyChanged;
            Items.Add(item);
        }

        /// <summary>
        /// Adds a bunch of <paramref name="items" /> to the internal data collection.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="Add" /> for each item internally.
        /// </remarks>
        /// <param name="items">The items to add.</param>
        public void AddRange(IEnumerable<TItem> items)
        {
            Items.CollectionChanged -= OnItemsCollectionChanged;
            foreach (var item in items)
            {
                Add(item);
            }
            Items.CollectionChanged += OnItemsCollectionChanged;
            if (_isInPreview)
            {
                ItemsView.Refresh();
            }
            _fullView.Refresh();
        }

        /// <summary>
        /// Clears the internal list of items.
        /// </summary>
        public void Clear()
        {
            Items?.Clear();
        }

        /// <summary>
        /// Retrieves the index of the first occurrence <paramref name="item" /> in the internal data collection or -1.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns>The index or -1 if items is <c>null</c>.</returns>
        public int IndexOf(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return Items?.IndexOf(item) ?? -1;
        }

        /// <summary>
        /// Resets the <see cref="Items" /> and <see cref="ItemsView" /> in one step.
        /// </summary>
        /// <param name="items">The items to take as the current data source.</param>
        /// <param name="performResetBefore">
        /// If set tot <c>true</c> <see cref="Reset" /> will be called before any init (defaults
        /// to <c>true</c>).
        /// </param>
        public void InitItems(IEnumerable<TItem> items, bool performResetBefore = true)
        {
            if (performResetBefore)
            {
                Reset();
            }
            Items = new ObservableCollection<TItem>(items);
            BindingOperations.EnableCollectionSynchronization(Items, _listLock);
            // connect events for any added item
            foreach (var item in Items)
            {
                item.PropertyChanged += OnItemPropertyChanged;
            }
            // ensure that future items are connected and items that are removed are disconnected from events
            Items.CollectionChanged += OnItemsCollectionChanged;
            // create the bindable view representation of the data
            _fullView = CollectionViewSource.GetDefaultView(Items) as ListCollectionView;
            _isInPreview = false;
            if (FastPreviewRows.HasValue && FastPreviewRows.Value > 0)
            {
                // start with a 
                _isInPreview = true;
                var view = new ListCollectionView(Items.Take(FastPreviewRows.Value).ToList());
                SetItemsView(view);
            }
            else
            {
                // just show all records at once
                SetItemsView(_fullView);
            }
        }

        /// <summary>
        /// Reacts on any property change of any item in <see cref="Items" />.
        /// </summary>
        /// <param name="sender">The item that has a changed property.</param>
        /// <param name="e">The event args.</param>
        public virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (RefreshViewOnItemChange)
            {
                ItemsView.Refresh();
            }
        }

        /// <summary>
        /// Removes a given <paramref name="item" /> from the internal data collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void Remove(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Items?.Remove(item);
        }

        /// <summary>
        /// Replaces all matches of <paramref name="matchPredicate" /> in the internal data collection with the
        /// <paramref name="newItem" />.
        /// </summary>
        /// <param name="newItem">The item which to insert instead of the old item(s).</param>
        /// <param name="matchPredicate">A predicate which will retrieve the old items to replace.</param>
        public void Replace(TItem newItem, Func<TItem, bool> matchPredicate)
        {
            if (!Items.Any())
            {
                // nothing to do
                return;
            }
            var item = Items.FirstOrDefault(matchPredicate);
            var lastIndex = -1;
            while (item != null)
            {
                var index = Items.IndexOf(item);
                if (index < 0 || index == lastIndex)
                {
                    // either the item wasn't found or it is the same as the
                    // last item
                    break;
                }
                // retrieve index and replace item
                lastIndex = index;
                Items[index] = newItem;
                item = Items.FirstOrDefault(matchPredicate);
            }
        }

        /// <summary>
        /// Performs a complete nulling of items.
        /// </summary>
        public void Reset()
        {
            Clear();
            Items = null;
            ItemsView = null;
        }

        /// <summary>
        /// Forces the <see cref="ItemsView" /> to use the view containing all items.
        /// </summary>
        /// <remarks>
        /// See <see cref="FastPreviewRows" /> for details of this.
        /// </remarks>
        public void SwitchToFullView()
        {
            if (!FastPreviewRows.HasValue || FastPreviewRows.Value <= 0)
            {
                return;
            }
            SetItemsView(_fullView);
            _isInPreview = false;
        }

        /// <inheritdoc />
        protected override void OnBeforeWatchedPropertiesDefined(List<PropertyInfo> collectedProperties)
        {
            base.OnBeforeWatchedPropertiesDefined(collectedProperties);
            var unwantedProps = new[] { "Item" };
            var propsToRemove = collectedProperties.Where(p => unwantedProps.Contains(p.Name)).ToList();
            propsToRemove.ForEach(p => collectedProperties.Remove(p));
        }

        /// <summary>
        /// Use this method to pass the content for <see cref="ItemsView" /> at design time.
        /// </summary>
        /// <param name="designTimeData">The data for the <see cref="ItemsView" />.</param>
        protected void InitItemsDesignTime(IEnumerable<TItem> designTimeData)
        {
            if (!IsInDesignMode)
            {
                return;
            }
            Items = new ObservableCollection<TItem>(designTimeData);
            ItemsView = new ListCollectionView(Items.ToList());
        }

        /// <summary>
        /// Handler for the collection changed event of the internal data collection.
        /// </summary>
        /// <param name="sender">The internal data collection.</param>
        /// <param name="e">The event args.</param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged added in e.NewItems)
                {
                    added.PropertyChanged += OnItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged removed in e.OldItems)
                {
                    removed.PropertyChanged -= OnItemPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="ItemsView" /> and hooks all needed logic in.
        /// </summary>
        /// <param name="view">The view to apply.</param>
        private void SetItemsView(ListCollectionView view)
        {
            DispatcherHelper.BeginInvoke(() => ItemsView = view);
            // inform about the change
            ItemsViewChanged?.Invoke(this, EventArgs.Empty);
            if (ItemsView == null)
            {
                // very strange because it indicates that GetDefaultView could not be casted to ListCollectionView
                return;
            }
            // ensure that the CurrentItem property will notify about the fact that the view changed it's current item
            ItemsView.CurrentChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(CurrentItem));
            };
        }

        #endregion

        #region properties

        /// <summary>
        /// The amount of data items currently present.
        /// </summary>
        public int Count => Items?.Count ?? 0;

        /// <summary>
        /// Gets/sets the current selected item.
        /// </summary>
        public TItem CurrentItem
        {
            get
            {
                var result = default(TItem);
                DispatcherHelper.BeginInvoke(
                    () =>
                    {
                        result = (TItem)ItemsView.CurrentItem;
                    });
                return result;
            }
            set
            {
                DispatcherHelper.BeginInvoke(
                    () =>
                    {
                        ItemsView.MoveCurrentTo(value);
                        OnPropertyChanged();
                    });
            }
        }

        /// <summary>
        /// If set to a value greater than 0 this will create a view which previews
        /// the amount of rows before binding against all others.
        /// </summary>
        /// <remarks>
        /// The caller has to decide when to call <see cref="SwitchToFullView" /> to jump off
        /// from the preview.
        /// </remarks>
        public int? FastPreviewRows { get; set; }

        /// <summary>
        /// Gets/sets the item on a given <paramref name="index" />.
        /// </summary>
        /// <param name="index">The 0-based index of the item.</param>
        /// <exception cref="InvalidOperationException">Is thrown when the internal data collection is <c>null</c>.</exception>
        public TItem this[int index]
        {
            get
            {
                if (Items == null)
                {
                    throw new InvalidOperationException("Items not initialized yet.");
                }
                return Items[index];
            }
            set
            {
                if (Items == null)
                {
                    throw new InvalidOperationException("Items not initialized yet.");
                }
                Items[index] = value;
            }
        }

        /// <summary>
        /// Retrieves an enumerable version of the internal data collection.
        /// </summary>
        public IEnumerable<TItem> ItemsEnum => Items?.AsEnumerable();

        /// <summary>
        /// Retrieves a queryable version of the internal data collection.
        /// </summary>
        public IQueryable<TItem> ItemsQuery => Items?.AsQueryable();

        /// <summary>
        /// The bindable view of the internal <see cref="Items" />.
        /// </summary>
        public ListCollectionView ItemsView { get; set; }

        /// <summary>
        /// If set to <c>true</c> a refresh on <see cref="ItemsView" /> will be called every time a property of one
        /// of the item changes.
        /// </summary>
        public bool RefreshViewOnItemChange { get; set; }

        /// <inheritdoc />
        protected override bool IgnoreInternalModels => true;

        /// <summary>
        /// The internal list of data.
        /// </summary>
        private ObservableCollection<TItem> Items { get; set; }

        #endregion
    }
}