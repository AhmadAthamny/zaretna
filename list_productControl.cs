using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;


namespace ZaretnaPanel
{
    public partial class list_productControl : UserControl
    {
        private DataRow ProductData;
        public list_productControl(DataRow row)
        {
            InitializeComponent();
            this.ProductData = row;
            FillProductData();
        }
        private void FillProductData()
        {
            this.lblName.Text = this.ProductData.Field<string>("A_NAME") + "\r\n" + this.ProductData.Field<string>("H_NAME");
            this.lblPrice.Text = string.Format("₪{0}", this.ProductData.Field<string>("FINAL_PRICE"));
            this.lblUnit.Text = this.ProductData.Field<string>("UNIT_NAME");

            string imgVersion = this.ProductData.Field<string>("IMG_VERSION");
            if (int.Parse(imgVersion) > 0)
            {
                if (File.Exists(returnProductImagePath()))
                {
                    try
                    {
                        productPic.Load(returnProductImagePath());
                    }
                    catch
                    {
                        productPic.Image = Properties.Resources.unknown;
                    }
                    finally
                    {
                        Console.Write("[FINALLY STATEMENT] At the loading image of control");
                    }
                    ShowData();
                }

                else
                {
                    API_CLASS API = new API_CLASS();
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(API.ReturnImageURL() + this.ProductData.Field<string>("ID") + ".jpg"), API.zaretnaProductsFolder + string.Format(@"\{0}-{1}.jpg", this.ProductData.Field<string>("ID"), this.ProductData.Field<string>("IMG_VERSION").ToString()));
                        client.DownloadFileCompleted += (se, ev) =>
                        {
                            try
                            {
                                productPic.Load(returnProductImagePath());
                            }
                            catch
                            {
                                productPic.Image.Dispose();
                                productPic.Image = Properties.Resources.unknown;
                            }
                            ShowData();
                        };
                    }
                }
            }
            else
                ShowData();
        }
        private void ShowData()
        {
            this.lblName.Visible = true;
            this.lblPrice.Visible = true;
            this.lblUnit.Visible = true;

            this.picLoading1.Visible = false;
            this.picLoading2.Visible = false;

            this.MouseClick += OnSelectProduct;
            this.Cursor = Cursors.Hand;
            foreach(Control ctrl in this.Controls)
            {
                ctrl.MouseClick += OnSelectProduct;
                ctrl.Cursor = Cursors.Hand;
            }
        }
        private string returnProductImagePath()
        {
           return new API_CLASS().zaretnaProductsFolder + string.Format(@"\{0}-{1}.jpg", this.ProductData.Field<string>("ID"), this.ProductData.Field<string>("IMG_VERSION"));
        }

        private void OnSelectProduct(object sender, MouseEventArgs e)
        {
            if (Application.OpenForms.OfType<NewProductsForm>().Count() > 0)
            {
                Application.OpenForms.OfType<NewProductsForm>().First().AddProductToCategory(int.Parse(this.ProductData.Field<string>("ID")));
            }
            (this.Parent.Parent as Form).Close();
        }
    }
}
