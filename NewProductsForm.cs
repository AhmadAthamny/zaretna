using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.IO;


namespace ZaretnaPanel
{
    public partial class NewProductsForm : Form
    {
        private int lastAddedRow = 0;
        private int lastAddedColumn = 4;
        private BackgroundWorker loadingProductsBW = new BackgroundWorker();
        private BackgroundWorker removeProductWorker = new BackgroundWorker();
        private TableLayoutPanel productsTable = new TableLayoutPanel();
        private int lastXCord = 1300;
        private ProductControl HoverProductControl = null;
        public int currentCategory = -1;
        private ProductControl movingProduct = null;
        private int currentVersion = -1;
        private int currentCategory_WPIndex = -1;

        private int removingProductID = -1;
        private int removingProductCategory = -1;
        private bool removingPermanent = false;
        private ProductControl removingProductCard = null;
        private LoadingForm removingLoadingForm = null;
        private bool removingIsUpToDate = false;
        public NewProductsForm()
        {
            InitializeComponent();

            removeProductWorker.DoWork += new DoWorkEventHandler(removeProductWorker_DoWork);
            removeProductWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(removeProductWorker_RunWorkerCompleted);

            // loading categories
            BackgroundWorker bw = new BackgroundWorker();
            DataTable dt = new DataTable();
            LoadingForm LFRM = new LoadingForm();
            LFRM.Show();
            bw.DoWork += (sender, args) =>
            {
                dt = new API_CLASS().API_QUERY("SELECT ID, H_NAME FROM categories ORDER BY ID DESC");
            };
            bw.RunWorkerCompleted += (sender, args) =>
            {
                string h_name;
                int id;
                int categoryToView = -1;
                foreach (DataRow row in dt.Rows)
                {
                    id = int.Parse(row.Field<string>(0));
                    h_name = row.Field<string>(1);

                    AddCategoryButton(id, h_name);
                    categoryToView = id;
                }
                LFRM.Close();
                UpdateProductsTable();
                LoadCategoryProducts(categoryToView);
                buildControl_AddCategory();
            };
            bw.RunWorkerAsync();

            BackgroundWorker filesWorker = new BackgroundWorker();
            filesWorker.DoWork += (s, e) =>
            {
                if(!Directory.Exists(new API_CLASS().zaretnaProductsFolder))
                {
                    Directory.CreateDirectory(new API_CLASS().zaretnaProductsFolder);
                }
           
            };
            filesWorker.RunWorkerAsync();
        }

        private void NewProductsForm_Load(object sender, EventArgs e)
        {
            this.categoryPanel.Select();
        }
        private void AddProductToDeck(int productID, DataRow row = null)
        {
            Control PCTRL;
            // if product ID equals to -1, then it's the control to add a new product.
            if (this.productsTable.Controls.Count > 0 && this.productsTable.Controls[this.productsTable.Controls.Count - 1].Name == "addNewProductControl")
                this.productsTable.Controls.RemoveAt(this.productsTable.Controls.Count - 1);
            else
            {
                lastAddedColumn--;
                if (lastAddedColumn < 0)
                {
                    lastAddedColumn = 3;
                    lastAddedRow++;
                    if (lastAddedRow > this.productsTable.RowCount - 1)
                    {
                        this.productsTable.RowStyles.Insert(lastAddedRow, new RowStyle(SizeType.Absolute, 145));
                        this.productsTable.RowCount++;
                    }
                }
            }
            if (productID == -1)
            {
                PCTRL = new addNewProductControl();
                PCTRL.Cursor = Cursors.Hand;
                (PCTRL as addNewProductControl).pictureBox1.Click += new EventHandler(PCTRL_Click);          
            }
            else
            {
                PCTRL = new ProductControl(productID, row);
                PCTRL.MouseEnter += (s, e) => OnMouseEnterProduct(s, e);
                foreach (Control c in PCTRL.Controls)
                {
                    if (c.Name == "productPriceTB")
                        c.Enter += (s, e) => OnMouseEnterProduct(s, e);
                    c.MouseEnter += (s, e) => OnMouseEnterProduct(s, e);
                }
            }

            this.productsTable.Controls.Add(PCTRL, lastAddedColumn, lastAddedRow);
        }

