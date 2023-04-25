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
    public partial class RemoveMotsatProduct : Form
    {
        private int RowIndex;
        private MostatForm SourceForm;
        public RemoveMotsatProduct(MostatForm mostatForm, int tmpRowIndex)
        {
            InitializeComponent();
            this.RowIndex = tmpRowIndex;
            this.SourceForm = mostatForm;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            SourceForm.productsTable.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Black;
            SourceForm.productsTable.Rows[RowIndex].DefaultCellStyle.ForeColor = Color.Gray;

            if (!SourceForm.DeletedRows.Contains(RowIndex))
                SourceForm.DeletedRows.Add(RowIndex);

            BiggerMessage.Show("قم بحفظ التغييرات من أجل حذف المنتج");
            this.Close();
        }
    }
}
