using DbHelperSQLLib;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;

namespace Jinsftpweb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //this.RunTheTask();
            //定时器
            var t1 = this.CreateTimer(10000);
            t1.Elapsed += t1_Elapsed;
            var t2 = this.CreateTimer(20000);
            t2.Elapsed += t2_Elapsed;
            var t3 = this.CreateTimer(30000);
            t3.Elapsed += t3_Elapsed;
        }

        void t3_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                RunUploadShippingXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "[Shipping]" + ex.Message + "<br />";
                DbHelperSQL dbHelper = new DbHelperSQL("HKOERPCONNECTION");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Jins_Log(");
                strSql.Append("ID,LogDate,Msg)");
                strSql.Append(" values (");
                strSql.Append("(select ERP_Management.dbo.SF_GetID(25)),getdate(),@Msg)");
                SqlParameter[] parameters = {
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
                parameters[0].Value = ex.Message;
                dbHelper.ExecuteSql(strSql.ToString(), parameters);
            }
        }

        void t2_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                RunUploadConfirmXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "[Confirm]" + ex.Message + "<br />";
            }
        }

        private System.Timers.Timer CreateTimer(double intervel)
        {
            System.Timers.Timer myTimer1 = new System.Timers.Timer(intervel);
            myTimer1.Enabled = true;
            myTimer1.AutoReset = true;
            return myTimer1;
        }

        private void t1_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                RunGetXMLFiles();
            }
            catch (Exception ex)
            {
                Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ex.Message + "<br />";
            }
        }

        private void RunGetXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.GetXMLFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] xmlfiles get from server.<br />";
        }

        private void RunUploadConfirmXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.UploadConfirmFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] confirmxmlfiles upload to server.<br />";
        }

        private void RunUploadShippingXMLFiles()
        {
            Jins jins = new Jins();
            var count = jins.UploadShippingFiles();
            Default.html += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " [" + count.ToString() + "] shippingxmlfiles upload to server.<br />";
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  
            Thread.Sleep(1000);
            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  
            string url = "http://www.123.com";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
