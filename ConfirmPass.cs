using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ZaretnaPanel
{
    public partial class ConfirmPass : Form
    {
        Form1 Source;
        public ConfirmPass(Form1 tmp_Source)
        {

           //names NMS = new names();
           //NMS.ShowDialog();

            InitializeComponent();
            Source = tmp_Source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtPassword.Text == "zar12341")
            {
                this.Close();
                Source.didPassword = true;
            }
            else
            {
                BiggerMessage.Show("סיסמה לא נכונה");
            }
        }

        private void ConfirmPass_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!Source.didPassword)
                System.Environment.Exit(0);
        }
    }
}
