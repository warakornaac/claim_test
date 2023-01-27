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
    public class apidataController : Controller
    {
        //
        // GET: /apidata/

        
        public ActionResult GetClimdataPm(string inPM, string inDOC, string inCOM, string inSTATS, string instkgrp, string instatdate, string inenddate, string instatdatecus, string inenddatecus, string initem)
        {
            string message = string.Empty;
            Climedata model = null;
            List<ClimeDetail> Getdata = new List<ClimeDetail>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_ProcessClaim_Pm", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inPM", inPM);
            command.Parameters.AddWithValue("@inDOC", inDOC);
            command.Parameters.AddWithValue("@inCOM", inCOM);
            command.Parameters.AddWithValue("@inSTATS", inSTATS);
            command.Parameters.AddWithValue("@instkgrp", instkgrp);
            command.Parameters.AddWithValue("@instatdate", instatdate);
            command.Parameters.AddWithValue("@inenddatecus", inenddatecus);
            command.Parameters.AddWithValue("@instatdatecus", instatdatecus);
            command.Parameters.AddWithValue("@inenddate", inenddate);
            command.Parameters.AddWithValue("@initem", initem);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                //model = new Climedata();
                  Getdata.Add(new ClimeDetail()
                         {
                            CLM_ID = dr["CLM_ID"].ToString(),
                            CLM_NO_SUB = dr["CLM_NO_SUB"].ToString(),
                            CUSCOD = dr["CUSCOD"].ToString(),
                            CLM_RCVBY = dr["CLM_RCVBY"].ToString(),
                            CLM_RCVDATE = dr["CLM_RCVDATE"].ToString(),
                            STKCOD = dr["STKCOD"].ToString(),
                            STKDES = dr["STKDES"].ToString(),
                            CLM_UOM = dr["CLM_UOM"].ToString(),
                            CLM_REQQTY = dr["CLM_REQQTY"].ToString(),
                            CLM_QTY = dr["CLM_QTY"].ToString(),
                            CLM_INVNO = dr["CLM_INVNO"].ToString(),
                            CLM_INVDATE = dr["CLM_INVDATE"].ToString(),
                            CLM_USEDAY = dr["CLM_USEDAY"].ToString(),
                            CLM_CAUSE = dr["CLM_CAUSE"].ToString(),
                            CLM_PERFORM = dr["CLM_PERFORM"].ToString(),
                            CLM_RCVSTATUS = dr["CLM_RCVSTATUS"].ToString(),
                            PERFORMDESCRIPTION = dr["PERFORMDESCRIPTION"].ToString(),
                            RCVSTATUSDESCRIPTION = dr["RCVSTATUSDESCRIPTION"].ToString(),
                            CLM_COMPANY = dr["CLM_COMPANY"].ToString(),
                            CUSNAM = dr["CUSNAM"].ToString(),
                            SLMCOD = dr["SLMCOD"].ToString(),
                            SLMNAM = dr["SLMNAM"].ToString(),
                            ADMIN_ANLYS_STATUS = dr["ADMIN_ANLYS_STATUS"].ToString(),
                            TECH1_PROCESS_STATUS = dr["TECH1_PROCESS_STATUS"].ToString(),
                            TECH2_PROCESS_STATUS = dr["TECH2_PROCESS_STATUS"].ToString(),
                            TECH2_ANLYS_STATUS = dr["TECH2_ANLYS_STATUS"].ToString(),
                            PM_PROCESS_STATUS = dr["PM_PROCESS_STATUS"].ToString(),
                            CLM_Ref = dr["CLM_Ref"].ToString(),
                            CLM_DATE = dr["CLM_DATE"].ToString(),
                            TECH1ANLYSSTATUDESCRIPTION = dr["TECH1ANLYSSTATUDESCRIPTION"].ToString(),
                            TECH1_ANLYS_STATUS = dr["TECH1_ANLYS_STATUS"].ToString(),
                            CLM_FOC = dr["CLM_FOC"].ToString(),
                            STKGRP = dr["STKGRP"].ToString(),
                            F_BtnApp = dr["F_BtnApp"].ToString(),
                            CLM_STATUS = dr["CLM_STATUS"].ToString(),
                            Requestdate = dr["Requestdate"].ToString(),
                            Requestdatecus = dr["CusRequestdate"].ToString()
                              //Getdata.Add(new ClimetempListDetail { val = model });
                         });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
         
            return Json(Getdata, JsonRequestBehavior.AllowGet);
            
        
           // return View();
        }
       
    }
}
