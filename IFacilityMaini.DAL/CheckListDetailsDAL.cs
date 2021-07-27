using IFacilityMaini.DAL.App_Start;
using IFacilityMaini.DAL.Helpers;
using IFacilityMaini.DAL.Resource;
using IFacilityMaini.DBModels;
using IFacilityMaini.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static IFacilityMaini.EntityModels.CheckListDetailsEntity;
using static IFacilityMaini.EntityModels.CommonEntity;

namespace IFacilityMaini.DAL
{
    public class CheckListDetailsDAL : ICheckListDetails
    {
        unitworksccsContext db = new unitworksccsContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CheckListDetailsDAL));
        public static IConfiguration configuration;
        private readonly AppSettings appSettings;

        public CheckListDetailsDAL(unitworksccsContext _db, IConfiguration _configuration, IOptions<AppSettings> _appSettings)
        {
            db = _db;
            configuration = _configuration;
            appSettings = _appSettings.Value;
        }

        public CommonResponse AddAndEditCheckListDetails(addCheckList data)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                if (data.flag == 0)
                {
                    var check = db.TblCheckListDetails.Where(m => m.CheckListId == data.CheckListId && m.HeaderId == data.HeaderId && m.IsDeleted == 0).FirstOrDefault();
                    if (check == null)
                    {
                        TblCheckListDetails tblCheckList = new TblCheckListDetails();
                        tblCheckList.HeaderId = data.HeaderId;
                        tblCheckList.WhatToCheck = data.WhatToCheck;
                        tblCheckList.How = data.How;
                        tblCheckList.Frequency = data.Frequency;
                        tblCheckList.RunningHrs = data.RunningHrs;
                        tblCheckList.PartsCount = data.PartsCount;
                        tblCheckList.PeriodFrequency = data.PeriodFrequency;
                        tblCheckList.IsNumeric = data.isNumeric;
                        tblCheckList.IsText = data.isText;
                        tblCheckList.RoleId = data.roleId;
                        tblCheckList.IsEdited = 0;
                        tblCheckList.IsPrepared = 0;
                        tblCheckList.IsApproved = 0;

                        if (data.Date != "")
                        {
                            string[] datesplite = data.Date.Split(',');
                            string savedate = "";
                            foreach (var dt in datesplite)
                            {
                                DateTime ddtt = Convert.ToDateTime(dt);
                                string dt1 = ddtt.ToString("yyyy-MM-dd");

                                savedate += dt1 + ",";

                            }
                            savedate = savedate.TrimEnd(',');
                            tblCheckList.Date = savedate;
                        }
                        else
                        {
                            tblCheckList.Date = data.Date;
                        }

                        tblCheckList.IsDeleted = 0;
                        tblCheckList.CreatedOn = DateTime.Now;
                        db.TblCheckListDetails.Add(tblCheckList);
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.AddedSuccessMessage;
                    }
                    else
                    {
                        check.HeaderId = data.HeaderId;
                        check.WhatToCheck = data.WhatToCheck;
                        check.How = data.How;
                        check.Frequency = data.Frequency;
                        check.RunningHrs = data.RunningHrs;
                        check.PartsCount = data.PartsCount;
                        check.PeriodFrequency = data.PeriodFrequency;
                        check.IsNumeric = data.isNumeric;
                        check.IsText = data.isText;
                        check.RoleId = data.roleId;
                        if(check.IsOk == "NOTOK")
                        {
                            check.IsEdited = 1;
                        }
                        else
                        {
                            check.IsEdited = 0;
                        }
                        check.IsPrepared = 0;

                        if (data.Date != "")
                        {
                            string[] datesplite = data.Date.Split(',');
                            string savedate = "";
                            foreach (var dt in datesplite)
                            {
                                DateTime ddtt = Convert.ToDateTime(dt);
                                string dt1 = ddtt.ToString("yyyy-MM-dd");
                                savedate += dt1 + ",";
                            }
                            savedate = savedate.TrimEnd(',');
                            check.Date = savedate;
                        }
                        check.Date = data.Date;
                        //  check.Date =data.Date;

                        check.IsDeleted = 0;
                        check.ModifiedOn = DateTime.Now;
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.UpdatedSuccessMessage;
                    }
                }
                else
                {
                    var checkList = db.TblCheckListDetailsNew.Where(m => m.CheckListId == data.CheckListId).FirstOrDefault();
                    if (checkList == null)
                    {
                        TblCheckListDetailsNew tblCheckListDetailsNew = new TblCheckListDetailsNew();
                        tblCheckListDetailsNew.CheckListId = data.CheckListId;
                        tblCheckListDetailsNew.WhatToCheck = data.WhatToCheck;
                        tblCheckListDetailsNew.How = data.How;
                        tblCheckListDetailsNew.Frequency = data.Frequency;
                        tblCheckListDetailsNew.RunningHrs = data.RunningHrs;
                        tblCheckListDetailsNew.PartsCount = data.PartsCount;
                        tblCheckListDetailsNew.PeriodFrequency = data.PeriodFrequency;
                        tblCheckListDetailsNew.IsNumeric = data.isNumeric;
                        tblCheckListDetailsNew.IsText = data.isText;
                        tblCheckListDetailsNew.RoleId = data.roleId;
                        tblCheckListDetailsNew.CreatedOn = DateTime.Now;
                        tblCheckListDetailsNew.AddEdit = data.addEdit;
                        if (data.Date != "")
                        {
                            string[] datesplite = data.Date.Split(',');
                            string savedate = "";
                            foreach (var dt in datesplite)
                            {
                                DateTime ddtt = Convert.ToDateTime(dt);
                                string dt1 = ddtt.ToString("yyyy-MM-dd");

                                savedate += dt1 + ",";
                            }
                            savedate = savedate.TrimEnd(',');
                            tblCheckListDetailsNew.Date = savedate;
                        }
                        tblCheckListDetailsNew.Date = data.Date;
                        tblCheckListDetailsNew.IsDeleted = 0;
                        tblCheckListDetailsNew.ModifiedOn = DateTime.Now;
                        tblCheckListDetailsNew.Flag = data.flag;
                        db.TblCheckListDetailsNew.Add(tblCheckListDetailsNew);
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.UpdatedSuccessMessage;
                    }
                    else
                    {
                        checkList.CheckListId = data.CheckListId;
                        checkList.WhatToCheck = data.WhatToCheck;
                        checkList.How = data.How;
                        checkList.Frequency = data.Frequency;
                        checkList.RunningHrs = data.RunningHrs;
                        checkList.PartsCount = data.PartsCount;
                        checkList.PeriodFrequency = data.PeriodFrequency;
                        checkList.IsNumeric = data.isNumeric;
                        checkList.IsText = data.isText;
                        checkList.RoleId = data.roleId;
                        checkList.AddEdit = data.addEdit;
                        if (data.Date != "")
                        {
                            string[] datesplite = data.Date.Split(',');
                            string savedate = "";
                            foreach (var dt in datesplite)
                            {
                                DateTime ddtt = Convert.ToDateTime(dt);
                                string dt1 = ddtt.ToString("yyyy-MM-dd");

                                savedate += dt1 + ",";

                            }
                            savedate = savedate.TrimEnd(',');
                            checkList.Date = savedate;
                        }
                        checkList.Date = data.Date;
                        //  check.Date =data.Date;

                        checkList.IsDeleted = 0;
                        checkList.ModifiedOn = DateTime.Now;
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.UpdatedSuccessMessage;
                    }
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

        public CommonResponse ViewMultipleCheckListDetails()
        {

            CommonResponse obj = new CommonResponse();
            try
            {
                List<ViewCheckList> ViewCheckedListDet = new List<ViewCheckList>();

                var tblDet = db.TblCheckListDetails.Where(m => m.IsDeleted == 0).ToList();

                foreach (var items in tblDet)
                {

                    ViewCheckList ViewChecklist = new ViewCheckList();
                    ViewChecklist.checkListId = items.CheckListId;
                    ViewChecklist.headerId = (int)items.HeaderId;
                    ViewChecklist.whatToCheck = items.WhatToCheck;
                    ViewChecklist.isNumeric = items.IsNumeric;
                    ViewChecklist.isText = items.IsText;
                    ViewChecklist.roleId = items.RoleId;
                    ViewChecklist.roleName = db.Tblroles.Where(m => m.RoleId == items.RoleId).Select(m => m.RoleName).FirstOrDefault();

                    string[] howSplite = items.How.Split(',');

                    ViewChecklist.how = howSplite;

                    // ViewChecklist.how = items.How;
                    ViewChecklist.frequency = items.Frequency;
                    ViewChecklist.runningHrs = items.RunningHrs;
                    ViewChecklist.partsCount = (int)items.PartsCount;
                    ViewChecklist.periodFrequency = items.PeriodFrequency;

                    if (items.IsOk == "OK")
                    {
                        ViewChecklist.ok = "1";
                    }
                    else if (items.IsOk == "NOTOK")
                    {
                        ViewChecklist.ok = "0";
                        ViewChecklist.comment = items.Comment;
                    }
                    else
                    {
                        ViewChecklist.ok = null;
                    }
                    // ddd.dateArray = items.Date;

                    // List<date> dddlist = new List<date>();

                    string[] datesplite = items.Date.Split(',');
                    ViewChecklist.dateList = datesplite;

                    // string[] datesplite = data.Date.Split(',');
                    //  string showdate = "";
                    string[] showdate;

                    List<string> datelist = new List<string>();
                    foreach (var dt in datesplite)
                    {
                        DateTime ddtt = Convert.ToDateTime(dt);
                        string dt1 = ddtt.ToString("dd-MM-yyyy");

                        datelist.Add(dt1);
                        //savedate += dt1 + ",";

                    }

                    showdate = datelist.ToArray();

                    ViewChecklist.showDateList = showdate;


                    // date[] ddd = datesplite;

                    //foreach (var dd in datesplite)
                    //{
                    //    date ddd = new date();
                    //    ddd.dateArray = dd;
                    //    string[] dddlist = ddd;
                    //   // dddlist.Add(ddd);
                    //}

                    //if (items.Date == null)
                    //{
                    //    ViewChecklist.date =null;
                    //}
                    //else
                    //{
                    //    DateTime dt = (DateTime)items.Date;
                    //    string date1 = dt.ToString("yyyy-MM-dd");
                    //    ViewChecklist.date = date1;

                    //}

                    ViewCheckedListDet.Add(ViewChecklist);

                }

                if (ViewCheckedListDet.Count > 0)
                {
                    obj.isStatus = true;
                    obj.response = ViewCheckedListDet;
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

        public CommonResponseWithStatus ViewMultipleCheckListDetailsByHeaderId(int headerId)
        {
            CommonResponseWithStatus obj = new CommonResponseWithStatus();
            try
            {
                List<ViewCheckList> ViewCheckedListDet = new List<ViewCheckList>();
                obj.ovarallStatus = true;
                var tblDet = db.TblCheckListDetails.Where(m => m.IsDeleted == 0 && m.HeaderId == headerId).ToList();
                if (tblDet.Count > 0)
                {
                    var okList = db.TblCheckListDetails.Where(m => m.HeaderId == headerId && m.IsOk == "OK").ToList();
                    int okCount = okList.Count();

                    var notOkList = db.TblCheckListDetails.Where(m => m.HeaderId == headerId && m.IsOk == "NOTOK").ToList();
                    int notOkCount = notOkList.Count();

                    if(notOkCount > 0)
                    {
                        obj.ovarallStatus = false;
                    }

                    foreach (var items in tblDet)
                    {
                        ViewCheckList ViewChecklist = new ViewCheckList();
                        ViewChecklist.checkListId = items.CheckListId;
                        ViewChecklist.headerId = (int)items.HeaderId;
                        ViewChecklist.whatToCheck = items.WhatToCheck;

                        if (items.How != null)
                        {
                            string[] howSplite = items.How.Split(',');
                            ViewChecklist.how = howSplite;
                        }
                        else
                        {
                            ViewChecklist.how = new string[] { };
                        }

                        ViewChecklist.frequency = items.Frequency;
                        ViewChecklist.runningHrs = items.RunningHrs;
                        ViewChecklist.partsCount = (int)items.PartsCount;
                        ViewChecklist.periodFrequency = items.PeriodFrequency;
                        ViewChecklist.isNumeric = items.IsNumeric;
                        ViewChecklist.isText = items.IsText;
                        ViewChecklist.roleId = items.RoleId;
                        ViewChecklist.roleName = db.Tblroles.Where(m => m.RoleId == items.RoleId).Select(m => m.RoleName).FirstOrDefault();
                        // ViewChecklist.ok = items.IsOk;
                        if (items.IsOk == "OK")
                        {
                            ViewChecklist.ok = "1";
                        }
                        else if (items.IsOk == "NOTOK")
                        {
                            ViewChecklist.ok = "0";
                            ViewChecklist.comment = items.Comment;
                        }
                        else
                        {
                            ViewChecklist.ok = null;
                        }

                        if (items.Date != "")
                        {
                            string[] datesplite = items.Date.Split(',');
                            ViewChecklist.dateList = datesplite;
                            string[] showdate;

                            List<string> datelist = new List<string>();
                            foreach (var dt in datesplite)
                            {
                                DateTime ddtt = Convert.ToDateTime(dt);
                                string dt1 = ddtt.ToString("dd-MM-yyyy");

                                datelist.Add(dt1);
                            }

                            showdate = datelist.ToArray();
                            ViewChecklist.showDateList = showdate;
                        }
                        else
                        {
                            ViewChecklist.showDateList = new string[] { };
                        }

                        ViewCheckedListDet.Add(ViewChecklist);
                    }
                }
                else
                {

                }

                if (ViewCheckedListDet.Count > 0)
                {
                    obj.isStatus = true;
                    obj.response = ViewCheckedListDet;
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

        public CommonResponse ViewCheckListById(int id)
        {

            CommonResponse obj = new CommonResponse();
            try
            {

                var items = db.TblCheckListDetails.Where(m => m.IsDeleted == 0 && m.CheckListId == id).FirstOrDefault();

                ViewCheckList ViewChecklist = new ViewCheckList();
                ViewChecklist.checkListId = items.CheckListId;
                ViewChecklist.headerId = (int)items.HeaderId;
                ViewChecklist.whatToCheck = items.WhatToCheck;
                // ViewChecklist.how = items.How;

                string[] howSplite = items.How.Split(',');

                ViewChecklist.how = howSplite;

                ViewChecklist.frequency = items.Frequency;
                ViewChecklist.runningHrs = items.RunningHrs;
                ViewChecklist.partsCount = (int)items.PartsCount;
                ViewChecklist.periodFrequency = items.PeriodFrequency;
                ViewChecklist.isNumeric = items.IsNumeric;
                ViewChecklist.isText = items.IsText;
                ViewChecklist.roleId = items.RoleId;
                ViewChecklist.roleName = db.Tblroles.Where(m => m.RoleId == items.RoleId).Select(m => m.RoleName).FirstOrDefault();

                if (items.IsOk == "OK")
                {
                    ViewChecklist.ok = "1";
                }
                else if (items.IsOk == "NOTOK")
                {
                    ViewChecklist.ok = "0";
                    ViewChecklist.comment = items.Comment;
                }
                else
                {
                    ViewChecklist.ok = null;
                }
                // string[] datesplite = items.Date.Split(',');
                // ViewChecklist.dateList = datesplite;



                string[] datesplite = items.Date.Split(',');
                ViewChecklist.dateList = datesplite;

                // string[] datesplite = data.Date.Split(',');
                //  string showdate = "";
                string[] showdate;

                List<string> datelist = new List<string>();
                foreach (var dt in datesplite)
                {
                    DateTime ddtt = Convert.ToDateTime(dt);
                    string dt1 = ddtt.ToString("dd-MM-yyyy");

                    datelist.Add(dt1);
                    //savedate += dt1 + ",";

                }

                showdate = datelist.ToArray();

                ViewChecklist.showDateList = showdate;


                //string[] datesplite = items.Date.Split(',');

                //foreach (var dd in datesplite)
                //{
                //    date ddd = new date();
                //    ddd.dateArray = dd;
                //    dddlist.Add(ddd);
                //}
                //ViewChecklist.dateList = dddlist;

                // ViewChecklist.dateList.dateArray = items.Date;

                //if (items.Date == null)
                //{
                //    ViewChecklist.date = null;
                //}
                //else
                //{
                //    DateTime dt = (DateTime)items.Date;
                //    string date1 = dt.ToString("yyyy-mm-dd");
                //    ViewChecklist.date = date1;

                //}

                if (ViewChecklist != null)
                {
                    obj.isStatus = true;
                    obj.response = ViewChecklist;
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

        public CommonResponse DeleteCheckListDetails(int id)
        {
            CommonResponse obj = new CommonResponse();
            try
            {
                var check = db.TblCheckListDetails.Where(m => m.CheckListId == id && m.IsDeleted == 0).FirstOrDefault();
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
                log.Error(e); if (e.InnerException != null) { log.Error(e.InnerException.ToString()); }
                obj.isStatus = false;
                obj.response = ResourceResponse.FailureMessage;
            }
            return obj;
        }

        //public CommonResponse ApprovedCheckList(approveDet data)
        //{
        //    CommonResponse obj = new CommonResponse();
        //    CommonFunction commonFunction = new CommonFunction();
        //    try
        //    {

        //        var check = db.TblCheckListHeader.Where(m => m.HeaderId == data.HeaderId && m.IsDeleted == 0).FirstOrDefault();
        //        if (check != null)
        //        {

        //            string[] checklist = data.checkListIds.Split(',');
        //            int count1 = checklist.Count();

        //            string[] okIdSplite = data.okIds.Split(',');
        //            int count2 = okIdSplite.Count();


        //            string[] notOkIdSplite = data.notOkIds.Split(',');

        //            if (!string.IsNullOrEmpty(data.okIds))
        //            {
        //                foreach (var ok in okIdSplite)
        //                {
        //                    int okid = Convert.ToInt32(ok);
        //                    var checkDet = db.TblCheckListDetails.Where(m => m.CheckListId == okid).FirstOrDefault();
        //                    checkDet.IsOk = "ok";
        //                    db.SaveChanges();

        //                }
        //                if (count1 == count2)
        //                {
        //                    check.IsApproved = 1;


        //                }

        //            }

        //            if (!string.IsNullOrEmpty(data.notOkIds))
        //            {
        //                foreach (var notok in notOkIdSplite)
        //                {
        //                    int notokid = Convert.ToInt32(notok);
        //                    var checkDet = db.TblCheckListDetails.Where(m => m.CheckListId == notokid).FirstOrDefault();
        //                    checkDet.IsOk = "not ok";
        //                    db.SaveChanges();

        //                }

        //            }



        //             check.ApprovedBy = data.approverId;

        //            check.CreationDate = DateTime.Now;
        //            check.LastRevDate = DateTime.Now;
        //            check.ApprovedByDate = DateTime.Now;
        //           // check.ApprovedByDate =Convert.ToDateTime(data.approvedDate);
        //            db.SaveChanges();
        //            obj.isStatus = true;
        //            obj.response = ResourceResponse.AddedSuccessMessage;


        //        }
        //        else
        //        {
        //            obj.isStatus = false;
        //            obj.response = "Header Id not found";

        //        }



        //    }
        //    catch (Exception e)
        //    {
        //        log.Error(e); if (e.InnerException != null) { log.Error(e.InnerException.ToString()); }
        //        obj.isStatus = false;
        //        obj.response = ResourceResponse.FailureMessage;
        //    }
        //    return obj;
        //}

        public CommonResponse ApprovedCheckList(approveDet data)
        {
            CommonResponse obj = new CommonResponse();
            CommonFunction commonFunction = new CommonFunction();
            try
            {
                var check = db.TblCheckListHeader.Where(m => m.HeaderId == data.headerId && m.IsDeleted == 0).FirstOrDefault();
                if (check != null)
                {
                    var checkDet = db.TblCheckListDetails.Where(m => m.CheckListId == data.checkListId && m.IsDeleted == 0 && m.HeaderId == data.headerId).FirstOrDefault();
                    if (checkDet != null)
                    {
                        if(data.isOk == "OK")
                        {
                            checkDet.IsApproved = 1;
                        }
                        else
                        {
                            checkDet.IsApproved = 0;
                            checkDet.IsPrepared = 0;
                        }

                        checkDet.IsOk = data.isOk;
                        checkDet.Comment = data.comment;
                        db.SaveChanges();

                        var count1 = db.TblCheckListDetails.Where(m => m.HeaderId == check.HeaderId && m.IsDeleted == 0).ToList();
                        var count2 = db.TblCheckListDetails.Where(m => m.HeaderId == check.HeaderId && m.IsOk == "OK" && m.IsDeleted == 0).ToList();


                        int l1 = count1.Count();
                        int l2 = count2.Count();

                        if (l1 == l2)
                        {
                            check.IsApproved = 1;

                        }
                        check.ApprovedBy = data.approverId;
                        check.CreationDate = DateTime.Now;
                        check.LastRevDate = DateTime.Now;
                        check.ApprovedByDate = DateTime.Now;
                        db.SaveChanges();
                        obj.isStatus = true;
                        obj.response = ResourceResponse.AddedSuccessMessage;
                    }
                    else
                    {
                        obj.isStatus = false;
                        obj.response = "checkList Id not found";
                    }
                }
                else
                {
                    obj.isStatus = false;
                    obj.response = "Header Id not found";

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
