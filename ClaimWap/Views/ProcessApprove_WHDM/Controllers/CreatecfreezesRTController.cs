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
    public class CreatecfreezesRTController : Controller
    {
        //
        // GET: /CreatecfreezesRT/

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
        public JsonResult GetClimtempSales(string Usrlogin, string company, string cusno, string iemno, string slmcod)
        {
            string message = string.Empty;
            ClimedataRt model = null;
            List<ClimeRttempListDetail> Getdata = new List<ClimeRttempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_SalesReturn_temp_Sales", Connection);
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
                model = new ClimedataRt();
                model.STMP_ID = dr["STMP_ID"].ToString();
                model.STMP_ID_SUB = dr["STMP_ID_SUB"].ToString();
                model.CUSCOD = dr["CUSCOD"].ToString();
                model.STMP_REQUESTBY = dr["STMP_REQUESTBY"].ToString();
                model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.UOM = dr["UOM"].ToString();
                model.STMP_INVNO = dr["STMP_INVNO"].ToString();
                model.STMP_INVDATE = dr["STMP_INVDATE"].ToString();
                model.STMP_QTY = dr["STMP_QTY"].ToString();
                model.STMP_CASE = dr["STMP_CASE"].ToString();
                model.STMP_PERFORM = dr["STMP_PERFORM"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.STMP_RCVSTATUS = dr["STMP_RCVSTATUS"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.InsertDate = dr["InsertDate"].ToString();
                model.COMPANY = dr["COMPANY"].ToString();
                model.CUSNAM = dr["CUSNAM"].ToString();
                model.SLMCOD = dr["SLMCOD"].ToString();
                model.SLMNAM = dr["SLMNAM"].ToString();
                model.Status = dr["Status"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                Getdata.Add(new ClimeRttempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }


    }
}
