using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\paulj\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }
        public void LoadProducts()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%"+txtSearch.Text+"%' ", con);
            con.Open();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();

        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm prodmoduleForm = new ProductModuleForm();
                prodmoduleForm.lblProdID.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                prodmoduleForm.txtProdName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                prodmoduleForm.txtPqty.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                prodmoduleForm.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                prodmoduleForm.txtPdescription.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                prodmoduleForm.comboCty.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                prodmoduleForm.btnSave.Enabled = false;
                prodmoduleForm.btnUpdate.Enabled = true;
                prodmoduleForm.ShowDialog();
            }

            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are You Sure You Want to DELETE This Product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbProduct WHERE pid LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "' ", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }

            LoadProducts();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm prodModule = new ProductModuleForm();
            prodModule.btnSave.Enabled = true;
            prodModule.btnUpdate.Enabled = false;
            prodModule.ShowDialog();
            LoadProducts();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }
    }
}
