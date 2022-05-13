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
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@" Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\paulj\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr; 
        
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCty.Items.Clear();
            cm = new SqlCommand("SELECT catname FROM tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboCty.Items.Add(dr[0].ToString());
            }

            dr.Close();
            con.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboCty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Save this Product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
                {
                    cm = new SqlCommand("INSERT INTO tbProduct(pname, pqty, pprice, pdescription, pcategory)VALUES(@pname,@pqty, @pprice, @pdescription, @pcategory)", con);
                    cm.Parameters.AddWithValue("@pname", txtProdName.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(txtPqty.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(txtPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtPdescription.Text); 
                    cm.Parameters.AddWithValue("@pcategory", comboCty.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully saved!");
                    Clear();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        public void Clear()
        {
            txtProdName.Clear();
            txtPqty.Clear();
            txtPrice.Clear();
            txtPdescription.Clear();
            comboCty.Text = "";

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Update this Product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
                {
                    cm = new SqlCommand("UPDATE tbProduct SET pname=@pname, pqty=@pqty, pprice=@pprice, pdescription=@pdescription, pcategory=@pcategory WHERE pid LIKE '" + lblProdID.Text + "' ", con);

                    cm.Parameters.AddWithValue("@pname", txtProdName.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(txtPqty.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(txtPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtPdescription.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboCty.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been updated!");
                    this.Dispose();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
