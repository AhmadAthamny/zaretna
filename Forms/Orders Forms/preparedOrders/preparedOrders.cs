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
    public partial class preparedOrders : Form
    {
        public preparedOrders()
        {
            InitializeComponent(); 
            dataGridView1.Columns.Add("ID", "#");
            dataGridView1.Columns.Add("NAME", "שם לקוח");
            dataGridView1.Columns.Add("total", "מחיר סופי");
            dataGridView1.Columns.Add("city", "איזור משלוח");
            dataGridView1.Columns.Add("buytime", "זמן הזמנה");

            dataGridView1.Columns[0].FillWeight = 20;
            dataGridView1.Columns[1].FillWeight = 30;
            dataGridView1.Columns[2].FillWeight = 30;
            dataGridView1.Columns[3].FillWeight = 30;
            dataGridView1.Columns[4].FillWeight = 30;

            dataGridView1.ReadOnly = true;

            FillData();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int orderid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                OrderInfo lord = new OrderInfo(orderid, null);
                if (!lord.InvalidOrder)
                    lord.Show();
            }
        }

        private void FillData()
        {
            dataGridView1.Rows.Clear();
            DataTable dt = new API_CLASS().API_QUERY("SELECT a.ID, a.F_NAME, a.L_NAME, a.TOTAL_PRICE, b.H_NAME, a.BUY_TIME FROM orders a INNER JOIN regions b on a.CITY = b.ID WHERE a.TOTAL_PRICE!=-1 AND a.STATUS=0 ORDER BY a.CITY ASC");

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row.Field<string>(0), row.Field<string>(1) + " " + row.Field<string>(2), row.Field<string>(3), row.Field<string>(4), row.Field<string>(5));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<WriteEmails>().Count() == 1)
                Application.OpenForms.OfType<WriteEmails>().First().Close();
            WriteEmails WRE = new WriteEmails();
            WRE.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillData();
        }

        private void preparedOrders_Load(object sender, EventArgs e)
        {
            dataGridView1.DoubleBuffered(true);
        }
    }
}
