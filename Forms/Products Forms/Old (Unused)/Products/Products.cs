using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace ZaretnaPanel
{
    public partial class Products : Form
    {

        public List<int> modifiedRows = new List<int>();
        public List<int> deletedRows = new List<int>();
        public Products()
        {
            InitializeComponent();
            SetupCategoriesTable();
            FillDataGridView();

            //tmp
            NewProductsForm NPRDCTS = new NewProductsForm();
            NPRDCTS.Show();
        }
        public void SetupCategoriesTable()
        {
            productsTable.Columns.Add("ProductID", "#");
            productsTable.Columns.Add("A_NAME", "الإسم بالعربية");
            productsTable.Columns.Add("H_NAME", "الإسم بالعبرية");

            DataGridViewCheckBoxColumn colmn = new DataGridViewCheckBoxColumn();
            colmn.HeaderText = "جديد";
            productsTable.Columns.Add(colmn);

            colmn = new DataGridViewCheckBoxColumn();
            colmn.HeaderText = "للبيع";
            productsTable.Columns.Add(colmn);

            productsTable.Columns[0].FillWeight = 6;
            productsTable.Columns[1].FillWeight = 32;
            productsTable.Columns[2].FillWeight = 32;
            productsTable.Columns[3].FillWeight = 15;
            productsTable.Columns[4].FillWeight = 15;

            productsTable.Columns[0].ReadOnly = true;

            productsTable.DefaultCellStyle.NullValue = null;

            foreach(DataGridViewColumn clmn in productsTable.Columns)
                clmn.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public void FillDataGridView()
        {
            API_CLASS api = new API_CLASS();
            string cmd = "SELECT ID, A_NAME, H_NAME, NEW, FORNOW, REMOVED FROM products WHERE REMOVED!=2";
            DataTable dt = api.API_QUERY(cmd);

            bool New, ForNow;
            List<int> toBeBlack = new List<int>();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                New = Convert.ToInt32(dt.Rows[i].Field<string>(3)) == 0 ? false : true;
                ForNow = Convert.ToInt32(dt.Rows[i].Field<string>(4)) == 0 ? false : true;
                productsTable.Rows.Add(Convert.ToInt32(dt.Rows[i].Field<string>(0)), dt.Rows[i].Field<string>(1), dt.Rows[i].Field<string>(2), New, ForNow);

                if (dt.Rows[i].Field<string>(5) == "1")
                    toBeBlack.Add(i);
            }

            foreach(int i in toBeBlack)
            {
                productsTable.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                productsTable.Rows[i].DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void Products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                productsTable.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int ID;
            string A_NAME, H_NAME, cmd;
            int NEW, FORNOW;

            API_CLASS api = new API_CLASS();

            DataGridViewRow row;

            foreach (int rowID in modifiedRows)
            {
                if (deletedRows.Contains(rowID))
                    continue;

                row = productsTable.Rows[rowID];

                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        ID = Convert.ToInt32(row.Cells[0].Value);
                        A_NAME = row.Cells[1].Value.ToString();
                        H_NAME = row.Cells[2].Value.ToString();

                        NEW = row.Cells[3].Value.ToString() == "True" ? 1 : 0;
                        FORNOW = row.Cells[4].Value.ToString() == "True" ? 1 : 0;

                        cmd = "UPDATE products SET A_NAME='" + MySqlHelper.EscapeString(A_NAME) + "', H_NAME='" + MySqlHelper.EscapeString(H_NAME) + "', NEW="+ NEW + ", FORNOW="+ FORNOW+ " WHERE ID="+ID;
                        api.API_QUERY_EXECUTE(cmd);
                        continue;
                    }
                }
                else
                {
                    if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        A_NAME = row.Cells[1].Value.ToString();
                        H_NAME = row.Cells[2].Value.ToString();

                        NEW = Convert.ToBoolean(row.Cells[3].Value) ? 1 : 0;
                        FORNOW = Convert.ToBoolean(row.Cells[4].Value) ? 1 : 0;

                        string theJSON = "{\"arabic\":null, \"hebrew\": null}";
                        cmd = "INSERT INTO products (A_NAME, H_NAME, NEW, FORNOW, NOTE, DISCOUNT_NAME) VALUES ('"+ MySqlHelper.EscapeString(A_NAME) +"', '" + MySqlHelper.EscapeString(H_NAME) +"', "+NEW+", "+FORNOW+ ", '"+theJSON+"', '"+theJSON+"')";
                        api.API_QUERY_EXECUTE(cmd);
                        continue;
                    }
                }
            }
            this.Close();
            Products prdcts = new Products();
            prdcts.Show();
            BiggerMessage.Show("تم حفظ جميع التغييرات", 1);
        }

        private void productsTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 3 || e.ColumnIndex == 4) && e.RowIndex != -1)
                if (!modifiedRows.Contains(e.RowIndex))
                    modifiedRows.Add(e.RowIndex);
        }

        private void productsTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3 && e.ColumnIndex != 4 && e.RowIndex != -1)
                if (!modifiedRows.Contains(e.RowIndex))
                    modifiedRows.Add(e.RowIndex);
        }

        private void productsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.RowIndex != productsTable.NewRowIndex)
            {
                if (productsTable.Rows[e.RowIndex].Cells[0].Value == null) 
                    return;

            }
        }

        private void Products_Load(object sender, EventArgs e)
        {
            productsTable.DoubleBuffered(true);
        }
    }
}
