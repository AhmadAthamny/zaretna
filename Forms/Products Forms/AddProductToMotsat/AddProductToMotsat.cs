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

namespace ZaretnaPanel
{
    public partial class AddProductToMotsat : Form
    {
        private int LastProductID = -1;
        private bool WasBefore = false;
        private string A_NAME;
        private float PRICE;
        private float UNIT_VALUE;
        private float CUSTOM_ADD;
        private MostatForm SourceForm;
        public AddProductToMotsat(MostatForm tmpSource)
        {
            InitializeComponent();
            this.SourceForm = tmpSource;
        }

        private void lblArabic_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int tmpValue;
            if(int.TryParse(txtID.Text, out tmpValue))
            {
                DataTable dt = new API_CLASS().API_QUERY("SELECT A_NAME, H_NAME, MOTSAT_ID, PRICE, UNIT_VALUE, CUSTOM_ADD FROM products WHERE REMOVED!=2 AND ID=" + tmpValue);
                if (dt.Rows.Count > 0)
                {
                    WasBefore = dt.Rows[0].Field<string>(2) == "-1" ? false : true;
                    if (WasBefore)
                    {
                        BiggerMessage.Show("هذا المنتج موجود بالقائمة مسبقاً");
                        txtID.Text = "";
                    }
                    else
                    {
                        lblArabic.Text = dt.Rows[0].Field<string>(0);
                        lblHebrew.Text = dt.Rows[0].Field<string>(1);
                        A_NAME = dt.Rows[0].Field<string>(0);
                        PRICE = float.Parse(dt.Rows[0].Field<string>(3));
                        UNIT_VALUE = float.Parse(dt.Rows[0].Field<string>(4));
                        CUSTOM_ADD = float.Parse(dt.Rows[0].Field<string>(5));
                        LastProductID = tmpValue;
                    }
                }
                else
                {
                    LastProductID = -1;
                    lblArabic.Text = "منتج غير معروف";
                    lblHebrew.Text = "";
                    txtID.Text = "";
                }
            }
            else
            {
                LastProductID = -1;
                lblArabic.Text = "منتج غير معروف";
                lblHebrew.Text = "";
                txtID.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LastProductID == -1)
            {
                BiggerMessage.Show("قم بإختيار منتج أولاً");
            }
            else
            {
                if (string.IsNullOrEmpty(motsatName.Text) || string.IsNullOrWhiteSpace(motsatName.Text))
                {
                    BiggerMessage.Show("قم بتسجيل إسم المنتج بالمجلس");
                }
                else
                {
                    string motsatname = motsatName.Text;
                    motsatname = motsatname.Replace("delete", "");
                    motsatname = motsatname.Replace("update", "");
                    motsatname = motsatname.Replace("insert", "");
                    new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET MOTSAT_NAME='{0}', MOTSAT_ID={1} WHERE ID={2}", MySqlHelper.EscapeString(motsatname), 9999, LastProductID));

                    float FINAL_PRICE, TEMP_VALUE;

                    TEMP_VALUE = (float)((PRICE + 1) * 1.5);
                    FINAL_PRICE = float.Parse((TEMP_VALUE * UNIT_VALUE).ToString());
                    SourceForm.productsTable.Rows.Add(LastProductID.ToString(), A_NAME, motsatname, PRICE, UNIT_VALUE, CUSTOM_ADD, FINAL_PRICE);
                    BiggerMessage.Show("تمت إضافة المنتج لقائمة المجلس", 1);
                    this.Close();
                }
            }
            

        }
    }
}
