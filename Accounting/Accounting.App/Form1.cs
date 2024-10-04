using Accounting.App.Accounting;
using Accounting.Business;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Stimulsoft.Report.StiOptions;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomers = new frmCustomers();
            frmCustomers.ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e)
        {
            frmNewAccounting frmNewAccounting = new frmNewAccounting();
            frmNewAccounting.ShowDialog();
        }

        private void btnReportRecive_Click(object sender, EventArgs e)
        {
            frmReports frmReports = new frmReports();
            frmReports.TypeID = 1;
            frmReports.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            frmReports frmReports = new frmReports();
            frmReports.TypeID = 2;
            frmReports.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                lblDate.Text = DateTime.Now.ToShortDateString().ToString();
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();
            }
            else
            {
                Application.Exit();
            }
        }

        void Report()
        {
            BindGrid();
        }
        private void BindGrid()
        {
            ReportViewModel report = Account.ReportFormMain();
            lblPay.Text = report.Pay.ToString("#,0");
            lblReceive.Text = report.Recive.ToString("#,0");
            lblAccountBalance.Text = report.AccountBalance.ToString("#,0");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnLoginSetting_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.IsEdit = true;
            frmLogin.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
