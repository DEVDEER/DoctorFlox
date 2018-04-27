namespace devdeer.DoctorFlox.Logic.WpfSample.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a data feed inside of the <see cref="FlightServiceResultModel"/>.
    /// </summary>
    public class FlightFeedModel
    {
        #region properties

        public int id { get; set; }

        public string name { get; set; }

        public bool polarPlot { get; set; }

        #endregion
    }
}