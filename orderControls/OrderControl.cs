using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace ZaretnaPanel
{
    public partial class OrderControl : UserControl
    {
        PrivateFontCollection PFC = new PrivateFontCollection();
        private DataRow OrderData;
        private BackgroundWorker loadingOrderWorker = null;
        public BackgroundWorker CalculatePriceWorker = null;
        private OrderInfo OrderBoard = null;
        public int indexInList = -1;
        public OrderControl(DataRow orderdata, int index)
        {
            InitializeComponent();
            initNewFont();
            this.lblFullName.Font = new Font(PFC.Families[0], this.lblFullName.Font.Size);
            this.lblOrderID.Font = new Font(PFC.Families[0], this.lblOrderID.Font.Size);
            this.lblPhone.Font = new Font(PFC.Families[0], this.lblPhone.Font.Size);
            this.lblRegion.Font = new Font(PFC.Families[0], this.lblRegion.Font.Size);
            //this.lblProductCount.Font = new Font(PFC.Families[0], this.lblProductCount.Font.Size);
            //this.lblWebPrice.Font = new Font(PFC.Families[0], this.lblWebPrice.Font.Size);
            //this.lblFinalPrice.Font = new Font(PFC.Families[0], this.lblFinalPrice.Font.Size);
            this.lblPayment.Font = new Font(PFC.Families[0], this.lblPayment.Font.Size);

            this.lblOrderID.Parent = this.pictureBox2;
            this.lblOrderID.BackColor = Color.Transparent;

            this.indexInList = index;

            this.Cursor = Cursors.Hand;
            this.MouseClick += OnClickOrder;
            foreach (Control ctrl in this.Controls)
                ctrl.MouseClick += OnClickOrder;

            this.OrderData = orderdata;
            FillOrderData();
            UpdateColor();

            /*if (decimal.Parse(this.OrderData.Field<string>("WEBSITE_PRICE")) == -1)
            {
                CalculatePriceWorker = new BackgroundWorker();
                CalculatePriceWorker.WorkerSupportsCancellation = true;
                CalculatePriceWorker.DoWork += (s, e) =>
                {
                    if (this.CalculatePriceWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    CalculateWebsitePrice();
                };
                CalculatePriceWorker.RunWorkerCompleted += (s, e) =>
                {
                    if (!e.Cancelled)
                    {
                        this.lblWebPrice.Text = "₪" + this.OrderData.Field<string>("WEBSITE_PRICE");
                        this.lblProductCount.Text = this.OrderData.Field<string>("PRODUCT_COUNT");
                    }
                };
                CalculatePriceWorker.RunWorkerAsync();
            }*/
        }

        private void OnClickOrder(object sender, MouseEventArgs e)
        {
            LoadingForm loadingBoard = new LoadingForm();
            if (!loadingOrderWorker.IsBusy && Application.OpenForms.OfType<NewOrdersForm>().Count() == 1)
            {
                if (Application.OpenForms.OfType<NewOrdersForm>().First().loadingOrderBoard == null)
                {
                    Application.OpenForms.OfType<NewOrdersForm>().First().loadingOrderBoard = loadingBoard;
                    loadingBoard.label1.Font = new Font(PFC.Families[0], loadingBoard.label1.Font.Size);
                    loadingBoard.label1.Text = "הזמנה מס' " + this.OrderData.Field<string>("ID");
                    loadingBoard.Show();
                    loadingOrderWorker.RunWorkerAsync();
                }
            }
        }

        private void initNewFont()
        {
            //Select your font from the resources.
            //My font here is "Digireu.ttf"
            int fontLength = Properties.Resources.VarelaRound_Regular.Length;

            // create a buffer to read in to
            byte[] fontdata = Properties.Resources.VarelaRound_Regular;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            PFC.AddMemoryFont(data, fontLength);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void FillOrderData()
        {
            this.lblFullName.Text = this.OrderData.Field<string>("F_NAME") + " " + this.OrderData.Field<string>("L_NAME");
            this.lblOrderID.Text = "#"+this.OrderData.Field<string>("ID");
            this.lblPhone.Text = this.OrderData.Field<string>("PHONE_NUMBER").Replace("-", "");
            this.lblRegion.Text = this.OrderData.Field<string>("CITY_NAME");
            //this.lblFinalPrice.Text = float.Parse(this.OrderData.Field<string>("TOTAL_PRICE")) == -1 ? "-" : "₪" + this.OrderData.Field<string>("TOTAL_PRICE");
            this.lblPayment.Text = int.Parse(this.OrderData.Field<string>("PAID")) == 0 ? "מוזמן" : "אשראי";
            this.lblBuyTime.Text = DateTime.Parse(this.OrderData.Field<string>("BUY_TIME")).ToString("dd-MM-yyyy HH:mm");

            /*if (decimal.Parse(this.OrderData.Field<string>("WEBSITE_PRICE")) == -1)
            {
                this.lblWebPrice.Text = "..חושב";
                this.lblProductCount.Text = "..חושב";
            }
            else
            {
                this.lblWebPrice.Text = "₪" + this.OrderData.Field<string>("WEBSITE_PRICE");
                this.lblProductCount.Text = this.OrderData.Field<string>("PRODUCT_COUNT");
            }*/

            this.loadingOrderWorker = new BackgroundWorker();
            this.loadingOrderWorker.DoWork += (s, e)  => {

                OrderBoard = new OrderInfo(int.Parse(this.OrderData.Field<string>("ID")), this);
            };
            this.loadingOrderWorker.RunWorkerCompleted += (s, e) =>
            {
                LoadingForm loadingBoard;
                if (Application.OpenForms.OfType<NewOrdersForm>().Count() == 1)
                {
                    loadingBoard = Application.OpenForms.OfType<NewOrdersForm>().First().loadingOrderBoard;
                    loadingBoard.Close();
                    Application.OpenForms.OfType<NewOrdersForm>().First().loadingOrderBoard = null;
                }
                if (!this.OrderBoard.InvalidOrder)
                this.OrderBoard.ShowDialog(this);
            };
        }

        private void CalculateWebsitePrice()
        {
            DataTable dt = new API_CLASS().API_QUERY("SELECT * FROM orders_products WHERE ORDER_ID=" + this.OrderData.Field<string>("ID"));
            float total = 0;
            foreach (DataRow row in dt.Rows)
            {
                float unitValue = float.Parse(row.Field<string>("UNIT_VALUE"));
                int discountQuantity = Convert.ToInt32(row.Field<string>("DISCOUNT_QUANTITY"));
                float discountPrice = float.Parse(row.Field<string>("DISCOUNT_PRICE"));
                float Quantity = float.Parse(row.Field<string>("QUANTITY"));
                float SellPrice = float.Parse(row.Field<string>("SELL_PRICE"));
                float kiloPrice = SellPrice / unitValue;
                float productPriceToAdd;

                if (unitValue == 1)
                {
                    if (discountQuantity == 1)
                        productPriceToAdd = discountPrice * Quantity;
                    else if (discountQuantity > 1)
                    {
                        float toReduceQuantityFrom = Quantity;
                        int quantitiesFoundForDiscount = 0;
                        while (toReduceQuantityFrom >= discountQuantity)
                        {
                            toReduceQuantityFrom -= discountQuantity;
                            quantitiesFoundForDiscount++;
                        }
                        productPriceToAdd = discountPrice * quantitiesFoundForDiscount + toReduceQuantityFrom * SellPrice;
                    }
                    else
                    {
                        productPriceToAdd = SellPrice * Quantity;
                    }
                }
                else
                {
                    if (discountQuantity == 1)
                        productPriceToAdd = discountPrice * unitValue * Quantity;
                    else if (discountQuantity > 1)
                    {
                        float toReduceQuantityFrom = Quantity * unitValue;
                        int quantitiesFoundForDiscount = 0;
                        while (toReduceQuantityFrom >= discountQuantity)
                        {
                            toReduceQuantityFrom -= discountQuantity;
                            quantitiesFoundForDiscount++;
                        }
                        productPriceToAdd = discountPrice * quantitiesFoundForDiscount + toReduceQuantityFrom * kiloPrice;
                    }
                    else
                    {
                        productPriceToAdd = SellPrice * Quantity;
                    }
                }
                total += productPriceToAdd;
            }
            total = float.Parse(total.ToString("F2"));
            this.OrderData.SetField("WEBSITE_PRICE", total);
            this.OrderData.SetField("PRODUCT_COUNT", dt.Rows.Count);
            new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE orders SET WEBSITE_PRICE={0}, PRODUCT_COUNT={1} WHERE ID={2}", total, dt.Rows.Count, int.Parse(this.OrderData.Field<string>("ID"))));
        }
        public void UpdateColor()
        {
            if(float.Parse(this.OrderData.Field<string>("TOTAL_PRICE")) == -1)
            {
                this.pictureBox2.Image = Properties.Resources.new_order_structure2;
                foreach (Control ctrl in this.Controls)
                    ctrl.BackColor = Color.White;
            }
            else
            {
                this.pictureBox2.Image = Properties.Resources.new_order_done;
                foreach (Control ctrl in this.Controls)
                    ctrl.BackColor = Color.FromArgb(255, 240, 118);
            }
            this.lblOrderID.BackColor = Color.Transparent;
        }
        public void UpdateFinalPrice(float finalPrice)
        {
            this.OrderData.SetField("TOTAL_PRICE", finalPrice);
            //this.lblFinalPrice.Text = finalPrice < 0 ? "-" : "₪" + finalPrice.ToString();
            this.UpdateColor();
        }
        public void RemoveOrder()
        {
            if (Application.OpenForms.OfType<NewOrdersForm>().Count() == 1)
                Application.OpenForms.OfType<NewOrdersForm>().First().RemoveOrderControl(this, this.indexInList);

            this.Dispose();
        }
    }
}
