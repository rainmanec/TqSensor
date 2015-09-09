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
        /// <summary>
        /// 将String准换为Int，若str不合法则返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int IntTryParse(string str)
        {
            int i;
            int.TryParse(str, out i);
            return i;
        }
        /// <summary>
        /// 初始化界面控件
        /// </summary>
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
            }
            catch (Exception ex)
            {
                throw new Exception("获取计算机COM口列表失败!\r\n错误信息:" + ex.Message);
            }
            if (sPort.Length > 0)
            {
                this.cb_PortName.SelectedIndex = 0;
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

            // 数据位
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

        /// <summary>
        /// 重新打开端口
        /// </summary>
        private void OpenPort()
        {
            // 串口名称
            string _PortName = cb_PortName.Text.ToString();

            // 串行波特率
            int _BaudRate = Convert.ToInt32(cb_BaudRate.Text.ToString());

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

            // 设置每个字节的标准数据位长度，此属性的值范围为 5 到 8，默认值为 8。
            int _DataBits = 8;

            // 设置每个字节的标准停止位数，StopBits 的默认值为 One。
            StopBits _StopBits = StopBits.One;

            modbus.Open(_PortName, _BaudRate, _DataBits, _Particy, _StopBits);

            // delay
            int delay = this.IntTryParse(this.tb_delay.Text.Trim());
            if (delay <= 0)
            {
                this.tb_delay.Text = "0";
            }
            else
            {
                this.tb_delay.Text = delay.ToString();
            }
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
        /// 将byte[]转换为16进制字符串
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        private string ByteToString16(byte[] bt)
        {
            string str = "";
            for (int i = 0; i < bt.Length; i++)
            {
                if (i == bt.Length - 1)
                {
                    str += bt[i].ToString("X2");
                }
                else
                {
                    str += bt[i].ToString("X2") + " ";
                }
            }
            return str;
        }

        public void KANG(byte[] buffer, bool flag)
        {
            this.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        if (buffer.Length == 0)
                        {
                            this.AppendText(this.tb_read, "EMPTY");
                        }
                        else
                        {
                            this.AppendText(this.tb_read, this.ByteToString16(buffer));
                        }
                    }
                )
            );

        }

        /// <summary>
        /// 发送并接收数据
        /// </summary>
        private void SendAndGetData()
        {
            // 检测端口
            if (modbus.IsOpen == false)
            {
                MessageBox.Show("请打开端口");
                return;
            }

            // 检验指令
            byte[] code = this.modbus.BuildMessage(this.tb_code.Text.Trim());
            if (code.Length != 6)
            {
                MessageBox.Show("Modbus指令错误!");
                return;
            }

            modbus.Reset();
            modbus.OnDataRreceived += new Modbus.DataReceivedEventHander(KANG);

            this.tb_CRC.Text = this.ByteToString16(this.modbus.GetCRC(code));
            if (modbus.SendModbusData(ref code))
            {
                this.AppendText(this.tb_send, this.ByteToString16(code));
            }
            else
            {
                MessageBox.Show(this.modbus.Msg);
            }
            

            /*

            if (!modbus.SendModbusData(ref code))
            {
                this.AppendText(this.tb_send, "Send Error:" + this.ByteToString16(code));
                this.tb_send.Text = "ERROR";
            }
            else
            {
                */
            /*
            this.AppendText(this.tb_send, this.ByteToString16(code));
            int delay = this.IntTryParse(this.tb_delay.Text.Trim());
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }
            byte[] result = null;
            if (this.modbus.GetModbusData(ref result, cb_CRC.Checked))
            {
                if (result != null)
                {
                    this.AppendText(this.tb_read, this.ByteToString16(result));
                }
                else
                {
                    this.AppendText(this.tb_read, "EMPTY");
                }
            }
            else
            {
                this.AppendText(this.tb_read, "ERROR:" + this.modbus.status);
            }
        }
            */
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
                this.OpenPort();
                if (this.modbus.IsOpen == false)
                {
                    MessageBox.Show("串口打开失败");
                }
                else
                {
                    this.btn_setup.Text = "关闭端口";
                }
            }
            else
            {
                this.modbus.Close();
                if (this.modbus.IsOpen == true)
                {
                    MessageBox.Show("串口关闭失败");
                }
                else
                {
                    this.btn_setup.Text = "打开端口";
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
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
            this.SendAndGetData();
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
