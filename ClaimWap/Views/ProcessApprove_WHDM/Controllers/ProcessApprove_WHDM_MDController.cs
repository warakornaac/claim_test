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

namespace ClaimWap.Controllers
{
    public class ProcessApprove_WHDM_MDController : Controller
    {
        //
        // GET: /ProcessApprove_WHDM_MD/

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

            }

            return View();
        }
        public JsonResult GetdataBuManager(string UserId)
        {
            List<Pm> List = new List<Pm>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Product_BuManager", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUsrID", UserId);
            command.Parameters.AddWithValue("@inType", 2);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Pm()
                {
                    PROD = dr["PROD"].ToString(),
                    PRODNAM = dr["PRODNAM"].ToString()

                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdataBuStkgroup(string UserId)
        {
            List<Stkgrp> List = new List<Stkgrp>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Product_BuManager", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inUsrID", UserId);
            command.Parameters.AddWithValue("@inType", 1);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                List.Add(new Stkgrp()
                {
                    STKGRP = dr["STKGRP"].ToString(),
                    GRPNAM = dr["GRPNAM"].ToString()
                });

            }

            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();


            return Json(List, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveProcessClaimwhDamangeMD(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_MD_APPRV_STATUS, string aj_MD_NAME, string aj_MD_REMARK, string aj_MD_QTY, string aj_MD_userlogin)
        {

            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {

                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_Damange_MD", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inMD_NAME", aj_MD_NAME);
                command.Parameters.AddWithValue("@inMD_APPRV_STATUS", aj_MD_APPRV_STATUS);

                command.Parameters.AddWithValue("@inMD_REMARK", aj_MD_REMARK);
                command.Parameters.AddWithValue("@inuserlogin", aj_MD_userlogin);
                command.Parameters.AddWithValue("@inMD_QTY", aj_MD_QTY);
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

    }
}
