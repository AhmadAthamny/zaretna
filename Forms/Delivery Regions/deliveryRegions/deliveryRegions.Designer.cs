namespace ZaretnaPanel
{
    partial class deliveryRegions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(deliveryRegions));
            this.regionsTable = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.worker_addRegion = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.regionsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // regionsTable
            // 
            this.regionsTable.AllowUserToAddRows = false;
            this.regionsTable.AllowUserToDeleteRows = false;
            this.regionsTable.AllowUserToResizeColumns = false;
            this.regionsTable.AllowUserToResizeRows = false;
            this.regionsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.regionsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.regionsTable.BackgroundColor = System.Drawing.Color.White;
            this.regionsTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.regionsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.regionsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.regionsTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.regionsTable.EnableHeadersVisualStyles = false;
            this.regionsTable.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.regionsTable.Location = new System.Drawing.Point(-2, -1);
            this.regionsTable.Margin = new System.Windows.Forms.Padding(2);
            this.regionsTable.MultiSelect = false;
            this.regionsTable.Name = "regionsTable";
            this.regionsTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.regionsTable.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.regionsTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.regionsTable.RowHeadersVisible = false;
            this.regionsTable.RowHeadersWidth = 70;
            this.regionsTable.RowTemplate.Height = 33;
            this.regionsTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.regionsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.regionsTable.Size = new System.Drawing.Size(492, 414);
            this.regionsTable.TabIndex = 1;
            this.regionsTable.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.regionsTable_CellBeginEdit);
            this.regionsTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regionsTable_CellDoubleClick);
            this.regionsTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.regionsTable_CellEndEdit);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(0, 417);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(490, 39);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "הוסף";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // worker_addRegion
            // 
            this.worker_addRegion.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_addRegion_DoWork);
            this.worker_addRegion.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_addRegion_RunWorkerCompleted);
            // 
            // deliveryRegions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(490, 456);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.regionsTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "deliveryRegions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "איזורי משלוח";
            this.Load += new System.EventHandler(this.deliveryRegions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.regionsTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.DataGridView regionsTable;
        private System.ComponentModel.BackgroundWorker worker_addRegion;
    }
}