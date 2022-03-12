using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Service.Helpers
{
    public class SMSNotificationHelper
    {

        public delegate void NotifyMessageResultDelegate(Models.SMSDataModel smsNotify);


        public static NotifyMessageResultDelegate NotifyMessage;
        public static void ReadNotification(Models.SIMDeviceModel simdevice, string _ns)
        {
            //+CMTI: "SM",
             

            string textNotif = "+CMTI: \"SM\",";
            if (_ns.Length > textNotif.Length && _ns.Substring(0, textNotif.Length) == textNotif)
            {
                SMSRequestReadNotif(simdevice, _ns, textNotif);
            }

            textNotif = "+CMTI: \"ME\",";
            if (_ns.Length > textNotif.Length && _ns.Substring(0, textNotif.Length) == textNotif)
            {
                SMSRequestReadNotif(simdevice, _ns, textNotif);
            }

            //AT+CMGR=
            textNotif = "AT+CMGR=";
            if (_ns.Length > textNotif.Length && _ns.Substring(0, textNotif.Length) == textNotif)
            {
                ReadMessage(simdevice, _ns);
            }

            if(simdevice.IsSending)
            {
                System.Threading.Thread.Sleep(8000);
            }
            //else if (simdevice.IsReadData)
            //{

                //var list = ResultHelper.getPortResults(simdevice.Serial, 300).ToList();
                //list.Insert(0, _ns);

                //ReadMessage(simdevice, _ns);
            //}

        }

        public static void SMSRequestReadNotif(Models.SIMDeviceModel simdevice, string _ns, string _NotifPrefix = "+CMTI: \"SM\",")
        {
            int message_id = int.Parse(_ns.Replace(_NotifPrefix, "").Split('\n')[0].Trim());

            var smsData = new Models.SMSDataModel()
            {
                //DataID = message_id,
                DataID = 0,
                RefID = message_id,
                ReadRef = ("AT+CMGR=" + message_id),
                DevicePortName = simdevice.ComputerPort.PortName,
                DeviceCNumber = simdevice.ContactNumber,
                ReceiverCNumber = simdevice.ContactNumber

            };
            simdevice.SMSData.Add(smsData);
           // simdevice.Serial.WriteLine("AT+CMGF=1");
           // System.Threading.Thread.Sleep(100);
            simdevice.Serial.WriteLine(smsData.ReadRef);
            
        }
        public static void ReadMessage(Models.SIMDeviceModel simdevice, string _ns)
        {
            //simdevice.CurrentSMSData.DataFragments.Add(_ns);
            var cursorDevice = simdevice.SMSData.Where(dev => dev.ReadRef == (_ns.Split('\n')[0] ?? "").Trim()).FirstOrDefault<Models.SMSDataModel>();

            cursorDevice.RecieveRawData = _ns + ResultHelper.getRawData(simdevice.Serial, 400, simdevice);

            NotifyMessage?.Invoke(cursorDevice);

        }




    }
}
