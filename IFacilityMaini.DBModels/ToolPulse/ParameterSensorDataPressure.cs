﻿using System;
using System.Collections.Generic;

namespace IFacilityMaini.DBModels.ToolPulse
{
    public partial class ParameterSensorDataPressure
    {
        public int ParameterSensorDataPressureId { get; set; }
        public int? ParameterSensorDataId { get; set; }
        public decimal? PressureValue { get; set; }
    }
}
