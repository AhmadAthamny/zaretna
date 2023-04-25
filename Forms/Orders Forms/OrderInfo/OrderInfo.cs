using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace ZaretnaPanel
{
    public partial class OrderInfo : Form
    {

        public int OrderID;
        private string LocationURL;
        private string Notes = "@";
        private string Address = "";
        private float LastPrice = -1;
        private bool isDelivered = false;
        public bool InvalidOrder = false;
        public float TotalFinalPrice = -1;
        private DataRow CityInfo = null;

        public float noDiscountPrice = 0;
        public float ToApplyDiscountOn = 0;
        private OrderControl SourceControl = null;
        public OrderInfo(int tmpOrderID, OrderControl srcControl)
        {
            InitializeComponent();
            this.OrderID = tmpOrderID;
            FillOrderInformation();

            this.SourceControl = srcControl;

            this.Top = Screen.PrimaryScreen.WorkingArea.Top;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }
        public void FillOrderInformation()
        {
            string cmd = String.Format("SELECT ID, F_NAME, L_NAME, PHONE_NUMBER, EMAIL, NOTE, BUY_TIME, PAID, LOCATION_LAT, LOCATION_LONG, CITY, HOUSE_NUM, STREET_NUM, ENTER_NUM, TOTAL_PRICE, STATUS, IP_ADDRESS FROM orders WHERE ID={0}", this.OrderID);
            DataTable dt = new API_CLASS().API_QUERY(cmd);
            //BiggerMessage.Show("https://zaretna.co.il/order?id=" + dt.Rows[0].Field<string>(0) + "&key=" + dt.Rows[0].Field<string>("VIEW_CODE"));
            string db_F_NAME, db_L_NAME, db_PHONE_NUMBER;
            if (dt.Rows.Count > 0)
            {
                db_F_NAME = dt.Rows[0].Field<string>(1);
                db_L_NAME = dt.Rows[0].Field<string>(2);
                db_PHONE_NUMBER = dt.Rows[0].Field<string>(3);

                this.CityInfo = ReturnCityInfo(Convert.ToInt32(dt.Rows[0].Field<string>(10))).Rows[0];

                txtOrderID.Text = dt.Rows[0].Field<string>(0);
                txtName.Text = db_F_NAME + " " + db_L_NAME;
                txtPhone.Text = db_PHONE_NUMBER;
                txtEmail.Text = dt.Rows[0].Field<string>(4);
                txtCity.Text = this.CityInfo.Field<string>("H_NAME");
                txtBuyTime.Text = dt.Rows[0].Field<string>(6);
                txtPaid.Text = Convert.ToInt32(dt.Rows[0].Field<string>(7)) == 1 ? "כרטיס אשראי" : "מזומן";
                txtBuyCount.Text = GetCustomerTotalOrders().ToString();
                lblIPAddress.Text = dt.Rows[0].Field<string>(16) == "NONE" ? "" : dt.Rows[0].Field<string>(16);
                this.Text = "פרטי הזמנה #" + dt.Rows[0].Field<string>(0);

                if(int.Parse(dt.Rows[0].Field<string>(7)) == 1)
                {
                    txtPaid.BackColor = Color.Red;
                    txtPaid.ForeColor = Color.White;
                }

                if ( ! string.IsNullOrEmpty ( dt.Rows[0].Field<string>( 11 ) ) && ! string.IsNullOrWhiteSpace( dt.Rows[0].Field<string>( 11 ) ) )
                    Address += "מס` בית: " + dt.Rows[0].Field<string>(11) + "\r\n\r\n";

                if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>(12)) && !string.IsNullOrWhiteSpace(dt.Rows[0].Field<string>(12)))
                    Address += "רחוב: " + dt.Rows[0].Field<string>(12) + "\r\n\r\n";

                if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>(13)) && !string.IsNullOrWhiteSpace(dt.Rows[0].Field<string>(13)))
                    Address += "כניסה: " + dt.Rows[0].Field<string>(13) + "\r\n\r\n";

                if (!string.IsNullOrEmpty(Address) && !string.IsNullOrWhiteSpace(Address))
                {
                    txtAddress.Text = "יש כתובת, לחץ כאן!";
                    txtAddress.ForeColor = Color.Green;
                }
                else
                    txtAddress.Text = "אין";

                Notes = dt.Rows[0].Field<string>(5);

                if (Notes != "-1" && Notes.Length > 0 && !string.IsNullOrEmpty(Notes) && !string.IsNullOrWhiteSpace(Notes))
                {
                    txtNotes.Text = "יש הערות, לחץ כאן!";
                    txtNotes.ForeColor = Color.Green;
                }
                else
                {
                    txtNotes.Text = "אין";
                    Notes = "-1";
                }


                if (float.Parse(dt.Rows[0].Field<string>(9)) == -1)
                {
                    txtLocation.Text = "אין";
                    LocationURL = "n/a";
                    txtLocation.ForeColor = Color.Black;
                }
                else
                {
                    LocationURL = "https://waze.com/ul?ll=" + dt.Rows[0].Field<string>(8) + "," + dt.Rows[0].Field<string>(9);
                }

                cmd = String.Format("SELECT b.ID, b.PRODUCT_ID, a.H_NAME, b.QUANTITY, d.H_NAME AS UNIT_NAME, b.SELL_PRICE, b.INCLUDED_IN_DISCOUNT, b.UNIT_VALUE, b.DISCOUNT_QUANTITY, b.DISCOUNT_PRICE, b.DISCOUNT_MAX_REPEAT FROM products a " +
                    "INNER JOIN orders_products b on a.ID = b.PRODUCT_ID" +
                    " LEFT JOIN unit_names d on a.UNIT_ID = d.ID" +
                    " WHERE b.ORDER_ID = {0} ORDER BY ID ASC", this.OrderID);

                DataTable dt3 = new API_CLASS().API_QUERY(cmd);
                if (dt != null)
                {

                    this.TotalFinalPrice = float.Parse(dt.Rows[0].Field<string>(14));

                    if (float.Parse(dt.Rows[0].Field<string>(14)) < 0)
                        textTotalPriceNew.Text = "-";
                    else
                        textTotalPriceNew.Text = string.Format("₪{0}", dt.Rows[0].Field<string>(14));


                    if(Convert.ToInt32(dt.Rows[0].Field<string>(15)) == 2)
                    {
                        this.isDelivered = true;
                        button1.Text = "לא נשלח";
                    }
                    else if(Convert.ToInt32(dt.Rows[0].Field<string>(15)) == 4)
                    {
                        isOrderRemovedLBL.Visible = true;
                    }
                
                    ordersTable.Columns.Add("listid", "#");
                    ordersTable.Columns.Add("productid", "קוד פריט");
                    ordersTable.Columns.Add("name", "שם פריט");
                    ordersTable.Columns.Add("quantity", "כמות");
                    ordersTable.Columns.Add("unitname", "יחידה");
                    ordersTable.Columns.Add("price", "מחיר יחידה");
                    ordersTable.Columns.Add("discountprice", "סכום פריט");

                    ordersTable.Columns[0].ReadOnly = true;
                    ordersTable.Columns[1].ReadOnly = true;
                    ordersTable.Columns[2].ReadOnly = true;
                    ordersTable.Columns[3].ReadOnly = true;
                    ordersTable.Columns[4].ReadOnly = true;
                    ordersTable.Columns[5].ReadOnly = true;

                    ordersTable.Columns[0].Visible = false;

                    ordersTable.Columns[0].FillWeight = 1;
                    ordersTable.Columns[1].FillWeight = 10;
                    ordersTable.Columns[2].FillWeight = 28;
                    ordersTable.Columns[3].FillWeight = 12;
                    ordersTable.Columns[4].FillWeight = 12;
                    ordersTable.Columns[5].FillWeight = 15;
                    ordersTable.Columns[6].FillWeight = 22;

                    int discountQuantity, discountMaxRepeat;
                    float discountPrice;
                    float unitValue;
                    float productPriceToAdd;
                    float Quantity;
                    float SellPrice;
                    float kiloPrice;
                    bool discountApplied = false;
                    int discountReptitions;
                    foreach (DataRow row in dt3.Rows)
                    {
                        unitValue = float.Parse(row.Field<string>(7));
                        discountQuantity = Convert.ToInt32(row.Field<string>(8));
                        Quantity = float.Parse(row.Field<string>(3));
                        SellPrice = float.Parse(row.Field<string>(5));
                        kiloPrice = SellPrice / unitValue;

                        if (discountQuantity != -1)
                        {
                            discountPrice = float.Parse(row.Field<string>(9));
                            discountMaxRepeat = Convert.ToInt32(row.Field<string>("DISCOUNT_MAX_REPEAT"));
                            discountReptitions = 0;
                            float qToRemoveFrom = Quantity * unitValue;
                            while( ( discountMaxRepeat == 0 ? true : discountReptitions < discountMaxRepeat ) && qToRemoveFrom-discountQuantity >= 0 )
                            {
                                qToRemoveFrom -= discountQuantity;
                                discountReptitions++;
                            }
                            productPriceToAdd = qToRemoveFrom * kiloPrice + discountReptitions*discountPrice;
                            discountApplied = true;
                        }
                        else {
                            productPriceToAdd = SellPrice * Quantity;
                            discountApplied = false;
                        }

                        if (Convert.ToInt32(row.Field<string>(6)) == 1)
                            ToApplyDiscountOn += productPriceToAdd;
                        else
                            noDiscountPrice += productPriceToAdd;

                        ordersTable.Rows.Add(row.Field<string>(0), row.Field<string>(1), row.Field<string>(2), row.Field<string>(3), row.Field<string>(4), row.Field<string>(5), productPriceToAdd.ToString("F2")+(discountApplied ? " (בהנחה)" : ""));
                    }
                    this.ttlProducts.Text = ordersTable.Rows.Count.ToString();
                    CalculateWebsitePrice(noDiscountPrice, ToApplyDiscountOn);
                }
                else
                {
                    BiggerMessage.Show("שגיאה, פתח את החלון מחדש");
                    this.Close();
                    this.InvalidOrder = true;
                }
            }
            else
            {
                BiggerMessage.Show("הזמנה לא קיימת");
                this.Close();
                this.InvalidOrder = true;
            }
        }

        private void txtLocation_Click(object sender, EventArgs e)
        {
            if(LocationURL != "n/a")
                System.Diagnostics.Process.Start(LocationURL);
        }

        private void txtNotes_Click(object sender, EventArgs e)
        {
            if (Notes != "-1")
                BiggerMessage.Show(Notes, 2);
        }

        private void ordersTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 3)
            {
                LastPrice = float.Parse(ordersTable.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
        }

        private void ordersTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                bool isFloat;
                float tmpValue;
                if (!string.IsNullOrEmpty(ordersTable.Rows[e.RowIndex].Cells[3].Value as string))
                {
                    isFloat = float.TryParse(ordersTable.Rows[e.RowIndex].Cells[3].Value.ToString(), out tmpValue);
                    if (!isFloat)
                    {
                        BiggerMessage.Show("ערך לא חוקי");
                        ordersTable.Rows[e.RowIndex].Cells[3].Value = LastPrice;
                    }
                }
                else
                {
                    BiggerMessage.Show("ערך לא חוקי");
                    ordersTable.Rows[e.RowIndex].Cells[3].Value = LastPrice;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            API_CLASS api = new API_CLASS();
            foreach(DataGridViewRow row in ordersTable.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Index == -1) continue;

                int IDOnList = Convert.ToInt32(row.Cells[0].Value);
                if (IDOnList != -1)
                {
                    float quantites = float.Parse(row.Cells[3].Value.ToString());
                    api.API_QUERY_EXECUTE(String.Format("UPDATE orders_products SET QUANTITY={0:F3} WHERE ID={1}", quantites, IDOnList));
                }
            }
            BiggerMessage.Show("لقد تم جفظ المعلومات", 1);
        }
        private DataTable ReturnCityInfo(int cityid)
        {
            return new API_CLASS().API_QUERY("SELECT ID, H_NAME, DELIVERY_PRICE FROM regions WHERE ID=" + cityid);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("האם אתה רוצה לבטל את ההזמנה?", "ביטול הזמנה", MessageBoxButtons.YesNoCancel);
            if(dlg == DialogResult.Yes)
            {
                API_CLASS api = new API_CLASS();
                api.API_QUERY_EXECUTE("DELETE FROM pay_card WHERE ORDER_ID="+ this.OrderID);
                api.API_QUERY_EXECUTE("UPDATE orders SET STATUS=4 WHERE ID=" + this.OrderID);

                if(this.SourceControl != null)
                    this.SourceControl.RemoveOrder();

                if (Application.OpenForms.OfType<latestOrders>().Count() == 1)
                    Application.OpenForms.OfType<latestOrders>().First().RemoveOrderRow(this.OrderID);

                BiggerMessage.Show("ההזמנה בוטלה");
                this.Close();
            }
        }

        private void txtAddress_Click(object sender, EventArgs e)
        {
            if ( Address.Length > 0 && !string.IsNullOrWhiteSpace(Address) && !string.IsNullOrEmpty(Address))
            BiggerMessage.Show(Address, 2);
        }

        private void ordersTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
            /*
             * Order product ID (cell 0) is hidden, but is accessible. Will be used to remove products from carts.
             * It's better to remove products from carts by sorting them out than `DELETING` them. (add REMOVED column to the database).
             * Cart total price should be re-calculated and re-printed upon removing a product.
             */
        }

        private void label10_Click(object sender, EventArgs e)
        {
            PaymentInfo pINF = new PaymentInfo(this.OrderID);
            if (pINF.CloseForm == false)
                pINF.ShowDialog(this);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!this.isDelivered)
            {
                if (Application.OpenForms.OfType<EnterTotalPrice>().Count() == 1)
                    Application.OpenForms.OfType<EnterTotalPrice>().First().Close();
                EnterTotalPrice ETP = new EnterTotalPrice(this);
                ETP.ShowDialog(this);
            }
            else
            {
                DialogResult dlg = MessageBox.Show("האם אתה רוצה לעדן ההזמנה שלט נשלחה?", "עדכן שלא נשלח", MessageBoxButtons.YesNoCancel);
                if(dlg == DialogResult.Yes)
                {
                    new API_CLASS().API_QUERY_EXECUTE("UPDATE orders SET STATUS=0 WHERE ID=" + this.OrderID);
                    BiggerMessage.Show("ההזמנה התעדכנה שלא נשלחה", 1);
                    this.Close();
                }

            }
        }

        private float GetProductUnitValue(int idonlist)
        {
            DataTable dt = new API_CLASS().API_QUERY("SELECT QUANTITY FROM orders_products WHERE ID=" + idonlist);
            if (dt != null)
                if (dt.Rows.Count > 0)
                    return float.Parse(dt.Rows[0].Field<string>(0));
            
            return -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<EnterTotalPrice>().Count() == 1)
                Application.OpenForms.OfType<EnterTotalPrice>().First().Close();
            EnterTotalPrice ETP = new EnterTotalPrice(this, 1);
            ETP.ShowDialog(this);
        }

        public void SetOrderAsDelivered()
        {
            new API_CLASS().API_QUERY_EXECUTE("UPDATE orders SET STATUS=2, TOTAL_PRICE="+ TotalFinalPrice + " WHERE ID=" + this.OrderID);

            if (this.SourceControl != null)
                this.SourceControl.RemoveOrder();

            if (Application.OpenForms.OfType<latestOrders>().Count() == 1)
                Application.OpenForms.OfType<latestOrders>().First().RemoveOrderRow(this.OrderID);

            this.Close();
            BiggerMessage.Show("ההזמנה התעדכנה שנשלחה", 1, "הזמנה");
        }
        public void UpdateOrderColorOnOrdersBoard()
        {
            if(this.SourceControl != null)
                this.SourceControl.UpdateColor();
        }

        private void addProduct_Click(object sender, EventArgs e)
        {
            AddProductToCart APTC = new AddProductToCart(this);
            APTC.ShowDialog(this);
        }

        public void AddProduct(int productid, float quantity)
        {
            DataTable dt = new API_CLASS().API_QUERY("SELECT A.H_NAME, B.A_NAME AS UNIT_NAME, A.UNIT_VALUE, A.FINAL_PRICE, A.DISCOUNT_QUANTITY, A.DISCOUNT_PRICE, A.INCLUDED_IN_DISCOUNT FROM products A LEFT JOIN unit_names B ON A.UNIT_ID = B.ID WHERE A.ID="+productid);
            if(dt.Rows.Count > 0)
            {
                int InsertedID = new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO orders_products (ORDER_ID, PRODUCT_ID, QUANTITY, SELL_PRICE, UNIT_VALUE) VALUES ({0}, {1}, {2}, {3}, {4})", 
                this.OrderID, productid, quantity, float.Parse(dt.Rows[0].Field<string>(3)), float.Parse(dt.Rows[0].Field<string>(2))));

                if (InsertedID == -1)
                {
                    BiggerMessage.Show("לא התאפשר להוסיף פריט\r\n\r\nבדוק את החיבור לרשת");
                }
                else
                {
                    float unitValue = float.Parse(dt.Rows[0].Field<string>(2));
                    int discountQuantity = Convert.ToInt32(dt.Rows[0].Field<string>(4));
                    float discountPrice = float.Parse(dt.Rows[0].Field<string>(5));
                    float Quantity = quantity;
                    float SellPrice = float.Parse(dt.Rows[0].Field<string>(3));
                    float kiloPrice = SellPrice / unitValue;
                    float productPriceToAdd;
                    bool discountApplied = true;

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
                            if (quantitiesFoundForDiscount == 0) discountApplied = false;
                        }
                        else
                        {
                            productPriceToAdd = SellPrice * Quantity;
                            discountApplied = false;
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
                            if (quantitiesFoundForDiscount == 0) discountApplied = false;

                        }
                        else
                        {
                            productPriceToAdd = SellPrice * Quantity;
                            discountApplied = false;
                        }
                    }

                    if (Convert.ToInt32(dt.Rows[0].Field<string>(6)) == 1)
                        ToApplyDiscountOn += productPriceToAdd;
                    else
                        noDiscountPrice += productPriceToAdd;

                    ordersTable.Rows.Add(InsertedID, productid, dt.Rows[0].Field<string>(0), quantity, dt.Rows[0].Field<string>(1), float.Parse(dt.Rows[0].Field<string>(3)).ToString("F2"), productPriceToAdd.ToString("F2") + (discountApplied ? " (בהנחה)" : ""));
                    ordersTable.Rows[ordersTable.Rows.Count - 1].ReadOnly = true;

                    CalculateWebsitePrice(noDiscountPrice, ToApplyDiscountOn);
                    BiggerMessage.Show("הפריט נוסף להזמנה", 1);
                }
            }
        }
        public void RemoveListProductID(int listid)
        {
            foreach(DataGridViewRow row in ordersTable.Rows)
            {
                if (Convert.ToInt32(row.Cells[0].Value) == listid)
                {
                    CalculateWebsitePrice(5,5);
                    ordersTable.Rows.RemoveAt(row.Index);
                    break;
                }
            }
        }

        private void CalculateWebsitePrice(float noDiscountPrice, float withDiscountPrice)
        {
            DataTable dt_discounts = new API_CLASS().API_QUERY("SELECT ID, VALUE_INT FROM global_variables WHERE ID=5 OR ID=4 OR ID=3 ORDER BY ID ASC");

            int MinPriceForDiscount = Convert.ToInt32(dt_discounts.Rows[1].Field<string>(1));
            int DiscountPercentage = Convert.ToInt32(dt_discounts.Rows[0].Field<string>(1));
            int DiscountEnabled = Convert.ToInt32(dt_discounts.Rows[2].Field<string>(1));
            float finalPercent = (float)(100 - DiscountPercentage) / 100;



            if (withDiscountPrice >= MinPriceForDiscount && DiscountEnabled == 1)
            {
                textPrice.Text = " (בהנחה) ";
                textPrice.Text += string.Format("₪{0}", ((withDiscountPrice * finalPercent) + noDiscountPrice).ToString("F2"));
                CalculateDeliveryPrice((withDiscountPrice * finalPercent) + noDiscountPrice);
            }
            else
            {
                textPrice.Text = string.Format("₪{0}", (withDiscountPrice + noDiscountPrice).ToString("F2"));
                CalculateDeliveryPrice(withDiscountPrice + noDiscountPrice);
            }
        }

        private void CalculateDeliveryPrice(float totalPrice)
        {
            int deliveryprice = -1;
            if (this.CityInfo != null)
            {
                int cityid = int.Parse(this.CityInfo.Field<string>("ID"));
                if (cityid == 45 || cityid == 46)
                {
                    if (totalPrice >= 300)
                        deliveryprice = 0;
                    else
                        deliveryprice = int.Parse(this.CityInfo.Field<string>("DELIVERY_PRICE"));
                }
                else
                {
                    if (totalPrice >= 250)
                        deliveryprice = 0;
                    else
                        deliveryprice = int.Parse(this.CityInfo.Field<string>("DELIVERY_PRICE"));
                }
            }
            else
                deliveryprice = -1;

            if (deliveryprice == 0)
                this.txtDeliveryPrice.Text = "חינם";
            else this.txtDeliveryPrice.Text = string.Format("₪{0}", deliveryprice);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<EnterTotalPrice>().Count() == 1)
                Application.OpenForms.OfType<EnterTotalPrice>().First().Close();
            EnterTotalPrice ETP = new EnterTotalPrice(this, 2);
            ETP.ShowDialog();
        }
        private int GetCustomerTotalOrders()
        {
            DataTable dt = new API_CLASS().API_QUERY("SELECT COUNT(*) FROM orders WHERE STATUS=2 AND (PHONE_NUMBER LIKE '%25" + MySqlHelper.EscapeString(txtPhone.Text) + "%25')");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Rows[0].IsNull(0))
                    {
                        return int.Parse(dt.Rows[0].Field<string>(0));
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        private void OrderInfo_Load(object sender, EventArgs e)
        {
            ordersTable.ClearSelection();
            ordersTable.DoubleBuffered(true);
        }

        private string GetFileName(int orderid, int addon = 0 )
        {
            if (addon == 0)
            {
                if (!File.Exists(Application.StartupPath + @"\PDF_To_Print\" + this.OrderID.ToString() + ".pdf"))
                    return Application.StartupPath + @"\PDF_To_Print\" + this.OrderID.ToString() + ".pdf";
                else
                    return GetFileName(orderid, addon + 1);
            }
            else
            {
                if (!File.Exists(Application.StartupPath + @"\PDF_To_Print\" + this.OrderID.ToString() + "_" + addon.ToString() + ".pdf"))
                    return Application.StartupPath + @"\PDF_To_Print\" + this.OrderID.ToString() + "_" + addon.ToString() + ".pdf";
                else
                    return GetFileName(orderid, addon + 1);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (ordersTable.Rows.Count > 0)
            {
                Directory.CreateDirectory(Application.StartupPath + @"\PDF_To_Print");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = GetFileName(this.OrderID);
                bool fileError = false;
                if (File.Exists(sfd.FileName))
                {
                    try
                    {
                        File.Delete(sfd.FileName);
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                    }
                }
                if (!fileError)
                {
                    try
                    {
                        PdfPTable pdfTable = new PdfPTable(ordersTable.Columns.Count-1);
                        pdfTable.DefaultCell.NoWrap = false;
                        pdfTable.DefaultCell.Padding = 3;
                        pdfTable.WidthPercentage = 65;
                        pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                        string DAVID_TTF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "david.TTF");

                        BaseFont bf = BaseFont.CreateFont(DAVID_TTF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                        iTextSharp.text.Font f = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

                        Paragraph orderIdPara = new Paragraph(this.OrderID.ToString());
                        orderIdPara.Alignment = Element.ALIGN_CENTER;
                        orderIdPara.SpacingAfter = 10;

                        foreach (DataGridViewColumn column in ordersTable.Columns)
                        {
                            if (column.Index == 0) continue;
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, f));
                            pdfTable.AddCell(cell);
                        }

                        foreach (DataGridViewRow row in ordersTable.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.ColumnIndex == 0) continue;
                                pdfTable.AddCell(new Phrase( cell.Value.ToString(), f ) );
                            }
                        }

                        using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            pdfDoc.Add(orderIdPara);
                            pdfDoc.Add(pdfTable);
                            pdfDoc.Close();
                            stream.Close();
                        }
                        var path = sfd.FileName;
                        using (var document = PdfiumViewer.PdfDocument.Load(path))
                        {
                            using (var printDocument = document.CreatePrintDocument())
                            {
                                printDocument.PrinterSettings.PrintFileName = sfd.FileName;
                                printDocument.PrinterSettings.PrinterName = "HP Officejet Pro 8610 (Network)";
                                printDocument.DocumentName = this.OrderID.ToString()+".pdf";
                                printDocument.PrinterSettings.PrintFileName = sfd.FileName;
                                printDocument.PrintController = new  StandardPrintController();
                                printDocument.Print();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BiggerMessage.Show("Error:" + ex.Message);
                    }
                }
            }
        }
        public void SetTotalPrice(float finalPrice)
        {
            if (this.SourceControl != null)
                this.SourceControl.UpdateFinalPrice(finalPrice);
            this.TotalFinalPrice = finalPrice;
            textTotalPriceNew.Text = finalPrice < 0 ? "-" : string.Format("₪{0}", finalPrice.ToString());
        }
    }
}
