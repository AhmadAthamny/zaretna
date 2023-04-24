using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace ZaretnaPanel
{
    public partial class OrdersForm : Form
    {
        public List<int> modifiedRows = new List<int>();
        public List<int> deletedRows = new List<int>();
        public List<int> hideOnlyRows = new List<int>();
        private int viewingOrderID = -1;
        private LoadingForm loadingBoard;
        private OrderInfo orderDetailsBoard;
        private bool isClosed = false;
        PrivateFontCollection PFC = new PrivateFontCollection();
        public OrdersForm()
        {
            InitializeComponent();
            initNewFont();
            SetupCategoriesTable();
            FillDataGridView();
        }
        public void SetupCategoriesTable()
        {
            ordersTable.Columns.Add("OrderID", "#");
            ordersTable.Columns.Add("FullName", "الإسم الكامل");
            ordersTable.Columns.Add("phoneNumber", "رقم الهاتف");
            ordersTable.Columns.Add("paid", "الدفع");
            ordersTable.Columns.Add("timebuy", "وقت الطلبية");

            ordersTable.Columns[0].FillWeight = 15;
            ordersTable.Columns[1].FillWeight = 28;
            ordersTable.Columns[2].FillWeight = 25;
            ordersTable.Columns[3].FillWeight = 15;
            ordersTable.Columns[4].FillWeight = 20;
        }

        public void FillDataGridView()
        {
            ordersTable.Rows.Clear();
            string cmd = "SELECT ID, F_NAME,L_NAME, PHONE_NUMBER, PAID, BUY_TIME, DID_DONE, TOTAL_PRICE FROM orders WHERE STATUS < 2 ORDER BY ID ASC";
            DataTable dt = new API_CLASS().API_QUERY(cmd);

            int OrderID;
            string First_Name, Last_Name, PhoneNumber;
            bool Paid, IsPrepared;
            foreach(DataRow row in dt.Rows)
            {
                OrderID = Convert.ToInt32(row.Field<string>(0));
                First_Name = row.Field<string>(1);
                Last_Name = row.Field<string>(2);
                PhoneNumber = row.Field<string>(3);
                Paid = Convert.ToInt32(row.Field<string>(4)) == 0 ? false : true;
                IsPrepared = float.Parse(row.Field<string>(7)) == -1 ? false : true;
                ordersTable.Rows.Add(OrderID, First_Name + " " + Last_Name, PhoneNumber, Paid ? "أشراي" : "عند الإستلام", row.Field<string>(5));

                if(IsPrepared)
                {
                    ordersTable.Rows[ordersTable.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(237, 221, 147);
                }

                if (Paid)
                {
                    ordersTable.Rows[ordersTable.Rows.Count-1].Cells[3].Style.ForeColor = Color.White;
                    ordersTable.Rows[ordersTable.Rows.Count-1].Cells[3].Style.BackColor = Color.Red;
                }
            }
            ordersTable.ClearSelection();
        }

        private void ordersTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                if (e.RowIndex != -1 && e.RowIndex != ordersTable.NewRowIndex)
                {
                    this.viewingOrderID = Convert.ToInt32(ordersTable.Rows[e.RowIndex].Cells[0].Value);
                    loadingBoard = new LoadingForm();
                    this.loadingBoard.label1.Font = new Font(PFC.Families[0], label1.Font.Size);
                    this.loadingBoard.label1.Text = "הזמנה מס' " + this.viewingOrderID;
                    backgroundWorker1.RunWorkerAsync();
                    loadingBoard.Show();
                }
            }
        }

        private void viewOrder_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                int value;
                if (int.TryParse(txtOrderID.Text, out value))
                {
                    this.viewingOrderID = value;
                    this.loadingBoard = new LoadingForm();
                    backgroundWorker1.RunWorkerAsync();
                    this.loadingBoard.Show();
                }
                else
                    BiggerMessage.Show("رقم غير سليم (يجب ان يحوي أرقام فقط!)");
            }
        }

        private void txtViewOrders_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<latestOrders>().Count() == 1)
                Application.OpenForms.OfType<latestOrders>().First().Close();
            latestOrders lord = new latestOrders();
            lord.Show();
        }

        public void RemoveOrderRow(int OrderID)
        {
            foreach(DataGridViewRow row in ordersTable.Rows)
            {
                if(!row.IsNewRow && row.Index != -1)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) == OrderID)
                        ordersTable.Rows.RemoveAt(row.Index);
                }
            }
        }

        public void ColorizeOrderBackcolor(int OrderID)
        {
            foreach(DataGridViewRow row in ordersTable.Rows)
            {
                if(!row.IsNewRow && row.Index != -1)
                {
                    if(Convert.ToInt32(row.Cells[0].Value.ToString()) == OrderID)
                        ordersTable.Rows[row.Index].DefaultCellStyle.BackColor = Color.FromArgb(237, 221, 147);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<preparedOrders>().Count() == 1)
                Application.OpenForms.OfType<preparedOrders>().First().Close();
            preparedOrders PREORD = new preparedOrders();
            PREORD.Show();
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            ordersTable.ClearSelection();
            ordersTable.DoubleBuffered(true);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.viewingOrderID != -1)
            {
                this.orderDetailsBoard = new OrderInfo(this.viewingOrderID, null);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingBoard.Close();
            if(!this.isClosed)
                if (!this.orderDetailsBoard.InvalidOrder)
                    this.orderDetailsBoard.ShowDialog(this);
        }

        private void OrdersForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                DataGridViewRow row = null;
                if (e.KeyData == Keys.Space || e.KeyData == Keys.Enter)
                {
                    e.Handled = true;
                    if (ordersTable.SelectedRows.Count > 0)
                        row = ordersTable.SelectedRows[0];

                    if (row != null)
                    {
                        if (row.Index != -1 && !row.IsNewRow)
                        {
                            this.viewingOrderID = Convert.ToInt32(ordersTable.Rows[row.Index].Cells[0].Value);
                            loadingBoard = new LoadingForm();
                            this.loadingBoard.label1.Text = "הזמנה מס' " + this.viewingOrderID;
                            backgroundWorker1.RunWorkerAsync();
                            loadingBoard.Show();
                        }
                    }
                }
            }
        }

        private void OrdersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isClosed = true;
        }
        private void initNewFont()
        {
            //Select your font from the resources.
            //My font here is "Digireu.ttf"
            int fontLength = Properties.Resources.VarelaRound_Regular.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.VarelaRound_Regular;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            PFC.AddMemoryFont(data, fontLength);
        }
    }
}
