using IFacilityMaini.DAL.Helpers;
using IFacilityMaini.DAL.Resource;
using IFacilityMaini.DBModels;
using IFacilityMaini.EntityModels;
using IFacilityMaini.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static IFacilityMaini.EntityModels.CommonEntity;

namespace IFacilityMaini.DAL
{
    public class VendorDAL : IVendor
    {
        unitworksccsContext db = new unitworksccsContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(VendorDAL));
        public static IConfiguration configuration;
        private readonly AppSettings appSettings;

        public VendorDAL(unitworksccsContext _db, IConfiguration _configuration, IOptions<AppSettings> _appSettings)
        {
            db = _db;
            configuration = _configuration;
            appSettings = _appSettings.Value;
        }

        /// <summary>
        /// Upload Vendor Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse UploadVendorDetails(List<VendorEntity> data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblVendor.Where(m => m.IsDeleted == 0).ToList();
                db.TblVendor.RemoveRange(check);
                db.SaveChanges();

                foreach(var item in data)
                {
                    TblVendor tblVendor = new TblVendor();
                    tblVendor.Vendor = item.vendor;
                    tblVendor.VendorName = item.vendorName;
                    tblVendor.IsDeleted = 0;
                    tblVendor.CreatedOn = DateTime.Now;
                    db.TblVendor.Add(tblVendor);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
            }
            catch (Exception e)
            {
                log.Error(e); if (e.InnerException != null) { log.Error(e.InnerException.ToString()); }
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// View Vendor Details
        /// </summary>
        /// <returns></returns>
        public CommonResponse ViewVendorDetails()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblVendor
                             where wf.IsDeleted == 0
                             select new
                             {
                                 vendor = wf.Vendor,
                                 vendorId = wf.VendorId,
                                 vendorName = wf.VendorName
                             }).ToList();
                if(check.Count > 0)
                {
                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = ResourceResponse.NoItemsFound;
                }
            }
            catch (Exception e)
            {
                log.Error(e); if (e.InnerException != null) { log.Error(e.InnerException.ToString()); }
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }
    }
}
