using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OrderModuleForm ordmoduleForm = new OrderModuleForm();
            ordmoduleForm.btnInsert.Enabled = true;
            ordmoduleForm.btnUpdate.Enabled = false; 
            ordmoduleForm.ShowDialog();
        }
    }
}
