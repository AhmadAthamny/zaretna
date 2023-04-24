namespace ZaretnaPanel
{
    partial class addNewProductControl
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
            this.transparentPic = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.existingProduct = new System.Windows.Forms.Button();
            this.newProduct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.transparentPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // transparentPic
            // 
            this.transparentPic.Image = global::ZaretnaPanel.Properties.Resources._61_opacity;
            this.transparentPic.Location = new System.Drawing.Point(0, 0);
            this.transparentPic.Name = "transparentPic";
            this.transparentPic.Size = new System.Drawing.Size(327, 130);
            this.transparentPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.transparentPic.TabIndex = 1;
            this.transparentPic.TabStop = false;
            this.transparentPic.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ZaretnaPanel.Properties.Resources.Untitled_12;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(327, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // existingProduct
            // 
            this.existingProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.existingProduct.Location = new System.Drawing.Point(169, 29);
            this.existingProduct.Name = "existingProduct";
            this.existingProduct.Size = new System.Drawing.Size(136, 76);
            this.existingProduct.TabIndex = 2;
            this.existingProduct.Text = "פריט קיים";
            this.existingProduct.UseVisualStyleBackColor = true;
            this.existingProduct.Visible = false;
            this.existingProduct.Click += new System.EventHandler(this.existingProduct_Click);
            // 
            // newProduct
            // 
            this.newProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newProduct.Location = new System.Drawing.Point(17, 29);
            this.newProduct.Name = "newProduct";
            this.newProduct.Size = new System.Drawing.Size(136, 76);
            this.newProduct.TabIndex = 3;
            this.newProduct.Text = "פריט חדש";
            this.newProduct.UseVisualStyleBackColor = true;
            this.newProduct.Visible = false;
            // 
            // addNewProductControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newProduct);
            this.Controls.Add(this.existingProduct);
            this.Controls.Add(this.transparentPic);
            this.Controls.Add(this.pictureBox1);
            this.Name = "addNewProductControl";
            this.Size = new System.Drawing.Size(327, 130);
            ((System.ComponentModel.ISupportInitialize)(this.transparentPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Button existingProduct;
        public System.Windows.Forms.Button newProduct;
        public System.Windows.Forms.PictureBox transparentPic;
        public System.Windows.Forms.PictureBox pictureBox1;
    }
}
