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
using System.Web.Script.Serialization;

namespace ClaimWap.Controllers
{
    public class Createclaim_WhController : Controller
    {
        //
        // GET: /Createclaim_Wh/

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
                string Department = Session["Department"].ToString();
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
                ViewBag.Department = Department;
                string Doc = string.Empty;
                string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;
                string SubDocwords = string.Empty;
                Docdisplay = Request.QueryString["ClmNUM"];

                if (Docdisplay != null)
                {
                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

                }
                ViewBag.Srtno = Doc;



            }
            return View();
        }
        public JsonResult Gendocno(string Gendoc, string Clno, string Com)
        {
            string Docnocm = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Gen_Doc_Control", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inDocNam", Gendoc);
            command.Parameters.AddWithValue("@inClno", Clno);
            command.Parameters.AddWithValue("@inCom", Com);
            SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
            returnValuedoc.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(returnValuedoc);
            Connection.Open();
            command.ExecuteNonQuery();
            Docnocm = returnValuedoc.Value.ToString();


            command.Dispose();
            Connection.Close();

            return Json(new { Docnocm }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveClimWHtemp(string inCLM_ID,string inCLM_ID_SUB, string inSTKCOD, string inCLM_CAUSE, string inCLM_QTY, string inCLM_LOCATION, string inCLM_PERFORM,string inCLM_PERFORM_DES, string inCLM_RCVSTATUS,string inCLM_RCVBY, string inCLM_RCVBYDEP, string inCLM_RCVDATE, string InCom)
        {

            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_ClaimWH_temp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inCLM_ID_SUB", inCLM_ID_SUB);
                command.Parameters.AddWithValue("@inSTKCOD", inSTKCOD);
                command.Parameters.AddWithValue("@inCLM_CAUSE", inCLM_CAUSE);
                command.Parameters.AddWithValue("@inCLM_QTY", inCLM_QTY);
                command.Parameters.AddWithValue("@inCLM_LOCATION", inCLM_LOCATION);
                command.Parameters.AddWithValue("@inCLM_PERFORM ", inCLM_PERFORM);
                command.Parameters.AddWithValue("@inCLM_PERFORM_DES ", inCLM_PERFORM_DES);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS", inCLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY ", inCLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_RCVBYDEP", inCLM_RCVBYDEP);
                command.Parameters.AddWithValue("@inCLM_RCVDATE", inCLM_RCVDATE);
                command.Parameters.AddWithValue("@InCom", InCom); 	
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
        public JsonResult SaveClimWH(string imgsignature, string inCLM_ID, string inCLM_RCVBY, string indepreq)
        {
            string message = string.Empty;
            string cmid = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Save_ClaimWH", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inCLM_ID", inCLM_ID);
                command.Parameters.AddWithValue("@inREQ_BY", inCLM_RCVBY);
                command.Parameters.AddWithValue("@indepreq", indepreq);
                command.Parameters.AddWithValue("@inimgsignature", imgsignature);
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
        public JsonResult GetClimWHtemp(string inCLM_ID)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimetempListDetail> Getdata = new List<ClimetempListDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_ClaimWH_temp", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DOC", inCLM_ID);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new Climedata();
                model.CLM_ID = dr["CLM_ID"].ToString();
                model.CLM_NO_SUB = dr["CLM_NO_SUB"].ToString();
               // model.CLM_RCVBY = dr["CLM_RCVBY"].ToString();
               // model.CLM_RCVDATE = dr["CLM_RCVDATE"].ToString();
                model.STKCOD = dr["STKCOD"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.CLM_UOM = dr["CLM_UOM"].ToString();
                model.CLM_QTY = dr["CLM_QTY"].ToString();
                model.CLM_CAUSE = dr["CLM_CAUSE"].ToString();
                model.CLM_PERFORM = dr["CLM_PERFORM"].ToString();
                model.CLM_PERFORM_DES = dr["CLM_PERFORM_DES"].ToString();
                model.CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString();
                model.PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString();
                model.RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString();
                model.CLM_COMPANY = dr["CLM_COMPANY"].ToString();
                //model.CUSNAM = dr["CUSNAM"].ToString();
                //model.SLMCOD = dr["SLMCOD"].ToString();
                //model.SLMNAM = dr["SLMNAM"].ToString();
                model.Status = dr["Status"].ToString();
                //model.CLM_Machine = dr["CLM_Machine"].ToString();
                //model.CLM_Model = dr["CLM_Model"].ToString();
                //model.CLM_ModelYear = dr["CLM_ModelYear"].ToString();
                //model.CLM_EngineCode = dr["CLM_EngineCode"].ToString();
                //model.CLM_ChassisNo = dr["CLM_ChassisNo"].ToString();
                //model.CLM_InjecPump = dr["CLM_InjecPump"].ToString();
                //model.CLM_TypeProduct = dr["CLM_TypeProduct"].ToString();
                //model.CLM_WarrantyNo = dr["CLM_WarrantyNo"].ToString();
                //model.CLM_Milage = dr["CLM_Milage"].ToString();
                //model.CLM_DateDamage = dr["CLM_DateDamage"].ToString();
                //model.CLM_BatchCode = dr["CLM_BatchCode"].ToString();
                //model.CLM_Installdate = dr["CLM_Installdate"].ToString();
                //model.CLM_Contact = dr["CLM_Contact"].ToString();
                //model.CLM_ContactTel = dr["CLM_ContactTel"].ToString();
                model.STKDES = dr["STKDES"].ToString();
                model.STKGRP = dr["STKGRP"].ToString();
                model.GRPNAM = dr["GRPNAM"].ToString();
                model.PROD = dr["PROD"].ToString();
                model.PRODNAM = dr["PRODNAM"].ToString();
                model.DEP = dr["DEP"].ToString();
                model.DEPNAM = dr["DEPNAM"].ToString();
                model.CLM_Owner = dr["CLM_Owner"].ToString();
                model.CLM_Location = dr["CLM_Location"].ToString();
                model.InsertBy = dr["InsertBy"].ToString();
                model.InsertDate = dr["InsertDate"].ToString(); 
                
                Getdata.Add(new ClimetempListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult UploadFiles(FormCollection formCollection)
        {
            //int pussend = 0;
            //int pus = 0;
            string uname = string.Empty;
            string Pathimg = string.Empty;
            string path = Server.MapPath(@"~\ImgUploadWH\");
            HttpFileCollectionBase files = Request.Files;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            for (int i = 0; i < files.Count; i++)
            {


                string name = formCollection["uploadername"];
                string inCim_NoSub = formCollection["inCim_NoSub"];
                string No = formCollection["No"];


                //Pathimg = name + "-" + pussend + ".png";
                var command = new SqlCommand("P_Save_PathImageWH", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inim_name", "");
                command.Parameters.AddWithValue("@inCim_NoSub", inCim_NoSub);
                command.Parameters.AddWithValue("@inCim_No", No);
                command.Parameters.AddWithValue("@inIm_No", "");

                SqlParameter returnValuedoc = new SqlParameter("@outimagename", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                uname = returnValuedoc.Value.ToString();
                command.Dispose();
                //pus = i;
                //pussend = (pus + 1);
                HttpPostedFileBase file = files[i];
                string fileName = file.FileName;
                // file.SaveAs(path + file.FileName);
                file.SaveAs(Server.MapPath(@"~\ImgUpload\" + uname));


            }
            Connection.Close();
            return Json(files.Count + " Files Uploaded!");
        }
        [HttpPost]
        public JsonResult UploadVideo(HttpPostedFileBase filex, string aj_CLM_NO_SUB, string aj_CLM_NO)
        {
            string fileName = string.Empty;
            string message = string.Empty;
            int Size = 0;
            // if (fileupload != null)
            // {
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    fileName = Path.GetFileName(file.FileName);
                    int fileSize = file.ContentLength;
                    Size = fileSize / 1000;
                    file.SaveAs(Server.MapPath("~/VideoFileUploadWH/" + fileName + ".mp4"));

                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { fileName, message, Size }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Savefilevideo(HttpPostedFileBase filex, string aj_CLM_NO_SUB, string aj_CLM_NO, string im_name, string Size, string Im_No)
        {
            string fileName = string.Empty;
            string message = string.Empty;
            // if (fileupload != null)
            // {
            try
            {


                var CS = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                // string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAddNewVideoFile_WH", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@inCim_No", aj_CLM_NO);
                    cmd.Parameters.AddWithValue("@inCim_NoSub", aj_CLM_NO_SUB);
                    cmd.Parameters.AddWithValue("@Name", im_name);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("@inImg_ID", Im_No);
                    cmd.Parameters.AddWithValue("FilePath", "~/VideoFileUploadWH/" + im_name + ".mp4");
                    cmd.ExecuteNonQuery();


                    cmd.Dispose();
                    con.Close();

                }
                message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { fileName, message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFileVideo(string comid, string clmnoup, string clmidimg, string absPath, string Im_No)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_DelvideoWH", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
                command.Parameters.AddWithValue("@inImg_ID", Im_No);
                command.ExecuteNonQuery();
                command.Dispose();

                message = "true";
                var root = @"..\VideoFileUploadWH\";

                if (message == "true")
                {
                    string tempFilePathWithFileName = Path.GetTempFileName();
                    string filePath = Path.Combine(root, absPath);
                    string filePath1 = Server.MapPath(filePath);
                    if (System.IO.File.Exists(filePath1))
                    {


                        System.IO.File.Delete(filePath1);


                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Deleteimage(string comid, string clmnoup, string clmidimg, string absPath)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_DelimageWH", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
                command.Parameters.AddWithValue("@CLMIMAGENO", clmidimg);
                command.ExecuteNonQuery();
                command.Dispose();

                message = "true";
                var root = @"..\ImgUploadWH\";

                if (message == "true")
                {
                    string tempFilePathWithFileName = Path.GetTempFileName();
                    string filePath = Path.Combine(root, absPath);
                    string filePath1 = Server.MapPath(filePath);
                    if (System.IO.File.Exists(filePath1))
                    {


                        System.IO.File.Delete(filePath1);


                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Connection.Close();
            return Json(new { message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteClaimtemp(string comid, string clmnoup)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_DelClaimWH_temp", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DOC", comid);
                command.Parameters.AddWithValue("@SUBDOC", clmnoup);
                command.ExecuteNonQuery();
                command.Dispose();

                message = "true";
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
