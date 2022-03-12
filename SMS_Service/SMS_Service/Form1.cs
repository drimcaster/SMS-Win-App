using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using Microsoft.Win32;
using SMS_Service.Helpers;
using SMS_Service.SimClasses;
using SMS_Service.Models;
using System.Text.RegularExpressions;

namespace SMS_Service
{
    public partial class Form1 : Form
    {

        //List<SIMDeviceModel> simDeviceList;


        private string _logString = "";
        private string LogString
        {
            get { return _logString; }
            set
            {
                _logString = value;
                if (_logString.Length > 20000)
                    _logString.Substring(0, 20000);
            }
        }

        public Models.MySqlData MySQL;
        public List<Models.ToSendMessageModel> PendingToSendStatic = null;

        public Form1()
        {
            InitializeComponent();

            //string s = Regex.Match("123412341234", @"(.{3})\s*$");


            PendingToSendStatic = new List<ToSendMessageModel>();
            //MYSQL
            MySQL = new MySqlData();
            if(MySQL.Connected)
            {
                menuStrip1.BackColor = Color.LimeGreen;
                if (MySQL.IsTest)
                {

                    tEXTINFOToolStripMenuItem.Text = "TEST";
                    tEXTINFOToolStripMenuItem.BackColor = Color.Red;
                }
                else
                {
                    tEXTINFOToolStripMenuItem.Visible = false;
                    //tEXTINFOToolStripMenuItem.Text = "DISCONNECTED";
                    //tEXTINFOToolStripMenuItem.BackColor = Color.Blue;
                }
            }
            else
            {
                tEXTINFOToolStripMenuItem.Text = "DISCONNECTED";
                tEXTINFOToolStripMenuItem.BackColor = Color.Red;
            }


            GlobalHelpers.RegisteredSIMDeviceList = SIMDeviceHelper.GetList();
            comboBox1.DataSource = GlobalHelpers.RegisteredSIMDeviceList;// ComputerPortsHelper.Get_ComPorts();
            comboBox1.DisplayMember = "PortDescription";
            SMSNotificationHelper.NotifyMessage = SMSNotify;

            //Open all devices port
            foreach (SIMDeviceModel sim in GlobalHelpers.RegisteredSIMDeviceList)
            {
                sim.Serial.Open();
                sim.DeviceNotify = Notifcation;
            }

            GlobalHelpers.MainForm = this;


            SendDynamicMessageTimer.Enabled = true;
            reloadNewandErrorDeviceTimer.Enabled = true;
            SendMessageStageForSendingMobileTimer.Enabled = true;
            SaveReceivedMessagesTimer.Enabled = true;

        }

        public void Notifcation(SIMDeviceModel device, bool success, string message, SerialDataReceivedEventArgs data_received = null, SerialErrorReceivedEventArgs error_received = null)
        {

            //MessageBox.Show(message);
            //if(stat)
            string compose = "\r\n";
            compose += "===" + device.ContactNumber + "===\r\n";
            compose += "Status: " + (success ? 1 : 0) + "\r\n";
            compose += "Type: " + (success ? data_received.EventType.ToString() : error_received.EventType.ToString()) + "\r\n";
            compose += "Content:\r\n";
            compose += message + "\r\n";
            compose += "================\r\n";

            LogString = compose + LogString;
            this.Invoke(new Action(() =>
            {
                txt_logs.Text = LogString;

            }));
            SMSNotificationHelper.ReadNotification(device, message);

        }

        void SMSNotify(Models.SMSDataModel _data)
        {
            this.Invoke(new Action(() =>
            {
                addOrSetRow(_data);
            }));
        }

        void addOrSetRow(Models.SMSDataModel _data)
        {
            DataGridViewRow row = null;
            //Check all Rows if SMSDataModel already exists
            foreach (DataGridViewRow rowItem in dataGridView1.Rows)
            {
                if (rowItem.Tag == _data)
                {
                    row = rowItem; break;
                }
            }

            if (row == null)
            {
                //row = new DataGridViewRow();
                dataGridView1.Rows.Add();
                row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                row.Tag = _data;
            }
            setRowColor(row);
        }

