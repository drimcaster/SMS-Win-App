using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using SMS_Service.Models;
using SMS_Service.Helpers;

namespace SMS_Service.SimClasses
{
    public class SIM800C_SmsSender
    {
        public delegate void SMSResult(SIMDeviceModel sim_device, SMSDataModel sms_data, bool isSuccess, string Message);
        public SerialPort _SPort;
        private SIMDeviceModel SimDevice;
        public SMSDataModel toSendData;


        public SMSResult SendResult; 
        public SIM800C_SmsSender()
        {

        }
        /*
        public SIM800C_SmsSender(string _portName, int _baudRate = 9600)
        {
            _SPort = new SerialPort(_portName, _baudRate);
            toSendData = null;
            //_SPort.DataReceived += _SPort_DataReceived;
        }*/
        public SIM800C_SmsSender(Models.SIMDeviceModel sim_device)
        {
            SimDevice = sim_device;
            _SPort = sim_device.Serial;
            //_SPort = sim_device.Serial;
        }
        public SIM800C_SmsSender(SerialPort serial)
        {
            _SPort = serial;
        }

        public bool SendNow(string Message, string Number)
        { 


            if (SimDevice == null)
            { 
               
                SendResult?.Invoke( SimDevice, toSendData, false, "Device Not Found!");
                return false;
            }
                //throw new Exception("Pl");
            if(toSendData != null)
            {
                SimDevice.CurrentSMSData = toSendData;
                toSendData.SetMStatus(MStatusTypes.Sending);
                SMSNotificationHelper.NotifyMessage?.Invoke(toSendData);
            }
            _SPort.Encoding = System.Text.Encoding.GetEncoding("GB2312");
            if ( _SPort.IsOpen && SimDevice.IsBusy == false)
            {
                SimDevice.ReadyToSend = false;
                SimDevice.SetSending(true);
                _SPort.DiscardOutBuffer();
                _SPort.WriteLine("AT+CMGF=1");
                //System.Threading.Thread.Sleep(100);
                _SPort.WriteLine("AT+CMGS=\"" + Number + "\"");
                _SPort.WriteLine(Message + ((char)26).ToString());

                //if (toSendData == null)
                //System.Threading.Thread.Sleep(8000);

                string str = "";
                int limit_counter = 0;
                while (true)
                {
                   str = Helpers.ResultHelper.getPortResult(_SPort, 1000, SimDevice);
                    if (str.Trim() == "OK" || str.Trim() == "ERROR")
                        break;
                    else if (limit_counter == 10)
                        break;
                    limit_counter++;
                }
                SimDevice.SetSending(false);
                SimDevice.ReadyToSend = true;
                
                SMSDataModel _SmsData = toSendData ?? new SMSDataModel(SimDevice, Number, Message);
                _SmsData.SetMStatus(MStatusTypes.Sending);
                SMSNotificationHelper.NotifyMessage?.Invoke(_SmsData);
                _SmsData.SendLastAttempt = DateTime.Now;

                if (str.Trim() == "OK" || _SmsData.MStatus == MStatusTypes.Success)
                {
                    _SmsData.SetMStatus(MStatusTypes.Success);
                    SMSNotificationHelper.NotifyMessage?.Invoke(_SmsData);
                    SendResult?.Invoke( SimDevice, _SmsData, true, "Send Completed");
                    SimDevice.ErrorCount = 0;
                    return true;
                }
                else 
                {
                   
                    _SmsData.SetMStatus(MStatusTypes.Failed);
                    _SmsData.DynamicFailedNumberSends.Add(_SmsData.DeviceCNumber);
                    SMSNotificationHelper.NotifyMessage?.Invoke(_SmsData);
                    SendResult?.Invoke( SimDevice, _SmsData, false, "Device SIM Error");
                    SimDevice.ErrorCount++;
                    //_SmsData.SendFailedCount++;
                    return false;
                }

            }
            else
            {
                //SendResult?.Invoke( SimDevice, toSendData, false, "Is Busy");
                //SimDevice.ErrorCount++;
                SimDevice.ReadyToSend = true;
                return false;
            }

        }
        public bool SendSMSData(SMSDataModel smsData, bool forceSend = false, SIMDeviceModel notBusy_Device = null)
        {
            if (smsData.ActionType != ActionTypes.Send)
            {
                throw new Exception("Should be a sending type");
            }
            else if (smsData.MStatus == MStatusTypes.Success && forceSend == false)
            {
                throw new Exception("Already sent, do you intent to resend?");
            }

            SimDevice = null;

            if (smsData.SendingType == SendingTypes.StaticDeviceCNumber)
            {
                SimDevice = GlobalHelpers.RegisteredSIMDeviceList.Where(device => device.ContactNumber == smsData.DeviceCNumber && device.IsBusy == false ).FirstOrDefault();
                if (SimDevice != null)
                {
                    smsData.DevicePortName = SimDevice.ComputerPort.PortName;
                }
            }
            else
            {

                if (notBusy_Device != null)
                {
                    if (notBusy_Device.IsBusy == false) //
                    {
                        SimDevice = notBusy_Device;
                        smsData.DevicePortName = SimDevice.ComputerPort.PortName;
                        smsData.DeviceCNumber = SimDevice.ContactNumber;
                        smsData.SetSenderCNumber(SimDevice.ContactNumber);
                    }
                }
                else
                {
                    var notBusyDevice = GlobalHelpers.RegisteredSIMDeviceList.Where(device => device.IsBusy == false && device.ReadyToSend).ToList();
                    if (notBusyDevice.Count > 0)
                    {
                        int random = new Random().Next(notBusyDevice.Count);
                        SimDevice = notBusyDevice[random]; //GlobalHelpers.ActiveSIMDeviceList.Where(device => device.IsBusy == false).FirstOrDefault();

                        smsData.DevicePortName = SimDevice.ComputerPort.PortName;
                        smsData.DeviceCNumber = SimDevice.ContactNumber;
                        smsData.SetSenderCNumber(SimDevice.ContactNumber);
                        
                    }
                }

            }




            if (SimDevice == null)
                return false;

            SimDevice.ReadyToSend = false;
            _SPort = SimDevice.Serial;
            toSendData = smsData;

            return this.SendNow(smsData.Message, smsData.ReceiverCNumber);

        }
        




    }
}
