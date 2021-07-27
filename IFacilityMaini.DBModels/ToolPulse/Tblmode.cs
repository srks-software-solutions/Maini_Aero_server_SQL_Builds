﻿using System;
using System.Collections.Generic;

namespace IFacilityMaini.DBModels.ToolPulse
{
    public partial class Tblmode
    {
        public int ModeId { get; set; }
        public int MachineId { get; set; }
        public string Mode { get; set; }
        public DateTime InsertedOn { get; set; }
        public int InsertedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string CorrectedDate { get; set; }
        public int IsDeleted { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ColorCode { get; set; }
        public int IsCompleted { get; set; }
        public int? DurationInSec { get; set; }
        public int? ModeMonth { get; set; }
        public int? ModeYear { get; set; }
        public int? ModeWeekNumber { get; set; }
        public int? ModeQuarter { get; set; }
    }
}
