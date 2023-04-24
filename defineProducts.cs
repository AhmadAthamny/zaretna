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
    public partial class defineProducts : Form
    {
        public defineProducts()
        {
            InitializeComponent();
            productsTable.Columns.Add("id", "#");
            productsTable.Columns.Add("hebrew", "שם בעברית");
            productsTable.Columns.Add("wc_id", "מספר פריט בוורדפרס");

            productsTable.Columns[0].Visible = false;

            productsTable.Columns[0].ReadOnly = true;
            productsTable.Columns[1].ReadOnly = true;

            DataTable dt = new API_CLASS().API_QUERY("SELECT ID, H_NAME FROM products WHERE WP_INDEX=-1 AND REMOVED!=2");
            foreach( DataRow row in dt.Rows )
            {
                productsTable.Rows.Add(int.Parse(row.Field<string>("ID")), row.Field<string>("H_NAME"));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string problems = "פריטים שלא נשמרו:"+ "\r\n";
            foreach ( DataGridViewRow row in productsTable.Rows)
            {
                if ( row.Cells[2].Value != null && row.Cells[2].Value != DBNull.Value && !string.IsNullOrWhiteSpace(row.Cells[2].Value.ToString()))
                {
                    int id = int.Parse(row.Cells[0].Value.ToString()), wp_index;
                    if ( int.TryParse(row.Cells[2].Value.ToString().Trim(), out wp_index) )
                    {
                        new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE products SET WP_INDEX={0} WHERE ID={1}", wp_index, id));
                        continue;
                    }
                }
                problems += row.Cells[1].Value.ToString() + "\r\n";
            }
            this.Close();
            BiggerMessage.Show(problems);
        }
    }
}
