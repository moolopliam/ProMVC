using Khruphanth.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Khruphanth.Reports
{
    public partial class Dis : System.Web.UI.Page
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
                List<Khruphanth.Models.View_Distributor> ViewR = null;
                using (Khruphanth.Models.ComCSDBEntities dc = new ComCSDBEntities())
                {
                    ViewR = dc.View_Distributor.OrderBy(a => a.DistributorID).ToList();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report3.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("DataSet1", ViewR);
                    ReportViewer1.LocalReport.DataSources.Add(rdc);
                    ReportViewer1.LocalReport.Refresh();

                }
            }

        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            var t1 = inputdatepicker.Text;
            var data = db.View_Distributor.OrderBy(p => p.DistributorID).ToList();

                if (Convert.ToDateTime(t1) != DateTime.Now.Date)
                {
                    data = db.View_Distributor.
                 Where(p => p.Di_Date.Contains(t1)).ToList();
                }
            var rd = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report3.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
 
            inputdatepicker.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
        }

        protected void Button2_Click2(object sender, EventArgs e)
        {
            var t1 = TextBox2.Text;
            var data = db.View_Distributor.OrderBy(p => p.DistributorID).ToList();
            if (!String.IsNullOrEmpty(t1))
            {
                data = db.View_Distributor.
             Where(p => p.DistributorID.Contains(t1)).ToList();


            }
            var rd = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report3.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
        }
    }
}