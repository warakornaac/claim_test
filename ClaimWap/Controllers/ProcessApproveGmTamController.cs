using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Controllers;
using ClaimWap.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class ProcessApproveGmTamController : Controller
    {
        //
        // GET: /ProcessApproveGmTam/

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
        }

        public JsonResult SaveProcessClaimGmTam(string SP_REQ_NO, string SP_CLM_NO_SUB, string SP_GM_NAME, string SP_GM_PROCESS_STATUS, string SP_GM_REMARK, string SP_GM_APPRV_DATE, string SP_USERLOGIN, string SP_CLAIMTYPE)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_Detail_Gm_Tam", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SP_REQ_NO", SP_REQ_NO);
                command.Parameters.AddWithValue("@SP_CLM_NO_SUB", SP_CLM_NO_SUB);
                command.Parameters.AddWithValue("@SP_GM_NAME", SP_GM_NAME);
                command.Parameters.AddWithValue("@SP_GM_PROCESS_STATUS", SP_GM_PROCESS_STATUS);
                command.Parameters.AddWithValue("@SP_GM_REMARK", SP_GM_REMARK);
                command.Parameters.AddWithValue("@SP_GM_APPRV_DATE", SP_GM_APPRV_DATE);
                command.Parameters.AddWithValue("@SP_USERLOGIN", SP_USERLOGIN);
                command.Parameters.AddWithValue("@inWarrantyClmType", SP_CLAIMTYPE);
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
