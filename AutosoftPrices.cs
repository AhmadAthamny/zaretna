using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaretnaPanel
{
    public partial class AutosoftPrices : Form
    {
        public LoadingForm lding = new LoadingForm();
        /* 
         * 0 = nothing has been changed in the product. No price and no discount.
         * 1 = price changed ONLY.
         * 2 = discount has been added to the product.
         * 3 = discount has been removed from the product.
         */
        public AutosoftPrices()
        {
            InitializeComponent();

            API_CLASS api = new API_CLASS();
            DataTable dt = api.API_QUERY("SELECT ID, FINAL_PRICE, H_NAME, PRICE_UPDATED, UNIT_VALUE FROM products WHERE PRICE_UPDATED>0 AND REMOVED!=2");

            productsTable.ReadOnly = true;

            productsTable.Columns.Add("ID", "#");
            productsTable.Columns.Add("H_NAME", "שם");
            productsTable.Columns.Add("customer_price", "מחיר צרכן");

            float tmpValue;
            
            foreach(DataRow row in dt.Rows)
            {
                tmpValue = float.Parse(row.Field<string>(1));
                productsTable.Rows.Add(row.Field<string>(0), row.Field<string>(2), tmpValue.ToString("F2"));

                if (float.Parse(row.Field<string>("UNIT_VALUE")) == 1)
                    productsTable.Rows[productsTable.Rows.Count - 1].DefaultCellStyle.BackColor = float.Parse(row.Field<string>(3)) == 2 ? Color.Green : float.Parse(row.Field<string>(3)) == 3 ? Color.Red : Color.White;
                else
                    productsTable.Rows[productsTable.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Blue;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPassword.Text != "zar12345671")
            {
                return;
            }

            DialogResult dlg = MessageBox.Show("האם סיימתה את השידור?", "אישור עדכון", MessageBoxButtons.YesNoCancel);
            if (dlg == DialogResult.Yes)
            {
                lding = new LoadingForm();
                lding.Show();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int productid;
            float totalRows = productsTable.Rows.Count;
            float rowReached = 0;
            float percentage;
            foreach (DataGridViewRow row in productsTable.Rows)
            {
                if (row.IsNewRow) continue;
                productid = Convert.ToInt32(row.Cells[0].Value);
                new API_CLASS().API_QUERY_EXECUTE("UPDATE products SET PRICE_UPDATED = 0 WHERE ID=" + productid);

                rowReached++;
                percentage = ((float)(rowReached / totalRows) * 100);

                lding.label1.Invoke((MethodInvoker)delegate {
                    lding.label1.Text = ((int)percentage).ToString() + "%";
                });
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lding.Close();
            BiggerMessage.Show("لقد تم حفظ التغييرات", 1);
            this.Close();
        }

        private void productsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex != -1 )
            {
                if ( productsTable.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Red)
                {
                    ManageProductDiscountInfo MPDI = new ManageProductDiscountInfo(int.Parse(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString()), false);
                    MPDI.ShowDialog(this);
                }
            }
        }

        private void AutosoftPrices_Load(object sender, EventArgs e)
        {
            productsTable.ClearSelection();
            productsTable.DoubleBuffered(true);
        }
    }
}
