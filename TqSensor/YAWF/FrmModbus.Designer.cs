namespace YAWF
{
    partial class FrmModbus
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cb_PortName = new System.Windows.Forms.ComboBox();
            this.cb_BaudRate = new System.Windows.Forms.ComboBox();
            this.cb_Parity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_send = new System.Windows.Forms.TextBox();
            this.cb_DataBits = new System.Windows.Forms.ComboBox();
            this.cb_StopBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_delay = new System.Windows.Forms.TextBox();
            this.btn_setup = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_CRC = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_CRC = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_read = new System.Windows.Forms.TextBox();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_send = new System.Windows.Forms.Button();
            this.tb_code = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_PortName
            // 
            this.cb_PortName.FormattingEnabled = true;
            this.cb_PortName.Location = new System.Drawing.Point(76, 30);
            this.cb_PortName.Name = "cb_PortName";
            this.cb_PortName.Size = new System.Drawing.Size(121, 20);
            this.cb_PortName.TabIndex = 0;
            // 
            // cb_BaudRate
            // 
            this.cb_BaudRate.FormattingEnabled = true;
            this.cb_BaudRate.Location = new System.Drawing.Point(76, 59);
            this.cb_BaudRate.Name = "cb_BaudRate";
            this.cb_BaudRate.Size = new System.Drawing.Size(121, 20);
            this.cb_BaudRate.TabIndex = 1;
            // 
            // cb_Parity
            // 
            this.cb_Parity.FormattingEnabled = true;
            this.cb_Parity.Location = new System.Drawing.Point(282, 28);
            this.cb_Parity.Name = "cb_Parity";
            this.cb_Parity.Size = new System.Drawing.Size(121, 20);
            this.cb_Parity.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "串口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "波特率：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "校验位：";
            // 
            // tb_send
            // 
            this.tb_send.Location = new System.Drawing.Point(17, 76);
            this.tb_send.Multiline = true;
            this.tb_send.Name = "tb_send";
            this.tb_send.Size = new System.Drawing.Size(350, 312);
            this.tb_send.TabIndex = 7;
            // 
            // cb_DataBits
            // 
            this.cb_DataBits.FormattingEnabled = true;
            this.cb_DataBits.Location = new System.Drawing.Point(282, 58);
            this.cb_DataBits.Name = "cb_DataBits";
            this.cb_DataBits.Size = new System.Drawing.Size(121, 20);
            this.cb_DataBits.TabIndex = 3;
            // 
            // cb_StopBits
            // 
            this.cb_StopBits.FormattingEnabled = true;
            this.cb_StopBits.Location = new System.Drawing.Point(488, 26);
            this.cb_StopBits.Name = "cb_StopBits";
            this.cb_StopBits.Size = new System.Drawing.Size(121, 20);
            this.cb_StopBits.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 34;
            this.label5.Text = "数据位：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(429, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "停止位：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_delay);
            this.groupBox1.Controls.Add(this.btn_setup);
            this.groupBox1.Controls.Add(this.cb_PortName);
            this.groupBox1.Controls.Add(this.cb_BaudRate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cb_Parity);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_StopBits);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cb_DataBits);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 100);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本参数设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(441, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "延迟：";
            // 
            // tb_delay
            // 
            this.tb_delay.Location = new System.Drawing.Point(488, 58);
            this.tb_delay.Name = "tb_delay";
            this.tb_delay.Size = new System.Drawing.Size(121, 21);
            this.tb_delay.TabIndex = 37;
            // 
            // btn_setup
            // 
            this.btn_setup.Location = new System.Drawing.Point(638, 24);
            this.btn_setup.Name = "btn_setup";
            this.btn_setup.Size = new System.Drawing.Size(93, 55);
            this.btn_setup.TabIndex = 36;
            this.btn_setup.Text = "打开端口";
            this.btn_setup.UseVisualStyleBackColor = true;
            this.btn_setup.Click += new System.EventHandler(this.btn_setup_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_CRC);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cb_CRC);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tb_read);
            this.groupBox2.Controls.Add(this.btn_clear);
            this.groupBox2.Controls.Add(this.btn_send);
            this.groupBox2.Controls.Add(this.tb_code);
            this.groupBox2.Controls.Add(this.tb_send);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(13, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 411);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modbus指令";
            // 
            // tb_CRC
            // 
            this.tb_CRC.Location = new System.Drawing.Point(448, 23);
            this.tb_CRC.Name = "tb_CRC";
            this.tb_CRC.Size = new System.Drawing.Size(67, 21);
            this.tb_CRC.TabIndex = 47;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(376, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 46;
            this.label10.Text = "CRC校验值：";
            // 
            // cb_CRC
            // 
            this.cb_CRC.AutoSize = true;
            this.cb_CRC.Location = new System.Drawing.Point(478, 56);
            this.cb_CRC.Name = "cb_CRC";
            this.cb_CRC.Size = new System.Drawing.Size(66, 16);
            this.cb_CRC.TabIndex = 45;
            this.cb_CRC.Text = "CRC校验";
            this.cb_CRC.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(394, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 44;
            this.label9.Text = "读取的数据：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 43;
            this.label8.Text = "发送的数据：";
            // 
            // tb_read
            // 
            this.tb_read.Location = new System.Drawing.Point(393, 75);
            this.tb_read.Multiline = true;
            this.tb_read.Name = "tb_read";
            this.tb_read.Size = new System.Drawing.Size(350, 313);
            this.tb_read.TabIndex = 42;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(640, 22);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 41;
            this.btn_clear.Text = "清空";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(538, 22);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 40;
            this.btn_send.Text = "发送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // tb_code
            // 
            this.tb_code.Location = new System.Drawing.Point(98, 23);
            this.tb_code.Name = "tb_code";
            this.tb_code.Size = new System.Drawing.Size(269, 21);
            this.tb_code.TabIndex = 39;
            this.tb_code.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_code_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "Modbus指令：";
            // 
            // FrmModbus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmModbus";
            this.Text = "Modbus调试工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_PortName;
        private System.Windows.Forms.ComboBox cb_BaudRate;
        private System.Windows.Forms.ComboBox cb_Parity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_send;
        private System.Windows.Forms.ComboBox cb_DataBits;
        private System.Windows.Forms.ComboBox cb_StopBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_delay;
        private System.Windows.Forms.Button btn_setup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_read;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox tb_code;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cb_CRC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_CRC;
        private System.IO.Ports.SerialPort serialPort1;
    }
}

