using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using SMS_Service.Models;

namespace SMS_Service.Helpers
{
    public class ResultHelper
    {

        public static string getPortResult(SerialPort port, int delay, SIMDeviceModel sim_device)
        {
            if (!port.IsOpen)
            {
                throw new Exception("Port should be opened outside inside this function.");
                return null;
            }
            string str = getPortResults(port, delay, sim_device).LastOrDefault<string>() ?? "";
            return str.Trim();
        }
        public static string[] getPortResults(SerialPort port, int delay, SIMDeviceModel sim_device)
        {

            if (!port.IsOpen)
            {
                throw new Exception("Port should be opened outside inside this function.");
                return null;
            }

            string[] result = new string[0];
            string str = getRawData( port,  delay, sim_device);// Encoding.ASCII.GetString(buff) ?? "";
            result = str.Trim().Split('\n').Where(s => s.Trim() != "" && s != null).Select(s => s.Trim()).ToArray<string>();

            /*
            //CATCH NOTIFICATION HERE IF POSSIBLE
            if (sim_device != null)
            {
                foreach (string s in result)
                {
                    foreach (string m in GlobalHelpers.MessagePrefix)
                    {
                        if (s.Length > m.Length && s.Substring(0, m.Length) == m)
                        {
                            SMSNotificationHelper.ReadNotification(sim_device, s);
                        }
                    }
                }
            }
            */

            return result;
        }
        public static string getRawData(SerialPort port, int delay, SIMDeviceModel sim_device)
        {

            if (!port.IsOpen)
            {
                throw new Exception("Port should be opened outside this function.");
                return null;
            }
            System.Threading.Thread.Sleep(delay / 2);
            int n = port.BytesToRead;
            byte[] buff = new byte[n];
            port.Read(buff, 0, n);

            //string[] result = new string[0];
            string str = Encoding.ASCII.GetString(buff) ?? "";
            //result = str.Trim().Split('\n').Where(s => s.Trim() != "" && s != null).Select(s => s.Trim()).ToArray<string>();
            port.DiscardOutBuffer();
            System.Threading.Thread.Sleep(delay / 2);

            //Notification Breakdown Should be here..
            //Reading in backwards
            //Creating dummy
            /*
            List<String> results = str.Split('\n').ToList();
            for (int i = results.Count - 1; i >= 0; i--)
            {


            }
            */
            //if (sim_device != null)
            //{
            List<String> results = str.Split('\n').ToList();
            
            bool _hasOK = false;
            for (int i = results.Count - 1; i >= 0; i--)
            {
                String s = results[i];

                if (s.Trim() == "OK")
                {
                    _hasOK = true;
                    break;
                }
                foreach (string m in GlobalHelpers.MessagePrefix)
                {
                    if (s.Length > m.Length && s.Substring(0, m.Length) == m)
                    {
                        if (sim_device != null)
                        {
                            SMSNotificationHelper.ReadNotification(sim_device, s);
                        }
                        //REMOVING NOTIFICATION
                        results.RemoveAt(i);
                    }
                }
            }
            for(int i = results.Count - 1; i>=0; i--)
            {
                String s = (results[i] ?? "").Trim();

                if(s == "OK")
                {
                    break;
                }
                else if (_hasOK && s == "")
                {
                    results.RemoveAt(i);
                }
            }


            return string.Join("\n", results);
            //results.Join('\n');
            //}




            //return str;
        }

    }
}
