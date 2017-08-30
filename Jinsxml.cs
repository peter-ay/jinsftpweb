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
                modelZMain.AxisR = model.SubRX.Oculus_Dexter_Axis.ToString();
                modelZMain.AxisL = model.SubRX.Oculus_Sinister_Axis.ToString();
                modelZMain.Center_ThicknessL = model.SubRX.SpecialLeft3;
                modelZMain.Center_ThicknessR = model.SubRX.SpecialRight3;
                modelZMain.CYLL = (model.SubRX.Oculus_Sinister_Cylinder * 100).ToString().GetIntStr();
                modelZMain.CYLR = (model.SubRX.Oculus_Dexter_Cylinder * 100).ToString().GetIntStr();
                modelZMain.DiameterL = model.SubRX.Oculus_Sinister_Diameter;
                modelZMain.DiameterR = model.SubRX.Oculus_Dexter_Diameter;
                modelZMain.SPHL = (model.SubRX.Oculus_Sinister_Sphere * 100).ToString().GetIntStr();
                modelZMain.SPHR = (model.SubRX.Oculus_Dexter_Sphere * 100).ToString().GetIntStr();
                modelZMain.X_ADDL = (model.SubRX.Oculus_Sinister_Add * 100).ToString().GetIntStr();
                modelZMain.X_ADDR = (model.SubRX.Oculus_Dexter_Add * 100).ToString().GetIntStr();
                modelZMain.QuantityL = model.SubRX.Oculus_Sinister_Quantity;
                modelZMain.QuantityR = model.SubRX.Oculus_Dexter_Quantity;
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
    }
}