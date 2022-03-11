using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace SMS_Service.Models
{
    public class MySqlData
    {

        MySqlConnection _MySQLCon = null;// new MySqlConnection();
        public MySqlData()
        {



            var _propSet = SMS_Service.Properties.Settings.Default;
            string con_str = "";

            _isTest = _propSet.IsTest;

            if (_propSet.IsTest)
            {
                con_str = "server=" + _propSet.TEST_HOST + ";";
                con_str += "port=" + _propSet.TEST_PORT + ";";
                con_str += "user=" + _propSet.TEST_USER + ";";
                con_str += "password=" + _propSet.TEST_PASSWORD + ";";
                con_str += "database=" + _propSet.DB + ";";
                con_str += "SslMode=" + _propSet.TEST_SSL + ";";
            }
            else
            {
                con_str = "server=" + _propSet.HOST + ";";
                con_str += "port=" + _propSet.PORT + ";";
                con_str += "user=" + _propSet.USER + ";";
                con_str += "password=" + _propSet.PASSWORD + ";";
                con_str += "database=" + _propSet.DB + ";";
                con_str += "SslMode=" + _propSet.TEST_SSL + ";";
            }

            try
            {
                _MySQLCon = new MySqlConnection(con_str);
                _MySQLCon.Open();
                _MySQLCon.Close();
                Connected = true;
            }
            catch (Exception ex)
            {
                Connected = false;
            }

        }
        public bool Connected = false;
        private bool _isTest = false;
        public bool IsTest => _isTest;

      /// <summary>
      /// Sending the messages to the server for analysis
      /// </summary>
      /// <param name="received_messages">To be send to server</param>
      /// <returns>Returns the IDs that has successfully saved to the server </returns>
        public int[] ReceivedMessage(List<Models.ReceivedMessagesModel> received_messages)
        {



            return null;
        }

        public void UpdateSentStatus(List<Models.ToSendMessagesStatusModel> toSendStatusList)
        {





        }


        public List<Models.ToSendMessageModel> GetToSendMessages()
        {
            MySqlCommand command = new MySqlCommand("spToSendMessages", _MySQLCon);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("_ISAPPGET", 1);
            _MySQLCon.Open();
            MySqlDataReader read = command.ExecuteReader();


            List<Models.ToSendMessageModel> tosendList = new List<ToSendMessageModel>();
            while (read.Read())
            {
                JObject jo = new JObject();
                for (int i = 0; i < read.FieldCount; i++)
                {
                    JProperty prop = new JProperty(read.GetName(i), read[i]);
                    jo.Add(prop);
                }
                var item = jo.ToObject<ToSendMessageModel>();
                tosendList.Add(item);
            }
            _MySQLCon.Close();
            return tosendList;
        }


        




    }
}
