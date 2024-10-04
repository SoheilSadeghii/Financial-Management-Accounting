using Accounting.App.Accounting;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.ViewModels.Customers;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App
{
    public partial class frmReports : Form
    {
        public int TypeID = 0;
        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "Select!"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";

            }
            if (TypeID == 1)
            {
                this.Text = "Receipt Reports";
            }
            else
            {
                this.Text = "Payment Reports";
            }

            Filter();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;

                if (txtFromDate.Text != "  -  -")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    //result = result.Where(r => r.DataTitle >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "  -  -")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    //result = result.Where(r => r.DataTitle <= endDate.Value).ToList();
                }

                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID && a.CustomerID == customerId));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID));
                }
                //dgReport.AutoGenerateColumns = false;
                //dgReport.DataSource = result;
                dgReport.Rows.Clear();

                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameByID(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DataTitle.ToShortDateString(), accounting.Description);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frmNew = new frmNewAccounting();
                frmNew.AccountingID = id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtTable = new DataTable();
            dtTable.Columns.Add("Customer");
            dtTable.Columns.Add("Amount");
            dtTable.Columns.Add("Date");
            dtTable.Columns.Add("Description");
            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtTable.Rows.Add(
                    item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString()
                    );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT",dtTable);
            stiPrint.Show();
        }
    }
}
