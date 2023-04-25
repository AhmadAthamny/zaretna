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
    public partial class names : Form
    {
        public names()
        {
            InitializeComponent();
            // for sms
            DataTable dt = new API_CLASS().API_QUERY ("SELECT PHONE_NUMBER, F_NAME, L_NAME, ID FROM orders WHERE STATUS=2 AND BUY_TIME>'2021-04-10 23:00:00' AND PHONE_NUMBER NOT IN (SELECT PHONE_NUMBER FROM orders WHERE ID<=3084 GROUP BY PHONE_NUMBER)" +
               "GROUP BY PHONE_NUMBER ORDER BY F_NAME");

            // from 1st January 2021 till now.
            //DataTable dt = new API_CLASS().API_QUERY("SELECT A.ID, A.PHONE_NUMBER, A.TOTAL_PRICE, B.DELIVERY_PRICE, A.BUY_TIME FROM orders A LEFT JOIN regions B ON A.CTTY=B.ID WHERE A.BUY_TIME>='2020-01-01 00:00:00' AND A.STATUS=2");
            //DataTable dt = new API_CLASS().API_QUERY("SELECT A.ID, CONCAT(A.F_NAME, ' ', A.L_NAME) AS NAME, A.PHONE_NUMBER, A.TOTAL_PRICE, B.DELIVERY_PRICE, B.H_NAME, A.BUY_TIME FROM orders A INNER JOIN regions B ON A.CITY=B.ID WHERE A.BUY_TIME>='2021-01-01 00:00:00' AND A.STATUS=2 ORDER BY A.BUY_TIME ASC");

            dataGridView1.DataSource = dt;
        }   
    }
}
