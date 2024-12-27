using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Npgsql;
using Telerik.Pdf;

namespace PCS_JIM_Web.Library
{

    public class exportAX
    {
        public string transaksiid;
        public string recid;
        sysUserSession session;
        public Boolean autoposting;
        public exportAX(string _transaksiid, sysUserSession session_)
        {
            this.transaksiid = _transaksiid;
            this.session = session_;
            this.autoposting = false;
            this.recid = "0";
        }

        public Boolean Autoposting
        {
            get
            {
                return this.autoposting;
            }
            set
            {
                this.autoposting = value;
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

        public void CreateJournalAX()
        {
            sysConnection dbcon;
            dbcon = new sysConnection();
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select T1.*,S.* from transaksiroom T1 " +
                                                                   "LEFT JOIN setupguestlist s ON T1.custcode::text = s.custcode::text " +
                                                                   "where (T1.recid = " + this.recid + " " +
                                                                   " or T1.transaksiid = '"+this.transaksiid+"') " +
                                                                   "order by T1.arrival ", null));


            //export to AX API
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var baseAddress = sysConfig.ApiRdAX() + "LedgerJournal/Create";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            http.PreAuthenticate = true;

            List<Ledgerjournaltables> objLedgerJournalTable;
            LedgerResult objLedgerResult = new LedgerResult();

            objLedgerJournalTable = new List<Ledgerjournaltables>();

            Ledgerjournaltables Header = new Ledgerjournaltables();
            string transaksiidparam = "";
            string idrefbooking = "";
            float totalrate = 0;
            int isexport = 0;
            string tipetrans = "";
            string noroom = "";
            string bookingsource = "";
            if (objreader.Read())
            {
                transaksiidparam = objreader["transaksiid"].ToString();
                idrefbooking = objreader["refbookingcode"].ToString();
                tipetrans = objreader["tipetrans"].ToString();
                noroom = objreader["noroom"].ToString();
                bookingsource = objreader["bookingsource"].ToString();

                Header.transaksiid = transaksiidparam;
                Header.isAutoPosting = this.autoposting ? 1 : 0;
                Header.JournalName = tipetrans == "walkin" ? "SALES_WI" : "SALES_RD";
                Header.dataareaid = "r01";
                Header.RequestDate = Convert.ToDateTime(objreader["departure"]);
                Header.JisLedgerJournalType = 2;
                Header.JISPaymMode = 0;
                //Header.Name = string.Format("Billing {3}#{0} TRXID={1} Room={2}", idrefbooking, transaksiidparam, objreader["noroom"].ToString() , tipetrans);

                Header.Name = string.Format("Rsv#{0} Trx={1} Room={2} {3}", idrefbooking, transaksiidparam, noroom, bookingsource, tipetrans);

                Header.Dimension1 = "R01";
                Header.Dimension2 = "R01";
                Header.Dimension3 = "0";
                Header.JISVoucher = "";
                Header.JournalId = "";

                Header.JISTglGiro = Header.RequestDate;

                Header.LedgerJournalInclTax = tipetrans == "walkin" ? true : false;

                if (Convert.IsDBNull(objreader["totalrate"]) == false)
                    totalrate = (float)Convert.ToDecimal(objreader["totalrate"]);

                isexport = Convert.ToInt32(objreader["exportstatus"]);
            }

            objreader.Close();
            dbcon.closeConnection();

            List<Ledgerjournaltran> addline = new List<Ledgerjournaltran>();
            Ledgerjournaltran detail;
            /*
            objreader = dbcon.executeQuery(new sysSQLParam("select T1.* from transaksiroomratedetail T1 " +
                                                        "where T1.transaksiid = '" + transaksiidparam + "' " +
                                                        "order by T1.tanggal ", null));
            while (objreader.Read())
            */
            {
                detail = new Ledgerjournaltran();
                detail.AccountType = 1;
                detail.AccountNum = tipetrans == "walkin" ? "C00002" : "C00001";
                detail.Payment = "";
                detail.PostingProfile = "Gen";
                detail.Invoice = transaksiidparam;
                detail.Transdate = Header.RequestDate;
                detail.Duedate = detail.Transdate;
                detail.TaxGroup = "";
                detail.TaxItemGroup = "";
                detail.Txt = string.Format("Rsv#{0} Trx={1} Room={2} {3}", idrefbooking, transaksiidparam, noroom, bookingsource);//string.Format("Billing #{0} TRXID={1}", idrefbooking, transaksiidparam);
                detail.Dimension1 = Header.Dimension1;
                detail.Dimension2 = Header.Dimension2;
                detail.Dimension3 = Header.Dimension3;
                detail.AmountCurCredit = 0;
                detail.AmountCurDebit = totalrate;
                detail.CurrencyCode = "IDR";
                detail.ExchRate = (float)100.0000;
                detail.OffsetAccount = "";
                detail.OffsetAccountType = 0;
                addline.Add(detail);

                float PB1 = (float)Math.Round((totalrate - (totalrate / (float)1.21)), 0);
                detail = new Ledgerjournaltran();
                detail.AccountType = 0;
                detail.AccountNum = tipetrans == "walkin" ? "411010" : "113010";
                detail.Payment = "";
                detail.PostingProfile = "Gen";
                detail.Invoice = "";
                detail.Transdate = Header.RequestDate;
                detail.Duedate = detail.Transdate;
                
                detail.TaxGroup = tipetrans == "walkin" ? "SC+PB-1" : "";
                detail.TaxItemGroup = tipetrans == "walkin" ? "SC+PB-1" : "";

                detail.Txt = string.Format("Rsv#{0} Trx={1} Room={2} {3}", idrefbooking, transaksiidparam, noroom, bookingsource);//string.Format("Billing #{0} TRXID={1}", idrefbooking, transaksiidparam);
                detail.Dimension1 = Header.Dimension1;
                detail.Dimension2 = Header.Dimension2;
                detail.Dimension3 = Header.Dimension3;
                detail.AmountCurCredit = totalrate;
                detail.AmountCurDebit = 0;
                detail.CurrencyCode = "IDR";
                detail.ExchRate = (float)100.0000;
                detail.OffsetAccount = "";
                detail.OffsetAccountType = 0;
                addline.Add(detail);
             
            }

            Header.ledgerJournalTrans = addline.ToArray();
            objLedgerJournalTable.Add(Header);
            if (isexport == 0)
            {
                //JObject parsedContent = (JObject)JToken.FromObject(objLedgerJournalTable);
                ASCIIEncoding encoding = new ASCIIEncoding();
                //Byte[] bytes = encoding.GetBytes(parsedContent.ToString());
                Byte[] bytes = encoding.GetBytes(JsonConvert.SerializeObject(objLedgerJournalTable));


                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                LedgerResult[] obj = JsonConvert.DeserializeObject<LedgerResult[]>(content);
                objLedgerResult = obj[0];//JsonConvert.DeserializeObject<LedgerResult>(content);//content.Cast<LedgerResult>().ToList();

                if (objLedgerResult.Result)
                {
                    if (objLedgerResult.LedgerJournalTables.JournalId != "" && transaksiidparam != "")
                    {
                        SqlParameter[] empparam = new SqlParameter[4];

                        empparam[0] = new SqlParameter("@updatedby", this.session.UserId);

                        empparam[1] = new SqlParameter("@createddatetime", DateTime.Now);

                        empparam[2] = new SqlParameter("@transaksiid", transaksiidparam);
                        empparam[3] = new SqlParameter("@journalidax", objLedgerResult.LedgerJournalTables.JournalId);

                        string sql = "update transaksiroom set exportstatus = 1 " +
                                                            ",journalidax = @journalidax " +
                                                            ",updatedatetime = @createddatetime" +
                                                            ",updatedby = @updatedby ";
                        sql += " where transaksiid = @transaksiid ";

                        dbcon.executeNonQuery(new sysSQLParam(sql, empparam));
                        dbcon.closeConnection();

                    }
                }
            }
        }
    }
}