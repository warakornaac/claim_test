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
    public class Process_WhController : Controller
    {
        //
        // GET: /Process_Wh/

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

                    // SubDocwords = words[1];
                    // byte[] datasub = System.Convert.FromBase64String(SubDocwords);
                    // Docsub = System.Text.ASCIIEncoding.ASCII.GetString(datasub);


                    // Doc = Docdisplay;
                    // Docsub = Docdisplay;
                }
                ViewBag.Claimno = Doc;
                // ViewBag.Claimsubno = Docsub;
            }
            return View();
        }
        public JsonResult SaveProcessClaimWHDetail(string aj_CLM_BATCODE, string aj_tecrevqty, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_TECH1_NAME, string aj_TECH1_ANLYS_DATE, string aj_CLM_PERFORM, string aj_TECH1_ANLYS_RESULT, string aj_CLM_SHELF_LOCATION, string aj_Technician)
        {
            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_Tec1", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inCLM_SHELF_LOCATION", aj_CLM_SHELF_LOCATION);
                command.Parameters.AddWithValue("@inTECH1_NAME", aj_TECH1_NAME);
                command.Parameters.AddWithValue("@inTECH1_ANLYS_DATE", aj_TECH1_ANLYS_DATE);
                command.Parameters.AddWithValue("@inCLM_PERFORM", aj_CLM_PERFORM);
                //command.Parameters.AddWithValue("@inTECH1_ANLYS_STATUS", aj_TECH1_ANLYS_STATUS);
                //command.Parameters.AddWithValue("@inANLYS_AFTERPROCESS", aj_ANLYS_AFTERPROCESS);
                command.Parameters.AddWithValue("@inCLM_BATCODE", aj_CLM_BATCODE);
                command.Parameters.AddWithValue("@inTECH1_ANLYS_RESULT", aj_TECH1_ANLYS_RESULT);
                command.Parameters.AddWithValue("@inTECH1_QTY", aj_tecrevqty);             
                command.Parameters.AddWithValue("@inTechnician", aj_Technician);               
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult SaveProcessClaimDetailTec2(string aj_CLM_TECH2_WASTE_QTY,string aj_CLM_TECH2_GOOD_QTY,string aj_TECH2_PROCESS_ANLYS, string aj_ANLYS_AFTERPROCESS, string aj_SCRAP_DATE, string aj_TTECH2_QTY, string clamtyp, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_TECH2_NAME, string aj_TECH2_APPRV_STATUS, string aj_TECH2_REMARK, string aj_TECH2_APPRV_DATE)
        {

            string message = string.Empty;
            string subno = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_Tec2", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inTECH2_NAME", aj_TECH2_NAME);
                command.Parameters.AddWithValue("@inTECH2_APPRV_STATUS", aj_TECH2_APPRV_STATUS);
                command.Parameters.AddWithValue("@inTECH2_REMARK", aj_TECH2_REMARK);
                command.Parameters.AddWithValue("@inTECH2_APPRV_DATE", aj_TECH2_APPRV_DATE);
                command.Parameters.AddWithValue("@inWarrantyClmType", clamtyp);
                command.Parameters.AddWithValue("@inTECH2_QTY", aj_TTECH2_QTY);
                command.Parameters.AddWithValue("@inTECH2_PROCESS_ANLYS", aj_TECH2_PROCESS_ANLYS);
                command.Parameters.AddWithValue("@inANLYS_AFTERPROCESS", aj_ANLYS_AFTERPROCESS);
                command.Parameters.AddWithValue("@inSCRAP_DATE", aj_SCRAP_DATE);
                command.Parameters.AddWithValue("@inCLM_TECH2_WASTE_QTY", aj_CLM_TECH2_WASTE_QTY);
                command.Parameters.AddWithValue("@inCLM_TECH2_GOOD_QTY", aj_CLM_TECH2_GOOD_QTY);
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

        public JsonResult SaveProcessClaimDetailAdmin(string aj_DOC_TRANSFER, string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_CLM_RCVSTATUS, string aj_CLM_RCVBY, string aj_CLM_QTY, string aj_CLM_DATE, string aj_CLM_ADMIN, string aj_CLM_SHELF_LOCATION, string aj_CLM_REMARK, string aj_CLM_RCVDATE, string aj_CLM_DUEDATE, string aj_ADMIN_ANLYS_STATUS, string aj_TECH1_PROCESS_STATUS)
        { 

            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_Detail_Admin", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inCLM_RCVSTATUS", aj_CLM_RCVSTATUS);
                command.Parameters.AddWithValue("@inCLM_RCVBY", aj_CLM_RCVBY);
                command.Parameters.AddWithValue("@inCLM_QTY", aj_CLM_QTY);
                command.Parameters.AddWithValue("@inADMIN_ANLYS_STATUS", aj_ADMIN_ANLYS_STATUS);
                command.Parameters.AddWithValue("@inTECH1_PROCESS_STATUS", aj_TECH1_PROCESS_STATUS);
                command.Parameters.AddWithValue("@inCLM_ADMIN", aj_CLM_ADMIN);
                command.Parameters.AddWithValue("@inCLM_SHELF_LOCATION", aj_CLM_SHELF_LOCATION);
                command.Parameters.AddWithValue("@inCLM_REMARK", aj_CLM_REMARK);
                command.Parameters.AddWithValue("@inCLM_RCVDATE", aj_CLM_RCVDATE);
                command.Parameters.AddWithValue("@inCLM_DUEDATE", aj_CLM_DUEDATE);
                command.Parameters.AddWithValue("@inCLM_DATE", aj_CLM_DATE);
                command.Parameters.AddWithValue("@inCLM_DOC_TRANSFER", aj_DOC_TRANSFER);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveProcessClaimUpdateAdmin(string aj_REQ_NO, string aj_CLM_NO_SUB, string aj_CLM_COMMENT, string aj_CLM_RCVBY)
        {

            string message = string.Empty;
            string subno = string.Empty;
            string fag = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Process_ClaimWH_UpdateComment_Admin", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inREQ_NO", aj_REQ_NO);
                command.Parameters.AddWithValue("@inCLM_NO_SUB", aj_CLM_NO_SUB);
                command.Parameters.AddWithValue("@inCLM_COMMENT", aj_CLM_COMMENT);
                command.Parameters.AddWithValue("@inCLM_RCVBY", aj_CLM_RCVBY);
              
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();
                fag = returnValuedoc.Value.ToString();
                if (fag == "Y")
                {




                    message = "true";
                }
                command.Dispose();



            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, subno }, JsonRequestBehavior.AllowGet);
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
                var command = new SqlCommand("P_Save_PathImageWH_tec", Connection);
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
                file.SaveAs(Server.MapPath(@"~\ImgUploadWH\" + uname));


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
    }
}
