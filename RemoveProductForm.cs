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
    public partial class RemoveProductForm : Form
    {
        private int RowID;
        private Products sourceForm;
        private int ProductID;
        public RemoveProductForm(Products tmp_sourceForm, int tmpRowID, int tmpProductID)
        {
            InitializeComponent();
            this.RowID = tmpRowID;
            this.sourceForm = tmp_sourceForm;
            this.ProductID = tmpProductID;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            this.sourceForm.productsTable.Rows[RowID].DefaultCellStyle.BackColor = Color.Black;
            this.sourceForm.productsTable.Rows[RowID].DefaultCellStyle.ForeColor = Color.White;
            this.sourceForm.deletedRows.Add(RowID);
            new API_CLASS().API_QUERY_EXECUTE("UPDATE products SET REMOVED=1 WHERE ID=" + this.ProductID);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.sourceForm.productsTable.Rows[RowID].DefaultCellStyle.BackColor = Color.White;
            this.sourceForm.productsTable.Rows[RowID].DefaultCellStyle.ForeColor = Color.Black;
            this.sourceForm.deletedRows.Remove(RowID);
            new API_CLASS().API_QUERY_EXECUTE("UPDATE products SET REMOVED=0 WHERE ID=" + this.ProductID);
            this.Close();
        }
    }
}
