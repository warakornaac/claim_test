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
    public class Createsalereturn_salesallController : Controller
    {
        //
        // GET: /Createsalereturn_salesall/

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

                string Cus = string.Empty;
                string Docsub = string.Empty;
                string Docsubcom = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                string Invno = string.Empty;
                string comy = string.Empty;
                Docdisplay = Request.QueryString["no"];

                if (Docdisplay != null)
                {
                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    Docsub = words[1];
                    Docsubcom = words[2];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    byte[] data1 = System.Convert.FromBase64String(Docsub);
                    byte[] data2 = System.Convert.FromBase64String(Docsubcom);
                    Cus = System.Text.ASCIIEncoding.ASCII.GetString(data);
                    Invno = System.Text.ASCIIEncoding.ASCII.GetString(data1);
                    comy = System.Text.ASCIIEncoding.ASCII.GetString(data2);
                }
                ViewBag.Cusno = Cus;
                ViewBag.Invno = Invno;
                ViewBag.Com = comy;


            }
            return View();
        }
        public JsonResult SaveClimtemp(string Inclmno,string Gendoc, string Clno, string Com,string data)
        {
            string message = string.Empty;
            string subno = string.Empty;
            string Docnocm = string.Empty;
            int Runnoline = 1;
            List<ItemConfirm> _ItemList = new JavaScriptSerializer().Deserialize<List<ItemConfirm>>(data);
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                if (Inclmno == "")
                {
                    var command = new SqlCommand("P_Gen_Doc_Control", Connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@inDocNam", Gendoc);
                    command.Parameters.AddWithValue("@inClno", Clno);
                    command.Parameters.AddWithValue("@inCom", Com);
                    SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(returnValuedoc);

                    command.ExecuteNonQuery();
                    command.Dispose();
                    Docnocm = returnValuedoc.Value.ToString();
                    if (Docnocm != "")
                    {
                        for (int i = 0; i < _ItemList.Count; i++)
                        {


                            var commandave = new SqlCommand("P_Save_Salesreturn_temp_allbin", Connection);
                            commandave.CommandType = CommandType.StoredProcedure;
                            commandave.Parameters.AddWithValue("@inCLM_ID", Docnocm);
                            commandave.Parameters.AddWithValue("@inNo_line", Runnoline + i);
                            commandave.Parameters.AddWithValue("@inCOM", _ItemList[i].InCom);
                            commandave.Parameters.AddWithValue("@inCLM_NO", "");
                            commandave.Parameters.AddWithValue("@inCUSCOD", _ItemList[i].CUSCOD);
                            commandave.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].inSTKCOD);
                            commandave.Parameters.AddWithValue("@inSTKDES", _ItemList[i].inSTKDES);
                            commandave.Parameters.AddWithValue("@inCLM_UOM", _ItemList[i].inCLM_UOM);
                            commandave.Parameters.AddWithValue("@inCLM_INVNO ", _ItemList[i].inCLM_INVNO);
                            commandave.Parameters.AddWithValue("@inCLM_INVDATE ", _ItemList[i].inCLM_INVDATE);
                            commandave.Parameters.AddWithValue("@inCLM_QTY", _ItemList[i].inCLM_QTY);
                            commandave.Parameters.AddWithValue("@inCLM_CAUSE  ", _ItemList[i].inCLM_CAUSE);
                            commandave.Parameters.AddWithValue("@inCLM_PERFORM   ", _ItemList[i].inCLM_PERFORM);
                            commandave.Parameters.AddWithValue("@inCLM_RCVSTATUS ", _ItemList[i].inCLM_RCVSTATUS);
                            commandave.Parameters.AddWithValue("@inCLM_RCVBY", _ItemList[i].inCLM_RCVBY);
                            commandave.Parameters.AddWithValue("@inCLM_RCVDATE ", "");
                            commandave.Parameters.AddWithValue("@inINV_QTY", _ItemList[i].inINV_QTY);
                            commandave.Parameters.AddWithValue("@inFoc", _ItemList[i].infoc);
                            commandave.Parameters.AddWithValue("@innet_price", _ItemList[i].innet_price);
                            commandave.Parameters.AddWithValue("@incasebec", "");
                            SqlParameter returnValuestat = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                            returnValuestat.Direction = System.Data.ParameterDirection.Output;
                            commandave.Parameters.Add(returnValuestat);


                            commandave.ExecuteNonQuery();
                            subno = returnValuestat.Value.ToString();
                            commandave.Dispose();
                            message = "true";
                        }
                    }
                }
                else
                {
                    if (Docnocm != "")
                    {
                        for (int i = 0; i < _ItemList.Count; i++)
                        {


                            var commandave = new SqlCommand("P_Save_Salesreturn_temp_allbin", Connection);
                            commandave.CommandType = CommandType.StoredProcedure;
                            commandave.Parameters.AddWithValue("@inCLM_ID", Inclmno);
                            commandave.Parameters.AddWithValue("@inNo_line", Runnoline + i);
                            commandave.Parameters.AddWithValue("@inCOM", _ItemList[i].InCom);
                            commandave.Parameters.AddWithValue("@inCLM_NO", "");
                            commandave.Parameters.AddWithValue("@inCUSCOD", _ItemList[i].CUSCOD);
                            commandave.Parameters.AddWithValue("@inSTKCOD", _ItemList[i].inSTKCOD);
                            commandave.Parameters.AddWithValue("@inSTKDES", _ItemList[i].inSTKDES);
                            commandave.Parameters.AddWithValue("@inCLM_UOM", _ItemList[i].inCLM_UOM);
                            commandave.Parameters.AddWithValue("@inCLM_INVNO ", _ItemList[i].inCLM_INVNO);
                            commandave.Parameters.AddWithValue("@inCLM_INVDATE ", _ItemList[i].inCLM_INVDATE);
                            commandave.Parameters.AddWithValue("@inCLM_QTY", _ItemList[i].inCLM_QTY);
                            commandave.Parameters.AddWithValue("@inCLM_CAUSE  ", _ItemList[i].inCLM_CAUSE);
                            commandave.Parameters.AddWithValue("@inCLM_PERFORM   ", _ItemList[i].inCLM_PERFORM);
                            commandave.Parameters.AddWithValue("@inCLM_RCVSTATUS ", _ItemList[i].inCLM_RCVSTATUS);
                            commandave.Parameters.AddWithValue("@inCLM_RCVBY", _ItemList[i].inCLM_RCVBY);
                            commandave.Parameters.AddWithValue("@inCLM_RCVDATE ", "");
                            commandave.Parameters.AddWithValue("@inINV_QTY", _ItemList[i].inINV_QTY);
                            commandave.Parameters.AddWithValue("@inFoc", _ItemList[i].infoc);
                            commandave.Parameters.AddWithValue("@innet_price", _ItemList[i].innet_price);
                            commandave.Parameters.AddWithValue("@incasebec", "");
                            SqlParameter returnValuestat = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                            returnValuestat.Direction = System.Data.ParameterDirection.Output;
                            commandave.Parameters.Add(returnValuestat);


                            commandave.ExecuteNonQuery();
                            subno = returnValuestat.Value.ToString();
                            commandave.Dispose();
                            message = "true";
                        }
                    }
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveClim(string imgsignatureman, string imgsignature, string inqtybox, string inCLM_ID, string inCLM_RCVBY, string inCusShipping, string incodeShipping, string inremakeclaim)
        {
            string message = string.Empty;
            string cmid = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_SalesReturn", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inREQ_BY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCusShipping", inCusShipping);
                command.Parameters.AddWithValue("@incodeShipping", incodeShipping);
                command.Parameters.AddWithValue("@inremakeclaim", inremakeclaim);
                command.Parameters.AddWithValue("@inqtybox", inqtybox);
                command.Parameters.AddWithValue("@inimgsignature", imgsignature);
                command.Parameters.AddWithValue("@inimgsignatureman", imgsignatureman);
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
    }
    public class ItemConfirm
    {
       
        public string CUSCOD { get; set; }
        public string inSTKCOD { get; set; }
        public string inSTKDES { get; set; }
        public string inCLM_UOM { get; set; }
        public string inCLM_INVNO { get; set; }
        public string inINV_QTY { get; set; }
        public string inCLM_INVDATE { get; set; }
        public string inCLM_QTY { get; set; }
        public string inCLM_CAUSE { get; set; }
        public string inCLM_PERFORM { get; set; }
        public string inCLM_RCVSTATUS { get; set; }
        public string inCLM_RCVBY { get; set; }
        public string inCLM_RCVDATE { get; set; }
        public string InCom { get; set; }
        public string innet_price { get; set; }
        public string incasebec { get; set; }
        public string infoc { get; set; }
    }
}
