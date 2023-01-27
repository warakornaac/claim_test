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
    public class AdminControController : Controller
    {
        //
        // GET: /AdminContro/

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
                //}

            }
            return View();
        }
        public JsonResult GetdatauserSp(string user, string status)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<UsrGrp_specialDetail> Getdata = new List<UsrGrp_specialDetail>();
            UsrGrp_special model = null;
            var command = new SqlCommand("P_SearhLogOn", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Usr", user);
            command.Parameters.AddWithValue("@Status", status);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new UsrGrp_special();
                model.UsrID = dr["UsrID"].ToString();
                model.Department = dr["Department"].ToString();
                model.Email = dr["Email"].ToString();
                model.UserCode = dr["UserCode"].ToString();
                model.Password = dr["Password"].ToString();
                model.LastLogOn = dr["LastLogOn"].ToString();
                model.pwdLastSet = dr["pwdLastSet"].ToString();
                model.Status = dr["Status"].ToString();
                Getdata.Add(new UsrGrp_specialDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult updatelogon(string user, string status)
        {
            string message = string.Empty;
           
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_updatelogon", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUser ", user);
                command.Parameters.AddWithValue("@instatus", status);
               
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                
                message = returnValuedoc.Value.ToString();
                command.Dispose();
               
                
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
    }
}
