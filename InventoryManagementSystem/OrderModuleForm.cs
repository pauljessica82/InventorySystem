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
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\paulj\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomers();
            LoadProducts();
        }

        public void LoadCustomers()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid, cname FROM tbCustomer WHERE CONCAT(cid, cname) LIKE '%"+txtSearchCust.Text+"%' ", con);
            con.Open();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();

        }

        public void LoadProducts()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%"+txtSearchProd.Text+"%' ", con);
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

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void txtSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

     

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if (Convert.ToInt16(UPQty.Value) > qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UPQty.Value = UPQty.Value - 1; 
                return;
            }
            if(Convert.ToInt16(UPQty.Value) > 0)
            {
                int total = Convert.ToInt16(txtPrice.Text) * Convert.ToInt16(UPQty.Value);
                txtTotal.Text = total.ToString();
            }

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProdId.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtProdName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCid.Text == "")
                {
                    MessageBox.Show("Please Select Customer!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtProdId.Text == "")
                {
                    MessageBox.Show("Please Select Product!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                    if (MessageBox.Show("Are You Sure You Want to Insert this Order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
                {
                    cm = new SqlCommand("INSERT INTO tbOrder(odate,pid,cid,qty,price,total)VALUES(@odate,@pid,@cid,@qty,@price,@total)", con);
                    cm.Parameters.AddWithValue("@odate", dtOrder.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtProdId.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt32(txtCid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32(UPQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32(txtPrice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32(txtTotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been successfully inserted.");
                   
                    cm = new SqlCommand("UPDATE tbProduct SET pqty=(pqty-@pqty) WHERE pid LIKE '" + txtProdId.Text + "' ", con);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(UPQty.Value));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    Clear();
                    LoadProducts();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            txtCid.Clear();
            txtCname.Clear();

            txtProdId.Clear();
            txtProdName.Clear();

            txtPrice.Clear();
            UPQty.Value = 0;
            txtTotal.Clear();
            dtOrder.Value = DateTime.Now;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void GetQty()
        {
            cm = new SqlCommand("SELECT pqty FROM tbProduct WHERE pid='" + txtProdId.Text + "'", con);
            con.Open();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                qty = Convert.ToInt32((dr[0].ToString()));
            }
            dr.Close();
            con.Close();
        }

    }
}
