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
    public partial class NewOrdersForm : Form
    {
        public LoadingForm loadingOrderBoard = null;

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
        public string OrdersQuery = "n/a";

        public NewOrdersForm()
        {
            InitializeComponent();
            LoadOrders();
            reloadOrders.Enabled = true;

            this.MaximumSize = new Size(this.MaximumSize.Width, Screen.FromControl(this).Bounds.Height);
        }

        private void reloadOrders_Tick(object sender, EventArgs e)
        {
            if (this.OrdersQuery != "n/a")
                LoadOrders(OrdersQuery);
            else
                LoadOrders();
        }

        public void LoadOrders(string query = "SELECT A.*, B.H_NAME AS CITY_NAME FROM orders A LEFT JOIN regions B ON A.CITY = B.ID WHERE A.STATUS < 2 ORDER BY A.ID ASC")
        {
            foreach(Control ctrl in this.panel1.Controls)
            {
                if (!(ctrl is OrderControl)) continue;

                OrderControl octrl = ctrl as OrderControl;
                if (octrl.CalculatePriceWorker != null)
                    octrl.CalculatePriceWorker.CancelAsync();
            }
            this.panel1.Controls.Clear();

            DataTable dt2 = new DataTable();
            if ( this.OrdersQuery == "n/a")
            {
                //string query2 = "query here";
                //dt2 = new API_CLASS().API_QUERY(query2);
            }
            DataTable dt = new API_CLASS().API_QUERY(query);
            int lastY = 0;
            int index = 0;

            foreach (DataRow row in dt.Rows)
            {
                OrderControl octrl = new OrderControl(row, index);

                this.panel1.Controls.Add(octrl);
                octrl.Location = new Point(0, lastY);
                lastY += octrl.Height + 9;
                index++;

                octrl.RightToLeft = RightToLeft.No;
            }
            foreach(DataRow row in dt2.Rows)
            {
                Console.Write("aa");
            }
            Label spacer = new Label();
            spacer.Height = 1;
            spacer.Location = new Point(0, lastY);
            this.panel1.Controls.Add(spacer);
        }
        public void RemoveOrderControl(OrderControl removeCtrl, int index)
        {
            this.panel1.Controls.Remove(removeCtrl);

            foreach (Control ctrl in this.panel1.Controls)
            {
                if (!(ctrl is OrderControl)) continue;
                OrderControl octrl = ctrl as OrderControl;
                if (octrl.indexInList > index)
                {
                    octrl.Location = new Point(0, octrl.Location.Y - (45 + 9)); // 45 is the control height, and 9 is the space!
                    octrl.indexInList--;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchText.Text))
            {
                txtSearchText.Text = "";
                ResetAdvanceSearch();
                this.reloadOrders.Enabled = true;
                LoadOrders();
            }
            else
            {
                string searchText = txtSearchText.Text.Trim().Replace("#", "");
                txtSearchText.Text = searchText;
                int number;
                if (int.TryParse(searchText, out number))
                {
                    LoadOrders("SELECT A.*, B.H_NAME AS CITY_NAME FROM orders A LEFT JOIN regions B ON A.CITY = B.ID WHERE A.STATUS!=4" +
                        " AND (A.PHONE_NUMBER LIKE '%25" + number + "%25'" +
                        " OR A.ID LIKE '%25" + number + "%25')" +
                        " ORDER BY A.ID ASC");
                }
                else
                {
                    string[] words = searchText.Split(' ');
                    string searchCondition = "";
                    if(words.Length == 1)
                    {
                        searchCondition = "A.F_NAME LIKE '%25" + words[0] + "%25' OR A.L_NAME LIKE '%25" + words[0] + "%25'";
                    }
                    else
                    {
                        for(int i = 0; i < words.Length; i++)
                        {
                            if (i > 0)
                                searchCondition += " OR ";
                            searchCondition += "A.F_NAME LIKE '%25" + words[i] + "%25' OR A.L_NAME LIKE '%25" + words[i] + "%25'";
                        }
                    }
                    LoadOrders("SELECT A.*, B.H_NAME AS CITY_NAME FROM orders A LEFT JOIN regions B ON A.CITY = B.ID WHERE A.STATUS!=4 AND (" +
                       searchCondition +
                       ")" +
                       " ORDER BY A.ID ASC");
                }
                this.reloadOrders.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<preparedOrders>().Count() == 1)
                Application.OpenForms.OfType<preparedOrders>().First().Close();

            preparedOrders PREORD = new preparedOrders();
            PREORD.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DatePickerForm DPF = new DatePickerForm(this);
            DPF.ShowDialog(this);
        }

        public void AdvancedSearch()
        {
            /* ** building query ** */

            string FROM_DATE = toEnglishNumber(FromDatePickerValue.Date.ToString("yyyy-MM-dd"));
            string TO_DATE = toEnglishNumber(ToDatePickerValue.Date.ToString("yyyy-MM-dd"));

            OrdersQuery = string.Format("SELECT A.*, B.H_NAME AS CITY_NAME FROM orders A LEFT JOIN regions B ON A.CITY = B.ID WHERE A.STATUS!=4 AND A.BUY_TIME>='{0}' AND A.BUY_TIME<='{1}'", FROM_DATE + " 00:00:00", TO_DATE + " 23:59:59");

            if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrWhiteSpace(this.FirstName))
                OrdersQuery += string.Format(" AND A.F_NAME LIKE '%25{0}%25'", this.FirstName);
            else
                this.FirstName = "";

            if (!string.IsNullOrEmpty(this.LastName) && !string.IsNullOrWhiteSpace(this.LastName))
                OrdersQuery += string.Format(" AND A.L_NAME LIKE '%25{0}%25'", this.LastName);
            else
                this.LastName = "";

            if (!string.IsNullOrEmpty(this.PhoneNumber) && !string.IsNullOrWhiteSpace(this.PhoneNumber))
                OrdersQuery += string.Format(" AND A.PHONE_NUMBER LIKE '%25{0}%25'", this.PhoneNumber);
            else
                this.PhoneNumber = "";

            if (!string.IsNullOrEmpty(this.EmailAddress) && !string.IsNullOrWhiteSpace(this.EmailAddress))
                OrdersQuery += string.Format(" AND A.EMAIL LIKE '%25{0}%25'", this.EmailAddress);
            else
                this.EmailAddress = "";

            if (this.CityID != -1)
                OrdersQuery += string.Format(" AND A.CITY={0}", this.CityID.ToString());

            if (this.MinimumPrice != 0)
                OrdersQuery += string.Format(" AND A.TOTAL_PRICE>={0}", this.MinimumPrice);

            if (this.MaxPrice != 0)
                OrdersQuery += string.Format(" AND A.TOTAL_PRICE<={0}", this.MaxPrice);

            if (this.DeliveredOnly)
                OrdersQuery += string.Format(" AND A.STATUS=2");

            /* ** end of building query ** */

            reloadOrders.Enabled = false;
            this.lblCancelSearch.Visible = true;
            this.LoadOrders(OrdersQuery);
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
        private void ResetAdvanceSearch()
        {
            FromDatePickerValue = DateTime.Now;
            ToDatePickerValue = DateTime.Now;

            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            EmailAddress = "";
            CityID = -1;
            MinimumPrice = 0;
            MaxPrice = 0;
            DeliveredOnly = false;

            OrdersQuery = "n/a";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ResetAdvanceSearch();
            this.txtSearchText.Text = "";
            this.lblCancelSearch.Visible = false;

            this.reloadOrders.Enabled = true;
            this.LoadOrders();
        }
    }   
}
