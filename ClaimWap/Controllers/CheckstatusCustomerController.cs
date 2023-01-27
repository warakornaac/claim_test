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
using System.Net;
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class CheckstatusCustomerController : Controller
    {
        //
        // GET: /CheckstatusCustomer/

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
               
                  
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                    ViewBag.UsrCode = UsrCode;
              

            }
            return View();
        }

    }
}
