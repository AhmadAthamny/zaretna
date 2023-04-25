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
    public partial class DatePickerForm : Form
    {
        private NewOrdersForm SourceForm;
        private int[] citiesIndexes;
        public DatePickerForm(NewOrdersForm srcFormTmp)
        {
            InitializeComponent();
            this.SourceForm = srcFormTmp;
            this.fromDate.Value = this.SourceForm.FromDatePickerValue;
            this.toDate.Value = this.SourceForm.ToDatePickerValue;
            this.txtFirstName.Text = this.SourceForm.FirstName;
            this.txtLastName.Text = this.SourceForm.LastName;
            this.txtPhoneNumber.Text = this.SourceForm.PhoneNumber;
            this.txtEmailAddress.Text = this.SourceForm.EmailAddress;
            this.nmbrMinPrice.Value = this.SourceForm.MinimumPrice;
            this.nmbrMaxPrice.Value = this.SourceForm.MaxPrice;
            this.cbDeliveriedOnly.Checked = this.SourceForm.DeliveredOnly ? true : false;

            FillCitiesCMBOX(this.SourceForm.CityID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SourceForm.FromDatePickerValue = fromDate.Value.Date;
            this.SourceForm.ToDatePickerValue = toDate.Value;
            this.SourceForm.FirstName = this.txtFirstName.Text;
            this.SourceForm.LastName = this.txtLastName.Text;
            this.SourceForm.PhoneNumber = this.txtPhoneNumber.Text;
            this.SourceForm.EmailAddress = this.txtEmailAddress.Text;
            this.SourceForm.MinimumPrice = (int)this.nmbrMinPrice.Value;
            this.SourceForm.MaxPrice = (int)this.nmbrMaxPrice.Value;
            this.SourceForm.DeliveredOnly = this.cbDeliveriedOnly.Checked ? true : false;
            this.SourceForm.CityID = this.comboCities.SelectedIndex == -1 ? -1 : this.citiesIndexes[this.comboCities.SelectedIndex];

            this.SourceForm.AdvancedSearch();
            this.Close();
        }

        public void FillCitiesCMBOX(int selectedcityid)
        {
            DataTable dt = new API_CLASS().API_QUERY("SELECT ID, H_NAME FROM regions");
            this.citiesIndexes = new int[dt.Rows.Count+1];

            this.citiesIndexes[0] = -1;
            this.comboCities.Items.Add("כל איזורים");

            int cityindexfound = -1;
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if (selectedcityid == int.Parse(dt.Rows[i].Field<string>(0)))
                    cityindexfound = i+1;

                this.citiesIndexes[i+1] = int.Parse(dt.Rows[i].Field<string>(0));
                this.comboCities.Items.Add(dt.Rows[i].Field<string>(1));
            }
            this.comboCities.SelectedIndex = cityindexfound == -1 ? 0 : cityindexfound;
        }
    }
}