        void setRowColor(DataGridViewRow row)
        {
            SMSDataModel _data = row.Tag as SMSDataModel;
           
            int i = 0;
            // i++;

            row.Cells[i++].Value = _data.DataID;
            row.Cells[i++].Value = _data.ActionType;

            int _mstatus = i++;
           
            row.Cells[_mstatus].Value = _data.MStatus;
            if (_data.MStatus == MStatusTypes.Pending)
                row.Cells[_mstatus].Style.ForeColor = Color.Yellow;
            else if (_data.MStatus == MStatusTypes.Failed)
                row.Cells[_mstatus].Style.ForeColor = Color.Red;
            else if (_data.MStatus == MStatusTypes.Sending)
                row.Cells[_mstatus].Style.ForeColor = Color.Orange;
            else if (_data.MStatus == MStatusTypes.Success)
                row.Cells[_mstatus].Style.ForeColor = Color.Green;

            row.Cells[i++].Value = _data.DevicePortName;
            row.Cells[i++].Value = _data.DeviceCNumber;
            row.Cells[i++].Value = _data.SenderCNumber;
            row.Cells[i++].Value = _data.ReceiverCNumber;
            row.Cells[i++].Value = _data.ActionDateTime;
            //row.Cells[i++].Value = _data.Message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gms_sender();
            SendOnce();
        }

