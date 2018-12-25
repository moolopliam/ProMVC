using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Khruphanth.Models;
using Microsoft.Reporting.WebForms;

namespace Khruphanth.Reports
{
    public partial class ViewRequisition : System.Web.UI.Page
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
                List<Khruphanth.Models.View_Requisition> ViewR = null;
                using (Khruphanth.Models.ComCSDBEntities dc = new ComCSDBEntities())
                {
                    ViewR = dc.View_Requisition.OrderBy(a => a.RL_NameKP).ToList();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_Requisittion.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("DataSet1", ViewR);
                    ReportViewer1.LocalReport.DataSources.Add(rdc);
                    ReportViewer1.LocalReport.Refresh();

                }
            }

        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            var t2 = DropDownList1.SelectedValue;
            var t1 = inputdatepicker.Text;
            var data = db.View_Requisition.OrderBy(p => p.TeaName).ToList();
            if (!String.IsNullOrEmpty(t2))
            {
                //var s1 = Convert.ToInt32(t2);
                data = db.View_Requisition.
                   Where(p => p.TeaName.Contains(t2)).ToList(); // Read data from file
                if(Convert.ToDateTime(t1) != DateTime.Now.Date)
                {
                    data = db.View_Requisition.
                 Where(p => p.Re_DateRequi.Contains(t1)).ToList();
                }

            }
            var rd = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_Requisittion.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
            DropDownList1.ClearSelection();
            inputdatepicker.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
        }

        protected void Button2_Click2(object sender, EventArgs e)
        {
            var t1 = TextBox2.Text;
            var data = db.View_Requisition.OrderBy(p => p.TeaName).ToList();
            if (!String.IsNullOrEmpty(t1))
            {
                    data = db.View_Requisition.
                 Where(p => p.RequisitionID.Contains(t1)).ToList();
                

            }
            var rd = new ReportDataSource("DataSet1", data);
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report_Requisittion.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
        }
    }
}