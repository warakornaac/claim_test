using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.Web.Script.Serialization;
namespace ClaimWap.Controllers
{
    public class Process_RtController : Controller
    {
        //
        // GET: /Process_Rt/
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
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                string Doc = string.Empty;
                string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                string SubDocwords = string.Empty;
                Docdisplay = Request.QueryString["ClmNUM"];

                if (Docdisplay != null)
                {
                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);


                }
                ViewBag.Claimno = Doc;
                // ViewBag.Claimsubno = Docsub;
            }
            return View();
        }
        public JsonResult Getsalesreturndetail(string indoc)
        {

            string message = string.Empty;
            SalesreturnDetail model = null;
            List<SalesreturnDetailList> Getdata = new List<SalesreturnDetailList>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Process_SalesReturnconfirm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", indoc);
            //command.Parameters.AddWithValue("@DOCSUB", indocsup);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new SalesreturnDetail();

                model.CUSCOD = dr["CUSCOD"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_LastDocNo = dr["STMP_LastDocNo"].ToString();
                model.STMP_LastDocDate = dr["STMP_LastDocDate"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.GRPNAM = dr["GRPNAM"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_QTY = dr["STMP_QTY"].ToString();
                model.STMP_UOM = dr["STMP_UOM"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_REQUESTDATE = dr["STMP_REQUESTDATE"].ToString();
                model.STMP_DATE = dr["STMP_DATE"].ToString();
                model.STMP_ADMIN = dr["STMP_ADMIN"].ToString();
                model.STMP_ADMIN_REQQTY = dr["STMP_ADMIN_REQQTY"].ToString();
                model.STMP_ADMIN_REQ_DATE = dr["STMP_ADMIN_REQ_DATE"].ToString();
                model.SMSUP_CODE = dr["SMSUP_CODE"].ToString();
                model.SMSUP_APPRV_DATE = dr["SMSUP_APPRV_DATE"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SMSUP_REMARK = dr["SMSUP_REMARK"].ToString();
                model.PM_NAME = dr["PM_NAME"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.PM_REMARK = dr["PM_REMARK"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();
                model.ADMIN_REMARK = dr["ADMIN_REMARK"].ToString();
                model.Remake_Admin = dr["Remake_Admin"].ToString();
                model.STMP_STATUS_CENTER = dr["STMP_STATUS_CENTER"].ToString();
                model.STMP_REASON_WH = dr["STMP_REASON_WH"].ToString();
                Getdata.Add(new SalesreturnDetailList { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult SaveProcessSalesReturnAdminWh(string DataSend, string stmp_whadmin, string stmp_whreqdate, string stmp_whremark, string stmp_insertby)
        {
            string message = string.Empty;
            string subno = string.Empty;
            List<Rtwhadmin> _ItemList = new JavaScriptSerializer().Deserialize<List<Rtwhadmin>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            SqlTransaction tran = Connection.BeginTransaction();

            try
            {


                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Process_SalesReturn_AdminWh", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = tran;
                    command.Parameters.AddWithValue("@inSTMP_ID",  _ItemList[i].STMP_ID);
                    command.Parameters.AddWithValue("@inSTMP_ID_SUB", _ItemList[i].STMP_ID_SUB);
                    command.Parameters.AddWithValue("@inSTMP_STKCOD", _ItemList[i].STMP_STKCOD);
                    command.Parameters.AddWithValue("@inSTMP_ADMIN", stmp_whadmin);
                    command.Parameters.AddWithValue("@inSTMP_ADMIN_REQQTY",  _ItemList[i].STMP_ADMIN_REQQTY);
                    command.Parameters.AddWithValue("@inSTMP_ADMIN_REQ_DATE", stmp_whreqdate);
                    command.Parameters.AddWithValue("@inADMIN_REMARK", _ItemList[i].ADMIN_REMARK);
                    command.Parameters.AddWithValue("@inADMIN_REMARKHEAD", stmp_whremark);
                    command.Parameters.AddWithValue("@inSTMP_QTY", _ItemList[i].STMP_ADMIN_REQQTY);
                    command.Parameters.AddWithValue("@inSTMP_UPDATE_DATE", "");
                    command.Parameters.AddWithValue("@inSTMP_UPDATE_BY", stmp_insertby);
                    command.Parameters.AddWithValue("@inSTMP_STATUS","");
                    command.Parameters.AddWithValue("@inuserlogin", _ItemList[i].userlogin);
                    SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValuedoc);
                    command.ExecuteNonQuery();
                    subno = returnValuedoc.Value.ToString();
                    command.Dispose();
                    message = "true";
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

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
    }   
}
