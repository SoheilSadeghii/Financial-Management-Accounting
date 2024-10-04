using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App.Accounting
{
    public partial class frmNewAccounting : Form
    {
        private UnitOfWork db;
        public int AccountingID = 0;
        public frmNewAccounting()
        {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();

            if (AccountingID != 0)
            {
                var account = db.AccountingRepository.GetById(AccountingID);
                txtAmount.Text = account.Amount.ToString();
                txtDescription.Text = account.Description.ToString();
                txtName.Text = db.CustomerRepository.GetCustomerNameByID(account.CustomerID);
                if (account.TypeID == 1)
                {
                    rbReceive.Checked = true;
                }
                else
                {
                    rbPay.Checked = true;
                }

                this.Text = "Edit Transaction";
                btnSave.Text = "Edit";
                db.Dispose();   
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbPay.Checked || rbReceive.Checked)
                {
                    db = new UnitOfWork();
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        TypeID = (rbReceive.Checked) ? 1 : 2,
                        DataTitle = DateTime.Now,
                        Description = txtDescription.Text
                    };
                    if (AccountingID == 0)
                    {
                        db.AccountingRepository.Insert(accounting);
                        db.Save();
                    }
                    else
                    {
                        accounting.ID = AccountingID;
                        db.AccountingRepository.Update(accounting);
                        //using (UnitOfWork db2= new UnitOfWork())
                        //{
                        //}
                    }

                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Please select the type of Transaction!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
