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
    public partial class TotalPriceDiscount : Form
    {
        private string PicturePath = "n/a";

        public TotalPriceDiscount()
        {
            InitializeComponent();
            DataTable dt = new API_CLASS().API_QUERY("SELECT VALUE_INT FROM global_variables WHERE ID=3 OR ID=4 OR ID=5 OR ID=6 ORDER BY ID ASC");
            if (dt != null)
            {
                discountEnabled.Checked = Convert.ToInt32(dt.Rows[2].Field<string>(0)) == 1 ? true : false;
                showImageCB.Checked = Convert.ToInt32(dt.Rows[3].Field<string>(0)) == 1 ? true : false;
                discountPercentage.Value = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                minPrice.Value = Convert.ToInt32(dt.Rows[1].Field<string>(0));
                btnPricePic.Width = 300;
                thepicturebox.ImageLocation = "https://zaretna.co.il/pop-ups/sale.jpg";
                checkBox1_CheckedChanged(null, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.discountEnabled.Checked)
            {
                this.theGroupBox.Enabled = true;
            }
            else
                this.theGroupBox.Enabled = false;
        }

        private void btnPricePic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.jpg)|*.jpg";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Image bitmap = Image.FromFile(fileDialog.FileName);
                this.PicturePath = fileDialog.FileName;
                this.thepicturebox.Image = bitmap;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            API_CLASS api = new API_CLASS();
            api.API_QUERY_EXECUTE("UPDATE global_variables SET VALUE_INT=" + (discountEnabled.Checked == true ? 1 : 0) + " WHERE ID=5");
            api.API_QUERY_EXECUTE("UPDATE global_variables SET VALUE_INT=" + (showImageCB.Checked == true ? 1 : 0) + " WHERE ID=6");
            api.API_QUERY_EXECUTE("UPDATE global_variables SET VALUE_INT=" + discountPercentage.Value + " WHERE ID=3");
            api.API_QUERY_EXECUTE("UPDATE global_variables SET VALUE_INT=" + minPrice.Value + " WHERE ID=4");

            if (PicturePath != "n/a")
                api.API_UPLOAD_POP_UP(this.PicturePath, "/testtoast.jpg");

            MessageBox.Show("لقد تم حفظ حميع التغييرات");
        }
    }
}
