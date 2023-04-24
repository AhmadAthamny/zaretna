using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ZaretnaPanel
{
    public partial class UnitControl : UserControl
    {
        private int UnitID;
        BackgroundWorker bw = new BackgroundWorker();
        private bool deleted = false;
        public UnitControl(int tmpUnitID, string arText, string heText, string ruText, string totalProducts)
        {
            InitializeComponent();
            this.UnitID = tmpUnitID;
            this.lblArabic.Text = arText;
            this.lblHebrew.Text = heText;
            this.lblRussian.Text = ruText;
            this.lblCount.Text = totalProducts;

            this.arabicName.KeyDown += new KeyEventHandler(UnitControl_KeyDown);
            this.hebrewName.KeyDown += new KeyEventHandler(UnitControl_KeyDown);
            this.russianName.KeyDown += new KeyEventHandler(UnitControl_KeyDown);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(!bw.IsBusy)
            {
                this.pictureBox1.Image = Properties.Resources.ezgif_5_454df066aa63;
                this.pictureBox1.Cursor = Cursors.Default;
                bw = new BackgroundWorker();
                bw.DoWork += (s, ev) =>
                {
                    DataTable dt = new API_CLASS().API_QUERY("SELECT COUNT(*) FROM products WHERE UNIT_ID=" + this.UnitID);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (int.Parse(dt.Rows[0].Field<string>(0)) > 0)
                            {
                                BiggerMessage.Show("אי אפשר למחק יחידה ששיך לה פריטים");
                            }
                            else
                            {
                                new API_CLASS().API_QUERY("DELETE FROM unit_names WHERE ID=" + this.UnitID);
                                this.deleted = true;
                            }
                        }
                        else
                            BiggerMessage.Show("שגיאה");
                    }
                    else
                        BiggerMessage.Show("שגיאה");
                };
                bw.RunWorkerCompleted += (s, ev) =>
                {
                    this.pictureBox1.Image = Properties.Resources.delete_unit;
                    this.pictureBox1.Cursor = Cursors.Hand;
                    if (this.deleted)
                        this.Enabled = false;
                };
                bw.RunWorkerAsync();
            }
        }

        private void UnitControl_Load(object sender, EventArgs e)
        {
            if (this.UnitID == -1)
            {
                this.pictureBox4.Image = Properties.Resources.ezgif_5_454df066aa63;
                this.pictureBox4.Cursor = Cursors.Default;

                bw = new BackgroundWorker();
                bw.DoWork += (s, ev) =>
                {
                    this.UnitID = new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO unit_names (A_NAME, H_NAME, E_NAME) VALUES ('{0}', '{1}', '{2}')", "العربية", "עברית", "Russian"));
                };
                bw.RunWorkerCompleted += (s, ev) =>
                {
                    this.pictureBox4.Image = Properties.Resources.pen_edit;
                    this.pictureBox4.Cursor = Cursors.Hand;
                };
                bw.RunWorkerAsync();
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (!this.arabicName.Visible)
                ShowTextboxes();
            else HideTextboxes();
        }
        private void ShowTextboxes()
        {
            this.lblArabic.Visible = false;
            this.lblHebrew.Visible = false;
            this.lblRussian.Visible = false;

            this.arabicName.Text = this.lblArabic.Text;
            this.hebrewName.Text = this.lblHebrew.Text;
            this.russianName.Text = this.lblRussian.Text;

            this.arabicName.Visible = true;
            this.hebrewName.Visible = true;
            this.russianName.Visible = true;
        }

        private void HideTextboxes()
        {
            this.lblArabic.Visible = true;
            this.lblHebrew.Visible = true;
            this.lblRussian.Visible = true;

            this.arabicName.Visible = false;
            this.hebrewName.Visible = false;
            this.russianName.Visible = false;
        }

        private void UnitControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && this.arabicName.Visible)
                HideTextboxes();
            else if(e.KeyCode == Keys.Enter && this.arabicName.Visible && !this.bw.IsBusy)
            {
                this.pictureBox4.Image = Properties.Resources.ezgif_5_454df066aa63;
                this.pictureBox4.Cursor = Cursors.Default;
                bw = new BackgroundWorker();
                bw.DoWork += (s, ev) =>
                {
                    new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE unit_names SET A_NAME='{0}', H_NAME='{1}', E_NAME='{2}' WHERE ID={3}", MySqlHelper.EscapeString(this.arabicName.Text), MySqlHelper.EscapeString(this.hebrewName.Text), MySqlHelper.EscapeString(this.russianName.Text), this.UnitID));
                };
                bw.RunWorkerCompleted += (s, ev) =>
                {
                    this.pictureBox4.Image = Properties.Resources.pen_edit;
                    this.pictureBox4.Cursor = Cursors.Hand;

                    this.lblArabic.Text = this.arabicName.Text;
                    this.lblHebrew.Text = this.hebrewName.Text;
                    this.lblRussian.Text = this.russianName.Text;
                    HideTextboxes();

                };
                bw.RunWorkerAsync();
            }
        }
    }
}
