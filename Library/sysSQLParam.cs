using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PCS_JIM_Web.Library
{
    public class sysSQLParam
    {
        string sql;
        SqlParameter[] sqlparam;

        public sysSQLParam(string _sql, SqlParameter[] _sqlParam)
        {
            this.sql = _sql;
            this.sqlparam = _sqlParam;
        }

        public string SQLQuery
        {
            get
            {
                return sql;
            }
            set
            {
                sql = value;
            }
        }

        public SqlParameter[] SQLParam
        {
            get
            {
                return sqlparam;
            }
        }
    }
}