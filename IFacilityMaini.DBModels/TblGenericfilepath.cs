﻿using System;
using System.Collections.Generic;

namespace IFacilityMaini.DBModels
{
    public partial class TblGenericfilepath
    {
        public int FilePathId { get; set; }
        public string CompleteFilePath { get; set; }
        public string FilePathDesc { get; set; }
        public string FilePathDet { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? TypeofFilePath { get; set; }
    }
}
