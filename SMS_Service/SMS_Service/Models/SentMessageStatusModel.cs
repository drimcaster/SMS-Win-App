using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Service.Models
{
    public class SentMessageStatusModel
    {
        public uint id { get; set; }
        public int status_id { get; set; }
        public string sent_mobile_at { get; set; }
        public string sender_no { get; set; }
       
    }
}
