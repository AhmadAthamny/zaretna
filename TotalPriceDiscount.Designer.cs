namespace ZaretnaPanel
{
    partial class TotalPriceDiscount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalPriceDiscount));
            this.label1 = new System.Windows.Forms.Label();
            this.discountEnabled = new System.Windows.Forms.CheckBox();
            this.theGroupBox = new System.Windows.Forms.GroupBox();
            this.showImageCB = new System.Windows.Forms.CheckBox();
            this.thepicturebox = new System.Windows.Forms.PictureBox();
            this.btnPricePic = new System.Windows.Forms.Button();
            this.discountPercentage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.minPrice = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.theGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thepicturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(142, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = ":السعر الأدنى لتفعيل التخفيض";
            // 
            // discountEnabled
            // 
            this.discountEnabled.AutoSize = true;
            this.discountEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discountEnabled.Location = new System.Drawing.Point(240, 15);
            this.discountEnabled.Name = "discountEnabled";
            this.discountEnabled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.discountEnabled.Size = new System.Drawing.Size(114, 28);
            this.discountEnabled.TabIndex = 1;
            this.discountEnabled.Text = "التخفيض فعال";
            this.discountEnabled.UseVisualStyleBackColor = true;
            this.discountEnabled.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // theGroupBox
            // 
            this.theGroupBox.Controls.Add(this.showImageCB);
            this.theGroupBox.Controls.Add(this.thepicturebox);
            this.theGroupBox.Controls.Add(this.btnPricePic);
            this.theGroupBox.Controls.Add(this.discountPercentage);
            this.theGroupBox.Controls.Add(this.label2);
            this.theGroupBox.Controls.Add(this.minPrice);
            this.theGroupBox.Controls.Add(this.label1);
            this.theGroupBox.Location = new System.Drawing.Point(12, 49);
            this.theGroupBox.Name = "theGroupBox";
            this.theGroupBox.Size = new System.Drawing.Size(342, 287);
            this.theGroupBox.TabIndex = 2;
            this.theGroupBox.TabStop = false;
            // 
            // showImageCB
            // 
            this.showImageCB.AutoSize = true;
            this.showImageCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showImageCB.Location = new System.Drawing.Point(142, 93);
            this.showImageCB.Name = "showImageCB";
            this.showImageCB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.showImageCB.Size = new System.Drawing.Size(115, 28);
            this.showImageCB.TabIndex = 8;
            this.showImageCB.Text = "إظهار الصورة";
            this.showImageCB.UseVisualStyleBackColor = true;
            this.showImageCB.Visible = false;
            // 
            // thepicturebox
            // 
            this.thepicturebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thepicturebox.Location = new System.Drawing.Point(21, 142);
            this.thepicturebox.Name = "thepicturebox";
            this.thepicturebox.Size = new System.Drawing.Size(300, 125);
            this.thepicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thepicturebox.TabIndex = 7;
            this.thepicturebox.TabStop = false;
            // 
            // btnPricePic
            // 
            this.btnPricePic.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPricePic.Location = new System.Drawing.Point(21, 101);
            this.btnPricePic.Name = "btnPricePic";
            this.btnPricePic.Size = new System.Drawing.Size(120, 32);
            this.btnPricePic.TabIndex = 6;
            this.btnPricePic.Text = "إختيار صورة";
            this.btnPricePic.UseVisualStyleBackColor = true;
            this.btnPricePic.Click += new System.EventHandler(this.btnPricePic_Click);
            // 
            // discountPercentage
            // 
            this.discountPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discountPercentage.Location = new System.Drawing.Point(21, 54);
            this.discountPercentage.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.discountPercentage.Name = "discountPercentage";
            this.discountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.discountPercentage.Size = new System.Drawing.Size(115, 29);
            this.discountPercentage.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(142, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = ":نسبة التخفيض";
            // 
            // minPrice
            // 
            this.minPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minPrice.Location = new System.Drawing.Point(21, 19);
            this.minPrice.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.minPrice.Name = "minPrice";
            this.minPrice.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.minPrice.Size = new System.Drawing.Size(115, 29);
            this.minPrice.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(102, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 38);
            this.button2.TabIndex = 3;
            this.button2.Text = "حفظ التغييرات";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TotalPriceDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(367, 392);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.theGroupBox);
            this.Controls.Add(this.discountEnabled);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(383, 431);
            this.MinimumSize = new System.Drawing.Size(383, 431);
            this.Name = "TotalPriceDiscount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "التخفيض الكلي";
            this.theGroupBox.ResumeLayout(false);
            this.theGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thepicturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discountPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox discountEnabled;
        private System.Windows.Forms.GroupBox theGroupBox;
        private System.Windows.Forms.PictureBox thepicturebox;
        private System.Windows.Forms.Button btnPricePic;
        private System.Windows.Forms.NumericUpDown discountPercentage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown minPrice;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox showImageCB;
    }
}