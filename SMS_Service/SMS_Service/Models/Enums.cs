using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Service.Models
{
    public enum SIMTypes
    {
        SIM800 = 1,
        SIM900 = 2
    }
    public enum MStatusTypes
    {
        Pending = 0,
        Sending = 2,
        Failed = 3,
        Success = 1
    }
    public enum ActionTypes
    {
        Send = 1,
        Receive = 2
    }

    public enum SendingTypes
    {
        None = 0, // Do nothing if fail
        DynamicDeviceCNumber = 2, // Resend to another SIMDevice if fail
        StaticDeviceCNumber = 1 // Retry sending to the current device
    }

}
