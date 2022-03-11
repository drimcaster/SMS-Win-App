using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using SMS_Service.Models;


namespace SMS_Service.Helpers
{
    public class SIMDeviceHelper
    {
        public static List<Models.SIMDeviceModel> GetList()
        {

            var ports = ComputerPortsHelper.Get_ComPorts();

            //GET SIM VIA PORT
            var simDeviceList = GETSIMPortAT(ports);

            //GET AND SET MODEL OF SIM DEVICE
            var filteredSimDeviceList = GetSIMDeviceModel(simDeviceList);


            //SET THE MOBILE NUMBER OF THE SIM
            var simContact = GetSetSIMDeviceContactNumber(filteredSimDeviceList);


            return simContact;
        }

        //CHECK ANG GET SIM PORTS IF ACCEPTS AT
        public static List<SIMDeviceModel> GETSIMPortAT( List<ComputerPortModel> ports  )
        {
            var simDeviceList = new List<SIMDeviceModel>();

            //GET THE SERIAL PORT IF RESPOND TO AT COMMAND
            foreach (ComputerPortModel computerport in ports)
            {
                try
                {
                    SerialPort _SPort = new SerialPort(computerport.PortName, 9600);

                    if (_SPort.IsOpen)
                        continue;

                    _SPort.Open();
                    _SPort.WriteLine("AT");
                    string result = ResultHelper.getPortResult(_SPort, 200, null);

                    if (result == "OK")
                    {
                        simDeviceList.Add(new SIMDeviceModel() { ComputerPort = computerport });
                    }

                    _SPort.Close();
                }
                catch
                {

                }

            }

            return simDeviceList;
        }

        //GET AND SET THE MODEL OF THE SIMDEVICE
        public static List<SIMDeviceModel> GetSIMDeviceModel(List<SIMDeviceModel> SIMDeviceList)
        {
            //GET MODEL OF SIM DEVICE
            foreach (SIMDeviceModel sim_device in SIMDeviceList)
            {
                var serial = sim_device.Serial;
                if (serial == null)
                    continue;

                if (serial.IsOpen)
                    continue;

                serial.Open();
                serial.WriteLine("AT+CGMM");
                string result = ResultHelper.getPortResults(serial, 300, sim_device).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 6) == "SIMCOM").ToArray().FirstOrDefault<string>() ?? "";//.Where(str => str.Trim().Substring("+CNUM";

                if (result != "" && result != null && result != "ERROR")
                    sim_device.SIMModel = result;
                serial.Close();

            }
            return SIMDeviceList.Where(s => s.SIMModel != null).ToList();

        }
        //GET AND SET THE MODEL OF THE SIMDEVICE
        public static List<SIMDeviceModel> GetSetSIMDeviceContactNumber(List<SIMDeviceModel> SIMDeviceList)
        {
            //GET MODEL OF SIM DEVICE
            foreach (SIMDeviceModel sim_device in SIMDeviceList)
            {

                if(sim_device.SIMModel == "SIMCOM_SIM800C")
                {
                    SIM800SeriesNumber(sim_device);
                    SIM800MobileNetwork(sim_device);
                }
                //if (result != "" && result != null)
                    //sim_device.SIMModel = result;

            }
            return SIMDeviceList.Where(s => s.ContactNumber != null && s.Network != null).ToList();

        }
        public static string SIM800SeriesNumber(SIMDeviceModel sim_device)
        {

            var serial = sim_device.Serial;
            if (serial == null)
                return null;

            if (serial.IsOpen)
                return null;

            serial.Open();
            serial.WriteLine("AT+CNUM");
            string[] result = (ResultHelper.getPortResults(serial, 400, sim_device).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 5) == "+CNUM").ToArray().FirstOrDefault<string>() ?? "").Replace('"', ' ').Split(',').Where(s => s.Trim() != String.Empty).Select(s => s.Trim()).ToArray<string>();//.Where(str => str.Trim().Substring("+CNUM";
            serial.Close();

            if (result.Length > 1)
                sim_device.ContactNumber = result[1];
            

            return sim_device.ContactNumber;
        }

        public static string SIM800MobileNetwork(SIMDeviceModel sim_device)
        {

            var serial = sim_device.Serial;
            if (serial == null)
                return null;

            if (serial.IsOpen)
                return null;

            serial.Open();
            serial.WriteLine("AT+COPS?");
            string[] result = (ResultHelper.getPortResults(serial, 400, sim_device).Select(s => s.Trim()).Where(s => s.Length > 5 && s.Substring(0, 5) == "+COPS").ToArray().FirstOrDefault<string>() ?? "").Replace('"', ' ').Split(',').Where(s => s.Trim() != String.Empty).Select(s => s.Trim()).ToArray<string>();//.Where(str => str.Trim().Substring("+CNUM";
            serial.Close();

            if (result.Length > 2)
                sim_device.Network = result[2];


            return sim_device.Network;
        }

        

        public static void RegisterNewSIMDevice()
        {
            var ports = ComputerPortsHelper.Get_ComPorts();


            //Adding port from non registered app.
            List<Models.ComputerPortModel> newport = new List<ComputerPortModel>();
            foreach( ComputerPortModel portTest in ports)
            {
                var hasPort = GlobalHelpers.RegisteredSIMDeviceList.Where(dev => dev.ComputerPort.PortName == portTest.PortName).FirstOrDefault();
                if (hasPort == null)
                    newport.Add(portTest);
            }

            if (newport.Count <= 0)
                return;

            //GET SIM VIA PORT
            var simDeviceList = GETSIMPortAT(newport);

            //GET AND SET MODEL OF SIM DEVICE
            var filteredSimDeviceList = GetSIMDeviceModel(simDeviceList);


            //SET THE MOBILE NUMBER OF THE SIM
            var simContact = GetSetSIMDeviceContactNumber(filteredSimDeviceList);

            foreach(Models.SIMDeviceModel dev in simContact)
            {
                dev.Serial.Open();
            }


            GlobalHelpers.RegisteredSIMDeviceList.AddRange(simContact);

        }



    }
}
