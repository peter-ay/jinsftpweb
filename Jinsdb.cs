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

        public static void AddOrd(OrdMain model)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(JinsPub.DbName);
            List<DbHelperCmdObject> cmdObjec = new List<DbHelperCmdObject>();
            DbHelperCmdObject obj = null;
            //
            model.ID = JinsPub.ID25;
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
            obj = PrePareOrdSaveVerify(model.ID);
            cmdObjec.Add(obj);
            dbHelper.ExecuteCmdObjectTran(cmdObjec);
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
            strSql.Append("ID,SpecialRight1,SpecialRight2,SpecialRight3,SpecialRight4,SpecialLeft1,SpecialLeft2,SpecialLeft3,SpecialLeft4,Lens_Type,Tint_Type,Tint,Polarized,Mirrored,Oculus_Dexter_Sphere,Oculus_Sinister_Sphere,Oculus_Dexter_Cylinder,Oculus_Sinister_Cylinder,Oculus_Dexter_Axis,Oculus_Sinister_Axis,Oculus_Dexter_Add,Oculus_Sinister_Add,Oculus_Dexter_Diameter,Oculus_Sinister_Diameter,Oculus_Dexter_Quantity,Oculus_Sinister_Quantity)");
            strSql.Append(" values (");
            strSql.Append("@ID,@SpecialRight1,@SpecialRight2,@SpecialRight3,@SpecialRight4,@SpecialLeft1,@SpecialLeft2,@SpecialLeft3,@SpecialLeft4,@Lens_Type,@Tint_Type,@Tint,@Polarized,@Mirrored,@Oculus_Dexter_Sphere,@Oculus_Sinister_Sphere,@Oculus_Dexter_Cylinder,@Oculus_Sinister_Cylinder,@Oculus_Dexter_Axis,@Oculus_Sinister_Axis,@Oculus_Dexter_Add,@Oculus_Sinister_Add,@Oculus_Dexter_Diameter,@Oculus_Sinister_Diameter,@Oculus_Dexter_Quantity,@Oculus_Sinister_Quantity)");
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
					new SqlParameter("@Oculus_Sinister_Quantity", SqlDbType.Int,4)};
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
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdMain(OrdMain model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Ord_Main(");
            strSql.Append("ID,OrdID,OrdHdID,OrdType,Address1,Postal,Tel,Memo,Created,SalesOfficeCode,SalesOfficeName)");
            strSql.Append(" values (");
            strSql.Append("@ID,@OrdID,@OrdHdID,@OrdType,@Address1,@Postal,@Tel,@Memo,@Created,@SalesOfficeCode,@SalesOfficeName)");
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
					new SqlParameter("@SalesOfficeName", SqlDbType.VarChar,100)};
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
            //
            DbHelperCmdObject obj = new DbHelperCmdObject(strSql.ToString(), parameters);
            return obj;
        }

    }
}