        public void gms_sender()
        {

            SIMDeviceModel model = comboBox1.SelectedItem as SIMDeviceModel;
            if (model == null)
            {
                MessageBox.Show("Device Not Available");
                return;
            }

            if (model.SIMModel == "SIMCOM_SIM800C")
            {
                SIM800C_SmsSender ss = new SIM800C_SmsSender(model);
                ss.SendResult = (sim_device, sms, is_success, message) =>
                {
                    //MessageBox.Show(message);
                };
                ss.SendNow(textBox2.Text, textBox1.Text);
            }

        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //throw new NotImplementedException();

            //MessageBox.Show(sp.ReadExisting() + e.EventType);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (SIMDeviceModel sim in GlobalHelpers.RegisteredSIMDeviceList)
            {
                sim.Serial.Close();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            var selectedDevice = comboBox1.SelectedItem as SIMDeviceModel;
            label1.Text = "Sender#: \n (" + selectedDevice.Network + ") " + selectedDevice.ContactNumber;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //SendMessageTimer.Enabled = false;
            //Sending 
            var notBusyDevices = GlobalHelpers.RegisteredSIMDeviceList.Where(device => device.IsBusy == false && device.ReadyToSend ).ToList();
            //System.Diagnostics.Debug.Write(new Random().Next(1));
            //if (notBusyDevices.Count > 0)
            // {
            int _RegisteredDevices_Count = GlobalHelpers.RegisteredSIMDeviceList.Count;
            foreach (Models.SIMDeviceModel notBusyDevice in notBusyDevices)
            {
                List<SMSDataModel> smsDynamicList = new List<SMSDataModel>();
                List<SMSDataModel> smsStaticList = new List<SMSDataModel>();

                //this.Invoke(new Action(() =>
                //{

                //PRIORITIZE THE STATIC
                //DYNAMIC
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var s = row.Tag as SMSDataModel;

                    if (s.SendFailedCount < GlobalHelpers.MaxResend && s.MStatus != MStatusTypes.Success && s.MStatus != MStatusTypes.Sending && s.ActionType == ActionTypes.Send && s.Sending == false && s.SendingType == SendingTypes.StaticDeviceCNumber && s.DeviceCNumber == notBusyDevice.ContactNumber)
                    {
                        smsStaticList.Add(s);
                    }
                    else if (s.SendFailedCount < GlobalHelpers.MaxResend && s.MStatus != MStatusTypes.Success && s.MStatus != MStatusTypes.Sending && s.ActionType == ActionTypes.Send && s.Sending == false && s.SendingType != SendingTypes.StaticDeviceCNumber && (_RegisteredDevices_Count == 1 || s.DynamicFailedNumberSends.Contains(notBusyDevice.ContactNumber) == false ))
                    {
                        //s.Sending = true;
                        //The condition that will prevent the Sending Type == None to just only send once.
                        if (s.SendingType == SendingTypes.None && s.SendFailedCount > 0)
                            continue;
                        

                        //Retry already sent but lacking of device.
                        int recursiveSendCount = 1 + (GlobalHelpers.MaxResend - _RegisteredDevices_Count);

                        if (s.SendFailedCount > 0 &&  s.SendFailedCount <= recursiveSendCount  )
                        {
                            var toRemove = s.DynamicFailedNumberSends.FirstOrDefault();
                            if (toRemove != null)
                                s.DynamicFailedNumberSends.Remove(toRemove);
                        }
                        smsDynamicList.Add(s);

                    }

                }
                //}));
                //if (smsDynamicList.Count > 0)
                //{

                SMSDataModel toSendData = null;

                //Get The StaticList which is the prioritize most
                if (smsStaticList.Count > 0)
                {
                    toSendData = smsStaticList.Where(smsItem => smsItem.DeviceCNumber == notBusyDevice.ContactNumber).OrderByDescending(dev => dev.RankPriority).ThenBy(dev => dev.SendLastAttempt).FirstOrDefault();
                }
                if (toSendData == null)
                {
                    //Where(smsItem => smsItem.DeviceCNumber == notBusyDevice.ContactNumber)
                    toSendData = smsDynamicList.OrderByDescending(dev => dev.RankPriority).ThenBy(dev => dev.SendLastAttempt).FirstOrDefault();
                }

                if (toSendData == null)
                    continue;

                BackgroundWorker bg = new BackgroundWorker();
                notBusyDevice.ReadyToSend = false;
                toSendData.Sending = true;
                bg.DoWork += (send, evt) =>
                {
                    SIM800C_SmsSender ss = new SIM800C_SmsSender();
                    ss.SendResult = (sim_device, sms, is_success, message) =>
                    {
                        if (sim_device != null && is_success == false)
                            sms.SendFailedCount++;
                        if (sms.MStatus != MStatusTypes.Sending)
                            sms.Sending = false;
                    };
                    ss.SendSMSData(toSendData, false, notBusyDevice);
                };
                bg.RunWorkerAsync();
                //}
            } 

        }





