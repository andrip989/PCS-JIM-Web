using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Npgsql;

namespace PCS_JIM_Web.Library
{
    public class sysfunction
    {
        private static Random random = new Random();
        public static void setGridStyle(GridView Grid, string primarykey = "")
        {
            Grid.PageSize = Convert.ToInt16(ConfigurationManager.AppSettings["Grid_MaxRows"]);
            Grid.AllowPaging = true;
            //Grid.AllowSorting = true;
            Grid.PagerStyle.CssClass = "GridPager";
            Grid.PagerStyle.HorizontalAlign = HorizontalAlign.Left;
            Grid.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            Grid.PagerSettings.PageButtonCount = 5;
        }

        public static void setCurrencyStyle(TextBox obj)
        {
            obj.Attributes.Add("onkeyup", "formatCurrency(this,'');");
            obj.Attributes.Add("onblur", "formatCurrency(this,'blur');");

            obj.Text = string.Format(sysConfig.CurrencyFormat(), (decimal)0);
        }

        public static int GetColumnIndexByName(GridViewRow row, string columnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                        break;
                columnIndex++; // keep adding 1 while we don't have the correct name
            }
            return columnIndex;
        }

        public static string getNextSequenceRoom(string NoRoom)
        {
            string numseq = "";
            sysConnection dbcon;
            dbcon = new sysConnection();
            string format = Convert.ToString(dbcon.executeScalar(new sysSQLParam("select numbersequencetrans from vwroom " +
                                                                                           " where noroom = '" + NoRoom + "' ", null)));
            dbcon.closeConnection();

            int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from transaksiroom " +
                                                                                         " where noroom = '" + NoRoom + "' and date_part('year',arrival) = " + DateTime.Now.ToString("yyyy") + " ", null)));
            countdata++;
            numseq = format;

            numseq = numseq.Replace("YY", DateTime.Now.ToString("yy"));

            int nomorformat = Regex.Matches(numseq, Regex.Escape("#")).Count;

            string lengthnomor = Convert.ToString(countdata);

            numseq = numseq.Replace("#", "");
            numseq += string.Concat(Enumerable.Repeat("0", nomorformat - lengthnomor.Length)) + Convert.ToString(countdata) ;

            return numseq;
        }

        public static string getNextSequenceHouseKeeping(string NoRoom)
        {
            string numseq = "";
            sysConnection dbcon;
            dbcon = new sysConnection();
            string format = "HK"+NoRoom+"-YY-####";

            int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from housekeepingroom " +
                                                                                         " where noroom = '" + NoRoom + "' and date_part('year',arrival) = " + DateTime.Now.ToString("yyyy") + " ", null)));
            countdata++;
            numseq = format;

            numseq = numseq.Replace("YY", DateTime.Now.ToString("yy"));

            int nomorformat = Regex.Matches(numseq, Regex.Escape("#")).Count;

            string lengthnomor = Convert.ToString(countdata);

            numseq = numseq.Replace("#", "");
            numseq += string.Concat(Enumerable.Repeat("0", nomorformat - lengthnomor.Length)) + Convert.ToString(countdata);

            return numseq;
        }

        public static string getNextSequenceMaintenance(string NoRoom)
        {
            string numseq = "";
            sysConnection dbcon;
            dbcon = new sysConnection();
            string format = "MT" + NoRoom + "-YY-####";

            int countdata = Convert.ToInt32(dbcon.executeScalar(new sysSQLParam("select count(*) from maitenanceroom " +
                                                                                         " where noroom = '" + NoRoom + "' and date_part('year',arrival) = " + DateTime.Now.ToString("yyyy") + " ", null)));
            countdata++;
            numseq = format;

            numseq = numseq.Replace("YY", DateTime.Now.ToString("yy"));

            int nomorformat = Regex.Matches(numseq, Regex.Escape("#")).Count;

            string lengthnomor = Convert.ToString(countdata);

            numseq = numseq.Replace("#", "");
            numseq += string.Concat(Enumerable.Repeat("0", nomorformat - lengthnomor.Length)) + Convert.ToString(countdata);

            return numseq;
        }

        public static void setTimeDevice(string param,string ipaddress,string ipport,string deviceidparam)
        {
            string timezone = "0";

            RestClient rClient = new RestClient();

            rClient.endPoint = "http://" + ipaddress + ":" + ipport + "/" + "zeroconf/time";

            rClient.httpMethod = httpVerb.POST;
                
            rClient.postJSON = "{\r\n  \"deviceid\": \"" + deviceidparam + "\",\r\n  \"data\": {\r\n    \"timeZone\": " + timezone + ",\r\n    \"date\": \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ".000Z\"\r\n  }\r\n}";

            string strResponse = rClient.makeRequest();
            rClient = null;
                
        }

        public static void SetONOFFOutlet(string recidparam, string switchsaklar = "")
        {
            Boolean isexec = true;
            sysConnection dbcon;
            dbcon = new sysConnection();
            NpgsqlDataReader objreader = dbcon.executeQuery(new sysSQLParam("select * from vwRoom where recid = " + recidparam + " or noroom = '"+recidparam+"' ", null));
            if (objreader.Read())
            {
                string outletnoparam = objreader["outletno"].ToString();
                string deviceidparam = objreader["deviceid"].ToString();
                string subdeviceidparam = objreader["subdeviceid"].ToString();
                string ipaddress = objreader["ipaddress"].ToString();
                string ipport = objreader["ipport"].ToString();
                string noroomv = objreader["noroom"].ToString();

                RestClient rClient = new RestClient();
                rClient.endPoint = "http://" + ipaddress + ":" + ipport + "/" + "zeroconf/switches";

                rClient.httpMethod = httpVerb.POST;
                string triggersaklar = "on";
                if (switchsaklar == "on")
                {
                    triggersaklar = "off";
                }
                else
                {
                    sysfunction.setTimeDevice(noroomv, ipaddress, ipport, deviceidparam);
                }

                if (isexec)
                {
                    rClient.postJSON = "{\r\n  \"deviceid\": \"" + deviceidparam + "\",\r\n  \"data\": {\r\n    \"subDevId\": \"" + subdeviceidparam + "\",\r\n    \"switches\": [\r\n      {\r\n        \"switch\": \"" + triggersaklar + "\",\r\n        \"outlet\": " + outletnoparam + "\r\n      }\r\n    ]\r\n  }\r\n}";

                    string strResponse = rClient.makeRequest();
                    rClient = null;
                }
            }
            objreader.Close();
            dbcon.closeConnection();
        }

        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
        {
            using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
            {
                Size newSize = sysfunction.CalculateDimensions(oldImage.Size, targetSize);

                using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppRgb))
                {
                    newImage.SetResolution(oldImage.HorizontalResolution, oldImage.VerticalResolution);
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                        MemoryStream m = new MemoryStream();
                        newImage.Save(m, ImageFormat.Jpeg);
                        return m.GetBuffer();
                    }
                }

            }
        }

        private static Size CalculateDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            if (oldSize.Width > oldSize.Height)
            {
                newSize.Width = targetSize;
                newSize.Height = (int)(oldSize.Height * (float)targetSize / (float)oldSize.Width);
            }
            else
            {
                newSize.Width = (int)(oldSize.Width * (float)targetSize / (float)oldSize.Height);
                newSize.Height = targetSize;
            }
            return newSize;
        }

        public static DataTable BloodType(DropDownList obj)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Rows.Add(new object[] { "A +", "A +"});
            dt.Rows.Add(new object[] { "A -", "A -" });
            dt.Rows.Add(new object[] { "B +", "B +" });
            dt.Rows.Add(new object[] { "B -", "B -" });
            dt.Rows.Add(new object[] { "AB +", "AB +" });
            dt.Rows.Add(new object[] { "AB -", "AB -" });
            dt.Rows.Add(new object[] { "O +", "O +" });
            dt.Rows.Add(new object[] { "O -", "O -" });

            obj.DataTextField = "Description";
            obj.DataValueField = "Name";

            return dt;
        }

        public static DataTable GuestType(DropDownList obj)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Rows.Add(new object[] { "RedDoorz", "RedDoorz" });
            dt.Rows.Add(new object[] { "Non-RedDoorz", "Non-RedDoorz" });

            obj.DataTextField = "Description";
            obj.DataValueField = "Name";

            return dt;
        }

        public static DataTable IdentificationType(DropDownList obj)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Rows.Add(new object[] { "Driver License", "Driver License" });
            dt.Rows.Add(new object[] { "Passport", "Passport" });
            dt.Rows.Add(new object[] { "National Id", "National Id" });
            dt.Rows.Add(new object[] { "Employee Card", "Employee Card" });

            obj.DataTextField = "Description";
            obj.DataValueField = "Name";

            return dt;
        }

        public static DataTable PlanType(DropDownList obj)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Rows.Add(new object[] { 1, "of all night %" });
            dt.Rows.Add(new object[] { 2, "of first night %" });
            dt.Rows.Add(new object[] { 3, "fixed amount per night" });
            dt.Rows.Add(new object[] { 4, "fixed amount per stay" });

            obj.DataTextField = "Description";
            obj.DataValueField = "Name";

            return dt;
        }

        public static string RandomStringCap(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringNum(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringSml(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringSpe(int length)
        {
            const string chars = "!@#$%^&*_-=+";
            return new string(Enumerable.Repeat(chars, length)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}