        public void RemoveProductFromDeck(int productID, int categoryID)
        {
            if(this.currentCategory == categoryID)
            {
                foreach (Control ctrl in this.productsTable.Controls)
                {
                    if (ctrl is ProductControl)
                    {
                        if ((ctrl as ProductControl).ProductID == productID)
                        {
                            if (this.lastAddedColumn == 3)
                            {
                                this.lastAddedColumn = 0;
                                this.lastAddedRow--;
                            }
                            else
                                this.lastAddedColumn++;
                            int pColumn, pRow, tmpRow, tmpClmn;
                            pColumn = this.productsTable.GetColumn(ctrl);
                            pRow = this.productsTable.GetRow(ctrl);
                            this.productsTable.Controls.Remove(ctrl);
                            foreach (Control repControl in this.productsTable.Controls)
                            {
                                if (repControl is ProductControl || repControl is addNewProductControl)
                                {
                                    tmpRow = this.productsTable.GetRow(repControl);
                                    tmpClmn = this.productsTable.GetColumn(repControl);
                                    if (tmpRow > pRow || (tmpRow == pRow && tmpClmn < pColumn))
                                    {
                                        this.productsTable.SetRow(repControl, tmpClmn == 3 ? tmpRow - 1 : tmpRow);
                                        this.productsTable.SetColumn(repControl, tmpClmn == 3 ? 0 : tmpClmn + 1);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        private void PCTRL_Click(object sender, EventArgs e)
        {
            addNewProductControl PCTRL = (sender as Control).Parent as addNewProductControl;
            PCTRL.Cursor = Cursors.Default;
            PCTRL.transparentPic.Visible = true;
            PCTRL.existingProduct.Visible = true;
            PCTRL.newProduct.Visible = true;

            PCTRL.newProduct.Click += (s,ev) => NewProduct_Click(s, ev);
        }

        private void NewProduct_Click(object sender, EventArgs e)
        {
            (sender as Control).Enabled = false;
            int product = -1;
            BackgroundWorker bw = new BackgroundWorker();
            LoadingForm LFRM = new LoadingForm();
            LFRM.Show();
            Control ctrl = productsTable.Controls[productsTable.Controls.Count - 1];
            int index = 4 - productsTable.GetColumn(ctrl) + productsTable.GetRow(ctrl) * 4;
            int wp_index = -1;
            bw.DoWork += (s, ev) =>
            {
                if (isCategoryUpToDate(this.currentCategory))
                {
                    wp_index = new API_CLASS().WP_addProduct("מוצר חדש", this.currentCategory_WPIndex);
                    if (wp_index > 0)
                    {
                        string theJSON = "{\"arabic\":null, \"hebrew\": null}";
                        string cmd = "INSERT INTO products (A_NAME, H_NAME, NEW, FORNOW, NOTE, DISCOUNT_NAME, REMOVED, WP_INDEX) VALUES ('منتج جديد', 'מוצר חדש', 0, 0, '" + theJSON + "', '" + theJSON + "', 1, "+ wp_index +")";
                        product = new API_CLASS().API_QUERY_EXECUTE(cmd);
                        string cmd2 = "INSERT INTO categories_products (CATEGORY_ID, PRODUCT_ID, PRODUCT_INDEX) VALUES (" + this.currentCategory.ToString() + ", " + product.ToString() + ", " + index + ")";
                        new API_CLASS().API_QUERY_EXECUTE(cmd2);
                        new API_CLASS().WP_updateProductCategories(product);

                        UpdateCategoryVersion(this.currentCategory);
                    }
                    else
                    {
                        BiggerMessage.Show("בדוק את החיבור לרשת");
                    }
                }
                else
                {
                    index = -2;
                    BiggerMessage.Show("יש לרענן את הדף ואחר כך להוסיף מוצר חדש");
                }
            };
            bw.RunWorkerCompleted += (s, ev) =>
            {
                if (index != -2 && wp_index > 0)
                {
                    AddProductToDeck(product);
                    AddProductToDeck(-1);
                }
                LFRM.Close();
            };
            bw.RunWorkerAsync();
        }

        private void productsTable_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void NewProductsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.categoryPanel.Select();
                ToggleMovingButtons(null);
            }
        }
        private void LoadCategoryProducts(int categoryid)
        {
            if (this == null)
                return;

            loadingProductsBW.WorkerSupportsCancellation = true;

            if (loadingProductsBW.IsBusy)
                loadingProductsBW.CancelAsync();

            if (!loadingProductsBW.IsBusy)
            {
                DataTable dt = new DataTable();

                loadingProductsBW = new BackgroundWorker();
                LoadingForm LFRM = new LoadingForm();
                LFRM.Show(this);

                loadingProductsBW.DoWork += (sender, args) =>
                {
                    if (loadingProductsBW.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                    //dt = new API_CLASS().API_QUERY("SELECT A.ID, A.A_NAME, A.H_NAME, A.FINAL_PRICE, A.IMG_VERSION, B.H_NAME AS UNIT_NAME, A.DISCOUNT_QUANTITY, A.REMOVED, A.PRICE, A.CUSTOM_ADD, A.UNIT_VALUE FROM products A LEFT JOIN unit_names B ON A.UNIT_ID = B.ID INNER JOIN categories_products C ON A.ID = C.PRODUCT_ID AND C.CATEGORY_ID=" + categoryid + " WHERE A.REMOVED<2 ORDER BY C.PRODUCT_INDEX ASC");

                    DataRow cRow = new API_CLASS().API_QUERY("SELECT LAST_PRODUCT_ORDER, 2ND_INDEX FROM categories WHERE ID=" + categoryid).Rows[0];
                    this.currentVersion = int.Parse(cRow.Field<string>("LAST_PRODUCT_ORDER"));
                    this.currentCategory_WPIndex = int.Parse(cRow.Field<string>("2ND_INDEX"));

                    dt = new API_CLASS().API_QUERY(string.Format("SELECT A.ID, A.A_NAME, A.H_NAME, A.FINAL_PRICE, A.IMG_VERSION, B.H_NAME AS UNIT_NAME, A.DISCOUNT_QUANTITY, A.REMOVED, A.PRICE, A.CUSTOM_ADD, A.UNIT_VALUE, C.DATE AS PRICE_DATE, C.TIME AS PRICE_TIME, NOW() AS DB_DATETIME, A.WP_INDEX" +
                        " FROM products A" +
                        " LEFT JOIN unit_names B ON A.UNIT_ID = B.ID" +
                        " LEFT JOIN (SELECT E.* FROM price_history E INNER JOIN (SELECT PRODUCT_ID, MAX(DATE) AS TOP_DATE FROM price_history GROUP BY PRODUCT_ID) F ON E.PRODUCT_ID = F.PRODUCT_ID AND E.DATE = F.TOP_DATE) C ON C.PRODUCT_ID = A.ID" +
                        " INNER JOIN categories_products D ON A.ID = D.PRODUCT_ID AND D.CATEGORY_ID = {0}" +
                        " WHERE A.REMOVED<2 ORDER BY D.PRODUCT_INDEX ASC", categoryid));
                };

                loadingProductsBW.RunWorkerCompleted += (sender, args) =>
                {
                    if (!loadingProductsBW.CancellationPending && !args.Cancelled)
                    {
                        this.productsTable.RowStyles.Insert(0, new RowStyle(SizeType.Absolute, 145));
                        foreach (DataRow row in dt.Rows)
                        {
                            AddProductToDeck(int.Parse(row.Field<string>(0)), row);
                        }
                        AddProductToDeck(-1);
                        this.currentCategory = categoryid;
                        this.movingProduct = null;
                    }
                    LFRM.Close();
                };
                loadingProductsBW.RunWorkerAsync();
            }
        }
        private void UpdateProductsTable()
        {
            foreach (Control ctrl in productsTable.Controls)
            {
                if (ctrl is ProductControl)
                {
                    ProductControl PCTRL = ctrl as ProductControl;
                    PCTRL.ImageLoader.WorkerSupportsCancellation = true;
                    PCTRL.ImageLoader.CancelAsync();
                }
            }
            lastAddedColumn = 4;
            lastAddedRow = 0;
            this.Controls.Remove(productsTable);
            this.productsTable.Controls.Clear();
            this.productsTable.RowStyles.Clear();
            this.productsTable = new TableLayoutPanel();
            this.productsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.productsTable.AutoScroll = true;
            this.productsTable.BackColor = System.Drawing.Color.Gainsboro;
            this.productsTable.ColumnCount = 4;
            this.productsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.productsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.productsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.productsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.productsTable.Location = new System.Drawing.Point(1, categoryPanel.Height+3);
            this.productsTable.Margin = new System.Windows.Forms.Padding(0);
            this.productsTable.Name = "productsTable";
            this.productsTable.RowCount = 1;
            this.productsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 801F));
            this.productsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 801F));
            this.productsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 801F));
            this.productsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 801F));
            this.productsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 801F));
            this.productsTable.Size = new System.Drawing.Size(1355, this.Height-productsTable.Location.Y-40);
            this.productsTable.TabIndex = 0;
            this.Controls.Add(productsTable);
            this.HoverProductControl = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateProductsTable();
            LoadCategoryProducts(5);
        }
        private void OnClickCategoryButton(object sender, EventArgs e)
        {
            UpdateProductsTable();
            Button tmp = sender as Button;
            LoadCategoryProducts(int.Parse(tmp.Name));
        }
        private void buildControl_AddCategory()
        {
            PictureBox btnAddCategory = new PictureBox();
            btnAddCategory.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAddCategory.Image = global::ZaretnaPanel.Properties.Resources.add_category_new;
            btnAddCategory.Location = new System.Drawing.Point(3, 4);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new System.Drawing.Size(25, 25);
            btnAddCategory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            btnAddCategory.TabIndex = 0;
            btnAddCategory.TabStop = false;
            btnAddCategory.Tag = "";
            btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            this.categoryPanel.Controls.Add(btnAddCategory);
            this.toolTip1.SetToolTip(btnAddCategory, "הוספת קטגוריה");
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            addNewCategoryForm ANCF = new addNewCategoryForm(this);
        }
        public void AddCategoryButton(int categoryid, string hebrew_name)
        {
            Button btn = new Button();
            btn.Width = 139;
            btn.Height = 29;
            btn.Name = categoryid.ToString();
            lastXCord -= 139;
            btn.Location = new Point(lastXCord, 3);
            btn.Text = hebrew_name;
            btn.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
            btn.Dock = DockStyle.Right;
            btn.AutoSize = true;
            btn.Tag = "button";
            btn.ContextMenuStrip = cmsEditCategory;
            btn.Click += new EventHandler(OnClickCategoryButton);
            categoryPanel.Controls.Add(btn);
        }
        public void LoadAllCategories()
        {
            this.lastXCord = 1300;
            this.categoryPanel.Controls.Clear();

            DataTable dt = new API_CLASS().API_QUERY("SELECT * FROM categories ORDER BY ID DESC");
            int id;
            string h_name;
            foreach (DataRow row in dt.Rows)
            {
                id = int.Parse(row.Field<string>(0));
                h_name = row.Field<string>(3);

                AddCategoryButton(id, h_name);
            }
            buildControl_AddCategory();
        }
        public void OnMouseEnterProduct(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            ProductControl pctrl;
            if (ctrl.Parent.Name == "ProductControl")
            {
                pctrl = ctrl.Parent as ProductControl;
            }
            else
                pctrl = ctrl as ProductControl;

            if (HoverProductControl != null && pctrl != HoverProductControl)
            {
                HoverProductControl.BorderStyle = BorderStyle.None;
            }
            this.HoverProductControl = pctrl;
            pctrl.BorderStyle = BorderStyle.FixedSingle;
        }

        private void cmsEditCategory_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int categoryid = int.Parse(cmsEditCategory.SourceControl.Name);
            addNewCategoryForm ANCF = new addNewCategoryForm(this, categoryid);
        }

        private void NewProductsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.loadingProductsBW.WorkerSupportsCancellation = true;
            this.loadingProductsBW.CancelAsync();
        }
        public void ToggleMovingButtons(ProductControl PCTRL)
        {
            this.movingProduct = PCTRL;
            for (int i = productsTable.Controls.Count - 1; i >= 0; i--)
            {
                if (productsTable.Controls[i] is ProductControl)
                {
                    ProductControl pctrl = productsTable.Controls[i] as ProductControl;
                    if (pctrl == this.movingProduct) continue;

                    pctrl.btnMoveProduct.Visible = this.movingProduct == null ? true : false;
                    pctrl.btnArrowMove.Visible = this.movingProduct == null ? false : true;
                }
            }

        }
        public void MoveProductCards(ProductControl PCTRL)
        {
            LoadingForm frm = new LoadingForm();

            frm.Show();
            BackgroundWorker bww = new BackgroundWorker();
            bool goodToGo = false;
            bww.DoWork += (s, e) =>
            {
                goodToGo = isCategoryUpToDate(this.currentCategory);
            };
            bww.RunWorkerCompleted += (s, e) =>
            {
                if (!goodToGo)
                {
                    BiggerMessage.Show("רענן את הדף");
                    ToggleMovingButtons(null);
                    frm.Close();
                    return;
                }
                int newrow;
                int newcolumn;

                int curRow = productsTable.GetRow(movingProduct);
                int curColumn = productsTable.GetColumn(movingProduct);

                int destRow = productsTable.GetRow(PCTRL);
                int destColumn = productsTable.GetColumn(PCTRL);

                // moving all products to the left side by one step, to make room for the new product.
                if (curRow > destRow || (curRow == destRow && curColumn < destColumn))
                {
                    string query = "UPDATE categories_products SET PRODUCT_INDEX=PRODUCT_INDEX%2B1 WHERE CATEGORY_ID=" + this.currentCategory + " AND (PRODUCT_ID=";
                    bool first = true;

                    for (int clm = curColumn + 1, row = curRow; row > destRow || (row == destRow && clm < destColumn); clm++)
                    {
                        if (clm == 4)
                        {
                            clm = 0;
                            row--;
                        }
                        if (row <= destRow && clm >= destColumn)
                            break;

                        Control ctrl;
                        ProductControl thePCTRL = null;
                        if (productsTable.GetControlFromPosition(clm, row) is ProductControl || productsTable.GetControlFromPosition(clm, row) is addNewProductControl)
                        {
                            ctrl = productsTable.GetControlFromPosition(clm, row) as Control;
                            if (productsTable.GetControlFromPosition(clm, row) is ProductControl)
                                thePCTRL = productsTable.GetControlFromPosition(clm, row) as ProductControl;
                        }
                        else
                            continue;

                        if (clm == 0)
                        {
                            newcolumn = 3;
                            newrow = row + 1;
                        }
                        else
                        {
                            newcolumn = clm - 1;
                            newrow = row;
                        }
                        productsTable.SetColumn(ctrl, newcolumn);
                        productsTable.SetRow(ctrl, newrow);

                        if (thePCTRL != null)
                        {
                            if (first)
                            {
                                query += (thePCTRL).ProductID;
                                first = false;
                            }
                            else query += " OR PRODUCT_ID=" + thePCTRL.ProductID;
                        }
                    }

                    BackgroundWorker bw = new BackgroundWorker();
                    // moving the product to the empty room.
                    if (productsTable.GetColumn(PCTRL) == 0)
                    {
                        productsTable.SetColumn(this.movingProduct, 3);
                        productsTable.SetRow(this.movingProduct, productsTable.GetRow(PCTRL) + 1);
                    }
                    else
                    {
                        productsTable.SetColumn(this.movingProduct, productsTable.GetColumn(PCTRL) - 1);
                        productsTable.SetRow(this.movingProduct, productsTable.GetRow(PCTRL));
                    }

                    bw.DoWork += (se, ev) =>
                    {
                        new API_CLASS().API_QUERY_EXECUTE(query + ")");
                        int index = 4 - productsTable.GetColumn(this.movingProduct) + productsTable.GetRow(this.movingProduct) * 4;
                        new API_CLASS().API_QUERY_EXECUTE("UPDATE categories_products SET PRODUCT_INDEX=" + index + " WHERE PRODUCT_ID=" + this.movingProduct.ProductID + " AND CATEGORY_ID=" + this.currentCategory);
                    };
                    bw.RunWorkerCompleted += (se, ev) =>
                    {
                        frm.Close();
                        ToggleMovingButtons(null);
                    };
                    bw.RunWorkerAsync();
                }
                else if (curRow < destRow || (curRow == destRow && curColumn > destColumn))
                {
                    productsTable.Controls.Remove(movingProduct);
                    string query = "UPDATE categories_products SET PRODUCT_INDEX=PRODUCT_INDEX-1 WHERE CATEGORY_ID=" + this.currentCategory + " AND (PRODUCT_ID=";
                    bool first = true;
                    for (int clm = curColumn - 1, row = curRow; row < destRow || (row == destRow && clm >= destColumn); clm--)
                    {
                        if (clm == -1)
                        {
                            clm = 3;
                            row++;
                        }
                        if (row > destRow || (row == destRow && clm < destColumn))
                            break;

                        Control ctrl;
                        ProductControl thePCTRL = null;
                        if (productsTable.GetControlFromPosition(clm, row) is ProductControl || productsTable.GetControlFromPosition(clm, row) is addNewProductControl)
                        {
                            ctrl = productsTable.GetControlFromPosition(clm, row) as Control;
                            if (ctrl is ProductControl)
                                thePCTRL = ctrl as ProductControl;
                        }
                        else
                            continue;

                        if (clm == 3)
                        {
                            newcolumn = 0;
                            newrow = row - 1;
                        }
                        else
                        {
                            newcolumn = clm + 1;
                            newrow = row;
                        }
                        productsTable.SetColumn(ctrl, newcolumn);
                        productsTable.SetRow(ctrl, newrow);
                        if (thePCTRL != null)
                        {
                            if (first)
                            {
                                query += (thePCTRL).ProductID;
                                first = false;
                            }
                            else query += " OR PRODUCT_ID=" + thePCTRL.ProductID;
                        }
                    }
                    // moving the product to the empty room.
                    productsTable.Controls.Add(movingProduct, destColumn, destRow);

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (se, ev) =>
                    {
                        int index = 4 - destColumn + destRow * 4;
                        new API_CLASS().API_QUERY_EXECUTE("UPDATE categories_products SET PRODUCT_INDEX=" + index + " WHERE PRODUCT_ID=" + this.movingProduct.ProductID + " AND CATEGORY_ID=" + this.currentCategory);
                        new API_CLASS().API_QUERY_EXECUTE(query + ")");

                        UpdateCategoryVersion(this.currentCategory);
                    };
                    bw.RunWorkerCompleted += (se, ev) =>
                    {
                        frm.Close();
                        ToggleMovingButtons(null);
                    };
                    bw.RunWorkerAsync();
                }
            };
            bww.RunWorkerAsync();            
        }
        public void saveallIndexes()
        {
            foreach(Control ctrl in productsTable.Controls)
            {
                if(ctrl is ProductControl)
                {
                    ProductControl pctrl = ctrl as ProductControl;
                    int index = productsTable.GetRow(ctrl) * 4 + (4 - productsTable.GetColumn(ctrl));
                    new API_CLASS().API_QUERY_EXECUTE("UPDATE categories_products SET PRODUCT_INDEX=" + index + " WHERE PRODUCT_ID=" + pctrl.ProductID + " AND CATEGORY_ID="+this.currentCategory);
                }
            }
        }
        public bool isCategoryUpToDate(int categoryid)
        {
            if (this.currentVersion == -1) return false;
            if (categoryid != this.currentCategory) return true;

            DataTable dt = new API_CLASS().API_QUERY("SELECT LAST_PRODUCT_ORDER FROM categories WHERE ID=" + this.currentCategory);
            if (int.Parse(dt.Rows[0].Field<string>(0)) > this.currentVersion)
                return false;
            else
                return true;
        }
        public void UpdateCategoryVersion(int categoryid)
        {
            new API_CLASS().API_QUERY_EXECUTE("UPDATE categories SET LAST_PRODUCT_ORDER=LAST_PRODUCT_ORDER%2B1 WHERE ID=" + categoryid);
            if(categoryid == this.currentCategory)
                this.currentVersion++;
        }
        public void RemoveProduct(Control card, int productid, int categoryid, bool permanent = true)
        {
            if (!removeProductWorker.IsBusy)
            {
                this.removingLoadingForm = new LoadingForm();
                this.removingLoadingForm.Show();
                this.removingPermanent = permanent;
                this.removingProductCard = card is ProductControl ? card as ProductControl : null;
                this.removingProductID = productid;
                this.removingProductCategory = categoryid;
                removeProductWorker.RunWorkerAsync();
            }
        }

        private void removeProductWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.removingIsUpToDate = isCategoryUpToDate(this.removingProductCategory);
            if (this.removingIsUpToDate)
            {
                DataTable dt = new API_CLASS().API_QUERY("SELECT CATEGORY_ID, PRODUCT_INDEX FROM categories_products WHERE PRODUCT_ID=" + this.removingProductID);
                if (this.removingPermanent)
                {
                    int index, catid;
                    foreach (DataRow row in dt.Rows)
                    {
                        index = int.Parse(row.Field<string>("PRODUCT_INDEX"));
                        catid = int.Parse(row.Field<string>("CATEGORY_ID"));
                        new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE categories_products SET PRODUCT_INDEX=PRODUCT_INDEX-1 WHERE CATEGORY_ID={0} AND PRODUCT_INDEX>{1}", removingProductCategory, index));
                    }
                    new API_CLASS().API_QUERY_EXECUTE("DELETE FROM categories_products WHERE PRODUCT_ID=" + this.removingProductID);
                }
                else
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        if(int.Parse(row.Field<string>("CATEGORY_ID")) == this.removingProductCategory)
                        {
                            new API_CLASS().API_QUERY_EXECUTE("DELETE FROM categories_products WHERE PRODUCT_ID=" + this.removingProductID + " AND CATEGORY_ID=" + this.removingProductCategory);
                            string theUpdateString = string.Format("UPDATE categories_products SET PRODUCT_INDEX=PRODUCT_INDEX-1 WHERE PRODUCT_INDEX>{0} AND CATEGORY_ID={1}", int.Parse(row.Field<string>("PRODUCT_INDEX")), removingProductCategory);
                            new API_CLASS().API_QUERY_EXECUTE(theUpdateString);
                            break;
                        }
                    }
                    if(dt.Rows.Count == 1)
                    {
                        new API_CLASS().API_QUERY_EXECUTE("UPDATE products SET REMOVED=2 WHERE ID=" + this.removingProductID);
                    }
                }
                new API_CLASS().WP_updateProductCategories(this.removingProductID);
                UpdateCategoryVersion(removingProductCategory);
            }
            else
            {
                BiggerMessage.Show("יש שגיאה.. רענן את הדף");
            }
        }

        private void removeProductWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.removingIsUpToDate)
            {
                this.removingLoadingForm.Close();
                RemoveProductFromDeck(this.removingProductID, this.removingProductCategory);
            }
        }
        public void AddProductToCategory(int productid)
        {
            BackgroundWorker bw = new BackgroundWorker();
            LoadingForm LFRM = new LoadingForm();
            LFRM.Show();
            int theError = 0;
            bw.DoWork += (s, ev) =>
            {
                if (isCategoryUpToDate(this.currentCategory))
                {
                    DataTable dt = new API_CLASS().API_QUERY("SELECT PRODUCT_ID FROM categories_products WHERE CATEGORY_ID=" + this.currentCategory + " AND PRODUCT_ID=" + productid);
                    if (dt.Rows.Count == 0)
                    {
                        Control latestControl = this.productsTable.Controls[this.productsTable.Controls.Count - 1];
                        int index = 4 - productsTable.GetColumn(latestControl) + productsTable.GetRow(latestControl) * 4;
                        new API_CLASS().API_QUERY_EXECUTE(string.Format("INSERT INTO categories_products VALUES ({0}, {1}, {2})", this.currentCategory, productid, index));
                        new API_CLASS().WP_updateProductCategories(productid);
                    }
                    else
                        theError = 1;
                }
                else
                {
                    theError = 2;
                }
            };
            bw.RunWorkerCompleted += (s, evv) =>
            {
                LFRM.Close();
                if(theError == 1) BiggerMessage.Show("הפריט קיים בקטגוריה, לא צריך להוסיף אותו עוד פעם");
                else if(theError == 2) BiggerMessage.Show("רענן את הדף");
                else
                {
                    AddProductToDeck(productid);
                    AddProductToDeck(-1);
                }
            };
            bw.RunWorkerAsync();
        }
    }
}