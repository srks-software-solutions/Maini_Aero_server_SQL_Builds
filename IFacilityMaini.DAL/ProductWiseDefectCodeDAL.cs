using IFacilityMaini.DAL.Resource;
using IFacilityMaini.DBModels;
using IFacilityMaini.Interface;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static IFacilityMaini.EntityModels.CommonEntity;
using static IFacilityMaini.EntityModels.ProductWiseDefectCodeEntity;

namespace IFacilityMaini.DAL
{
    public class ProductWiseDefectCodeDAL : IProductWiseDefectCode
    {
        unitworksccsContext db = new unitworksccsContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProductWiseDefectCodeDAL));
        public static IConfiguration configuration;

        public ProductWiseDefectCodeDAL(unitworksccsContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }

        /// <summary>
        /// Add Update Product Wise Defect Code
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse AddUpdateProductWiseDefectCode(List<AddEditProuctWiseDefectCodes> data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                foreach (var item in data)
                {
                    var check = db.TblProductWiseDefectCode.Where(m => m.ProductWiseDefectCodeId == item.productWiseDefectCodeId && m.Trim == item.trim).FirstOrDefault();
                    if (check == null)
                    {
                        TblProductWiseDefectCode tblProductWiseDefectCode = new TblProductWiseDefectCode();
                        tblProductWiseDefectCode.PlantId = item.plantId;
                        tblProductWiseDefectCode.PartNo = item.partNo;
                        tblProductWiseDefectCode.PartName = item.partName;
                        tblProductWiseDefectCode.Trim = item.trim;
                        tblProductWiseDefectCode.DefectCodeId = item.defectCodeId;
                        tblProductWiseDefectCode.IsDeleted = 0;
                        tblProductWiseDefectCode.CreatedOn = DateTime.Now;
                        db.TblProductWiseDefectCode.Add(tblProductWiseDefectCode);
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.AddedSuccessMessage;
                    }
                    else
                    {
                        check.PlantId = item.plantId;
                        check.PartNo = item.partNo;
                        check.PartName = item.partName;
                        check.Trim = item.trim;
                        check.DefectCodeId = item.defectCodeId;
                        check.IsDeleted = 0;
                        check.ModifiedOn = DateTime.Now;
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.UpdatedSuccessMessage;
                    }
                }
            }
            catch (Exception e)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// ViewMultipleProductWiseDefectCode
        /// </summary>
        /// <returns></returns>
        public CommonResponse ViewMultipleProductWiseDefectCode()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.TblProductWiseDefectCode
                             where wf.IsDeleted == 0
                             select new
                             {
                                 trim = wf.Trim,
                                 productWiseDefectCodeId = wf.ProductWiseDefectCodeId,
                                 plantName = db.Tblplant.Where(m => m.PlantId == wf.PlantId).Select(m => m.PlantCode).FirstOrDefault(),
                                 plantId = wf.PlantId,
                                 partNo = wf.PartNo,
                                 partName = wf.PartName,
                                 defectCode = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault(),
                                 defectCodeId = wf.DefectCodeId,
                                 defectDesc = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == wf.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault()
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
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Delete Product Wise Defect Code
        /// </summary>
        /// <param name="productWiseDefectCodeId"></param>
        /// <returns></returns>
        public CommonResponse DeleteProductWiseDefectCode(int productWiseDefectCodeId)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblProductWiseDefectCode.Where(m => m.ProductWiseDefectCodeId == productWiseDefectCodeId).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = 1;
                    check.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.DeletedSuccessMessage;
                }
            }
            catch (Exception e)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// View Multiple Plant Codes
        /// </summary>
        /// <returns></returns>
        public CommonResponse ViewMultiplePlantCodes()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblplant
                             where wf.IsDeleted == 0
                             select new
                             {
                                 plantId = wf.PlantId,
                                 plantCode = wf.PlantCode
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
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// ViewMultiplePartName
        /// </summary>
        /// <returns></returns>
        public CommonResponse ViewMultiplePartName()
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = (from wf in db.Tblparts
                             where wf.IsDeleted == 0
                             select new
                             {
                                 partId = wf.PartId,
                                 partNo = wf.PartNo,
                                 partName = wf.PartName
                             }).Distinct().ToList();

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
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// ViewMultipleDefectCode
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
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
                                 defectCode = wf.DefectCodeName,
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
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Add Defect Code
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse AddDefectCode(List<AddDefectCodes> data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                foreach (var item in data)
                {
                    var check = db.TblDefectCodeMaster.Where(m => m.DefectCodeName == item.defectCodeName).FirstOrDefault();
                    if (check == null)
                    {
                        TblDefectCodeMaster tblDefectCodeMaster = new TblDefectCodeMaster();
                        tblDefectCodeMaster.DefectCodeName = item.defectCodeName;
                        tblDefectCodeMaster.DefectCodeDesc = item.defectCodeDesc;
                        tblDefectCodeMaster.IsDeleted = 0;
                        tblDefectCodeMaster.CreatedOn = DateTime.Now;
                        db.TblDefectCodeMaster.Add(tblDefectCodeMaster);
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.AddedSuccessMessage;
                    }
                    else
                    {
                        check.DefectCodeName = item.defectCodeName;
                        check.DefectCodeDesc = item.defectCodeDesc;
                        check.IsDeleted = 0;
                        check.ModifiedOn = DateTime.Now;
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.UpdatedSuccessMessage;
                    }
                }
            }
            catch (Exception e)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Add General Defect Codes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse AddGeneralDefectCodes(List<AddDefectCodes> data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblGeneralDefectCodes.Where(m => m.IsDeleted == 0).ToList();
                db.RemoveRange(check);
                db.SaveChanges();

                foreach (var item in data)
                {
                    TblGeneralDefectCodes tblGeneralDefectCodes = new TblGeneralDefectCodes();
                    tblGeneralDefectCodes.DefectCodeName = item.defectCodeName;
                    tblGeneralDefectCodes.DefectCodeDesc = item.defectCodeDesc;
                    tblGeneralDefectCodes.IsDeleted = 0;
                    tblGeneralDefectCodes.CreatedOn = DateTime.Now;
                    db.TblGeneralDefectCodes.Add(tblGeneralDefectCodes);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
            }
            catch (Exception ex)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        public CommonResponse DownloadProductWiseDefectCode()
        {
            CommonResponse obj = new CommonResponse();
            try
            {

                FileInfo templateFile = new FileInfo(@"C:\SRKS_ifacility\MainTemplate\ProductwiseDefectCodes.xlsx");

                ExcelPackage templatep = new ExcelPackage(templateFile);
                ExcelWorksheet Templatews = templatep.Workbook.Worksheets[0];
                //  ExcelWorksheet TemplateGraph = templatep.Workbook.Worksheets[1];


                //excel file save and  downloaded link



                string ImageUrlSave = configuration.GetSection("AppSettings").GetSection("ImageUrlSave").Value;
                string ImageUrl = configuration.GetSection("AppSettings").GetSection("ImageUrl").Value;

                String FileDir = ImageUrlSave + "\\" + "ProductwiseDefectCodes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                String retrivalPath = ImageUrl + "ProductwiseDefectCodes_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";



                FileInfo newFile = new FileInfo(FileDir);

                if (newFile.Exists)
                {
                    try
                    {
                        newFile.Delete();  // ensures we create a new workbook
                        newFile = new FileInfo(FileDir);
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
                        //ErrorLog.SendErrorToDB(ex);
                        obj.response = ResourceResponse.ExceptionMessage; ;
                    }
                }


                //Using the File for generation and populating it
                ExcelPackage p = null;
                p = new ExcelPackage(newFile);
                ExcelWorksheet worksheet = null;
                //  ExcelWorksheet worksheetGraph = null;

                //Creating the WorkSheet for populating
                try
                {
                    worksheet = p.Workbook.Worksheets.Add(DateTime.Now.ToString("yyyy-MM-dd"), Templatews);
                    //  worksheetGraph = p.Workbook.Worksheets.Add("Graphs", TemplateGraph);
                }
                catch { }

                if (worksheet == null)
                {
                    worksheet = p.Workbook.Worksheets.Add(DateTime.Now.ToString("yyyy-MM-dd") + "1", Templatews);
                    //  worksheetGraph = p.Workbook.Worksheets.Add(System.DateTime.Now.ToString("dd-MM-yyyy") + "Graph", TemplateGraph);
                }


                int sheetcount = p.Workbook.Worksheets.Count;
                p.Workbook.Worksheets.MoveToStart(sheetcount - 1);
                worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                int StartRow = 2;
                // int slno = 1;
                var GetUtilList = db.TblProductWiseDefectCode.Where(m => m.IsDeleted == 0).ToList();

                if (GetUtilList.Count > 0)
                {
                    foreach (var MacRow in GetUtilList)
                    {
                        // trim	plantCode	partNo	partName	defectCode	defectDescription
                        // worksheet.Cells["" + StartRow].Value = slno++;
                        worksheet.Cells["A" + StartRow].Value = MacRow.Trim;
                        worksheet.Cells["B" + StartRow].Value = db.Tblplant.Where(m => m.PlantId == MacRow.PlantId).Select(m => m.PlantCode).FirstOrDefault();
                        worksheet.Cells["C" + StartRow].Value = MacRow.PartNo;
                        worksheet.Cells["D" + StartRow].Value = MacRow.PartName;
                        worksheet.Cells["E" + StartRow].Value = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == MacRow.DefectCodeId).Select(m => m.DefectCodeName).FirstOrDefault();
                        worksheet.Cells["F" + StartRow].Value = db.TblDefectCodeMaster.Where(m => m.DefectCodeId == MacRow.DefectCodeId).Select(m => m.DefectCodeDesc).FirstOrDefault();

                        StartRow++;
                    }
                }

                p.Save();

                obj.isStatus = true;
                obj.response = retrivalPath;


            }
            catch (Exception e)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        /// <summary>
        /// Upload Product Wise Defect Code
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommonResponse UploadProductWiseDefectCode(List<UploadProuctWiseDefectCodes> data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblProductWiseDefectCode.Where(m => m.IsDeleted == 0).ToList();
                db.RemoveRange(check);
                db.SaveChanges();

                foreach (var item in data)
                {
                    TblProductWiseDefectCode tblProductWiseDefectCode = new TblProductWiseDefectCode();
                    if (item.plantCode != null)
                    {
                        var plantId = db.Tblplant.Where(m => m.PlantCode == item.plantCode).Select(m => m.PlantId).FirstOrDefault();
                        tblProductWiseDefectCode.PlantId = plantId;
                    }

                    tblProductWiseDefectCode.PartNo = item.partNo;
                    tblProductWiseDefectCode.PartName = item.partName;
                    tblProductWiseDefectCode.Trim = item.trim;

                    if (item.defectCode != null)
                    {
                        var defectIdCode = db.TblDefectCodeMaster.Where(m => m.DefectCodeName == item.defectCode).Select(m => m.DefectCodeId).FirstOrDefault();
                        tblProductWiseDefectCode.DefectCodeId = defectIdCode;
                    }

                    tblProductWiseDefectCode.IsDeleted = 0;
                    tblProductWiseDefectCode.CreatedOn = DateTime.Now;
                    db.TblProductWiseDefectCode.Add(tblProductWiseDefectCode);
                    db.SaveChanges();
                    obj.isStatus = true;
                    obj.response = ResourceResponse.AddedSuccessMessage;
                }
            }
            catch (Exception e)
            {
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }
    }
}
