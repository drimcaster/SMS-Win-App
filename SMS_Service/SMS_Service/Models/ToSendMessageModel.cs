using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Service.Models
{
    public class ToSendMessageModel
    {
        public uint id { get; set; }
        public string app_name { get; set; }
        public string app_id { get; set; }
        public int status_id { get; set; }
        public int sending_type_id { get; set; }
        public string received_app_at { get; set; }
        public string sent_mobile_at { get; set; }
        public string sender_no { get; set; }
        public string receiver_no { get; set; }
        public string message { get; set; }
        public int rank_priority { get; set; }
        

    }
}
