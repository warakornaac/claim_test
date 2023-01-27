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
    public class CheckstatusRtController : Controller
    {
        //
        // GET: /CheckstatusRt/

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
                else if (UserType == "8" || UserType == "9")
                {
                    return RedirectToAction("Index", "Disposal");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
               
                // else
                // {
                // return RedirectToAction("Index", "Checkstatus_Sc");
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                //}

            }
            return View();
           
        }
        public JsonResult GetClimcheckdata(string inslmcod,string inclaimnokey,string incompany,string incusno, string initemno ,string instkgrp, string  instatdate,string inenddate, string instatusrt ,string instatdatepm,string inenddatepm,string instatuspm,string instaturec,string inclimstatus,string instatuswh,string inproceedtofter,string instatusprint,string incktyp)
        {

            string message = string.Empty;
            Salesreturnsupper model = null;
            List<SalesreturnsupperListDetail> Getdata = new List<SalesreturnsupperListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessCheckStstusRT", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inslmcod", inslmcod);
            command.Parameters.AddWithValue("@inclaimnokey", inclaimnokey);
            command.Parameters.AddWithValue("@incompany", incompany);
            command.Parameters.AddWithValue("@incusno", incusno);
            command.Parameters.AddWithValue("@initemno", initemno);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@instatusrt", instatusrt);
            command.Parameters.AddWithValue("@instatdatepm", instatdatepm);
            command.Parameters.AddWithValue("@inenddatepm", inenddatepm);
            command.Parameters.AddWithValue("@instatuspm", instatuspm);
            command.Parameters.AddWithValue("@instaturec", instaturec);
            command.Parameters.AddWithValue("@inclimstatus", inclimstatus);
            command.Parameters.AddWithValue("@instatusw", instatuswh);
            command.Parameters.AddWithValue("@inproceedtofter", inproceedtofter);
            command.Parameters.AddWithValue("@instatusprin", instatusprint);
            command.Parameters.AddWithValue("@incktyp", incktyp);
           
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Salesreturnsupper();

                model.REQ_ID = dr["REQ_ID"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.STMP_REQQTY = dr["STMP_REQQTY"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_CAUSE = dr["STMP_CAUSE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.STMP_RCVDATE = dr["STMP_REQUESTDATE"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.STMP_LineAMT = dr["STMP_LineAMT"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.SMSUP_APPRV_STATUS = dr["SMSUP_APPRV_STATUS"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.PM_APPRV_DATE = dr["PM_APPRV_DATE"].ToString();
                model.PM_APPRV_STATUS = dr["PM_APPRV_STATUS"].ToString();
                model.PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString();
                model.STMP_STATUS = dr["STMP_STATUS"].ToString();
                model.STMP_STATUSDES = dr["STMP_STATUSDES"].ToString();
                model.STMP_ADMIN = dr["STMP_ADMIN"].ToString();
                model.STMP_ADMIN_REQQTY = dr["STMP_ADMIN_REQQTY"].ToString();
                model.STMP_ADMIN_REQ_DATE = dr["STMP_ADMIN_REQ_DATE"].ToString();
                model.ResonDes = dr["ResonDes"].ToString();
                model.CN_No = dr["CN_No"].ToString();
                Getdata.Add(new SalesreturnsupperListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult ProcessCheckRT(string nokey)
        {
             string message = string.Empty;
            Salesreturnsupper model = null;
            List<SalesreturnsupperListDetail> Getdata = new List<SalesreturnsupperListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Chk_Process_Controll_RT", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inrtnokey", nokey);         
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Salesreturnsupper();

                model.REQ_ID = dr["REQ_ID"].ToString();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.STMP_COMPANY = dr["STMP_COMPANY"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                //model.SLMCOD = dr["SLMCOD"].ToString();
                //model.SLMNAM = dr["SLMNAM"].ToString();
                Getdata.Add(new SalesreturnsupperListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
          }

        public JsonResult Cancelclaim(string User, string req_no, string clm_no_sub, string reasonwh)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Cancel_ClaimRT", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", req_no);
                command.Parameters.AddWithValue("@inCLM_SUB", clm_no_sub);
                command.Parameters.AddWithValue("@inusrlogin", User);
                command.Parameters.AddWithValue("@inreasonwh", reasonwh);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
               // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Updatercvstatusprocess(string User, string DataSend)
        {
            string message = string.Empty;
            List<Datasave> _ItemList = new JavaScriptSerializer().Deserialize<List<Datasave>>(DataSend);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_updatePrintordercarRT", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inUser", _ItemList[i].user);
                    command.Parameters.AddWithValue("@inCim_No", _ItemList[i].req_no);
                    command.Parameters.AddWithValue("@inCim_NoSub", _ItemList[i].clm_no_sub);


                    command.ExecuteNonQuery();

                    command.Dispose();
                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
    }
}
