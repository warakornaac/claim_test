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
    public partial class frmDisposal : System.Web.UI.Page
    {
        protected ReportViewer ReportViewer4 = new ReportViewer();
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


            Docdisplay = Request.QueryString["IDNUM"];
            byte[] data = System.Convert.FromBase64String(Docdisplay);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);
            //Doc = "CM18110019,CM18120036,CM18120037";
           // Doc = "CMT19050064";
           
            string noid = string.Empty;
            string picsImagepath = string.Empty;
            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                ReportViewer4.ProcessingMode = ProcessingMode.Local;
                ReportViewer4.LocalReport.ReportPath = Server.MapPath("~/Report/rptDisposal.rdlc");
             
                con.Open();

                SqlDataAdapter sda1 = new SqlDataAdapter();


                SqlCommand cmd = new SqlCommand("P_Rpt_Disposal", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inDOC", Doc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    noid = dr["CLM_NO_Disposal"].ToString();
                   
                   // picsImagepath = new Uri(Server.MapPath("~/Images/" + dr["Imgsignature"].ToString() + "")).AbsoluteUri;
                    
                }
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
                con.Close();
            }
            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
            ReportViewer4.LocalReport.DataSources.Clear();
            ReportViewer4.LocalReport.DataSources.Add(datasource2);
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
            " <PageWidth>11.7in</PageWidth>" +
            "<PageHeight>8.5in</PageHeight>" +
            "<MarginTop>0.1in</MarginTop>" +
            " <MarginLeft>0.1in</MarginLeft>" +
            " <MarginRight>0.1in</MarginRight>" +
            " <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = ReportViewer4.LocalReport.Render(
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
            Response.AddHeader("content-disposition", "attachment; filename=rptSupplier-" + noid + "." + fileNameExtension);

            Response.BinaryWrite(renderedBytes);


            // string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptRequestClaim" + ".pdf";
            string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptDisposal" + noid + ".pdf";
            //WebClient client = new WebClient();
            // Byte[] buffer = client.DownloadData(path);
            System.IO.File.Delete(path);


            Response.End();


        }
    }
}