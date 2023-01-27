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
    public class ProcessApprovePMController : Controller
    {
        //
        // GET: /Process_Sc/
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

        public JsonResult SaveProcessClaimDetailPM(string clamtyp, string aj_userlogin, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_PM_NAME, string aj_PM_APPRV_STATUS, string aj_PM_REMARK, string aj_PM_APPRV_DATE, string aj_PM_Replacement, string aj_anlysstatustecpm, string aj_anlysafterprotecpm, string aj_scrapdatetecpm)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {

                Connection.Open();
                var command = new SqlCommand("P_Process_Claim_DetailPM", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inPM_NAME", aj_PM_NAME);
                command.Parameters.AddWithValue("@inPM_APPRV_STATUS", aj_PM_APPRV_STATUS);
                command.Parameters.AddWithValue("@inPM_REMARK", aj_PM_REMARK);
                command.Parameters.AddWithValue("@inPM_APPRV_DATE", aj_PM_APPRV_DATE);
                command.Parameters.AddWithValue("@inPM_Replacement", aj_PM_Replacement);
                command.Parameters.AddWithValue("@inuserlogin", aj_userlogin);
                command.Parameters.AddWithValue("@inPM_TECANLYS_STATUS", aj_anlysstatustecpm);
                command.Parameters.AddWithValue("@inPM_TECPROCESS_STATUS", aj_anlysafterprotecpm);
                command.Parameters.AddWithValue("@inPM_TECANLYS_AFTERPROCESS", aj_scrapdatetecpm);
                command.Parameters.AddWithValue("@inWarrantyClmType", clamtyp);
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