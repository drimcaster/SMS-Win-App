using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;


namespace SMS_Service.Models
{
    public class SIMDeviceModel
    {

        public delegate void DEVICENotification(SIMDeviceModel device,  bool success, string message, SerialDataReceivedEventArgs data_received = null, SerialErrorReceivedEventArgs error_received = null);

        private SerialPort _serialPort;
        private ComputerPortModel _computerPortModel;

        private bool isReceive { get; set; } = false;
        //public SMSDataModel SendSMS { get;set;}
        //public SMSDataModel ReceiveSMS { get; set; }
        public DEVICENotification DeviceNotify { get; set; }
        
        public ComputerPortModel ComputerPort
        {
            get { return _computerPortModel; }
            set
            {
               // Serial.DataReceived += Serial_DataReceived;
                _computerPortModel = value;
                if (_computerPortModel != null)
                {
                    if(this.Serial != null)
                    {
                        this.Serial.DataReceived += Serial_DataReceived1;
                        this.Serial.ErrorReceived += Serial_ErrorReceived;
                    }
                }
            }
        }

        public string StateInfo
        {
            get {
                if (_isSending)
                    return "The device is currently sending";
                else if (ErrorCount >= 10)
                    return "The device is currently failing";
                return "Is available.";
            }
        }

        public int ErrorCount { get; set; } = 0;

        private bool _isSending = false;
        public bool IsSending => _isSending ;

        public bool ReadyToSend { get; set; } = true;// false;
        

        public bool IsBusy => (_isSending || IsReconnecting || ErrorCount >= 10);
        public void SetSending(bool issending)
        {
            _isSending = issending;
        }
        private void Serial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            DeviceNotify?.Invoke(this, false, (this.Serial.ReadExisting() ?? "").Trim() ,  null, e);
        }

        private void Serial_DataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            DeviceNotify?.Invoke(this, true, (this.Serial.ReadExisting() ?? "").Trim(), e, null);
        }

        public string SIMModel { get; set; }// SIMTypes.SIM800;
        public int BaudRate { get; set; } = 9600;

        public string ContactNumber { get; set; }
        public string Network { get; set; }



        
        public string PortDescription
        {
            get
            {
                
                return ComputerPort?.Description ?? "";
            }
        }
        public List<SMSDataModel> SMSData { get; set; } = new List<SMSDataModel>();
        public SMSDataModel CurrentSMSData { get; set; }
        //public bool IsReadData { get;set; }
         

        public SerialPort Serial
        {
            get
            {
                if(ComputerPort == null || ComputerPort?.PortName == null)
                {
                    return null;
                }

                if (_serialPort == null)
                    _serialPort = new SerialPort(ComputerPort.PortName, BaudRate);

                return _serialPort;
            }

        }

        public DateTime LastReconnectAt { get; set; } = DateTime.Now;
        public bool IsReconnecting { get; set; } = false; 
        public bool ReconnectDevice()
        {
            if (IsReconnecting)
                return false;

            LastReconnectAt = DateTime.Now;
            IsReconnecting = true;
            if (ErrorCount <= 10)
                return false;

            //CLOSING Device
            if (Serial.IsOpen)
            {
                IsReconnecting = false;
                Serial.Close();
            }

            /**
             * CONNECT TO THE PORT
             */
            SerialPort _SPort = new SerialPort(ComputerPort.PortName, BaudRate);
            try
            {
                _SPort.Open();
            }
            catch
            {
                IsReconnecting = false;
                return false;
            }
            _SPort.WriteLine("AT");
            string result = Helpers.ResultHelper.getPortResult(_SPort, 200, this);
            if (result != "OK")
            {
                IsReconnecting = false;
                _SPort.Close();
                return false;
            }

            /*GETTING THE DEVICE MODEL*/
            _SPort.WriteLine("AT+CGMM");
            result  = Helpers.ResultHelper.getPortResults(_SPort, 300, this).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 6) == "SIMCOM").ToArray().FirstOrDefault<string>() ?? "";
            if (result != "" && result != null && result != "ERROR")
                SIMModel = result;
            else
            {
                IsReconnecting = false;
                _SPort.Close();
                return false;
            }

            /*GETTING THE SIM NUMBER*/
            _SPort.WriteLine("AT+CNUM");
            string[] results = ( Helpers.ResultHelper.getPortResults(_SPort, 400, this).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 5) == "+CNUM").ToArray().FirstOrDefault<string>() ?? "").Replace('"', ' ').Split(',').Where(s => s.Trim() != String.Empty).Select(s => s.Trim()).ToArray<string>();//.Where(str => str.Trim().Substring("+CNUM";

            ContactNumber = null;
            if (results.Length > 1)
                 ContactNumber = results[1];
            else
            {
                IsReconnecting = false;
                _SPort.Close();
                return false;
            }


            /*GETTING THE DEVICE Mobile Network */
            _SPort.WriteLine("AT+COPS?");
             results = ( Helpers.ResultHelper.getPortResults(_SPort, 400, this).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 5) == "+COPS").ToArray().FirstOrDefault<string>() ?? "").Replace('"', ' ').Split(',').Where(s => s.Trim() != String.Empty).Select(s => s.Trim()).ToArray<string>();//.Where(str => str.Trim().Substring("+CNUM";


            //OTHER SETTINGS
            //AT+CNMI=2 = for new message notification
            _SPort.WriteLine("AT+CNMI=2");
            Helpers.ResultHelper.getPortResults(_SPort, 100, this);



            //_SPort.Close();




            Network = null;
            if (result.Length > 2)
                 Network = (results[2] ?? "").Trim();

            if(Network == "" || Network == null)
            {
                IsReconnecting = false;
                _SPort.Close();
                return false;
            }

            //Fixing Error Here.. Just set ErrorCount into Zero

            IsReconnecting = false;
            ErrorCount = 0;


            //_SPort.Close();
            return true;
        }
    }
}
