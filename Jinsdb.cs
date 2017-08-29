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
        private static string dbName = "HKOERPCONNECTION";
        public static void AddLog(string msg)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(dbName);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Log(");
            strSql.Append("ID,LogDate,Msg)");
            strSql.Append(" values (");
            strSql.Append("(select ERP_Management.dbo.SF_GetID(25)),getdate(),@Msg)");
            SqlParameter[] parameters = {
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
            parameters[0].Value = msg;
            dbHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public static void AddOrd(OrdMain model)
        {
            DbHelperSQL dbHelper = new DbHelperSQL(dbName);
            List<DbHelperCmdObject> cmdObjec = new List<DbHelperCmdObject>();
            DbHelperCmdObject obj = PrePareOrdMain(model);
            cmdObjec.Add(obj);
            if (model.Order_Type.ToLower() == "rx")
            {
                obj = PrePareOrdRX(model.SubRX);
                cmdObjec.Add(obj);
            }
            else
            {
                foreach (var item in model.SubST)
                {
                    obj = PrePareOrdST(item);
                    cmdObjec.Add(obj);
                }
            }
            obj = PrePareOrdSaveVerify(model.Order_Type);
            cmdObjec.Add(obj);
            dbHelper.ExecuteCmdObjectTran(cmdObjec);
        }

        private static DbHelperCmdObject PrePareOrdSaveVerify(string id)
        {
            DbHelperCmdObject obj = new DbHelperCmdObject();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SP_JinsOrd_SaveVerify");
            SqlParameter[] parameters ={
					new SqlParameter("@ID", SqlDbType.VarChar,50)};
            parameters[0].Value = id;
            //
            obj.CmdCommandType = CommandType.StoredProcedure;
            obj.StrSQL = strSql.ToString();
            obj.Parameters = parameters;
            //
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdST(OrdST item)
        {
            DbHelperCmdObject obj = new DbHelperCmdObject();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Log(");
            strSql.Append("ID,LogDate,Msg)");
            strSql.Append(" values (");
            strSql.Append("(select ERP_Management.dbo.SF_GetID(25)),getdate(),@Msg)");
            SqlParameter[] parameters ={
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
            parameters[0].Value = "";
            //
            obj.StrSQL = strSql.ToString();
            obj.Parameters = parameters;
            //
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdRX(OrdRX model)
        {
            DbHelperCmdObject obj = new DbHelperCmdObject();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Log(");
            strSql.Append("ID,LogDate,Msg)");
            strSql.Append(" values (");
            strSql.Append("(select ERP_Management.dbo.SF_GetID(25)),getdate(),@Msg)");
            SqlParameter[] parameters ={
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
            parameters[0].Value = "";
            //
            obj.StrSQL = strSql.ToString();
            obj.Parameters = parameters;
            //
            return obj;
        }

        private static DbHelperCmdObject PrePareOrdMain(OrdMain model)
        {
            DbHelperCmdObject obj = new DbHelperCmdObject();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Jins_Log(");
            strSql.Append("ID,LogDate,Msg)");
            strSql.Append(" values (");
            strSql.Append("(select ERP_Management.dbo.SF_GetID(25)),getdate(),@Msg)");
            SqlParameter[] parameters ={
					new SqlParameter("@Msg", SqlDbType.VarChar,2000)};
            parameters[0].Value = "";
            //
            obj.StrSQL = strSql.ToString();
            obj.Parameters = parameters;
            //
            return obj;
        }

    }
}