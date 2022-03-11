using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMS_Service.Models
{

    public class SMSDataModel
    {

        public SMSDataModel()
        {

        }
        public SMSDataModel(Models.SIMDeviceModel device_model, string _receiverNo, string message, SendingTypes sendingtype = SendingTypes.None )
        {
            ReadRef = "N/A";
            DataID = 0;
            DevicePortName = device_model.ComputerPort.PortName;
            DeviceCNumber = device_model.ContactNumber;
            ActionType =  ActionTypes.Send;
            _mstatus =  MStatusTypes.Pending;
            _sendernumber = device_model.ContactNumber;
            ReceiverCNumber = _receiverNo;
            // _sent_receive_datetime
            _sent_receive_datetime = DateTime.Now.ToShortDateString();
            _message = message;
            _sendingtype = sendingtype;
        }
        public void SetMStatus(MStatusTypes mstatus)
        {
            _mstatus = mstatus;
            //if (mstatus == MStatusTypes.Sending)
               // Sending = true;
            //else
                //Sending = false;
            //ActionType = action;
        }
        public void SetSendingType(SendingTypes sendingtype)
        {
            _sendingtype = sendingtype;
        }

        public bool Sending { get; set; }
        public int RankPriority { get; set; } //The higher the number the more priority.
        public string ReadRef { get; set; }
        public int DataID { get; set; }
        public string DevicePortName { get; set; }
        public string DeviceCNumber { get; set; }

        public List<string> DynamicFailedNumberSends { get; set; } = new List<string>();

        public int SendFailedCount { get; set; } = 0;
        public DateTime SendLastAttempt { get; set; } = DateTime.Now;

        private SendingTypes _sendingtype = SendingTypes.None;
        public SendingTypes SendingType => _sendingtype;
        public ActionTypes ActionType { get; set; }

        private MStatusTypes _mstatus = MStatusTypes.Pending; //Success, Failed, Pending
        public MStatusTypes MStatus => _mstatus;

        private string _sendernumber;
        public string SenderCNumber => _sendernumber;
        public void SetSenderCNumber(string sendernumber)
        {
            _sendernumber = sendernumber;
        }
        public string ReceiverCNumber { get; set; }

        private string _sent_receive_datetime;
        public string ActionDateTime => _sent_receive_datetime;

        private string _message = "";
        public string Message => _message;

        //public List<string> DataFragments { get; set; }

        public string _rawData ;
        public string RecieveRawData { get { return _rawData; } set { ActionType = ActionTypes.Receive; _mstatus = MStatusTypes.Success; _rawData = value; ProcessRawData(); } }

        
        private void ProcessRawData()
        {
            if (RecieveRawData == null) return;
            string[] ss = RecieveRawData.Split('\n').Select(s => s.Replace("\r", "")).ToArray();
            if(ss.Length > 3)
            {

            }
            //0 AT+CMGR=76
            //1 +CMGR: "REC UNREAD","+639127994666","","22/03/05,15:29:53+32"
            //2 HELLO
            //3 
            //4 OK
            //5
            string _cmgr = "[" + ss[1].Substring(6) + "]";

            string[] s1 = JsonConvert.DeserializeObject<string[]>(_cmgr);

            
            ///////////////// HEAD////////////////////////
            //S1
            // 0 - status
            //_mstatus = s1[0] == ;
            // 1 - cnumber
            _sendernumber = s1[1];
            // 2 - type/title
            // 3 - datetime
            _sent_receive_datetime = s1[3];

            ///////////// BODY /////////////////////////////

            
            ////////////END CLEARING//////////////////////////
            //this is to be removed if has okay
            int i = ss.Length;
            int _final_detected = ss.Length - 1;
            if(i > 2)
            {
                if (ss[i - 2] == "OK")
                {
                    _final_detected = i - 2;
                    ss[i - 2] = "";
                }
                if (ss[i - 1] == "OK")
                {
                    _final_detected = i - 1;
                    ss[i - 1] = "";
                }

                if(ss[_final_detected - 1] == "")
                {
                    //getting the final end text
                    _final_detected = _final_detected - 1;
                }

            }

            for( int _count = 2; _count < _final_detected; _count++)
            {
                _message += (ss[_count] );
                if (_count < _final_detected - 1)
                    _message += "\r\n";
            }

        }

        //public List<string> _dataFragments;
        //public List<string> RawDataFragments { get { return _dataFragments; } }


    }
}
