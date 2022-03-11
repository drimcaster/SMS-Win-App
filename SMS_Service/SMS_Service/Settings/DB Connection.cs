using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Service.Settings
{
    public partial class DB_Connection : Form
    {

        BackgroundWorker BackgroundFailedConnection;
        bool Cancel = false;
        public DB_Connection()
        {
            InitializeComponent();
            txt_prod_port.Text = txt_prod_port.Tag.ToString();
            txt_test_port.Text = txt_test_port.Tag.ToString();
            BackgroundFailedConnection = new BackgroundWorker();
            BackgroundFailedConnection.DoWork += BackgroundFailedConnection_DoWork;
        }

        private void BackgroundFailedConnection_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            System.Threading.Thread.Sleep(4000);
            this.Invoke(new Action(() =>
            {
                if (Cancel == false)
                    label_test.Text = "";
                Cancel = false;
            }));
        }

        private void DB_Connection_FormClosing(object sender, FormClosingEventArgs e)
        {
            SMS_Service.Properties.Settings.Default.Reload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SMS_Service.Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void txt_prod_port_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            int.TryParse(txt_prod_port.Text, out i);
            txt_prod_port.Tag = i;


        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            int.TryParse(txt_test_port.Text, out i);
            txt_test_port.Tag = i;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var conTest = new Models.MySqlData();
            if (conTest.Connected)
            {
                Cancel = true;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                chk_test.Enabled = false;
                btn_test.Enabled = false;

                btn_save.Enabled = true;
                btn_cancel.Enabled = true;
                label_test.Text = "Succeed";
                label_test.ForeColor = Color.Green;

            }
            else
            {
                label_test.Text = "Failed";
                label_test.ForeColor = Color.Red;
                BackgroundFailedConnection.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            label_test.Text = "";
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            chk_test.Enabled = true;
            btn_test.Enabled = true;

            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            
        }
    }
}
