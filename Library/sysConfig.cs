using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;

namespace PCS_JIM_Web.Library
{
    public class sysConfig
    {
        public static string DateFormat()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["dateformat"]);
        }
        public static string TimeFormat()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["timeformat"]);
        }
        public static string DateTimeFormat()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["datetimeformat"]);
        }
        public static string CurrencyFormat()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["currencyformat"]);
        }
        public static string IDFTitle()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["IDFTitle"]);
        }
        public static string IDFlokasi()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["LokasiOutlet"]);
        }
        
        public static string APIemailsender()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["apiemailsender"]);
        }

        public static string routerwifi()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["routerwifi"]);
        }

        public static string routerpassword()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["routerpassword"]);
        }
        public static string routeruser()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["routeruser"]);
        }
        public static string routerssidhotspot()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["routerssidhotspot"]);
        }
        
        public static string DBConnectionString()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);
        }

        public static string ReportODBCDSN()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["reportOdbcDSN"]);            
        }

        public static string ApiRdAX()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["apirdax"]);
        }

        public static string HouseKeepingMinutes()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["housekeepingminutes"]);            
        }

        public static string PMSIPClient()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["PMSIPClient"]);
        }
        public static int PMSIPPort()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["PMSIPPort"]);
        }

        public static string HostDB()
        {
            string val = "";
            string getparam = Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);

            foreach (string val_ in getparam.Split(';'))
            {
                var paramsCollection = HttpUtility.ParseQueryString(val_);

                foreach (var key in paramsCollection.AllKeys)
                {
                    if (key.ToLower() == "host")
                    {
                        val = paramsCollection[key];
                        break;
                    }
                }

                if (val != "")
                    break;
            }
            return val;
        }

        public static string PortDB()
        {
            string val = "";
            string getparam = Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);

            foreach (string val_ in getparam.Split(';'))
            {
                var paramsCollection = HttpUtility.ParseQueryString(val_);

                foreach (var key in paramsCollection.AllKeys)
                {
                    if (key.ToLower() == "port")
                    {
                        val = paramsCollection[key];
                        break;
                    }
                }

                if (val != "")
                    break;
            }
            return val;
        }
        public static string UsernameDB()
        {
            string val = "";
            string getparam = Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);

            foreach (string val_ in getparam.Split(';'))
            {
                var paramsCollection = HttpUtility.ParseQueryString(val_);

                foreach (var key in paramsCollection.AllKeys)
                {
                    if (key.ToLower() == "username")
                    {
                        val = paramsCollection[key];
                        break;
                    }
                }

                if (val != "")
                    break;
            }
            return val;
        }

        public static string PasswordDB()
        {
            string val = "";
            string getparam = Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);

            foreach (string val_ in getparam.Split(';'))
            {
                var paramsCollection = HttpUtility.ParseQueryString(val_);

                foreach (var key in paramsCollection.AllKeys)
                {
                    if (key.ToLower() == "password")
                    {
                        val = paramsCollection[key];
                        break;
                    }
                }

                if (val != "")
                    break;
            }
            return val;
        }

        public static string DatabaseDB()
        {
            string val = "";
            string getparam = Convert.ToString(ConfigurationManager.AppSettings["DBConnectionString"]);

            foreach (string val_ in getparam.Split(';'))
            {
                var paramsCollection = HttpUtility.ParseQueryString(val_);

                foreach (var key in paramsCollection.AllKeys)
                {
                    if (key.ToLower() == "database")
                    {
                        val = paramsCollection[key];
                        break;
                    }
                }

                if (val != "")
                    break;
            }
            return val;
        }
    }
}