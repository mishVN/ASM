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
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class createcustomer : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public createcustomer()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            StyleCreditGrid();
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcusname.Text) ||
                string.IsNullOrWhiteSpace(txtaddress.Text) ||
                string.IsNullOrWhiteSpace(txtnic.Text) ||
                string.IsNullOrWhiteSpace(txtnic.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        conn.Open();
                        string insert = @"INSERT INTO Customer(CustomerName,ContactNumber,Address,CustomerNIC) VALUES(@CustomerName,@ContactNumber,@Address,@CustomerNIC)";
                        using (SqlCommand cmd = new SqlCommand(insert, conn))
                        {
                            
                            cmd.Parameters.AddWithValue("@CustomerName", txtcusname.Text.Trim());
                            cmd.Parameters.AddWithValue("@ContactNumber", txtnumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", txtaddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@CustomerNIC", txtnic.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Successfully added to the database.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                            }
                            else
                            {
                                MessageBox.Show("No rows were inserted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void LoadCustomerCreditDetails()
        {
            string query = @"
    SELECT 
        VehicleRegNumber AS [Vehicle Reg. No],
        SellingPrice AS [Selling Price],
        NoOfInstallments AS [Installments],
        InstallmentValue AS [Installment Value]
    FROM customers_credits
    WHERE CustomerNIC = @nic AND VehicleRegNumber = @vehicleno";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@nic", txtnic.Text.Trim());
                        cmd.Parameters.AddWithValue("@vehicleno", txtregnumber.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading credit details: " + ex.Message);
            }

        }
        private void StyleCreditGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 255);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Teal;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtnic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtnic.Text))
                {
                    MessageBox.Show("Please enter a NIC number.");
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        conn.Open();
                        string query = @"SELECT CustomerName, ContactNumber, Address
                             FROM Customer
                             WHERE CustomerNIC = @nic";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nic", txtnic.Text.Trim());

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtcusname.Text = reader["CustomerName"].ToString();
                                    txtnumber.Text = reader["ContactNumber"].ToString();
                                    txtaddress.Text = reader["Address"].ToString();

                                    txtregnumber.Focus();
                                   
                                }
                                else
                                {
                                    MessageBox.Show("No customer found with that NIC.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txtcusname.Clear();
                                    txtnumber.Clear();
                                    txtaddress.Clear();
                                    txtnic.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcusname.Text) ||
        string.IsNullOrWhiteSpace(txtaddress.Text) ||
        string.IsNullOrWhiteSpace(txtnic.Text) ||
        string.IsNullOrWhiteSpace(txtnumber.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    string update = @"UPDATE Customer 
                              SET CustomerName = @CustomerName,
                                  ContactNumber = @ContactNumber,
                                  Address = @Address
                              WHERE CustomerNIC = @CustomerNIC";

                    using (SqlCommand cmd = new SqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerName", txtcusname.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtnumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtaddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerNIC", txtnic.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No matching customer found to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtregnumber_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                LoadCustomerCreditDetails();
                CalculateTotalInstallmentValue();
                CalculateBalance();
                ShowNextPaymentDate();
            }
        }

        private void CalculateTotalInstallmentValue()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Installment Value"].Value != null)
                {
                    decimal value;
                    if (decimal.TryParse(row.Cells["Installment Value"].Value.ToString(), out value))
                    {
                        total += value;
                    }
                }
            }

            lbltotalvalue.Text = total.ToString("N2");

        }

        private decimal GetSellingPrice(string vehicleRegNumber)
        {
            decimal sellingPrice = 0;
            string query = "SELECT SellingPrice FROM VehicleDetails WHERE RegisteredNumber = @vehicleno";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@vehicleno", vehicleRegNumber);

                        object result = cmd.ExecuteScalar();
                        if (result != null && decimal.TryParse(result.ToString(), out decimal price))
                        {
                            sellingPrice = price;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching selling price: " + ex.Message);
            }

            return sellingPrice;
        }

        private void CalculateBalance()
        {
            if (string.IsNullOrWhiteSpace(txtregnumber.Text)) return;

            decimal totalPaid = 0;
            if (!decimal.TryParse(lbltotalvalue.Text.Replace("Rs.", "").Trim(), out totalPaid))
            {
                MessageBox.Show("Invalid total paid amount.");
                return;
            }

            decimal sellingPrice = GetSellingPrice(txtregnumber.Text.Trim());
            decimal balance = sellingPrice - totalPaid;

            lblbalance.Text = $"Balance: Rs. {balance:N2}";
        }

        private DateTime? GetNextPaymentDate(string vehicleRegNumber)
        {
            try
            {
                DateTime reservationDateOnlyDay;
                int monthsPaid = 0;

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    
                    string resQuery = @"SELECT TOP 1 ReservationDate 
                            FROM Reservation 
                            WHERE VehicleNumber = @vehicleno 
                            ORDER BY ReservationDate ASC";

                    using (SqlCommand cmdRes = new SqlCommand(resQuery, conn))
                    {
                        cmdRes.Parameters.AddWithValue("@vehicleno", vehicleRegNumber);

                        object result = cmdRes.ExecuteScalar();
                        if (result != null && DateTime.TryParse(result.ToString(), out DateTime fullResDate))
                        {
                            
                            reservationDateOnlyDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, fullResDate.Day);
                        }
                        else
                        {
                            MessageBox.Show("Reservation date not found.");
                            return null;
                        }
                    }

                    
                    string payQuery = @"SELECT COUNT(*) 
                            FROM customers_credits 
                            WHERE VehicleRegNumber = @vehicleno";

                    using (SqlCommand cmdPay = new SqlCommand(payQuery, conn))
                    {
                        cmdPay.Parameters.AddWithValue("@vehicleno", vehicleRegNumber);
                        monthsPaid = Convert.ToInt32(cmdPay.ExecuteScalar());
                    }
                }

                
                DateTime nextPaymentDate = reservationDateOnlyDay.AddMonths(monthsPaid);
                return nextPaymentDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating next payment date: " + ex.Message);
                return null;
            }

        }

        private void ShowNextPaymentDate()
        {
            var nextDate = GetNextPaymentDate(txtregnumber.Text.Trim());

            if (nextDate.HasValue)
            {
                lblnext.Text = $"{nextDate.Value:yyyy-MM-dd}";
            }
        }

        private void txtcusname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtnumber.Focus();
            }
        }

        private void txtnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtaddress.Focus();
            }
        }
    }
}
