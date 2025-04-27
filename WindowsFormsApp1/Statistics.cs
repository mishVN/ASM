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
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Statistics : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public Statistics()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            LoadVehicleProfitChart();
            LoadMonthlySalesChart();
            LoadYearlySalesChart();
            LoadSummary();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            LoadVehicleProfitChart();
            LoadMonthlySalesChart();
            LoadYearlySalesChart();
            LoadSummary();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmaxmin_Click(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Normal) ? FormWindowState.Maximized : FormWindowState.Normal;
        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void button4_Click(object sender, EventArgs e)
        {
            Sales_History sale = new Sales_History();
            sale.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Purchase_Details pd = new Purchase_Details();
            pd.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            logout log = new logout();
            log.Show();
            this.Hide();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadVehicleProfitChart();
            LoadMonthlySalesChart();
            LoadYearlySalesChart();
            LoadSummary();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void LoadVehicleProfitChart()
        {
            chartProfit.Series.Clear();
            chartProfit.Titles.Clear();
            chartProfit.Titles.Add("Profit Per Vehicle (Last 7 Days)");

            Series series = new Series("Profit")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SeaGreen
            };

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    string query = @"
                SELECT v.RegisteredNumber, v.Cost, p.SellingPrice
                FROM VehicleDetails v
                INNER JOIN PurchasingDetails p ON v.RegisteredNumber = p.VehicleRegNumber
                WHERE CAST(p.PurchaseDate AS DATE) >= CAST(DATEADD(DAY, -6, GETDATE()) AS DATE)
                  AND CAST(p.PurchaseDate AS DATE) <= CAST(GETDATE() AS DATE)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string reg = reader["RegisteredNumber"].ToString();
                        decimal cost = Convert.ToDecimal(reader["Cost"]);
                        decimal selling = Convert.ToDecimal(reader["SellingPrice"]);
                        decimal profit = selling - cost;

                        series.Points.AddXY(reg, profit);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicle profit chart: " + ex.Message);
                }
            }

            chartProfit.Series.Add(series);
        }


        private void LoadMonthlySalesChart()
        {
            chartMonthlySales.Series.Clear();
            chartMonthlySales.Titles.Clear();
            chartMonthlySales.Titles.Add("Monthly Sales (From MonthEndLog)");

            Series series = new Series("Monthly Sales")
            {
                ChartType = SeriesChartType.Line
            };

            SqlConnection conn = new SqlConnection(connstring);

            try
            {
                string query = @"
                    SELECT MonthName, TotalSales
                    FROM MonthEndLog
                    ORDER BY TRY_CAST(MonthName + '-01' AS DATE)";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string month = reader["MonthName"].ToString();
                    decimal totalSales = Convert.ToDecimal(reader["TotalSales"]);
                    series.Points.AddXY(month, totalSales);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading monthly sales chart: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            chartMonthlySales.Series.Add(series);
        }

        private void LoadYearlySalesChart()
        {
            chartYearlySales.Series.Clear();
            chartYearlySales.Titles.Clear();
            chartYearlySales.Titles.Add("Yearly Revenue (From YearEndLog)");

            Series series = new Series("Revenue")
            {
                ChartType = SeriesChartType.Bar
            };

            SqlConnection conn = new SqlConnection(connstring);

            try
            {
                string query = @"
                    SELECT Year AS SaleYear, TotalSales AS TotalRevenue
                    FROM YearEndLog
                    ORDER BY Year";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string year = reader["SaleYear"].ToString();
                    decimal revenue = Convert.ToDecimal(reader["TotalRevenue"]);
                    series.Points.AddXY(year, revenue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading yearly sales chart: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            chartYearlySales.Series.Add(series);
        }

        private void LoadSummary()
        {
            SqlConnection conn = new SqlConnection(connstring);

            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) AS TotalSales, 
                        SUM(v.SellingPrice) AS TotalRevenue,
                        SUM(v.SellingPrice - v.Cost) AS TotalProfit
                    FROM VehicleDetails v
                    WHERE v.RegisteredNumber IN (
                        SELECT p.VehicleRegNumber
                        FROM PurchasingDetails p
                        WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)
                    )";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTotalSales.Text = "Total Sales: " + reader["TotalSales"].ToString();
                    lblRevenue.Text = "Total Revenue: Rs. " + Convert.ToDecimal(reader["TotalRevenue"]).ToString("N2");
                    lblProfit.Text = "Total Profit: Rs. " + Convert.ToDecimal(reader["TotalProfit"]).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
