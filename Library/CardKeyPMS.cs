using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using Npgsql;

namespace PCS_JIM_Web.Library
{
    public enum PMSType
    {
        Create,
        Duplicate,
        Check,
        Checkout,
        Clear
    }

    public class CardKeyPMS
    {
        public string transaksiid;
        public string recid;
        public string resultlog;
        public PMSType PMStype;
        public string guestname;
        public string startDateTime;
        public string endDateTime;
        public string room;

        public string Room
        {
            get
            {
                return this.room;
            }
            set
            {
                this.room = value;
            }
        }

        public string EndDateTime
        {
            get
            {
                return this.endDateTime;
            }
            set
            {
                this.endDateTime = value;
            }
        }

        public string StartDateTime
        {
            get
            {
                return this.startDateTime;
            }
            set
            {
                this.startDateTime = value;
            }
        }

        public string Guestname
        {
            get
            {
                return this.guestname;
            }
            set
            {
                this.guestname = value;
            }
        }

        public CardKeyPMS(string _transaksiid,string _recid = "")
        {
            this.transaksiid = _transaksiid;
            this.recid = _recid;
            this.recid = "0";
        }

        public string Resultlog
        {
            get
            {
                return this.resultlog;
            }
        }

        public PMSType PMSType
        {
            get
            {
                return this.PMStype;
            }
            set
            {
                this.PMStype = value;
            }
        }

        public string Transaksiid
        {
            get
            {
                return this.transaksiid;
            }
            set
            {
                this.transaksiid = value;
            }
        }

        public string Recid
        {
            get
            {
                return this.recid;
            }
            set
            {
                this.recid = value;
            }
        }

        private string CreateKey(string name, string startDateTime, string endDateTime, string room) // untuk chekin
        {
            //((char)2).ToString() + "0000B|R101" + ((char)3).ToString();
            string ambil2digitroom = room.Substring(0, 1);
            ambil2digitroom += room.Substring(2, 1);

            return $"{(char)2}01{ambil2digitroom}I|R{room}|N{name}|D{endDateTime}|O{startDateTime}{(char)3}";
        }

        private string CreateKeyDuplicate(string name, string startDateTime, string endDateTime, string room) // untuk chekin
        {
            //((char)2).ToString() + "0000B|R101" + ((char)3).ToString();
            string ambil2digitroom = room.Substring(0, 1);
            ambil2digitroom += room.Substring(2, 1);

            return $"{(char)2}01{ambil2digitroom}G|R{room}|N{name}|D{endDateTime}|O{startDateTime}{(char)3}";
        }

        private string CheckoutKey(string room) // untuk checkout
        {
            string ambil2digitroom = room.Substring(0, 1);
            ambil2digitroom += room.Substring(2, 1);

            return $"{(char)2}{ambil2digitroom}00B|R{room}{(char)3}";
        }

        private string ClearKey(string room) // untuk checkout
        {
            string ambil2digitroom = room.Substring(0, 1);
            ambil2digitroom += room.Substring(2, 1);

            return $"{(char)2}98{ambil2digitroom}Q|R0{room}|K200{(char)3}";
        }

        private string CekKey(string room) // untuk checkout
        {
            return $"{(char)2}0{room}E{(char)3}";
        }

        public void LoadTrans(ref string guestname_, ref string startDateTime_, ref string endDateTime_, ref string room_)
        {
            string sqltax = " select * from transaksiroom t where " +
                        "(t.transaksiid = '" + this.transaksiid + "' or " +
                        "t.recid = " + this.recid + ")";

            sysConnection dbcon = new sysConnection();
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam(sqltax, null));
            DateTime arrival = DateTime.Now, departure = DateTime.Now;
            if (objreader.Read())
            {
                if (Convert.IsDBNull(objreader["arrival"]) == false)
                    arrival = Convert.ToDateTime(objreader["arrival"]);

                if (Convert.IsDBNull(objreader["departure"]) == false)
                    departure = Convert.ToDateTime(objreader["departure"]);

                if (Convert.IsDBNull(objreader["noroom"]) == false)
                    room_ = Convert.ToString(objreader["noroom"].ToString());

                if (Convert.IsDBNull(objreader["custcode"]) == false)
                    guestname_ = Convert.ToString(objreader["custcode"].ToString());

                startDateTime_ = arrival.ToString("yyyyMMddHHmm");//"2002 12 15 21 00" ;
                endDateTime_ = departure.ToString("yyyyMMddHHmm");//
            }

            dbcon.closeConnection();
        }

        public void Run()
        {
            if (this.transaksiid != "")
            {
                this.LoadTrans(ref this.guestname, ref this.startDateTime, ref this.endDateTime, ref this.room);
            }

            this.guestname = this.guestname.Replace(" ", ""); // buang spasi

            string value = "";

            switch (this.PMStype)
            {
                case PMSType.Create:
                    value = this.CreateKey(this.guestname, this.startDateTime, this.endDateTime, this.room);
                    break;
                case PMSType.Clear:
                    value = this.ClearKey(this.room);
                    break;
                case PMSType.Checkout:
                    value = this.CheckoutKey(this.room);
                    break;
                case PMSType.Check:
                    value = this.CekKey(this.room);
                    break;
                case PMSType.Duplicate:
                    value = this.CreateKeyDuplicate(this.guestname, this.startDateTime, this.endDateTime, this.room);
                    break;
            }


            TcpClient client = new TcpClient(sysConfig.PMSIPClient(), sysConfig.PMSIPPort());//"10.200.1.40", 10003);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(value);

            NetworkStream stream = client.GetStream();

            //stream.WriteByte(0x02);
            stream.Write(data, 0, data.Length);
            //stream.WriteByte(0x03);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            var result = response.Trim(new char[] { '\u0002', '\u0003' });

            bool keyWasSent = result.Contains("\"ack\":0");

            if (keyWasSent)
            {
                this.resultlog = result;
                client.Close();
            }
            else
            {
                this.resultlog = result;
                client.Close();
                throw new Exception(this.resultlog);
            }
            //masukin comment

        }
    }
}