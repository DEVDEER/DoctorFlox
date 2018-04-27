namespace devdeer.DoctorFlox.Logic.WpfSample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents one item in the <see cref="FlightServiceResultModel" />.
    /// </summary>
    public class FlightDataModel : BaseDataModel
    {
        #region properties

        public int Alt { get; set; }

        public int AltT { get; set; }

        public bool Bad { get; set; }

        public float Brng { get; set; }

        public string Call { get; set; }

        public bool CallSus { get; set; }

        public int CMsgs { get; set; }

        public string CNum { get; set; }

        public string Cou { get; set; }

        public float Dst { get; set; }

        public string Engines { get; set; }

        public int EngMount { get; set; }

        public int EngType { get; set; }

        public int FlightsCount { get; set; }

        public string From { get; set; }

        public DateTime FSeen { get; set; }

        public int GAlt { get; set; }

        public bool Gnd { get; set; }

        public bool HasPic { get; set; }

        public bool HasSig { get; set; }

        public bool Help { get; set; }

        public string Icao { get; set; }

        public int Id { get; set; }

        public float InHg { get; set; }

        public bool Interested { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        public string Man { get; set; }

        public string Mdl { get; set; }

        public bool Mil { get; set; }

        public bool Mlat { get; set; }

        public string Op { get; set; }

        public string OpIcao { get; set; }

        public int PicX { get; set; }

        public int PicY { get; set; }

        public bool PosStale { get; set; }

        public long PosTime { get; set; }

        public int Rcvr { get; set; }

        public string Reg { get; set; }

        public float Spd { get; set; }

        public int SpdTyp { get; set; }

        public int Species { get; set; }

        public string Sqk { get; set; }

        public List<string> Stops { get; set; }

        public int TAlt { get; set; }

        public bool Tisb { get; set; }

        public string To { get; set; }

        public float Trak { get; set; }

        public bool TrkH { get; set; }

        public int Trt { get; set; }

        public int TSecs { get; set; }

        public float TTrk { get; set; }

        public string Type { get; set; }

        public int Vsi { get; set; }

        public int VsiT { get; set; }

        public int WTC { get; set; }

        public string Year { get; set; }

        #endregion
    }
}