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
            model.ID = "";
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
                    try { model.Procurement_Type = _XElement.Element("procurement_type").Value; }
                    catch { model.Procurement_Type = ""; }
                    model.SubExtend.f_test = true;
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
                            modelrx.Oculus_Dexter_Diameter = _detail.Element("oculus_dexter_diameter").Value.GetIntFromStr();
                            modelrx.Oculus_Sinister_Diameter = _detail.Element("oculus_sinister_diameter").Value.GetIntFromStr();
                            //
                            modelrx.Oculus_Dexter_Quantity = _detail.Element("oculus_dexter_quantity").Value.GetIntFromStr();
                            modelrx.Oculus_Sinister_Quantity = _detail.Element("oculus_sinister_quantity").Value.GetIntFromStr();

                            //new column
                            try { modelrx.Frame_Code = _detail.Element("frame_code").Value; }
                            catch { modelrx.Frame_Code = ""; }
                            try { modelrx.Polishing = _detail.Element("polishing").Value; }
                            catch { modelrx.Polishing = ""; }
                            try { modelrx.Oculus_Eye_Point = _detail.Element("oculus_eye_point").Value; }
                            catch { modelrx.Oculus_Eye_Point = ""; }
                            try { modelrx.Oculus_Dexter_Pupillary_Distance = _detail.Element("oculus_dexter_pupillary_distance").Value; }
                            catch { modelrx.Oculus_Dexter_Pupillary_Distance = ""; }
                            try { modelrx.Oculus_Sinister_Pupillary_Distance = _detail.Element("oculus_sinister_pupillary_distance").Value; }
                            catch { modelrx.Oculus_Sinister_Pupillary_Distance = ""; }

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
                                    SubID = item.Attribute("id").Value.GetIntFromStr(),
                                    OPC = item.Element("opc").Value,
                                    Qty = item.Element("qty").Value.GetIntFromStr(),
                                };
                                modelst.Add(ordst);
                            }
                            break;
                    }
                }
                //ordz
                InitOrdZ(model);
                //ordzmain
                InitOrdZMain(model);

                return model;
            }
            catch (Exception ex)
            {
                Jinsdb.AddLog(ex.Message);
                throw;
            }
        }

        private static void InitOrdZMain(OrdMain model)
        {
            model.SubZMain = new OrdZMain()
            {
                //AxisR = model.SubRX.Oculus_Dexter_Axis.ToString(),
                //AxisL = model.SubRX.Oculus_Sinister_Axis.ToString(),
                AxisR = "",
                AxisL = "",
                BillCode = model.OrdID,
                CaiBian = "",
                //Center_ThicknessL = model.SubRX.SpecialLeft3,
                //Center_ThicknessR = model.SubRX.SpecialRight3,
                Center_ThicknessL = "",
                Center_ThicknessR = "",
                ChaSe = "",
                CheBian = "",
                CusCode = "",
                //CYLL = (model.SubRX.Oculus_Sinister_Cylinder * 100).ToString().GetIntStr(),
                //CYLR = (model.SubRX.Oculus_Dexter_Cylinder * 100).ToString().GetIntStr(),
                CYLL = 0,
                CYLR = 0,
                DaoBianL = false,
                DaoBianR = false,
                Decenter1L = "",
                Decenter1R = "",
                Decenter2L = "",
                Decenter2R = "",
                Decenter3L = "",
                Decenter3R = "",
                Decenter4L = "",
                Decenter4R = "",
                //DiameterL = model.SubRX.Oculus_Sinister_Diameter,
                //DiameterR = model.SubRX.Oculus_Dexter_Diameter,
                DiameterL = 0,
                DiameterR = 0,
                ExtraProcess = "",
                Flag_CX = false,
                Hardened = false,
                ID = model.ID,
                JingJia = "",
                JuSe = "",
                KaiKeng = "",
                LR_Flag = "",
                MianWanL = "",
                MianWanR = "",
                Mnumber = "",
                MnumberR = "",
                MnumberL = "",
                Notes = model.Memo,
                OBillCode = "",
                OtherProcess = "",
                PaoGuang = "",
                PD = "",
                PiHua = "",
                Prism1L = "",
                Prism1R = "",
                Prism2L = "",
                Prism2R = "",
                Prism3L = "",
                Prism3R = "",
                Prism4L = "",
                Prism4R = "",
                QuantityL = 0,
                QuantityR = 0,
                RanSe = "",
                RanSeName = "",
                Remark = "",
                ShuiYin = "",
                //SPHL = (model.SubRX.Oculus_Sinister_Sphere * 100).ToString().GetIntStr(),
                //SPHR = (model.SubRX.Oculus_Dexter_Sphere * 100).ToString().GetIntStr(),
                SPHL = 0,
                SPHR = 0,
                UV = false,
                //X_ADDL = (model.SubRX.Oculus_Sinister_Add * 100).ToString().GetIntStr(),
                //X_ADDR = (model.SubRX.Oculus_Dexter_Add * 100).ToString().GetIntStr(),
                X_ADDL = 0,
                X_ADDR = 0,
                ZuanKong = ""
            };
            var modelZMain = model.SubZMain;
            if (model.OrdType.ToLower() == "rx")
            {
                modelZMain.Center_ThicknessL = model.SubRX.SpecialLeft3.Replace("CT ", "");
                modelZMain.Center_ThicknessR = model.SubRX.SpecialRight3.Replace("CT ", "");

                ////////Prism//////////////
                var pr = model.SubRX.SpecialRight1.Replace("P ", "");
                if (!string.IsNullOrEmpty(pr))
                {
                    var p1r = pr.Substring(4).GetPrismDecenterStr();
                    var p2r = pr.Substring(0, 4);
                    if (p1r == "UP" || p1r == "DOWN")
                    {
                        modelZMain.Prism1R = p1r;
                        modelZMain.Prism2R = p2r;
                    }
                    else
                    {
                        modelZMain.Prism3R = p1r;
                        modelZMain.Prism4R = p2r;
                    }
                }
                //
                var pl = model.SubRX.SpecialLeft1.Replace("P ", "");
                if (!string.IsNullOrEmpty(pl))
                {
                    var p1l = pl.Substring(4).GetPrismDecenterStr();
                    var p2l = pl.Substring(0, 4);
                    if (p1l == "UP" || p1l == "DOWN")
                    {
                        modelZMain.Prism1L = p1l;
                        modelZMain.Prism2L = p2l;
                    }
                    else
                    {
                        modelZMain.Prism3L = p1l;
                        modelZMain.Prism4L = p2l;
                    }

                }
                ////////Prism//////////////
                ////////Decenter//////////////
                var dr = model.SubRX.SpecialRight2.Replace("D ", "");
                if (!string.IsNullOrEmpty(dr))
                {
                    var d1r = dr.Substring(4).GetPrismDecenterStr();
                    var d2r = dr.Substring(0, 4);
                    modelZMain.Decenter3R = d1r;
                    modelZMain.Decenter4R = d2r;
                }
                //
                var dl = model.SubRX.SpecialLeft2.Replace("D ", "");
                if (!string.IsNullOrEmpty(dl))
                {
                    var d1l = dl.Substring(4).GetPrismDecenterStr();
                    var d2l = dl.Substring(0, 4);
                    modelZMain.Decenter3L = d1l;
                    modelZMain.Decenter4L = d2l;
                }
                ////////Decenter//////////////
                modelZMain.SPHL = (model.SubRX.Oculus_Sinister_Sphere * 100).ToString().GetIntFromStr();
                modelZMain.SPHR = (model.SubRX.Oculus_Dexter_Sphere * 100).ToString().GetIntFromStr();
                modelZMain.CYLL = (model.SubRX.Oculus_Sinister_Cylinder * 100).ToString().GetIntFromStr();
                modelZMain.CYLR = (model.SubRX.Oculus_Dexter_Cylinder * 100).ToString().GetIntFromStr();
                modelZMain.X_ADDL = (model.SubRX.Oculus_Sinister_Add * 100).ToString().GetIntFromStr();
                modelZMain.X_ADDR = (model.SubRX.Oculus_Dexter_Add * 100).ToString().GetIntFromStr();
                modelZMain.AxisR = (model.SubRX.Oculus_Dexter_Axis).ToString().GetIntFromStr().ToString();
                modelZMain.AxisL = (model.SubRX.Oculus_Sinister_Axis).ToString().GetIntFromStr().ToString();

                modelZMain.DiameterL = model.SubRX.Oculus_Sinister_Diameter;
                modelZMain.DiameterR = model.SubRX.Oculus_Dexter_Diameter;

                modelZMain.QuantityL = model.SubRX.Oculus_Sinister_Quantity;
                modelZMain.QuantityR = model.SubRX.Oculus_Dexter_Quantity;

                //+cyl
                // 1、CYL1 > 0，AX1 大于 90 时
                //SPH2=SPH1 +CYL1
                //CYL2=-CYL1
                //AX2=AX1-90
                //2、CYL1> 0，AX1 小于等于 90 时
                //SPH2=SPH1+CYL1
                //CYL2=-CYL1
                //AX2=AX1+90

                int axis = 0;
                int sphC = 0;
                int cylC = 0;
                if (modelZMain.CYLR > 0)
                {
                    try
                    {
                        axis = Convert.ToInt32(modelZMain.AxisR);
                    }
                    catch (Exception) { axis = 0; }
                    if (axis > 90)
                    {
                        sphC = modelZMain.SPHR + modelZMain.CYLR;
                        cylC = (-1) * modelZMain.CYLR;
                        axis = axis - 90;
                    }
                    else
                    {
                        sphC = modelZMain.SPHR + modelZMain.CYLR;
                        cylC = (-1) * modelZMain.CYLR;
                        axis = axis + 90;
                    }
                    modelZMain.SPHR = sphC;
                    modelZMain.CYLR = cylC;
                    modelZMain.AxisR = axis.ToString();
                }
                if (modelZMain.CYLL > 0)
                {
                    try
                    {
                        axis = Convert.ToInt32(modelZMain.AxisL);
                    }
                    catch (Exception) { axis = 0; }
                    if (axis > 90)
                    {
                        sphC = modelZMain.SPHL + modelZMain.CYLL;
                        cylC = (-1) * modelZMain.CYLL;
                        axis = axis - 90;
                    }
                    else
                    {
                        sphC = modelZMain.SPHL + modelZMain.CYLL;
                        cylC = (-1) * modelZMain.CYLL;
                        axis = axis + 90;
                    }
                    modelZMain.SPHL = sphC;
                    modelZMain.CYLL = cylC;
                    modelZMain.AxisL = axis.ToString();
                }
            }

            if (model.OrdType.ToLower() == "rx" && model.Procurement_Type == "subcontract")
            {
                modelZMain.Flag_CX = true;
                modelZMain.JingJia = model.SubRX.Frame_Code;
                if (model.SubRX.Polishing == "true")
                    modelZMain.PaoGuang = "EXE";
                modelZMain.ExtraProcess = model.SubRX.Oculus_Eye_Point;
                modelZMain.PD = model.SubRX.Oculus_Dexter_Pupillary_Distance + "/" + model.SubRX.Oculus_Sinister_Pupillary_Distance;
            }


        }

        private static void InitOrdZ(OrdMain model)
        {
            model.SubZ = new OrdZ()
            {
                BillCode = model.OrdID,
                BillDate = model.Created,
                ConsignDate = model.Created.AddDays(2),
                BillType = model.OrdType.ToLower() == "rx" ? "XSSD" : "XSPD",
                CusCode = "",
                ID = model.ID,
                MR = "",
                ML = "",
                Notes = model.Memo,
                OBillCode = "",
                QtyR = 0,
                QtyL = 0,
                Remark = "",
                SumQty = 0
            };
            var modelZ = model.SubZ;
            if (model.OrdType.ToLower() == "rx")
            {
                modelZ.QtyL = model.SubRX.Oculus_Sinister_Quantity;
                modelZ.QtyR = model.SubRX.Oculus_Dexter_Quantity;
                modelZ.SumQty = modelZ.QtyL + modelZ.QtyR;
            }
            else
            {
                modelZ.SumQty = model.SubST.Sum(it => it.Qty);
            }

        }

        public static XmlDocument CreateConfirmXMLFile(OrdMain model)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("order");
            doc.AppendChild(root);
            //
            XmlElement node = doc.CreateElement("purchase_order_header_id");
            node.InnerText = model.OrdHdID;
            root.AppendChild(node);
            //
            if (model.OrdType.ToLower() == "rx")
            {
                node = doc.CreateElement("detail");
                node.SetAttribute("id", "10");
                XmlElement nodeUnder = doc.CreateElement("confirmed_qty");
                if (model.F_Reject)
                    nodeUnder.InnerText = "0";
                else
                    nodeUnder.InnerText = "1";
                node.AppendChild(nodeUnder);
                root.AppendChild(node);
            }
            else
            {
                model.SubST.ForEach(it =>
                {
                    node = doc.CreateElement("detail");
                    node.SetAttribute("id", it.SubID.ToString());
                    XmlElement nodeUnder = doc.CreateElement("opc");
                    nodeUnder.InnerText = it.OPC;
                    node.AppendChild(nodeUnder);
                    nodeUnder = doc.CreateElement("confirmed_qty");
                    if (model.F_Reject)
                        nodeUnder.InnerText = "0";
                    else
                        nodeUnder.InnerText = it.Qty.ToString();
                    node.AppendChild(nodeUnder);
                    root.AppendChild(node);
                });

            }
            return doc;
        }

        public static XmlDocument CreateShippingXMLFile(OrdMain model)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("order");
            doc.AppendChild(root);
            //
            XmlElement node = doc.CreateElement("purchase_order_header_id");
            node.InnerText = model.OrdHdID;
            root.AppendChild(node);
            //
            node = doc.CreateElement("goods_issue_slip");
            node.InnerText = model.ECode;
            root.AppendChild(node);

            if (model.OrdType.ToLower() != "rx")
            {
                model.SubST.ForEach(it =>
                {
                    node = doc.CreateElement("detail");
                    node.SetAttribute("id", it.SubID.ToString());
                    XmlElement nodeUnder = doc.CreateElement("opc");
                    nodeUnder.InnerText = it.OPC;
                    node.AppendChild(nodeUnder);
                    nodeUnder = doc.CreateElement("issued_quantity");
                    nodeUnder.InnerText = it.Qty.ToString();
                    node.AppendChild(nodeUnder);
                    root.AppendChild(node);
                });
            }

            return doc;
        }
    }
}