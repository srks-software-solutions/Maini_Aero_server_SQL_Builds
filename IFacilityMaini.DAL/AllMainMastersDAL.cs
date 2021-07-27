﻿using IFacilityMaini.DAL.Helpers;
using IFacilityMaini.DAL.Resource;
using IFacilityMaini.DBModels;
using IFacilityMaini.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static IFacilityMaini.EntityModels.AllMainMastersEntity;
using static IFacilityMaini.EntityModels.CommonEntity;

namespace IFacilityMaini.DAL
{
    public class AllMainMastersDAL : IAllMainMasters
    {


        unitworksccsContext db = new unitworksccsContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AllMainMastersDAL));
        public static IConfiguration configuration;
        private readonly AppSettings appSettings;

        public AllMainMastersDAL(unitworksccsContext _db, IConfiguration _configuration, IOptions<AppSettings> _appSettings)
        {
            db = _db;
            configuration = _configuration;
            appSettings = _appSettings.Value;
        }

        #region  1) Plant Master 

        public CommonResponse AddUpdatePlant(addPlantDet data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = db.Tblplant.Where(m => m.PlantId == data.plantId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    Tblplant tblPlantdet = new Tblplant();
                    tblPlantdet.PlantName = data.plantName;
                    tblPlantdet.PlantDesc = data.plantName;
                    tblPlantdet.PlantDisplayName = data.plantName;
                    tblPlantdet.PlantCode = data.plantName;
                    tblPlantdet.PlantCode = data.plantCode;
                    tblPlantdet.PlantLocation = data.plantLocation;
                    tblPlantdet.IsDeleted = 0;
                    tblPlantdet.CreatedBy = 1;
                    tblPlantdet.CreatedOn = DateTime.Now;
                    db.Tblplant.Add(tblPlantdet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.PlantName = data.plantName;
                    check.PlantDesc = data.plantName;
                    check.PlantDisplayName = data.plantName;
                    check.PlantCode = data.plantName;
                    check.PlantCode = data.plantCode;
                    check.PlantLocation = data.plantLocation;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultiplePlant()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblplant
                             where wf.IsDeleted == 0
                             select new
                             {
                                 plantId = wf.PlantId,
                                 plantName = wf.PlantName,
                                 plantDesc = wf.PlantDesc,
                                 plantDisplayName = wf.PlantDisplayName,
                                 plantCode = wf.PlantCode,
                                 plantLocation = wf.PlantLocation,

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse ViewMultiplePlantById(int plantId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = (from wf in db.Tblplant
                             where wf.IsDeleted == 0 && wf.PlantId == plantId
                             select new
                             {
                                 plantId = wf.PlantId,
                                 plantName = wf.PlantName,
                                 plantDesc = wf.PlantDesc,
                                 plantDisplayName = wf.PlantDisplayName,
                                 plantCode = wf.PlantCode,
                                 plantLocation = wf.PlantLocation,

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeletePlant(int plantId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.Tblplant.Where(m => m.PlantId == plantId && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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

        #endregion

        #region 2) Cell Master 

        public CommonResponse AddUpdateCell(addCellDet data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.Tblshop.Where(m => m.ShopId == data.cellId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {
                    Tblshop tblShopdet = new Tblshop();
                    tblShopdet.ShopName = data.cellName;
                    tblShopdet.ShopDesc = data.cellName;
                    tblShopdet.Shopdisplayname = data.cellName;
                    tblShopdet.PlantId = data.plantId;
                    tblShopdet.IsDeleted = 0;
                    tblShopdet.CreatedBy = 1;
                    tblShopdet.CreatedOn = DateTime.Now;
                    db.Tblshop.Add(tblShopdet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.ShopName = data.cellName;
                    check.ShopDesc = data.cellName;
                    check.Shopdisplayname = data.cellName;
                    check.PlantId = data.plantId;

                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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

        public CommonResponse ViewMultipleCell()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblshop
                             where wf.IsDeleted == 0
                             select new
                             {
                                 cellId = wf.ShopId,
                                 cellName = wf.ShopName,
                                 cellDesc = wf.ShopDesc,
                                 cellDisplayName = wf.Shopdisplayname,
                                 plantId = wf.PlantId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantCode).FirstOrDefault()

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse ViewMultipleCellByPlantId(int plantId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblshop
                             where wf.IsDeleted == 0 && wf.PlantId == plantId
                             select new
                             {
                                 cellId = wf.ShopId,
                                 cellName = wf.ShopName,
                                 cellDesc = wf.ShopDesc,
                                 cellDisplayName = wf.Shopdisplayname,
                                 plantId = wf.PlantId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantName).FirstOrDefault()

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeleteCell(int CellId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.Tblshop.Where(m => m.ShopId == CellId && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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

        #endregion

        #region 3)  Sub Cell Master 

        public CommonResponse AddUpdateSubCell(addSubCellDet data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = db.Tblcell.Where(m => m.CellId == data.subCellId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    Tblcell tblCelldet = new Tblcell();
                    tblCelldet.CellName = data.subCellName;
                    tblCelldet.CellDesc = data.subCellName;
                    tblCelldet.CelldisplayName = data.subCellName;
                    tblCelldet.PlantId = data.plantId;
                    tblCelldet.ShopId = data.cellId;
                    tblCelldet.IsDeleted = 0;
                    tblCelldet.CreatedBy = 1;
                    tblCelldet.CreatedOn = DateTime.Now;
                    db.Tblcell.Add(tblCelldet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.CellName = data.subCellName;
                    check.CellDesc = data.subCellName;
                    check.CelldisplayName = data.subCellName;
                    check.PlantId = data.plantId;
                    check.ShopId = data.cellId;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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

        public CommonResponse ViewMultipleSubCell()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblcell
                             where wf.IsDeleted == 0
                             select new
                             {
                                 subCellId = wf.CellId,
                                 subCellName = wf.CellName,
                                 subCellDesc = wf.CellDesc,
                                 subcellDisplayName = wf.CelldisplayName,
                                 plantId = wf.PlantId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantCode).FirstOrDefault(),
                                 cellId = wf.ShopId,
                                 cellName = db.Tblshop.Where(m => m.ShopId == wf.ShopId).Select(m => m.ShopName).FirstOrDefault()

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse ViewMultipleSubCellByCellId(int cellId)
        {


            CommonResponse obj = new CommonResponse();
            try
            {

                var check = (from wf in db.Tblcell
                             where wf.IsDeleted == 0 && wf.ShopId == cellId
                             select new
                             {
                                 subCellId = wf.CellId,
                                 subCellName = wf.CellName,
                                 subCellDesc = wf.CellDesc,
                                 subcellDisplayName = wf.CelldisplayName,
                                 plantId = wf.PlantId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantName).FirstOrDefault(),
                                 cellId = wf.ShopId,
                                 cellName = db.Tblshop.Where(m => m.ShopId == wf.ShopId).Select(m => m.ShopName).FirstOrDefault()

                             }).ToList();



                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeleteSubCell(int subCellId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.Tblcell.Where(m => m.CellId == subCellId && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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

        #endregion

        #region 4) Defect Code Master 

        public CommonResponse AddUpdateDefectCode(addDefectCodeDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == data.defectCodeId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblDefectCodeMaster tblDefectCodeDet = new TblDefectCodeMaster();
                    tblDefectCodeDet.DefectCodeName = data.defectCodeName;
                    tblDefectCodeDet.DefectCodeDesc = data.defectCodeDesc;

                    tblDefectCodeDet.IsDeleted = 0;
                    tblDefectCodeDet.CreatedBy = 1;
                    tblDefectCodeDet.CreatedOn = DateTime.Now;
                    db.TblDefectCodeMaster.Add(tblDefectCodeDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.DefectCodeName = data.defectCodeName;
                    check.DefectCodeDesc = data.defectCodeDesc;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleDefectCode()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblDefectCodeMaster
                             where wf.IsDeleted == 0
                             select new
                             {
                                 defectCodeId = wf.DefectCodeId,
                                 defectCodeName = wf.DefectCodeName,
                                 defectCodeDesc = wf.DefectCodeDesc

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeleteDefectCode(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 5) Category  Master 

        public CommonResponse AddUpdateCategoryMaster(addCategoryMasterDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {
                var check = db.TblCategoryMaster.Where(m => m.CategoryId == data.categoryId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblCategoryMaster tblCategoryMasterDet = new TblCategoryMaster();
                    tblCategoryMasterDet.CategoryName = data.categoryName;
                    tblCategoryMasterDet.CategoryDesc = data.categoryName;
                    tblCategoryMasterDet.IsDeleted = 0;
                    tblCategoryMasterDet.CreatedBy = 1;
                    tblCategoryMasterDet.CreatedOn = DateTime.Now;
                    db.TblCategoryMaster.Add(tblCategoryMasterDet);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {
                    check.CategoryName = data.categoryName;
                    check.CategoryDesc = data.categoryName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;
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


        public CommonResponse ViewMultipleCategoryMaster()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblCategoryMaster
                             where wf.IsDeleted == 0
                             select new
                             {
                                 categoryId = wf.CategoryId,
                                 categoryName = wf.CategoryName,
                                 cstegoryDesc = wf.CategoryDesc

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeleteCategoryMaster(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblCategoryMaster.Where(m => m.CategoryId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 6) Roles Master

        public CommonResponse AddUpdateRoles(addRolesDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.Tblroles.Where(m => m.RoleId == data.roleId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    Tblroles tblRolesDet = new Tblroles();
                    tblRolesDet.RoleName = data.roleName;
                    tblRolesDet.RoleDesc = data.roleName;
                    tblRolesDet.RoleDisplayName = data.roleName;
                    tblRolesDet.IsDeleted = 0;
                    tblRolesDet.CreatedBy = 1;
                    tblRolesDet.CreatedOn = DateTime.Now;
                    db.Tblroles.Add(tblRolesDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.RoleName = data.roleName;
                    check.RoleDesc = data.roleName;
                    check.RoleDisplayName = data.roleName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleRoles()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblroles
                             where wf.IsDeleted == 0
                             select new
                             {
                                 roleId = wf.RoleId,
                                 roleName = wf.RoleName,
                                 roleDesc = wf.RoleDesc,
                                 roleDisplayName = wf.RoleDisplayName,
                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteRoles(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.Tblroles.Where(m => m.RoleId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 7) Business  Master 

        public CommonResponse AddUpdateBusiness(addBusinessDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblBusinessDetails.Where(m => m.BusinessId == data.businessId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblBusinessDetails tblBusinessDet = new TblBusinessDetails();
                    tblBusinessDet.BusinessName = data.businessName;
                    tblBusinessDet.BusinessDesc = data.businessName;
                    tblBusinessDet.IsDeleted = 0;
                    tblBusinessDet.Createdby = 1;
                    tblBusinessDet.CreatedOn = DateTime.Now;
                    db.TblBusinessDetails.Add(tblBusinessDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.BusinessName = data.businessName;
                    check.BusinessDesc = data.businessName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleBusiness()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblBusinessDetails
                             where wf.IsDeleted == 0
                             select new
                             {
                                 businessId = wf.BusinessId,
                                 businessName = wf.BusinessName,
                                 businessDesc = wf.BusinessDesc

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteBusiness(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblBusinessDetails.Where(m => m.BusinessId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region  8) Department  Master 

        public CommonResponse AddUpdateDepartment(addDepartmentDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblDepartmentDetails.Where(m => m.DepartmentId == data.departmentId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblDepartmentDetails tblDepartmentDet = new TblDepartmentDetails();
                    tblDepartmentDet.DepartmentName = data.departmentName;
                    tblDepartmentDet.DepartmentDesc = data.departmentName;
                    tblDepartmentDet.IsDeleted = 0;
                    tblDepartmentDet.CreatedBy = 1;
                    tblDepartmentDet.CreatedOn = DateTime.Now;
                    db.TblDepartmentDetails.Add(tblDepartmentDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.DepartmentName = data.departmentName;
                    check.DepartmentDesc = data.departmentName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleDepartment()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblDepartmentDetails
                             where wf.IsDeleted == 0
                             select new
                             {
                                 departmentId = wf.DepartmentId,
                                 departmentName = wf.DepartmentName,
                                 departmentDesc = wf.DepartmentDesc

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteDepartment(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblDepartmentDetails.Where(m => m.DepartmentId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 9) Employee Category  Master 

        public CommonResponse AddUpdateEmployeeCategory(addEmpCategoryDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblCategoryDetails.Where(m => m.CategoryId == data.categoryId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblCategoryDetails tblEmpCatDet = new TblCategoryDetails();
                    tblEmpCatDet.CategoryName = data.categoryName;
                    tblEmpCatDet.CategoryIdDesc = data.categoryName;
                    tblEmpCatDet.IsDeleted = 0;
                    tblEmpCatDet.CreatedBy = 1;
                    tblEmpCatDet.CreatedOn = DateTime.Now;
                    db.TblCategoryDetails.Add(tblEmpCatDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.CategoryName = data.categoryName;
                    check.CategoryIdDesc = data.categoryName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleEmployeeCategory()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblCategoryDetails
                             where wf.IsDeleted == 0
                             select new
                             {
                                 categoryId = wf.CategoryId,
                                 categoryName = wf.CategoryName,
                                 categoryDesc = wf.CategoryIdDesc

                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteEmployeeCategory(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblCategoryDetails.Where(m => m.CategoryId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 10) Machine Category Master 

        public CommonResponse AddUpdateMachineCategory(addMacCatDet data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = db.TblMachineCategoryNames.Where(m => m.MachineCategoryId == data.machineCategoryId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblMachineCategoryNames tblMacCatdet = new TblMachineCategoryNames();
                    tblMacCatdet.MachineCategoryName = data.machineCategoryName;
                    tblMacCatdet.CategoryDesc = data.machineCategoryName;
                    tblMacCatdet.IsDeleted = 0;
                    tblMacCatdet.CreatedBy = 1;
                    tblMacCatdet.CreatedOn = DateTime.Now;
                    db.TblMachineCategoryNames.Add(tblMacCatdet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.MachineCategoryName = data.machineCategoryName;
                    check.CategoryDesc = data.machineCategoryName;
                    check.NodifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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
        public CommonResponse ViewMultipleMachineCategory()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblMachineCategoryNames
                             where wf.IsDeleted == 0
                             select new
                             {
                                 machineCategoryId = wf.MachineCategoryId,
                                 machineCategoryName = wf.MachineCategoryName,
                                 machineCategoryDesc = wf.CategoryDesc,
                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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
        public CommonResponse ViewMultipleMachineCategoryByPlantId(int plantId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = (from wf in db.TblMachineCategoryNames
                             where wf.IsDeleted == 0 && wf.PlantId == plantId
                             select new
                             {
                                 machineCategoryId = wf.MachineCategoryId,
                                 machineCategoryName = wf.MachineCategoryName,
                                 machineCategoryDesc = wf.CategoryDesc,
                             }).ToList();

                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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
        public CommonResponse DeleteMachineCategory(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblMachineCategoryNames.Where(m => m.MachineCategoryId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.NodifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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

        #endregion

        #region  11) Machine Make Master 

        public CommonResponse AddUpdateMachineMake(addMacMakeDet data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                var check = db.TblMachineMakeDetails.Where(m => m.MakeId == data.makeId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblMachineMakeDetails tblMacMakedet = new TblMachineMakeDetails();
                    tblMacMakedet.MakeName = data.makeName;
                    tblMacMakedet.MachineCategoryId = data.machineCategoryId;
                    tblMacMakedet.IsDeleted = 0;
                    tblMacMakedet.CreatedBy = 1;
                    tblMacMakedet.CreatedOn = DateTime.Now;
                    db.TblMachineMakeDetails.Add(tblMacMakedet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {
                    check.MakeName = data.makeName;
                    check.MachineCategoryId = data.machineCategoryId;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;
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

        public CommonResponse ViewMultipleMachineMake()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblMachineMakeDetails
                             where wf.IsDeleted == 0
                             select new
                             {
                                 makeId = wf.MakeId,
                                 makeName = wf.MakeName,
                                 machineCategoryId = wf.MachineCategoryId,
                                 machineCategoryName = db.TblMachineCategoryNames.Where(m => m.MachineCategoryId == wf.MachineCategoryId).Select(m => m.MachineCategoryName).FirstOrDefault()
                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse ViewMultipleMachineMakeByPlantIdCategoryId(int plantId, int categoryId)
        {


            CommonResponse obj = new CommonResponse();
            try
            {

                var check = (from wf in db.TblMachineMakeDetails
                             where wf.IsDeleted == 0 && wf.PlantId == plantId && wf.MachineCategoryId == categoryId
                             select new
                             {

                                 makeId = wf.MakeId,
                                 makeName = wf.MakeName,
                                 machineCategoryId = wf.MachineCategoryId,
                                 machineCategoryName = db.TblMachineCategoryNames.Where(m => m.MachineCategoryId == wf.MachineCategoryId).Select(m => m.MachineCategoryName).FirstOrDefault(),
                                 plantId = wf.PlantId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantName).FirstOrDefault(),

                             }).ToList();



                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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

        public CommonResponse DeleteMachineMake(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblMachineMakeDetails.Where(m => m.MakeId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region  12) No Of Axis  Master 

        public CommonResponse AddUpdateNoOfAxis(addAxisDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblNoOfAxis.Where(m => m.NoOfAxisId == data.noOfAxisId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblNoOfAxis tblAxisDet = new TblNoOfAxis();
                    tblAxisDet.NoOfAxis = data.noOfAxis;
                    tblAxisDet.IsDeleted = 0;
                    tblAxisDet.CreatedBy = 1;
                    tblAxisDet.CreatedOn = DateTime.Now;
                    db.TblNoOfAxis.Add(tblAxisDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.NoOfAxis = data.noOfAxis;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleNoOfAxis()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblNoOfAxis
                             where wf.IsDeleted == 0
                             select new
                             {
                                 noOfAxisId = wf.NoOfAxisId,
                                 noOfAxisName = wf.NoOfAxis,


                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteNoOfAxis(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblNoOfAxis.Where(m => m.NoOfAxisId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 13) Fall Stock Availability  Master 

        public CommonResponse AddUpdateFallStockAvailability(addFallStockDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblTallStockAvailibility.Where(m => m.TallStockAvailId == data.tallStockAvailId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblTallStockAvailibility tblStockDet = new TblTallStockAvailibility();
                    tblStockDet.TallStockAvailName = data.tallStockAvailName;
                    tblStockDet.IsDeleted = 0;
                    tblStockDet.CreatedBy = 1;
                    tblStockDet.CreatedOn = DateTime.Now;
                    db.TblTallStockAvailibility.Add(tblStockDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.TallStockAvailName = data.tallStockAvailName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultipleFallStockAvailability()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblTallStockAvailibility
                             where wf.IsDeleted == 0
                             select new
                             {
                                 fallStockId = wf.TallStockAvailId,
                                 fallStockName = wf.TallStockAvailName,


                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeleteFallStockAvailability(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblTallStockAvailibility.Where(m => m.TallStockAvailId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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


        #endregion

        #region 14) Pallet  Master 

        public CommonResponse AddUpdatePallet(addPalletDet data)
        {
            CommonResponse obj = new CommonResponse();

            try
            {

                var check = db.TblPallet.Where(m => m.PalletId == data.palletId && m.IsDeleted == 0).FirstOrDefault();
                if (check == null)
                {

                    TblPallet tblPalletDet = new TblPallet();
                    tblPalletDet.PalletName = data.palletName;
                    tblPalletDet.IsDeleted = 0;
                    tblPalletDet.CreatedBy = 1;
                    tblPalletDet.CreatedOn = DateTime.Now;
                    db.TblPallet.Add(tblPalletDet);
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
                else
                {

                    check.PalletName = data.palletName;
                    check.ModifiedBy = 2;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.UpdatedSuccessMessage;

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


        public CommonResponse ViewMultiplePallet()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblPallet
                             where wf.IsDeleted == 0
                             select new
                             {
                                 palletId = wf.PalletId,
                                 palletName = wf.PalletName,


                             }).ToList();


                if (check.Count > 0)
                {

                    obj.isStatus = true;
                    obj.response = check;
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "No Items Found";
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


        public CommonResponse DeletePallet(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblPallet.Where(m => m.PalletId == id && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedBy = 3;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
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
        #endregion
    }
}
