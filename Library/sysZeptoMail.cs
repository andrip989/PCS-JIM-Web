using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace PCS_JIM_Web.Library
{
    public class sysZeptoMail
    {
        string subject = "";
        string toaddress = "";
        string ccaddress = "";
        string bccaddress = "";
        string body = "";
        string response = "";
        string sender = "";

        public sysZeptoMail()
        {           

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

        public string convertmail(string address, string keyaddress)
        {
            string val = "";
            if (address.Contains(",") || address.Contains(";") || address.Contains(":"))
            {
                address = address.Replace(",", ";");
                address = address.Replace(":", ";");

                string[] charx = address.Split(new string[] { ";" }, StringSplitOptions.None);
                val = "'" + keyaddress + "'" + ": [ ";
                int loop = 0;
                foreach (string value in charx)
                {
                    if (loop > 0)
                        val += ",";
                    val += " { 'email_address': { 'address': '" + value + "','name': 'Reddoor Customer'} }";
                    loop++;
                }
                val += " ], ";

            }
            else if (address != "")
            {
                val = " '"+ keyaddress + "' : [{ 'email_address': { 'address': '"+ address + "','name': 'Reddoor Customer'} }] , ";
            }
            return val;
        }

        public void SendMail()
        {
            try
            {
                string _toaddress = this.convertmail(this.toaddress, "to");
                string _ccadddress = this.convertmail(this.ccaddress, "cc");
                string _bccaddress = this.convertmail(this.bccaddress, "bcc");

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var baseAddress = "https://api.zeptomail.com/v1.1/email";

                var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";
                http.PreAuthenticate = true;
                http.Headers.Add("Authorization", "Zoho-enczapikey wSsVR61xrkb0Dap5lDKtcbxumlgGU1r/Rk51jgOj7n/6Ta3Bosc+lkzMDA+jTfQaQzNqFjsSp+ovkR0G0mFdit8onA0GCSiF9mqRe1U4J3x17qnvhDzCVm5bkBSML4wPzg5vnWhhEc5u");
                JObject parsedContent = JObject.Parse("{'from': { 'address': '" + sysConfig.APIemailsender() + "' } ," +
                                                        _toaddress +
                                                        _ccadddress +
                                                        _bccaddress +
                                                        "'subject':'" + this.subject + "'" +
                                                        ",'htmlbody':'" + this.body + "'}");
                //Console.WriteLine(parsedContent.ToString());
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
            }
            catch {
                sysMail obj = new sysMail();
                obj.ToAddress = this.toaddress;
                obj.CCAddress = this.ccaddress;
                obj.BCCAddress = this.bccaddress;
                obj.Subject = this.subject;
                obj.Body = this.body;
                obj.SendMail();
            }
        }
    }
}