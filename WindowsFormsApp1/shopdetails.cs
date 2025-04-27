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
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace WindowsFormsApp1
{
    public partial class shopdetails : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public shopdetails()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            LoadShopDetails();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null) return null;

            
            Image resizedImage = ResizeImage(image, 150, 150);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Logo Image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }


        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtshopname.Text) ||
       string.IsNullOrWhiteSpace(txtaddress.Text) ||
       string.IsNullOrWhiteSpace(txtcontact.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] logoBytes = null;
            if (pictureBox1.Image != null)
            {
                logoBytes = ImageToByteArray(pictureBox1.Image);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO ShopDetails (ShopName, Address, ContactNumber, Logo)
                                   VALUES (@ShopName, @Address, @ContactNumber, @Logo)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ShopName", txtshopname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtaddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtcontact.Text.Trim());
                        cmd.Parameters.AddWithValue("@Logo", (object)logoBytes ?? DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Shop details saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert shop details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtaddress.Text) ||
        string.IsNullOrWhiteSpace(txtcontact.Text) ||
        string.IsNullOrWhiteSpace(txtshopname.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"UPDATE ShopDetails 
                             SET ShopName = @ShopName, 
                                 Address = @Address, 
                                 ContactNumber = @ContactNumber, 
                                 Logo = @Logo 
                             WHERE Id = 1"; // assuming a single row setup

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ShopName", txtshopname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtaddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtcontact.Text.Trim());
                        cmd.Parameters.AddWithValue("@Logo", ImageToByteArray(pictureBox1.Image));

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Shop details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating shop details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Logo Image";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }
        private void LoadShopDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT TOP 1 ShopName, Address, ContactNumber, Logo FROM ShopDetails";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtshopname.Text = reader["ShopName"].ToString();
                            txtaddress.Text = reader["Address"].ToString();
                            txtcontact.Text = reader["ContactNumber"].ToString();

                            if (reader["Logo"] != DBNull.Value)
                            {
                                byte[] logoBytes = (byte[])reader["Logo"];
                                using (MemoryStream ms = new MemoryStream(logoBytes))
                                {
                                    Image loadedImage = Image.FromStream(ms);
                                    pictureBox1.Image = ResizeImage(loadedImage, 150, 150); 
                                }
                            }

                            else
                            {
                                pictureBox1.Image = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shop details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image ResizeImage(Image originalImage, int targetWidth, int targetHeight)
        {
            var resized = new Bitmap(targetWidth, targetHeight);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
            }
            return resized;
        }

        private void txtshopname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtaddress.Focus();
            }
        }

        private void txtaddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtcontact.Focus();
            }
        }
    }
}
