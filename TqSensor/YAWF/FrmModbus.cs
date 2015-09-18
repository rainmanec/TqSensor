using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace YAWF
{
    public partial class FrmModbus : Form
    {
        public Modbus modbus = new Modbus();
        public System.Timers.Timer timer = new System.Timers.Timer();
        public System.Timers.Timer tm = new System.Timers.Timer();
        
        public FrmModbus()
        {
            InitializeComponent();
        }

        #region 辅助函数
        // 初始化界面控件
        private void InitInterface()
        {

            // 获取所有COM端口
            string[] sPort = null;
            try
            {
                sPort = SerialPort.GetPortNames();
                foreach (string pt in sPort)
                {
                    this.cb_PortName.Items.Add(pt);
                }
                if (sPort.Length > 0)
                {
                    this.cb_PortName.SelectedIndex = 0;
                }
            }
            catch
            {
                // "获取计算机COM口列表失败!\r\n错误信息:" + ex.Message
            }

            // 奇偶校验
            string[] sParity = new string[] { "不校验", "偶校验", "奇校验", "1", "0" };
            foreach (string p in sParity)
            {
                this.cb_Parity.Items.Add(p);
            }
            if (sParity.Length > 0)
            {
                this.cb_Parity.SelectedIndex = 0;
            }

            // 波特率
            string[] sBaudRate = new string[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "115200", "0" };
            foreach (string p in sBaudRate)
            {
                this.cb_BaudRate.Items.Add(p);
            }
            if (sBaudRate.Length > 5)
            {
                this.cb_BaudRate.SelectedIndex = 5;
            }

            // 数据位
            string[] sDataBits = new string[] { "8", "7", "6" };
            foreach (string p in sDataBits)
            {
                this.cb_DataBits.Items.Add(p);
            }
            if (sDataBits.Length > 0)
            {
                this.cb_DataBits.SelectedIndex = 0;
            }

            // 停止位
            string[] sStopBits = new string[] { "1", "2" };
            foreach (string p in sStopBits)
            {
                this.cb_StopBits.Items.Add(p);
            }
            if (sStopBits.Length > 0)
            {
                this.cb_StopBits.SelectedIndex = 0;
            }

            // 读取超时
            this.tb_delay.Text = "10";
        }

        // 重新打开端口
        private bool OpenPort()
        {
            // 串口名称
            string _PortName = cb_PortName.Text.ToString();

            // 串行波特率
            int _BaudRate = Util.IntTryParse(cb_BaudRate.Text.ToString());

            // 设置奇偶校验检查协议
            Parity _Particy;
            if (this.cb_Parity.Text.ToString() == "偶校验")
            {
                _Particy = Parity.Even;
            }
            else if (this.cb_Parity.Text.ToString() == "奇校验")
            {
                _Particy = Parity.Odd;
            }
            else if (this.cb_Parity.Text.ToString() == "1")
            {
                _Particy = Parity.Mark;
            }
            else if (this.cb_Parity.Text.ToString() == "0")
            {
                _Particy = Parity.Space;
            }
            else
            {
                _Particy = Parity.None;
            }

            // 数据位：设置每个字节的标准数据位长度，此属性的值范围为 5 到 8，默认值为 8。
            int _DataBits = Util.IntTryParse(this.cb_DataBits.Text);

            // 停止位：设置每个字节的标准停止位数，StopBits 的默认值为 One。
            StopBits _StopBits = StopBits.One;
            if (this.cb_StopBits.Text.Trim() != "1")
            {
                _StopBits = StopBits.Two;
            }

            // ReadTimeOut && WriteTimeOut
            int delay = Util.IntTryParse(this.tb_delay.Text);
            delay = (delay <= 0) ? 10 : delay;
            this.tb_delay.Text = delay.ToString();

            // 打开串口
            return modbus.Open(_PortName, _BaudRate, _DataBits, _Particy, _StopBits, delay, delay);
            
        }

        /// <summary>
        /// 向TextBox添加一行文字
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="str"></param>
        private void AppendText(TextBox tb, string str)
        {
            if (tb.Text.Trim() == "")
            {
                tb.Text = str;
            }
            else
            {
                tb.Text += "\r\n" + str; ;
            }
        }

        /// <summary>
        /// String字符串转换成byte[6]，不添加CRC校验
        /// </summary>
        /// <param name="str">字符串，不包含CRC校验</param>
        /// <returns></returns>
        public byte[] BuildCode(string str)
        {
            string[] code = str.Split(' ');
            List<byte> list = new List<byte>();
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i].Trim().Length == 2)
                {
                    int j = -1;
                    try
                    {
                        j = Convert.ToInt32(code[i], 16);   // 16进制转为10进制
                    }
                    catch { }
                    if (j != -1)
                    {
                        list.Add((byte)j);
                    }
                }
            }

            int length = list.Count > 6 ? 6 : list.Count;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

        /// <summary>
        /// 发送并接收数据
        /// </summary>
        private void SendAndGetData()
        {
            // 检测端口
            if (modbus.IsOpen == false)
            {
                this.AppendText(this.tb_send, "请打开端口");
                return;
            }

            // 检验指令
            byte[] message = this.BuildCode(this.tb_code.Text.Trim());
            if (message.Length != 6)
            {
                MessageBox.Show("Modbus指令错误!");
                return;
            }

            byte[] response = null;
            if (this.modbus.SendFc03(ref message, ref response))
            {
                // 发送接收Log
                this.AppendText(this.tb_send, Util.ByteToStr16(message));
                this.AppendText(this.tb_read, Util.ByteToStr16(response));
                // 重置文本框
                byte[] code = new byte[message.Length - 2];
                Array.Copy(message, code, message.Length - 2);
                byte[] CRC = new byte[2];
                CRC[0] = message[message.Length - 2];
                CRC[1] = message[message.Length - 1];
                this.tb_CRC.Text = Util.ByteToStr16(CRC);
                this.tb_code.Text = Util.ByteToStr16(code);
            }
            else
            {
                this.AppendText(this.tb_send, Util.ByteToStr16(message));
                this.AppendText(this.tb_read, this.modbus.Status);
            }
        }
        
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitInterface();
        }

        private void btn_setup_Click(object sender, EventArgs e)
        {
            string txt = this.btn_setup.Text.Trim();
            if (txt == "打开端口")
            {
                if (!this.OpenPort())
                {
                    MessageBox.Show(this.modbus.Status);
                }
                else
                {
                    this.btn_setup.Text = "关闭端口";
                    this.cb_PortName.Enabled = false;
                    this.cb_BaudRate.Enabled = false;
                    this.cb_DataBits.Enabled = false;
                    this.cb_Parity.Enabled = false;
                    this.cb_StopBits.Enabled = false;
                    this.tb_delay.Enabled = false;
                }
            }
            else
            {
                if (!this.modbus.Close())
                {
                    MessageBox.Show("串口关闭失败");
                }
                else
                {
                    this.btn_setup.Text = "打开端口";
                    this.cb_PortName.Enabled = true;
                    this.cb_BaudRate.Enabled = true;
                    this.cb_DataBits.Enabled = true;
                    this.cb_Parity.Enabled = true;
                    this.cb_StopBits.Enabled = true;
                    this.tb_delay.Enabled = true;
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            this.tb_send.Text = "";
            this.tb_read.Text = "";
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            int length = 1000;
            for (int i = 0; i < length; i++)
            {
                this.SendAndGetData();
            }
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            double per = (double)ts.TotalMilliseconds / (double)100;
            MessageBox.Show(string.Format("公用{0}毫秒，发送了{1}条数据，平均用时{2}毫秒", ts.TotalMilliseconds.ToString(), length.ToString(), per.ToString()));
        }

        private void tb_code_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                this.SendAndGetData();
            }
        }


    }
}
