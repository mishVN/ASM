using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Purchase_Details : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;
        string shopName = "";
        string shopAddress = "";
        string shopContact = "";
        Image shopLogo = null;

        public Purchase_Details()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);

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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"INSERT INTO PurchasingDetails
(CustomerName, ContactNumber, Address, CustomerNIC, ListingPrice, Discount, SellingPrice, VehicleRegNumber)
VALUES
(@CustomerName, @ContactNumber, @Address, @CustomerNIC, @ListingPrice, @Discount, @SellingPrice, @VehicleRegNumber)";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@CustomerNIC", txtnic.Text);
                        cmd.Parameters.AddWithValue("@ListingPrice", double.Parse(txtListingPrice.Text));
                        cmd.Parameters.AddWithValue("@Discount", double.Parse(txtDiscount.Text));
                        cmd.Parameters.AddWithValue("@SellingPrice", double.Parse(lblsellingprice.Text));
                        cmd.Parameters.AddWithValue("@VehicleRegNumber", txtVehicleRegNumber.Text);


                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Purchase record saved successfully!");
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving purchase: " + ex.Message);
            }
        }

        private void ClearPurchaseFields()
        {
            txtCustomerName.Clear();
            txtContactNumber.Clear();
            txtAddress.Clear();
            txtnic.Clear();
            txtVehicleRegNumber.Clear();
            txtListingPrice.Clear();
            txtDiscount.Clear();
            txtVehicleRegNumber.Clear();
            lblsellingprice.Text = "N/A";

            lblVehicleModel.Text = "N/A";
            lblYOM.Text = "N/A";
            lblROM.Text = "N/A";
            lblChassis.Text = "N/A";

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;


        }

        private void button9_Click(object sender, EventArgs e)
        {
            ClearPurchaseFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Vehicle add = new Add_Vehicle();
            add.Show();
            this.Hide();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {


            double listingPrice;

            if (double.TryParse(txtListingPrice.Text, out listingPrice))
            {
                double discount = 0.0;
                if (double.TryParse(txtDiscount.Text, out discount))
                {
                    double selling = 0.0;

                    selling = listingPrice - discount;

                    lblsellingprice.Text = selling.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid listing price. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid listing price. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void LoadVehicleData(string regNumber)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = "SELECT * FROM VehicleDetails WHERE RegisteredNumber = @reg";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reg", regNumber);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblVehicleModel.Text = reader["VehicleModel"].ToString();
                    lblYOM.Text = reader["YOM"].ToString();
                    lblROM.Text = reader["ROM"].ToString();
                    lblChassis.Text = reader["ChassisNumber"].ToString();

                    pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"]);
                    pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"]);
                    pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"]);
                }
            }
        }
        private Image ByteArrayToImage(byte[] bytes, int width = 150, int height = 100)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image original = Image.FromStream(ms);

                // Create a new blank image with the desired size
                Bitmap resized = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(original, 0, 0, width, height);
                }

                return resized;
            }
        }


        private void txtVehicleRegNumber_TextChanged(object sender, EventArgs e)
        {
            

        }


        private void button6_Click(object sender, EventArgs e)
        {
            LoadShopDetails(); 

            LoadVehicleData(txtVehicleRegNumber.Text); 
            
            printDocument1.PrintPage -= PrintInvoicePage;
            printDocument1.PrintPage += new PrintPageEventHandler(PrintInvoicePage);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDocument1;
            preview.ShowDialog();

            printDocument1.Print();

            UpdateVehicleStatus();
            UpdateCashValue();
            InsertCashDetail();
        }



        private void LoadShopDetails()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = "SELECT TOP 1 ShopName, Address, ContactNumber, Logo FROM ShopDetails";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    shopName = reader["ShopName"].ToString();
                    shopAddress = reader["Address"].ToString();
                    shopContact = reader["ContactNumber"].ToString();

                    byte[] imgBytes = reader["Logo"] as byte[];
                    if (imgBytes != null)
                    {
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            shopLogo = Image.FromStream(ms);
                        }
                    }
                }
            }
        }

        private void PrintInvoicePage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font headerFont = new Font("Segoe UI", 18, FontStyle.Bold);
            Font subHeaderFont = new Font("Segoe UI", 12, FontStyle.Regular);
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font contentFont = new Font("Segoe UI", 10);

            int left = 50;
            int top = 40;
            int gap = 25;

           
            if (shopLogo != null)
                g.DrawImage(shopLogo, left, top, 100, 100);

            
            g.DrawString(shopName, headerFont, Brushes.Black, left + 120, top);
            g.DrawString(shopAddress, subHeaderFont, Brushes.Black, left + 120, top + 35);
            g.DrawString("Contact: " + shopContact, subHeaderFont, Brushes.Black, left + 120, top + 60);

            top += 110;
            g.DrawLine(Pens.Black, left, top, 800, top);
            g.DrawString("INVOICE", headerFont, Brushes.Black, 350, top + 10);
            top += 60;

           
            g.DrawString("Customer Details", labelFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Name: ", labelFont, Brushes.Black, left, top);
            g.DrawString(txtCustomerName.Text, contentFont, Brushes.Black, left + 100, top); top += gap;

            g.DrawString("Address: ", labelFont, Brushes.Black, left, top);
            g.DrawString(txtAddress.Text, contentFont, Brushes.Black, left + 100, top); top += gap;

            g.DrawString("Contact: ", labelFont, Brushes.Black, left, top);
            g.DrawString(txtContactNumber.Text, contentFont, Brushes.Black, left + 100, top); top += gap;

            g.DrawString("NIC: ", labelFont, Brushes.Black, left, top);
            g.DrawString(txtnic.Text, contentFont, Brushes.Black, left + 100, top); top += gap;

            top += 10;
            g.DrawLine(Pens.Gray, left, top, 800, top); top += gap;

            
            g.DrawString("Vehicle Details", labelFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Model: " + lblVehicleModel.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Vehicle No: " + txtVehicleRegNumber.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("YOM: " + lblYOM.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("ROM: " + lblROM.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Chassis No: " + lblChassis.Text, contentFont, Brushes.Black, left, top); top += gap;

            top += 10;
            g.DrawLine(Pens.Gray, left, top, 800, top); top += gap;

            
            g.DrawString("Pricing", labelFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Listing Price: Rs. " + txtListingPrice.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Discount: Rs. " + txtDiscount.Text, contentFont, Brushes.Black, left, top); top += gap;
            g.DrawString("Selling Price: Rs. " + lblsellingprice.Text, contentFont, Brushes.Black, left, top); top += gap;

            top += gap;

            
            g.DrawString("Vehicle Images", labelFont, Brushes.Black, left, top); top += gap;

            int imageLeft = left;
            int imageTop = top;

            if (pictureBox1.Image != null)
                g.DrawImage(pictureBox1.Image, imageLeft, imageTop, 150, 100);
            if (pictureBox2.Image != null)
                g.DrawImage(pictureBox2.Image, imageLeft + 160, imageTop, 150, 100);
            if (pictureBox3.Image != null)
                g.DrawImage(pictureBox3.Image, imageLeft + 320, imageTop, 150, 100);

            top = imageTop + 120;

            
            int signatureTop = top + 100;
            g.DrawLine(Pens.Black, left, signatureTop, left + 200, signatureTop);
            g.DrawLine(Pens.Black, 600, signatureTop, 800, signatureTop);
            g.DrawString("Customer Signature", contentFont, Brushes.Black, left, signatureTop + 5);
            g.DrawString("Seller Signature", contentFont, Brushes.Black, 600, signatureTop + 5);
        }

        
    


        private void txtVehicleRegNumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtVehicleRegNumber.Text.Trim().Length == 0)
                    return;

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT * FROM VehicleDetails WHERE RegisteredNumber = @reg";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", txtVehicleRegNumber.Text.Trim());

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["Status"]?.ToString();

                                if (!string.Equals(status, "Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show($"This vehicle cannot be selected because its current status is: {status}",
                                        "Vehicle Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }


                                lblVehicleModel.Text = reader["VehicleModel"].ToString();
                                lblYOM.Text = reader["YOM"].ToString();
                                lblROM.Text = reader["ROM"].ToString();
                                lblChassis.Text = reader["ChassisNumber"].ToString();

                                if (reader["SellingPrice"] != DBNull.Value)
                                {
                                    double sellingPrice = Convert.ToDouble(reader["SellingPrice"]);
                                    txtListingPrice.Text = sellingPrice.ToString("F2");
                                }
                                else
                                {
                                    txtListingPrice.Text = "0.00";
                                }

                                if (reader["Image1"] != DBNull.Value)
                                    pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"]);

                                if (reader["Image2"] != DBNull.Value)
                                    pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"]);

                                if (reader["Image3"] != DBNull.Value)
                                    pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"]);

                                txtDiscount.Focus();
                            }
                            else
                            {

                                lblVehicleModel.Text = "";
                                lblYOM.Text = "";
                                lblROM.Text = "";
                                lblChassis.Text = "";
                                txtListingPrice.Text = "";

                                pictureBox1.Image = null;
                                pictureBox2.Image = null;
                                pictureBox3.Image = null;

                                MessageBox.Show("Vehicle not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }


        }

        private void txtnic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT CustomerName,ContactNumber,Address FROM Customer WHERE CustomerNIC = @nic";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nic", txtnic.Text);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCustomerName.Text = reader["CustomerName"].ToString();
                                txtContactNumber.Text = reader["ContactNumber"].ToString();
                                txtAddress.Text = reader["Address"].ToString();
                                txtVehicleRegNumber.Focus();

                            }
                            else
                            {
                                MessageBox.Show("Customer not found!");
                            }
                        }
                    }
                }
            }
        }

        private bool UpdateVehicleStatus()
        {
            string query = "UPDATE VehicleDetails SET Status = 'SoldOut' WHERE RegisteredNumber = @regNumber";

            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@regNumber", txtVehicleRegNumber.Text.Trim());

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private void btndashboard_Click(object sender, EventArgs e)
        {
            SystemDashboard dash = new SystemDashboard();
            dash.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Purchase_Details details = new Purchase_Details();  
            details.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            createcustomer cus = new createcustomer();
            cus.Show(); 
        }

        private bool UpdateCashValue()
        {
            decimal sellingprice;
            if (decimal.TryParse(lblsellingprice.Text, out sellingprice))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        conn.Open();

                        string selectQuery = "SELECT cash FROM cashValue WHERE id = 1";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
                        object result = selectCmd.ExecuteScalar();

                        decimal currentValue = result != null ? Convert.ToDecimal(result) : 0.00m;
                        decimal updatedValue = currentValue + sellingprice;

                        string updateQuery = "UPDATE CashValue SET cash = @updatedValue WHERE id = 1";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@updatedValue", updatedValue);

                        updateCmd.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating cash value: " + ex.Message);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Invalid selling price. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void InsertCashDetail()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "INSERT INTO CashDetail (description, value) VALUES (@description, @value)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@description", txtVehicleRegNumber.Text);
                        cmd.Parameters.AddWithValue("@value", lblsellingprice.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting cash detail: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Statistics sta = new Statistics();
            sta.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sales_History sale = new Sales_History();
            sale.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            logout lou = new logout();
            lou.Show();
            this.Hide();
        }
    }
}
