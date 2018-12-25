﻿using Khruphanth.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Khruphanth.Reports
{
    public partial class QRCODE : System.Web.UI.Page
    {
        private readonly ComCSDBEntities db = new ComCSDBEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CHKNAME"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Home/Login");
            }
            if (!IsPostBack)
            {
                List<View_QRCODE> ViewR = null;
                using (ComCSDBEntities dc = new ComCSDBEntities())
                {
                    ViewR = dc.View_QRCODE.OrderBy(a => a.KhruphanthID).ToList();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report2.rdlc");
                    //ReportViewer1.LocalReport.EnableExternalImages = true;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("DataSet1", ViewR);
                    ReportViewer1.LocalReport.DataSources.Add(rdc);
                    ReportViewer1.LocalReport.Refresh();

                }


            }
        }

        protected void Button2_Click2(object sender, EventArgs e)
        {
            var t1 = TextBox2.Text;
            var data = db.View_QRCODE.OrderBy(p => p.KhruphanthID).ToList();
            if (!String.IsNullOrEmpty(t1))
            {
                data = db.View_QRCODE.
             Where(p => p.KhruphanthID.Contains(t1)).ToList();


            }
            var rd = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report2.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
        }
    }
}