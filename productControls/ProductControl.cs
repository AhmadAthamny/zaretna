using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ZaretnaPanel
{
    public partial class ProductControl : UserControl
    {
        public int ProductID;
        public DataRow ProductData;
        public BackgroundWorker ImageLoader = new BackgroundWorker();
        public ProductControl(int productID, DataRow row = null)
        {
            InitializeComponent();
            StartProductControl(productID, row);
        }

        public void StartProductControl(int productID, DataRow row)
        {
            if (productID != -1)
                this.ProductID = productID;

            if (row == null)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (sender, args) =>
                {

                    this.ProductData = new API_CLASS().API_QUERY(string.Format("SELECT A.ID, A.A_NAME, A.H_NAME, A.FINAL_PRICE, A.IMG_VERSION, B.H_NAME AS UNIT_NAME, A.DISCOUNT_QUANTITY, A.REMOVED, A.PRICE, A.CUSTOM_ADD, A.UNIT_VALUE, C.DATE AS PRICE_DATE, C.TIME AS PRICE_TIME, NOW() AS DB_DATETIME, A.WP_INDEX" +
                        " FROM products A" +
                        " LEFT JOIN unit_names B ON A.UNIT_ID = B.ID" +
                        " LEFT JOIN (SELECT * FROM price_history WHERE PRODUCT_ID={0} ORDER BY DATE DESC LIMIT 1) C ON A.ID = C.PRODUCT_ID" +
                        " WHERE A.ID={1}", this.ProductID, this.ProductID)).Rows[0];
                };
                bw.RunWorkerCompleted += (sender, args) =>
                {
                    FillProductData();
                };
                bw.RunWorkerAsync();
            }
            else
            {
                this.ProductData = row;
                FillProductData();
            }
        }

        private void productPriceTB_KeyDown(object sender, KeyEventArgs e)
        {
            this.productPriceTB.Leave -= new EventHandler(productPriceTB_Leave);
            if (e.KeyCode == Keys.Enter)
            {
                decimal newPrice;
                bool isValidPrice;
                isValidPrice = decimal.TryParse(productPriceTB.Text.Trim(' '), out newPrice);
                if (isValidPrice)
                {
                    this.savingIconImg.Image = Properties.Resources.ezgif_5_454df066aa63;
                    this.savingIconImg.Cursor = Cursors.Default;
                    this.productPriceTB.Enabled = false;
                    this.productPriceTB.Text = newPrice.ToString("F2");
                    new Task(() => { SaveProductPrice(decimal.Parse(newPrice.ToString("F2"))); }).Start();
                    this.productImage.Select();
                }
                else
                {
                    BiggerMessage.Show("قيمة خاطئة");
                }
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.productImage.Select();
                this.productPriceTB.Text = this.ProductData.Field<string>(3);
            }
            this.productPriceTB.Leave += new EventHandler(productPriceTB_Leave);
        }
        private void SaveProductPrice(decimal newPrice)
        {
            decimal productWeight = decimal.Parse(this.ProductData.Field<string>("UNIT_VALUE"));
            decimal kiloprice = decimal.Parse(this.ProductData.Field<string>("PRICE"));
            decimal newpercent = decimal.Parse(((((newPrice / productWeight) - kiloprice) / kiloprice) * 100).ToString("F7"));
            new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET FINAL_PRICE={0}, CUSTOM_ADD={1}, PRICE_UPDATED = CASE WHEN PRICE_UPDATED<2 THEN 1 ELSE PRICE_UPDATED END WHERE ID={2}", newPrice, newpercent, this.ProductID));
            int total_affected = new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE price_history SET NEW_PRICE={0}, TIME=curtime() WHERE PRODUCT_ID={1} AND DATE=curdate()", newPrice, this.ProductID));
            if (total_affected == 0)
                new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO price_history VALUES ({0}, {1}, curdate(), curtime())", this.ProductID, newPrice));
            else
            {
                int tmpid = int.Parse(this.ProductData.Field<string>("WP_INDEX"));
                if (tmpid != -1)
                {
                    new API_CLASS().WP_updateProductPrice(tmpid, newPrice);
                }
            }
            this.ProductData.SetField<string>(3, newPrice.ToString("F2"));
            this.savingIconImg.Invoke((MethodInvoker)delegate {
                this.savingIconImg.Image = Properties.Resources.rsz_settings_512;
                this.savingIconImg.Cursor = Cursors.Hand;
                this.productPriceTB.Enabled = true;
            });
        }
        private void FillProductData()
        {
            if (this.ImageLoader.IsBusy) return;
            this.ImageLoader = new BackgroundWorker();

            this.ProductID = int.Parse(this.ProductData.Field<string>(0));
            arabicAndHebrewName.Text = this.ProductData.Field<string>(1) + "\n" + this.ProductData.Field<string>(2);
            productPriceTB.Text = this.ProductData.Field<string>(3);
            unitName.Text = this.ProductData.Field<string>(5);

            DateTime db_date, date, time;
            DateTime.TryParse(this.ProductData.Field<string>("DB_DATETIME"), out db_date);
            DateTime.TryParse(this.ProductData.Field<string>("PRICE_DATE"), out date);
            DateTime.TryParse(this.ProductData.Field<string>("PRICE_TIME"), out time);
            lblLastPriceUpdate.Text = this.ProductData.IsNull("PRICE_DATE") ? "" : "לפני "+TimeToString(db_date, date.Date.Add(time.TimeOfDay));

            string imgVersion = this.ProductData.Field<string>(4);
            if (int.Parse(imgVersion) > 0)
            {
                if (File.Exists(returnProductImagePath()))
                {
                    try { productImage.Load(returnProductImagePath()); }
                    catch { productImage.Image = Properties.Resources.unknown; }
                    LoadProductControls();
                }
                else
                {
                    API_CLASS API = new API_CLASS();
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(API.ReturnImageURL() + this.ProductID.ToString() + ".jpg"), API.zaretnaProductsFolder + string.Format(@"\{0}-{1}.jpg", this.ProductID, this.ProductData.Field<string>("IMG_VERSION").ToString()));
                        client.DownloadFileCompleted += (se, ev) =>
                        {
                            try
                            {
                                productImage.Load(returnProductImagePath());
                            }
                            catch
                            {
                                productImage.Image.Dispose();
                                productImage.Image = Properties.Resources.unknown;
                            }
                            LoadProductControls();
                        };
                    }
                }
            }
            else
                LoadProductControls();
        }

        private void productPriceTB_Leave(object sender, EventArgs e)
        {
            this.productPriceTB.Text = this.ProductData.Field<string>(3);
        }

        private void ProductControl_Leave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.None;
        }

        private void savingIconImg_Click(object sender, EventArgs e)
        {
            if (savingIconImg.Cursor == Cursors.Hand && savingIconImg.Visible)
            {
                NewProductsForm NPF = this.Parent.Parent as NewProductsForm;
                NPF.ToggleMovingButtons(null);
                AddNewProductForm ANPF = new AddNewProductForm(this.ProductID, this.Parent.Parent as Form, NPF.currentCategory, this);
            }
        }
        private string TimeToString(DateTime d1, DateTime d2)
        {
            if ((int)(d1 - d2).TotalDays > 0) return (int)(d1 - d2).TotalDays + " ימים";
            if ((int)(d1 - d2).TotalHours > 0) return (int)(d1 - d2).TotalHours + " שעות";
            if ((int)(d1 - d2).TotalMinutes > 0) return (int)(d1 - d2).TotalMinutes + " דקות";
            return "שניות";
        }
        public void ResetProductControls()
        {
            this.productImage.Image = Properties.Resources.unknown;
            this.productImage.Visible = true;
            this.loadingImage.Visible = true;
            this.lblLastPriceUpdate.Visible = false;
            this.picDiscount.Visible = false;
            this.picNotActive.Visible = false;
            this.savingIconImg.Visible = false;
            this.unitName.Visible = true;
            this.productPriceTB.Visible = true;
            this.arabicAndHebrewName.Visible = true;
        }

        private void btnMoveProduct_Click(object sender, EventArgs e)
        {
            NewProductsForm NPF = this.Parent.Parent as NewProductsForm;
            NPF.ToggleMovingButtons(this);
            //NPF.saveallIndexes();
        }

        private void btnArrowMove_Click(object sender, EventArgs e)
        {
            NewProductsForm NPF = this.Parent.Parent as NewProductsForm;
            NPF.MoveProductCards(this);
        }

        private string returnProductImagePath()
        {
            return new API_CLASS().zaretnaProductsFolder + string.Format(@"\{0}-{1}.jpg", this.ProductID, this.ProductData.Field<string>("IMG_VERSION"));
        }
        private void LoadProductControls()
        {
            this.loadingImage.Visible = false;
            this.savingIconImg.Visible = true;
            this.savingIconImg.Cursor = Cursors.Hand;
            this.lblLastPriceUpdate.Visible = !this.ProductData.IsNull("PRICE_DATE");

            if (int.Parse(this.ProductData.Field<string>(6)) != -1)
                this.picDiscount.Visible = true;
            if (int.Parse(this.ProductData.Field<string>(7)) == 1)
                this.picNotActive.Visible = true;
        }
    }
}
