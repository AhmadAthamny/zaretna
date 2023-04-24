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
using System.IO;
using System.Net;

namespace ZaretnaPanel
{
    public partial class AddNewProductForm : Form
    {
        private int ProductID;
        public DataRow ProductDataRow = null;
        private Form parentForm;
        private ProductControl ProductControlSource;
        private LoadingForm LFRM = new LoadingForm();
        private int[] UnitIDs = null;
        private bool didLoading = false;
        private BackgroundWorker savingBackground = null;
        private string ImageFileName = "N/A";
        private int CategoryID = -1;
        public AddNewProductForm(int productid, Form parent, int categoryid, ProductControl source = null)
        {
            InitializeComponent();
            this.ProductID = productid;
            this.parentForm = parent;
            this.ProductControlSource = source;
            this.CategoryID = categoryid;

            if (this.ProductID != -1)
            {
                this.Text = "ערוך מוצר";
                LFRM.Show(this);

                BackgroundWorker bw = new BackgroundWorker();
                DataTable dt = new DataTable();
                bw.DoWork += (s, e) =>
                {
                    dt = new API_CLASS().API_QUERY("SELECT A.*, B.H_INFO, B.A_INFO, B.RU_INFO FROM products A LEFT JOIN products_info B ON A.ID = B.PRODUCT_ID WHERE ID=" + this.ProductID);
                };
                bw.RunWorkerCompleted += (s, e) =>
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        this.ProductDataRow = dt.Rows[0];
                        FillProductData();
                    }
                    else
                    {
                        BiggerMessage.Show("יש שגיאה, בדוק את החיבור לרשת");
                    }
                };
                bw.RunWorkerAsync();
            }
            else
                this.ShowDialog(this.parentForm);
        }

        private void AddNewProductForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private void FillProductData()
        {
            if(this.ProductDataRow != null)
            {
                // product name
                txtArName.Text = this.ProductDataRow.Field<string>("A_NAME");
                txtHeName.Text = this.ProductDataRow.Field<string>("H_NAME");
               
                // product notes
                DataTable productNotes = (DataTable)JsonConvert.DeserializeObject("[" + ProductDataRow.Field<string>("NOTE") + "]", (typeof(DataTable)));
                txtArNotes.Text = productNotes.Rows[0].IsNull("arabic") ? " " : productNotes.Rows[0].Field<string>("arabic");
                txtHeNotes.Text = productNotes.Rows[0].IsNull("hebrew") ? " " : productNotes.Rows[0].Field<string>("hebrew");

                // product prices
                txtKiloPrice.Text = (decimal.Parse(this.ProductDataRow.Field<string>("PRICE")).ToString("F2"));
                txtProfitPercentage.Text = decimal.Parse(this.ProductDataRow.Field<string>("CUSTOM_ADD")).ToString();
                txtFinalPrice.Text = decimal.Parse(this.ProductDataRow.Field<string>("FINAL_PRICE")).ToString("F2");
                txtKiloPrice.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtProfitPercentage.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtFinalPrice.KeyDown += new KeyEventHandler(OnHitEnterOnControl);

                // product unit and weight
                cbIsWeighable.Checked = int.Parse(this.ProductDataRow.Field<string>("WEIGH_ABLE")) == 1 ? true : false;
                cbIsWeighable.CheckedChanged += new EventHandler(cbIsWeighable_CheckedChanged);
                if (!cbIsWeighable.Checked)
                {
                    txtWeight.Enabled = false; 
                    txtWeight.Text = "";
                }
                else
                    txtWeight.Text = this.ProductDataRow.Field<string>("UNIT_VALUE");
                txtWeight.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                FillUnitsComboBox(int.Parse(this.ProductDataRow.Field<string>("UNIT_ID")));

                // product quantites and step
                txtMinQty.Text = ProductDataRow.Field<string>("MIN_QTY");
                txtStep.Text = ProductDataRow.Field<string>("STEP");
                txtMaxQty.Text = float.Parse(ProductDataRow.Field<string>("MAX_QTY")) == -1 ? "" : float.Parse(ProductDataRow.Field<string>("MAX_QTY")).ToString("F1");
                txtMinQty.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtStep.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtMaxQty.KeyDown += new KeyEventHandler(OnHitEnterOnControl);

                // product package info
                cbIsPackage.Checked = !ProductDataRow.IsNull("H_INFO") || !ProductDataRow.IsNull("A_INFO") || !ProductDataRow.IsNull("RU_INFO");
                if (!cbIsPackage.Checked)
                    btnEditPackage.Enabled = false;
                cbIsPackage.CheckedChanged += new EventHandler(OnIsPackageCheckChanged);
                btnEditPackage.Click += new EventHandler(OnEditPackageButtonClicked);

                // product discounts
                RefreshDiscountControls();
                txtDiscountQty.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtDiscountPrice.KeyDown += new KeyEventHandler(OnHitEnterOnControl);
                txtMaxDiscount.KeyDown += new KeyEventHandler(OnHitEnterOnControl);

                // product removed or no
                if (int.Parse(ProductDataRow.Field<string>("REMOVED")) == 0)
                    cbIsActive.Checked = true;
                else
                    cbIsActive.Checked = false;
            }
        }
        public void FillUnitsComboBox(int unitid = -1)
        {
            BackgroundWorker bw = new BackgroundWorker();
            DataTable dt = null;
            int itemSelected = -1;

            this.cbUnitName.Enabled = false;
            this.loadingIcon.Visible = true;
            this.lblEditUnits.Visible = false;

            bw.DoWork += (s, e) =>
            {
                dt = new API_CLASS().API_QUERY("SELECT ID, H_NAME FROM unit_names");
                this.UnitIDs = new int[dt.Rows.Count];
            };
            bw.RunWorkerCompleted += (s, e) =>
            {
                this.cbUnitName.Enabled = true;
                this.loadingIcon.Visible = false;
                this.lblEditUnits.Visible = true;

                cbUnitName.Items.Clear();

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cbUnitName.Items.Add(dt.Rows[i].Field<string>("H_NAME"));
                    this.UnitIDs[i] = int.Parse(dt.Rows[i].Field<string>("ID"));
                    if (this.UnitIDs[i] == unitid)
                        itemSelected = cbUnitName.Items.Count - 1;
                }
                if (itemSelected == -1) 
                    this.cbUnitName.SelectedText = "unidentified";
                else
                    this.cbUnitName.SelectedIndex = itemSelected;

                if (unitid == 1)
                {
                    txtWeight.Text = " ";
                    txtWeight.Enabled = false;
                    cbIsWeighable.Checked = true;
                    cbIsWeighable.Enabled = false;
                }
                UpdatePricesUnits();
            };
            bw.RunWorkerAsync();
        }

        private void cbUnitName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbUnitName.SelectedIndex == 0)
            {
                cbIsWeighable.Checked = true;
                cbIsWeighable.Enabled = false;

                txtWeight.Enabled = false;

                txtWeight.Text = " ";
                errorProvider1.SetError(txtWeight, "");
                RecalculateProductPrices(4);
            }
            else
            {
                cbIsWeighable.Enabled = true;
                if (this.cbIsWeighable.Checked)
                {
                    this.txtWeight.Enabled = true;
                    this.txtWeight.Text = GetProductWeight().ToString("F2");
                    txtWeight_Leave(null, null);
                }
                RecalculateProductPrices(4);
            }
            UpdatePricesUnits();
        }

        private void cbIsWeighable_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.cbIsWeighable.Checked)
            {
                if (this.cbUnitName.SelectedIndex == 0)
                {
                    this.cbIsWeighable.Checked = true;
                    errorProvider1.SetError(cbIsWeighable, "לא יכול להיות \"לא שקיל\" עם יחידת ק\"ג");
                }
                else
                {
                    this.txtWeight.Text = "";
                    this.txtWeight.Enabled = false;
                    RecalculateProductPrices(4);
                    errorProvider1.SetError(cbIsWeighable, "");
                    errorProvider1.SetError(txtWeight, "");
                }
            }
            else
            {
                this.txtWeight.Text = "1.00";
                this.txtWeight.Enabled = true;
                RecalculateProductPrices(4);
                errorProvider1.SetError(cbIsWeighable, "");
                errorProvider1.SetError(txtWeight, "");
            }
            UpdatePricesUnits();
        }
        private void UpdatePricesUnits()
        {
            string unitname = this.cbUnitName.Text;
            bool isKilo = this.cbUnitName.SelectedIndex == 0 ? true : false;
            bool isWeighable = this.cbIsWeighable.Checked || isKilo;

            if(isWeighable)
            {
                this.lblKiloPrice.Text = string.Format("מחיר לקילו:");
                if (isKilo)
                    this.lblFinalPrice.Text = string.Format("מחיר סופי לקילו:");
                else
                    this.lblFinalPrice.Text = string.Format("מחיר סופי ל{0}:", unitname);
            }
            else
            {
                this.lblKiloPrice.Text = string.Format("מחיר ל{0}:", unitname);
                this.lblFinalPrice.Text = string.Format("מחיר סופי ל{0}:", unitname);
            }
            if (!this.Visible && !didLoading)
            {
                LFRM.Close();

                API_CLASS api = new API_CLASS();

                if (int.Parse(this.ProductDataRow.Field<string>("IMG_VERSION")) > 0)
                {
                    string fileName = api.zaretnaProductsFolder + string.Format(@"\{0}-{1}.jpg", this.ProductID, this.ProductDataRow.Field<string>("IMG_VERSION"));
                    if (File.Exists(fileName))
                        this.picProductImage.LoadAsync(fileName);

                    else
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFileAsync(new Uri(api.ReturnImageURL() + ProductID.ToString() + ".jpg"), fileName);
                            client.DownloadFileCompleted += (se, ev) =>
                            {
                                this.picProductImage.LoadAsync(fileName);
                            };
                        }
                    }
                }
                this.ShowDialog(this.parentForm); 
                this.didLoading = true;
            }
        }

        private void txtWeight_Leave(object sender, EventArgs e)
        {
            decimal weight;
            if(decimal.TryParse(this.txtWeight.Text.Trim(), out weight))
            {
                this.txtWeight.Text = weight.ToString();
                if(weight != 1 && this.cbUnitName.SelectedIndex == 0)
                {
                    errorProvider1.SetError(txtWeight, string.Format("רק ערך משקל \"1\" אפשרי ביחידת קילו.\r\nניתן לשנות את היחידה למשהו אחר בקודם."));
                }
                else
                {
                    if (weight > 0)
                    {
                        this.txtWeight.Text = weight.ToString("F2");
                        RecalculateProductPrices(4);
                        errorProvider1.SetError(txtWeight, "");
                    }
                    else
                    {
                        errorProvider1.SetError(txtWeight, string.Format("ערך משקל לא חוקי"));
                    }
                }
            }
            else
            {
                errorProvider1.SetError(txtWeight, "ערך לא חוקי למשקל");
            }
        }
        private void RecalculateProductPrices(int source)
        {
            // source = 1 => kiloprice
            // source = 2 => profit percentage
            // source = 3 => final price
            // source = 4 => weight
            decimal
                kiloprice = -1,
                profit,
                finalprice = -1,
                weight = -1,
                newPrice;

            if (cbIsWeighable.Checked)
            {
                if (!txtWeight.Enabled)
                    weight = 1;
            }
            else
                weight = 1;

            if (
                 decimal.TryParse(txtKiloPrice.Text, out kiloprice) && kiloprice > 0
                 && decimal.TryParse(txtProfitPercentage.Text, out profit)
                 && (source != 3 || ( source == 3 && decimal.TryParse(txtFinalPrice.Text, out finalprice) ) )
                 && (weight == 1 || decimal.TryParse(txtWeight.Text, out weight))
            )
            switch (source)
            {
                case 1:
                case 2:
                case 4:
                    newPrice = kiloprice * ((100 + profit) / 100) * weight;
                    txtFinalPrice.Text = newPrice.ToString("F2");
                    RecalculateProductPrices(3);
                    break;

                case 3:
                        // new percentage
                        decimal newPercent;

                        if (decimal.TryParse((((finalprice / weight) - kiloprice) / kiloprice * 100).ToString("F7"), out newPercent))
                        {

                            if (newPercent < 0)
                                errorProvider1.SetError(txtProfitPercentage, "אחוז לא חוקי");
                            else
                                errorProvider1.SetError(txtProfitPercentage, "");

                            txtProfitPercentage.Text = newPercent.ToString("F7");
                            errorProvider1.SetError(txtFinalPrice, "");
                        }
                        break;

                default:
                    break;
            }
        }
        private float GetProductWeight()
        {
            if (!this.cbIsWeighable.Checked || cbUnitName.SelectedIndex == 0)
                return 1;

            float temp;
            if (float.TryParse(this.txtWeight.Text, out temp))
                return temp;
            else
                return 1;
        }

        private void txtKiloPrice_Leave(object sender, EventArgs e)
        {
            decimal newKiloPrice;
            if(decimal.TryParse(txtKiloPrice.Text.Trim(), out newKiloPrice) && newKiloPrice > 0)
            {
                txtKiloPrice.Text = newKiloPrice.ToString("F2");
                RecalculateProductPrices(1);
                errorProvider1.SetError(txtKiloPrice, "");
            }
            else
            {
                errorProvider1.SetError(txtKiloPrice, string.Format(lblKiloPrice.Text + "\r\n" + "ערך לא חוקי"));
            }
        }

        private void txtProfitPercentage_Leave(object sender, EventArgs e)
        {
            decimal newPercentage;
            if (decimal.TryParse(txtProfitPercentage.Text.Trim(), out newPercentage))
            {
                txtProfitPercentage.Text = newPercentage.ToString("F7");
                RecalculateProductPrices(2);
                errorProvider1.SetError(txtProfitPercentage, "");
            }
            else
            {
                errorProvider1.SetError(txtProfitPercentage, "אחוז רווח לא חוקי");
            }
        }

        private void txtFinalPrice_Leave(object sender, EventArgs e)
        {
            decimal newFinalPrice;
            if(decimal.TryParse(txtFinalPrice.Text.Trim(), out newFinalPrice) && newFinalPrice > 0)
            {
                txtFinalPrice.Text = newFinalPrice.ToString("F2");
                RecalculateProductPrices(3);
                errorProvider1.SetError(txtFinalPrice, "");
            }
            else
            {
                errorProvider1.SetError(txtFinalPrice, "ערך מחיר סופי לא חוקי");
            }
        }

        private void txtMinQty_Leave(object sender, EventArgs e)
        {
            float newMinQty;
            if(float.TryParse(txtMinQty.Text.Trim(), out newMinQty) && newMinQty > 0)
            {
                txtMinQty.Text = newMinQty.ToString("F1");
                errorProvider1.SetError(txtMinQty, "");
            }
            else
            {
                errorProvider1.SetError(txtMinQty, "ערך כמות מינימום לא חוקי");
            }
        }

        private void txtMaxQty_Leave(object sender, EventArgs e)
        {
            float newMaxQty;
            if (float.TryParse(txtMaxQty.Text.Trim(), out newMaxQty))
            {
                if (newMaxQty <= 0)
                    txtMaxQty.Text = "";
                else
                    txtMaxQty.Text = newMaxQty.ToString("F1");

                errorProvider1.SetError(txtMaxQty, "");
            }
            else if(string.IsNullOrWhiteSpace(txtMaxQty.Text.Trim()))
            {
                txtMaxQty.Text = "";
                errorProvider1.SetError(txtMaxQty, "");
            }
            else
            {
                errorProvider1.SetError(txtMaxQty, "ערך כמות מקסימום לא חוקי");
            }
        }

        private void txtStep_Leave(object sender, EventArgs e)
        {
            float newStep;
            if (float.TryParse(txtStep.Text.Trim(), out newStep) && newStep > 0 )
            {
                txtStep.Text = newStep.ToString("F2");
                errorProvider1.SetError(txtStep, "");
            }
            else
            {
                errorProvider1.SetError(txtStep, "ערך צעד לא חוקי");
            }
        }
        private void OnHitEnterOnControl(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Control c = sender as Control;

                if (c.Name == "txtKiloPrice")
                    txtKiloPrice_Leave(sender, null);
                else if (c.Name == "txtProfitPercentage")
                    txtProfitPercentage_Leave(sender, null);
                else if (c.Name == "txtFinalPrice")
                    txtFinalPrice_Leave(sender, null);
                else if (c.Name == "txtWeight")
                    txtWeight_Leave(sender, null);
                else if (c.Name == "txtDiscountQty")
                    txtDiscountQty_Leave(sender, null);
                else if (c.Name == "txtDiscountPrice")
                    txtDiscountPrice_Leave(sender, null);
                else if (c.Name == "txtMaxDiscount")
                    txtMaxDiscount_Leave(sender, null);
                else if (c.Name == "txtMinQty")
                    txtMinQty_Leave(sender, null);
                else if (c.Name == "txtStep")
                    txtStep_Leave(sender, null);
                else if (c.Name == "txtMaxQty")
                    txtMaxQty_Leave(sender, null);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void OnIsPackageCheckChanged(object sender, EventArgs e)
        {
            this.btnEditPackage.Enabled = this.cbIsPackage.Checked;
        }
        private void OnEditPackageButtonClicked(object sender, EventArgs e)
        {
            string hebrew = ProductDataRow.IsNull("H_INFO") ? "n/a" : ProductDataRow.Field<string>("H_INFO");
            string arabic = ProductDataRow.IsNull("A_INFO") ? "n/a" : ProductDataRow.Field<string>("A_INFO");
            string russian = ProductDataRow.IsNull("RU_INFO") ? "n/a" : ProductDataRow.Field<string>("RU_INFO");
            ManagePackageInfo MPI = new ManagePackageInfo(this.ProductDataRow, arabic, hebrew, russian);
            MPI.ShowDialog(this);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // package name
            decimal tmpValue;
            float tmpValue_float;
            int tmpValue_int;
            bool error = false;
            bool removeFromDiscounts = false;
            if (string.IsNullOrWhiteSpace(txtArName.Text))
            {
                BiggerMessage.Show("שם פריט בערבית לא חוקי");
                error = true;
            }
            else if (string.IsNullOrWhiteSpace(txtHeName.Text))
            {
                BiggerMessage.Show("שם פריט בעברית לא חוקי");
                error = true;
            }
            else if (cbUnitName.SelectedIndex == -1)
            {
                BiggerMessage.Show("בחר בסוג יחידת הפריט");
                error = true;
            }
            else if (cbUnitName.SelectedIndex != 0 && cbIsWeighable.Checked && (!decimal.TryParse(txtWeight.Text, out tmpValue) || tmpValue <= 0))
            { 
                BiggerMessage.Show("ערך משקל לא חוקי");
                error = true;
            }
            else if (!decimal.TryParse(txtKiloPrice.Text, out tmpValue) || tmpValue <= 0)
            {
                BiggerMessage.Show(lblKiloPrice.Text + " לא חוקי");
                error = true;
            }
            else if (!decimal.TryParse(txtProfitPercentage.Text, out tmpValue))
            {
                BiggerMessage.Show("אחוז רווח לא חוקי");
                error = true;
            }
            else if (!decimal.TryParse(txtFinalPrice.Text, out tmpValue) || tmpValue <= 0)
            {
                BiggerMessage.Show("ערך מחיר סופי לא חוקי");
                error = true;
            }
            else if (!float.TryParse(txtMinQty.Text, out tmpValue_float) || tmpValue_float <= 0)
            {
                BiggerMessage.Show("ערך כמות מינימום לא חוקי");
                error = true;
            }
            else if (!float.TryParse(txtStep.Text, out tmpValue_float) || tmpValue_float <= 0)
            {
                BiggerMessage.Show("ערך צעד לא חוקי");
                error = true;
            }
            else if (!string.IsNullOrWhiteSpace(txtMaxQty.Text) && (!float.TryParse(txtMaxQty.Text, out tmpValue_float) || tmpValue_float <= 0))
            {
                BiggerMessage.Show("ערך כמות מקסימום לא חוקי");
                error = true;
            }
            else if (cbHasDiscount.Checked && (string.IsNullOrWhiteSpace(txtDiscountPrice.Text) || !decimal.TryParse(txtDiscountPrice.Text, out tmpValue)))
            {
                BiggerMessage.Show("ערך מחיר הנחה לא חוקי");
                error = true;
            }
            else if (cbHasDiscount.Checked && (string.IsNullOrWhiteSpace(txtDiscountQty.Text) || !int.TryParse(txtDiscountQty.Text, out tmpValue_int)))
            {
                BiggerMessage.Show("ערך כמות בהנחה לא חוקי");
                error = true;
            }
            else if (cbHasDiscount.Checked && !string.IsNullOrWhiteSpace(txtMaxDiscount.Text) && (!int.TryParse(txtMaxDiscount.Text, out tmpValue_int) || tmpValue_int < 0))
            { 
                BiggerMessage.Show("ערך כפל הנחה לא חוקי");
                error = true;
            }
            else if (ImageFileName != "N/A" && !System.IO.File.Exists(ImageFileName))
            {
                BiggerMessage.Show("קובץ תמונה לא קיים");
                error = true;
            }
            else if (ImageFileName != "N/A" && System.IO.Path.GetExtension(ImageFileName) != ".jpg")
            {
                BiggerMessage.Show("תמונה לא תקינה" + "\r\n" + ".jpg בלבד" + "\r\n" + "ratio 1:1" + "\r\n" + "recommended 450x450");
                error = true;
            }
            else if (ImageFileName != "N/A" && new System.IO.FileInfo(ImageFileName).Length > 500000)
            {
                BiggerMessage.Show("גודל תמונה גדול מ-150mb ");
                error = true;
            }
            
            if(!error)
            {
                this.Enabled = false;
                LFRM = new LoadingForm();
                LFRM.Show();
                this.ProductControlSource.ResetProductControls();

                string updateString = "UPDATE products SET ";

                // product name 
                updateString += string.Format("A_NAME='{0}', H_NAME='{1}', ", MySqlHelper.EscapeString(txtArName.Text.Trim()), MySqlHelper.EscapeString(txtHeName.Text.Trim()));

                // arabic notes
                string arabicNotes = txtArNotes.Text.Replace("\"", "\\\"").Replace("'", "\'");
                if (arabicNotes.Length == 0 || string.IsNullOrEmpty(arabicNotes) || string.IsNullOrWhiteSpace(arabicNotes))
                    arabicNotes = "null";
                else
                    arabicNotes = "\"" + arabicNotes + "\"";

                // hebrew notes
                string hebrewNotes = txtHeNotes.Text.Replace("\"", "\\\"").Replace("'", "\'");
                if (hebrewNotes.Length == 0 || string.IsNullOrEmpty(hebrewNotes) || string.IsNullOrWhiteSpace(hebrewNotes))
                    hebrewNotes = "null";
                else
                    hebrewNotes = "\"" + hebrewNotes + "\"";

                string finalNoteString = string.Format("{{\"arabic\":{0}, \"hebrew\": {1}}}", arabicNotes, hebrewNotes);
                updateString += string.Format("NOTE='{0}', ", MySqlHelper.EscapeString(finalNoteString));

                // unit id
                updateString += string.Format("UNIT_ID={0}, ", this.UnitIDs[cbUnitName.SelectedIndex]);

                // is weighable & weight
                updateString += string.Format("WEIGH_ABLE={0}, UNIT_VALUE={1}, ", cbIsWeighable.Checked ? 1 : 0, GetProductWeight());

                // prices
                updateString += string.Format("PRICE={0}, CUSTOM_ADD={1}, FINAL_PRICE={2}, ", decimal.Parse(txtKiloPrice.Text), decimal.Parse(txtProfitPercentage.Text), decimal.Parse(txtFinalPrice.Text));

                // quantities in order
                updateString += string.Format("MIN_QTY={0}, STEP={1}, MAX_QTY={2}, ", float.Parse(txtMinQty.Text), float.Parse(txtStep.Text), string.IsNullOrWhiteSpace(txtMaxQty.Text) ? -1 : float.Parse(txtMaxQty.Text));

                // discount
                if (cbHasDiscount.Checked)
                {
                    string arabicDiscountName = txtArDiscountName.Text.Trim().Replace("\"", "\\\"").Replace("'", "\'");
                    string hebrewDiscountName = txtHeDiscountName.Text.Trim().Replace("\"", "\\\"").Replace("'", "\'");
                    string disc_finalString = MySqlHelper.EscapeString(string.Format("{{\"arabic\": \"{0}\", \"hebrew\":\"{1}\"}}", arabicDiscountName, hebrewDiscountName));
                    updateString += string.Format("DISCOUNT_QUANTITY={0}, DISCOUNT_PRICE={1}, DISCOUNT_MAX_REPEAT={2}, DISCOUNT_NAME='{3}', ", int.Parse(txtDiscountQty.Text), decimal.Parse(txtDiscountPrice.Text), string.IsNullOrWhiteSpace(txtMaxDiscount.Text) ? 0 : int.Parse(txtMaxDiscount.Text), disc_finalString);
                }
                else
                {
                    updateString += "DISCOUNT_QUANTITY=-1, DISCOUNT_PRICE=0.00, DISCOUNT_MAX_REPEAT=0, DISCOUNT_NAME='{\"arabic\":null, \"hebrew\": null}', ";
                }

                // price update status
                if (cbHasDiscount.Checked != !(this.ProductDataRow.Field<string>("DISCOUNT_QUANTITY") == "-1"))
                    updateString += "PRICE_UPDATED = 2, ";
                else if (decimal.Parse(txtFinalPrice.Text) != decimal.Parse(this.ProductDataRow.Field<string>("FINAL_PRICE")))
                    updateString += "PRICE_UPDATED = CASE WHEN PRICE_UPDATED < 2 THEN 1 ELSE PRICE_UPDATED END, ";

                // product is active
                updateString += "REMOVED=" + (this.cbIsActive.Checked ? 0 : 1) + " WHERE ID=" + this.ProductID;

                this.savingBackground = new BackgroundWorker();
                this.savingBackground.DoWork += (s, ev) =>
                {
                    new API_CLASS().API_QUERY_EXECUTE(updateString);

                    // product package info
                    string iArabic, iHebrew, iRussian;
                    if (this.cbIsPackage.Checked)
                    {
                        iArabic = ProductDataRow.IsNull("A_INFO") ? "n/a" : ProductDataRow.Field<string>("A_INFO")
                        .Replace("&", "%26")
                        .Replace("+", "%2B");

                        iHebrew = ProductDataRow.IsNull("H_INFO") ? "n/a" : ProductDataRow.Field<string>("H_INFO")
                        .Replace("&", "%26")
                        .Replace("+", "%2B");

                        iRussian = ProductDataRow.IsNull("RU_INFO") ? "n/a" : ProductDataRow.Field<string>("RU_INFO")
                        .Replace("&", "%26")
                        .Replace("+", "%2B");

                        if (iArabic == "n/a" && iHebrew == "n/a" && iRussian == "n/a")
                        {
                            new API_CLASS().API_QUERY_EXECUTE("DELETE FROM products_info WHERE PRODUCT_ID=" + this.ProductID);
                        }
                        else
                        {
                            new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO products_info VALUES ({0}, '{1}', '{2}', '{3}') ON DUPLICATE KEY UPDATE A_INFO='{4}', H_INFO='{5}', RU_INFO='{6}'", this.ProductID, iArabic, iHebrew, iRussian, iArabic, iHebrew, iRussian));
                        }
                    }
                    else
                        new API_CLASS().API_QUERY_EXECUTE("DELETE FROM products_info WHERE PRODUCT_ID=" + this.ProductID);

                    if (decimal.Parse(this.txtFinalPrice.Text) != decimal.Parse(this.ProductDataRow.Field<string>("FINAL_PRICE")))
                    {
                        int total_affected = new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE price_history SET NEW_PRICE={0}, TIME=curtime() WHERE PRODUCT_ID={1} AND DATE=curdate()", decimal.Parse(txtFinalPrice.Text), this.ProductID));
                        if (total_affected == 0) new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO price_history VALUES ({0}, {1}, curdate(), curtime())", this.ProductID, decimal.Parse(txtFinalPrice.Text)));
                    }

                    // product image
                    int imgVersion = int.Parse(this.ProductDataRow.Field<string>("IMG_VERSION"));
                    if (this.ImageFileName != "N/A")
                    {
                        imgVersion++;
                        API_CLASS api = new API_CLASS();
                        api.API_QUERY_EXECUTE(string.Format("UPDATE products SET IMG_VERSION={0} WHERE ID={1}", imgVersion, this.ProductID));
                        api.API_UPLOAD_PRODUCT_IMAGE(ImageFileName, "/" + this.ProductID.ToString() + ".jpg");
                    }

                    if (cbHasDiscount.Checked)
                    {
                        // adding product to discounts category
                        // category ID 17 is the discounts category
                        if (new API_CLASS().API_QUERY(string.Format("SELECT PRODUCT_ID FROM categories_products WHERE PRODUCT_ID={0} AND CATEGORY_ID={1}", this.ProductID, 17)).Rows.Count == 0)
                        {
                            DataTable dt2 = new API_CLASS().API_QUERY("SELECT * FROM categories_products WHERE CATEGORY_ID=17 ORDER BY PRODUCT_INDEX DESC LIMIT 1");
                            int theIndex = -1;
                            if (dt2.Rows.Count == 0) theIndex = 1;
                            else
                                theIndex = int.Parse(dt2.Rows[0].Field<string>("PRODUCT_INDEX"));

                            new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO categories_products VALUES (17, {0}, {1})", this.ProductID, theIndex+1));
                            new API_CLASS().WP_updateProductCategories(this.ProductID);
                        }

                        int pID = int.Parse(this.ProductDataRow.Field<string>("WP_INDEX"));
                        new API_CLASS().WP_updateProductPrice(pID, decimal.Parse(txtFinalPrice.Text), int.Parse(txtDiscountQty.Text) == 1 ? decimal.Parse(txtDiscountPrice.Text) : 0);

                    }
                    else
                    {
                        // removing it from discounts category
                        int pID = int.Parse(this.ProductDataRow.Field<string>("WP_INDEX"));
                        new API_CLASS().WP_updateProductPrice(pID, decimal.Parse(txtFinalPrice.Text), 0);
                        
                        DataTable prdctInDiscountData = new API_CLASS().API_QUERY(string.Format("SELECT PRODUCT_ID, PRODUCT_INDEX FROM categories_products WHERE PRODUCT_ID={0} AND CATEGORY_ID={1}", this.ProductID, 17));
                        if(prdctInDiscountData.Rows.Count > 0)
                        {
                            int productid = int.Parse(prdctInDiscountData.Rows[0].Field<string>("PRODUCT_ID"));
                            int productindex = int.Parse(prdctInDiscountData.Rows[0].Field<string>("PRODUCT_INDEX"));
                            new API_CLASS().API_QUERY_EXECUTE("DELETE FROM categories_products WHERE PRODUCT_ID=" + productid + " AND CATEGORY_ID=17");
                            new API_CLASS().API_QUERY_EXECUTE("UPDATE categories_products SET PRODUCT_INDEX=PRODUCT_INDEX-1 WHERE PRODUCT_INDEX>" + productindex + " AND CATEGORY_ID=17");

                            if((this.parentForm as NewProductsForm).currentCategory == 17)
                                removeFromDiscounts = true;

                            new API_CLASS().WP_updateProductCategories(productid);
                        }
                    }
                    int wp_index = int.Parse(this.ProductDataRow.Field<string>("WP_INDEX"));
                    new API_CLASS().WP_updateProductInfo(ProductID, wp_index, txtHeName.Text, imgVersion, cbIsActive.Checked ? 1 : 0);
                };
                this.savingBackground.RunWorkerCompleted += (s, ev) =>
                {
                    this.Close();

                    if (!removeFromDiscounts)
                        this.ProductControlSource.StartProductControl(this.ProductID, null);
                    else
                        (this.parentForm as NewProductsForm).RemoveProductFromDeck(this.ProductID, 17);

                    LFRM.Close();
                };
                this.savingBackground.RunWorkerAsync();
            }
        }
        private void RefreshDiscountControls()
        {
            int discountQuantity = int.Parse(ProductDataRow.Field<string>("DISCOUNT_QUANTITY"));
            if(discountQuantity == -1)
            {
                this.cbHasDiscount.Checked = false;
                DisableDiscountControls();
            }
            else
            {
                this.cbHasDiscount.Checked = true;
                EnableDiscountControls();
            }
        }

        private void DisableDiscountControls()
        {

            this.txtDiscountQty.Text = "";
            this.txtDiscountQty.Enabled = false;
            errorProvider1.SetError(this.txtDiscountQty, "");

            this.txtDiscountPrice.Text = "";
            this.txtDiscountPrice.Enabled = false;
            errorProvider1.SetError(this.txtDiscountPrice, "");

            this.txtMaxDiscount.Text = "";
            this.txtMaxDiscount.Enabled = false;
            errorProvider1.SetError(this.txtMaxDiscount, "");

            this.txtArDiscountName.Text = "";
            this.txtArDiscountName.Enabled = false;
            errorProvider1.SetError(this.txtArDiscountName, "");

            this.txtHeDiscountName.Text = "";
            this.txtHeDiscountName.Enabled = false;
            errorProvider1.SetError(this.txtHeDiscountName, "");

            this.txtRuDiscountName.Text = "";
            this.txtRuDiscountName.Enabled = false;
            errorProvider1.SetError(this.txtRuDiscountName, "");
        }

        private void EnableDiscountControls()
        {
            DataTable discountName = (DataTable)JsonConvert.DeserializeObject("[" + ProductDataRow.Field<string>("DISCOUNT_NAME") + "]", (typeof(DataTable)));

            this.txtDiscountQty.Text = int.Parse(ProductDataRow.Field<string>("DISCOUNT_QUANTITY")) == -1 ? "" : int.Parse(ProductDataRow.Field<string>("DISCOUNT_QUANTITY")).ToString();
            this.txtDiscountQty.Enabled = true;
            errorProvider1.SetError(this.txtDiscountQty, "");

            this.txtDiscountPrice.Text = decimal.Parse(ProductDataRow.Field<string>("DISCOUNT_PRICE")) == 0 ? "" : ProductDataRow.Field<string>("DISCOUNT_PRICE");
            this.txtDiscountPrice.Enabled = true;
            errorProvider1.SetError(this.txtDiscountPrice, "");

            this.txtMaxDiscount.Text = int.Parse(ProductDataRow.Field<string>("DISCOUNT_MAX_REPEAT")) == 0 ? "" : int.Parse(ProductDataRow.Field<string>("DISCOUNT_MAX_REPEAT")).ToString();
            this.txtMaxDiscount.Enabled = true;
            errorProvider1.SetError(this.txtMaxDiscount, "");

            this.txtArDiscountName.Text = discountName.Rows[0].Field<string>("arabic");
            this.txtArDiscountName.Enabled = true;
            errorProvider1.SetError(this.txtArDiscountName, "");

            this.txtHeDiscountName.Text = discountName.Rows[0].Field<string>("hebrew");
            this.txtHeDiscountName.Enabled = true;
            errorProvider1.SetError(this.txtHeDiscountName, "");

            this.txtRuDiscountName.Text = "";
            this.txtRuDiscountName.Enabled = true;
            errorProvider1.SetError(this.txtRuDiscountName, "");
        }

        private void txtDiscountQty_Leave(object sender, EventArgs e)
        {
            int discountQuantity;
            if(int.TryParse(txtDiscountQty.Text.Trim(), out discountQuantity))
            {
                if (discountQuantity == -1 || discountQuantity >= 1)
                {
                    if (discountQuantity == -1)
                        txtDiscountQty.Text = "";
                    else
                        txtDiscountQty.Text = discountQuantity.ToString();
                    errorProvider1.SetError(txtDiscountQty, "");
                }
                else
                {
                    errorProvider1.SetError(txtDiscountQty, "ערך לא חוקי");
                }
            }
            else
            {
                errorProvider1.SetError(txtDiscountQty, "ערך לא חוקי");
            }
        }

        private void cbHasDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbHasDiscount.Checked)
                EnableDiscountControls();
            else
                DisableDiscountControls();
        }

        private void txtDiscountPrice_Leave(object sender, EventArgs e)
        {
            decimal discountPrice;
            if (decimal.TryParse(txtDiscountPrice.Text.Trim(), out discountPrice))
            {
                if (discountPrice >= 0)
                {
                    if (discountPrice == 0)
                        txtDiscountPrice.Text = "";
                    else
                        txtDiscountPrice.Text = discountPrice.ToString();
                    errorProvider1.SetError(txtDiscountPrice, "");
                }
                else
                {
                    errorProvider1.SetError(txtDiscountPrice, "ערך לא חוקי");
                }
            }
            else
            {
                errorProvider1.SetError(txtDiscountPrice, "ערך לא חוקי");
            }
        }

        private void txtMaxDiscount_Leave(object sender, EventArgs e)
        {
            int maxdiscount;
            if (string.IsNullOrWhiteSpace(txtMaxDiscount.Text))
            {
                txtMaxDiscount.Text = "";
                errorProvider1.SetError(txtMaxDiscount, "");
            }

            else if (int.TryParse(txtMaxDiscount.Text.Trim(), out maxdiscount))
            {
                if (maxdiscount >= 0 || maxdiscount == -1)
                {
                    if (maxdiscount <= 0)
                        txtMaxDiscount.Text = "";
                    else
                        txtMaxDiscount.Text = maxdiscount.ToString();
                    errorProvider1.SetError(txtMaxDiscount, "");
                }
                else
                {
                    errorProvider1.SetError(txtMaxDiscount, "ערך לא חוקי");
                }
            }
            else
            {
                errorProvider1.SetError(txtMaxDiscount, "ערך לא חוקי");
            }
        }

        private void picProductImage_MouseEnter(object sender, EventArgs e)
        {
            this.picProductImage.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picProductImage_MouseLeave(object sender, EventArgs e)
        {
            this.picProductImage.BorderStyle = BorderStyle.None;
        }

        private void picProductImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.jpg)|*.jpg";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.Path.GetExtension(fileDialog.FileName) != ".jpg")
                    BiggerMessage.Show("תמונה לא תקינה" + "\r\n" + ".jpg בלבד" + "\r\n" + "ratio 1:1" + "\r\n" + "recommended 450x450");
                else if (new System.IO.FileInfo(fileDialog.FileName).Length > 500000)
                    BiggerMessage.Show("גודל תמונה גדול מ-150mb ");
                else
                {
                    Image bitmap = Image.FromFile(fileDialog.FileName);
                    this.ImageFileName = fileDialog.FileName;
                    picProductImage.Image = bitmap;
                }
            }
        }

        private void lblEditUnits_Click(object sender, EventArgs e)
        {
            ManageUnits MUNTS = new ManageUnits(this);
        }

        private void picProductImage_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                picProductImage.Image = Properties.Resources.unknown;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult DRSLT = MessageBox.Show("האם אתה רוצה למחק את הפריט לגמרי מהאתר?", "מחיקת פריט?", MessageBoxButtons.YesNo);
            if (DRSLT == DialogResult.Yes)
            {
                BackgroundWorker bw = new BackgroundWorker();
                LoadingForm LFRM = new LoadingForm();
                LFRM.Show();
                bw.DoWork += (s, ev) =>
                {
                    new API_CLASS().API_QUERY_EXECUTE("UPDATE products SET REMOVED=2 WHERE ID=" + this.ProductID);
                    new API_CLASS().WP_updateProductInfo(this.ProductID, int.Parse(this.ProductDataRow.Field<string>("WP_INDEX")), txtHeName.Text, int.Parse(this.ProductDataRow.Field<string>("IMG_VERSION")), 0);
                };
                bw.RunWorkerCompleted += (s, ev) =>
                {
                    LFRM.Close();
                    this.ProductControlSource.Enabled = false;
                    this.ProductControlSource.picNotActive.Visible = true;
                    (this.parentForm as NewProductsForm).RemoveProduct(this.ProductControlSource, this.ProductID, this.CategoryID);
                    
                    // in case it has a discount, then remove it from discounts page and match indexes.
                    if(cbHasDiscount.Enabled)
                    {
                        (this.parentForm as NewProductsForm).RemoveProduct(null, this.ProductID, 17, false);
                    }

                    this.Close();
                };
                bw.RunWorkerAsync();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (this.parentForm as NewProductsForm).RemoveProduct(this.ProductControlSource, this.ProductID, this.CategoryID, false);
            this.Close();
        }
    }
}
