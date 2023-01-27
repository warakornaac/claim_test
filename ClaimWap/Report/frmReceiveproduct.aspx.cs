using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;


namespace ClaimWap.Report
{
    public partial class frmReceiveproduct : System.Web.UI.Page
    {
        protected ReportViewer ReportViewer2 = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportBoc();
            }
        }
       
        private void fnLoadReportBoc()
        {

            string Doc = string.Empty;
            string Docwords = string.Empty;
            string Docdisplay = string.Empty;


            Docdisplay = Request.QueryString["ClmNUM"];
            byte[] data = System.Convert.FromBase64String(Docdisplay);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);
            //Doc = "CM18110019,CM18120036,CM18120037";
           // Doc = "CMT19050064";
            string Cus = string.Empty;
            string slm = string.Empty;
            string item = string.Empty;
            string cusre = string.Empty;
            string cmsib = string.Empty;
            string picsImagepath = string.Empty;
            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                ReportViewer2.ProcessingMode = ProcessingMode.Local;
                ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/Report/rptReceiveproduct.rdlc");
             
                con.Open();

                SqlDataAdapter sda1 = new SqlDataAdapter();


                SqlCommand cmd = new SqlCommand("P_GetReqClaim_ByDoc_test", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cusre = dr["CUSNAM"].ToString();
                    Cus = dr["CUSCOD"].ToString() + '-' + Regex.Replace(cusre, "\"[^\"]*\"", string.Empty);
                    slm = dr["SLMCOD"].ToString() + '-' + dr["SLMNAM"].ToString();
                    item = dr["STKCOD"].ToString() + '-' + dr["STKDES"].ToString();
                    cmsib = dr["CLM_NO_SUB"].ToString();
                   // picsImagepath = new Uri(Server.MapPath("~/Images/" + dr["Imgsignature"].ToString() + "")).AbsoluteUri;
                    
                }
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
                con.Close();
            }
            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
            ReportViewer2.LocalReport.DataSources.Clear();
            ReportViewer2.LocalReport.DataSources.Add(datasource2);
            ReportViewer rpt = new ReportViewer();
            //rpt.SetPageSettings(new System.Drawing.Printing.PageSettings() { Landscape = true });
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType

            string deviceInfo =
            "<DeviceInfo>" +
            " <OutputFormat>PDF</OutputFormat>" +
            " <PageWidth>8.5in</PageWidth>" +
            "<PageHeight>11.7in</PageHeight>" +
            "<MarginTop>0.5in</MarginTop>" +
            " <MarginLeft>0.1in</MarginLeft>" +
            " <MarginRight>0.1in</MarginRight>" +
            " <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = ReportViewer2.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            ////clear the response stream and write the bytes to the outputstream
            //set content-disposition to “attachment” so that user is prompted to take an action
            //on the file (open or save)
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=rptReceiveproduct-" + cmsib + "-" + Cus + "-" + slm + "." + fileNameExtension);

            Response.BinaryWrite(renderedBytes);


            // string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptRequestClaim" + ".pdf";
            string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptReceiveproduct-" + cmsib + "-" + Cus + "-" + slm + ".pdf";
            //WebClient client = new WebClient();
            // Byte[] buffer = client.DownloadData(path);
            System.IO.File.Delete(path);


            Response.End();


        }
    }
}