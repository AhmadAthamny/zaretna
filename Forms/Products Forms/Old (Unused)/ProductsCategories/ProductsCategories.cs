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
    public partial class ProductsCategories : Form
    {

        private int LastCategory = -1;
        List<int> EditedProducts = new List<int>();
        List<int> ExistsAlready = new List<int>();
        int LastValue = -1;
        public ProductsCategories()
        {
            InitializeComponent();
            SetupTables();
        }
        public void SetupTables()
        {
            string cmd = "SELECT ID, A_NAME FROM categories";
            categoriesTable.DataSource = new API_CLASS().API_QUERY(cmd);

            if (categoriesTable.Rows.Count == 0) return;

            categoriesTable.Columns[0].HeaderText = "#";
            categoriesTable.Columns[1].HeaderText = "الإسم بالعربية";

            categoriesTable.Columns[0].FillWeight = 10;
            categoriesTable.Columns[1].FillWeight = 90;

            productsTable.Columns.Add("ID", "#");
            productsTable.Columns.Add("A_NAME", "الإسم بالعربية");
            productsTable.Columns.Add("INDEX", "الترتيب");

            productsTable.Columns[0].ReadOnly = true;
            productsTable.Columns[1].ReadOnly = true;

            DataGridViewCheckBoxColumn clmn = new DataGridViewCheckBoxColumn();
            clmn.HeaderText = "موجود";
            productsTable.Columns.Add(clmn);

            productsTable.Columns[0].FillWeight = 20;
            productsTable.Columns[1].FillWeight = 70;
            productsTable.Columns[2].FillWeight = 25;
            productsTable.Columns[3].FillWeight = 25;

            foreach (DataGridViewColumn clmn2 in categoriesTable.Columns)
                clmn2.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void FillProductsTable(int categoryID)
        {
            productsTable.Rows.Clear();
            EditedProducts.Clear();
            ExistsAlready.Clear();

            LastCategory = categoryID;

            string cmd = String.Format("SELECT a.id AS 'product_id', a.a_name AS 'a_name', b.PRODUCT_INDEX from products a INNER JOIN categories_products b where b.CATEGORY_ID={0} and a.FINAL_PRICE > 0 and a.REMOVED!=2 and a.ID = b.PRODUCT_ID ORDER BY PRODUCT_INDEX ASC", categoryID);
            DataTable dt = new API_CLASS().API_QUERY(cmd);

            int ID;
            string Name;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ID = Convert.ToInt32(row.Field<string>(0));
                    Name = row.Field<string>(1);
                    productsTable.Rows.Add(ID, Name, Convert.ToInt32(row.Field<string>(2)), true);
                    ExistsAlready.Add(ID);
                }
            }

            cmd = String.Format("SELECT p.ID, p.A_NAME FROM products p WHERE NOT EXISTS " +
                "(SELECT s.product_id FROM categories_products s WHERE " +
                "s.PRODUCT_ID = p.ID AND s.CATEGORY_ID = {0}) AND p.FINAL_PRICE > 0 AND p.REMOVED!=2", categoryID);

            dt = new API_CLASS().API_QUERY(cmd);
            foreach (DataRow row in dt.Rows)
            {
                ID = Convert.ToInt32(row.Field<string>(0));
                Name = row.Field<string>(1);
                productsTable.Rows.Add(ID, Name, -1, false);
            }
        }

        private void categoriesTable_SelectionChanged(object sender, EventArgs e)
        {
            if (categoriesTable.Rows.Count > 0)
                FillProductsTable(GetCategoryID());
        }

        public int GetCategoryID()
        {
            foreach (DataGridViewCell cell in categoriesTable.SelectedCells)
            {
                if (!categoriesTable.Rows[cell.RowIndex].IsNewRow && cell.RowIndex != -1)
                    return Convert.ToInt32(categoriesTable.Rows[cell.RowIndex].Cells[0].Value);
            }
            return -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CurrentCat = GetCategoryID();
            if (CurrentCat != -1)
            {
                int isChecked;

                string cmd;
                int index;
                int ProductID;
                foreach (DataGridViewRow row in productsTable.Rows)
                {
                    ProductID = int.Parse(row.Cells[0].Value.ToString());
                    if (!EditedProducts.Contains(ProductID)) continue;

                    index = Convert.ToInt32(row.Cells[2].Value);
                    isChecked = row.Cells[3].Value.ToString() == "True" ? 1 : 0;

                    if (isChecked == 1)
                    {
                        if (!ExistsAlready.Contains(ProductID))
                        {
                            cmd = String.Format("INSERT INTO categories_products (CATEGORY_ID, PRODUCT_ID, PRODUCT_INDEX) VALUES ({0}, {1}, {2})", CurrentCat, ProductID, index);
                            new API_CLASS().API_QUERY_EXECUTE(cmd);
                            ExistsAlready.Add(ProductID);
                        }
                        else
                        {
                            cmd = string.Format("UPDATE categories_products SET PRODUCT_INDEX={0} WHERE CATEGORY_ID={1} AND PRODUCT_ID={2}", index, LastCategory, ProductID);
                            new API_CLASS().API_QUERY_EXECUTE(cmd);
                        }
                    }
                    else
                    {
                        if (ExistsAlready.Contains(ProductID))
                        {
                            cmd = String.Format("DELETE FROM categories_products WHERE PRODUCT_ID={0} AND CATEGORY_ID={1}", ProductID, CurrentCat);
                            new API_CLASS().API_QUERY_EXECUTE(cmd);
                            ExistsAlready.Remove(ProductID);
                        }
                    }
                }
                BiggerMessage.Show("لقد تم حفظ جميع التغييرات", 1);
            }
            else
                BiggerMessage.Show("الرجاء إختيار فئة");
        }

        private void productsTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
                if (!EditedProducts.Contains(int.Parse(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString())))
                    EditedProducts.Add(int.Parse(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString()));          
        }

        private void productsTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            LastValue = int.Parse(productsTable.Rows[e.RowIndex].Cells[2].Value.ToString());
        }

        private void productsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool isInteger;
            int tmpValue;
            isInteger = int.TryParse(productsTable.Rows[e.RowIndex].Cells[2].Value.ToString(), out tmpValue);
            if(isInteger)
            {
                if (!EditedProducts.Contains(int.Parse(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString())))
                    EditedProducts.Add(int.Parse(productsTable.Rows[e.RowIndex].Cells[0].Value.ToString()));
            }
            else
            {
                BiggerMessage.Show("هذه ليست قيمة صحيحة");
                productsTable.Rows[e.RowIndex].Cells[2].Value = LastValue;
            }
        }

        private void ProductsCategories_Load(object sender, EventArgs e)
        {
            if (categoriesTable.Rows.Count > 0)
                FillProductsTable(int.Parse(categoriesTable.Rows[0].Cells[0].Value.ToString()));

            categoriesTable.DoubleBuffered(true);
            productsTable.DoubleBuffered(true);
        }
    }  
}
