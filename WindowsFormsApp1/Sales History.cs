using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Sales_History : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public Sales_History()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            LoadAllSales();
            StyleGrid();
            WindowState = FormWindowState.Maximized;    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmaxmin_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btndashboard_Click(object sender, EventArgs e)
        {
            SystemDashboard dash = new SystemDashboard();
            dash.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Vehicle add = new Add_Vehicle();
            add.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Statistics s = new Statistics();
            s.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Purchase_Details details = new Purchase_Details();  
            details.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            logout logout = new logout();
            logout.Show();
            this.Hide();
        }

        private void LoadAllSales()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"SELECT 
                                VehicleRegNumber AS [Reg. Number],
                                CustomerName AS [Customer Name],
                                CustomerNIC AS [NIC],
                               PurchaseDate AS [Date],
                                SellingPrice AS [Selling Price]
                             FROM PurchasingDetails";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SearchByRegNumber(string reg)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"SELECT 
                                VehicleRegNumber AS [Reg. Number],
                                CustomerName AS [Customer Name],
                                CustomerNIC AS [NIC],
                                PurchaseDate AS [Date],
                                SellingPrice AS [Selling Price]
                             FROM PurchasingDetails
                             WHERE VehicleRegNumber LIKE @reg";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@reg", "%" + reg + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error while searching: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkSlateBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.RowTemplate.Height = 35;

            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        private void txtSearchReg_TextChanged(object sender, EventArgs e)
        {
            SearchByRegNumber(txtSearchReg.Text.Trim());
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}

