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
            var file = xmlPath.Substring(xmlPath.LastIndexOf("\\") + 1);
            model.OrdID = file.Substring(0, file.LastIndexOf("."));
            try
            {
                using (XmlReader _XReader = XmlReader.Create(xmlPath))
                {
                    XElement _XElement = XElement.Load(_XReader);
                    model.OrdHdID = _XElement.Element("purchase_order_header_id").Value;
                    model.OrdType = _XElement.Element("po_type_id").Value;
                    model.Address1 = _XElement.Element("address").Value;
                    model.Postal = _XElement.Element("postal").Value;
                    model.Tel = _XElement.Element("telephone").Value;
                    model.Memo = _XElement.Element("memorandum").Value;
                    var t = _XElement.Element("created").Value;
                    try { model.Created = Convert.ToDateTime(t); }
                    catch { model.Created = DateTime.Now; }
                    model.SalesOfficeCode = _XElement.Element("salesOfficeCode").Value;
                    model.SalesOfficeName = _XElement.Element("salesOfficeName").Value;
                    //
                    switch (model.OrdType)
                    {
                        case "rx":
                            model.SubRX = new OrdRX();
                            var modelrx = model.SubRX;
                            var _detail = _XElement.Elements("detail").FirstOrDefault();
                            //
                            modelrx.SpecialRight1 = _detail.Element("specialright1").Value;
                            modelrx.SpecialRight2 = _detail.Element("specialright2").Value;
                            modelrx.SpecialRight3 = _detail.Element("specialright3").Value;
                            modelrx.SpecialRight4 = _detail.Element("specialright4").Value;
                            modelrx.SpecialLeft1 = _detail.Element("specialleft1").Value;
                            modelrx.SpecialLeft2 = _detail.Element("specialleft2").Value;
                            modelrx.SpecialLeft3 = _detail.Element("specialleft3").Value;
                            modelrx.SpecialLeft4 = _detail.Element("specialleft4").Value;
                            // 
                            modelrx.Lens_Type = _detail.Element("lens_type").Value;
                            modelrx.Tint_Type = _detail.Element("tint_type").Value;
                            modelrx.Tint = _detail.Element("tint").Value;
                            modelrx.Polarized = _detail.Element("polarized").Value;
                            modelrx.Mirrored = _detail.Element("mirrored").Value;
                            //
                            modelrx.Oculus_Dexter_Sphere = _detail.Element("oculus_dexter_sphere").Value.GetDecimalStr();
                            modelrx.Oculus_Sinister_Sphere = _detail.Element("oculus_sinister_sphere").Value.GetDecimalStr();
                            modelrx.Oculus_Dexter_Cylinder = _detail.Element("oculus_dexter_cylinder").Value.GetDecimalStr();
                            modelrx.Oculus_Sinister_Cylinder = _detail.Element("oculus_sinister_cylinder").Value.GetDecimalStr();
                            modelrx.Oculus_Dexter_Axis = _detail.Element("oculus_dexter_axis").Value.GetDecimalStr();
                            modelrx.Oculus_Sinister_Axis = _detail.Element("oculus_sinister_axis").Value.GetDecimalStr();
                            modelrx.Oculus_Dexter_Add = _detail.Element("oculus_dexter_add").Value.GetDecimalStr();
                            modelrx.Oculus_Sinister_Add = _detail.Element("oculus_sinister_add").Value.GetDecimalStr();
                            //
                            modelrx.Oculus_Dexter_Diameter = _detail.Element("oculus_dexter_diameter").Value.GetIntStr();
                            modelrx.Oculus_Sinister_Diameter = _detail.Element("oculus_sinister_diameter").Value.GetIntStr();
                            //
                            modelrx.Oculus_Dexter_Quantity = _detail.Element("oculus_dexter_quantity").Value.GetIntStr();
                            modelrx.Oculus_Sinister_Quantity = _detail.Element("oculus_sinister_quantity").Value.GetIntStr();
                            break;

                        default:

                            model.SubST = new List<OrdST>();
                            var modelst = model.SubST;
                            OrdST ordst = null;
                            var details = _XElement.Elements("detail");
                            foreach (var item in details)
                            {
                                ordst = new OrdST()
                                {
                                    SubID = item.Attribute("id").Value.GetIntStr(),
                                    OPC = item.Element("opc").Value,
                                    Qty = item.Element("qty").Value.GetIntStr(),
                                };
                                modelst.Add(ordst);
                            }
                            break;
                    }
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