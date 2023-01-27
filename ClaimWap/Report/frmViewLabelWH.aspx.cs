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


namespace ClaimWap.Report
{
    public partial class frmViewLabelWH : System.Web.UI.Page
    {
        protected ReportViewer ReportViewerLableWH = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnLoadReportLableWH();
            }    
          
        }
        private void fnLoadReportLableWH()
        {
            string Doc = string.Empty;
            string Docwords = string.Empty;
            string Docdisplay = string.Empty;
            string SubDocwords = string.Empty;
            string SubDoc = string.Empty;
            //string Doc_subdisplay = string.Empty;
            Docdisplay = Request.QueryString["ClmNUM"];
            string[] words = Docdisplay.Split('/');
            // Doc_subdisplay = Request.QueryString["ClmsubNUM"];
            Docwords = words[0];
            byte[] data = System.Convert.FromBase64String(Docwords);
            Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);

            SubDocwords = words[1];
            byte[] datasub = System.Convert.FromBase64String(SubDocwords);
            SubDoc = System.Text.ASCIIEncoding.ASCII.GetString(datasub);
            
            DataSet ds1 = new DataSet();
            string conString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                ReportViewerLableWH.ProcessingMode = ProcessingMode.Local;
                ReportViewerLableWH.LocalReport.ReportPath = Server.MapPath("~/Report/rptLabelWH.rdlc");
                con.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter();


                SqlCommand cmd = new SqlCommand("P_GetClaim_ByDocSubWH", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DOC", Doc.ToString());
                cmd.Parameters.AddWithValue("@DOCSUB", SubDoc.ToString());
                sda1.SelectCommand = cmd;
                sda1.Fill(ds1, "DataSet1");
            }
            ReportDataSource datasource2 = new ReportDataSource("DataSet1", ds1.Tables[0]);
            //ReportDataSource datasource3 = new ReportDataSource("DataSet2", ds1.Tables[1]);
            ReportViewerLableWH.LocalReport.DataSources.Clear();
            ReportViewerLableWH.LocalReport.DataSources.Add(datasource2);
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
            " <PageWidth>3in</PageWidth>" +
            "<PageHeight>4in</PageHeight>" +
            "<MarginTop>0in</MarginTop>" +
            " <MarginLeft>0in</MarginLeft>" +
            " <MarginRight>0in</MarginRight>" +
            " <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = ReportViewerLableWH.LocalReport.Render(
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
            Response.AddHeader("content-disposition", "attachment; filename=rptClaim " + SubDoc + "-" +DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + fileNameExtension);

            Response.BinaryWrite(renderedBytes);


            string path = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) + @"\Downloads\rptClaimLableWH" + ".pdf";
            //WebClient client = new WebClient();
            // Byte[] buffer = client.DownloadData(path);
            System.IO.File.Delete(path);


            Response.End();


        }
    }
}