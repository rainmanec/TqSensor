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

namespace YAWF
{
    public partial class FrmPort : Form
    {
        public SerialPort _serialPort = new SerialPort();

        public bool _isBind = false;

        public Thread _readThread;
        public bool _keepReading;

        public FrmPort()
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
        }

        private void InitPort()
        {
            if (_serialPort.IsOpen)
            {
                return;
            }
            // 串口名称
            _serialPort.PortName = cb_PortName.Text.ToString();

            // 串行波特率
            _serialPort.BaudRate = Convert.ToInt32(cb_BaudRate.Text.ToString());

            // 设置奇偶校验检查协议
            if (this.cb_Parity.Text.ToString() == "偶校验")
            {
                _serialPort.Parity = Parity.Even;
            }
            else if (this.cb_Parity.Text.ToString() == "奇校验")
            {
                _serialPort.Parity = Parity.Odd;
            }
            else if (this.cb_Parity.Text.ToString() == "1")
            {
                _serialPort.Parity = Parity.Mark;
            }
            else if (this.cb_Parity.Text.ToString() == "0")
            {
                _serialPort.Parity = Parity.Space;
            }
            else
            {
                _serialPort.Parity = Parity.None;
            }

            // 设置每个字节的标准数据位长度，此属性的值范围为 5 到 8，默认值为 8。
            _serialPort.DataBits = 8;

            // 设置每个字节的标准停止位数，StopBits 的默认值为 One。
            _serialPort.StopBits = StopBits.One;

            // 设置读取操作未完成时发生超时之前的毫秒数，默认值500毫秒
            _serialPort.ReadTimeout = 500;

            // 设置写入操作未完成时发生超时之前的毫秒数，默认值500毫秒
            _serialPort.WriteTimeout = 500;
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        private bool OpenPort()
        {
            this.InitPort();
            if (!this._serialPort.IsOpen)
            {
                try
                {
                    this._serialPort.Open();
                    return true;
                }
                catch (System.Exception ex)
                {
                    // MessageBox.Show(string.Format("未能打开串口，请检查是否连接。\n" + ex.Message));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        /// <returns></returns>
        private bool ClosePort()
        {
            if (this._serialPort.IsOpen)
            {
                try
                {
                    this._serialPort.Close();
                    return true;
                }
                catch (System.Exception ex)
                {
                    //MessageBox.Show(string.Format("未能关闭串口，请检查是否连接。\n" + ex.Message));
                    return false;
                }
            }
            return true;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitInterface();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort port = (SerialPort)sender;
                if (port.BytesToRead <= 0)
                {
                    return;
                }
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);
                this.tb_receive.Invoke(
                     new MethodInvoker(
                           delegate
                           {
                               if (this.cb_16.Checked)
                               {
                                   string str = "";
                                   for (int i = 0; i < buffer.Length; i++)
                                   {
                                       str += buffer[i].ToString("X2");
                                   }
                                   tb_receive.Text += str;
                               }
                               else
                               {
                                   tb_receive.Text += Encoding.Default.GetString(buffer);
                               }
                           }
                    )
                );
            }
            catch { }            
        }


        private void btn_begin_Click(object sender, EventArgs e)
        {
            if (this._serialPort.IsOpen == false)
            {
                if (this.OpenPort())
                {
                    this.btn_begin.Text = "关闭端口";
                }
                else
                {
                    MessageBox.Show("未能打开端口");
                }
            }
            else
            {
                if (this.ClosePort())
                {
                    this.btn_begin.Text = "打开端口";
                }
                else
                {
                    MessageBox.Show("未能关闭端口");
                }
            }
        }
    }
}
