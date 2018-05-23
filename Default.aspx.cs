using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jinsftpweb
{
    public partial class Default : System.Web.UI.Page
    {
        public static String html = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " HKO Jins FTP test180523 System Runing..." + "<br />";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetMsg();
        }

        private void GetMsg()
        {
            if (!string.IsNullOrEmpty(html))
            {
                Response.Write(html);
                if (html.Length >= 8000)
                {
                    html = string.Empty;
                    html = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " HKO Jins FTP System Runing..." + "<br />";
                }
            }
        }
    }
}