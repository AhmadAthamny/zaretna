namespace ZaretnaPanel
{
    partial class ProductControl
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
            this.unitName = new System.Windows.Forms.Label();
            this.arabicAndHebrewName = new System.Windows.Forms.Label();
            this.productPriceTB = new System.Windows.Forms.TextBox();
            this.lblLastPriceUpdate = new System.Windows.Forms.Label();
            this.btnArrowMove = new System.Windows.Forms.PictureBox();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.picNotActive = new System.Windows.Forms.PictureBox();
            this.picDiscount = new System.Windows.Forms.PictureBox();
            this.savingIconImg = new System.Windows.Forms.PictureBox();
            this.productImage = new System.Windows.Forms.PictureBox();
            this.btnMoveProduct = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnArrowMove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNotActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.savingIconImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // unitName
            // 
            this.unitName.BackColor = System.Drawing.Color.White;
            this.unitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitName.Location = new System.Drawing.Point(64, 110);
            this.unitName.Name = "unitName";
            this.unitName.Size = new System.Drawing.Size(66, 22);
            this.unitName.TabIndex = 5;
            this.unitName.Text = "יחידה";
            this.unitName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // arabicAndHebrewName
            // 
            this.arabicAndHebrewName.BackColor = System.Drawing.Color.White;
            this.arabicAndHebrewName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arabicAndHebrewName.Location = new System.Drawing.Point(0, 2);
            this.arabicAndHebrewName.Name = "arabicAndHebrewName";
            this.arabicAndHebrewName.Size = new System.Drawing.Size(205, 97);
            this.arabicAndHebrewName.TabIndex = 4;
            this.arabicAndHebrewName.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            this.arabicAndHebrewName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // productPriceTB
            // 
            this.productPriceTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productPriceTB.Location = new System.Drawing.Point(136, 101);
            this.productPriceTB.Name = "productPriceTB";
            this.productPriceTB.Size = new System.Drawing.Size(69, 29);
            this.productPriceTB.TabIndex = 1;
            this.productPriceTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.productPriceTB_KeyDown);
            this.productPriceTB.Leave += new System.EventHandler(this.productPriceTB_Leave);
            // 
            // lblLastPriceUpdate
            // 
            this.lblLastPriceUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblLastPriceUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastPriceUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblLastPriceUpdate.Location = new System.Drawing.Point(211, 121);
            this.lblLastPriceUpdate.Name = "lblLastPriceUpdate";
            this.lblLastPriceUpdate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastPriceUpdate.Size = new System.Drawing.Size(114, 21);
            this.lblLastPriceUpdate.TabIndex = 10;
            this.lblLastPriceUpdate.Text = "01/01/0001";
            this.lblLastPriceUpdate.Visible = false;
            // 
            // btnArrowMove
            // 
            this.btnArrowMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnArrowMove.Image = global::ZaretnaPanel.Properties.Resources.arrow_left;
            this.btnArrowMove.Location = new System.Drawing.Point(0, 35);
            this.btnArrowMove.Name = "btnArrowMove";
            this.btnArrowMove.Size = new System.Drawing.Size(20, 60);
            this.btnArrowMove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnArrowMove.TabIndex = 12;
            this.btnArrowMove.TabStop = false;
            this.btnArrowMove.Visible = false;
            this.btnArrowMove.Click += new System.EventHandler(this.btnArrowMove_Click);
            // 
            // loadingImage
            // 
            this.loadingImage.Image = global::ZaretnaPanel.Properties.Resources.loading;
            this.loadingImage.Location = new System.Drawing.Point(4, 0);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(205, 137);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingImage.TabIndex = 7;
            this.loadingImage.TabStop = false;
            // 
            // picNotActive
            // 
            this.picNotActive.Image = global::ZaretnaPanel.Properties.Resources.new_notactive;
            this.picNotActive.Location = new System.Drawing.Point(0, 66);
            this.picNotActive.Name = "picNotActive";
            this.picNotActive.Size = new System.Drawing.Size(327, 29);
            this.picNotActive.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picNotActive.TabIndex = 9;
            this.picNotActive.TabStop = false;
            this.picNotActive.Visible = false;
            // 
            // picDiscount
            // 
            this.picDiscount.Image = global::ZaretnaPanel.Properties.Resources.discount_icon;
            this.picDiscount.Location = new System.Drawing.Point(265, 6);
            this.picDiscount.Name = "picDiscount";
            this.picDiscount.Size = new System.Drawing.Size(63, 22);
            this.picDiscount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDiscount.TabIndex = 8;
            this.picDiscount.TabStop = false;
            this.picDiscount.Visible = false;
            // 
            // savingIconImg
            // 
            this.savingIconImg.BackColor = System.Drawing.Color.White;
            this.savingIconImg.Image = global::ZaretnaPanel.Properties.Resources.rsz_settings_512;
            this.savingIconImg.Location = new System.Drawing.Point(4, 104);
            this.savingIconImg.Name = "savingIconImg";
            this.savingIconImg.Size = new System.Drawing.Size(24, 25);
            this.savingIconImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.savingIconImg.TabIndex = 6;
            this.savingIconImg.TabStop = false;
            this.savingIconImg.Visible = false;
            this.savingIconImg.Click += new System.EventHandler(this.savingIconImg_Click);
            // 
            // productImage
            // 
            this.productImage.BackColor = System.Drawing.Color.White;
            this.productImage.Image = global::ZaretnaPanel.Properties.Resources.unknown;
            this.productImage.Location = new System.Drawing.Point(209, 3);
            this.productImage.Name = "productImage";
            this.productImage.Size = new System.Drawing.Size(116, 116);
            this.productImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productImage.TabIndex = 0;
            this.productImage.TabStop = false;
            // 
            // btnMoveProduct
            // 
            this.btnMoveProduct.BackColor = System.Drawing.Color.White;
            this.btnMoveProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoveProduct.Image = global::ZaretnaPanel.Properties.Resources.cursor_new;
            this.btnMoveProduct.Location = new System.Drawing.Point(31, 104);
            this.btnMoveProduct.Name = "btnMoveProduct";
            this.btnMoveProduct.Size = new System.Drawing.Size(24, 25);
            this.btnMoveProduct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMoveProduct.TabIndex = 11;
            this.btnMoveProduct.TabStop = false;
            this.btnMoveProduct.Click += new System.EventHandler(this.btnMoveProduct_Click);
            // 
            // ProductControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnArrowMove);
            this.Controls.Add(this.loadingImage);
            this.Controls.Add(this.picNotActive);
            this.Controls.Add(this.lblLastPriceUpdate);
            this.Controls.Add(this.picDiscount);
            this.Controls.Add(this.savingIconImg);
            this.Controls.Add(this.unitName);
            this.Controls.Add(this.arabicAndHebrewName);
            this.Controls.Add(this.productPriceTB);
            this.Controls.Add(this.productImage);
            this.Controls.Add(this.btnMoveProduct);
            this.Name = "ProductControl";
            this.Size = new System.Drawing.Size(327, 140);
            this.Leave += new System.EventHandler(this.ProductControl_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.btnArrowMove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNotActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.savingIconImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMoveProduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.PictureBox productImage;
        public System.Windows.Forms.PictureBox savingIconImg;
        public System.Windows.Forms.Label unitName;
        public System.Windows.Forms.Label arabicAndHebrewName;
        public System.Windows.Forms.TextBox productPriceTB;
        public System.Windows.Forms.PictureBox loadingImage;
        private System.Windows.Forms.PictureBox picDiscount;
        public System.Windows.Forms.PictureBox picNotActive;
        private System.Windows.Forms.Label lblLastPriceUpdate;
        public System.Windows.Forms.PictureBox btnMoveProduct;
        public System.Windows.Forms.PictureBox btnArrowMove;
    }
}
