using IFacilityMaini.DAL.Resource;
using IFacilityMaini.DBModels;
using IFacilityMaini.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static IFacilityMaini.EntityModels.CommonEntity;
using static IFacilityMaini.EntityModels.ReworkAndRejectionEntity;
using IFacilityMaini.DAL.App_Start;

namespace IFacilityMaini.DAL
{
    public class ReworkAndRejectionDAL : IReworkAndRejection
    {
        unitworksccsContext db = new unitworksccsContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ReworkAndRejectionDAL));
        public static IConfiguration configuration;

        public ReworkAndRejectionDAL(unitworksccsContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }

        /// <summary>
        /// View Multiple Defect Code
        /// </summary>
        /// <returns></returns>
        public CommonResponse ViewMultipleDefectCode(int fgPartId, int machineId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var dbCheck = db.TblFgPartNoDet.Where(m => m.FgPartId == fgPartId && m.MachineId == machineId).Select(m => m.FgPartNo).FirstOrDefault();

                //var partNo = db.TblPlanLinkageMaster.Where(m => m.PlanLinkageId == dbCheck).Select(m => m.FgPartNo).FirstOrDefault();
                if (dbCheck == null)
                {
                    var check = (from wf in db.TblGeneralDefectCodes
                                 where wf.IsDeleted == 0
                                 select new
                                 {
                                     DefectCodeId = wf.GeneralDefectCodeId,
                                     DefectCodeName = wf.DefectCodeName + " - " + wf.DefectCodeDesc
                                 }).ToList();

                    if (check.Count > 0)
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
                else
                {
                    var check = (from wf in db.TblProductWiseDefectCode
                                 where wf.IsDeleted == 0 && wf.PartNo == dbCheck
                                 select new
                                 {
                                     DefectCodeId = wf.DefectCodeId,
                                     DefectCodeName = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault() + " - " + db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault()
                                 }).ToList();

                    if (check.Count > 0)
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
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Add Rejection Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse AddRejectionDetails(AddRejection data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                CommonFunction commonFunction = new CommonFunction();
                string shift = commonFunction.GetCurrentShift();
                addedResopnse addresopnce = new addedResopnse();


                string correctedDate = commonFunction.GetCorrectedDate();

                if (data.dmcCodeStatus == "Enable")
                {
                    if (data.actual > 1)
                    {
                        var check = db.TblRejectionDetails.Where(m => m.QrCodeNo == data.qrCodeNo).FirstOrDefault();
                        if (check == null)
                        {
                            var dbCheck = db.TblReworkDetails.Where(m => m.QrCodeNo == data.qrCodeNo).FirstOrDefault();
                            if (dbCheck == null)
                            {
                                TblRejectionDetails tblRejectionDetails = new TblRejectionDetails();
                                tblRejectionDetails.DefectCodeId = data.defectCodeId;
                                tblRejectionDetails.DefectQty = 1;
                                tblRejectionDetails.MachineId = data.machineId;
                                tblRejectionDetails.OperatorId = data.operatorId;
                                tblRejectionDetails.IsDeleted = 0;
                                tblRejectionDetails.Shift = shift;
                                tblRejectionDetails.FgPartId = data.fgPartId;
                                tblRejectionDetails.CorrectedDate = correctedDate;
                                tblRejectionDetails.CreatedOn = DateTime.Now;
                                tblRejectionDetails.QrCodeNo = data.qrCodeNo;
                                tblRejectionDetails.DmcCodeStatus = data.dmcCodeStatus;
                                tblRejectionDetails.ReasonForRejection = data.reasonForRejection;
                                db.TblRejectionDetails.Add(tblRejectionDetails);
                                db.SaveChanges();
                                obj.isStatus = true;
                                // obj.response = ResourceResponse.AddedSuccessMessage;

                                var reLast = db.TblRejectionDetails.Where(m => m.IsDeleted == 0).LastOrDefault();

                                var fgpartdet = db.TblFgPartNoDet.Where(m => m.FgPartId == data.fgPartId).FirstOrDefault();
                                var cellid = db.TblFgAndCellAllocation.Where(m => m.PartNo == fgpartdet.FgPartNo).FirstOrDefault();

                                var cellname = db.Tblshop.Where(m => m.ShopId == cellid.CellFinalId).Select(m => m.ShopName).FirstOrDefault();
                                var subcellname = db.Tblcell.Where(m => m.CellId == cellid.SubCellFinalId).Select(m => m.CellName).FirstOrDefault();

                                var defectcode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == data.defectCodeId).FirstOrDefault();


                                var empno = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OpNo).FirstOrDefault();


                                int? machineid = db.TblFgPartNoDet.Where(m => m.FgPartId == fgpartdet.FgPartId).Select(m => m.MachineId).FirstOrDefault();

                                var machinedet = db.Tblmachinedetails.Where(m => m.MachineId == (int)machineid).FirstOrDefault();

                                var plantcode = db.Tblplant.Where(m => m.PlantId == machinedet.PlantId).Select(m => m.PlantCode).FirstOrDefault();

                                var operatorname = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OperatorName).FirstOrDefault();

                                addresopnce.cellId = (int)cellid.CellFinalId;
                                addresopnce.cellName = cellname;

                                addresopnce.subCellId = (int)cellid.SubCellFinalId;
                                addresopnce.subCellName = subcellname;

                                // addresopnce.date = Convert.ToString(fgpartdet.StartDate);

                                DateTime date1 = Convert.ToDateTime(fgpartdet.StartDate);
                                addresopnce.date = date1.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                                //  DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK")

                                addresopnce.defectCodeId = data.defectCodeId;

                                addresopnce.defectCode = defectcode.DefectCodeName;
                                addresopnce.defectDescription = defectcode.DefectCodeDesc;
                                addresopnce.employeeNo = (int)empno;
                                addresopnce.fgPartId = fgpartdet.FgPartId;
                                addresopnce.isRejectionOrRework = "Rejection";
                                addresopnce.machineId = (int)machineid;
                                string[] machinename = machinedet.MachineName.Split('_');
                                addresopnce.machineName = machinename[1];
                                addresopnce.operatorId = (int)fgpartdet.OperatorId;
                                addresopnce.operatorName = operatorname;
                                addresopnce.partDescription = fgpartdet.PartName;
                                addresopnce.partId = null;
                                addresopnce.partNumber = fgpartdet.FgPartNo;
                                addresopnce.partQrCode = data.qrCodeNo;
                                addresopnce.plantId = (int)machinedet.PlantId;
                                addresopnce.plantName = plantcode;
                                addresopnce.quantity = (int)reLast.DefectQty;
                                addresopnce.rejectionId = reLast.RejectionId;
                                addresopnce.reworkId = 0;
                                addresopnce.shift = fgpartdet.Shift;
                                addresopnce.updatedRejectionOrRework = null;
                                addresopnce.workorderNumber = fgpartdet.WorkOrderNo;
                                addresopnce.reasonForRejection = reLast.ReasonForRejection;
                                obj.response = addresopnce;
                            }
                            else
                            {
                                obj.isStatus = false;
                                obj.response = "Qr Code already Added";
                            }
                        }
                        else
                        {
                            obj.isStatus = false;
                            obj.response = "Qr Code already Added";
                        }
                    }
                    else
                    {
                        obj.isStatus = false;
                        obj.response = "Please enter Defect Qty less than Actal value" + data.actual;
                    }
                }
                else
                {
                    if (data.actual > 1)
                    {
                        TblRejectionDetails tblRejectionDetails = new TblRejectionDetails();
                        tblRejectionDetails.DefectCodeId = data.defectCodeId;
                        tblRejectionDetails.DefectQty = data.defectQty;
                        tblRejectionDetails.MachineId = data.machineId;
                        tblRejectionDetails.OperatorId = data.operatorId;
                        tblRejectionDetails.IsDeleted = 0;
                        tblRejectionDetails.Shift = shift;
                        tblRejectionDetails.FgPartId = data.fgPartId;
                        tblRejectionDetails.CorrectedDate = correctedDate;
                        tblRejectionDetails.CreatedOn = DateTime.Now;
                        tblRejectionDetails.DmcCodeStatus = data.dmcCodeStatus;
                        tblRejectionDetails.ReasonForRejection = data.reasonForRejection;
                        db.TblRejectionDetails.Add(tblRejectionDetails);
                        db.SaveChanges();
                        obj.isStatus = true;
                        //   obj.response = ResourceResponse.AddedSuccessMessage;

                        var reLast = db.TblRejectionDetails.Where(m => m.IsDeleted == 0).LastOrDefault();

                        var fgpartdet = db.TblFgPartNoDet.Where(m => m.FgPartId == data.fgPartId).FirstOrDefault();
                        var cellid = db.TblFgAndCellAllocation.Where(m => m.PartNo == fgpartdet.FgPartNo).FirstOrDefault();

                        var cellname = db.Tblshop.Where(m => m.ShopId == cellid.CellFinalId).Select(m => m.ShopName).FirstOrDefault();
                        var subcellname = db.Tblcell.Where(m => m.CellId == cellid.SubCellFinalId).Select(m => m.CellName).FirstOrDefault();

                        var defectcode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == data.defectCodeId).FirstOrDefault();


                        var empno = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OpNo).FirstOrDefault();


                        int? machineid = db.TblFgPartNoDet.Where(m => m.FgPartId == fgpartdet.FgPartId).Select(m => m.MachineId).FirstOrDefault();

                        var machinedet = db.Tblmachinedetails.Where(m => m.MachineId == (int)machineid).FirstOrDefault();

                        var plantcode = db.Tblplant.Where(m => m.PlantId == machinedet.PlantId).Select(m => m.PlantCode).FirstOrDefault();

                        var operatorname = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OperatorName).FirstOrDefault();




                        addresopnce.cellId = (int)cellid.CellFinalId;
                        addresopnce.cellName = cellname;

                        addresopnce.subCellId = (int)cellid.SubCellFinalId;
                        addresopnce.subCellName = subcellname;
                        // addresopnce.date = Convert.ToString(fgpartdet.StartDate);

                        DateTime date1 = Convert.ToDateTime(fgpartdet.StartDate);
                        addresopnce.date = date1.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                        addresopnce.defectCodeId = data.defectCodeId;
                        addresopnce.defectCode = defectcode.DefectCodeName;
                        addresopnce.defectDescription = defectcode.DefectCodeDesc;
                        addresopnce.employeeNo = (int)empno;
                        addresopnce.fgPartId = fgpartdet.FgPartId;
                        addresopnce.isRejectionOrRework = "Rejection";
                        addresopnce.machineId = (int)machineid;
                        string[] machinename = machinedet.MachineName.Split('_');
                        addresopnce.machineName = machinename[1];
                        addresopnce.operatorId = (int)fgpartdet.OperatorId;

                        addresopnce.operatorName = operatorname;

                        addresopnce.partDescription = fgpartdet.PartName;

                        addresopnce.partId = null;
                        addresopnce.partNumber = fgpartdet.FgPartNo;

                        addresopnce.partQrCode = data.qrCodeNo;

                        addresopnce.plantId = (int)machinedet.PlantId;

                        addresopnce.plantName = plantcode;
                        addresopnce.quantity = (int)reLast.DefectQty;
                        addresopnce.rejectionId = reLast.RejectionId;
                        addresopnce.reworkId = 0;
                        addresopnce.shift = fgpartdet.Shift;
                        addresopnce.updatedRejectionOrRework = null;
                        addresopnce.workorderNumber = fgpartdet.WorkOrderNo;
                        addresopnce.reasonForRejection = reLast.ReasonForRejection;
                        obj.response = addresopnce;
                    }
                    else
                    {
                        obj.isStatus = false;
                        obj.response = "Please enter Defect Qty less than Actal value" + data.actual;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Delete Rejection Details
        /// </summary>
        /// <param name="rejectionId"></param>
        /// <returns></returns>
        public CommonResponse DeleteRejectionDetails(int rejectionId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblRejectionDetails.Where(m => m.RejectionId == rejectionId).FirstOrDefault();
                if (check != null)
                {
                    db.Remove(check);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
                }
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// View Rejection Details
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="fgPartId"></param>
        /// <returns></returns>
        public CommonResponse ViewRejectionDetails(int machineId, int fgPartId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblRejectionDetails
                             where wf.MachineId == machineId && wf.FgPartId == fgPartId
                             select new
                             {
                                 RejectionId = wf.RejectionId,
                                 DefectCodeId = wf.DefectCodeId,
                                 //DefectCodeName = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault(),
                                 DefectQty = wf.DefectQty,
                                 //DefectDesc = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault(),
                                 DefectCode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault() + " - " + db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault(),
                                 QrCodeNo = wf.QrCodeNo,
                                 reasonForRejection = wf.ReasonForRejection
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
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Add Rework Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse AddReworkDetails(AddRework data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                CommonFunction commonFunction = new CommonFunction();
                string shift = commonFunction.GetCurrentShift();
                string correctedDate = commonFunction.GetCorrectedDate();

                addedResopnse addresopnce = new addedResopnse();

                if (data.dmcCodeStatus == "Enable")
                {
                    if (data.actual > 1)
                    {
                        var check = db.TblReworkDetails.Where(m => m.QrCodeNo == data.qrCodeNo).FirstOrDefault();
                        if (check == null)
                        {
                            var dbCheck = db.TblRejectionDetails.Where(m => m.QrCodeNo == data.qrCodeNo).FirstOrDefault();
                            if (dbCheck == null)
                            {
                                TblReworkDetails tblReworkDetails = new TblReworkDetails();
                                tblReworkDetails.DefectCodeId = data.defectCodeId;
                                tblReworkDetails.DefectQty = 1;
                                tblReworkDetails.MachineId = data.machineId;
                                tblReworkDetails.OperatorId = data.operatorId;
                                tblReworkDetails.IsDeleted = 0;
                                tblReworkDetails.Shift = shift;
                                tblReworkDetails.FgPartId = data.fgPartId;
                                tblReworkDetails.CreatedOn = DateTime.Now;
                                tblReworkDetails.CorrectedDate = correctedDate;
                                tblReworkDetails.QrCodeNo = data.qrCodeNo;
                                tblReworkDetails.DmcCodeStatus = data.dmcCodeStatus;
                                tblReworkDetails.ReasonForRework = data.reasonForRework;
                                db.TblReworkDetails.Add(tblReworkDetails);
                                db.SaveChanges();
                                obj.isStatus = true;
                                //  obj.response = ResourceResponse.AddedSuccessMessage;


                                var reLast = db.TblReworkDetails.Where(m => m.IsDeleted == 0).LastOrDefault();

                                var fgpartdet = db.TblFgPartNoDet.Where(m => m.FgPartId == data.fgPartId).FirstOrDefault();
                                var cellid = db.TblFgAndCellAllocation.Where(m => m.PartNo == fgpartdet.FgPartNo).FirstOrDefault();

                                var cellname = db.Tblshop.Where(m => m.ShopId == cellid.CellFinalId).Select(m => m.ShopName).FirstOrDefault();
                                var subcellname = db.Tblcell.Where(m => m.CellId == cellid.SubCellFinalId).Select(m => m.CellName).FirstOrDefault();

                                var defectcode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == data.defectCodeId).FirstOrDefault();


                                var empno = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OpNo).FirstOrDefault();


                                int? machineid = db.TblFgPartNoDet.Where(m => m.FgPartId == fgpartdet.FgPartId).Select(m => m.MachineId).FirstOrDefault();

                                var machinedet = db.Tblmachinedetails.Where(m => m.MachineId == (int)machineid).FirstOrDefault();

                                var plantcode = db.Tblplant.Where(m => m.PlantId == machinedet.PlantId).Select(m => m.PlantCode).FirstOrDefault();

                                var operatorname = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OperatorName).FirstOrDefault();

                                addresopnce.cellId = (int)cellid.CellFinalId;
                                addresopnce.cellName = cellname;

                                addresopnce.subCellId = (int)cellid.SubCellFinalId;
                                addresopnce.subCellName = subcellname;
                                // addresopnce.date = Convert.ToString(fgpartdet.StartDate);
                                DateTime date1 = Convert.ToDateTime(fgpartdet.StartDate);
                                addresopnce.date = date1.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");


                                addresopnce.defectCodeId = data.defectCodeId;
                                addresopnce.defectCode = defectcode.DefectCodeName;
                                addresopnce.defectDescription = defectcode.DefectCodeDesc;
                                addresopnce.employeeNo = (int)empno;
                                addresopnce.fgPartId = fgpartdet.FgPartId;
                                addresopnce.isRejectionOrRework = "Rework";
                                addresopnce.machineId = (int)machineid;
                                string[] machinename = machinedet.MachineName.Split('_');
                                addresopnce.machineName = machinename[1];
                                addresopnce.operatorId = (int)fgpartdet.OperatorId;
                                addresopnce.operatorName = operatorname;
                                addresopnce.partDescription = fgpartdet.PartName;
                                addresopnce.partId = null;
                                addresopnce.partNumber = fgpartdet.FgPartNo;
                                addresopnce.partQrCode = data.qrCodeNo;
                                addresopnce.plantId = (int)machinedet.PlantId;
                                addresopnce.plantName = plantcode;
                                addresopnce.quantity = (int)reLast.DefectQty;
                                addresopnce.rejectionId = 0;
                                addresopnce.reworkId = reLast.ReworkId;
                                addresopnce.shift = fgpartdet.Shift;
                                addresopnce.updatedRejectionOrRework = null;
                                addresopnce.workorderNumber = fgpartdet.WorkOrderNo;
                                addresopnce.reasonForRework = reLast.ReasonForRework;
                                obj.response = addresopnce;

                            }
                            else
                            {
                                obj.isStatus = false;
                                obj.response = "Qr Code already Added";
                            }
                        }
                        else
                        {
                            obj.isStatus = false;
                            obj.response = "Qr Code already Added";
                        }
                    }
                    else
                    {
                        obj.isStatus = false;
                        obj.response = "Please enter Defect Qty less than Actal value" + data.actual;
                    }
                }
                else
                {
                    if (data.actual > 1)
                    {
                        TblReworkDetails tblReworkDetails = new TblReworkDetails();
                        tblReworkDetails.DefectCodeId = data.defectCodeId;
                        tblReworkDetails.DefectQty = data.defectQty;
                        tblReworkDetails.MachineId = data.machineId;
                        tblReworkDetails.OperatorId = data.operatorId;
                        tblReworkDetails.IsDeleted = 0;
                        tblReworkDetails.Shift = shift;
                        tblReworkDetails.FgPartId = data.fgPartId;
                        tblReworkDetails.CreatedOn = DateTime.Now;
                        tblReworkDetails.CorrectedDate = correctedDate;
                        tblReworkDetails.DmcCodeStatus = data.dmcCodeStatus;
                        tblReworkDetails.ReasonForRework = data.reasonForRework;
                        db.TblReworkDetails.Add(tblReworkDetails);
                        db.SaveChanges();
                        obj.isStatus = true;
                        // obj.response = ResourceResponse.AddedSuccessMessage;

                        var reLast = db.TblReworkDetails.Where(m => m.IsDeleted == 0).LastOrDefault();



                        var fgpartdet = db.TblFgPartNoDet.Where(m => m.FgPartId == data.fgPartId).FirstOrDefault();
                        var cellid = db.TblFgAndCellAllocation.Where(m => m.PartNo == fgpartdet.FgPartNo).FirstOrDefault();

                        var cellname = db.Tblshop.Where(m => m.ShopId == cellid.CellFinalId).Select(m => m.ShopName).FirstOrDefault();
                        var subcellname = db.Tblcell.Where(m => m.CellId == cellid.SubCellFinalId).Select(m => m.CellName).FirstOrDefault();

                        var defectcode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == data.defectCodeId).FirstOrDefault();


                        var empno = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OpNo).FirstOrDefault();


                        int? machineid = db.TblFgPartNoDet.Where(m => m.FgPartId == fgpartdet.FgPartId).Select(m => m.MachineId).FirstOrDefault();

                        var machinedet = db.Tblmachinedetails.Where(m => m.MachineId == (int)machineid).FirstOrDefault();

                        var plantcode = db.Tblplant.Where(m => m.PlantId == machinedet.PlantId).Select(m => m.PlantCode).FirstOrDefault();

                        var operatorname = db.TblOperatorDetails.Where(m => m.OpId == fgpartdet.OperatorId).Select(m => m.OperatorName).FirstOrDefault();

                        addresopnce.cellId = (int)cellid.CellFinalId;
                        addresopnce.cellName = cellname;
                        addresopnce.subCellId = (int)cellid.SubCellFinalId;
                        addresopnce.subCellName = subcellname;
                        //  addresopnce.date = Convert.ToString(fgpartdet.StartDate);

                        DateTime date1 = Convert.ToDateTime(fgpartdet.StartDate);
                        addresopnce.date = date1.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                        addresopnce.defectCodeId = data.defectCodeId;
                        addresopnce.defectCode = defectcode.DefectCodeName;
                        addresopnce.defectDescription = defectcode.DefectCodeDesc;
                        addresopnce.employeeNo = (int)empno;
                        addresopnce.fgPartId = fgpartdet.FgPartId;
                        addresopnce.isRejectionOrRework = "Rework";
                        addresopnce.machineId = (int)machineid;
                        string[] machinename = machinedet.MachineName.Split('_');
                        addresopnce.machineName = machinename[1];
                        addresopnce.operatorId = (int)fgpartdet.OperatorId;
                        addresopnce.operatorName = operatorname;
                        addresopnce.partDescription = fgpartdet.PartName;
                        addresopnce.partId = null;
                        addresopnce.partNumber = fgpartdet.FgPartNo;
                        addresopnce.partQrCode = data.qrCodeNo;
                        addresopnce.plantId = (int)machinedet.PlantId;
                        addresopnce.plantName = plantcode;
                        addresopnce.quantity = (int)reLast.DefectQty;
                        addresopnce.rejectionId = 0;
                        addresopnce.reworkId = reLast.ReworkId;
                        addresopnce.shift = fgpartdet.Shift;
                        addresopnce.updatedRejectionOrRework = null;
                        addresopnce.workorderNumber = fgpartdet.WorkOrderNo;
                        addresopnce.reasonForRework = reLast.ReasonForRework;
                        obj.response = addresopnce;
                    }
                    else
                    {
                        obj.isStatus = false;
                        obj.response = "Please enter Defect Qty less than Actal value" + data.actual;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Delete Rework Details
        /// </summary>
        /// <param name="reworkId"></param>
        /// <returns></returns>
        public CommonResponse DeleteReworkDetails(int reworkId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblReworkDetails.Where(m => m.ReworkId == reworkId).FirstOrDefault();
                if (check != null)
                {
                    db.Remove(check);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
                }
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// View Rework Details
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="fgPartId"></param>
        /// <returns></returns>
        public CommonResponse ViewReworkDetails(int machineId, int fgPartId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblReworkDetails
                             where wf.MachineId == machineId && wf.FgPartId == fgPartId
                             select new
                             {
                                 ReworkId = wf.ReworkId,
                                 DefectCodeId = wf.DefectCodeId,
                                 DefectQty = wf.DefectQty,
                                 DefectCode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault() + " - " + db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault(),
                                 QrCodeNo = wf.QrCodeNo,
                                 reasonForRework = wf.ReasonForRework
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
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }
    }
}
