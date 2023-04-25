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
    public partial class addNewCategoryForm : Form
    {
        private NewProductsForm SourceForm;
        private int EditingCategoryID;
        private int tmp_2ndIndex;
        public addNewCategoryForm(NewProductsForm parent, int categoryid = -1)
        {
            InitializeComponent();
            this.SourceForm = parent;
            this.EditingCategoryID = categoryid;

            if (this.EditingCategoryID != -1)
            {
                DataTable dt = new DataTable();
                LoadingForm LFRM = new LoadingForm();
                LFRM.Show(this);

                this.Text = "ערוך קטגוריה";

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (s, e) =>
                {
                    dt = new API_CLASS().API_QUERY("SELECT A_NAME, H_NAME, URL_name, 2ND_INDEX FROM categories WHERE ID=" + this.EditingCategoryID);
                };

                bw.RunWorkerCompleted += (s, e) =>
                {
                    txtArabicName.Text = dt.Rows[0].Field<string>(0);
                    txtHebrewName.Text = dt.Rows[0].Field<string>(1);
                    txtURL.Text = dt.Rows[0].Field<string>(2);
                    this.tmp_2ndIndex = int.Parse(dt.Rows[0].Field<string>(3));
                    LFRM.Close();
                    this.ShowDialog(this.SourceForm);
                    
                };
                bw.RunWorkerAsync();
            }

            else
                this.ShowDialog(this.SourceForm);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtArabicName.Text.Trim();
            txtHebrewName.Text.Trim();
            txtURL.Text.Trim();
            if(txtArabicName.TextLength < 2)
            {
                BiggerMessage.Show("שם בערבית קצר מאוד");
            }
            else if(txtHebrewName.TextLength < 2)
            {
                BiggerMessage.Show("שם בעברית קצר מאוד");
            }
            else if (txtURL.TextLength < 2)
            {
                BiggerMessage.Show("שם קיצור קצר מאוד");
            }
            else
            {
                if (this.EditingCategoryID == -1)
                {
                    API_CLASS api = new API_CLASS();
                    int ID = api.WP_addCategory(txtHebrewName.Text, txtImgUrl.Text);
                    if (ID > 0)
                    {
                        api.API_QUERY_EXECUTE(string.Format("INSERT INTO categories (URL_name, A_NAME, H_NAME, STATUS, 2ND_INDEX) VALUES ('{0}', '{1}', '{2}', {3}, {4})", MySqlHelper.EscapeString(txtURL.Text), MySqlHelper.EscapeString(txtArabicName.Text), MySqlHelper.EscapeString(txtHebrewName.Text), 1, ID));
                    }
                    else
                    {
                        BiggerMessage.Show("קישור תמונה צריך להיות תקין.");
                    }
                }
                else
                {
                    API_CLASS api = new API_CLASS();
                    api.API_QUERY_EXECUTE(string.Format("UPDATE categories SET A_NAME='{0}', H_NAME='{1}', URL_name='{2}' WHERE ID={3}", MySqlHelper.EscapeString(txtArabicName.Text), MySqlHelper.EscapeString(txtHebrewName.Text), MySqlHelper.EscapeString(txtURL.Text), this.EditingCategoryID));
                    api.WP_editCategory(this.tmp_2ndIndex, txtHebrewName.Text);
                }
                this.SourceForm.LoadAllCategories();
                this.Close();
            }
        }

        private void addNewCategoryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if(e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }
}
