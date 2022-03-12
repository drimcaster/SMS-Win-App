using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Service.Models
{
    public class ReceivedMessagesModel
    {
        public uint id { get; set; }

        //public string app_name { get; set; }
        //public int status_id { get; set; }
        public string sender_no { get; set; }
        public string receiver_no { get; set; }
        public string message { get; set; }
        //public int focus_id { get; set; }
        public string received_app_at { get; set; }
        public string received_mobile_at { get; set; }

    }
}
