namespace devdeer.DoctorFlox.Logic.WpfSample.ViewModels
{
    using System;
    using System.Device.Location;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Attributes;

    using Commands;

    using DoctorFlox.Helpers;
    using DoctorFlox.Interfaces;

    using Interfaces;

    using Models;

    /// <summary>
    /// The view model for the flight collection window.
    /// </summary>
    [AssociatedView("devdeer.DoctorFlox.Ui.WpfSample.FlightCollectionWindow")]
    public class FlightCollectionViewModel : BaseCollectionViewModel<FlightDataModel>
    {
        #region member vars

        private bool _enableAutoRefresh;

        private CancellationTokenSource _tokenSource;

        #endregion

        #region constructors and destructors

        public FlightCollectionViewModel(IFlightDataService dataService)
        {
            DataService = dataService;
        }

        public FlightCollectionViewModel(IMessenger messenger, SynchronizationContext synchronizationContext, IFlightDataService dataService) : base(messenger, synchronizationContext)
        {
            DataService = dataService;
        }

        #endregion

        #region methods

        /// <inheritdoc />
        public override void OnWindowClosing()
        {
            base.OnWindowClosing();
            _tokenSource?.Cancel();
        }

        /// <inheritdoc />
        protected override void InitCommands()
        {
            base.InitCommands();
            RefreshCommand = new RelayCommand(() => Task.Run(async () => await FillDataAsync()), () => !IsLoading && !EnableAutoRefresh);
        }

        /// <inheritdoc />
        protected override void InitData()
        {
            base.InitData();
            IsQueryingPosition = true;
            Trace.TraceWarning("Trying to resolve location for current computer.");
            using (var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default))
            {
                if (watcher.TryStart(false, TimeSpan.FromSeconds(3)))
                {
                    if (watcher.Position.Location.IsUnknown)
                    {
                        Trace.TraceWarning("Could not locate the current computer using Location API.");
                    }
                    else
                    {
                        Trace.TraceInformation("Location of computer received.");
                        Lat = watcher.Position.Location.Latitude;
                        Lon = watcher.Position.Location.Longitude;
                    }
                }
            }
            IsQueryingPosition = false;
            FillData();
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            base.InitDesignTimeData();
            FillData();
        }

        /// <summary>
        /// Is used to refresh the data from the <see cref="DataService" />.
        /// </summary>
        private void FillData()
        {
            Task.Run(async () => await FillDataAsync());
        }

        /// <summary>
        /// Is used to refresh the data from the <see cref="DataService" /> asynchronously.
        /// </summary>
        private async Task FillDataAsync()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            var flights = await DataService.GetFlightDataByLocationAsync(Lat, Lon);
            DispatcherHelper.Invoke(() => InitItems(flights));
            LastRefreshTime = DateTime.Now;
            IsLoading = false;
        }

        #endregion

        #region properties

        /// <summary>
        /// The amont of secods after which an automatic call of <see cref="FillDataAsync" />  will happen if
        /// <see cref="EnableAutoRefresh" /> is set to <c>true</c>.
        /// </summary>
        public int AutoRefreshSeconds { get; set; } = 10;

        /// <summary>
        /// The data service to use for retrieving flight data.
        /// </summary>
        public IFlightDataService DataService { get; }

        /// <summary>
        /// Indicates if the view will retrieve new data after <see cref="AutoRefreshSeconds" /> seconds.
        /// </summary>
        public bool EnableAutoRefresh
        {
            get => _enableAutoRefresh;
            set
            {
                _enableAutoRefresh = value;
                if (_enableAutoRefresh)
                {
                    _tokenSource = new CancellationTokenSource();
                    var token = _tokenSource.Token;
                    Task.Run(
                        async () =>
                        {
                            while (!token.IsCancellationRequested)
                            {
                                await FillDataAsync();
                                await Task.Delay(TimeSpan.FromSeconds(AutoRefreshSeconds));
                            }
                        },
                        token).ContinueWith(
                        t =>
                        {
                            Trace.TraceInformation("AutoRefresh disabled.");
                        });
                }
                else
                {
                    _tokenSource?.Cancel();
                }
            }
        }

        /// <summary>
        /// Indicates if the view is loading data currently.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// The opposite value of <see cref="IsLoading" /> for binding purposes.
        /// </summary>
        public bool IsNotLoading => !IsLoading;

        /// <summary>
        /// Indicates if the location-tracking process is ongoing currently.
        /// </summary>
        public bool IsQueryingPosition { get; private set; }

        /// <summary>
        /// The timestamp on which the last result was received.
        /// </summary>
        public DateTime? LastRefreshTime { get; private set; }

        /// <summary>
        /// The latitude the user chose.
        /// </summary>
        public double Lat { get; set; } = 33.433638;

        /// <summary>
        /// The longitude the user chose.
        /// </summary>
        public double Lon { get; set; } = -112.008113;

        /// <summary>
        /// Can be used to manually trigger data reload.
        /// </summary>
        public RelayCommand RefreshCommand { get; private set; }

        /// <summary>
        /// The resolved status.
        /// </summary>
        public string Status
        {
            get
            {
                if (IsQueryingPosition)
                {
                    return "Trying to resolve location...";
                }
                if (IsLoading)
                {
                    return "Loading data...";
                }
                if (EnableAutoRefresh)
                {
                    return "Data loaded. Waiting for refresh.";
                }
                return "OK";
            }
        }

        #endregion
    }
}