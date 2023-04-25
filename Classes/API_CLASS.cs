using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Windows.Forms;
using FluentFTP;
using System.IO;

namespace ZaretnaPanel
{
    class API_CLASS
    {
        public bool BETA_TESTING = false;
        string API_KEY = "vkcJ5YbyYMUDFwnSU0gZDZDqy3SmViMS";
        //string API_BETA_KEY = "FJd6CH4hfEmU6TmXyXY5mBrq78Nsm8UR";
        string API_BETA_KEY = "GqgtKoJYTkcA5CmNFY4RifmZQJmxcoQDfZDZ";
        string WP_API_KEY = "pCGjLjGUcJc8qxSJWpvjX99VB5iC1n6E";
        string WP_API_URL = "https://zaretna.co.il/api/wp/";

        private static string myDoucmentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public string zaretnaProductsFolder = myDoucmentsPath + @"\zaretna_products";

        FtpClient FTPclient = new FtpClient();
        public DataTable API_QUERY(string Command)
        {
            string API_URL;
            if (!BETA_TESTING) API_URL = "https://zaretna.co.il/api/api.php?key=" + API_KEY + "&query=";
            else API_URL = "https://beta1122.zaretna.co.il/api/api.php?key=" + API_BETA_KEY + "&query=";
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            if (Command.Contains("DROP")) return null;

            string json;
            try
            {
                json = client.DownloadString(API_URL + Command);
            }
            catch
            {
                BiggerMessage.Show("تأكد من الإتصال بالإنترنت");
                return new DataTable();
            }

            try
            {
                return (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
            }
            catch (Exception ex)
            {
                BiggerMessage.Show(ex.Message + "\n\n" +Command);
                return new DataTable();
            }
        }
        public int API_QUERY_EXECUTE(string Command)
        {
            string API_URL;
            if (!BETA_TESTING) API_URL = "https://zaretna.co.il/api/api.php?key=" + API_KEY + "&query=";
            else API_URL = "https://beta1122.zaretna.co.il/api/api.php?key=" + API_KEY + "&query=";
            WebClient client = new WebClient();
            string downloadString = "";
            try
            {
                downloadString = client.DownloadString(API_URL + Command);
            }
            catch
            {
                return -1;
            }

            int intResult;
            if(int.TryParse(downloadString, out intResult))
            {
                return intResult;
            }
            else
            {
                return -1;
            }
        }
        public void FTP_CONNECT(bool productImages = true)
        {
            this.FTPclient = new FtpClient();
            this.FTPclient.Host = "162.241.224.203";
            this.FTPclient.Port = 21;
            if (!BETA_TESTING)
            {
                if(productImages)
                    this.FTPclient.Credentials = new NetworkCredential("images@zaretna.co.il", "XTNGBM{yqXPM"); // product images
                else
                    this.FTPclient.Credentials = new NetworkCredential("popups@zaretna.co.il", "Z6mSxv*VwtG)"); // pop-ups images
            }
            else
            {
                if(productImages)
                    this.FTPclient.Credentials = new NetworkCredential("images@beta1122.zaretna.co.il", "er(bqyO@1y*,"); // product images
                else
                    this.FTPclient.Credentials = new NetworkCredential("popups@beta1122.zaretna.co.il", "@Tk,.3P]NRqD"); // pop-ups
            }
            this.FTPclient.Connect();
        }
        public void FTP_DISCONNECT()
        {
            this.FTPclient.Disconnect();
        }
        public void API_UPLOAD_PRODUCT_IMAGE(string FileLocation, string NameToSet)
        {
            FTP_CONNECT(true); 
            this.FTPclient.UploadFile(FileLocation, NameToSet);
            FTP_DISCONNECT();
        }
        public void API_UPLOAD_POP_UP(string FileLocation, string NameToSet)
        {
            FTP_CONNECT(false);
            this.FTPclient.UploadFile(FileLocation, NameToSet);
            FTP_DISCONNECT();
        }
        public string ReturnImageURL()
        {
            if (this.BETA_TESTING)
                return "https://beta1122.zaretna.co.il/product_images/";
            else
                return "https://zaretna.co.il/product_images/";
        }
        public int WP_addCategory(string name, string img_url)
        {
            string URL = WP_API_URL + "category/add.php?key=" + WP_API_KEY + "&name=" + name + "&img=" + img_url;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            int ID = -1;
            try
            {
                string content = client.DownloadString(URL);
                int.TryParse(content, out ID);
            }
            catch
            {
                ID = -2;
            }
            return ID;
        }
        public int WP_editCategory(int ID, string name)
        {
            string URL = WP_API_URL + "category/edit.php?key=" + WP_API_KEY + "&id=" + ID + "&name=" + name;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            try
            {
                string content = client.DownloadString(URL);
            }
            catch
            {
                BiggerMessage.Show("בדוק החיבור לרשת");
            }
            return ID;
        }
        public int WP_addProduct(string name, int category)
        {
            string URL = WP_API_URL + "product/add.php?key=" + WP_API_KEY + "&name=" + name + "&category=" + category;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            int ID = -1;
            try
            {
                string content = client.DownloadString(URL);
                int.TryParse(content, out ID);
            }
            catch
            {
                ID = -2;
            }
            return ID;
        }
        public void WP_updateProductPrice(int ID, decimal price, decimal salePrice = -1)
        {
            string URL = WP_API_URL + "product/update_price.php?key=" + WP_API_KEY + "&id=" + ID + "&price=" + price + "&discount=" + salePrice;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            try
            {
                string content = client.DownloadString(URL);
            }
            catch
            {
                BiggerMessage.Show("בדוק החיבור לרשת");
            }
        }
        public void WP_updateProductCategories(int ID)
        {
            string URL = WP_API_URL + "product/set_category.php?key=" + WP_API_KEY + "&id=" + ID;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            try
            {
                string content = client.DownloadString(URL);
            }
            catch
            {
                BiggerMessage.Show("בדוק החיבור לרשת");
            }
        }
        public void WP_updateProductInfo(int ID, int index, string name, int version, int active = 1)
        {
            string URL = WP_API_URL + "product/update_info.php?key=" + WP_API_KEY + "&id=" + ID + "&index=" + index + "&name=" + name + "&version=" + version + "&active=" + active;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            try
            {
                string content = client.DownloadString(URL);
            }
            catch
            {
                BiggerMessage.Show("בדוק החיבור לרשת");
            }
        }
    }
}
