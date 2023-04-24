using FluentFTP;
using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ZaretnaPanel
{
    public partial class ProductsImages : Form
    {

        string FileName = "N/A";

        private int CurrentProductID = -1;
        private int VersionID = -1;
        public ProductsImages()
        {
            InitializeComponent();
            ViewProduct("FIRST");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ViewProduct("BEFORE");
        }


        private void ViewProduct(string CASE)
        {
            if (CASE == "FIRST")
            {
                string cmd = "SELECT ID, A_NAME, H_NAME, IMG_VERSION FROM products WHERE REMOVED!=2 LIMIT 1";
                DataTable dt = new API_CLASS().API_QUERY(cmd);
                if (dt.Rows.Count == 0)
                {
                    BiggerMessage.Show("لا يوجد منتجات. الرجاء إضافة منتجات");
                    arabicName.Text = "";
                    hebrewName.Text = "";
                    CurrentProductID = -1;
                }
                else
                {
                    CurrentProductID = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                    arabicName.Text = dt.Rows[0].Field<string>(1);
                    hebrewName.Text = dt.Rows[0].Field<string>(2);
                    this.VersionID = Convert.ToInt32(dt.Rows[0].Field<string>(3));
                    try
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() + CurrentProductID + ".jpg?v=" + this.VersionID);
                    }
                    catch
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL()+"unknown.jpg");
                    }
                }
            }
            else if (CASE == "AFTER")
            {
                string cmd = "SELECT ID, A_NAME, H_NAME, IMG_VERSION FROM products WHERE ID>" + CurrentProductID + " AND REMOVED!=2 LIMIT 1";
                DataTable dt = new API_CLASS().API_QUERY(cmd);

                if (dt.Rows.Count == 0)
                {
                    ViewProduct("FIRST");
                }
                else
                {
                    CurrentProductID = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                    arabicName.Text = dt.Rows[0].Field<string>(1);
                    hebrewName.Text = dt.Rows[0].Field<string>(2);
                    this.VersionID = Convert.ToInt32(dt.Rows[0].Field<string>(3));
                    try
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() + CurrentProductID + ".jpg?v=" + this.VersionID);
                    }
                    catch
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL()+"unknown.jpg");
                    }
                }
            }
            else if (CASE == "BEFORE")
            {
                string cmd = "SELECT ID, A_NAME, H_NAME, IMG_VERSION FROM products WHERE ID<" + CurrentProductID + " AND REMOVED!=2 ORDER BY ID DESC LIMIT 1";
                DataTable dt = new API_CLASS().API_QUERY(cmd);
                if (dt.Rows.Count == 0)
                {
                    ViewProduct("LAST");
                }
                else
                {
                    CurrentProductID = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                    arabicName.Text = dt.Rows[0].Field<string>(1);
                    hebrewName.Text = dt.Rows[0].Field<string>(2);
                    this.VersionID = Convert.ToInt32(dt.Rows[0].Field<string>(3));
                    try
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() + CurrentProductID + ".jpg?v=" + this.VersionID);
                    }
                    catch
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL()+"unknown.jpg");
                    }
                }
            }
            else if (CASE == "LAST")
            {
                string cmd = "SELECT ID, A_NAME, H_NAME, IMG_VERSION FROM products WHERE REMOVED!=2 ORDER BY ID DESC LIMIT 1";

                DataTable dt = new API_CLASS().API_QUERY(cmd);
                if (dt.Rows.Count == 0)
                {
                    BiggerMessage.Show("لا يوجد منتجات. الرجاء إضافة منتجات");
                    arabicName.Text = "";
                    hebrewName.Text = "";
                    CurrentProductID = -1;

                }
                else
                {
                    CurrentProductID = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                    arabicName.Text = dt.Rows[0].Field<string>(1);
                    hebrewName.Text = dt.Rows[0].Field<string>(2);
                    this.VersionID = Convert.ToInt32(dt.Rows[0].Field<string>(3));
                    try
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() + CurrentProductID + ".jpg?v="+this.VersionID);
                    }
                    catch
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL()+"unknown.jpg");
                    }
                }
            }
            FileName = "N/A";
            ProductID.Text = CurrentProductID.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewProduct("AFTER");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.jpg)|*.jpg";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Path.GetExtension(FileName) != ".jpg")
                    MessageBox.Show("lol");
                else
                {
                    Image bitmap = Image.FromFile(fileDialog.FileName);
                    FileName = fileDialog.FileName;
                    pictureBox1.Image = bitmap;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentProductID != -1 && FileName != "N/A")
            {
                API_CLASS api = new API_CLASS();
                this.VersionID++;
                api.API_QUERY_EXECUTE(string.Format("UPDATE products SET IMG_VERSION={0} WHERE ID={1}", this.VersionID, this.CurrentProductID));
                api.API_UPLOAD_PRODUCT_IMAGE(FileName, "/" + CurrentProductID.ToString() + ".jpg");
                FileName = "N/A";
                BiggerMessage.Show("لقد تم حفظ الصورة", 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ViewProduct("AFTER");
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
            ViewProduct("BEFORE");
        }

        private void btnNavi_Click(object sender, EventArgs e)
        {
            int tmpValue;
            bool isInt = int.TryParse(ProductID.Text, out tmpValue);
            if (isInt && tmpValue > 0)
            {
                string cmd = "SELECT ID, A_NAME, H_NAME, IMG_VERSION FROM products WHERE REMOVED!=2 AND ID="+tmpValue;

                DataTable dt = new API_CLASS().API_QUERY(cmd);
                if (dt.Rows.Count == 0)
                {
                    BiggerMessage.Show("هذا المنتج غير موجود");
                    ProductID.Text = CurrentProductID.ToString();
                }
                else
                {
                    CurrentProductID = Convert.ToInt32(dt.Rows[0].Field<string>(0));
                    arabicName.Text = dt.Rows[0].Field<string>(1);
                    hebrewName.Text = dt.Rows[0].Field<string>(2);
                    ProductID.Text = CurrentProductID.ToString();
                    this.VersionID = Convert.ToInt32(dt.Rows[0].Field<string>(3));
                    try
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() + CurrentProductID + ".jpg?v="+this.VersionID);
                    }
                    catch
                    {
                        pictureBox1.Load(new API_CLASS().ReturnImageURL() +"unknown.jpg");
                    }
                }
            }
            else
            {
                BiggerMessage.Show("هذه ليست قيمة صالحة");
                ProductID.Text = CurrentProductID.ToString();
            }
        }
    }
}
