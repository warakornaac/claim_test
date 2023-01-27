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
    public class CreatecfreezesController : Controller
    {
        //
        // GET: /Createcfreezes/

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
        public JsonResult GetClimtempSales(string Usrlogin, string company, string cusno, string iemno, string slmcod)
        {   
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Claim_temp_Sales", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Userlogin", Usrlogin);
            command.Parameters.AddWithValue("@Com", company);
            command.Parameters.AddWithValue("@Cus", cusno);
            command.Parameters.AddWithValue("@PN", iemno);
            command.Parameters.AddWithValue("@Sm", slmcod);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_INVNO = dr["CLM_INVNO"].ToString();
                model.CLM_INVDATE = dr["CLM_INVDATE"].ToString();
                model.CLM_USEDAY = dr["CLM_USEDAY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.Status = dr["Status"].ToString();
                model.CLM_Machine = dr["CLM_Machine"].ToString();
                model.CLM_Model = dr["CLM_Model"].ToString();
                model.CLM_ModelYear = dr["CLM_ModelYear"].ToString();
                model.CLM_EngineCode = dr["CLM_EngineCode"].ToString();
                model.CLM_ChassisNo = dr["CLM_ChassisNo"].ToString();
                model.CLM_InjecPump = dr["CLM_InjecPump"].ToString();
                model.CLM_TypeProduct = dr["CLM_TypeProduct"].ToString();
                model.CLM_WarrantyNo = dr["CLM_WarrantyNo"].ToString();
                model.CLM_Milage = dr["CLM_Milage"].ToString();
                model.CLM_DateDamage = dr["CLM_DateDamage"].ToString();
                model.CLM_BatchCode = dr["CLM_BatchCode"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

    }
}