        private void reloadNewandErrorDevice_Tick(object sender, EventArgs e)
        {
           // foreach( var device in GlobalHelpers.ActiveSIMDeviceList)
          //  {
                //if (device.IsBusy == false && device.ErrorCount < 4)
                //    device.Serial.WriteLine("AT");
           // }


            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += Bg_DoWork;
            bg.RunWorkerAsync();
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            var deviceItem = GlobalHelpers.InActiveSIMDeviceList.OrderBy(dev => dev.LastReconnectAt).FirstOrDefault();
            if (deviceItem != null)
                deviceItem.ReconnectDevice();

            //Add to the Source a new SIM Device
            Helpers.SIMDeviceHelper.RegisterNewSIMDevice();
            //if (this.IsDisposed || comboBox1.IsDisposed || comboBox1.Disposing  || this.Disposing )
            //  return;

            this?.Invoke(new Action(() =>
            {

                if (comboBox1.Focused || this.Disposing) return;

                int i = comboBox1.SelectedIndex;
                comboBox1.DataSource = null;
                comboBox1.DataSource = GlobalHelpers.RegisteredSIMDeviceList;
                comboBox1.DisplayMember = "PortDescription";
                if (comboBox1.Items.Count > i && i >= 0)
                    comboBox1.SelectedIndex = i;

                //dataGridView1.Refresh();
                //dataGridView1.DataSource = GlobalHelpers.RegisteredSIMDeviceList;
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LogString = "";
            txt_logs.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //GlobalHelpers.ActiveSIMDeviceList
            int i = GlobalHelpers.ActiveSIMDeviceList.ToList().Count;

            if(i<=0)
            {
                MessageBox.Show("No active SIM DeviceList");
                return;
            }
            int random = new Random().Next(i);
            //Add a data 

            Models.SIMDeviceModel sim_device = GlobalHelpers.ActiveSIMDeviceList.ToList()[random];
            SMSDataModel sms = new SMSDataModel(sim_device, textBox1.Text, textBox2.Text, SendingTypes.DynamicDeviceCNumber);
            //sms.MStatus
            
            addOrSetRow(sms);



        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            // if (e.RowIndex < 0)
            //   return;
            if (dataGridView1.SelectedRows.Count <= 0)
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];//.Rows[e.RowIndex];
            if (row.Index == -1 || row == null)
                return;
            if (row.Tag == null)
                return;


            Models.SMSDataModel sms = row.Tag as Models.SMSDataModel;
            if (sms == null) return;

            txt_failedattempt_count.Text = sms.SendFailedCount.ToString();
            txt_last_sent_at.Text = sms.SendLastAttempt.ToString();
            txt_sending_type.Text = sms.SendingType.ToString();
            txt_message.Text = sms.Message;

            btn_reply_sender.Enabled = sms.ActionType == ActionTypes.Receive;
            btn_resend_retry.Enabled = sms.ActionType == ActionTypes.Send && sms.MStatus != ( MStatusTypes.Success | MStatusTypes.Sending );
            
        }
        
        private void btn_resend_retry_Click(object sender, EventArgs e)
        {
            //
            //senderrorcount =0
            //DynamicFailedSendNumbers = new List<string>();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //GlobalHelpers.ActiveSIMDeviceList
            //int i = GlobalHelpers.ActiveSIMDeviceList.ToList().Count;
            SIMDeviceModel selectedDevice = null;

            if( comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a Device to send your message");
            }

            selectedDevice = comboBox1.SelectedItem as SIMDeviceModel;

            if (selectedDevice == null)
            {
                MessageBox.Show("Device item has been invalidated.");
                return;
            }
            //int random = new Random().Next(i);
            //Add a data 

            //Models.SIMDeviceModel sim_device = GlobalHelpers.ActiveSIMDeviceList.ToList()[random];
            SMSDataModel sms = new SMSDataModel(selectedDevice, textBox1.Text, textBox2.Text, SendingTypes.StaticDeviceCNumber);
            //sms.MStatus

            addOrSetRow(sms);

        }

        private void SendOnce()
        {

            SIMDeviceModel selectedDevice = null;

            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a Device to send your message");
            }

            selectedDevice = comboBox1.SelectedItem as SIMDeviceModel;

            if (selectedDevice == null)
            {
                MessageBox.Show("Device item has been invalidated.");
                return;
            }
            //int random = new Random().Next(i);
            //Add a data 

            //Models.SIMDeviceModel sim_device = GlobalHelpers.ActiveSIMDeviceList.ToList()[random];
            SMSDataModel sms = new SMSDataModel(selectedDevice, textBox1.Text, textBox2.Text, SendingTypes.None);
            //sms.MStatus

            addOrSetRow(sms);
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dbSet = new Settings.DB_Connection();
            if( dbSet.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Please restart to take effect.");
            }
            dbSet.Dispose();
        }

        private void SaveReceivedMessages_Tick(object sender, EventArgs e)
        {
            if (MySQL.Connected == false || MySQL.IsBusy == true) return;

            SaveReceivedMessagesTimer.Enabled = false;
            
            var smsReceivedList = new List<SMSDataModel>();
            //Use for putting ID on row
            uint max_id = 0;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                var smsItem = dgvr.Tag as SMSDataModel;
                if (smsItem == null)
                    continue;

                if (smsItem.ActionType == ActionTypes.Receive && smsItem.DataID == 0)
                {
                    smsReceivedList.Add(smsItem);
                }
                else if (smsItem.ActionType == ActionTypes.Receive && max_id > smsItem.DataID)
                {
                    max_id = smsItem.DataID;
                }
            }
            if (smsReceivedList.Count == 0)
            {
                SaveReceivedMessagesTimer.Enabled = true;
                return;
            }

            //set max_id = increment to 1;
            //max_id++;
            foreach (SMSDataModel s in smsReceivedList)
            {
                s.DataID = max_id++;
            }

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += (sen, evt) =>
                {

                    //Sending messages to the server upto 20
                    List<Models.ReceivedMessagesModel> receivedMessage = new List<ReceivedMessagesModel>();

                    foreach (SMSDataModel smsItem in smsReceivedList)
                    {

                        receivedMessage.Add(
                            new ReceivedMessagesModel()
                            {
                                id = smsItem.DataID,
                                message = smsItem.Message,
                                received_app_at = smsItem.SendLastAttempt.ToString("MM/dd/yyyy hh:mm:ss tt"),
                                received_mobile_at = smsItem.ActionDateTime,
                                receiver_no = smsItem.ReceiverCNumber,
                                sender_no = smsItem.SenderCNumber

                            }
                       );

                    }

                    uint[] toDeleteIDs = MySQL.ReceivedMessage(receivedMessage);
                    //GET DEVICE TO REMOVE RECORD
                    var removeReceivedToRemoveList = smsReceivedList.Where(sms => toDeleteIDs.Contains(sms.DataID));
                    foreach(var sms in removeReceivedToRemoveList)
                    {
                        var sDevice = GlobalHelpers.ActiveSIMDeviceList.Where(device => device.ContactNumber == sms.ReceiverCNumber).FirstOrDefault();
                        if (sDevice == null)
                            continue;
                        Helpers.SIMDeviceHelper.RemoveDeviceRecord(sDevice, sms.RefID);

                    }




                    //Reactivating the sending messages
                    this.Invoke(new Action(() =>
                    {

                        if (toDeleteIDs.Length > 0)
                        {
                            foreach (DataGridViewRow toDelteRow in dataGridView1.Rows)
                            {
                                SMSDataModel sms_item = toDelteRow.Tag as SMSDataModel;
                                if (sms_item == null) continue;

                                if (sms_item.ActionType == ActionTypes.Receive && toDeleteIDs.Contains(sms_item.DataID))
                                    dataGridView1.Rows.Remove(toDelteRow);

                            }
                        }
                        var failedSavingSMS = smsReceivedList.Where(sms => toDeleteIDs.Contains(sms.DataID) == false);
                        foreach(var failed in failedSavingSMS)
                        {
                            failed.DataID = 0;
                        }


                        SaveReceivedMessagesTimer.Enabled = true;
                    }));

                };
            bg.RunWorkerAsync();


        }

