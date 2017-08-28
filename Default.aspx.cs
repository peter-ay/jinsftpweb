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
        public static String html = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " HKO Jins FTP System Runing..." + "<br />";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDataBind();
        }

        private void GetDataBind()
        {
            if (!string.IsNullOrEmpty(html))
            {
                Response.Write(html);
            }
        }
    }
}