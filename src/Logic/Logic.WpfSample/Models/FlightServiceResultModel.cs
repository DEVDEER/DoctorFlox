using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devdeer.DoctorFlox.Logic.WpfSample.Models
{
    /// <summary>
    /// Represents the structure of the JSON response coming from https://public-api.adsbexchange.com/VirtualRadar/AircraftList.json.
    /// </summary>
    public class FlightServiceResultModel
    {
        #region properties

        public List<FlightDataModel> acList { get; set; }

        public List<FlightFeedModel> feeds { get; set; }

        public int flgH { get; set; }

        public int flgW { get; set; }

        public string lastDv { get; set; }

        public bool showFlg { get; set; }

        public bool showPic { get; set; }

        public bool showSil { get; set; }

        public int shtTrlSec { get; set; }

        public int src { get; set; }

        public int srcFeed { get; set; }

        public long stm { get; set; }

        public int totalAc { get; set; }

        #endregion
    }
}
