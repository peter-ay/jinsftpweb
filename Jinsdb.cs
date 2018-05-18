using DbHelperSQLLib;
using Jinsftpweb.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Jinsftpweb
{
    public class Jinsdb
    {
        public static void AddLog(string msg)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Jins_Log");
            strSql.Append(" where LogDate<DATEADD(mm,-2,getdate()) ;");
            strSql.Append("insert into Jins_Log(");
            strSql.Append("ID,LogDate,OrdID,Msg)");
            strSql.Append(" values (");
            strSql.Append("(select ERP_Management.dbo.SF_GetID25()),getdate(),@OrdID,@Msg)");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrdID", SqlDbType.VarChar,50),
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
            parameters[0].Value = JinsPub.OrdID;
            parameters[1].Value = msg;
            dbHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        #region AddOrd

        public static void AddOrdErr(OrdMain model)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            List<DbHelperCmdObject> cmdObjec = new List<DbHelperCmdObject>();
            DbHelperCmdObject obj = null;
            //
            model.ID = JinsPub.ID25;
            //
            obj = PrePareOrdMain(model);
            cmdObjec.Add(obj);
            //
            model.SubZ.ID = model.ID;
            obj = PrePareOrdZ(model.SubZ);
            cmdObjec.Add(obj);
            //
            model.SubConet.ID = JinsPub.ID25;
            obj = PrePareOrdConet(model.SubConet);
            cmdObjec.Add(obj);
            //
            dbHelper.ExecuteCmdObjectTran(cmdObjec);
        }

        public static void DeleteOrdErr(string id)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            List<DbHelperCmdObject> cmdObjec = new List<DbHelperCmdObject>();
            DbHelperCmdObject obj = null;
            //
            obj = PrePareOrdDeleteErr(id);
            cmdObjec.Add(obj);
            //
            dbHelper.ExecuteCmdObjectTran(cmdObjec);
        }
        public static bool CheckOrdReject(string ordID)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            //
            var strSql = @"select ID from Jins_Conet A1 with (nolock) where BCode1=@OrdID and F_Reject=1";
            SqlParameter[] parameters ={
					new SqlParameter("@OrdID", SqlDbType.VarChar,50)};
            parameters[0].Value = ordID;
            var ds = dbHelper.GetSingle(strSql, parameters);
            return ds != null;
            // 
        }

        private static DbHelperCmdObject PrePareOrdDeleteErr(string ordID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SP_JinsOrd_DeleteOrdErr");
            SqlParameter[] parameters ={
					new SqlParameter("@OrdID", SqlDbType.VarChar,50)};
            parameters[0].Value = ordID;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters, CommandType.StoredProcedure);
            return obj;
        }

        public static void AddOrd(OrdMain model)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            List<DbHelperCmdObject> cmdObjec = new List<DbHelperCmdObject>();
            DbHelperCmdObject obj = null;
            //
            model.ID = JinsPub.ID25;
            //
            obj = PrePareOrdMain(model);
            cmdObjec.Add(obj);
            if (model.OrdType.ToLower() == "rx")
            {
                model.SubRX.ID = model.ID;
                obj = PrePareOrdRX(model.SubRX);
                cmdObjec.Add(obj);
            }
            else
            {
                foreach (var item in model.SubST)
                {
                    item.ID = model.ID;
                    obj = PrePareOrdST(item);
                    cmdObjec.Add(obj);
                }
            }
            model.SubZ.ID = model.ID;
            obj = PrePareOrdZ(model.SubZ);
            cmdObjec.Add(obj);
            model.SubZMain.ID = model.ID;
            obj = PrePareOrdZMain(model.SubZMain);
            cmdObjec.Add(obj);
            //
            obj = PrePareOrdSaveVerify(model.ID);
            cmdObjec.Add(obj);
            dbHelper.ExecuteCmdObjectTran(cmdObjec);
        }

        private static DbHelperCmdObject PrePareOrdConet(OrdConet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Conet(");
            strSql.Append("ID,BCode1,BCode2,F_Read,F_Confirm,F_Shipping,F_Reject,F_Err_Convert)");
            strSql.Append(" values (");
            strSql.Append("@ID,@BCode1,@BCode2,@F_Read,@F_Confirm,@F_Shipping,@F_Reject,@F_Err_Convert)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@BCode1", SqlDbType.VarChar,50),
					new SqlParameter("@BCode2", SqlDbType.VarChar,30),
					new SqlParameter("@F_Read", SqlDbType.Bit,1),
					new SqlParameter("@F_Confirm", SqlDbType.Bit,1),
					new SqlParameter("@F_Shipping", SqlDbType.Int,4),
					new SqlParameter("@F_Reject", SqlDbType.Bit,1),
                    new SqlParameter("@F_Err_Convert", SqlDbType.Bit,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BCode1;
            parameters[2].Value = model.BCode2;
            parameters[3].Value = model.F_Read;
            parameters[4].Value = model.F_Confirm;
            parameters[5].Value = model.F_Shipping;
            parameters[6].Value = model.F_Reject;
            parameters[7].Value = model.F_Err_Convert;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdZMain(OrdZMain model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_Z_Main(");
            strSql.Append("ID,BillCode,CusCode,Remark,Notes,PD,OBillCode,Flag_CX,AxisR,Center_ThicknessR,CYLR,DaoBianR,Decenter1R,Decenter2R,Decenter3R,Decenter4R,DiameterR,MianWanR,MnumberR,Prism1R,Prism2R,Prism3R,Prism4R,QuantityR,SPHR,X_ADDR,AxisL,Center_ThicknessL,CYLL,DaoBianL,Decenter1L,Decenter2L,Decenter3L,Decenter4L,DiameterL,MianWanL,MnumberL,Prism1L,Prism2L,Prism3L,Prism4L,QuantityL,SPHL,X_ADDL,CaiBian,ChaSe,CheBian,ExtraProcess,Hardened,JingJia,JuSe,KaiKeng,OtherProcess,PaoGuang,PiHua,RanSe,RanSeName,ShuiYin,UV,ZuanKong,LR_Flag,Mnumber)");
            strSql.Append(" values (");
            strSql.Append("@ID,@BillCode,@CusCode,@Remark,@Notes,@PD,@OBillCode,@Flag_CX,@AxisR,@Center_ThicknessR,@CYLR,@DaoBianR,@Decenter1R,@Decenter2R,@Decenter3R,@Decenter4R,@DiameterR,@MianWanR,@MnumberR,@Prism1R,@Prism2R,@Prism3R,@Prism4R,@QuantityR,@SPHR,@X_ADDR,@AxisL,@Center_ThicknessL,@CYLL,@DaoBianL,@Decenter1L,@Decenter2L,@Decenter3L,@Decenter4L,@DiameterL,@MianWanL,@MnumberL,@Prism1L,@Prism2L,@Prism3L,@Prism4L,@QuantityL,@SPHL,@X_ADDL,@CaiBian,@ChaSe,@CheBian,@ExtraProcess,@Hardened,@JingJia,@JuSe,@KaiKeng,@OtherProcess,@PaoGuang,@PiHua,@RanSe,@RanSeName,@ShuiYin,@UV,@ZuanKong,@LR_Flag,@Mnumber)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@BillCode", SqlDbType.NVarChar,30),
					new SqlParameter("@CusCode", SqlDbType.VarChar,22),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@PD", SqlDbType.NVarChar,10),
					new SqlParameter("@OBillCode", SqlDbType.NVarChar,30),
					new SqlParameter("@Flag_CX", SqlDbType.Bit,1),
					new SqlParameter("@AxisR", SqlDbType.NVarChar,5),
					new SqlParameter("@Center_ThicknessR", SqlDbType.NVarChar,5),
					new SqlParameter("@CYLR", SqlDbType.Int,4),
					new SqlParameter("@DaoBianR", SqlDbType.Bit,1),
					new SqlParameter("@Decenter1R", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter2R", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter3R", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter4R", SqlDbType.NVarChar,10),
					new SqlParameter("@DiameterR", SqlDbType.Int,4),
					new SqlParameter("@MianWanR", SqlDbType.NVarChar,10),
					new SqlParameter("@MnumberR", SqlDbType.NVarChar,30),
					new SqlParameter("@Prism1R", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism2R", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism3R", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism4R", SqlDbType.NVarChar,10),
					new SqlParameter("@QuantityR", SqlDbType.Int,4),
					new SqlParameter("@SPHR", SqlDbType.Int,4),
					new SqlParameter("@X_ADDR", SqlDbType.Int,4),
					new SqlParameter("@AxisL", SqlDbType.NVarChar,5),
					new SqlParameter("@Center_ThicknessL", SqlDbType.NVarChar,5),
					new SqlParameter("@CYLL", SqlDbType.Int,4),
					new SqlParameter("@DaoBianL", SqlDbType.Bit,1),
					new SqlParameter("@Decenter1L", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter2L", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter3L", SqlDbType.NVarChar,10),
					new SqlParameter("@Decenter4L", SqlDbType.NVarChar,10),
					new SqlParameter("@DiameterL", SqlDbType.Int,4),
					new SqlParameter("@MianWanL", SqlDbType.NVarChar,10),
					new SqlParameter("@MnumberL", SqlDbType.NVarChar,30),
					new SqlParameter("@Prism1L", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism2L", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism3L", SqlDbType.NVarChar,10),
					new SqlParameter("@Prism4L", SqlDbType.NVarChar,10),
					new SqlParameter("@QuantityL", SqlDbType.Int,4),
					new SqlParameter("@SPHL", SqlDbType.Int,4),
					new SqlParameter("@X_ADDL", SqlDbType.Int,4),
					new SqlParameter("@CaiBian", SqlDbType.NVarChar,20),
					new SqlParameter("@ChaSe", SqlDbType.NVarChar,20),
					new SqlParameter("@CheBian", SqlDbType.NVarChar,20),
					new SqlParameter("@ExtraProcess", SqlDbType.NVarChar,20),
					new SqlParameter("@Hardened", SqlDbType.Bit,1),
					new SqlParameter("@JingJia", SqlDbType.NVarChar,20),
					new SqlParameter("@JuSe", SqlDbType.NVarChar,20),
					new SqlParameter("@KaiKeng", SqlDbType.NVarChar,20),
					new SqlParameter("@OtherProcess", SqlDbType.NVarChar,20),
					new SqlParameter("@PaoGuang", SqlDbType.NVarChar,20),
					new SqlParameter("@PiHua", SqlDbType.NVarChar,20),
					new SqlParameter("@RanSe", SqlDbType.NVarChar,20),
					new SqlParameter("@RanSeName", SqlDbType.NVarChar,20),
					new SqlParameter("@ShuiYin", SqlDbType.NVarChar,20),
					new SqlParameter("@UV", SqlDbType.Bit,1),
					new SqlParameter("@ZuanKong", SqlDbType.NVarChar,20),
					new SqlParameter("@LR_Flag", SqlDbType.NVarChar,1),
					new SqlParameter("@Mnumber", SqlDbType.NVarChar,30)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BillCode;
            parameters[2].Value = model.CusCode;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.PD;
            parameters[6].Value = model.OBillCode;
            parameters[7].Value = model.Flag_CX;
            parameters[8].Value = model.AxisR;
            parameters[9].Value = model.Center_ThicknessR;
            parameters[10].Value = model.CYLR;
            parameters[11].Value = model.DaoBianR;
            parameters[12].Value = model.Decenter1R;
            parameters[13].Value = model.Decenter2R;
            parameters[14].Value = model.Decenter3R;
            parameters[15].Value = model.Decenter4R;
            parameters[16].Value = model.DiameterR;
            parameters[17].Value = model.MianWanR;
            parameters[18].Value = model.MnumberR;
            parameters[19].Value = model.Prism1R;
            parameters[20].Value = model.Prism2R;
            parameters[21].Value = model.Prism3R;
            parameters[22].Value = model.Prism4R;
            parameters[23].Value = model.QuantityR;
            parameters[24].Value = model.SPHR;
            parameters[25].Value = model.X_ADDR;
            parameters[26].Value = model.AxisL;
            parameters[27].Value = model.Center_ThicknessL;
            parameters[28].Value = model.CYLL;
            parameters[29].Value = model.DaoBianL;
            parameters[30].Value = model.Decenter1L;
            parameters[31].Value = model.Decenter2L;
            parameters[32].Value = model.Decenter3L;
            parameters[33].Value = model.Decenter4L;
            parameters[34].Value = model.DiameterL;
            parameters[35].Value = model.MianWanL;
            parameters[36].Value = model.MnumberL;
            parameters[37].Value = model.Prism1L;
            parameters[38].Value = model.Prism2L;
            parameters[39].Value = model.Prism3L;
            parameters[40].Value = model.Prism4L;
            parameters[41].Value = model.QuantityL;
            parameters[42].Value = model.SPHL;
            parameters[43].Value = model.X_ADDL;
            parameters[44].Value = model.CaiBian;
            parameters[45].Value = model.ChaSe;
            parameters[46].Value = model.CheBian;
            parameters[47].Value = model.ExtraProcess;
            parameters[48].Value = model.Hardened;
            parameters[49].Value = model.JingJia;
            parameters[50].Value = model.JuSe;
            parameters[51].Value = model.KaiKeng;
            parameters[52].Value = model.OtherProcess;
            parameters[53].Value = model.PaoGuang;
            parameters[54].Value = model.PiHua;
            parameters[55].Value = model.RanSe;
            parameters[56].Value = model.RanSeName;
            parameters[57].Value = model.ShuiYin;
            parameters[58].Value = model.UV;
            parameters[59].Value = model.ZuanKong;
            parameters[60].Value = model.LR_Flag;
            parameters[61].Value = model.Mnumber;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdZ(OrdZ model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_Z(");
            strSql.Append("ID,BillCode,BillDate,ConsignDate,CusCode,Remark,Notes,BillType,OBillCode,MR,QtyR,ML,QtyL,SumQty)");
            strSql.Append(" values (");
            strSql.Append("@ID,@BillCode,@BillDate,@ConsignDate,@CusCode,@Remark,@Notes,@BillType,@OBillCode,@MR,@QtyR,@ML,@QtyL,@SumQty)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@BillCode", SqlDbType.NVarChar,30),
					new SqlParameter("@BillDate", SqlDbType.Date,3),
					new SqlParameter("@ConsignDate", SqlDbType.Date,3),
					new SqlParameter("@CusCode", SqlDbType.NVarChar,10),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@BillType", SqlDbType.NVarChar,10),
					new SqlParameter("@OBillCode", SqlDbType.NVarChar,30),
					new SqlParameter("@MR", SqlDbType.NVarChar,30),
					new SqlParameter("@QtyR", SqlDbType.Int,4),
					new SqlParameter("@ML", SqlDbType.NVarChar,30),
					new SqlParameter("@QtyL", SqlDbType.Int,4),
					new SqlParameter("@SumQty", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BillCode;
            parameters[2].Value = model.BillDate;
            parameters[3].Value = model.ConsignDate;
            parameters[4].Value = model.CusCode;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.BillType;
            parameters[8].Value = model.OBillCode;
            parameters[9].Value = model.MR;
            parameters[10].Value = model.QtyR;
            parameters[11].Value = model.ML;
            parameters[12].Value = model.QtyL;
            parameters[13].Value = model.SumQty;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdSaveVerify(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SP_JinsOrd_SaveVerify");
            SqlParameter[] parameters ={
					new SqlParameter("@ID", SqlDbType.VarChar,25)};
            parameters[0].Value = id;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters, CommandType.StoredProcedure);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdST(OrdST model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_ST(");
            strSql.Append("ID,SubID,OPC,Qty)");
            strSql.Append(" values (");
            strSql.Append("@ID,@SubID,@OPC,@Qty)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@SubID", SqlDbType.Int,4),
					new SqlParameter("@OPC", SqlDbType.VarChar,50),
					new SqlParameter("@Qty", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.SubID;
            parameters[2].Value = model.OPC;
            parameters[3].Value = model.Qty;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdRX(OrdRX model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_RX(");
            strSql.Append("ID,SpecialRight1,SpecialRight2,SpecialRight3,SpecialRight4,SpecialLeft1,SpecialLeft2,SpecialLeft3,SpecialLeft4,Lens_Type,Tint_Type,Tint,Polarized,Mirrored,Oculus_Dexter_Sphere,Oculus_Sinister_Sphere,Oculus_Dexter_Cylinder,Oculus_Sinister_Cylinder,Oculus_Dexter_Axis,Oculus_Sinister_Axis,Oculus_Dexter_Add,Oculus_Sinister_Add,Oculus_Dexter_Diameter,Oculus_Sinister_Diameter,Oculus_Dexter_Quantity,Oculus_Sinister_Quantity,Frame_Code,Polishing,Oculus_Eye_Point,Oculus_Dexter_Pupillary_Distance,Oculus_Sinister_Pupillary_Distance)");
            strSql.Append(" values (");
            strSql.Append("@ID,@SpecialRight1,@SpecialRight2,@SpecialRight3,@SpecialRight4,@SpecialLeft1,@SpecialLeft2,@SpecialLeft3,@SpecialLeft4,@Lens_Type,@Tint_Type,@Tint,@Polarized,@Mirrored,@Oculus_Dexter_Sphere,@Oculus_Sinister_Sphere,@Oculus_Dexter_Cylinder,@Oculus_Sinister_Cylinder,@Oculus_Dexter_Axis,@Oculus_Sinister_Axis,@Oculus_Dexter_Add,@Oculus_Sinister_Add,@Oculus_Dexter_Diameter,@Oculus_Sinister_Diameter,@Oculus_Dexter_Quantity,@Oculus_Sinister_Quantity,@Frame_Code,@Polishing,@Oculus_Eye_Point,@Oculus_Dexter_Pupillary_Distance,@Oculus_Sinister_Pupillary_Distance)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@SpecialRight1", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialRight2", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialRight3", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialRight4", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialLeft1", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialLeft2", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialLeft3", SqlDbType.VarChar,20),
					new SqlParameter("@SpecialLeft4", SqlDbType.VarChar,20),
					new SqlParameter("@Lens_Type", SqlDbType.VarChar,30),
					new SqlParameter("@Tint_Type", SqlDbType.VarChar,30),
					new SqlParameter("@Tint", SqlDbType.VarChar,30),
					new SqlParameter("@Polarized", SqlDbType.VarChar,30),
					new SqlParameter("@Mirrored", SqlDbType.VarChar,30),
					new SqlParameter("@Oculus_Dexter_Sphere", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Sinister_Sphere", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Dexter_Cylinder", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Sinister_Cylinder", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Dexter_Axis", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Sinister_Axis", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Dexter_Add", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Sinister_Add", SqlDbType.Decimal,5),
					new SqlParameter("@Oculus_Dexter_Diameter", SqlDbType.Int,4),
					new SqlParameter("@Oculus_Sinister_Diameter", SqlDbType.Int,4),
					new SqlParameter("@Oculus_Dexter_Quantity", SqlDbType.Int,4),
					new SqlParameter("@Oculus_Sinister_Quantity", SqlDbType.Int,4),
					new SqlParameter("@Frame_Code", SqlDbType.NVarChar,30),
					new SqlParameter("@Polishing", SqlDbType.NVarChar,30),
					new SqlParameter("@Oculus_Eye_Point", SqlDbType.NVarChar,30),
					new SqlParameter("@Oculus_Dexter_Pupillary_Distance", SqlDbType.NVarChar,30),
					new SqlParameter("@Oculus_Sinister_Pupillary_Distance", SqlDbType.NVarChar,30)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.SpecialRight1;
            parameters[2].Value = model.SpecialRight2;
            parameters[3].Value = model.SpecialRight3;
            parameters[4].Value = model.SpecialRight4;
            parameters[5].Value = model.SpecialLeft1;
            parameters[6].Value = model.SpecialLeft2;
            parameters[7].Value = model.SpecialLeft3;
            parameters[8].Value = model.SpecialLeft4;
            parameters[9].Value = model.Lens_Type;
            parameters[10].Value = model.Tint_Type;
            parameters[11].Value = model.Tint;
            parameters[12].Value = model.Polarized;
            parameters[13].Value = model.Mirrored;
            parameters[14].Value = model.Oculus_Dexter_Sphere;
            parameters[15].Value = model.Oculus_Sinister_Sphere;
            parameters[16].Value = model.Oculus_Dexter_Cylinder;
            parameters[17].Value = model.Oculus_Sinister_Cylinder;
            parameters[18].Value = model.Oculus_Dexter_Axis;
            parameters[19].Value = model.Oculus_Sinister_Axis;
            parameters[20].Value = model.Oculus_Dexter_Add;
            parameters[21].Value = model.Oculus_Sinister_Add;
            parameters[22].Value = model.Oculus_Dexter_Diameter;
            parameters[23].Value = model.Oculus_Sinister_Diameter;
            parameters[24].Value = model.Oculus_Dexter_Quantity;
            parameters[25].Value = model.Oculus_Sinister_Quantity;
            parameters[26].Value = model.Frame_Code;
            parameters[27].Value = model.Polishing;
            parameters[28].Value = model.Oculus_Eye_Point;
            parameters[29].Value = model.Oculus_Dexter_Pupillary_Distance;
            parameters[30].Value = model.Oculus_Sinister_Pupillary_Distance;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdMain(OrdMain model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_Main(");
            strSql.Append("ID,OrdID,OrdHdID,OrdType,Address1,Postal,Tel,Memo,Created,SalesOfficeCode,SalesOfficeName,Procurement_Type)");
            strSql.Append(" values (");
            strSql.Append("@ID,@OrdID,@OrdHdID,@OrdType,@Address1,@Postal,@Tel,@Memo,@Created,@SalesOfficeCode,@SalesOfficeName,@Procurement_Type)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.VarChar,25),
					new SqlParameter("@OrdID", SqlDbType.VarChar,50),
					new SqlParameter("@OrdHdID", SqlDbType.VarChar,10),
					new SqlParameter("@OrdType", SqlDbType.VarChar,10),
					new SqlParameter("@Address1", SqlDbType.VarChar,100),
					new SqlParameter("@Postal", SqlDbType.VarChar,10),
					new SqlParameter("@Tel", SqlDbType.VarChar,20),
					new SqlParameter("@Memo", SqlDbType.VarChar,200),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@SalesOfficeCode", SqlDbType.VarChar,10),
					new SqlParameter("@SalesOfficeName", SqlDbType.VarChar,100),
					new SqlParameter("@Procurement_Type", SqlDbType.NVarChar,30)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrdID;
            parameters[2].Value = model.OrdHdID;
            parameters[3].Value = model.OrdType;
            parameters[4].Value = model.Address1;
            parameters[5].Value = model.Postal;
            parameters[6].Value = model.Tel;
            parameters[7].Value = model.Memo;
            parameters[8].Value = model.Created;
            parameters[9].Value = model.SalesOfficeCode;
            parameters[10].Value = model.SalesOfficeName;
            parameters[11].Value = model.Procurement_Type;
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        #endregion

        #region GetUnConfirmFiles

        public static DataSet GetUnConfirmFiles()
        {
            DataSet rs = null;
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            var strSql = @"select 
                            A1.ID,A1.OrdType,A1.OrdHdID,A1.OrdID,A1.F_Reject
                            from V_Jins_UnConfirm A1";
            rs = db.Query(strSql);
            return rs;
        }

        public static DataSet GetOrdDetail(string id)
        {
            DataSet rs = null;
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            var strSql = @"SELECT 
                            A1.ID,A1.SubID,A1.OPC,A1.Qty
                            FROM 
                            dbo.Jins_Ord_ST A1 with (nolock) 
                            where ID='" + id + @"'";
            rs = db.Query(strSql);
            return rs;
        }

        public static DataSet GetOrdShipDetail(string id)
        {
            DataSet rs = null;
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            var strSql = @"SELECT 
                            A1.ID,A1.SubID,A1.OPC,A1.QtyShip
                            FROM 
                            dbo.Jins_Ord_ST_Shipping2 A1 with (nolock) 
                            where ID='" + id + @"'";
            rs = db.Query(strSql);
            return rs;
        }

        public static void UpdateConfirmFlat(string ordid)
        {
            var strSql = @"update Jins_Conet
                            set F_Confirm=1
                            where BCode1='" + ordid + @"'";
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            db.ExecuteSql(strSql);
        }

        #endregion

        #region GetUnShippingFiles

        public static DataSet GetUnShippingFilesRX()
        {
            DataSet rs = null;
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            var strSql = @"select 
                            A1.ID,A1.OrdType,A1.OrdHdID,A1.OrdID,A1.ECode 
                            from V_Jins_UnShippingRX A1";
            rs = db.Query(strSql);
            return rs;
        }

        public static DataSet GetUnShippingFilesST()
        {
            DataSet rs = null;
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            var strSql = @"select 
                            A1.ID,A1.OrdType,A1.OrdHdID,A1.OrdID,A1.ECode  
                            from V_Jins_UnShippingST A1";
            rs = db.Query(strSql);
            return rs;
        }

        public static void UpdateShippingFlatRX(string ordid)
        {
            var strSql = @"update Jins_Conet
                            set F_Shipping=1
                            where BCode1='" + ordid + @"'";
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            db.ExecuteSql(strSql);
        }

        public static void UpdateShippingFlatST(OrdMain model)
        {
            var strSql = @"update Jins_Ord_ST_Shipping1
                            set F_Shipping=1
                            where ECode='" + model.ECode + @"' and OrdID='" + model.OrdID + @"'";
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            db.ExecuteSql(strSql);
        }

        #endregion

        #region Get Import shipping
        public static void GetImportShippingST()
        {
            var strSql = @"SP_JinsOrd_ImportUnShippingST";
            DbHelperSQL db = new DbHelperSQL(JinsPub.DbName);
            db.RunProcedure(strSql, new IDataParameter[] { });
        }

        #endregion
    }
}