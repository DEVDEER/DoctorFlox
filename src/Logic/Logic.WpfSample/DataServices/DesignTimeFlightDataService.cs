namespace devdeer.DoctorFlox.Logic.WpfSample.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;

    using Models;

    public class DesignTimeFlightDataService : IFlightDataService
    {
        #region explicit interfaces

        /// <inheritdoc />
        public Task<IEnumerable<FlightDataModel>> GetFlightDataByLocationAsync(double lat, double lon)
        {
            return Task.FromResult(
                new List<FlightDataModel>
                {
                    new FlightDataModel
                    {
                        Id = 1,
                        Alt = 100000,
                        Spd = 300
                    }
                }.AsEnumerable());
        }

        #endregion
    }
}