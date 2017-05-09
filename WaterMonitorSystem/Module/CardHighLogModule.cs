using DBUtility;
using Maticsoft.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    //add by kqz 2017-5-9 17:14
    public class CardHighLogModule
    {
        public static long AddCardClearLog(CardHighLog cardClearLog)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CardHighLog(");
            strSql.Append("SerialNumber,LogUserId,LogUserName,LogAddress,LogTime,LogType,LogContent)");
            strSql.Append(" values (");
            strSql.Append("@SerialNumber,@LogUserId,@LogUserName,@LogAddress,@LogTime,@LogType,@LogContent)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@SerialNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@LogUserId", SqlDbType.BigInt,8),
					new SqlParameter("@LogUserName", SqlDbType.NVarChar,50),
					new SqlParameter("@LogAddress", SqlDbType.NVarChar,200),
					new SqlParameter("@LogTime", SqlDbType.DateTime),
					new SqlParameter("@LogType", SqlDbType.NVarChar,50),
					new SqlParameter("@LogContent", SqlDbType.NVarChar,-1)};
            parameters[0].Value = cardClearLog.SerialNumber;
            parameters[1].Value = cardClearLog.LogUserId;
            parameters[2].Value = cardClearLog.LogUserName;
            parameters[3].Value = cardClearLog.LogAddress;
            parameters[4].Value = cardClearLog.LogTime;
            parameters[5].Value = cardClearLog.LogType;
            parameters[6].Value = cardClearLog.LogContent;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

    }
}
