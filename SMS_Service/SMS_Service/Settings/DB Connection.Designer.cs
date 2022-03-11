namespace SMS_Service.Settings
{
    partial class DB_Connection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_save = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_test_ssl = new System.Windows.Forms.TextBox();
            this.txt_test_host = new System.Windows.Forms.TextBox();
            this.txt_test_port = new System.Windows.Forms.TextBox();
            this.txt_test_password = new System.Windows.Forms.TextBox();
            this.txt_test_user = new System.Windows.Forms.TextBox();
            this.txt_prod_ssl = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.txt_prod_user = new System.Windows.Forms.TextBox();
            this.txt_prod_port = new System.Windows.Forms.TextBox();
            this.txt_prod_host = new System.Windows.Forms.TextBox();
            this.chk_test = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_prod_db = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_test_db = new System.Windows.Forms.TextBox();
            this.btn_test = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label_test = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(317, 339);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(101, 34);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "SAVE";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txt_prod_db);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_prod_ssl);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_prod_user);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_prod_port);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_prod_host);
            this.groupBox1.Location = new System.Drawing.Point(12, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 267);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PRODUCTION";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_test_db);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txt_test_ssl);
            this.groupBox2.Controls.Add(this.txt_test_host);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txt_test_port);
            this.groupBox2.Controls.Add(this.txt_test_password);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_test_user);
            this.groupBox2.Location = new System.Drawing.Point(218, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 267);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TEST";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "HOST";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "PORT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "USER";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "PASSWORD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "SSL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "SSL";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "PASSWORD";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "USER";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "PORT";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "HOST";
            // 
            // txt_test_ssl
            // 
            this.txt_test_ssl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "TEST_SSL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_ssl.Location = new System.Drawing.Point(15, 197);
            this.txt_test_ssl.Name = "txt_test_ssl";
            this.txt_test_ssl.Size = new System.Drawing.Size(163, 20);
            this.txt_test_ssl.TabIndex = 18;
            this.txt_test_ssl.Text = global::SMS_Service.Properties.Settings.Default.TEST_SSL;
            // 
            // txt_test_host
            // 
            this.txt_test_host.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "TEST_HOST", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_host.Location = new System.Drawing.Point(15, 41);
            this.txt_test_host.Name = "txt_test_host";
            this.txt_test_host.Size = new System.Drawing.Size(163, 20);
            this.txt_test_host.TabIndex = 10;
            this.txt_test_host.Text = global::SMS_Service.Properties.Settings.Default.TEST_HOST;
            // 
            // txt_test_port
            // 
            this.txt_test_port.DataBindings.Add(new System.Windows.Forms.Binding("Tag", global::SMS_Service.Properties.Settings.Default, "TEST_PORT", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_port.Location = new System.Drawing.Point(15, 80);
            this.txt_test_port.Name = "txt_test_port";
            this.txt_test_port.Size = new System.Drawing.Size(163, 20);
            this.txt_test_port.TabIndex = 12;
            this.txt_test_port.Tag = global::SMS_Service.Properties.Settings.Default.TEST_PORT;
            this.txt_test_port.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // txt_test_password
            // 
            this.txt_test_password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "TEST_PASSWORD", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_password.Location = new System.Drawing.Point(15, 158);
            this.txt_test_password.Name = "txt_test_password";
            this.txt_test_password.Size = new System.Drawing.Size(163, 20);
            this.txt_test_password.TabIndex = 16;
            this.txt_test_password.Text = global::SMS_Service.Properties.Settings.Default.TEST_PASSWORD;
            this.txt_test_password.UseSystemPasswordChar = true;
            // 
            // txt_test_user
            // 
            this.txt_test_user.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "TEST_USER", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_user.Location = new System.Drawing.Point(15, 119);
            this.txt_test_user.Name = "txt_test_user";
            this.txt_test_user.Size = new System.Drawing.Size(163, 20);
            this.txt_test_user.TabIndex = 14;
            this.txt_test_user.Text = global::SMS_Service.Properties.Settings.Default.TEST_USER;
            // 
            // txt_prod_ssl
            // 
            this.txt_prod_ssl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "SSL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_prod_ssl.Location = new System.Drawing.Point(19, 197);
            this.txt_prod_ssl.Name = "txt_prod_ssl";
            this.txt_prod_ssl.Size = new System.Drawing.Size(163, 20);
            this.txt_prod_ssl.TabIndex = 8;
            this.txt_prod_ssl.Text = global::SMS_Service.Properties.Settings.Default.SSL;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "PASSWORD", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(19, 158);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(163, 20);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = global::SMS_Service.Properties.Settings.Default.PASSWORD;
            this.textBox4.UseSystemPasswordChar = true;
            // 
            // txt_prod_user
            // 
            this.txt_prod_user.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "USER", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_prod_user.Location = new System.Drawing.Point(19, 119);
            this.txt_prod_user.Name = "txt_prod_user";
            this.txt_prod_user.Size = new System.Drawing.Size(163, 20);
            this.txt_prod_user.TabIndex = 4;
            this.txt_prod_user.Text = global::SMS_Service.Properties.Settings.Default.USER;
            // 
            // txt_prod_port
            // 
            this.txt_prod_port.DataBindings.Add(new System.Windows.Forms.Binding("Tag", global::SMS_Service.Properties.Settings.Default, "PORT", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_prod_port.Location = new System.Drawing.Point(19, 80);
            this.txt_prod_port.Name = "txt_prod_port";
            this.txt_prod_port.Size = new System.Drawing.Size(163, 20);
            this.txt_prod_port.TabIndex = 2;
            this.txt_prod_port.Tag = global::SMS_Service.Properties.Settings.Default.PORT;
            this.txt_prod_port.TextChanged += new System.EventHandler(this.txt_prod_port_TextChanged);
            // 
            // txt_prod_host
            // 
            this.txt_prod_host.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "HOST", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_prod_host.Location = new System.Drawing.Point(19, 41);
            this.txt_prod_host.Name = "txt_prod_host";
            this.txt_prod_host.Size = new System.Drawing.Size(163, 20);
            this.txt_prod_host.TabIndex = 0;
            this.txt_prod_host.Text = global::SMS_Service.Properties.Settings.Default.HOST;
            // 
            // chk_test
            // 
            this.chk_test.AutoSize = true;
            this.chk_test.Checked = global::SMS_Service.Properties.Settings.Default.IsTest;
            this.chk_test.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::SMS_Service.Properties.Settings.Default, "IsTest", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chk_test.Location = new System.Drawing.Point(12, 12);
            this.chk_test.Name = "chk_test";
            this.chk_test.Size = new System.Drawing.Size(119, 17);
            this.chk_test.TabIndex = 0;
            this.chk_test.Text = "Run system as Test";
            this.chk_test.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 220);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "DB";
            // 
            // txt_prod_db
            // 
            this.txt_prod_db.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "DB", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_prod_db.Location = new System.Drawing.Point(19, 236);
            this.txt_prod_db.Name = "txt_prod_db";
            this.txt_prod_db.Size = new System.Drawing.Size(163, 20);
            this.txt_prod_db.TabIndex = 10;
            this.txt_prod_db.Text = global::SMS_Service.Properties.Settings.Default.DB;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 220);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(22, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "DB";
            // 
            // txt_test_db
            // 
            this.txt_test_db.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SMS_Service.Properties.Settings.Default, "TEST_DB", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txt_test_db.Location = new System.Drawing.Point(15, 236);
            this.txt_test_db.Name = "txt_test_db";
            this.txt_test_db.Size = new System.Drawing.Size(163, 20);
            this.txt_test_db.TabIndex = 12;
            this.txt_test_db.Text = global::SMS_Service.Properties.Settings.Default.TEST_DB;
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(12, 339);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(88, 34);
            this.btn_test.TabIndex = 4;
            this.btn_test.Text = "TEST";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Enabled = false;
            this.btn_cancel.Location = new System.Drawing.Point(213, 339);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(98, 34);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // label_test
            // 
            this.label_test.AutoSize = true;
            this.label_test.Location = new System.Drawing.Point(106, 357);
            this.label_test.Name = "label_test";
            this.label_test.Size = new System.Drawing.Size(0, 13);
            this.label_test.TabIndex = 6;
            // 
            // DB_Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 385);
            this.Controls.Add(this.label_test);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_test);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.chk_test);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DB_Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DB_Connection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DB_Connection_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chk_test;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_prod_ssl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_prod_user;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_prod_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_prod_host;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_test_ssl;
        private System.Windows.Forms.TextBox txt_test_host;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_test_port;
        private System.Windows.Forms.TextBox txt_test_password;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_test_user;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_prod_db;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_test_db;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label_test;
    }
}