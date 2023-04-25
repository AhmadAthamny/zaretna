using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ZaretnaPanel
{
    public partial class ManageProductDiscountInfo : Form
    {
        private int ProductID;
        private bool IsAddingNewDiscount;
        public ManageProductDiscountInfo(int tmpProductID, bool adding = true)
        {
            InitializeComponent();
            this.ProductID = tmpProductID;
            this.IsAddingNewDiscount = adding;

            DataTable productInfo = new API_CLASS().API_QUERY("SELECT A.ID, A.A_NAME, A.H_NAME, A.FINAL_PRICE, A.DISCOUNT_QUANTITY, A.DISCOUNT_PRICE, A.DISCOUNT_NAME, B.A_NAME AS UNIT_NAME, A.UNIT_VALUE, A.CUSTOM_ADD, A.PRICE FROM products A LEFT JOIN unit_names B ON A.UNIT_ID = B.ID WHERE A.ID=" + this.ProductID);
            DataTable discountName = (DataTable)JsonConvert.DeserializeObject("[" + productInfo.Rows[0].Field<string>(6) + "]", (typeof(DataTable)));

            this.productID.Text = productInfo.Rows[0].Field<string>(0);
            this.product_A_NAME.Text = productInfo.Rows[0].Field<string>(1);
            this.product_H_NAME.Text = productInfo.Rows[0].Field<string>(2);
            this.txtDiscountPrice.Text = productInfo.Rows[0].Field<string>(5);
            this.txtDiscountQuantity.Value = Convert.ToInt32(productInfo.Rows[0].Field<string>(4)) == -1 ? 1 : Convert.ToInt32(productInfo.Rows[0].Field<string>(4));
            this.txtDiscountName.Text = discountName.Rows[0].Field<string>(0);
            this.txtHebrewDiscountName.Text = discountName.Rows[0].Field<string>(1);
            if (float.Parse(productInfo.Rows[0].Field<string>(8)) != 1)
            {
                this.txtUnitName.Text = "ק\"ג";
                this.txtFinalPrice.Text = ((float.Parse(productInfo.Rows[0].Field<string>(9))/100+1)*float.Parse(productInfo.Rows[0].Field<string>(10))).ToString();
            }
            else
            {
                this.txtUnitName.Text = productInfo.Rows[0].Field<string>(7);
                this.txtFinalPrice.Text = productInfo.Rows[0].Field<string>(3);
            }

            if (this.IsAddingNewDiscount)
            {
                btnRemoveDiscount.Enabled = false;
                btnRemoveDiscount.Visible = false;
                btnSaveChanges.Text = "إضافة المنتج";
            }           
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            float tmpDiscountPrice;
            if(float.TryParse(this.txtDiscountPrice.Text, out tmpDiscountPrice))
            {
                if (!string.IsNullOrEmpty(txtDiscountName.Text) && !string.IsNullOrWhiteSpace(txtDiscountName.Text) && !string.IsNullOrEmpty(txtHebrewDiscountName.Text) && !string.IsNullOrWhiteSpace(txtHebrewDiscountName.Text))                
                {
                    string arabicDiscountName = txtDiscountName.Text.Trim().Replace("\"", "\\\"").Replace("'", "\'");
                    string hebrewDiscountName = txtHebrewDiscountName.Text.Trim().Replace("\"", "\\\"").Replace("'", "\'");
                    string finalString = MySqlHelper.EscapeString(string.Format("{{\"arabic\": \"{0}\", \"hebrew\":\"{1}\"}}", arabicDiscountName, hebrewDiscountName));

                    new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET DISCOUNT_QUANTITY={0}, DISCOUNT_PRICE={1}, DISCOUNT_NAME='{2}', PRICE_UPDATED=2 WHERE ID={3}", Convert.ToInt32(txtDiscountQuantity.Value), tmpDiscountPrice, finalString, this.ProductID));
                    if (this.IsAddingNewDiscount) BiggerMessage.Show("لقد تمت إضافة التخفيض", 1);
                    else BiggerMessage.Show("لقد تم حفظ التغييرات", 1);

                    if (Application.OpenForms.OfType<ProductsDiscounts>().Count() == 1)
                        Application.OpenForms.OfType<ProductsDiscounts>().First().AddProductToTable(this.ProductID, this.product_H_NAME.Text, float.Parse(this.txtFinalPrice.Text), Convert.ToInt32(this.txtDiscountQuantity.Value), float.Parse(this.txtDiscountPrice.Text.ToString()), arabicDiscountName);

                    this.Close();
                }
                else
                {
                    BiggerMessage.Show("الرجاء كتابة إسم للتخفيض باللغتين");
                }
            }
            else
            {
                BiggerMessage.Show("الرجاء كتابة قيمة طبيعية");
            }
        }

        private void btnRemoveDiscount_Click(object sender, EventArgs e)
        {
            if (this.IsAddingNewDiscount)
                BiggerMessage.Show("لا يمكن حذف المنتج بحيث لم تتم إضافته بعد");
            else
            {
                string deletedDiscountName = "{\"arabic\": null, \"hebrew\":null}";
                new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET DISCOUNT_QUANTITY=-1, DISCOUNT_PRICE=(0.00), DISCOUNT_NAME='{0}', PRICE_UPDATED=3 WHERE ID={1}", MySqlHelper.EscapeString(deletedDiscountName), this.ProductID));
                BiggerMessage.Show("لقد تم حذف المنتج من التخفيضات", 1);

                if (Application.OpenForms.OfType<ProductsDiscounts>().Count() == 1)
                    Application.OpenForms.OfType<ProductsDiscounts>().First().RemoveProductFromTable(this.ProductID);

                this.Close();
            }
        }

        private void btnShekel_Click(object sender, EventArgs e)
        {
            txtDiscountName.Text += "₪";
        }

        private void hebrewBtnShekel_Click(object sender, EventArgs e)
        {
            txtHebrewDiscountName.Text += "₪";
        }
    }
}
