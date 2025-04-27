using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class saller_dashboard : Form
    {
        public saller_dashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createcustomer cus = new createcustomer();
            cus.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            createReservation res = new createReservation();
            res.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createpayment pay = new createpayment();
            pay.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Show_Vehicles show = new Show_Vehicles();
            show.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            logout log = new logout();
            log.Show();
            this.Hide();
        }
    }
}
