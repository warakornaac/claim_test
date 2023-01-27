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
    public class ProcessApproveSmSupController : Controller
    {
        //
        // GET: /ProcessApproveSmSup/

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
        public JsonResult SaveProcessSalesreturnDetailSupAll(string data)
        {

            string message = string.Empty;
            string subno = string.Empty;
            List<ItemConfirmsup> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemConfirmsup>>(data);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                for (int i = 0; i < _ItemList.Count; i++)
                {
                    var command = new SqlCommand("P_Process_SalesReturn_SmSup", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inREQ_NO",  _ItemList[i].indoc);
                    command.Parameters.AddWithValue("@inCLM_NO_SUB", _ItemList[i].indocsup);
                    command.Parameters.AddWithValue("@inSMSUP_NAME", _ItemList[i].userlogin);
                    command.Parameters.AddWithValue("@inSMSUP_APPRV_STATUS", _ItemList[i].supstus);
                    command.Parameters.AddWithValue("@inSMSUP_REMARK ", _ItemList[i].remake);
                    command.Parameters.AddWithValue("@inSMSUP_APPRV_DATE ","");
                    command.Parameters.AddWithValue("@inuserlogin", _ItemList[i].userlogin);
                    SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValuedoc);
                    command.ExecuteNonQuery();
                    subno = returnValuedoc.Value.ToString();
                    command.Dispose();
                    
                }
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
        public JsonResult SaveProcessSalesreturnDetailSup(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_SUP_APPRV_STATUS, string aj_SUP_REMARK, string aj_SUP_APPRV_DATE, string aj_userlogin)
        {
              
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_SalesReturn_SmSup", Connection);
                command.CommandType = CommandType.StoredProcedure;               
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inSMSUP_NAME", aj_userlogin);
                command.Parameters.AddWithValue("@inSMSUP_APPRV_STATUS", aj_SUP_APPRV_STATUS);
                command.Parameters.AddWithValue("@inSMSUP_REMARK ", aj_SUP_REMARK);
                command.Parameters.AddWithValue("@inSMSUP_APPRV_DATE ", aj_SUP_APPRV_DATE);
                command.Parameters.AddWithValue("@inuserlogin", aj_userlogin);
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
    public class ItemConfirmsup
    {
            public string indoc{ get; set; }
            public string indocsup { get; set; }
            public string insertdate{ get; set; }
            public string supstus{ get; set; }
            public string remake{ get; set; }
            public string userlogin { get; set; }
       
    }
}
