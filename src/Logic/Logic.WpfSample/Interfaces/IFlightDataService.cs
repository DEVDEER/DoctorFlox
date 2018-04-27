using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devdeer.DoctorFlox.Logic.WpfSample.Interfaces
{
    using Models;

    public interface IFlightDataService
    {

        Task<IEnumerable<FlightDataModel>> GetFlightDataByLocationAsync(double lat, double lon);

    }
}
