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
    public partial class RemoveOrderProduct : Form
    {
        private int productID;
        public OrderInfo sourceForm;
        public RemoveOrderProduct(int tmp_productID, OrderInfo tmp_SourceForm)
        {
            InitializeComponent();
            this.productID = tmp_productID;
            this.sourceForm = tmp_SourceForm;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            new API_CLASS().API_QUERY("DELETE FROM orders_products WHERE ID=" + productID);
            this.sourceForm.RemoveListProductID(this.productID);
            BiggerMessage.Show("لقد تمت إزالة المنتج", 1);
            this.Close();
        }
    }
}
