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
    public partial class ProductsMainPage : Form
    {
        public ProductsMainPage()
        {
            InitializeComponent();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            //Products prdcts = new Products();
            NewProductsForm NPDCTS = new NewProductsForm();
            NPDCTS.Show();
        }

        private void btnProduct_Prices_Click(object sender, EventArgs e)
        {
            ProductPrices prdctPrices = new ProductPrices();
            prdctPrices.Show();
        }

        private void btnProducts_Categories_Click(object sender, EventArgs e)
        {
            ProductsCategories prdctsCtgrs = new ProductsCategories();
            prdctsCtgrs.Show();
        }

        private void ProductImages_Click(object sender, EventArgs e)
        {
            ProductsImages prdctsImages = new ProductsImages();
            prdctsImages.Show();
        }

        private void btnMotsat_Click(object sender, EventArgs e)
        {
            MostatForm mst = new MostatForm();
            mst.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AutosoftPrices APR = new AutosoftPrices();
            APR.ShowDialog(this);
        }

        private void discountsButton_Click(object sender, EventArgs e)
        {
            TotalPriceDiscount TPD = new TotalPriceDiscount();
            TPD.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ProductsDiscounts>().Count() == 1)
                Application.OpenForms.OfType<ProductsDiscounts>().First().Close();

            ProductsDiscounts PDSC = new ProductsDiscounts();
            PDSC.ShowDialog(this);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            defineProducts DFPRD = new defineProducts();
            DFPRD.Show(this);
        }
    }
}
