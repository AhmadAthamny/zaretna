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
    public partial class customerSelection : Form
    {
        private int OrderID;
        private bool DoesContianOrder;
        public customerSelection(int orderid, bool doesContain)
        {
            InitializeComponent();
            this.OrderID = orderid;
            this.DoesContianOrder = doesContain;
            if (this.DoesContianOrder)
                this.btnUpdateOrder.Text = "סמן שלא מוכן";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OrderInfo oInfo = new OrderInfo(this.OrderID, null);
            oInfo.ShowDialog(this);
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (!DoesContianOrder)
            {
                API_CLASS api = new API_CLASS();
                api.API_QUERY_EXECUTE("UPDATE orders SET CALLED=1 WHERE ID=" + this.OrderID);
                this.Close();

                if (Application.OpenForms.OfType<customersList>().Count() == 1)
                {
                    customersList cList = Application.OpenForms.OfType<customersList>().First();
                    foreach (DataGridViewRow row in cList.dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (Convert.ToInt32(row.Cells[0].Value) == this.OrderID)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(130, 168, 255);
                            cList.dataGridView1.ClearSelection();

                            cList.calledOrders.Push(this.OrderID);
                            break;
                        }

                    }
                }
            }
            else
            {
                API_CLASS api = new API_CLASS();
                api.API_QUERY_EXECUTE("UPDATE orders SET CALLED=0 WHERE ID=" + this.OrderID);
                this.Close();

                if (Application.OpenForms.OfType<customersList>().Count() == 1)
                {
                    customersList cList = Application.OpenForms.OfType<customersList>().First();
                    foreach (DataGridViewRow row in cList.dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (Convert.ToInt32(row.Cells[0].Value) == this.OrderID)
                        {
                            row.DefaultCellStyle.BackColor = Color.White;
                            cList.dataGridView1.ClearSelection();

                            cList.RemoveFromStack(this.OrderID);
                            break;
                        }

                    }
                }
            }
        }
    }
}
