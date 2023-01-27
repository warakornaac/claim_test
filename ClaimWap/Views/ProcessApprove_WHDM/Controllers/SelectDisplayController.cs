using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimWap.Controllers
{
    public class SelectDisplayController : Controller
    {
        //
        // GET: /SelectDisplay/

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
                if (UserType == "3") //PM//
                {
                    if (User != "paxte")
                    {
                        return RedirectToAction("Index", "ProcessApprove");
                    }
                    else
                    {
                        return RedirectToAction("Index", "ProcessApprove_WHDM_MD");
                    }
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
                else if (UserType == "8" || UserType == "9") //PM//
                {
                    return RedirectToAction("Index", "Disposal");
                    ViewBag.UserId = User;
                    ViewBag.UserType = UserType;
                }
                //else if (UserType == "10") //WH//
                //{
                //    return RedirectToAction("Index", "CheckstatusRt");
                //    ViewBag.UserId = User;
                //    ViewBag.UserType = UserType;
                //}
                // else
                // {
                // return RedirectToAction("Index", "Checkstatus_Sc");
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                //}

            }
            return View();
        }

    }
}
