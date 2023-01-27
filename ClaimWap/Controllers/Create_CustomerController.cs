using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.IO;

namespace ClaimWap.Controllers
{
    public class Create_CustomerController : Controller
    {
        //
        // GET: /Create_Customer/

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
                string UsrCode = Session["UsrCode"].ToString();
                string Contact = Session["Contact"].ToString();
                string ContactPhone = Session["ContactPhone"].ToString();
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.UsrCode = UsrCode;
                ViewBag.Contact =Contact;
                ViewBag.ContactPhone = ContactPhone;
                string Doc = string.Empty;
                // string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                // string SubDocwords = string.Empty;
                Docdisplay = Request.QueryString["ClmNUM"];

                if (Docdisplay != null)
                {
                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

                  
                }
                ViewBag.Claimno = Doc;



            }
            return View();
        }
        public JsonResult SaveClim(string inqtybox, string inCLM_ID, string inCLM_RCVBY, string inCusShipping, string incodeShipping, string inremakeclaim)
        {
            string message = string.Empty;
            string cmid = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_Claim_Customer", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inREQ_BY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCusShipping", inCusShipping);
                command.Parameters.AddWithValue("@incodeShipping", incodeShipping);
                command.Parameters.AddWithValue("@inremakeclaim", inremakeclaim);
                command.Parameters.AddWithValue("@inqtybox", inqtybox);
                command.Parameters.AddWithValue("@inimgsignature","");
                SqlParameter returnValuedoc = new SqlParameter("@outGenDoc", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                cmid = returnValuedoc.Value.ToString();
                command.Dispose();
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, cmid }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveClimtemp(string incontactcus, string inclaimcontactcus, string installdatecus, string inreminqtynew, string infoc, string inINV_QTY, string inCLM_ID, string CLM_NO, string InCom, string CUSCOD, string inSTKCOD, string inSTKDES, string inCLM_UOM, string inCLM_INVNO, string inCLM_INVDATE, string inCLM_QTY, string inCLM_USEDAY, string inCLM_CAUSE, string inCLM_PERFORM, string inCLM_RCVSTATUS, string inCLM_RCVBY, string inCLM_RCVDATE, string vehicle, string modeltype, string modelyear, string enginecode, string chassisno, string pump, string typeofProduct, string warrantycardno, string milage, string dateofdamage, string BatchCode)
        {
            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_Claim_temp_Customer", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inCOM", InCom);
                command.Parameters.AddWithValue("@inCLM_NO", CLM_NO);
                command.Parameters.AddWithValue("@inCUSCOD", CUSCOD);
                command.Parameters.AddWithValue("@inSTKCOD", inSTKCOD);
                command.Parameters.AddWithValue("@inSTKDES", inSTKDES);
                command.Parameters.AddWithValue("@inCLM_UOM", inCLM_UOM);
                command.Parameters.AddWithValue("@inCLM_INVNO", inCLM_INVNO);
                command.Parameters.AddWithValue("@inINV_QTY", inINV_QTY);
                command.Parameters.AddWithValue("@inCLM_INVDATE", inCLM_INVDATE);
                command.Parameters.AddWithValue("@inCLM_QTY", inCLM_QTY);
                command.Parameters.AddWithValue("@inCLM_USEDAY", inCLM_USEDAY);
                command.Parameters.AddWithValue("@inCLM_CAUSE", inCLM_CAUSE);
                command.Parameters.AddWithValue("@inCLM_PERFORM", inCLM_PERFORM);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS", inCLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_RCVDATE", inCLM_RCVDATE);
                command.Parameters.AddWithValue("@inCLM_Machine", vehicle);
                command.Parameters.AddWithValue("@inCLM_Model", modeltype);
                command.Parameters.AddWithValue("@inCLM_ModelYear", modelyear);
                command.Parameters.AddWithValue("@inCLM_EngineCode", enginecode);
                command.Parameters.AddWithValue("@inCLM_ChassisNo", chassisno);
                command.Parameters.AddWithValue("@inCLM_InjecPump", pump);
                command.Parameters.AddWithValue("@inCLM_TypeProduct", typeofProduct);
                command.Parameters.AddWithValue("@inCLM_WarrantyNo", warrantycardno);
                command.Parameters.AddWithValue("@inCLM_Milage", milage);
                command.Parameters.AddWithValue("@inCLM_DateDamage", dateofdamage);
                command.Parameters.AddWithValue("@inCLM_BatchCode", BatchCode);
                command.Parameters.AddWithValue("@inFoc", "");
                command.Parameters.AddWithValue("@ininreminqtynew", inreminqtynew);
                command.Parameters.AddWithValue("@installdatecus", installdatecus);
                command.Parameters.AddWithValue("@inclaimcontactcus", inclaimcontactcus);
                command.Parameters.AddWithValue("@incontactcus", incontactcus);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                subno = returnValuedoc.Value.ToString();
                command.Dispose();
                message = "true";

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
