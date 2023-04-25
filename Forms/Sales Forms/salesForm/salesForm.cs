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
    public partial class salesForm : Form
    {
        public salesForm()
        {
            InitializeComponent();
            BuildStructure();
        }

        private void BuildStructure()
        {
            this.fromMonth.SelectedIndex = 0;
            this.toMonth.SelectedIndex = DateTime.Now.Month - 1;

            for(int i = 2020; i <= DateTime.Now.Year; i++)
            {
                this.fromYear.Items.Add(i);
                this.toYear.Items.Add(i);
            }
            this.fromYear.SelectedIndex = 0;
            this.toYear.SelectedIndex = this.toYear.Items.Count-1;

            // hidden
            resultsTable.Columns.Add("_month", "");
            resultsTable.Columns.Add("_year", "");

            resultsTable.Columns[0].Visible = false;
            resultsTable.Columns[1].Visible = false;

            // visible
            resultsTable.Columns.Add("date", "חודש");
            resultsTable.Columns.Add("total", "מכירות");

            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.Name = "regions";
            btnCol.Text = "איזורים";
            btnCol.HeaderText = "איזורים";
            btnCol.UseColumnTextForButtonValue = true;

            resultsTable.Columns.Add(btnCol);

            theChart.ChartAreas[0].AxisX.Interval = 2;
            theChart.Legends[0].Enabled = false;
        }

        private void LoadResults()
        {
            int szFromMonth = this.fromMonth.SelectedIndex+1;
            int szToMonth = this.toMonth.SelectedIndex + 1;

            int szFromYear = this.fromYear.SelectedIndex + 2020;
            int szToYear = this.toYear.SelectedIndex + 2020;

            DateTime fromDate = new DateTime(szFromYear, szFromMonth, 1);
            DateTime toDate = new DateTime(szToYear, szToMonth, DateTime.DaysInMonth(szToYear, szToMonth));

            if (fromDate > toDate)
            {
                BiggerMessage.Show("תקופה לא תקינה, תאריך בצד ימין צריך להיות קטן מצד שמולה");
                return;
            }

            int totalmonths = (szToYear - szFromYear) * 12 + szToMonth - szFromMonth;
            if (totalmonths >= 11)
                theChart.ChartAreas[0].AxisX.Interval = 2;
            else
                theChart.ChartAreas[0].AxisX.Interval = 1;


            resultsTable.Rows.Clear();
            DataTable dt = new API_CLASS().API_QUERY(string.Format("SELECT COUNT(*) AS TOTAL, MONTH(BUY_TIME) AS MONTH, YEAR(BUY_TIME) AS YEAR FROM orders" +
                " WHERE STATUS!=4 AND TOTAL_PRICE!=-1" +
                " AND BUY_TIME >= '{0}' AND BUY_TIME <= '{1}'" +
                " GROUP BY YEAR(BUY_TIME), MONTH(BUY_TIME)",
                fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate.ToString("yyyy-MM-dd HH:mm:ss") )
                );

            Series salesSeries = new Series("מכירות");
            salesSeries.ChartType = SeriesChartType.Line;
            salesSeries.MarkerStyle = MarkerStyle.Circle;
            salesSeries.MarkerColor = Color.Red;
            salesSeries.MarkerBorderWidth = 6;
            salesSeries.BorderWidth = 6;

            theChart.Series.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string db_month = row.Field<string>("MONTH"), db_year = row.Field<string>("YEAR");
                string month = db_month + "/" + db_year[2]+db_year[3];

                int totalorders = int.Parse(row.Field<string>("TOTAL"));
                resultsTable.Rows.Add(db_month, db_year, month, totalorders, new Button());

                salesSeries.Points.AddXY(month, totalorders);
                salesSeries.Points[salesSeries.Points.Count - 1].Label = row.Field<string>("TOTAL");
                salesSeries.Points[salesSeries.Points.Count - 1].LabelBackColor = Color.LightGray;
            }
            theChart.Series.Add(salesSeries);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadResults();
        }

        private void resultsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == resultsTable.Columns["regions"].Index)
                {
                    int month = int.Parse(resultsTable.Rows[e.RowIndex].Cells[0].Value.ToString());
                    int year = int.Parse(resultsTable.Rows[e.RowIndex].Cells[1].Value.ToString());
                    sales_RegionsChart SRC = new sales_RegionsChart(month, year);
                    SRC.ShowDialog(this);
                }
            }
        }
    }
}
