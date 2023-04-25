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
    public partial class TotalOrdersForm : Form
    {
        public TotalOrdersForm()
        {
            InitializeComponent();

            productsTable.Columns.Add("prdctName", "שם פריט");
            productsTable.Columns.Add("required_units","סה\"כ ביחידות");
            FillOrderInformation();
        }

        public void FillOrderInformation()
        {
            API_CLASS api = new API_CLASS();
            string cmd = "SELECT a.H_NAME as NAME, c.H_NAME AS 'UNIT_NAME', SUM(b.QUANTITY*b.UNIT_VALUE) as TOTAL, SUM(b.QUANTITY) AS 'QUANTITY' FROM products a INNER JOIN orders_products b on a.ID = b.PRODUCT_ID LEFT JOIN unit_names c ON a.UNIT_ID = c.ID WHERE b.ORDER_ID IN (SELECT ID FROM orders WHERE STATUS=0 AND TOTAL_PRICE=-1) GROUP BY b.PRODUCT_ID";

            DataTable dt = api.API_QUERY(cmd);

            productsTable.Rows.Clear();

            string requiredInKilos;
            foreach (DataRow row in dt.Rows)
            {
                if (row.Field<string>(2) == row.Field<string>(3))
                    requiredInKilos = "";
                else
                    requiredInKilos = " (" + float.Parse(row.Field<string>(2)).ToString("F2") + " ק\"ג)";

                productsTable.Rows.Add(row.Field<string>(0), row.Field<string>(3) + " " + row.Field<string>(1)+requiredInKilos);
            }
        }

        public void SendTotalToEmail()
        {
            string cmd = "SELECT a.H_NAME as 'NAME', c.H_NAME as 'UNIT_NAME', SUM(b.QUANTITY*b.UNIT_VALUE) as 'TOTAL', SUM(b.QUANTITY) as 'QUANTITY', b.UNIT_VALUE as 'WEIGHT' FROM products a INNER JOIN orders_products b on a.ID = b.PRODUCT_ID" +
                " LEFT JOIN unit_names c ON a.UNIT_ID=c.ID" +
                " WHERE b.ORDER_ID IN (SELECT ID FROM orders WHERE STATUS=0 AND TOTAL_PRICE=-1) GROUP BY b.PRODUCT_ID";

            API_CLASS api = new API_CLASS();

            DataTable dt = api.API_QUERY(cmd);

            cmd = String.Format("UPDATE global_variables SET `VALUE_STRING`='{0}' WHERE ID=1", DateTime.Now);
            api.API_QUERY_EXECUTE(cmd);

            string Content = "";
            Content += "<font size=\"3\">";
            Content += "<center><div style=\"border: 1px solid black; \"><table width=\"75%\" dir=\"rtl\">";
            Content += "<b>";
            Content += "<tr>";
            Content += "<th>פריט</th>";
            Content += "<th>נדרש ביחידות</th>";
            Content += "</tr></b>";

            string requiredinUnits,
                requiredinKilos
                ;
            foreach(DataRow row in dt.Rows)
            {
                requiredinUnits = row.Field<string>(3) + " " + row.Field<string>(1);
                if (float.Parse(row.Field<string>("WEIGHT")) != 1)
                {
                    requiredinKilos = " ("+(float.Parse(row.Field<string>("WEIGHT")) * float.Parse(row.Field<string>("QUANTITY"))).ToString("F2")+ " ק\"ג)";
                }
                else requiredinKilos = "";
                Content += "<tr>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + row.Field<string>(0) + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + requiredinUnits + requiredinKilos+ "</td>";
                Content += "</tr>";
            }
            Content += "</table></div></center></font>";

            List<string> emails = new List<string>();
            emails.Add("a.m.datamny@gmail.com");

            EmailSend eSend = new EmailSend("סיכום פריטים שהוזמנו", emails, "פריטים שהוזמנו", Content);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SendTotalToEmail();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FillOrderInformation();
        }

        private void TotalOrdersForm_Load(object sender, EventArgs e)
        {
            productsTable.DoubleBuffered(true);
        }
    }
}
