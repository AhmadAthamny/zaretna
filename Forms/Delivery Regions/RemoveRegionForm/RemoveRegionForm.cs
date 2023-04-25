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
    public partial class RemoveRegionForm : Form
    {
        private int RegionID, RowID;
        private deliveryRegions SourceForm;
        public RemoveRegionForm(deliveryRegions tmpSource, int tmpRegionID, int tmpRowID)
        {
            InitializeComponent();
            this.RegionID = tmpRegionID;
            this.RowID = tmpRowID;
            this.SourceForm = tmpSource;
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            BiggerMessage.Show("لقد تم تعطيل خيار حذف المناطق لأسباب تقنية. الرجاء التواصل مع أحمد اذا أصررت على حذف منطقة");
            return;

            /*this.SourceForm.deletedRegions.Add(RegionID);
            this.SourceForm.regionsTable.Rows[RowID].DefaultCellStyle.BackColor = Color.Black;
            this.SourceForm.regionsTable.Rows[RowID].DefaultCellStyle.ForeColor = Color.Gray;
            this.SourceForm.regionsTable.Rows[RowID].DefaultCellStyle.SelectionBackColor = Color.Black;
            this.SourceForm.regionsTable.Rows[RowID].DefaultCellStyle.SelectionForeColor = Color.Gray;
            this.Close();*/
        }
    }
}
