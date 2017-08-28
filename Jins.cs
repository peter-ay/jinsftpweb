using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jinsftpweb
{
    public class Jins
    {
        private string ftpServerIP;
        private string ftpServerFolder_order;
        private string ftpUserID;
        private string ftpPassword;
        private string ftpServerFolder_confirmation;
        private string ftpServerFolder_shipping;

        public Jins()
        {
            this.ftpServerIP = "180.167.68.150";
            this.ftpServerFolder_order = "Output";
            this.ftpServerFolder_confirmation = "confirmation";
            this.ftpServerFolder_shipping = "shipping";
            this.ftpUserID = "UserSino";
            this.ftpPassword = "UserSino010!@#$%^";
        }

        public void GetXMLFiles()
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            string[] fileList = null;
            string fileName = "";
            string saveFilePath = HttpContext.Current.Server.MapPath(@"\") + @"xml\order";
            while (true)
            {
                try
                {
                    fileList = ftp.GetFileList(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword);
                    if (null == fileList) break;
                    fileName = fileList[0];
                    ftp.DownloadFile(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword, fileName, saveFilePath, fileName);
                    ftp.DeleteFile(this.ftpServerIP, this.ftpServerFolder_order, fileName, this.ftpUserID, this.ftpPassword);
                }
                catch (Exception)
                {
                    break;
                }
                finally
                {

                }
            }
        }
    }
}