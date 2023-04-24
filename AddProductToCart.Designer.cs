namespace ZaretnaPanel
{
    partial class AddProductToCart
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProductToCart));
            this.productsTable = new System.Windows.Forms.DataGridView();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAdd = new System.Windows.Forms.Button();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.productsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // productsTable
            // 
            this.productsTable.AllowUserToAddRows = false;
            this.productsTable.AllowUserToDeleteRows = false;
            this.productsTable.AllowUserToResizeRows = false;
            this.productsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.productsTable.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGreen;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(182)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.productsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.productsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.productsTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.productsTable.EnableHeadersVisualStyles = false;
            this.productsTable.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.productsTable.Location = new System.Drawing.Point(-2, -3);
            this.productsTable.Margin = new System.Windows.Forms.Padding(2);
            this.productsTable.Name = "productsTable";
            this.productsTable.ReadOnly = true;
            this.productsTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.productsTable.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.productsTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.productsTable.RowHeadersVisible = false;
            this.productsTable.RowHeadersWidth = 70;
            this.productsTable.RowTemplate.Height = 33;
            this.productsTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.productsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.productsTable.Size = new System.Drawing.Size(854, 531);
            this.productsTable.TabIndex = 7;
            this.productsTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.productsTable_CellClick);
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantity.Location = new System.Drawing.Point(177, 559);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(301, 38);
            this.txtQuantity.TabIndex = 8;
            this.txtQuantity.Text = "1.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(486, 562);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 31);
            this.label1.TabIndex = 9;
            this.label1.Text = "כמות";
            // 
            // txtAdd
            // 
            this.txtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdd.Location = new System.Drawing.Point(12, 550);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(148, 55);
            this.txtAdd.TabIndex = 10;
            this.txtAdd.Text = "הוסף";
            this.txtAdd.UseVisualStyleBackColor = true;
            this.txtAdd.Click += new System.EventHandler(this.txtAdd_Click);
            // 
            // txtProductID
            // 
            this.txtProductID.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductID.Location = new System.Drawing.Point(651, 559);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(117, 38);
            this.txtProductID.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(774, 562);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 31);
            this.label2.TabIndex = 12;
            this.label2.Text = "פריט";
            // 
            // AddProductToCart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(851, 626);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProductID);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.productsTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(867, 665);
            this.MinimumSize = new System.Drawing.Size(867, 665);
            this.Name = "AddProductToCart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "إضافة منتج";
            this.Load += new System.EventHandler(this.AddProductToCart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView productsTable;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button txtAdd;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Label label2;
    }
}