        private void SendMessagesToMobileTimer_Tick(object sender, EventArgs e)
        {
            SendMessageStageForSendingMobileTimer.Enabled = false;
            if (MySQL.Connected && MySQL.IsBusy == false)
            {

                List<SentMessageStatusModel> sentStatusList = new List<SentMessageStatusModel>();

                //GET DATATABLE ROW SEND
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    SMSDataModel sms = row.Tag as SMSDataModel;
                    if (sms == null) continue;

                    if (sms.DataID > 0 && sms.ActionType == ActionTypes.Send && sms.StatusUpdating == false)
                    {
                        SentMessageStatusModel _stat = new SentMessageStatusModel();
                        _stat.id = sms.DataID;
                        _stat.sender_no = sms.SenderCNumber;
                        _stat.sent_mobile_at = sms.SendLastAttempt.ToString("MM/dd/yyyy hh:mm:ss tt");// sms.ActionDateTime;
                        if (sms.MStatus == MStatusTypes.Success)
                        {
                            _stat.status_id = 2;
                        }
                        else if (sms.MStatus == MStatusTypes.Failed && sms.SendFailedCount >= 3)
                        {
                            _stat.status_id = 3;
                        }
                        else if (sms.MStatus == MStatusTypes.Failed && sms.SendingType == SendingTypes.DynamicDeviceCNumber && sms.SendFailedCount >= 3)
                        {
                            _stat.status_id = 3;
                        }
                        else if (sms.MStatus == MStatusTypes.Failed && sms.SendingType == SendingTypes.None && sms.SendFailedCount == 1)
                            _stat.status_id = 3;

                        if (_stat.status_id > 1)
                        {
                            sms.StatusUpdating = true;
                            sentStatusList.Add(_stat);
                        }
                    }

                }



                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += (sen, evt) =>
                {
                    int activeDeviceCount = GlobalHelpers.ActiveSIMDeviceList.Count();
                    if (activeDeviceCount == 0)
                    {
                        this.Invoke(new Action(() => { SendMessageStageForSendingMobileTimer.Enabled = true; }));
                        return;
                    }
                
                    List<Models.ToSendMessageModel> toSendList = MySQL.GetToSendMessages(sentStatusList);

                    this.Invoke(new Action(() => {

                        foreach (SentMessageStatusModel sentItem in sentStatusList)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                SMSDataModel sms = row.Tag as SMSDataModel;
                                if (sms == null) continue;

                                if (sentItem.id == sms.DataID)
                                    dataGridView1.Rows.Remove(row);
                            }
                        }



                    }));

