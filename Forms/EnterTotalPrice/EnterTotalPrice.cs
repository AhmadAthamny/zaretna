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
    public partial class EnterTotalPrice : Form
    {
        private OrderInfo SourceForm;
        private float theFinalPrice;
        private int sourceValue = 0;
        public EnterTotalPrice(OrderInfo tmp_src, int source = 0)
        {
            InitializeComponent();
            SourceForm = tmp_src;
            this.theFinalPrice = this.SourceForm.TotalFinalPrice;
            this.sourceValue = source;

            if(this.theFinalPrice != -1)
                this.finalPrice.Text = this.theFinalPrice.ToString("F2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(float.TryParse(finalPrice.Text, out theFinalPrice))
            {
                theFinalPrice = float.Parse(theFinalPrice.ToString("F2"));
                this.SourceForm.SetTotalPrice(theFinalPrice);
                if (this.sourceValue == 0)
                {
                    this.SourceForm.SetOrderAsDelivered();
                }
                else if (this.sourceValue == 1)
                {
                    new API_CLASS().API_QUERY_EXECUTE("UPDATE orders SET TOTAL_PRICE=" + theFinalPrice + " WHERE ID=" + this.SourceForm.OrderID);
                    System.Diagnostics.Process.Start(String.Format("https://zaretna.co.il/ss/aaa.php?total={0}&order={1}&key=zar12341", theFinalPrice, this.SourceForm.OrderID));
                }
                else if (this.sourceValue == 2)
                {
                    new API_CLASS().API_QUERY_EXECUTE("UPDATE orders SET TOTAL_PRICE=" + theFinalPrice + " WHERE ID=" + this.SourceForm.OrderID);                    
                }
                this.Close();
            }
            else
            {
                BiggerMessage.Show("מחיר לא חוקי");
            }
        }
    }
}
