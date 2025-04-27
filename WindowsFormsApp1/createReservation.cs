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
    public partial class createReservation : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public createReservation()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
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

        private void txtRegisteredNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT * FROM VehicleDetails WHERE RegisteredNumber LIKE @reg";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", "%" + txtRegisteredNumber.Text + "%");

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["Status"]?.ToString();

                                if (!string.Equals(status, "Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show($"This vehicle cannot be processed because its current status is: {status}",
                                        "Vehicle Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }                               

                                txtMake.Text = reader["VehicleMake"].ToString();
                                txtModel.Text = reader["VehicleModel"].ToString();
                                txtYOM.Text = reader["YOM"].ToString();
                                txtROM.Text = reader["ROM"].ToString();
                                txtFuel.Text = reader["Fuel"].ToString();
                                txtEngineCapacity.Text = reader["EngineCapacity"].ToString();
                                txtRegisteredNumber.Text = reader["RegisteredNumber"].ToString();
                                txtMileage.Text = reader["Mileage"].ToString();
                                txtChassisNumber.Text = reader["ChassisNumber"].ToString();
                                txtEngineNumber.Text = reader["EngineNumber"].ToString();
                                txtBodyType.Text = reader["BodyType"].ToString();
                                txtColour.Text = reader["Colour"].ToString();
                                txtConditionType.Text = reader["ConditionType"].ToString();

                                if (reader["SellingPrice"] != DBNull.Value)
                                {
                                    double sellingPrice = Convert.ToDouble(reader["SellingPrice"]);
                                    lblsallingprice.Text = sellingPrice.ToString("F2");
                                }
                                else
                                {
                                    lblsallingprice.Text = "0.00";
                                }

                                if (reader["Image1"] != DBNull.Value)
                                    pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"]);

                                if (reader["Image2"] != DBNull.Value)
                                    pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"]);

                                if (reader["Image3"] != DBNull.Value)
                                    pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"]);

                                txtdownpayment.Focus();
                            }
                            else
                            {
                                MessageBox.Show("Vehicle not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }

            }
        }

        private Image ByteArrayToImage(byte[] byteArray, int width = 150, int height = 100)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                using (Image originalImage = Image.FromStream(ms))
                {
                    Bitmap resizedImage = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(resizedImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(originalImage, 0, 0, width, height);
                    }
                    return resizedImage;
                }
            }
        }


        private void txtdownpayment_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtdownpayment.Text.Trim(), out double downpayment) &&
                double.TryParse(lblsallingprice.Text.Trim(), out double sellingprice))
            {
                double balancepayment = sellingprice - downpayment;
                lblbalncepayment.Text = balancepayment.ToString("F2"); 
            }
            else
            {
                lblbalncepayment.Text = string.Empty;
                MessageBox.Show("Please enter valid numeric values for down payment and selling price.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            createcustomer cus = new createcustomer();
            cus.ShowDialog();

        }

        private void txtcustomernic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT CustomerName FROM Customer WHERE CustomerNIC = @nic";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nic", txtcustomernic.Text);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtcustomername.Text = reader["CustomerName"].ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"INSERT INTO Reservation 
                         (VehicleNumber, CustomerNIC, ReservationDate, DownPayment, BalancePayment, Rate, NumberOfInstallments) 
                         VALUES 
                         (@VehicleNumber, @CustomerNIC, @ReservationDate, @DownPayment, @BalancePayment, @Rate, @NumberOfInstallments)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Input validation
                        if (!decimal.TryParse(txtdownpayment.Text.Trim(), out decimal downPayment))
                        {
                            MessageBox.Show("Please enter a valid down payment.");
                            return;
                        }

                        if (!decimal.TryParse(txtinterstrate.Text.Trim(), out decimal rate))
                        {
                            MessageBox.Show("Please enter a valid interest rate.");
                            return;
                        }

                        if (!int.TryParse(txtnumberofinstallment.Text.Trim(), out int installments))
                        {
                            MessageBox.Show("Please enter a valid number of installments.");
                            return;
                        }

                        if (!decimal.TryParse(lblbalncepayment.Text.Trim(), out decimal balancePayment))
                        {
                            MessageBox.Show("Balance payment value is invalid.");
                            return;
                        }

                        // Add parameters
                        cmd.Parameters.AddWithValue("@VehicleNumber", txtRegisteredNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerNIC", txtcustomernic.Text.Trim());
                        cmd.Parameters.AddWithValue("@ReservationDate", DateTime.Now.Date); // Fixed current date
                        cmd.Parameters.AddWithValue("@DownPayment", downPayment);
                        cmd.Parameters.AddWithValue("@BalancePayment", balancePayment);
                        cmd.Parameters.AddWithValue("@Rate", rate);
                        cmd.Parameters.AddWithValue("@NumberOfInstallments", installments);

                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();

                        MessageBox.Show(rows > 0
                            ? "Reservation saved successfully."
                            : "Failed to save reservation.");
                        UpdateReservation();
                        UpdateCashValue();
                        InsertCustomerCredit();


                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("A database error occurred:\n" + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException fmtEx)
            {
                MessageBox.Show("Data format error:\n" + fmtEx.Message, "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show("Connection error:\n" + invOpEx.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void txtnumberofinstallment_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(lblbalncepayment.Text.Trim(), out double balance))
            {
                if (int.TryParse(txtnumberofinstallment.Text.Trim(), out int noOfInstallments))
                {
                    if (noOfInstallments > 0)
                    {
                        double one = balance / noOfInstallments;
                        lblq.Text = one.ToString("F2");
                    }
                    else
                    {
                        double value = 0.00;
                        lblq.Text = value.ToString("F2"); 

                    }
                }
                else
                {
                    lblq.Text = string.Empty;
                    MessageBox.Show("Please enter a valid number for installments.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                lblq.Text = string.Empty;
                MessageBox.Show("Please enter a valid numeric value for balance.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txtinterstrate_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(lblbalncepayment.Text.Trim(), out double tot))
            {
                if (double.TryParse(txtinterstrate.Text.Trim(), out double rate))
                {
                    if (int.TryParse(txtnumberofinstallment.Text.Trim(), out int noOfInstallments))
                    {
                        if (noOfInstallments > 0)
                        {
                            double total = ((tot * (rate / 100)) + tot) / noOfInstallments;
                            lblvalueoneinstalment.Text = total.ToString("F2");
                        }
                        else
                        {
                            double value1 = 0.00; 
                            lblvalueoneinstalment.Text = value1.ToString("F2");
                        }
                    }
                    else
                    {
                        lblvalueoneinstalment.Text = string.Empty;
                        MessageBox.Show("Please enter a valid number for installments.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    lblvalueoneinstalment.Text = string.Empty;
                    MessageBox.Show("Please enter a valid interest rate.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                lblvalueoneinstalment.Text = string.Empty;
                MessageBox.Show("Please enter a valid balance amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private bool UpdateReservation()
        {
            string query = "UPDATE VehicleDetails SET Status = 'Reserved' WHERE RegisteredNumber = @regNumber";

            using (SqlConnection conn = new SqlConnection(connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@regNumber", txtRegisteredNumber.Text.Trim());

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

        private void txtdownpayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtnumberofinstallment.Focus();
            }
        }

        private void txtnumberofinstallment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtinterstrate.Focus();
            }
        }

        private void txtinterstrate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtcustomernic.Focus();
            }
        }

        private bool UpdateCashValue()
        {
            decimal downpayment;
            if (decimal.TryParse(txtdownpayment.Text, out downpayment))
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
                        decimal updatedValue = currentValue + downpayment;

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

        private void InsertCustomerCredit()
        {
            string query = @"INSERT INTO customers_credits
                    (CustomerNIC, VehicleRegNumber, SellingPrice, NoOfInstallments, InstallmentValue)
                     VALUES (@nic, @reg, @price, @installments, @installmentValue)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nic", txtcustomernic.Text.Trim());
                        cmd.Parameters.AddWithValue("@reg", txtRegisteredNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@price", lblsallingprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@installments", txtnumberofinstallment.Text.Trim());
                        cmd.Parameters.AddWithValue("@installmentValue", lblvalueoneinstalment.Text.Trim());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Credit purchase saved successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting data: " + ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = "SELECT * FROM VehicleDetails WHERE RegisteredNumber LIKE @reg";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@reg", "%" + txtRegisteredNumber.Text + "%");

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        { 
                            string status = reader["Status"]?.ToString();

                            if (!string.Equals(status, "Available", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show($"This vehicle cannot be processed because its current status is: {status}",
                                    "Vehicle Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; 
                            }
                             
                            txtMake.Text = reader["VehicleMake"].ToString();
                            txtModel.Text = reader["VehicleModel"].ToString();
                            txtYOM.Text = reader["YOM"].ToString();
                            txtROM.Text = reader["ROM"].ToString();
                            txtFuel.Text = reader["Fuel"].ToString();
                            txtEngineCapacity.Text = reader["EngineCapacity"].ToString();
                            txtRegisteredNumber.Text = reader["RegisteredNumber"].ToString();
                            txtMileage.Text = reader["Mileage"].ToString();
                            txtChassisNumber.Text = reader["ChassisNumber"].ToString();
                            txtEngineNumber.Text = reader["EngineNumber"].ToString();
                            txtBodyType.Text = reader["BodyType"].ToString();
                            txtColour.Text = reader["Colour"].ToString();
                            txtConditionType.Text = reader["ConditionType"].ToString();

                            if (reader["SellingPrice"] != DBNull.Value)
                            {
                                double sellingPrice = Convert.ToDouble(reader["SellingPrice"]);
                                lblsallingprice.Text = sellingPrice.ToString("F2");
                            }
                            else
                            {
                                lblsallingprice.Text = "0.00";
                            }

                            if (reader["Image1"] != DBNull.Value)
                                pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"]);

                            if (reader["Image2"] != DBNull.Value)
                                pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"]);

                            if (reader["Image3"] != DBNull.Value)
                                pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"]);

                            txtdownpayment.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Vehicle not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }


        }
    }
}
