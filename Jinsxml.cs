using Jinsftpweb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Jinsftpweb
{
    public class Jinsxml
    {
        public static OrdMain ConvertXMLtoOrdModel(string xmlPath)
        {
            OrdMain model = new OrdMain();
            try
            {
                using (XmlReader _XReader = XmlReader.Create(xmlPath))
                {
                    XElement _XElement = XElement.Load(_XReader);
                    //var t = _XElement.Element("id").Value;
                    //model.WebOrderCode = t;
                    //t = _XElement.Element("otype").Value;
                    //model.OrderType = t == "1" ? "A" : "C";
                    //t = _XElement.Element("odate").Value;
                    //string y = t.Substring(0, 4);
                    //string m = t.Substring(4, 2);
                    //string d = t.Substring(6, 2);
                    //model.OrderDate = Convert.ToDateTime(y + "-" + m + "-" + d);

                }
                return model;
            }
            catch (Exception ex)
            {
                Jinsdb.AddLog(ex.Message);
                throw;
            }
        }

    }
}