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
    public class ProcessApproveWHController : Controller
    {
        //
        // GET: /ProcessApproveWH/

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

        public JsonResult SaveProcessClaimDetailPM(string aj_PM_AFTER_APP, string aj_PM_Qty, string clamtyp, string aj_userlogin, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_PM_NAME, string aj_PM_APPRV_STATUS, string aj_PM_REMARK, string aj_PM_APPRV_DATE, string aj_PM_Replacement, string aj_anlysstatustecpm, string aj_anlysafterprotecpm, string aj_scrapdatetecpm, string aj_PM_Option1, string aj_PM_Optiontext1, string aj_PM_Option2, string aj_PM_Optiontext2, string aj_PM_Option3, string aj_PM_Optiontext3, string aj_PM_Option4, string aj_PM_Optiontext4, string aj_PM_Option5, string aj_PM_Optiontext5)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {

                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_DetailPM", Connection);
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
                command.Parameters.AddWithValue("@inPM_QTY", aj_PM_Qty);
                command.Parameters.AddWithValue("@inPM_AFTER_APP", aj_PM_AFTER_APP);

                command.Parameters.AddWithValue("@inPM_Option1", aj_PM_Option1);
                command.Parameters.AddWithValue("@inPM_Optiontext1", aj_PM_Optiontext1);
                command.Parameters.AddWithValue("@inPM_Option2", aj_PM_Option2);
                command.Parameters.AddWithValue("@inPM_Optiontext2", aj_PM_Optiontext2);
                command.Parameters.AddWithValue("@inPM_Option3", aj_PM_Option3);
                command.Parameters.AddWithValue("@inPM_Optiontext3", aj_PM_Optiontext3);
                command.Parameters.AddWithValue("@inPM_Option4", aj_PM_Option4);
                command.Parameters.AddWithValue("@inPM_Optiontext4", aj_PM_Optiontext4);
                command.Parameters.AddWithValue("@inPM_Option5", aj_PM_Option5);
                command.Parameters.AddWithValue("@inPM_Optiontext5", aj_PM_Optiontext5);

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
