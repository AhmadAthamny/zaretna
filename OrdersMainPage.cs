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
    public partial class OrdersMainPage : Form
    {
        public OrdersMainPage()
        {
            InitializeComponent();
        }

        private void ProductImages_Click(object sender, EventArgs e)
        {
            //if (Application.OpenForms.OfType<OrdersForm>().Count() == 1)
            //  Application.OpenForms.OfType<OrdersForm>().First().Close();

            // OrdersForm oFrm = new OrdersForm();
            //oFrm.Show();

            if (Application.OpenForms.OfType<NewOrdersForm>().Count() == 1)
                Application.OpenForms.OfType<NewOrdersForm>().First().Close();

            NewOrdersForm oFrm = new NewOrdersForm();
             oFrm.Show();

            //names nms = new names();
            //nms.Show();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<TotalOrdersForm>().Count() == 1)
                Application.OpenForms.OfType<TotalOrdersForm>().First().Close();

            TotalOrdersForm tOrdForm = new TotalOrdersForm();
            tOrdForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<deliveryRegions>().Count() == 1)
                Application.OpenForms.OfType<deliveryRegions>().First().Close();

            deliveryRegions dlv = new deliveryRegions();
            dlv.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<customersList>().Count() == 1)
                Application.OpenForms.OfType<customersList>().First().Close();

            customersList cList = new customersList();
            cList.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            salesForm SFRM = new salesForm();
            SFRM.ShowDialog(this);
        }
    }
}
