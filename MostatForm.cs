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
    public partial class MostatForm : Form
    {
        float LastValue;
        public List<int> DeletedRows = new List<int>();
        List<int> modifiedRows = new List<int>();
        public MostatForm()
        {
            InitializeComponent();
            SetupCategoriesTable();
            FillDataGridView();
        }
        public void SetupCategoriesTable()
        {
            productsTable.Columns.Add("productNumber", "#");
            productsTable.Columns.Add("A_NAME", "الإسم بالعبرية");
            productsTable.Columns.Add("MOTSAT_NAME", "الإسم بالمجلس");
            productsTable.Columns.Add("UNIT_NAME", "إسم الوحدة");
            productsTable.Columns.Add("PRICE", "سعر الوحدة");
            productsTable.Columns.Add("UNIT_VALUE", "وزن الوحدة");
            productsTable.Columns.Add("CUSTOM_ADD", "إضافة");
            productsTable.Columns.Add("finalPrice", "السعر النهائي");
            productsTable.Columns.Add("step", "الخطوة");

            productsTable.Columns[0].FillWeight = 6;
            productsTable.Columns[1].FillWeight = 30;
            productsTable.Columns[2].FillWeight = 30;
            productsTable.Columns[3].FillWeight = 14;
            productsTable.Columns[4].FillWeight = 14;
            productsTable.Columns[5].FillWeight = 14;
            productsTable.Columns[6].FillWeight = 14;
            productsTable.Columns[7].FillWeight = 14;
            productsTable.Columns[8].FillWeight = 5;

            productsTable.Columns[0].ReadOnly = true;
            productsTable.Columns[1].ReadOnly = true;
            productsTable.Columns[3].ReadOnly = true;

            productsTable.Columns[8].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;


            foreach (DataGridViewColumn clmn in productsTable.Columns)
                clmn.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void FillDataGridView()
        {
            string cmd = "SELECT A.ID, A.H_NAME, A.MOTSAT_NAME, B.H_NAME AS UNIT_NAME, A.PRICE, A.UNIT_VALUE, A.CUSTOM_ADD, A.FINAL_PRICE, A.REMOVED, A.STEP FROM products A LEFT JOIN unit_names B ON A.UNIT_ID = B.ID WHERE A.MOTSAT_ID != -1 AND REMOVED!=2 ORDER BY A.H_NAME ASC";
            DataTable dt = new API_CLASS().API_QUERY(cmd);

            int ID, REMOVED;
            float PRICE, UNIT_VALUE, CUSTOM_ADD, FINAL_PRICE, STEP;
            string A_NAME, MOTSAT_NAME, UNIT_NAME;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ID = Convert.ToInt32(dt.Rows[i].Field<string>(0));
                A_NAME = dt.Rows[i].Field<string>(1);
                MOTSAT_NAME = dt.Rows[i].Field<string>(2);
                UNIT_NAME = dt.Rows[i].Field<string>(3);
                PRICE = float.Parse(dt.Rows[i].Field<string>(4));
                UNIT_VALUE = float.Parse(dt.Rows[i].Field<string>(5));
                CUSTOM_ADD = float.Parse(dt.Rows[i].Field<string>(6));
                FINAL_PRICE = float.Parse(dt.Rows[i].Field<string>(7));
                REMOVED = int.Parse(dt.Rows[i].Field<string>(8));
                STEP = float.Parse(dt.Rows[i].Field<string>(9));

                productsTable.Rows.Add(ID, A_NAME, MOTSAT_NAME, UNIT_NAME, PRICE, UNIT_VALUE, CUSTOM_ADD, FINAL_PRICE, STEP);

                if (REMOVED == 1)
                {
                    productsTable.Rows[productsTable.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Black;
                    productsTable.Rows[productsTable.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }
        private void productsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex <= 3)
            {
                if (!modifiedRows.Contains(e.RowIndex))
                    modifiedRows.Add(e.RowIndex);
            }
            else
            {
                bool isFloat;
                float tmpValue;
                if (e.RowIndex != -1)
                {
                    isFloat = float.TryParse(productsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out tmpValue);
                    if (!isFloat || tmpValue < 0)
                    {
                        productsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = LastValue;
                        BiggerMessage.Show("قيمة غير صالحة");
                        return;
                    }
                    else
                    {
                        UpdateFinalPrice(productsTable.Rows[e.RowIndex], e.ColumnIndex == 7 ? true : false);
                        if (!modifiedRows.Contains(e.RowIndex))
                            modifiedRows.Add(e.RowIndex);
                    }
                }
            }
        }

        private void productsTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(e.ColumnIndex > 3 )
                LastValue = float.Parse(productsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }

        private void UpdateFinalPrice(DataGridViewRow row, bool finalChanged = false)
        {
            float
                PRICE = float.Parse(row.Cells[4].Value.ToString()),
                UNIT_VALUE = float.Parse(row.Cells[5].Value.ToString()),
                CUSTOM_ADD = float.Parse(row.Cells[6].Value.ToString()),
                FINAL_PRICE = float.Parse(row.Cells[7].Value.ToString()),
                tmpValue;

            if (!finalChanged)
            {
                tmpValue = (float)(PRICE) * ((CUSTOM_ADD / 100) + 1);

                row.Cells[7].Value = tmpValue * UNIT_VALUE;
            }
            else
            {
                tmpValue = (FINAL_PRICE / UNIT_VALUE) / ((CUSTOM_ADD / 100) + 1);
                row.Cells[4].Value = tmpValue;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int PRODUCT_ID;
            float PRICE, UNIT_VALUE, CUSTOM_ADD, FINAL_PRICE, STEP;
            string MOTSAT_NAME, UNIT_NAME;
            DataGridViewRow row;

            foreach (int rowID in modifiedRows)
            {
                if (DeletedRows.Contains(rowID)) continue;
                row = productsTable.Rows[rowID];

                PRODUCT_ID = Convert.ToInt32(row.Cells[0].Value);
                MOTSAT_NAME = row.Cells[2].Value.ToString();
                UNIT_NAME = row.Cells[3].Value.ToString();
                PRICE = float.Parse(row.Cells[4].Value.ToString());
                UNIT_VALUE = float.Parse(row.Cells[5].Value.ToString());
                CUSTOM_ADD = float.Parse(row.Cells[6].Value.ToString());
                FINAL_PRICE = float.Parse(row.Cells[7].Value.ToString());
                STEP = float.Parse(row.Cells[8].Value.ToString());

                string cmd = string.Format("UPDATE products SET MOTSAT_NAME='{0}', PRICE={1}, UNIT_VALUE={2}, CUSTOM_ADD={3}, FINAL_PRICE={4}, STEP={5}, PRICE_UPDATED = CASE WHEN PRICE_UPDATED<2 THEN 1 ELSE PRICE_UPDATED END WHERE ID={6}", MySqlHelper.EscapeString(MOTSAT_NAME), PRICE, UNIT_VALUE, CUSTOM_ADD, FINAL_PRICE, STEP, PRODUCT_ID);
                new API_CLASS().API_QUERY_EXECUTE(cmd);
            }
            foreach(int rowID in DeletedRows)
            {
                string cmd = string.Format("UPDATE products SET MOTSAT_ID=-1 WHERE ID=" + Convert.ToInt32(productsTable.Rows[rowID].Cells[0].Value));
                new API_CLASS().API_QUERY_EXECUTE(cmd);
            }
            BiggerMessage.Show("لقد تم حفظ جميع التغييرات", 1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProductToMotsat APTM = new AddProductToMotsat(this);
            APTM.Show(this);
        }

        private void productsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                RemoveMotsatProduct RMP = new RemoveMotsatProduct(this, e.RowIndex);
                RMP.ShowDialog(this);
            }
        }

        private void MostatForm_Load(object sender, EventArgs e)
        {
            productsTable.DoubleBuffered(true);
        }
    }
}
