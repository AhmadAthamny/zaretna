using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ZaretnaPanel
{
    public partial class addProductToCategory : Form
    {
        public addProductToCategory()
        {
            InitializeComponent(); 
            panel1.HorizontalScroll.Maximum = 0;
            panel1.AutoScroll = false;
            panel1.VerticalScroll.Visible = false;
            panel1.AutoScroll = true;            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DoSearch();
        }
        private void DoSearch()
        {
            string searchingText = this.txtSearchText.Text.Trim();
            if (searchingText.Length > 2)
            {
                string[] searchingWords = searchingText.Split(' ');
                string queryString = "SELECT P.ID, P.A_NAME, P.H_NAME, P.FINAL_PRICE, P.IMG_VERSION, UN.H_NAME AS UNIT_NAME FROM products P LEFT JOIN unit_names UN ON P.UNIT_ID = UN.ID WHERE P.REMOVED!=2 AND (";
                int wordsFound = 0;
                for (int i = 0; i < searchingWords.Length; i++)
                {
                    if (searchingWords[i].Trim().Length <= 2) continue;
                    searchingWords[i] = MySqlHelper.EscapeString(searchingWords[i]);

                    if (wordsFound > 0) queryString += " OR ";
                    queryString += string.Format("P.A_NAME LIKE '%25" + searchingWords[i] + "%25' OR P.H_NAME LIKE '%25" + searchingWords[i] + "%25'", searchingWords[i], searchingWords[i]);
                    wordsFound++;
                }
                if (wordsFound > 0)
                {
                    DataTable dt = new API_CLASS().API_QUERY(queryString + ")");
                    FillResults(dt);
                }
            }
            else
                this.txtSearchText.Text = "";
        }
        private void FillResults(DataTable dt)
        {
            this.panel1.Controls.Clear();
            int lastY = 4;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list_productControl LPCTRL = new list_productControl(row);
                    LPCTRL.Location = new Point(0, lastY);
                    LPCTRL.MouseEnter += OnMouseEnterProduct;
                    foreach (Control ctrls in LPCTRL.Controls)
                        ctrls.MouseEnter += OnMouseEnterProduct;

                    this.panel1.Controls.Add(LPCTRL);

                    lastY += LPCTRL.Height + 8;
                }
            }
            fixWindowSize(lastY - 5, dt.Rows.Count);
        }
        private void fixWindowSize(int totalY, int productCount)
        {
            // height
            int diff = this.panel1.Height - totalY;
            if (diff <= 0)
            {
                this.Height += -diff;
            }
            else
                this.Height -= diff;

            if (productCount > 5)
            {
                this.panel1.Width = 363;
                this.Width = 384;
                this.txtSearchText.Size = new Size(333, 29);
            }
            else
            { 
                this.panel1.Width = 343;
                this.Width = 366;
                this.txtSearchText.Size = new Size(313, 29);
            }
        }
        private void OnMouseEnterProduct(object sender, EventArgs ev)
        {
            foreach (Control ctrl in panel1.Controls)
                (ctrl as list_productControl).BorderStyle = BorderStyle.None;

            if (sender is list_productControl)
                (sender as list_productControl).BorderStyle = BorderStyle.FixedSingle;
            else
                (((sender as Control).Parent) as list_productControl).BorderStyle = BorderStyle.FixedSingle;
        }

        private void txtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                DoSearch();
            }
        }
    }
}
