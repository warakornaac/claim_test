using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimWap.Models;
using System.Data;
namespace ClaimWap.Controllers
{
    public class InvoiceHistory_ScController : Controller
    {
        //
        // GET: /InvoiceHistory_Sc/
     
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
                //}

            }
            return View();
        }


        public JsonResult GetdataInvoiceHistory(string _foc, string _cusno, string _invoiceno, string _itemno, string _com, string _psdate, string _statdate, string _enddate, string _slmcod,string _Stkgrp)
        {
            string PSTDAT = string.Empty;
            string formattedLInvdate = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<InvoiceStatusListDetailGetdata> Getdata = new List<InvoiceStatusListDetailGetdata>();
            InvoiceStatus model = null;
            var command = new SqlCommand("P_InvoiceHistory", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CUS", _cusno);
            command.Parameters.AddWithValue("@PN", _itemno);
            command.Parameters.AddWithValue("@DOC", _invoiceno);
            command.Parameters.AddWithValue("@GRP", _Stkgrp);
            command.Parameters.AddWithValue("@PSTDATEStat", _statdate);
            command.Parameters.AddWithValue("@PSTDATEEnd", _enddate);
            command.Parameters.AddWithValue("@Com", _com);
            command.Parameters.AddWithValue("@SLM", _slmcod);
            command.Parameters.AddWithValue("@FOC", _foc);
            //command.Parameters.AddWithValue("@PN", "JTC215");
            //command.Parameters.AddWithValue("@DOC", "");
            //command.Parameters.AddWithValue("@GRP", "");
            //command.Parameters.AddWithValue("@PSTDATE", ""); 
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new InvoiceStatus();

                model.COMP = dr["COMP"].ToString();
                model.DOCNUM = dr["DOCNUM"].ToString();
                //model.PSTDAT = dr["PSTDAT"].ToString();
                PSTDAT = dr["PSTDAT"].ToString();
                if (PSTDAT != "")
                {
                    DateTime dateLastInvdate = Convert.ToDateTime(PSTDAT);
                    formattedLInvdate = dateLastInvdate.ToString("dd/MM/yyyy");
                    model.PSTDAT = formattedLInvdate;
                }
                else
                {
                    model.PSTDAT = "-";

                }
                model.PEOPLE = dr["PEOPLE"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SEC = dr["SEC"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.Qty = dr["Qty"].ToString();
                model.Price = dr["@Price"].ToString();
                model.Amt = dr["Amt"].ToString();
                model.DiscountPer = dr["Discount%"].ToString();
                model.DiscountAmt = dr["DiscountAmt"].ToString();
                model.Net_Price = dr["Net_Price"].ToString();
                model.NetAmt = dr["NetAmt"].ToString();
                model.FOC = dr["FOC"].ToString();
                model.AsOf = dr["AsOf"].ToString();
                model.Uom = dr["UOM"].ToString();
                model.Prod = dr["PROD"].ToString();
                model.ProdName = dr["PRODNAM"].ToString();
                model.GrpName = dr["GRPNAM"].ToString();
                model.QTY_Remaining = dr["QTY_Remaining"].ToString();
                //model.LastClaimNo = dr["LastClaimNo"].ToString();
                //model.LastClaimdate = dr["LastClaimdate"].ToString();
                //model.ClaimStatus = dr["ClaimStatus"].ToString();
                model.LastClaimNo = "";
                model.LastClaimdate = "";
                model.ClaimStatus = "";
                Getdata.Add(new InvoiceStatusListDetailGetdata { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
    }
}
