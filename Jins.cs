using Jinsftpweb.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Jinsftpweb
{
    public class Jins
    {
        private string ftpServerIP;
        private string ftpUserID;
        private string ftpPassword;

        private string ftpServerFolder_order;
        private string ftpServerFolder_confirmation;
        private string ftpServerFolder_shipping;

        private string localFolder_order = HttpRuntime.AppDomainAppPath + @"xml\order";
        private string localFolder_confirm = HttpRuntime.AppDomainAppPath + @"xml\confirmation";
        private string localFolder_shipping = HttpRuntime.AppDomainAppPath + @"xml\shipping";

        public Jins()
        {
            using (XmlReader _XReader = XmlReader.Create(HttpRuntime.AppDomainAppPath + @"ftp.xml"))
            {
                XElement _XElement = XElement.Load(_XReader);
                this.ftpServerIP = _XElement.Element("ftpServerIP").Attribute("value").Value;
                this.ftpUserID = _XElement.Element("ftpUserID").Attribute("value").Value;
                this.ftpPassword = _XElement.Element("ftpPassword").Attribute("value").Value;
            }
            this.ftpServerFolder_order = "order";
            this.ftpServerFolder_confirmation = "confirmation";
            this.ftpServerFolder_shipping = "shipping";
        }

        public int GetXMLFiles()
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            string[] fileList = null;
            string fileName = "";
            int countGetFiles = 0;
            while (true)
            {
                fileList = ftp.GetFileList(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword);
                if (null == fileList) break;
                fileName = fileList[0];
                JinsPub.OrdID = fileName.Substring(0, fileName.LastIndexOf("."));
                ftp.DownloadFile(this.ftpServerIP, this.ftpServerFolder_order, this.ftpUserID, this.ftpPassword, fileName, localFolder_order, fileName);
                File.Copy(Path.Combine(localFolder_order, fileName), Path.Combine(localFolder_order + @"\backup", fileName), true);
                //var fullPath = localFolder_order + @"\" + fileName;
                //var model = Jinsxml.ConvertXMLtoOrdModel(fullPath);
                //Jinsdb.AddOrd(model);
                //FileInfo fi = new FileInfo(fullPath);
                //fi.Delete();
                ftp.DeleteFile(this.ftpServerIP, this.ftpServerFolder_order, fileName, this.ftpUserID, this.ftpPassword);
                countGetFiles++;
            }
            return countGetFiles;
        }

        public int ConvertXMLFile()
        {
            DirectoryInfo folder = new DirectoryInfo(localFolder_order);
            FileInfo[] fileList = null;
            string fileName = "";
            var count = 0;
            fileList = folder.GetFiles("*.xml");
            if (fileList.Count() <= 0) return 0;

            foreach (FileInfo fi in fileList)
            {
                fileName = fi.Name;
                JinsPub.OrdID = fileName.Substring(0, fileName.LastIndexOf("."));
                var fullPath = localFolder_order + @"\" + fileName;
                OrdMain model = Jinsxml.ConvertXMLtoOrdModel(fullPath);
                if (Jinsdb.CheckOrdReject(model.OrdID))
                {
                    fi.Delete();
                    continue;
                }
                Jinsdb.DeleteOrdErr(model.OrdID);
                try
                {
                    Jinsdb.AddOrd(model);
                    fi.Delete();
                }
                catch (Exception ex)
                {
                    OrdMain modelErr = new OrdMain()
                    {
                        Address1 = "",
                        Created = DateTime.Now,
                        ECode = "",
                        OrdHdID = model.OrdHdID,
                        Postal = "",
                        SalesOfficeCode = "",
                        SalesOfficeName = "",
                        OrdType = model.OrdType,
                        OrdID = model.OrdID,
                        Tel = "",
                        Memo = "Err",
                    };
                    modelErr.SubZ = new OrdZ()
                    {
                        BillCode = model.OrdID,
                        Remark = ex.Message,
                        BillDate = model.Created,
                        BillType = "",
                        ConsignDate = model.Created,
                        CusCode = "",
                        ML = "",
                        MR = "",
                        Notes = "",
                        OBillCode = "",
                        QtyL = 0,
                        QtyR = 0,
                        SumQty = 0
                    };
                    modelErr.SubConet = new OrdConet()
                    {
                        BCode1 = model.OrdID,
                        BCode2 = "*轉換錯誤請參考備註信息*",
                        F_Confirm = false,
                        F_Read = false,
                        F_Reject = false,
                        F_Shipping = 0,
                        F_Err_Convert = true
                    };
                    Jinsdb.AddOrdErr(modelErr);
                }
                count++;
            }

            return count;
        }

        public int GetConfirmFiles()
        {
            var rs = Jinsdb.GetUnConfirmFiles();
            if (rs.Tables[0].Rows.Count <= 0) return 0;
            foreach (DataRow item in rs.Tables[0].Rows)
            {
                var _ID = item["ID"].ToString();
                var _OrdType = item["OrdType"].ToString();
                var _OrdHdID = item["OrdHdID"].ToString();
                var _OrdID = item["OrdID"].ToString();
                var _F_Reject = item["F_Reject"].ToString().GetBoolStr();
                JinsPub.OrdID = _OrdID;
                OrdMain model = new OrdMain()
                {
                    ID = _ID,
                    OrdID = _OrdID,
                    OrdHdID = _OrdHdID,
                    OrdType = _OrdType,
                    F_Reject = _F_Reject
                };
                if (model.OrdType.ToLower() != "rx")
                {
                    model.SubST = new List<OrdST>();
                    var rsdetail = Jinsdb.GetOrdDetail(model.ID);
                    foreach (DataRow item2 in rsdetail.Tables[0].Rows)
                    {
                        var subid = item2["SubID"].ToString().GetIntFromStr();
                        var opc = item2["OPC"].ToString();
                        var qty = item2["Qty"].ToString().GetIntFromStr();
                        model.SubST.Add(new OrdST() { ID = model.ID, SubID = subid, OPC = opc, Qty = qty });
                    }
                }
                var xmldoc = Jinsxml.CreateConfirmXMLFile(model);
                xmldoc.Save(localFolder_confirm + @"\" + model.OrdID + @".xml");
                Jinsdb.UpdateConfirmFlat(model.OrdID);
            }
            return rs.Tables[0].Rows.Count;
        }

        public int GetShippingFilesRX()
        {
            var rs = Jinsdb.GetUnShippingFilesRX();
            if (rs.Tables[0].Rows.Count <= 0) return 0;
            foreach (DataRow item in rs.Tables[0].Rows)
            {
                OrdMain model = this.PrepareShippingModel(item);
                var xmldoc = Jinsxml.CreateShippingXMLFile(model);
                xmldoc.Save(localFolder_shipping + @"\" + model.OrdID + @".xml");
                Jinsdb.UpdateShippingFlatRX(model.OrdID);
            }
            return rs.Tables[0].Rows.Count;
        }

        private OrdMain PrepareShippingModel(DataRow item)
        {
            var _ID = item["ID"].ToString();
            var _OrdType = item["OrdType"].ToString();
            var _OrdHdID = item["OrdHdID"].ToString();
            var _OrdID = item["OrdID"].ToString();
            var _ECode = item["ECode"].ToString();
            JinsPub.OrdID = _OrdID;
            OrdMain model = new OrdMain()
            {
                ID = _ID,
                OrdID = _OrdID,
                OrdHdID = _OrdHdID,
                OrdType = _OrdType,
                ECode = _ECode
            };
            return model;
        }

        public int GetShippingFilesST()
        {
            Jinsdb.GetImportShippingST();

            var rs = Jinsdb.GetUnShippingFilesST();
            if (rs.Tables[0].Rows.Count <= 0) return 0;
            foreach (DataRow item in rs.Tables[0].Rows)
            {
                OrdMain model = this.PrepareShippingModel(item);
                model.SubST = new List<OrdST>();
                var rsdetail = Jinsdb.GetOrdShipDetail(model.ID);
                foreach (DataRow item2 in rsdetail.Tables[0].Rows)
                {
                    var subid = item2["SubID"].ToString().GetIntFromStr();
                    var opc = item2["OPC"].ToString();
                    var qtyship = item2["QtyShip"].ToString().GetIntFromStr();
                    model.SubST.Add(new OrdST() { ID = model.ID, SubID = subid, OPC = opc, Qty = qtyship });
                }
                var xmldoc = Jinsxml.CreateShippingXMLFile(model);
                xmldoc.Save(localFolder_shipping + @"\" + model.OrdID + @".xml");
                Jinsdb.UpdateShippingFlatST(model);
            }
            return rs.Tables[0].Rows.Count;
        }

        public int UploadConfirmFiles()
        {
            return this.UploadXMLFiles(this.localFolder_confirm, this.ftpServerFolder_confirmation);
        }

        private int UploadXMLFiles(string fromLocalFolder, string toFTPServerFolder)
        {
            FTPLib.FTP ftp = new FTPLib.FTP();
            DirectoryInfo folder = new DirectoryInfo(fromLocalFolder);
            FileInfo[] fileList = null;
            string fileName = "";
            string fullFileName = "";
            var count = 0;
            while (true)
            {
                fileList = folder.GetFiles("*.xml");
                if (fileList.Count() == 0)
                    break;
                fileName = fileList[0].Name;
                JinsPub.OrdID = fileName;
                fullFileName = fileList[0].FullName;
                ftp.UploadFile(ftpServerIP, toFTPServerFolder, fullFileName, ftpUserID, ftpPassword);
                File.Copy(Path.Combine(fromLocalFolder, fileName), Path.Combine(fromLocalFolder + @"\backup", fileName), true);
                fileList[0].Delete();
                count++;
            }
            return count;
        }

        public int UploadShippingFiles()
        {
            return this.UploadXMLFiles(this.localFolder_shipping, this.ftpServerFolder_shipping);
        }

    }
}