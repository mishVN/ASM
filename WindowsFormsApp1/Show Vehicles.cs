using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Show_Vehicles : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public Show_Vehicles()
        {
            InitializeComponent();
            LoadVehicleData();
            conn = new SqlConnection(connstring);
            dataGridView1.CellClick += dataGridView1_CellClick;      
            StyleDataGridView();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
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
        private void LoadVehicleData()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = @"
        SELECT  
            v.VehicleID,
            v.RegisteredNumber, 
            v.Status, 
            CASE 
                WHEN v.Status = 'Available' THEN NULL
                ELSE r.CustomerNIC
            END AS CustomerNIC
        FROM 
            VehicleDetails v
        LEFT JOIN 
            Reservation r ON v.RegisteredNumber = r.VehicleNumber";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int vehicleId = Convert.ToInt32(row.Cells["VehicleID"].Value);
                LoadVehicleImages(vehicleId);
            }
        }


        private void LoadVehicleImages(int vehicleId)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = "SELECT Image1, Image2, Image3 FROM VehicleDetails WHERE VehicleID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", vehicleId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["Image1"] != DBNull.Value)
                                pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"], pictureBox1.Width, pictureBox1.Height);

                            if (reader["Image2"] != DBNull.Value)
                                pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"], pictureBox2.Width, pictureBox2.Height);

                            if (reader["Image3"] != DBNull.Value)
                                pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"], pictureBox3.Width, pictureBox3.Height);
                        }
                    }
                }
            }
        }

        private Image ByteArrayToImage(byte[] bytes, int width, int height)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image original = Image.FromStream(ms);
                Bitmap resized = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(original, 0, 0, width, height);
                }
                return resized;
            }
        }

        private void StyleDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 49, 63);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.RowTemplate.Height = 30;

            // Optional settings
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }


    }
}
