using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace WindowsFormsApp1
{
    public partial class createpayment : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;
        PrintDocument printDocument = new PrintDocument();
        private Font headerFont = new Font("Segoe UI", 14, FontStyle.Bold);
        private Font subHeaderFont = new Font("Segoe UI", 10, FontStyle.Italic);
        private Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
        private Font valueFont = new Font("Segoe UI", 10, FontStyle.Regular);
        private Font shopFont = new Font("Segoe UI", 12, FontStyle.Bold);
        private Pen linePen = new Pen(Color.Gray);


        public createpayment()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            StylePaymentsGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void txtnic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT CustomerName, ContactNumber, Address FROM Customer WHERE CustomerNIC = @NIC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NIC", txtnic.Text.Trim());

                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                lblname.Text = reader["CustomerName"].ToString();
                                lblcontact.Text = reader["ContactNumber"].ToString();
                                lbladdress.Text = reader["Address"].ToString();

                                txtvehiclenumber.Focus();
                            }
                            else
                            {
                                MessageBox.Show("Customer not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error loading customer data: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void LoadCustomerPayments()
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = @"
            SELECT 
                VehicleRegNumber AS [Vehicle Reg. No], 
                SellingPrice AS [Selling Price], 
                NoOfInstallments AS [Installments], 
                InstallmentValue AS [Installment Value], 
                Date AS [Date]
            FROM customers_credits
            WHERE CustomerNIC = @NIC AND VehicleRegNumber = @VehicleRegNumber";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NIC", txtnic.Text.Trim());
                    cmd.Parameters.AddWithValue("@VehicleRegNumber", txtvehiclenumber.Text.Trim());

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading payment data: " + ex.Message);
                    }
                }
            }
        }


        private void StylePaymentsGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 255);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowTemplate.Height = 30;

           
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }


        private void txtvehiclenumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string vehicleNo = txtvehiclenumber.Text.Trim();

                if (string.IsNullOrWhiteSpace(vehicleNo))
                {
                    MessageBox.Show("Please enter a vehicle number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"SELECT DownPayment, BalancePayment, Rate, NumberOfInstallments
                             FROM Reservation 
                             WHERE VehicleNumber = @VehicleNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNo);

                        try
                        {
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                decimal downPayment = Convert.ToDecimal(reader["DownPayment"]);
                                decimal balancePayment = Convert.ToDecimal(reader["BalancePayment"]);
                                decimal rate = Convert.ToDecimal(reader["Rate"]);
                                int numberOfInstallments = Convert.ToInt32(reader["NumberOfInstallments"]);

                                lblno.Text = numberOfInstallments.ToString();

                                GetSellingPriceByVehicle();
                                LoadCustomerPayments();
                                CalculateInstallmentTotal();
                                CalculateBalance();

                                txtvalue.Focus();

                                decimal sell;

                                if (decimal.TryParse(lblsellingprice.Text, out sell))
                                {
                                    decimal ins = ((sell * rate / 100) + sell) / numberOfInstallments;
                                    lblinstalmentvalue.Text = ins.ToString("N2");
                                }
                                else
                                {
                                    
                                    MessageBox.Show("Invalid decimal value in label.");
                                }


                            }
                            else
                            {
                                MessageBox.Show("No reservation data found for this vehicle number.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearReservationLabels();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error retrieving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void ClearReservationLabels()
        {
            lblsellingprice.Text = "";
            lblbalance.Text = "";
            lblinstalmentvalue.Text = "";
            lblpaidvalue.Text = "";
        }


        private void CalculateInstallmentTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Installment Value"].Value != null &&
                    decimal.TryParse(row.Cells["Installment Value"].Value.ToString(), out decimal value))
                {
                    total += value;
                }
            }

            lblpaidvalue.Text = total.ToString("N2");
        }


        private void CalculateBalance()
        {
            if (decimal.TryParse(lblsellingprice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal sellingPrice) &&
                decimal.TryParse(lblpaidvalue.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal paid))
            {
                decimal balance = sellingPrice - paid;
                lblbalance.Text = balance.ToString("N2");
            }
        }

        private void GetSellingPriceByVehicle()
        {
            string query = "SELECT SellingPrice FROM VehicleDetails WHERE RegisteredNumber = @RegNumber";

            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RegNumber", txtvehiclenumber.Text.Trim());

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && decimal.TryParse(result.ToString(), out decimal sellingPrice))
                    {
                        lblsellingprice.Text = sellingPrice.ToString("N2");
                    }
                    else
                    {
                        lblsellingprice.Text = "0.00";
                        MessageBox.Show("Vehicle not found or no selling price available.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving selling price: " + ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtnic.Text) ||
                string.IsNullOrWhiteSpace(txtvehiclenumber.Text) ||
                string.IsNullOrWhiteSpace(lblsellingprice.Text) ||
                string.IsNullOrWhiteSpace(lblinstalmentvalue.Text))
            {
                MessageBox.Show("Please make sure all fields are filled properly.");
                return;
            }

            string insertQuery = @"INSERT INTO customers_credits
        (CustomerNIC, VehicleRegNumber, SellingPrice, NoOfInstallments, InstallmentValue)
        VALUES (@NIC, @VehicleRegNumber, @SellingPrice, @Installments, @InstallmentValue)";

            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@NIC", txtnic.Text.Trim());
                cmd.Parameters.AddWithValue("@VehicleRegNumber", txtvehiclenumber.Text.Trim());

                if (!decimal.TryParse(lblsellingprice.Text, out decimal sellingPrice) ||
                    !decimal.TryParse(txtvalue.Text, out decimal installmentValue) ||
                    !int.TryParse(lblno.Text, out int noOfInstallments)) 
                {
                    MessageBox.Show("Invalid values detected. Please check the data.");
                    return;
                }

                cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                cmd.Parameters.AddWithValue("@Installments", noOfInstallments);
                cmd.Parameters.AddWithValue("@InstallmentValue", installmentValue);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                try
                {
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Payment details saved successfully.");
                        LoadCustomerPayments();
                        GetSellingPriceByVehicle();
                        CalculateInstallmentTotal();
                        CalculateBalance();
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving payment: " + ex.Message);
                }
            }
        }

        public class ShopInfo
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Contact { get; set; }
        }

        private ShopInfo GetShopDetails()
        {
            ShopInfo shop = new ShopInfo();

            string query = "SELECT TOP 1 ShopName, Address, ContactNumber FROM ShopDetails";

            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    shop.Name = reader["ShopName"].ToString();
                    shop.Address = reader["Address"].ToString();
                    shop.Contact = reader["ContactNumber"].ToString();
                }

                reader.Close();
            }

            return shop;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = pd;
            preview.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            float y = 30;
            float left = e.MarginBounds.Left;
            float right = e.MarginBounds.Right;

            ShopInfo shop = GetShopDetails();

            
            g.DrawString(shop.Name, shopFont, Brushes.Black, left, y);
            g.DrawString(shop.Address, subHeaderFont, Brushes.Black, left, y + 20);
            g.DrawString("Contact: " + shop.Contact, subHeaderFont, Brushes.Black, left, y + 40);
            y += 70;

            g.DrawLine(linePen, left, right, left + 700, y);
            y += 15;

            
            g.DrawString("VEHICLE PAYMENT RECEIPT", headerFont, Brushes.Black, left, y);
            y += 40;

            
            g.DrawString("Date:", labelFont, Brushes.Black, left, y);
            g.DrawString(DateTime.Now.ToString("yyyy-MM-dd"), valueFont, Brushes.Black, left + 100, y);
            y += 25;

            
            g.DrawString("Customer NIC:", labelFont, Brushes.Black, left, y);
            g.DrawString(txtnic.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Customer Name:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblname.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Contact:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblcontact.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            
            g.DrawString("Vehicle No:", labelFont, Brushes.Black, left, y);
            g.DrawString(txtvehiclenumber.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            
            g.DrawString("Selling Price:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblsellingprice.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Installment Value:", labelFont, Brushes.Black, left, y);
            g.DrawString(txtvalue.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Total Installments:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblno.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Total Paid:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblpaidvalue.Text, valueFont, Brushes.Black, left + 150, y);
            y += 25;

            g.DrawString("Balance:", labelFont, Brushes.Black, left, y);
            g.DrawString(lblbalance.Text, valueFont, Brushes.Black, left + 150, y);
            y += 40;

            
            g.DrawString("Thank you for your payment!", subHeaderFont, Brushes.Black, left, y);
        }

    }
}
