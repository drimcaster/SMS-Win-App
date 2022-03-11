using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.IO.Ports;
using SMS_Service.Models;

namespace SMS_Service.Helpers
{
    public class ComputerPortsHelper
    {
        public static List<ComputerPortModel> Get_ComPorts()
        {
            List<ComputerPortModel> lp = new List<ComputerPortModel>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%)'"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                //var tList = (from n in portnames join p in ports on n equals p["DeviceID"].ToString() select n + " - " + p["Caption"]).ToList();
                //ports[0][""]
                //tList.ForEach(Console.WriteLine);
                foreach (ManagementBaseObject s in ports)
                {
                    string s_DeviceID = s.GetPropertyValue("PnpDeviceID").ToString();//["DeviceID"];
                    string s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";

                    string port_name = Registry.GetValue(s_RegPath, "PortName", "").ToString();
                    //textBox2.Text += ("\r\n" + s["Caption"] + port_name);
                    lp.Add(new ComputerPortModel { PortName = port_name, Description = s["Caption"].ToString() });
                }
            }

            return lp;
        }
    }
}
