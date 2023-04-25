using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace ZaretnaPanel
{
    public partial class WriteEmails : Form
    {
        public WriteEmails()
        {
            InitializeComponent();
            DataGridViewCheckBoxColumn dgvCBColumn = new DataGridViewCheckBoxColumn(false);
            dgvCBColumn.HeaderText = " ";
            dgvCBColumn.Name = "checked";
            dgvTable.Columns.Add(dgvCBColumn);
            dgvTable.Columns.Add("email", "Email");
            dgvTable.Rows.Add(false, "a.m.datamny@gmail.com");
            dgvTable.Rows.Add(false, "lebnawy@yahoo.com");
            dgvTable.Rows.Add(false, "omardroby@gmail.com");
            dgvTable.Rows.Add(false, "mkh158914@gmail.com");

            dgvTable.Columns["checked"].FillWeight = 15;
            dgvTable.Columns["email"].FillWeight = 85;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string url = "https://zaretna.co.il/api/deliveryboy_mail.php";
            string emails = "";
            foreach(DataGridViewRow row in dgvTable.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    emails += emails.Length > 0 ? "," : "" + row.Cells[1].Value.ToString();
                }
            }

            Dictionary<string, string> FormData = new Dictionary<string, string>();
            FormData.Add("key", "Oy5i6SgCmY0DEhc89Zx5NXdI3H3zc6Wa");
            FormData.Add("emails", emails);

            try
            {
                string result = await PostHTTPRequestAsync(url, FormData);
                if ( result == "success" )
                {
                    BiggerMessage.Show("רשימה נשלחה בהצלחה", 1);
                }
                else
                {
                    BiggerMessage.Show("רשימה לא נשלחה, לבדוק");
                }
            }
            catch (Exception ex)
            {
                BiggerMessage.Show("רשימה לא נשלחה" + "\r\n" + ex.Message);
            }
            this.Close();


            /*List<string> EmailRec = new List<string>();
            foreach(DataGridViewRow row in dgvTable.Rows)
            {
                if (row.IsNewRow) continue;

                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    EmailRec.Add(row.Cells[1].Value.ToString());
            }

            if(EmailRec.Count == 0)
            {
                BiggerMessage.Show("לבחור לפחות דוא\"ל אחד!");
                return;
            }
            
            string Content = "<center><b><font size=\"6\">" + DateTime.Now + "</font></b><br><br></center>";
            Content += "<font size=\"3\">";
            Content += "<center><div style=\"border: 1px solid black; \"><table width=\"100%\" dir=\"rtl\">";
            Content += "<b>";
            Content += "<tr>";
            Content += "<th>#</th>";
            Content += "<th>שם</th>";
            Content += "<th>טלפון</th>";
            Content += "<th>סכום</th>";
            Content += "<th>שלם</th>";
            Content += "<th>עיר</th>";
            Content += "<th>כתובת</th>";
            Content += "<th>הערות</th>";
            Content += "<th>מיקום</th>";
            Content += "<th>נשלח?</th>";
            Content += "</tr></b>";

            int ID;
            string F_NAME, L_NAME, PHONE_NUMBER, CITY, ADDRESS, NOTE, LOCATION_LAT, LOCATION_LONG, LOCATION, TOTAL_PRICE, CODE, PAID;

            DataTable dt = new API_CLASS().API_QUERY("SELECT a.ID, a.F_NAME, a.L_NAME, a.PHONE_NUMBER, b.H_NAME, a.NOTE, a.HOUSE_NUM, a.ENTER_NUM, a.STREET_NUM, a.TOTAL_PRICE, a.LOCATION_LAT, a.LOCATION_LONG, a.PAID FROM orders a INNER JOIN regions b on a.CITY = b.ID WHERE a.TOTAL_PRICE!=-1 AND a.STATUS=0 ORDER BY a.CITY ASC");

            if(dt.Rows.Count == 0)
            {
                BiggerMessage.Show("אין הזמנות מוכנות");
                return;
            }
            foreach (DataRow row in dt.Rows)
            {
                ID = int.Parse(row.Field<string>(0));

                F_NAME = row.Field<string>(1);
                L_NAME = row.Field<string>(2);
                PHONE_NUMBER = row.Field<string>(3);
                CITY = row.Field<string>(4);
                NOTE = row.Field<string>(5) == "-1" ? "אין" : row.Field<string>(5);

                TOTAL_PRICE = row.Field<string>(9);

                LOCATION_LAT = row.Field<string>(10);
                LOCATION_LONG = row.Field<string>(11);

                ADDRESS = "<b>מספר בית:</b> " + row.Field<string>(6) + "<br>";
                ADDRESS += "<b>מספר רחוב:</b> " + row.Field<string>(8) + "<br>";
                ADDRESS += "<b>כניסה:</b> " + row.Field<string>(7);

                if (float.Parse(LOCATION_LAT) > 0 && float.Parse(LOCATION_LONG) > 0)
                    LOCATION = "<a href=\"https://waze.com/ul?ll=" + LOCATION_LAT + "," + LOCATION_LONG + "\">WAZE</a>";
                else LOCATION = "אין";

                CODE = RandomString(random.Next(6, 10));

                PAID = row.Field<string>(12) == "0" ? "<font color=\"red\"><b>"+"מוזמן"+"</b></font>" : "<font color=\"green\"><b>"+"אשראי"+"</b></font>";

                new API_CLASS().API_QUERY_EXECUTE(string.Format("UPDATE orders SET CODE='{0}' WHERE ID={1}", CODE, ID));

                CODE = "https://zaretna.co.il/background/delivery/delivery.php?ID=" + ID + "&CODE=" + CODE;

                Content += "<tr>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + ID.ToString() + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + F_NAME + " " + L_NAME + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\"><a href=\"tel:" + PHONE_NUMBER + "\">"+PHONE_NUMBER+"</a></td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\"><font color=\"red\"><b>" + TOTAL_PRICE + "</b></font></td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + PAID + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + CITY + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + ADDRESS + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + NOTE + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\">" + LOCATION + "</td>";
                Content += "<td style=\"border: 1px solid #999999; padding: 8px;\"><a href=\"" + CODE + "\">עדכון שנשלחה</a></td>";
                Content += "</tr>";
            }

            Content += "</table></div></center></font>";

            EmailSend email = new EmailSend("רשימת משלוחים", EmailRec, "משלוח", Content);
            */
        }
        
        /*private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }*/

        private void WriteEmails_Load(object sender, EventArgs e)
        {
            dgvTable.DoubleBuffered(true);
        }
        private async Task<string> PostHTTPRequestAsync(string url, Dictionary<string, string> data)
        {
            HttpClient client = new HttpClient();
            using (HttpContent formContent = new FormUrlEncodedContent(data))
            {
                using (HttpResponseMessage response = await client.PostAsync(url, formContent).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
