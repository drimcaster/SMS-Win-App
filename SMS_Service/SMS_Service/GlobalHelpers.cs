using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS_Service.Models;



namespace SMS_Service
{
    public class GlobalHelpers
    {
        public static List<SIMDeviceModel> RegisteredSIMDeviceList { get; set; }
        public static IEnumerable<SIMDeviceModel> InActiveSIMDeviceList { get { return RegisteredSIMDeviceList.Where(device => device.IsReconnecting || device.ErrorCount >= 10); } }

        public static IEnumerable<SIMDeviceModel> ActiveSIMDeviceList { get { return RegisteredSIMDeviceList.Where(device => device.ErrorCount < 10 && device.IsReconnecting == false); } }



        public const int MaxResend = 3;


        public static readonly string[] MessagePrefix = { "+CMTI: \"SM\",", "+CMTI: \"ME\"," };

    }
    
}
