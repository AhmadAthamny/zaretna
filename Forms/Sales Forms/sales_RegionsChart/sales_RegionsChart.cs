using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ZaretnaPanel
{
    public partial class sales_RegionsChart : Form
    {
        public sales_RegionsChart(int month, int year)
        {
            InitializeComponent();
            BuildChartAndTable(month, year);
            FillChartData();
        }
        public void BuildChartAndTable(int month, int year)
        {
            Series salesSeries = new Series();
            salesSeries.ChartType = SeriesChartType.Column;

            theChart.Legends[0].Enabled = false;
            theChart.ChartAreas[0].AxisX.Interval = 1;

            // visible
            resultsTable.Columns.Add("region", "איזור");
            resultsTable.Columns.Add("total", "מכירות");

            resultsTable.Columns[0].FillWeight = 2;
            resultsTable.Columns[1].FillWeight = 1;

            this.fromMonth.SelectedIndex = month-1;
            this.toMonth.SelectedIndex = month - 1;

            for (int i = 2020; i <= DateTime.Now.Year; i++)
            {
                this.fromYear.Items.Add(i);
                this.toYear.Items.Add(i);
            }
            this.fromYear.SelectedIndex = year - 2020;
            this.toYear.SelectedIndex = year - 2020;

        }
        public void FillChartData()
        {
            int szFromMonth = this.fromMonth.SelectedIndex + 1;
            int szToMonth = this.toMonth.SelectedIndex + 1;

            int szFromYear = this.fromYear.SelectedIndex + 2020;
            int szToYear = this.toYear.SelectedIndex + 2020;

            DateTime fromDate = new DateTime(szFromYear, szFromMonth, 1);
            DateTime toDate = new DateTime(szToYear, szToMonth, DateTime.DaysInMonth(szToYear, szToMonth));

            if ( fromDate > toDate)
            {
                BiggerMessage.Show("תקופה לא תקינה, תאריך בצד ימין צריך להיות קטן מצד שמולה");
                return;
            }

            theChart.Width = 0;
            this.MaximumSize = new Size(0, 0);
            string query = string.Format("SELECT REG.H_NAME AS NAME, COUNT(ORD.ID) AS TOTAL FROM orders ORD LEFT JOIN regions REG " +
                "ON ORD.CITY = REG.ID " +
                "WHERE ORD.STATUS!=4 AND ORD.TOTAL_PRICE!=-1 " +                
                "AND ORD.BUY_TIME>='{0}' AND ORD.BUY_TIME<='{1}' " +
                "GROUP BY ORD.CITY", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd 23:59:59"));

            theChart.Series.Clear();
            resultsTable.Rows.Clear();

            Series regions = new Series("regions");
            regions["PixelPointWidth"] = "6";
            theChart.Series.Add(regions);

            DataTable dt = new API_CLASS().API_QUERY(query);
            foreach(DataRow row in dt.Rows)
            {
                regions.Points.AddXY(row.Field<string>("NAME"), row.Field<string>("TOTAL"));
                regions.Points[regions.Points.Count - 1].Label = row.Field<string>("TOTAL");
                regions.Points[regions.Points.Count - 1].LabelBackColor = Color.LightGray;
                theChart.Width += 61;

                resultsTable.Rows.Add(row.Field<string>("NAME"), row.Field<string>("TOTAL"));
            }
            if (theChart.Width < 560) theChart.Width = 560;
            resultsTable.Location = new Point(theChart.Location.X + theChart.Width + 4, resultsTable.Location.Y);
            this.Width = resultsTable.Location.X + resultsTable.Width + 20;
            this.MaximumSize = new Size(this.Width >= this.MinimumSize.Width ? this.Width : this.MinimumSize.Width, 800);

            this.lblFrom.Location = new Point(this.resultsTable.Location.X + this.resultsTable.Width - 239, this.resultsTable.Location.Y - this.lblFrom.Height - 3);
            this.lblTitle.Location = new Point(this.lblFrom.Location.X + this.lblFrom.Width - this.lblTitle.Width, this.lblFrom.Location.Y - this.lblTitle.Height - 3);
            this.groupBox1.Location = new Point(this.theChart.Location.X + this.theChart.Width - this.groupBox1.Width, this.groupBox1.Location.Y);

            this.lblFrom.Text = string.Format("{0}/{1}", szToMonth, szToYear) + " - " + string.Format("{0}/{1}", szFromMonth, szFromYear);

            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillChartData();
        }
    }
}
