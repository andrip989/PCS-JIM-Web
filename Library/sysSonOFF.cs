using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace PCS_JIM_Web.Library
{
    public class ResultsysSonOFF
    {
        public int seq { get; set; }
        public int error { get; set; }
        public sysSonOFFdata data { get; set; }

    }

    public class ResultsysSonOFFSub
    {
        public int seq { get; set; }
        public int error { get; set; }
        public DataSonOFF data { get; set; }

    }

    public class ResultsysSonOFFAllState
    {
        public int seq { get; set; }
        public int error { get; set; }
        public sysSonOFFdataSub data { get; set; }

    }

    public class sysSonOFF
    {
        public string deviceid { get; set; }
        public sysSonOFFdataSub data { get; set; }
    }
    public class sysSonOFFdata
    {
        public string deviceid { get; set; }
    }

    public class sysSonOFFdataSub
    {
        public string subDevId { get; set; }
        public int type { get; set; }
        public sysSonOFFswitches[] switches { get; set; }
    }

    public class sysSonOFFswitches
    {
        public string Switch { get; set; }
        public int outlet { get; set; }
    }

    public class DataSonOFF
    {
        public sysSonOFFdataSub[] subDevList { get; set; }
    }




    public class ResultgetState
    {
        public int seq { get; set; }
        public int error { get; set; }
        public DataSwitches data { get; set; }
    }

    public class DataSwitches
    {
        public Switchoutlet[] switches { get; set; }
        public string fwVersion { get; set; }
        public Faultstate faultState { get; set; }
        public Threshold threshold { get; set; }
        public Overload_00 overload_00 { get; set; }
        public Overload_01 overload_01 { get; set; }
        public Overload_02 overload_02 { get; set; }
        public Overload_03 overload_03 { get; set; }
        public Configure[] configure { get; set; }
    }

    public class Faultstate
    {
        public int subDevCom { get; set; }
        public int[] cse7761Com { get; set; }
    }

    public class Threshold
    {
        public Actpow actPow { get; set; }
        public Voltage voltage { get; set; }
        public Current current { get; set; }
    }

    public class Actpow
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class Voltage
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class Current
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class Overload_00
    {
        public Minap minAP { get; set; }
        public Maxap maxAP { get; set; }
        public Minv minV { get; set; }
        public Maxv maxV { get; set; }
        public Maxc maxC { get; set; }
        public int delayTime { get; set; }
    }

    public class Minap
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxap
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Minv
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxv
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxc
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Overload_01
    {
        public Minap1 minAP { get; set; }
        public Maxap1 maxAP { get; set; }
        public Minv1 minV { get; set; }
        public Maxv1 maxV { get; set; }
        public Maxc1 maxC { get; set; }
        public int delayTime { get; set; }
    }

    public class Minap1
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxap1
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Minv1
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxv1
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxc1
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Overload_02
    {
        public Minap2 minAP { get; set; }
        public Maxap2 maxAP { get; set; }
        public Minv2 minV { get; set; }
        public Maxv2 maxV { get; set; }
        public Maxc2 maxC { get; set; }
        public int delayTime { get; set; }
    }

    public class Minap2
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxap2
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Minv2
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxv2
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxc2
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Overload_03
    {
        public Minap3 minAP { get; set; }
        public Maxap3 maxAP { get; set; }
        public Minv3 minV { get; set; }
        public Maxv3 maxV { get; set; }
        public Maxc3 maxC { get; set; }
        public int delayTime { get; set; }
    }

    public class Minap3
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxap3
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Minv3
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxv3
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Maxc3
    {
        public int en { get; set; }
        public int val { get; set; }
    }

    public class Switchoutlet
    {
        public string Switch { get; set; }
        public int outlet { get; set; }
    }

    public class Configure
    {
        public int outlet { get; set; }
        public string startup { get; set; }
    }



}