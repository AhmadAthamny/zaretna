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
    public partial class BiggerMessageBox : Form
    {
        public BiggerMessageBox(string content, int type, string title = "הערה")
        {
            InitializeComponent();
            textBox1.Text = content;
            this.Text = title;
            if (type == 0)
                msgType.Image = Properties.Resources.fail;
            else if (type == 1)
                msgType.Image = Properties.Resources.success;
            else if (type == 2)
                msgType.Image = Properties.Resources.msgInfo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BiggerMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            else if (e.KeyCode == Keys.Enter)
                this.Close();
        }
    }
}

