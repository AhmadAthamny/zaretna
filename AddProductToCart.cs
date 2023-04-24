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
    public partial class AddProductToCart : Form
    {
        OrderInfo sourceForm;
        public AddProductToCart(OrderInfo tmp_sourceForm)
        {
            InitializeComponent();
            this.sourceForm = tmp_sourceForm;
            productsTable.DataSource = new API_CLASS().API_QUERY("SELECT A.ID, A.A_NAME, A.H_NAME, B.H_NAME AS UNIT_NAME, A.FINAL_PRICE FROM products A LEFT JOIN unit_names B ON A.UNIT_ID = B.ID WHERE A.REMOVED=0 ORDER BY A.H_NAME ASC");

            if(productsTable.Rows.Count > 0)
            {
                productsTable.Columns[0].HeaderText = "#";
                productsTable.Columns[1].HeaderText = "שם בערבית";
                productsTable.Columns[2].HeaderText = "שם בעברית";
                productsTable.Columns[3].HeaderText = "שם יחידה";
                productsTable.Columns[4].HeaderText = "מחיר";

                productsTable.Columns[0].FillWeight = 10;
                productsTable.Columns[1].FillWeight = 25;
                productsTable.Columns[2].FillWeight = 25;
                productsTable.Columns[3].FillWeight = 20;
                productsTable.Columns[4].FillWeight = 15;
            }
            productsTable.ReadOnly = true;
        }

        private void txtAdd_Click(object sender, EventArgs e)
        {
            float quantity;
            if(float.TryParse(txtQuantity.Text, out quantity))
            {
                int theProductID;
                if (int.TryParse(txtProductID.Text, out theProductID))
                {
                    foreach (DataGridViewRow row in productsTable.Rows)
                    {
                        if (int.Parse(row.Cells[0].Value.ToString()) == int.Parse(txtProductID.Text))
                        {
                            sourceForm.AddProduct(int.Parse(txtProductID.Text), float.Parse(quantity.ToString("F2")));
                            this.Close();
                            return;
                        }
                    }
                    BiggerMessage.Show("פריט לא קיים");
                    return;
                }
                else
                {
                    BiggerMessage.Show("פריט לא קיים");
                }
            }
            else
            {
                BiggerMessage.Show("ערך כמות לא חוקי");
            }
        }

        private void productsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                txtProductID.Text = productsTable.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void AddProductToCart_Load(object sender, EventArgs e)
        {
            productsTable.DoubleBuffered(true);
        }
    }
}
