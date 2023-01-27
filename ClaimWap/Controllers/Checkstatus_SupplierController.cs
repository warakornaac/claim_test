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
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class Checkstatus_SupplierController : Controller
    {
        //
        // GET: /Checkstatus_Supplier/

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

                ViewBag.UserId = User;
                ViewBag.UserType = UserType;


            }
            return View();
        }


        public JsonResult CheckStatus_Supplier(string suppli,string intec,string inDOC, string inStatus, string inComdisplay, string inCuscod, string inDOCSUB, string inStkcod)
        {
            List<ListGetdataReceive_Supplier> Getdata = new List<ListGetdataReceive_Supplier>();
            Receive_SupplierGetdata model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_CheckStatus_Supplier", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inStatus", inStatus);
            command.Parameters.AddWithValue("@inComdisplay", inComdisplay);
            command.Parameters.AddWithValue("@inCuscod", inCuscod);
            command.Parameters.AddWithValue("@inDOCSUB", inDOCSUB);
            command.Parameters.AddWithValue("@inStkcod", inStkcod);
            command.Parameters.AddWithValue("@intec", intec);
            command.Parameters.AddWithValue("@insuppli", suppli);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Receive_SupplierGetdata();
                model.CLM_NO_Supplier = dr["CLM_NO_Supplier"].ToString();
                model.REQ_TotalQty = dr["REQ_TotalQty"].ToString();
                model.REQ_TotalAMT = dr["REQ_TotalAMT"].ToString();
                model.REQ_TotalItem = dr["REQ_TotalItem"].ToString();
                model.REQ_BY = dr["REQ_BY"].ToString();
                model.REQ_DATE = dr["REQ_DATE"].ToString();
                model.Remake = dr["Remake"].ToString();
                model.CLM_SUB_No = dr["CLM_NO_SUB"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.Send_Qty = dr["Send_Qty"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.Unit_Price = dr["Unit_Price"].ToString();
                model.AMT = dr["AMT"].ToString();
                model.Supplier = dr["Supplier"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.UOM = dr["UOM"].ToString();
                model.VENDORNAME = dr["VENDORNAME"].ToString();
                model.INV_Sup_No = dr["INV_Sup_No"].ToString();
                model.INV_Qty_Sup = dr["INV_Qty_Sup"].ToString();
                model.CN_Sup_No = dr["CN_Sup_No"].ToString();
                model.CN_Qty_Sup = dr["CN_Qty_Sup"].ToString();
                model.CN_Amt = dr["CN_Amt"].ToString();
                model.INV_Qty_Sup_Cancel = dr["INV_Qty_Sup_Cancel"].ToString();
                model.CN_Qty_Sup_Cancel = dr["CN_Qty_Sup_Cancel"].ToString();
                model.Status = dr["Status"].ToString();
                model.CS_No = dr["CS_No"].ToString();
                model.Log_No = dr["Log_No"].ToString();
                model.TECH2_NAME = dr["TECH2_NAME"].ToString();
                model.InvoiceSupplier = dr["InvoiceSupplier"].ToString();
                model.InvoiceDateSupplier = dr["InvoiceDateSupplier"].ToString();
                model.Cur_Sup = dr["Cur_Sup"].ToString();
                Getdata.Add(new ListGetdataReceive_Supplier { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);
        }
    }
}
