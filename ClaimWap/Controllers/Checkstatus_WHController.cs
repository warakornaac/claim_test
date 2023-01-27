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
using System.Net;
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class Checkstatus_WHController : Controller
    {
        //
        // GET: /Checkstatus_WH/

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
                //if (UserType == "3")
                //{
                //    return RedirectToAction("Index", "ProcessApprove");
                //    ViewBag.UserId = User;
                //    ViewBag.UserType = UserType;
                //}
                if (UserType == "8" || UserType == "9")
                {
                    return RedirectToAction("Index", "Disposal");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
                //else if (UserType == "10")
                //{
                //    return RedirectToAction("Index", "CheckstatusRt");
                //    ViewBag.UserId = User;
                //    ViewBag.UserType = UserType;
                //}
                // else
                // {
                // return RedirectToAction("Index", "Checkstatus_Sc");
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                //}

            }
            return View();
        }
        public JsonResult CountPrint(string User, string clm_no_sub)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Count_Print_WH", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inCLM_SUB", clm_no_sub);
                command.Parameters.AddWithValue("@inusrlogin", User);
                command.ExecuteNonQuery();

                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClimcheckdata(string inuser, string intype, string inCLM_ID, string incompany, string instatus, string initemno, string instatdate, string inenddate, string instatusrec, string instatusclaimafter, string instkgrp, string statusprint, string proceedtofter)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessCheckStstusClaim_WH", Connection);
            //var command = new SqlCommand("P_Search_ProcessCheckStstusClaim_WH_test2021", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@inDOC", inCLM_ID);
            command.Parameters.AddWithValue("@inDOCSUB", "");
            command.Parameters.AddWithValue("@inCOM", incompany); 
            command.Parameters.AddWithValue("@inSTKCOD", initemno);
            command.Parameters.AddWithValue("@inSTATS", instatus);
            command.Parameters.AddWithValue("@inSTATSREC", instatusrec);
            command.Parameters.AddWithValue("@inRequeststartdate", instatdate);
            command.Parameters.AddWithValue("@inRequestenddate", inenddate);
            command.Parameters.AddWithValue("@instatusclaimafter", "0");
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@inproceed", proceedtofter);
            command.Parameters.AddWithValue("@inprint", statusprint);
            command.Parameters.AddWithValue("@inuser", inuser);
            command.Parameters.AddWithValue("@intype", intype);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_REQQTY = dr["CLM_REQQTY"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CLM_SHELF_LOCATION = dr["CLM_SHELF_LOCATION"].ToString();
                model.ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString();
                model.TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString();
                model.TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString();
                model.TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.CLM_Ref = dr["CLM_Ref"].ToString();
                model.CLM_DATE = dr["CLM_DATE"].ToString();
                model.TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString();
                model.TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString();
                //model.CLM_FOC = dr["CLM_FOC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.F_BtnApp = dr["F_BtnApp"].ToString();
                model.CLM_STATUS = dr["CLM_STATUS"].ToString();
                model.Requestdate = dr["Requestdate"].ToString();
                //model.Requestdatecus = dr["CusRequestdate"].ToString();
                model.CLM_TECH1_QTY = dr["CLM_TECH1_QTY"].ToString();
                model.CLM_TECH2_QTY = dr["CLM_TECH2_QTY"].ToString();
                model.CLM_PM_QTY = dr["CLM_PM_QTY"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_Dep_BY = dr["REQ_Dep_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.CLM_UPDATE_BY = dr["CLM_UPDATE_BY"].ToString();
                model.CLM_UPDATE_DATE = dr["CLM_UPDATE_DATE"].ToString();
                model.CLM_DUEDATE = dr["CLM_DUEDATE"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.STATUSTEXT = dr["STATUSTEXT"].ToString();
                model.ANLYS_AFTERPROCESSTEXT = dr["ANLYS_AFTERPROCESSTEXT"].ToString();
                model.ANLYS_AFTERPROCESS = dr["ANLYS_AFTERPROCESS"].ToString();
                model.P_Claim = dr["P_Claim"].ToString();
                model.User_Print = dr["User_Print"].ToString();

                model.PM_Option1 = dr["PM_Option1"].ToString();
                model.PM_Optiontext1 = dr["PM_Optiontext1"].ToString();
                model.PM_Option2 = dr["PM_Option2"].ToString();
                model.PM_Optiontext2 = dr["PM_Optiontext2"].ToString();
                model.PM_Option3 = dr["PM_Option3"].ToString();
                model.PM_Optiontext3 = dr["PM_Optiontext3"].ToString();
                model.PM_Option4 = dr["PM_Option4"].ToString();
                model.PM_Optiontext4 = dr["PM_Optiontext4"].ToString();
                model.PM_Option5 = dr["PM_Option5"].ToString();
                model.PM_Optiontext5 = dr["PM_Optiontext5"].ToString();


                model.BU_Head = dr["BU_Head"].ToString();
                model.BU_APPRV_DATE = dr["BU_APPRV_DATE"].ToString();
                model.BU_APPRV_STATUS = dr["BU_APPRV_STATUS"].ToString();
                model.BU_REMARK = dr["BU_REMARK"].ToString();
                model.CLM_BU_QTY = dr["CLM_BU_QTY"].ToString();
                model.CLM_BU_Ref = dr["CLM_BU_Ref"].ToString();

                model.MD_Head = dr["MD_Head"].ToString();
                model.MD_APPRV_DATE = dr["MD_APPRV_DATE"].ToString();
                model.MD_APPRV_STATUS = dr["MD_APPRV_STATUS"].ToString();
                model.MD_REMARK = dr["MD_REMARK"].ToString();
                model.CLM_MD_QTY = dr["CLM_MD_QTY"].ToString();
                model.CLM_MD_Ref = dr["CLM_MD_Ref"].ToString();

                Getdata.Add(new ClimetempListDetail { val = model });
                
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult SaveProcessClaimUpdateAdminWh(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_ADMINWH_BY, string aj_ADMINWH_REMARK)
        {

            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_UpdateComment_AdminWh", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inADMINWH_BY", aj_ADMINWH_BY);
                command.Parameters.AddWithValue("@inADMINWH_REMARK", aj_ADMINWH_REMARK);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        } 
         public JsonResult Cancelclaim(string User,string req_no,string clm_no_sub){
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Cancel_ClaimWH", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", req_no);
                command.Parameters.AddWithValue("@inCLM_SUB", clm_no_sub);
                command.Parameters.AddWithValue("@inusrlogin", User);
              


                command.ExecuteNonQuery();
               
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message}, JsonRequestBehavior.AllowGet);
        }
    }
    }

