using System;
using System.Collections.Generic;

namespace IFacilityMaini.DBModels
{
    public partial class SamplingFrequency1
    {
        public int Id { get; set; }
        public string Lotsize { get; set; }
        public string Reduced { get; set; }
        public string Normal { get; set; }
        public string Tightened { get; set; }
    }
}
