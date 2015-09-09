namespace YAWF
{
    partial class FrmPort
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
            this.cb_PortName = new System.Windows.Forms.ComboBox();
            this.cb_BaudRate = new System.Windows.Forms.ComboBox();
            this.cb_Parity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_begin = new System.Windows.Forms.Button();
            this.tb_receive = new System.Windows.Forms.TextBox();
            this.cb_DataBits = new System.Windows.Forms.ComboBox();
            this.cb_StopBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_16 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_PortName
            // 
            this.cb_PortName.FormattingEnabled = true;
            this.cb_PortName.Location = new System.Drawing.Point(76, 24);
            this.cb_PortName.Name = "cb_PortName";
            this.cb_PortName.Size = new System.Drawing.Size(121, 20);
            this.cb_PortName.TabIndex = 0;
            // 
            // cb_BaudRate
            // 
            this.cb_BaudRate.FormattingEnabled = true;
            this.cb_BaudRate.Location = new System.Drawing.Point(76, 53);
            this.cb_BaudRate.Name = "cb_BaudRate";
            this.cb_BaudRate.Size = new System.Drawing.Size(121, 20);
            this.cb_BaudRate.TabIndex = 1;
            // 
            // cb_Parity
            // 
            this.cb_Parity.FormattingEnabled = true;
            this.cb_Parity.Location = new System.Drawing.Point(274, 24);
            this.cb_Parity.Name = "cb_Parity";
            this.cb_Parity.Size = new System.Drawing.Size(121, 20);
            this.cb_Parity.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "串口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "波特率：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "校验位：";
            // 
            // btn_begin
            // 
            this.btn_begin.Location = new System.Drawing.Point(636, 24);
            this.btn_begin.Name = "btn_begin";
            this.btn_begin.Size = new System.Drawing.Size(105, 51);
            this.btn_begin.TabIndex = 5;
            this.btn_begin.Text = "打开端口";
            this.btn_begin.UseVisualStyleBackColor = true;
            this.btn_begin.Click += new System.EventHandler(this.btn_begin_Click);
            // 
            // tb_receive
            // 
            this.tb_receive.Location = new System.Drawing.Point(11, 20);
            this.tb_receive.Multiline = true;
            this.tb_receive.Name = "tb_receive";
            this.tb_receive.Size = new System.Drawing.Size(735, 380);
            this.tb_receive.TabIndex = 7;
            // 
            // cb_DataBits
            // 
            this.cb_DataBits.FormattingEnabled = true;
            this.cb_DataBits.Location = new System.Drawing.Point(274, 54);
            this.cb_DataBits.Name = "cb_DataBits";
            this.cb_DataBits.Size = new System.Drawing.Size(121, 20);
            this.cb_DataBits.TabIndex = 3;
            // 
            // cb_StopBits
            // 
            this.cb_StopBits.FormattingEnabled = true;
            this.cb_StopBits.Location = new System.Drawing.Point(481, 24);
            this.cb_StopBits.Name = "cb_StopBits";
            this.cb_StopBits.Size = new System.Drawing.Size(121, 20);
            this.cb_StopBits.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 34;
            this.label5.Text = "数据位：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(422, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "停止位：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_16);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cb_Parity);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cb_PortName);
            this.groupBox1.Controls.Add(this.btn_begin);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cb_BaudRate);
            this.groupBox1.Controls.Add(this.cb_StopBits);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_DataBits);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 100);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口设置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(434, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "数据：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_receive);
            this.groupBox2.Location = new System.Drawing.Point(13, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 413);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "实时数据";
            // 
            // cb_16
            // 
            this.cb_16.AutoSize = true;
            this.cb_16.Location = new System.Drawing.Point(481, 55);
            this.cb_16.Name = "cb_16";
            this.cb_16.Size = new System.Drawing.Size(72, 16);
            this.cb_16.TabIndex = 41;
            this.cb_16.Text = "十六进制";
            this.cb_16.UseVisualStyleBackColor = true;
            // 
            // FrmPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmPort";
            this.Text = "端口测试工具";
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
        private System.Windows.Forms.Button btn_begin;
        private System.Windows.Forms.TextBox tb_receive;
        private System.Windows.Forms.ComboBox cb_DataBits;
        private System.Windows.Forms.ComboBox cb_StopBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cb_16;
    }
}

