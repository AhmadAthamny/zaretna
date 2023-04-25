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
    public partial class categories : Form
    {

        private Color OrangeColor = Color.FromArgb(255, 168, 65);
        private Color GreenColor = Color.FromArgb(101, 255, 131);
        private Color RedColor = Color.FromArgb(255, 45, 45);
        public categories()
        {
            InitializeComponent();
            SetupCategoriesTable();
            FillDataGridView();
        }
        public void SetupCategoriesTable()
        {
            categoriesTable.Columns.Add("ID", "رقم الفئة");
            categoriesTable.Columns.Add("ARABIC_NAME", "الإسم بالعربية");
            categoriesTable.Columns.Add("HEBREW_NAME", "الإسم بالعبرية");
            categoriesTable.Columns.Add("STATUS", "الحالة");

            categoriesTable.Columns[0].FillWeight = 15;
            categoriesTable.Columns[1].FillWeight = 35;
            categoriesTable.Columns[2].FillWeight = 35;
            categoriesTable.Columns[3].FillWeight = 15;

            categoriesTable.Columns[0].ReadOnly = true;
            categoriesTable.Columns[3].ReadOnly = true;
        }

        public void FillDataGridView()
        {
            API_CLASS api = new API_CLASS();
            string cmd = "SELECT ID, A_NAME, H_NAME, STATUS FROM categories";
            DataTable dt = api.API_QUERY(cmd);

            if (dt.Rows.Count > 0)
            {
                int CatID;
                string A_Name, H_Name;
                bool Status;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CatID = Convert.ToInt32(dt.Rows[i].Field<string>(0));
                    A_Name = dt.Rows[i].Field<string>(1);
                    H_Name = dt.Rows[i].Field<string>(2);
                    Status = dt.Rows[i].Field<string>(3) == "0" ? false : true;
                    categoriesTable.Rows.Add(CatID.ToString(), A_Name, H_Name, null);

                    if (!Status) categoriesTable.Rows[i].Cells[3].Style.BackColor = OrangeColor;
                    else categoriesTable.Rows[i].Cells[3].Style.BackColor = GreenColor;
                }
                categoriesTable.Rows[dt.Rows.Count].Cells[3].Style.BackColor = OrangeColor;

                this.categoriesTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.categoriesTable_CellClick);
                this.categoriesTable.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.categoriesTable_RowsAdded);
            }
        }
        private void categoriesTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == categoriesTable.Columns[3].Index && e.RowIndex != -1)
            {
                UpdateCellBackColor(categoriesTable.Rows[e.RowIndex].Cells[categoriesTable.Columns[3].Index]);
                categoriesTable.ClearSelection();
            }
        }
        private void UpdateCellBackColor(DataGridViewCell cell)
        {
            Color nextcolor = GetCellNextColor(cell);
            cell.Style.BackColor = nextcolor;
            cell.Style.SelectionBackColor = nextcolor;
        }
        private Color GetCellNextColor(DataGridViewCell cell)
        {

            if (cell.Style.BackColor == GreenColor)
                return OrangeColor;

            else if (cell.Style.BackColor == OrangeColor)
                return RedColor;

            else return GreenColor;
        }
        private Color FromStatusToColor(int status)
        {
            // Visible to public
            if (status == 0)
                return GreenColor;

            // Keep category but make it invisible.
            else if (status == 1)
                return OrangeColor;

            // Remove category upon saving.
            else
                return RedColor;
        }
        private int FromColorToStatusID(DataGridViewCell cell)
        {
            // Visible to public
            if (cell.Style.BackColor == GreenColor)
                return 1;

            // Keep category but make it invisible.
            else if (cell.Style.BackColor == OrangeColor)
                return 0;

            // Remove category upon saving.
            else
                return -1;
        }

        private void categories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                categoriesTable.ClearSelection();
        }

        private void categoriesTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (categoriesTable.Columns.Contains("STATUS"))
            {
                categoriesTable.Rows[e.RowIndex].Cells[3].Style.BackColor = OrangeColor;
                categoriesTable.Rows[e.RowIndex].Cells[3].Style.SelectionBackColor = OrangeColor;
                categoriesTable.Rows[e.RowIndex].Cells[0].Style.SelectionBackColor = Color.White;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            API_CLASS api = new API_CLASS();
            int ID;
            string A_NAME, H_NAME, cmd;
            int STATUS;
            foreach (DataGridViewRow row in categoriesTable.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        ID = Convert.ToInt32(row.Cells[0].Value);
                        A_NAME = row.Cells[1].Value.ToString();
                        H_NAME = row.Cells[2].Value.ToString();
                        STATUS = FromColorToStatusID(row.Cells[3]);

                        if (STATUS != -1)
                        {
                            cmd = "UPDATE categories SET A_NAME='" + MySqlHelper.EscapeString(A_NAME) + "', H_NAME='" + MySqlHelper.EscapeString(H_NAME) + "', STATUS=" + STATUS + " WHERE ID=" + ID + "";
                            api.API_QUERY_EXECUTE(cmd);
                            continue;
                        }
                    }
                }
                else
                {
                    if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                    {
                        A_NAME = row.Cells[1].Value.ToString();
                        H_NAME = row.Cells[2].Value.ToString();
                        STATUS = FromColorToStatusID(row.Cells[3]);
                        if (STATUS != -1)
                        {
                            cmd = "INSERT INTO categories (A_NAME, H_NAME, STATUS) VALUES ('" + MySqlHelper.EscapeString(A_NAME) + "', '" + MySqlHelper.EscapeString(H_NAME) + "', " + STATUS + ")";
                            api.API_QUERY_EXECUTE(cmd);
                            continue;
                        }
                    }
                }
            }
            this.Close();
            categories ctgrs = new categories();
            ctgrs.Show();
            BiggerMessage.Show("تم حفظ جميع التغييرات", 1);
        }

        private void categories_Load(object sender, EventArgs e)
        {
            categoriesTable.ClearSelection();
            categoriesTable.DoubleBuffered(true);
        }
    }
}
