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
    public partial class useraccount : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public useraccount()
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

        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtname.Text) ||
                string.IsNullOrWhiteSpace(txtaccounttype.Text) ||
                string.IsNullOrWhiteSpace(txtpassword.Text))
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
                        string insert = @"INSERT INTO users(Username,Password,account_type) VALUES(@Username,@Password,@account_type)";
                        using (SqlCommand cmd = new SqlCommand(insert, conn))
                        {

                            cmd.Parameters.AddWithValue("@Username", txtname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", txtpassword.Text.Trim());
                            cmd.Parameters.AddWithValue("@account_type", txtaccounttype.Text.Trim());

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

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT password,account_type FROM users WHERE username = @name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtname.Text);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtpassword.Text = reader["password"].ToString();
                                txtaccounttype.Text = reader["account_type"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User not found!");
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtname.Text) ||
    string.IsNullOrWhiteSpace(txtaccounttype.Text) ||
    string.IsNullOrWhiteSpace(txtpassword.Text))
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

                        string updateQuery = @"UPDATE users 
                                   SET Password = @Password, 
                                       account_type = @account_type 
                                   WHERE Username = @Username";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", txtname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", txtpassword.Text.Trim());
                            cmd.Parameters.AddWithValue("@account_type", txtaccounttype.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User details updated successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No user found to update. Please check the username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button3_Click(object sender, EventArgs e)
        {
            txtname.Clear();
            txtpassword.Clear();
            txtaccounttype.Text = "";
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtaccounttype.Focus();
            }
        }
    }
}
