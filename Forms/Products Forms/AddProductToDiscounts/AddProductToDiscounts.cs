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
    public partial class AddProductToDiscounts : Form
    {
        public AddProductToDiscounts()
        {
            InitializeComponent();
            DataTable dt = new API_CLASS().API_QUERY("SELECT ID, H_NAME, PRICE, CUSTOM_ADD, REMOVED FROM products WHERE REMOVED!=2 AND DISCOUNT_QUANTITY=-1");

            productsTable.Columns.Add("id", "#");
            productsTable.Columns.Add("hname", "الإسم بالعبرية");
            productsTable.Columns.Add("fprice", "السعر النهائي");

            int pID;
            string H_NAME;
            float FINAL_PRICE;
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                pID = Convert.ToInt32(dt.Rows[i].Field<string>(0));
                H_NAME = dt.Rows[i].Field<string>(1);
                FINAL_PRICE = float.Parse(dt.Rows[i].Field<string>(2)) * (((float.Parse(dt.Rows[i].Field<string>(3)))/100)+1);
                productsTable.Rows.Add(pID, H_NAME, FINAL_PRICE.ToString("0.##"));
                if(Convert.ToInt32(dt.Rows[i].Field<string>(4)) == 1)
                {
                    productsTable.Rows[productsTable.Rows.Count-1].DefaultCellStyle.BackColor = Color.Black;
                    productsTable.Rows[productsTable.Rows.Count-1].DefaultCellStyle.ForeColor = Color.White;
                }
            }

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            int rowid = -1;
            foreach(DataGridViewRow row in productsTable.SelectedRows)
            {
                if(row.Index != -1 && !row.IsNewRow)
                {
                    rowid = Convert.ToInt32(row.Cells[0].Value.ToString());
                }
            }
            if(rowid == -1)
            {
                BiggerMessage.Show("الرجاء إختيار منتج لإضافته");
            }
            else
            {
                ManageProductDiscountInfo MPDI = new ManageProductDiscountInfo(rowid);
                MPDI.ShowDialog(this);
                this.Close();
            }
        }

        private void AddProductToDiscounts_Load(object sender, EventArgs e)
        {
            productsTable.ClearSelection();
            productsTable.DoubleBuffered(true);
        }
    }
}
