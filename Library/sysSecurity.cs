using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Npgsql;

namespace PCS_JIM_Web.Library
{
    public class sysSecurity
    {
        public static void checkUserSession(ref sysUserSession session, HttpServerUtility Server, string _sessionId)
        {
            try
            {
                if (_sessionId != "")
                {
                    sysConnection dbcon = new sysConnection();

                    string username = Convert.ToString(dbcon.executeScalar(new sysSQLParam("select username from syssession s where s.sessionid = '"+ _sessionId +"' limit 1;", null)));

                    NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from sysuser where username = '" + username + "' ", null));
                    if (objreader.Read())
                    {
                        session = new sysUserSession(objreader["username"].ToString()
                                                    , objreader["email"].ToString()
                                                    , Convert.ToDateTime(objreader["lastlogindatetime"])
                                                    , objreader["fullname"].ToString()
                                                    , objreader["password"].ToString()
                                                    , objreader["usergroupid"].ToString()
                                                    , Convert.ToInt16(objreader["opencashier"]));
                    }
                    else
                        Server.Transfer("~/Module/signin.aspx");

                    dbcon.closeConnection();

                    if (session.UserId.Equals(""))
                        Server.Transfer("~/Module/signin.aspx");
                }
                else
                    Server.Transfer("~/Module/signin.aspx");
            }
            catch
            {
                if (_sessionId != "")
                    Server.Transfer("~/Module/signin.aspx");

            }


        }
        
    }
}