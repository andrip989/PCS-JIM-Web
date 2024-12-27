using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace PCS_JIM_Web.Library
{
    /*
    Iwan Hartanto(10/25/2024 8:08:33 AM):
    smtp2go 
    Api Key : api-E1EEB39FCF88448989DAA6EA21466617
    https://api.smtp2go.com/v3/
    API Documentation: https://developers.smtp2go.com/
    Iwan Hartanto(10/25/2024 10:37:28 AM):
    sender : redz.hotel@bitubee.com
    */
    public class sysMail
    {
        string subject = "";
        string toaddress = "";
        string ccaddress = "";
        string bccaddress = "";
        string body = "";
        RestClient RC;
        string response = "";
        string sender = "";
        public sysMail() 
        {
            RC = new RestClient();
            RC.endPoint = "https://api.smtp2go.com/v3/email/send";
            RC.httpMethod = httpVerb.POST;
            RC.APIKeySmtp = "api-E1EEB39FCF88448989DAA6EA21466617";
            this.sender = sysConfig.APIemailsender();
        }

        public string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                this.subject = value;
            }
        }

        public string ToAddress
        {
            get
            {
                return this.toaddress;
            }
            set
            {
                this.toaddress = value;
            }
        }

        public string CCAddress
        {
            get
            {
                return this.ccaddress;
            }
            set
            {
                this.ccaddress = value;
            }
        }

        public string BCCAddress
        {
            get
            {
                return this.bccaddress;
            }
            set
            {
                this.bccaddress = value;
            }
        }

        public string Body
        {
            get
            {
                return this.body;
            }
            set
            {
                this.body = value;
            }
        }

        public string Response
        {
            get 
            {
                return this.response;
            }
        }

        public string convertmail(string address,string keyaddress)
        {
            string val = "";
            if (address.Contains(",") || address.Contains(";") || address.Contains(":"))
            {
                address = address.Replace(",", ";");
                address = address.Replace(":", ";");

                string[] charx = address.Split(new string[] { ";" }, StringSplitOptions.None);
                val = "\""+ keyaddress + "\": [\r\n    ";
                int loop = 0;
                foreach (string value in charx)
                {
                    if (loop > 0)
                        val += ",";
                    val += "\"" + value + "\"\r\n ";
                    loop++;
                }
                val += "],\r\n  ";

            }
            else if(address != "")
            {
                val = "\""+ keyaddress + "\": [\r\n    ";
                val += "\"" + address + "\"\r\n ";
                val += "],\r\n  ";
            }
            return val;
        }

        public void SendMail()
        {
            string _toaddress = this.convertmail(this.toaddress, "to");
            string _ccadddress = this.convertmail(this.ccaddress,"cc");
            string _bccaddress = this.convertmail(this.bccaddress,"bcc");

            RC.postJSON = "{\r\n  \"sender\": \""+ this.sender + "\",\r\n  " +
                            _toaddress +
                            _ccadddress +
                            _bccaddress + 
                            "\"subject\": \"" +this.subject+"\",\r\n  " +
                            "\"html_body\": \"" + this.body+"\" \r\n" +
                            "}\r\n";

            this.response = RC.makeRequest();

            RC = null;
        }
    }
}