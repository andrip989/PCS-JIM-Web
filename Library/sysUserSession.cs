using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCS_JIM_Web.Library
{
    public class sysUserSession
    {
        string userid, email;
        DateTime lastlogindatetime;
        string password;
        string fullname;
        string usergroup;
        int statuscashier;
        public sysUserSession(string _userid, string _email,
                                DateTime _lastlogindatetime,string _fullname , string _password,string _usergroup,int _statuscashier) 
        {
            this.userid = _userid;
            this.email = _email;

            this.lastlogindatetime = _lastlogindatetime;
            this.fullname = _fullname;
            this.password = _password;
            this.usergroup = _usergroup;
            this.statuscashier = _statuscashier;
        }
        public int Statuscashier
        {
            get
            {
                return this.statuscashier;
            }
            set
            {
                this.statuscashier = value;
            }
        }

        public string Usergroupid
        {
            get
            {
                return this.usergroup;
            }
            set
            {
                this.usergroup = value;
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

        public string Fullname
        {
            get
            {
                return this.fullname;
            }
            set
            {
                this.fullname = value;
            }
        }

        public string UserId
        {
            get
            {
                return this.userid;
            }
            set
            {
                this.userid = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }

        public DateTime Lastlogindatetime
        {
            get
            {
                return this.lastlogindatetime;
            }
            set
            {
                this.lastlogindatetime = value;
            }
        }

    }
}