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
    public partial class customersList : Form
    {
        public Stack<int> calledOrders = new Stack<int>();
        public customersList()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("ID", "#");
            dataGridView1.Columns.Add("NAME", "שם לקוח");
            dataGridView1.Columns.Add("phone", "מספר טלפון");
            dataGridView1.Columns.Add("total", "מחיר סופי");
            dataGridView1.Columns.Add("city", "איזור משלוח");
            dataGridView1.Columns.Add("buytime", "זמן הזמנה");

            dataGridView1.Columns[0].FillWeight = 9;
            dataGridView1.Columns[1].FillWeight = 27;
            dataGridView1.Columns[2].FillWeight = 19;
            dataGridView1.Columns[3].FillWeight = 10;
            dataGridView1.Columns[4].FillWeight = 15;
            dataGridView1.Columns[5].FillWeight = 19;

            dataGridView1.ReadOnly = true;

            FillData();
        }
        private string toEnglishNumber(string input)
        {
            string EnglishNumbers = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }
        public void FillData()
        {
            dataGridView1.Rows.Clear();

            calledOrders.Clear();

            /* ** building query ** */

            string minDate = toEnglishNumber(DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd")) + " 00:00:00";

            string sQuery = string.Format("SELECT A.ID, A.F_NAME, A.L_NAME, A.TOTAL_PRICE, B.H_NAME, A.BUY_TIME, A.CALLED, A.PHONE_NUMBER FROM orders A LEFT JOIN regions B ON A.CITY = B.ID WHERE A.STATUS!=4 AND A.BUY_TIME>='{0}'", minDate);
            DataTable dt = new API_CLASS().API_QUERY(sQuery);

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row.Field<string>(0), row.Field<string>(1) + " " + row.Field<string>(2), row.Field<string>(7), row.Field<string>(3) == "-1" ? "-" : row.Field<string>(3)+ " ₪", row.Field<string>(4), row.Field<string>(5));
                if (Convert.ToInt32(row.Field<string>(6)) == 1)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(130, 168, 255);
                    calledOrders.Push(Convert.ToInt32(row.Field<string>(0)));
                }
            }
            dataGridView1.ClearSelection();
        }

        private void customersList_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.DoubleBuffered(true);
        }

        public void RemoveFromStack(int id)
        {
            Stack<int> tmpStack = new Stack<int>();

            // moving all objects to a clear stack, except of the one we looking for
            while (calledOrders.Count > 0)
            {
                int popped = calledOrders.Pop();
                if (popped != id) tmpStack.Push(popped);

                // if we found it, then no need to keep looking for it so.. break the loop
                else
                    break;
            }
            while (tmpStack.Count > 0)
                calledOrders.Push(tmpStack.Pop());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int orderid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                customerSelection cSelection = new customerSelection(orderid, calledOrders.Contains(orderid));
                cSelection.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillData();
        }
    }
}
