namespace ZaretnaPanel
{
    partial class list_productControl
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
            this.lblName = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.productPic = new System.Windows.Forms.PictureBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.picLoading1 = new System.Windows.Forms.PictureBox();
            this.picLoading2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(69, 5);
            this.lblName.Name = "lblName";
            this.lblName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblName.Size = new System.Drawing.Size(192, 55);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "بطيخ أصفر\r\nאבטיח צהוב";
            this.lblName.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ZaretnaPanel.Properties.Resources.line;
            this.pictureBox2.Location = new System.Drawing.Point(68, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1, 55);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // productPic
            // 
            this.productPic.Image = global::ZaretnaPanel.Properties.Resources.unknown;
            this.productPic.Location = new System.Drawing.Point(267, 3);
            this.productPic.Name = "productPic";
            this.productPic.Size = new System.Drawing.Size(52, 52);
            this.productPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productPic.TabIndex = 0;
            this.productPic.TabStop = false;
            // 
            // lblPrice
            // 
            this.lblPrice.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.Location = new System.Drawing.Point(6, 10);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPrice.Size = new System.Drawing.Size(62, 19);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "11.80";
            this.lblPrice.Visible = false;
            // 
            // lblUnit
            // 
            this.lblUnit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit.Location = new System.Drawing.Point(6, 27);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUnit.Size = new System.Drawing.Size(62, 19);
            this.lblUnit.TabIndex = 4;
            this.lblUnit.Text = "יחידה";
            this.lblUnit.Visible = false;
            // 
            // picLoading1
            // 
            this.picLoading1.Image = global::ZaretnaPanel.Properties.Resources.loading;
            this.picLoading1.Location = new System.Drawing.Point(75, 5);
            this.picLoading1.Name = "picLoading1";
            this.picLoading1.Size = new System.Drawing.Size(186, 46);
            this.picLoading1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoading1.TabIndex = 5;
            this.picLoading1.TabStop = false;
            // 
            // picLoading2
            // 
            this.picLoading2.Image = global::ZaretnaPanel.Properties.Resources.loading;
            this.picLoading2.Location = new System.Drawing.Point(5, 5);
            this.picLoading2.Name = "picLoading2";
            this.picLoading2.Size = new System.Drawing.Size(61, 46);
            this.picLoading2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoading2.TabIndex = 6;
            this.picLoading2.TabStop = false;
            // 
            // list_productControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.picLoading2);
            this.Controls.Add(this.picLoading1);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.productPic);
            this.Name = "list_productControl";
            this.Size = new System.Drawing.Size(343, 60);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblUnit;
        public System.Windows.Forms.PictureBox productPic;
        private System.Windows.Forms.PictureBox picLoading1;
        private System.Windows.Forms.PictureBox picLoading2;
    }
}
