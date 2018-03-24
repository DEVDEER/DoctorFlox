namespace devdeer.DoctorFlox.Logic.WpfSample.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    using Interfaces;

    using Models;

    using RestSharp;

    public class FlightDataService : IFlightDataService

    {
        #region member vars

        private readonly Lazy<RestClient> _apiClientFactory = new Lazy<RestClient>(() => new RestClient(WebConfigurationManager.AppSettings["ApiBaseUrl"]));

        #endregion

        #region explicit interfaces

        public async Task<IEnumerable<FlightDataModel>> GetFlightDataByLocationAsync(double lat, double lon)
        {
            var request = new RestRequest(
                $"VirtualRadar/AircraftList.json?lat={lat.ToString(CultureInfo.InvariantCulture)}&lng={lon.ToString(CultureInfo.InvariantCulture)}&fDstL=0&fDstU=100",
                Method.GET);
            var result = await ApiClient.ExecuteTaskAsync<FlightServiceResultModel>(request);
            return result.Data.acList;
        }

        #endregion

        #region properties

        private RestClient ApiClient => _apiClientFactory.Value;

        #endregion
    }
}