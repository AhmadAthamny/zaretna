using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaretnaPanel
{
    public partial class addNewProductControl : UserControl
    {
        public addNewProductControl()
        {
            InitializeComponent();
            transparentPic.Parent = pictureBox1;
            transparentPic.BackColor = Color.Transparent;
        }

        private void existingProduct_Click(object sender, EventArgs e)
        {
            addProductToCategory APTC = new addProductToCategory();
            APTC.ShowDialog(this.Parent.Parent);
        }
    }
}
