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
    public partial class latestOrders : Form
    {
        /* saved data */
        public DateTime FromDatePickerValue = DateTime.Now;
        public DateTime ToDatePickerValue = DateTime.Now;

        public string FirstName = "";
        public string LastName = "";
        public string PhoneNumber = "";
        public string EmailAddress = "";
        public int CityID = -1;
        public int MinimumPrice = 0;
        public int MaxPrice = 0;
        public bool DeliveredOnly = false;

        /* ** */

        public string OrdersQuery;
        public latestOrders()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("ID", "#");
            dataGridView1.Columns.Add("NAME", "الإسم الكامل");
            dataGridView1.Columns.Add("total", "السعر النهائي");
            dataGridView1.Columns.Add("buytime", "وقت الشراء");

            dataGridView1.Columns[0].FillWeight = 20;
            dataGridView1.Columns[1].FillWeight = 30;
            dataGridView1.Columns[2].FillWeight = 20;
            dataGridView1.Columns[3].FillWeight = 30;

            dataGridView1.ReadOnly = true;

            FillData();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                int orderid = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                OrderInfo lord = new OrderInfo(orderid, null);
                if (!lord.InvalidOrder)
                    lord.Show();
            }
        }

        public void FillData()
        {
            dataGridView1.Rows.Clear();

            /* ** building query ** */

            string FROM_DATE = toEnglishNumber(FromDatePickerValue.Date.ToString("yyyy-MM-dd"));
            string TO_DATE = toEnglishNumber(ToDatePickerValue.Date.ToString("yyyy-MM-dd"));

            OrdersQuery = string.Format("SELECT ID, F_NAME, L_NAME, TOTAL_PRICE, BUY_TIME FROM orders WHERE STATUS!=4 AND BUY_TIME>='{0}' AND BUY_TIME<='{1}'", FROM_DATE + " 00:00:00", TO_DATE + " 23:59:59");

            if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrWhiteSpace(this.FirstName))
                OrdersQuery += string.Format(" AND F_NAME LIKE '%25{0}%25'", this.FirstName);
            else
                this.FirstName = "";

            if (!string.IsNullOrEmpty(this.LastName) && !string.IsNullOrWhiteSpace(this.LastName))
                OrdersQuery += string.Format(" AND L_NAME LIKE '%25{0}%25'", this.LastName);
            else
                this.LastName = "";

            if (!string.IsNullOrEmpty(this.PhoneNumber) && !string.IsNullOrWhiteSpace(this.PhoneNumber))
                OrdersQuery += string.Format(" AND PHONE_NUMBER LIKE '%25{0}%25'", this.PhoneNumber);
            else
                this.PhoneNumber = "";

            if (!string.IsNullOrEmpty(this.EmailAddress) && !string.IsNullOrWhiteSpace(this.EmailAddress))
                OrdersQuery += string.Format(" AND EMAIL LIKE '%25{0}%25'", this.EmailAddress);
            else
                this.EmailAddress = "";

            if (this.CityID != -1)
                OrdersQuery += string.Format(" AND CITY={0}", this.CityID.ToString());

            if (this.MinimumPrice != 0)
                OrdersQuery += string.Format(" AND TOTAL_PRICE>={0}", this.MinimumPrice);

            if (this.MaxPrice != 0)
                OrdersQuery += string.Format(" AND TOTAL_PRICE<={0}", this.MaxPrice);

            if (this.DeliveredOnly)
                OrdersQuery += string.Format(" AND STATUS=2");

            /* ** end of building query ** */

            DataTable dt = new API_CLASS().API_QUERY(this.OrdersQuery);

            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row.Field<string>(0), row.Field<string>(1) + " " + row.Field<string>(2), row.Field<string>(3), row.Field<string>(4));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillData();
        }
        public void RemoveOrderRow(int OrderID)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow && row.Index != -1)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) == OrderID)
                        dataGridView1.Rows.RemoveAt(row.Index);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void latestOrders_Load(object sender, EventArgs e)
        {
            dataGridView1.DoubleBuffered(true);
        }
    }
}
