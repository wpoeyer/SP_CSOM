using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace ManageAlerts
{
    class Program
    {
        static void Main(string[] args)
        {
            string webUrl = "https://boskalis.sharepoint.com/sites/eagle/community/";
            string userName = "wpoeb@boskalis.com";
            Console.WriteLine("Please input your password: ");
            SecureString password = CreateSecureStringPasswordFromConsoleInput();
            using (var context = new ClientContext(webUrl))
            {
                
                context.Credentials = new SharePointOnlineCredentials(userName, password);
                context.Load(context.Web, w => w.Title);
                context.ExecuteQuery();

                GetAlerts(context);
            }
        }

        private static void GetAlerts(ClientContext context)
        {
            Web web = context.Web;
            
            context.Load(web);
            context.Load(web.Alerts);
            context.ExecuteQuery();

            Console.WriteLine("Get alerts from web: " + web.Title);

            AlertCollection alerts = web.Alerts;

            foreach (Alert alert in alerts)
            {
                web.Alerts.DeleteAlert(alert.ID);
            }
            web.Update();
            Console.WriteLine("Update Alerts..");
            context.ExecuteQuery();

            Console.ReadKey(true);
        }

        private static SecureString CreateSecureStringPasswordFromConsoleInput()
        {
            ConsoleKeyInfo info;

            //Get the user's password as a SecureString
            SecureString securePassword = new SecureString();
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    securePassword.AppendChar(info.KeyChar);
                }
            }
            while (info.Key != ConsoleKey.Enter);
            return securePassword;
        }
    }
}
