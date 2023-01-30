using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;

namespace ClaimWap.Controllers
{
    public class ProcessApproveSmTamController : Controller
    {
        //
        // GET: /ProcessApproveSmTam/

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
                string Company = Session["company"].ToString();

                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.Company = Company;
            }

            return View();
            //return Content("Hi there!");
        }
        public JsonResult SaveProcessClaimSmTam(string SP_REQ_NO, string SP_CLM_NO_SUB, string SP_SM_NAME, string SP_SM_APPRV_STATUS, string SP_SM_REMARK, string SP_SM_APPRV_DATE, string SP_USERLOGIN, string SP_CLAIMTYPE, string SP_CLAIMTYPE_SM)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_Detail_Sm_Tam", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SP_REQ_NO", SP_REQ_NO);
                command.Parameters.AddWithValue("@SP_CLM_NO_SUB", SP_CLM_NO_SUB);
                command.Parameters.AddWithValue("@SP_SM_NAME", SP_SM_NAME);
                command.Parameters.AddWithValue("@SP_SM_APPRV_STATUS", SP_SM_APPRV_STATUS);
                command.Parameters.AddWithValue("@SP_SM_REMARK", SP_SM_REMARK);
                command.Parameters.AddWithValue("@SP_SM_APPRV_DATE", SP_SM_APPRV_DATE);
                command.Parameters.AddWithValue("@SP_USERLOGIN", SP_USERLOGIN);
                command.Parameters.AddWithValue("@inWarrantyClmType", SP_CLAIMTYPE);
                command.Parameters.AddWithValue("@SP_CLAIMTYPE_SM", SP_CLAIMTYPE_SM);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                subno = returnValuedoc.Value.ToString();
                command.Dispose();
                if (subno == "Y")
                {
                    message = "true";
                }
                else
                {
                    message = "false";
                }
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
