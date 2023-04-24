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
    public partial class PaymentInfo : Form
    {
        private int OrderID;
        public bool CloseForm = false;
        public PaymentInfo(int tmpOrderID)
        {
            InitializeComponent();
            OrderID = tmpOrderID;

            DataTable dt = new API_CLASS().API_QUERY("SELECT * FROM pay_card WHERE ORDER_ID=" + this.OrderID);
            if(dt.Rows.Count > 0)
            {
                txtCardNum.Text = dt.Rows[0].Field<string>(2);
                txtCardMonth.Text = dt.Rows[0].Field<string>(3);
                txtCardYear.Text = dt.Rows[0].Field<string>(4);
                txtCVV.Text = dt.Rows[0].Field<string>(5);
                txtBuyerID.Text = dt.Rows[0].Field<string>(6);
            }
            else
            {
                BiggerMessage.Show("אין פרטי אשראי");
                this.Close();
                CloseForm = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult rslt = MessageBox.Show("האם אתה רוצה למחק פרטים?", "מחק פרטים", MessageBoxButtons.YesNo);
            if (rslt == DialogResult.Yes)
            {
                API_CLASS api = new API_CLASS();
                api.API_QUERY("DELETE FROM pay_card WHERE ORDER_ID=" + this.OrderID);
                BiggerMessage.Show("פרטים נמחקו בהצלחה", 1);
                this.Close();
            }
        }
    }
}
