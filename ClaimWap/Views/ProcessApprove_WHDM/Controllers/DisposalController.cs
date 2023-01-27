using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.Data;
using ClaimWap.Controllers;
using System.IO;
using System.Web.Script.Serialization;
namespace ClaimWap.Controllers
{
    public class DisposalController : Controller
    {
        //
        // GET: /Disposal/

        public ActionResult Index()
        {
            if (Session["UserID"] == null && Session["UserPassword"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {
                string User = Session["UserID"].ToString();
                string UserType = Session["UserType"].ToString();
                if (UserType == "3")
                {
                    return RedirectToAction("Index", "ProcessApprove");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
               
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                //}

            }
            return View();
        }
        public JsonResult GetClimcDisposal(string instacs,string instasupre,string instalog,string intecwork, string inshelfloctec, string instswewr, string incompany, string initemno, string instatdate, string inenddate, string instkgrp, string statusprint, string ingendoc)
        {
            string message = string.Empty;
            ClimeDetail model = null;
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Process_Disposal", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inDOCGEN", ingendoc);
            command.Parameters.AddWithValue("@inCOM", incompany);
            command.Parameters.AddWithValue("@inSTKCOD", initemno);
            command.Parameters.AddWithValue("@inSTATS", statusprint);        
            command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@inDisposal", "");
            command.Parameters.AddWithValue("@inwewrwf", instswewr);
            command.Parameters.AddWithValue("@intecwork", intecwork);
            command.Parameters.AddWithValue("@inshelfloctec", inshelfloctec);
            command.Parameters.AddWithValue("@instacs", instacs);
            command.Parameters.AddWithValue("@instasupr", instasupre);
            command.Parameters.AddWithValue("@instalog", instalog);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimeDetail();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = dr["CLM_REMARK"].ToString();
                model.CLM_QTY = dr["CLM_REQQTY"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.Technician = dr["Technician"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.TECH1_NAME = dr["TECH1_NAME"].ToString();
                model.TECH1_ANLYS_DATE = dr["TECH1_ANLYS_DATE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.ANLYS_AFTERPROCESS = dr["ANLYS_AFTERPROCESS"].ToString();
                model.SCRAP_DATE = dr["SCRAP_DATE"].ToString();
                model.TECH1_ANLYS_RESULT = dr["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_REMARK = dr["TECH2_REMARK"].ToString();
                model.TECH2_ANLYS_DATE = dr["TECH2_ANLYS_DATE"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_Replacement = dr["PM_Replacement"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                //model.CLM_UPDATE_DATE = "";
                // model.CLM_UPDATE_BY = "";
                model.Customer = dr["CUSCOD"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.CLM_FRMSUP_STATUS = dr["CLM_FRMSUP_STATUS"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.STATUSTEXT = dr["STATUSTEXT"].ToString();
                model.CLM_PERFORMTEXT = dr["CLM_PERFORMTEXT"].ToString();
                model.ANLYS_AFTERPROCESSTEXT = dr["ANLYS_AFTERPROCESSTEXT"].ToString();
                model.CLM_FRMSUP_DATE = dr["CLM_FRMSUP_DATE"].ToString();
                model.CLM_FRMSUP_NO = dr["CLM_FRMSUP_NO"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.FCLM_QTY = dr["FCLM_QTY"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.CLM_FOC = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                //model.Print_statusCN = dr["Print_statusCN"].ToString();
                model.IF_InvoiceNo = dr["Invoice_No"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimcDisposalWh(string instacs, string instasupre, string instalog, string intecwork, string inshelfloctec, string instswewr, string incompany, string initemno, string instatdate, string inenddate, string instkgrp, string statusprint, string ingendoc)
        {
            string message = string.Empty;
            ClimeDetail model = null;
            List<ClimeListDetail> Getdata = new List<ClimeListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Process_DisposalWh", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inDOCGEN", "");
            command.Parameters.AddWithValue("@inCOM", incompany);
            command.Parameters.AddWithValue("@inSTKCOD", initemno);
            command.Parameters.AddWithValue("@inSTATS", statusprint);
            command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@inDisposal", "");
            command.Parameters.AddWithValue("@inwewrwf", instswewr);
            command.Parameters.AddWithValue("@intecwork", intecwork);
            command.Parameters.AddWithValue("@inshelfloctec", inshelfloctec);
            command.Parameters.AddWithValue("@instacs", instacs);
            command.Parameters.AddWithValue("@instasupr", instasupre);
            command.Parameters.AddWithValue("@instalog", instalog);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ClimeDetail();
                model.CLM_ADMIN = dr["CLM_ADMIN"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.CLM_REMARK = dr["CLM_REMARK"].ToString();
                model.CLM_QTY = dr["CLM_REQQTY"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.Technician = dr["Technician"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.TECH1_NAME = dr["TECH1_NAME"].ToString();
                model.TECH1_ANLYS_DATE = dr["TECH1_ANLYS_DATE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                model.ANLYS_AFTERPROCESS = dr["ANLYS_AFTERPROCESS"].ToString();
                model.SCRAP_DATE = dr["SCRAP_DATE"].ToString();
                model.TECH1_ANLYS_RESULT = dr["TECH1_ANLYS_RESULT"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.TECH2_REMARK = dr["TECH2_REMARK"].ToString();
                model.TECH2_ANLYS_DATE = dr["TECH2_ANLYS_DATE"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_Replacement = dr["PM_Replacement"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                //model.CLM_UPDATE_DATE = "";
                // model.CLM_UPDATE_BY = "";
                model.Customer = dr["CUSCOD"].ToString();
                model.REQ_NO = dr["REQ_NO"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.CLM_FRMSUP_STATUS = dr["CLM_FRMSUP_STATUS"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.STATUSTEXT = dr["STATUSTEXT"].ToString();
                model.CLM_PERFORMTEXT = dr["CLM_PERFORMTEXT"].ToString();
                model.ANLYS_AFTERPROCESSTEXT = dr["ANLYS_AFTERPROCESSTEXT"].ToString();
                model.CLM_FRMSUP_DATE = dr["CLM_FRMSUP_DATE"].ToString();
                model.CLM_FRMSUP_NO = dr["CLM_FRMSUP_NO"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.FCLM_QTY = dr["FCLM_QTY"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.CLM_FOC = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                //model.Print_statusCN = dr["Print_statusCN"].ToString();
                model.IF_InvoiceNo = dr["Invoice_No"].ToString();
                Getdata.Add(new ClimeListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult GetClimcDisposalAftertech(string ingenno)
        {
            string message = string.Empty;
            ItemDisposallineAfter model = null;
            List<ClimeListDetailAfter> Getdata = new List<ClimeListDetailAfter>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Process_Disposal_After_bytech", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inDOC", ingenno);
            //command.Parameters.AddWithValue("@inCOM", incompany);
            //command.Parameters.AddWithValue("@inSTKCOD", initemno);
            //command.Parameters.AddWithValue("@inSTATS", statusprint);
            //command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            //command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            //command.Parameters.AddWithValue("@instkgrp", instkgrp);
            //command.Parameters.AddWithValue("@inDisposal", "");
            //command.Parameters.AddWithValue("@inwewrwf", instswewr);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ItemDisposallineAfter();
                model.CLM_NO_Disposal = dr["CLM_NO_Disposal"].ToString();
                model.REQ_TotalQty = dr["REQ_TotalQty"].ToString();
                model.REQ_TotalItem = dr["REQ_TotalItem"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.Remake = dr["Remake"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.CLM_Qty = dr["CLM_Qty"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.COMP = dr["COMP"].ToString();
                model.ClaimType = dr["ClaimType"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_Foc = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.INVNUM = dr["INVNUM"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.Invoice_No = dr["Invoice_No"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.Original_CLM_Qty = dr["Original_CLM_Qty"].ToString();
                model.Ststus_Scrap = dr["Ststus_Scrap"].ToString();
                Getdata.Add(new ClimeListDetailAfter { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimcDisposalAfter(string ingenno)
        {
            string message = string.Empty;
            ItemDisposallineAfter model = null;
            List<ClimeListDetailAfter> Getdata = new List<ClimeListDetailAfter>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Process_Disposal_After", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inDOC", ingenno);
            //command.Parameters.AddWithValue("@inCOM", incompany);
            //command.Parameters.AddWithValue("@inSTKCOD", initemno);
            //command.Parameters.AddWithValue("@inSTATS", statusprint);
            //command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            //command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            //command.Parameters.AddWithValue("@instkgrp", instkgrp);
            //command.Parameters.AddWithValue("@inDisposal", "");
            //command.Parameters.AddWithValue("@inwewrwf", instswewr);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ItemDisposallineAfter();
                model.CLM_NO_Disposal = dr["CLM_NO_Disposal"].ToString();
                model.REQ_TotalQty = dr["REQ_TotalQty"].ToString();
                model.REQ_TotalItem = dr["REQ_TotalItem"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.Remake = dr["Remake"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.CLM_Qty = dr["CLM_Qty"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.COMP = dr["COMP"].ToString();
                model.ClaimType = dr["ClaimType"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.CLM_Foc = dr["CLM_Foc"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.INVNUM = dr["INVNUM"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.Invoice_No = dr["Invoice_No"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.Original_CLM_Qty = dr["Original_CLM_Qty"].ToString();
                model.Ststus_Scrap = dr["Ststus_Scrap"].ToString();
                Getdata.Add(new ClimeListDetailAfter { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetClimcDisposaltotal(string ingenno)
        {
            string message = string.Empty;
            ItemDisposallineAfter model = null;
            List<ClimeListDetailAfter> Getdata = new List<ClimeListDetailAfter>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Chk_Claim_Disposal", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inCLM_NO_Disposal", ingenno);
            //command.Parameters.AddWithValue("@inCOM", incompany);
            //command.Parameters.AddWithValue("@inSTKCOD", initemno);
            //command.Parameters.AddWithValue("@inSTATS", statusprint);
            //command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            //command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            //command.Parameters.AddWithValue("@instkgrp", instkgrp);
            //command.Parameters.AddWithValue("@inDisposal", "");
            //command.Parameters.AddWithValue("@inwewrwf", instswewr);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new ItemDisposallineAfter();
                model.CLM_NO_Disposal = dr["CLM_NO_Disposal"].ToString();
                model.REQ_TotalQty = dr["REQ_TotalQty"].ToString();
                model.REQ_TotalItem = dr["REQ_TotalItem"].ToString();
              
                Getdata.Add(new ClimeListDetailAfter { val = model });
                message = dr["CLM_NO_Disposal"].ToString();
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata, message }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult SaveClaimDisposal(string commentsend, string com, string Doc, string CLM_NO_Disposal, string DataSend, string usesend, string countstksend, string countnumbersend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<ItemDisposalline> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemDisposalline>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                var command = new SqlCommand("P_Save_Claim_Disposal_H", Connection);
                command.Transaction = tran;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCOMP ", com);
                command.Parameters.AddWithValue("@inCLM_NO_Disposal", CLM_NO_Disposal);
                command.Parameters.AddWithValue("@inREQ_TotalQty", countnumbersend);
                command.Parameters.AddWithValue("@inREQ_TotalItem",countstksend);
                command.Parameters.AddWithValue("@inREQ_BY", usesend);
                command.Parameters.AddWithValue("@inRemake", commentsend);
                command.Parameters.AddWithValue("@inDocNam ", Doc);
                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();

                gendocno = returnValuedoc.Value.ToString();
                if (gendocno != "")
                {

                    for (int i = 0; i < _ItemList.Count; i++)
                    {
                        var commandLine = new SqlCommand("P_Save_Claim_Disposal_L", Connection);
                        commandLine.CommandType = CommandType.StoredProcedure;
                        commandLine.Transaction = tran;

                            commandLine.Parameters.AddWithValue("@inCLM_NO_Disposal", gendocno);
		                    commandLine.Parameters.AddWithValue("@inCompany", _ItemList[i].Company);
		                    commandLine.Parameters.AddWithValue("@inREQ_NO", "");
                            commandLine.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].CLM_NO_SUB);
                            commandLine.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].STKCOD);
                            commandLine.Parameters.AddWithValue("@inCLM_Qty", _ItemList[i].CLM_Qty);
		                    commandLine.Parameters.AddWithValue("@inCLM_Disposal_Date", com);
		                    commandLine.Parameters.AddWithValue("@inScrap_Acc_Qty ",_ItemList[i].Scrap_Acc_Qty);
		                    commandLine.Parameters.AddWithValue("@inScrap_Acc_Date","");
		                    commandLine.Parameters.AddWithValue("@inReplacement","");
		                    commandLine.Parameters.AddWithValue("@inStatus_Scrap",_ItemList[i].Status_Scrap);
		                    commandLine.Parameters.AddWithValue("@inInsertBy", _ItemList[i].InsertBy);
		                    commandLine.Parameters.AddWithValue("@inInsertDate","");
                            commandLine.Parameters.AddWithValue("@inUpdateby", _ItemList[i].InsertBy);
		                    commandLine.Parameters.AddWithValue("@inUpdateDate ", "");
                            commandLine.Parameters.AddWithValue("@inCommentTec", _ItemList[i].CommentTec);
                            commandLine.Parameters.AddWithValue("@inCommentAcc", _ItemList[i].CommentAcc);
                            SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                            returnValue.Direction = System.Data.ParameterDirection.Output;
                            commandLine.Parameters.Add(returnValue);
                            commandLine.ExecuteNonQuery();
                            message = returnValue.Value.ToString();
                            commandLine.Dispose();
                   
                    }
                }
                command.Dispose();
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddClaimDisposal(string ingenno, string DataSend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            List<ItemDisposalline> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemDisposalline>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                        gendocno=ingenno;
                      

                    for (int i = 0; i < _ItemList.Count; i++)
                    {
                        var commandLine = new SqlCommand("P_Add_Claim_Disposal", Connection);
                        commandLine.CommandType = CommandType.StoredProcedure;
                        commandLine.Transaction = tran;

                        commandLine.Parameters.AddWithValue("@inCLM_NO_Disposal", gendocno);
                        commandLine.Parameters.AddWithValue("@inCompany", _ItemList[i].Company);
                        commandLine.Parameters.AddWithValue("@inREQ_NO", "");
                        commandLine.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].CLM_NO_SUB);
                        commandLine.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].STKCOD);
                        commandLine.Parameters.AddWithValue("@inCLM_Qty", _ItemList[i].CLM_Qty);
                        commandLine.Parameters.AddWithValue("@inCLM_Disposal_Date", "");
                        commandLine.Parameters.AddWithValue("@inScrap_Acc_Qty ", _ItemList[i].Scrap_Acc_Qty);
                        commandLine.Parameters.AddWithValue("@inScrap_Acc_Date", "");
                        commandLine.Parameters.AddWithValue("@inReplacement", "");
                        commandLine.Parameters.AddWithValue("@inStatus_Scrap", _ItemList[i].Status_Scrap);
                        commandLine.Parameters.AddWithValue("@inInsertBy", _ItemList[i].InsertBy);
                        commandLine.Parameters.AddWithValue("@inInsertDate", "");
                        commandLine.Parameters.AddWithValue("@inUpdateby", _ItemList[i].InsertBy);
                        commandLine.Parameters.AddWithValue("@inUpdateDate ", "");
                        commandLine.Parameters.AddWithValue("@inCommentTec", _ItemList[i].CommentTec);
                        commandLine.Parameters.AddWithValue("@inCommentAcc", _ItemList[i].CommentAcc);
                        commandLine.Parameters.AddWithValue("@inREQ_TotalItem", 1);
                        commandLine.Parameters.AddWithValue("@inREQ_TotalQty", 1);
                          
                        SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                        returnValue.Direction = System.Data.ParameterDirection.Output;
                        commandLine.Parameters.Add(returnValue);
                        commandLine.ExecuteNonQuery();
                        message = returnValue.Value.ToString();
                        commandLine.Dispose();

                    }
                
                
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ReopenClaimDisposal(string CLM_NO_Disposal,string usesend)
        {
            string message = string.Empty;
            string gendocno = string.Empty;
            
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {

                var command = new SqlCommand("P_Reopen_Claim_Disposal", Connection);
                command.Transaction = tran;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_NO_Disposal", CLM_NO_Disposal);
                command.Parameters.AddWithValue("@inREQ_BY", usesend);
             
                SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                returnValue.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.ExecuteNonQuery();

                message = returnValue.Value.ToString();
                command.Dispose();
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, gendocno }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteClaimDisposal(string DataSend ,string usesend)
        {
            string message = string.Empty;
            List<ItemDisposaldelete> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemDisposaldelete>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();
            try
            {
              
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Del_Claim_Disposal", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = tran;
                    command.Parameters.AddWithValue("@inCLM_NO_Disposal", _ItemList[i].CLM_NO_Disposal);
                    command.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].CLM_NO_SUB);
                    command.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].STKCOD);
                    command.Parameters.AddWithValue("@inCOM", _ItemList[i].Company);
                    command.Parameters.AddWithValue("@infix", _ItemList[i].fix);
                    command.Parameters.AddWithValue("@inInsertBy", usesend);
                    SqlParameter returnValue = new SqlParameter("@outsaveResult", SqlDbType.NVarChar, 100);
                    returnValue.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.ExecuteNonQuery();
                    message = returnValue.Value.ToString();
                    command.Dispose();
                }
               
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();

                //throw;
                message = ex.Message;
            }
            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
    }
    public class ItemDisposal
    {
        public string CartID { get; set; }
        public string VSTCLMNO { get; set; }
        public string VSTKCOD { get; set; }
        public string VUNITPRICE { get; set; }
        public string Clmsupno { get; set; }
        public string VQty { get; set; }
        public string Vsupplier { get; set; }
        public string keysupplier { get; set; }

    }
    public class ItemDisposaldelete
    {
       public string CLM_NO_Disposal{ get; set; }
       public string CLM_NO_SUB{ get; set; }
       public string STKCOD{ get; set; }
       public string Company{ get; set; }
       public string CLM_Qty{ get; set; }
       public string InsertBy{ get; set; }
       public string fix { get; set; }
    }
    public class ItemDisposalline
    {
        public string REQ_ID { get; set; }
        public string CLM_NO_Disposal { get; set; }
        public string Company { get; set; }
        public string REQ_NO { get; set; }
        public string CLM_NO_SUB { get; set; }
        public string STKCOD { get; set; }
        public string CLM_Qty { get; set; }
        public string CLM_Disposal_Date { get; set; }
        public string Scrap_Acc_Qty { get; set; }
        public string Scrap_Acc_Date { get; set; }
        public string Replacement { get; set; }
        public string Status_Scrap { get; set; }
        public string InsertBy { get; set; }
        public string InsertDate { get; set; }
        public string Updateby { get; set; }
        public string UpdateDate { get; set; }
        public string CommentTec { get; set; }
        public string CommentAcc { get; set; }
    }

    public class ItemDisposallineAfter
    {
        public string CLM_NO_Disposal { get; set; }
        public string REQ_TotalQty { get; set; }
        public string REQ_TotalItem { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_DATE { get; set; }
        public string Remake { get; set; }
        public string CLM_NO_SUB { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string STKGRP { get; set; }
        public string CLM_Qty { get; set; }
        public string CLM_UOM { get; set; }
        public string CLM_SHELF_LOCATION { get; set; }
        public string COMP { get; set; }
        public string ClaimType { get; set; }
        public string ADMIN_ANLYS_STATUS { get; set; }
        public string CLM_Foc { get; set; }
        public string CS_No { get; set; }
        public string INVNUM { get; set; }
        public string Log_No { get; set; }
        public string Invoice_No { get; set; }
        public string CLM_DUEDATE { get; set; }
        public string CLM_DATE { get; set; }
        public string Original_CLM_Qty { get; set; }
        public string Ststus_Scrap { get; set; }
    }
    public class ClimeListDetailAfter
    {
        public ItemDisposallineAfter val { get; set; }

    }
}