                    //CONVERT TO toSendMessagesinto

                    List<Models.SMSDataModel> smsdataList = new List<SMSDataModel>();
                    for(int i = 0; i < toSendList.Count; i++)
                    {
                        var toSendItem = toSendList[i];
                        SIMDeviceModel device = null;
                        SendingTypes sendType = SendingTypes.None;
                        //for Static
                        if (toSendItem.sending_type_id == 1)
                        {
                            //get the device
                            var matchItem = GlobalHelpers.ActiveSIMDeviceList.Where(s => (s.ContactNumber ?? "").Length > 10 && toSendItem.sender_no.Length > 10 && s.ContactNumber.Substring(s.ContactNumber.Length - 10) == toSendItem.sender_no.Substring(toSendItem.sender_no.Length - 10)).FirstOrDefault();
                            if (matchItem != null)
                            {
                                device = matchItem;
                                sendType = SendingTypes.StaticDeviceCNumber;
                            }
                            else
                            {
                                //IF Not Active..
                                PendingToSendStatic.Add(toSendItem);
                                continue;
                            }
                        }
                        else
                        {
                            //for Dynamic
                            device = GlobalHelpers.ActiveSIMDeviceList.ElementAt(i % activeDeviceCount);
                            if (toSendItem.sending_type_id <= 2)
                                sendType = SendingTypes.DynamicDeviceCNumber;
                            else
                                sendType = SendingTypes.None;
                        }


                        SMSDataModel smsItem = new SMSDataModel(device, toSendItem.receiver_no, toSendItem.message, sendType);
                        smsItem.DataID = toSendItem.id;
                        smsdataList.Add(smsItem);

                    }

                    System.Threading.Thread.Sleep(500);
                    this.Invoke(new Action(() => {
                        
                        //Adding Row
                        foreach (var sms in smsdataList)
                        {
                            addOrSetRow(sms);
                        }

                        SendMessageStageForSendingMobileTimer.Enabled = true;
                    }));
                   
                };
                bg.RunWorkerAsync();
            }
        }
    }
}