namespace ZaretnaPanel
{
    partial class NewProductsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProductsForm));
            this.categoryPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmsEditCategory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ערוךToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEditCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // categoryPanel
            // 
            this.categoryPanel.AutoScroll = true;
            this.categoryPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.categoryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.categoryPanel.Location = new System.Drawing.Point(0, 0);
            this.categoryPanel.Name = "categoryPanel";
            this.categoryPanel.Size = new System.Drawing.Size(1356, 54);
            this.categoryPanel.TabIndex = 0;
            // 
            // cmsEditCategory
            // 
            this.cmsEditCategory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ערוךToolStripMenuItem});
            this.cmsEditCategory.Name = "cmsEditCategory";
            this.cmsEditCategory.Size = new System.Drawing.Size(99, 26);
            this.cmsEditCategory.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsEditCategory_ItemClicked);
            // 
            // ערוךToolStripMenuItem
            // 
            this.ערוךToolStripMenuItem.Image = global::ZaretnaPanel.Properties.Resources.edit_pen;
            this.ערוךToolStripMenuItem.Name = "ערוךToolStripMenuItem";
            this.ערוךToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.ערוךToolStripMenuItem.Text = "ערוך";
            // 
            // NewProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1356, 801);
            this.Controls.Add(this.categoryPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1302, 840);
            this.Name = "NewProductsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ניהול פריטים";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewProductsForm_FormClosed);
            this.Load += new System.EventHandler(this.NewProductsForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewProductsForm_KeyDown);
            this.cmsEditCategory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel categoryPanel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip cmsEditCategory;
        private System.Windows.Forms.ToolStripMenuItem ערוךToolStripMenuItem;
    }
}