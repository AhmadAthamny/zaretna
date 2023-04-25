using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EASendMail;

namespace ZaretnaPanel
{
    class EmailSend
    {
        public EmailSend(string From, List<string> To, string Subject, string Content)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("Tryit");

                // Set sender email address, please change it to yours
                oMail.From = From+" <info@zaretna.co.il>";

                // Set recipient email address, please change it to yours
                for (int i = 0; i < To.Count; i++)
                    oMail.To.Add(To[i]);

                // Set email subject
                oMail.Subject = Subject;

                // Set email body
                oMail.HtmlBody = Content;

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("mail.zaretna.co.il");

                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                oServer.User = "info@zaretna.co.il";
                oServer.Password = "zaretnainfo";

                // Set 465 SMTP port
                oServer.Port = 465;

                // Enable SSL connection
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
    }
}
