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
    public partial class ManagePackageInfo : Form
    {
        private DataRow ProductDataRow;
        public ManagePackageInfo(DataRow PDR, string strArabic, string strHebrew, string strRussian = "n/a")
        {
            InitializeComponent();

            this.ProductDataRow = PDR;
            arabicProducts.RowHeadersVisible = false;
            arabicProducts.Columns.Add("name", "פריט");
            arabicProducts.Columns.Add("quantity", "כמות");

            hebrewProducts.Columns.Add("name", "פריט");
            hebrewProducts.Columns.Add("quantity", "כמות");

            russianProducts.Columns.Add("name", "פריט");
            russianProducts.Columns.Add("quantity", "כמות");

            string arabicProductsString = strArabic;
            string hebrewProductsString = strHebrew;
            string russianProductsString = strRussian;

            if (arabicProductsString != "n/a")
            {
                string[] ProductsArray = arabicProductsString.Split('&');
                string[] productAndQuantity;
                for (int i = 0; i < ProductsArray.Length; i++)
                {
                    productAndQuantity = ProductsArray[i].Split('×');
                    arabicProducts.Rows.Add(productAndQuantity[0], productAndQuantity[1]);
                }
            }
            if (hebrewProductsString != "n/a")
            {
                string[] ProductsArray = hebrewProductsString.Split('&');
                string[] productAndQuantity;
                for (int i = 0; i < ProductsArray.Length; i++)
                { 
                    productAndQuantity = ProductsArray[i].Split('×');
                    hebrewProducts.Rows.Add(productAndQuantity[0], productAndQuantity[1]);
                }
            }
            if (russianProductsString != "n/a")
            {
                string[] ProductsArray = russianProductsString.Split('&');
                string[] productAndQuantity;
                for (int i = 0; i < ProductsArray.Length; i++)
                {
                    productAndQuantity = ProductsArray[i].Split('×');
                    russianProducts.Rows.Add(productAndQuantity[0], productAndQuantity[1]);
                }
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string packageInfoArabic = "";
            foreach(DataGridViewRow row in arabicProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells[0].Value == null || row.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[0].Value.ToString())) continue;
                    if (row.Cells[1].Value == null || row.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[1].Value.ToString())) continue;

                    if (row.Index > 0) packageInfoArabic += "&";
                    packageInfoArabic += MySqlHelper.EscapeString(row.Cells[0].Value.ToString().Trim(' ').Replace('&','+')) + " × " + MySqlHelper.EscapeString(row.Cells[1].Value.ToString().Trim(' ').Replace('&', '+'));
                }
            }
            // building hebrew string
            string packageInfoHebrew = "";
            foreach (DataGridViewRow row in hebrewProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells[0].Value == null || row.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[0].Value.ToString())) continue;
                    if (row.Cells[1].Value == null || row.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[1].Value.ToString())) continue;

                    if (row.Index > 0) packageInfoHebrew += "&";
                    packageInfoHebrew += MySqlHelper.EscapeString(row.Cells[0].Value.ToString().Trim(' ').Replace('&', '+')) + " × " + MySqlHelper.EscapeString(row.Cells[1].Value.ToString().Trim(' ').Replace('&', '+'));
                }
            }

            // building russian string
            string packageInfoRussian = "";
            foreach (DataGridViewRow row in russianProducts.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells[0].Value == null || row.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[0].Value.ToString())) continue;
                    if (row.Cells[1].Value == null || row.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[1].Value.ToString())) continue;

                    if (row.Index > 0) packageInfoRussian += "&";
                    packageInfoRussian += MySqlHelper.EscapeString(row.Cells[0].Value.ToString().Trim(' ').Replace('&', '+')) + " × " + MySqlHelper.EscapeString(row.Cells[1].Value.ToString().Trim(' ').Replace('&', '+'));
                }
            }

            if (packageInfoArabic.Length == 0) packageInfoArabic = "n/a";
            if (packageInfoHebrew.Length == 0) packageInfoHebrew = "n/a";
            if (packageInfoRussian.Length == 0) packageInfoRussian = "n/a";

            // new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET INCLUDED_IN_DISCOUNT={0}, NOTE='{1}' WHERE ID={2}", includedinDiscount, MySqlHelper.EscapeString(finalNoteString), this.ProductID));

            this.ProductDataRow.SetField<string>("H_INFO", packageInfoHebrew);
            this.ProductDataRow.SetField<string>("A_INFO", packageInfoArabic);
            this.ProductDataRow.SetField<string>("RU_INFO", packageInfoRussian);
            this.Close();
        }

        private void ManagePackageInfo_Load(object sender, EventArgs e)
        {
            arabicProducts.DoubleBuffered(true);
            hebrewProducts.DoubleBuffered(true);
        }
    }
}
