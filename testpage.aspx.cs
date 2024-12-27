using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using CrystalDecisions.Shared;
using Google.Apis.PeopleService.v1.Data;
using Npgsql;
using PCS_JIM_Web.Library;
using Telerik.Charting.Styles;
using Telerik.Web.UI.com.hisoftware.api2;

namespace PCS_JIM_Web
{
    public partial class testpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var newContact = new Person
                {
                    Names = new List<Name> { new Name { GivenName = "Andri", FamilyName = "Andri" } },
                    EmailAddresses = new List<EmailAddress> { new EmailAddress { Value = "andrip@jimmaill.com" } },
                    PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Value = "+6287884808272" } }
                };

                GoogleContact.AddNewContact(newContact).Wait();
                
            }
            else
            {
                string parameter = Request["__EVENTARGUMENT"]; // parameter
                string value = Request["__EVENTTARGET"]; // Request["__EVENTTARGET"]; // btnSave

                if (parameter.Contains("create"))
                {
                    Thread.Sleep(5000);
                }
            }
        }
    
        private void action(PMSType pMSType)
        {
            CardKeyPMS obj = new CardKeyPMS("");
            obj.guestname = "andrip";
            obj.startDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
            obj.endDateTime = DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmm");
            obj.Room = "101";
            obj.PMSType = pMSType;
            obj.Run();
        }

        private void ExecuteBut(PMSType pMSType)
        {
            int tryCount = 3;

            if (tryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(tryCount));

            while (true)
            {
                try
                {
                    action(pMSType);
                    break; // success!
                }
                catch
                {
                    if (--tryCount == 0)
                        break;
                    Thread.Sleep(5000);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ExecuteBut(PMSType.Create);
        }
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            ExecuteBut(PMSType.Checkout);
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            ExecuteBut(PMSType.Check);
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ExecuteBut(PMSType.Clear);
        }
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            ExecuteBut(PMSType.Duplicate);
        }
    }
}