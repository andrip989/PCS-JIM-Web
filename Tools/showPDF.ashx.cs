using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Web;

namespace PCS_JIM_Web.Tools
{
    /// <summary>
    /// Summary description for showPDF
    /// </summary>
    public class showPDF : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string url = "";
            if (context.Request.QueryString["urlparam"] != null)
                url = context.Request.QueryString["urlparam"];
            else
                url = context.Request.PhysicalApplicationPath + "/Temporary/Download/";

            if (context.Request.QueryString["userid"] != null)
            {
                url += context.Request.QueryString["userid"];
            }
            else
                throw new ArgumentException("No parameter specified");

            FileStream strm = new FileStream(url, FileMode.Open);
            int filesize = (int)strm.Length;
            byte[] buffer = new byte[strm.Length];
            int byteSeq = strm.Read(buffer, 0, filesize);
            strm.Close();

            context.Response.ContentType = "application/pdf";


            //context.Response.AddHeader("Content-Disposition", "attachment");            
            //context.Response.TransmitFile(url);
            //context.Response.Flush();
            //context.Response.Clear();
            //context.ApplicationInstance.CompleteRequest();

            //while (byteSeq > 0)
            {
                context.Response.OutputStream.Write(buffer, 0, byteSeq);
                //byteSeq = strm.Read(buffer, 0, filesize);
            }
            context.Response.Flush();
            context.Response.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}