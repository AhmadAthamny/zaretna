using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace ZaretnaPanel
{
    public partial class Form1 : Form
    {
        public bool didPassword = false;
        public Form1()
        {
            InitializeComponent();
            
            ConfirmPass cfp = new ConfirmPass(this);
            cfp.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (didPassword)
            {
                categories ctgrs = new categories();
                ctgrs.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (didPassword)
            {
                ProductsMainPage prdctsMP = new ProductsMainPage();
                prdctsMP.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (didPassword)
            {
                OrdersMainPage ordMainPage = new OrdersMainPage();
                ordMainPage.Show();
            }
        }
    }
}
