using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaretnaPanel
{
    public static partial class BiggerMessage
    {
        public static void Show(string content, int type = 0, string title = "הערה")
        {
            BiggerMessageBox big = new BiggerMessageBox(content, type, title);
            big.TopMost = true;
            big.ShowDialog();
        }
    }
}
