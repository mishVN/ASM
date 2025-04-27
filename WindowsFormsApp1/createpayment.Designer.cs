namespace WindowsFormsApp1
{
    partial class createpayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(createpayment));
            this.button6 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtnic = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtvalue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblname = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbladdress = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblcontact = new System.Windows.Forms.Label();
            this.lblbalance = new System.Windows.Forms.Label();
            this.Balance = new System.Windows.Forms.Label();
            this.lblinstalmentvalue = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblsellingprice = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtvehiclenumber = new System.Windows.Forms.TextBox();
            this.lblpaidvalue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblno = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Navy;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button6.Location = new System.Drawing.Point(606, 19);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(166, 35);
            this.button6.TabIndex = 76;
            this.button6.Text = "Pay";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(812, 47);
            this.panel2.TabIndex = 75;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label4.Location = new System.Drawing.Point(267, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 25);
            this.label4.TabIndex = 15;
            this.label4.Text = "CREATE PAYMENT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(668, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 47);
            this.panel1.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(93, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 18);
            this.label1.TabIndex = 74;
            this.label1.Text = "NIC";
            // 
            // txtnic
            // 
            this.txtnic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnic.Location = new System.Drawing.Point(112, 72);
            this.txtnic.Name = "txtnic";
            this.txtnic.Size = new System.Drawing.Size(280, 24);
            this.txtnic.TabIndex = 73;
            this.txtnic.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnic_KeyPress);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.button6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 319);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(812, 209);
            this.panel3.TabIndex = 77;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(589, 209);
            this.dataGridView1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(478, 268);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 79;
            this.label2.Text = "Value";
            // 
            // txtvalue
            // 
            this.txtvalue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtvalue.Location = new System.Drawing.Point(536, 265);
            this.txtvalue.Name = "txtvalue";
            this.txtvalue.Size = new System.Drawing.Size(235, 24);
            this.txtvalue.TabIndex = 78;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 18);
            this.label3.TabIndex = 80;
            this.label3.Text = "Name";
            // 
            // lblname
            // 
            this.lblname.AutoSize = true;
            this.lblname.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblname.Location = new System.Drawing.Point(109, 112);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(32, 18);
            this.lblname.TabIndex = 81;
            this.lblname.Text = "N/A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(40, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 18);
            this.label6.TabIndex = 82;
            this.label6.Text = "Address";
            // 
            // lbladdress
            // 
            this.lbladdress.AutoSize = true;
            this.lbladdress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbladdress.Location = new System.Drawing.Point(109, 152);
            this.lbladdress.Name = "lbladdress";
            this.lbladdress.Size = new System.Drawing.Size(32, 18);
            this.lbladdress.TabIndex = 83;
            this.lbladdress.Text = "N/A";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(40, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 18);
            this.label8.TabIndex = 84;
            this.label8.Text = "Contact";
            // 
            // lblcontact
            // 
            this.lblcontact.AutoSize = true;
            this.lblcontact.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcontact.Location = new System.Drawing.Point(109, 188);
            this.lblcontact.Name = "lblcontact";
            this.lblcontact.Size = new System.Drawing.Size(32, 18);
            this.lblcontact.TabIndex = 85;
            this.lblcontact.Text = "N/A";
            // 
            // lblbalance
            // 
            this.lblbalance.AutoSize = true;
            this.lblbalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblbalance.Location = new System.Drawing.Point(532, 231);
            this.lblbalance.Name = "lblbalance";
            this.lblbalance.Size = new System.Drawing.Size(32, 18);
            this.lblbalance.TabIndex = 91;
            this.lblbalance.Text = "N/A";
            // 
            // Balance
            // 
            this.Balance.AutoSize = true;
            this.Balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Balance.Location = new System.Drawing.Point(463, 231);
            this.Balance.Name = "Balance";
            this.Balance.Size = new System.Drawing.Size(61, 18);
            this.Balance.TabIndex = 90;
            this.Balance.Text = "Balance";
            // 
            // lblinstalmentvalue
            // 
            this.lblinstalmentvalue.AutoSize = true;
            this.lblinstalmentvalue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblinstalmentvalue.Location = new System.Drawing.Point(533, 160);
            this.lblinstalmentvalue.Name = "lblinstalmentvalue";
            this.lblinstalmentvalue.Size = new System.Drawing.Size(32, 18);
            this.lblinstalmentvalue.TabIndex = 89;
            this.lblinstalmentvalue.Text = "N/A";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(409, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 18);
            this.label10.TabIndex = 88;
            this.label10.Text = "Instalment Value";
            // 
            // lblsellingprice
            // 
            this.lblsellingprice.AutoSize = true;
            this.lblsellingprice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsellingprice.Location = new System.Drawing.Point(533, 120);
            this.lblsellingprice.Name = "lblsellingprice";
            this.lblsellingprice.Size = new System.Drawing.Size(32, 18);
            this.lblsellingprice.TabIndex = 87;
            this.lblsellingprice.Text = "N/A";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(456, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 18);
            this.label12.TabIndex = 86;
            this.label12.Text = "Full Value";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(467, 69);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 36);
            this.label13.TabIndex = 93;
            this.label13.Text = "Vehicle\r\nNumber";
            // 
            // txtvehiclenumber
            // 
            this.txtvehiclenumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtvehiclenumber.Location = new System.Drawing.Point(537, 75);
            this.txtvehiclenumber.Name = "txtvehiclenumber";
            this.txtvehiclenumber.Size = new System.Drawing.Size(235, 24);
            this.txtvehiclenumber.TabIndex = 92;
            this.txtvehiclenumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtvehiclenumber_KeyPress);
            // 
            // lblpaidvalue
            // 
            this.lblpaidvalue.AutoSize = true;
            this.lblpaidvalue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpaidvalue.Location = new System.Drawing.Point(534, 195);
            this.lblpaidvalue.Name = "lblpaidvalue";
            this.lblpaidvalue.Size = new System.Drawing.Size(32, 18);
            this.lblpaidvalue.TabIndex = 95;
            this.lblpaidvalue.Text = "N/A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(432, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 18);
            this.label7.TabIndex = 94;
            this.label7.Text = "Paid Amount ";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(589, 209);
            this.panel4.TabIndex = 1;
            // 
            // lblno
            // 
            this.lblno.AutoSize = true;
            this.lblno.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblno.ForeColor = System.Drawing.SystemColors.Control;
            this.lblno.Location = new System.Drawing.Point(51, 271);
            this.lblno.Name = "lblno";
            this.lblno.Size = new System.Drawing.Size(32, 18);
            this.lblno.TabIndex = 96;
            this.lblno.Text = "N/A";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Navy;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(606, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 35);
            this.button1.TabIndex = 77;
            this.button1.Text = "Print Recipt";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // createpayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 528);
            this.Controls.Add(this.lblno);
            this.Controls.Add(this.lblpaidvalue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtvehiclenumber);
            this.Controls.Add(this.lblbalance);
            this.Controls.Add(this.Balance);
            this.Controls.Add(this.lblinstalmentvalue);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblsellingprice);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblcontact);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbladdress);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtvalue);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtnic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "createpayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "createpayment";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtnic;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtvalue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbladdress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblcontact;
        private System.Windows.Forms.Label lblbalance;
        private System.Windows.Forms.Label Balance;
        private System.Windows.Forms.Label lblinstalmentvalue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblsellingprice;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtvehiclenumber;
        private System.Windows.Forms.Label lblpaidvalue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblno;
        private System.Windows.Forms.Button button1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
    }
}