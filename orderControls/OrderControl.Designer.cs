namespace ZaretnaPanel
{
    partial class OrderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblOrderID = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.lblPayment = new System.Windows.Forms.Label();
            this.lblBuyTime = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(651, 2);
            this.lblFullName.Margin = new System.Windows.Forms.Padding(0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(212, 25);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "מסארן מצארווה\t";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblFullName.UseCompatibleTextRendering = true;
            // 
            // lblOrderID
            // 
            this.lblOrderID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblOrderID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOrderID.Location = new System.Drawing.Point(866, 2);
            this.lblOrderID.Margin = new System.Windows.Forms.Padding(0);
            this.lblOrderID.Name = "lblOrderID";
            this.lblOrderID.Size = new System.Drawing.Size(101, 25);
            this.lblOrderID.TabIndex = 2;
            this.lblOrderID.Text = "#25961";
            this.lblOrderID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRegion
            // 
            this.lblRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegion.Location = new System.Drawing.Point(305, 6);
            this.lblRegion.Margin = new System.Windows.Forms.Padding(0);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(168, 19);
            this.lblRegion.TabIndex = 4;
            this.lblRegion.Text = "באקה אל-גרבייה";
            this.lblRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRegion.UseCompatibleTextRendering = true;
            // 
            // lblPayment
            // 
            this.lblPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayment.Location = new System.Drawing.Point(10, 6);
            this.lblPayment.Margin = new System.Windows.Forms.Padding(0);
            this.lblPayment.Name = "lblPayment";
            this.lblPayment.Size = new System.Drawing.Size(79, 19);
            this.lblPayment.TabIndex = 8;
            this.lblPayment.Text = "אשראי";
            this.lblPayment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPayment.UseCompatibleTextRendering = true;
            this.lblPayment.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // lblBuyTime
            // 
            this.lblBuyTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblBuyTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBuyTime.Location = new System.Drawing.Point(100, 3);
            this.lblBuyTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblBuyTime.Name = "lblBuyTime";
            this.lblBuyTime.Size = new System.Drawing.Size(198, 24);
            this.lblBuyTime.TabIndex = 9;
            this.lblBuyTime.Text = "17/05/2021 23:23";
            this.lblBuyTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.Location = new System.Drawing.Point(495, 7);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(138, 19);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "0584546951";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPhone.UseCompatibleTextRendering = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ZaretnaPanel.Properties.Resources.new_order_structure2;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(966, 32);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // OrderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.lblBuyTime);
            this.Controls.Add(this.lblPayment);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblOrderID);
            this.Controls.Add(this.pictureBox2);
            this.Name = "OrderControl";
            this.Size = new System.Drawing.Size(965, 32);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label lblFullName;
        public System.Windows.Forms.Label lblOrderID;
        public System.Windows.Forms.Label lblRegion;
        public System.Windows.Forms.Label lblPayment;
        public System.Windows.Forms.Label lblBuyTime;
        private System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.Label lblPhone;
    }
}
