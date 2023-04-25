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
    public partial class ManageUnits : Form
    {
        private int y = 0;
        private AddNewProductForm sourceForm;
        public ManageUnits(AddNewProductForm srcForm)
        {
            InitializeComponent();
            this.sourceForm = srcForm;

            LoadingForm LFRM = new LoadingForm();
            LFRM.Show();

            BackgroundWorker BWRKER = new BackgroundWorker();
            BWRKER.DoWork += (s, e) =>
            {
                DataTable dt = new API_CLASS().API_QUERY("SELECT A.*, C.TOTAL FROM unit_names A LEFT JOIN (SELECT UNIT_ID, COUNT(ID) AS TOTAL FROM products GROUP BY UNIT_ID) C ON A.ID = C.UNIT_ID");

                foreach (DataRow row in dt.Rows)
                {
                    UnitControl ctrl = new UnitControl(int.Parse(row.Field<string>("ID")), row.Field<string>("A_NAME"), row.Field<string>("H_NAME"), row.Field<string>("E_NAME"), row.IsNull("TOTAL") ? "0" : row.Field<string>("TOTAL"));
                    ctrl.Location = new Point(0, y);
                    y += 26;
                    this.unitsContainer.Controls.Add(ctrl);
                }
                this.unitsContainer.Height = y-22 + this.okButton.Height;
                this.okButton.Location = new Point(this.okButton.Location.X, this.unitsContainer.Height);
                this.btnAdd.Location = new Point(this.btnAdd.Location.X, this.unitsContainer.Height);
                this.Height = this.okButton.Location.Y + this.okButton.Height+41;
                this.MaximumSize = new Size(this.Width, this.Height);
            };
            BWRKER.RunWorkerCompleted += (s, e) =>
            {
                LFRM.Close();
                this.ShowDialog();
            };
            BWRKER.RunWorkerAsync();
        }

        private void ManageUnits_Load(object sender, EventArgs e)
        {
        }



        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UnitControl ctrl = new UnitControl(-1, "arabic", "hebrew", "russian", "0");
            ctrl.Location = new Point(0, y);
            y += 26;
            this.unitsContainer.Controls.Add(ctrl);

            this.unitsContainer.Height = y - 22 + this.okButton.Height;
            this.okButton.Location = new Point(this.okButton.Location.X, this.unitsContainer.Height);
            this.btnAdd.Location = new Point(this.btnAdd.Location.X, this.unitsContainer.Height);
            this.MaximumSize = new Size(this.Width, this.okButton.Location.Y + this.okButton.Height + 41);
            this.Size = new Size(this.Width, this.MaximumSize.Height);
        }

        private void ManageUnits_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.sourceForm.FillUnitsComboBox(int.Parse(this.sourceForm.ProductDataRow.Field<string>("UNIT_ID")));
        }
    }
}
