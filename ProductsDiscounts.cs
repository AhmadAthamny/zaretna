using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ZaretnaPanel
{
    public partial class ProductsDiscounts : Form
    {
        public ProductsDiscounts()
        {
            InitializeComponent();
            productsTable.Columns.Add("productid", "#");
            productsTable.Columns.Add("name", "الإسم بالعبرية");
            productsTable.Columns.Add("fprice", "ألسعر الطبيعي");
            productsTable.Columns.Add("discAmount", "كمية التخفيض");
            productsTable.Columns.Add("discPrice", "سعر الكمية المحددة");
            productsTable.Columns.Add("discName", "إسم التخفيض"); 
            
            productsTable.Columns[0].FillWeight = 9;
            productsTable.Columns[1].FillWeight = 25;
            productsTable.Columns[2].FillWeight = 10;
            productsTable.Columns[3].FillWeight = 10;
            productsTable.Columns[4].FillWeight = 10;
            productsTable.Columns[5].FillWeight = 36;

            productsTable.ReadOnly = true;

            API_CLASS api = new API_CLASS();
            DataTable dt = api.API_QUERY("SELECT ID, H_NAME, FINAL_PRICE, DISCOUNT_QUANTITY, DISCOUNT_PRICE, DISCOUNT_NAME, UNIT_VALUE, CUSTOM_ADD, PRICE FROM products WHERE DISCOUNT_QUANTITY!=-1 AND REMOVED!=2");
            DataTable discountName;
            float priceToList;
            foreach(DataRow row in dt.Rows)
            {
                priceToList = float.Parse(row.Field<string>(6)) == 1 ? float.Parse(row.Field<string>(2)) : (((float.Parse(row.Field<string>(7))/100)+1)*float.Parse(row.Field<string>(8)));
                discountName = (DataTable)JsonConvert.DeserializeObject("["+ row.Field<string>(5) + "]", (typeof(DataTable)));
                productsTable.Rows.Add(
                    Convert.ToInt32(row.Field<string>(0)),
                    row.Field<string>(1),
                    priceToList.ToString("0.##"),
                    Convert.ToInt32(row.Field<string>(3)),
                    float.Parse(row.Field<string>(4)),
                    discountName.Rows[0].Field<string>(0)
                    );
            }
        }

        private void productsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && !productsTable.Rows[e.RowIndex].IsNewRow)
            {
                int rowid = Convert.ToInt32(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                ManageProductDiscountInfo MPDI = new ManageProductDiscountInfo(rowid, false);
                MPDI.ShowDialog(this);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddProductToDiscounts APTD = new AddProductToDiscounts();
            APTD.ShowDialog(this);
        }

        public void AddProductToTable(int ProductID, string HebrewName, float ProductFinalPrice, int DiscountQuantity, float DiscountPrice, string DiscountName)
        {
            int rowid = GetProductRowID(ProductID);
            if(rowid == -1)
                productsTable.Rows.Add(ProductID, HebrewName, ProductFinalPrice, DiscountQuantity, DiscountPrice, DiscountName);
            else
            {
                productsTable.Rows[rowid].Cells[3].Value = DiscountQuantity;
                productsTable.Rows[rowid].Cells[4].Value = DiscountPrice;
                productsTable.Rows[rowid].Cells[5].Value = DiscountName;
            }
        }
        public void RemoveProductFromTable(int ProductID)
        {
            int rowid = GetProductRowID(ProductID);
            productsTable.Rows.RemoveAt(rowid);
        }
        public int GetProductRowID(int tmp_productid)
        {
            int rowid = -1;
            foreach(DataGridViewRow row in productsTable.Rows)
            {
                if(!row.IsNewRow)
                {
                    if (Convert.ToInt32(row.Cells[0].Value) == tmp_productid)
                    {
                        rowid = row.Index;
                    }
                }
            }
            return rowid;
        }

        private void ProductsDiscounts_Load(object sender, EventArgs e)
        {
            productsTable.DoubleBuffered(true);
        }
    }
}
