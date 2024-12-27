using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using MikrotikDotNet;
using Newtonsoft.Json.Linq;
using PCS_JIM_Web.Module;

namespace PCS_JIM_Web.Library
{
    public class sysCreateHotspot
    {
        string username = "";
        string password = "";
        int limituptime = 1;
        int devicecount = 4;
        string emailhotspot = "";
        public sysCreateHotspot()
        {

        }
        public string Emailhotspot
        {
            get
            {
                return this.emailhotspot;
            }
            set
            {
                this.emailhotspot = value;
            }
        }
        public int Devicecount
        {
            get
            {
                return this.devicecount;
            }
            set
            {
                this.devicecount = value;
            }
        }
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }
        public int Limituptime
        {
            get
            {
                return this.limituptime;
            }
            set
            {
                this.limituptime = value;
            }
        }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }
        public void Create()
        {
            if(this.password == "") //generate password
                this.Password = sysfunction.RandomStringSml(4) + sysfunction.RandomStringNum(4);

            try
            {
                var conn = new MKConnection(sysConfig.routerwifi(), sysConfig.routeruser(), sysConfig.routerpassword(), 8728);

                conn.Open();
                var cmd = conn.CreateCommand("ip hotspot user add");
                cmd.Parameters.Add("name", this.username);
                cmd.Parameters.Add("password", this.password);
                cmd.Parameters.Add("profile", this.devicecount.ToString() + "Dev");
                //cmd.Parameters.Add("limit-uptime", this.limituptime.ToString() + "d");
                cmd.ExecuteNonQuery();
                conn.Close();

                //ip hotspot user add name = user102 password = test123 profile = 4Dev limit-uptime = 1d
                //ip hotspot user remove user102

                sysZeptoMail objmail = new sysZeptoMail();
                objmail.ToAddress = this.emailhotspot;
                if (objmail.ToAddress != "andrip@jimmail.com,iwanh@jimmail.com,yanto@jimmail.com")
                {
                    objmail.BCCAddress = "andrip@jimmail.com,iwanh@jimmail.com,yanto@jimmail.com";
                }
                objmail.Subject = "Create Password Hotspot (" + this.username + ") - " + sysConfig.routerssidhotspot();

                string value = "<table border=0>";
                value += "<tr> <td>No. Room           </td><td> : </td> <td> " + this.username + "</td></tr>";
                value += "<tr> <td>SSID               </td><td> : </td> <td> <b>" + sysConfig.routerssidhotspot() + "</b></td></tr>";
                value += "<tr> <td>UserName Hotspot   </td><td> : </td> <td> <b>" + this.username + "</b></td></tr>";
                value += "<tr> <td>Password Hotspot   </td><td> : </td> <td> <b>" + this.password + "</b></td></tr>";
                value += "</table>";

                objmail.Body = value;

                objmail.SendMail();
            }
            catch (Exception ex)
            {
                sysZeptoMail objmail = new sysZeptoMail();
                objmail.ToAddress = "andrip@jimmail.com,iwanh@jimmail.com";
                objmail.Subject = "Error Create Password Hotspot (" + this.username + ") - " + sysConfig.routerssidhotspot();

                string value = "<table border=0>";
                value += "<tr> <td>No. Room           </td><td> : </td> <td> " + this.username + "</td></tr>";
                value += "<tr> <td>SSID               </td><td> : </td> <td> <b>" + sysConfig.routerssidhotspot() + "</b></td></tr>";
                value += "<tr> <td>UserName Hotspot   </td><td> : </td> <td> <b>" + this.username + "</b></td></tr>";
                value += "<tr> <td>Error Message      </td><td> : </td> <td> <b>" + ex.Message + "</b></td></tr>";
                value += "</table>";

                objmail.Body = value;

                objmail.SendMail();
            }
        }

        public void HapusUser(string _username)
        {
            if (_username == "")
                _username = this.username;

            try
            {
                var conn = new MKConnection(sysConfig.routerwifi(), sysConfig.routeruser(), sysConfig.routerpassword(), 8728);
                conn.Open();
                var cmd = conn.CreateCommand("ip hotspot user remove");
                cmd.Parameters.Add(".id", _username);
                cmd.ExecuteNonQuery();
                conn.Close();

                sysZeptoMail objmail = new sysZeptoMail();
                objmail.ToAddress = "andrip@jimmail.com,iwanh@jimmail.com";
                objmail.Subject = "Clear Password Hotspot (" + this.username + ") - " + sysConfig.routerssidhotspot();

                string value = "<table border=0>";
                value += "<tr> <td>No. Room           </td><td> : </td> <td> " + _username + "</td></tr>";
                value += "<tr> <td>SSID               </td><td> : </td> <td> <b>" + sysConfig.routerssidhotspot() + "</b></td></tr>";
                value += "<tr> <td>UserName Hotspot   </td><td> : </td> <td> <b>" + _username + "</b></td></tr>";
                value += "</table>";

                objmail.Body = value;

                objmail.SendMail();

            }
            catch (Exception ex)
            {
                sysZeptoMail objmail = new sysZeptoMail();
                objmail.ToAddress = "andrip@jimmail.com,iwanh@jimmail.com";
                objmail.Subject = "Error Clear Password Hotspot (" + this.username + ") - " + sysConfig.routerssidhotspot();

                string value = "<table border=0>";
                value += "<tr> <td>No. Room           </td><td> : </td> <td> " + _username + "</td></tr>";
                value += "<tr> <td>SSID               </td><td> : </td> <td> <b>" + sysConfig.routerssidhotspot() + "</b></td></tr>";
                value += "<tr> <td>UserName Hotspot   </td><td> : </td> <td> <b>" + _username + "</b></td></tr>";
                value += "<tr> <td>Error Message      </td><td> : </td> <td> <b>" + ex.Message + "</b></td></tr>";
                value += "</table>";

                objmail.Body = value;

                objmail.SendMail();

            }
        }
    }
}