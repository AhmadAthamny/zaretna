namespace ZaretnaPanel
{
    partial class ManageUnits
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageUnits));
            this.unitsContainer = new System.Windows.Forms.Panel();
            this.okButton = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // unitsContainer
            // 
            this.unitsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitsContainer.AutoScroll = true;
            this.unitsContainer.BackColor = System.Drawing.Color.Black;
            this.unitsContainer.Location = new System.Drawing.Point(0, 0);
            this.unitsContainer.MaximumSize = new System.Drawing.Size(700, 600);
            this.unitsContainer.Name = "unitsContainer";
            this.unitsContainer.Size = new System.Drawing.Size(460, 300);
            this.unitsContainer.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(150, 302);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "סגור";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(231, 302);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "הוספת יחידה";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ManageUnits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(458, 329);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.unitsContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(16, 39);
            this.Name = "ManageUnits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ערוך יחידות";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageUnits_FormClosing);
            this.Load += new System.EventHandler(this.ManageUnits_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel unitsContainer;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button btnAdd;
    }
}