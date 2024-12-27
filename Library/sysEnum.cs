using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PCS_JIM_Web.Library
{
    public enum PaymentType
    {
        Cash,
        CreditCard,
        MarketPlace,
        CreditLimit
    }
    public enum CreditCardType
    {
        Visa,
        Mastercard,
        Discover,
        AmericanExpress,
        ATMCard
    }

    public enum StatusBlock
    {
        Open,
        CreditBlocked,
        BlockedAll
    }

    public enum MarketPlace
    {
        Domestic,
        EastAfrica,
        SouthAfrica,
        Walkin,
        Taxi,
        TravelAgent
    }

    public enum GenderType
    {
        Male,
        Female
    }

    public enum MaritalStatus
    {
        Married,
        Single,
        Divorced
    }

    public enum Followup
    {
        Email,
        Fax,
        HomePhone,
        Mail,
        Mobile,
        OfficePhone,
        Phonecall
    }
    public enum HeardFrom
    {
        Brochure,
        Email,
        Website,
        Radio,
        Advertise,
        Newspaper,
        Magazine,
        TravelGuide,
        PastGuest,
        Other
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
         
    public class LedgerResult
    {
        public Ledgerjournaltables LedgerJournalTables { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
    }

    public class Ledgerjournaltables
    {
        public string JournalId { get; set; }
        public string JournalName { get; set; }
        public DateTime RequestDate { get; set; }
        public int JISPaymMode { get; set; }
        public int JisLedgerJournalType { get; set; }
        public string Name { get; set; }
        public DateTime JISTglGiro { get; set; }
        public string JISVoucher { get; set; }
        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public string Dimension3 { get; set; }
        public string dataareaid { get; set; }
        public Ledgerjournaltran[] ledgerJournalTrans { get; set; }
        public int isAutoPosting { get; set; }
        public bool LedgerJournalInclTax { get; set; }
        public string transaksiid { get; set; }
    }

    public class Ledgerjournaltran
    {
        public object JournalId { get; set; }
        public object Voucher { get; set; }
        public int LineNum { get; set; }
        public int AccountType { get; set; }
        public string AccountNum { get; set; }
        public string Invoice { get; set; }
        public string Payment { get; set; }
        public string PostingProfile { get; set; }
        public string TaxGroup { get; set; }
        public DateTime Transdate { get; set; }
        public DateTime Duedate { get; set; }
        public string Txt { get; set; }
        public float AmountCurCredit { get; set; }
        public float AmountCurDebit { get; set; }
        public string CurrencyCode { get; set; }
        public float ExchRate { get; set; }
        public int OffsetAccountType { get; set; }
        public string OffsetAccount { get; set; }
        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public string Dimension3 { get; set; }
        public string TaxItemGroup { get; set; }
    }

}