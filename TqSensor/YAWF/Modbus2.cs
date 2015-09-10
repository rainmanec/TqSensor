using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace YAWF
{
    public class Modbus2
    {
        private SerialPort Port = new SerialPort(); // 串口变量
        public string Msg;              // 错误或成功提示语
        public int DIR = 1;             // 串口的方向， 1 读取中； 2发送
        public int LEN = 0;             // 接收时对应的数据长度
        public decimal CharTime = 0;    // 一个字符的时间
        public System.Timers.Timer Tm = null;   // 定时器

        // 串口状态
        public bool IsOpen
        {
            get
            {
                return this.Port.IsOpen;
            }
        }

        // 超时函数
        public void TimeOutEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            this.DIR = 1;
            if (this.Port.BytesToRead > 0)
            {
                byte[] buffer = new byte[this.Port.BytesToRead];
                this.Port.Read(buffer, 0, buffer.Length);
                if (this.OnDataRreceived != null)
                {
                    this.OnDataRreceived(buffer, this.CheckCRC(buffer));
                }
                this.Port.DiscardInBuffer();
            }
            else
            {
                this.OnDataRreceived(new byte[0], false);
            }
        }

        public void Init()
        {
            int parity = (this.Port.Parity != Parity.None) ? 1 : 0;
            int stopBits = (this.Port.StopBits == StopBits.Two) ? 2 : 1;
            this.CharTime = (this.Port.DataBits + stopBits + stopBits + parity) / this.Port.BaudRate;
        }


        // 回调函数
        public delegate void DataReceivedEventHander(byte[] buffer, bool success);    // 定义一个委托
        public event DataReceivedEventHander OnDataRreceived;           // 生命一个委托变量

        public void Reset()
        {
            this.OnDataRreceived = null;
        }

        // 构造函数
        public Modbus2()
        {
            this.Port.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceivedHandler);
        }

        // DataReceived事件
        private void SerialPortDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;

            // 判断需要接收的字节数
            if (port.BytesToRead >= 2 && this.LEN == 0)
            {
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);
                byte code = buffer[1];
                if (code == 0x03)
                {
                    this.LEN = 7;
                }
                else if (code == 0x83)
                {
                    this.LEN = 4;
                }
            }
            else
            {
                return;
            }

            try
            {
                if (port.BytesToRead >= this.LEN)
                {
                    // 重置
                    this.DIR = 1;
                    this.LEN = 0;
                    this.Tm.Dispose();
                    this.Tm = null;

                    // 读取数据
                    byte[] buffer = new byte[port.BytesToRead];
                    port.Read(buffer, 0, buffer.Length);
                    if (this.OnDataRreceived != null)
                    {
                        this.OnDataRreceived(buffer, this.CheckCRC(buffer));
                    }
                    port.DiscardInBuffer();
                }
                else
                {
                    this.Tm.Stop();
                    this.Tm.Interval = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(this.CharTime * 2))) ;
                    this.Tm.Start();
                }
            }
            catch
            {
                this.DIR = 0;
            }
            //MessageBox.Show("BytesToRead:" + port.BytesToRead.ToString() + "   LEN:" + this.LEN.ToString() + "  DIR" + this.DIR);
        }

        #region 打开端口
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="databits">数据位长度</param>
        /// <param name="parity">奇偶校验</param>
        /// <param name="stopBits">停止位</param>
        /// <returns></returns>
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            if (!Port.IsOpen)
            {
                Port.PortName = portName;
                Port.BaudRate = baudRate;
                Port.DataBits = databits;
                Port.Parity = parity;
                Port.StopBits = stopBits;
                Port.ReadTimeout = -1;
                Port.WriteTimeout = 500;
                try
                {
                    Port.Open();
                }
                catch (Exception err)
                {
                    Msg = "端口" + portName + "打开失败: " + err.Message;
                    return false;
                }
                Msg = "端口" + portName + " 打开成功";
                return true;
            }
            else
            {
                Msg = "端口" + portName + " 已经打开";
                return false;
            }
        }
        
        // 关闭串口
        public bool Close()
        {
            if (Port.IsOpen)
            {
                try
                {
                    Port.Close();
                }
                catch (Exception err)
                {
                    Msg = "端口" + Port.PortName + "关闭错误: " + err.Message;
                    return false;
                }
                Msg = "端口" + Port.PortName + "成功关闭";
                return true;
            }
            else
            {
                Msg = "端口" + Port.PortName + "已经关闭";
                return false;
            }
        }
        #endregion

        #region 获取CRC校验值
        /// <summary>
        /// 获得CRC校验值
        /// </summary>
        /// <param name="message">原始byte[]</param>
        /// <returns>计算后的CRC校验值byte[2]</returns>
        public byte[] GetCRC(byte[] message)
        {
            byte[] CRC = new byte[2];
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < message.Length; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return CRC;
        }
        #endregion

        #region 检验CRC
        /// <summary>
        /// 返回的数据是否符合CRC校验
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool CheckCRC(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = GetCRC(response);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region BuildMessage

        /// <summary>
        /// 构造发送Message，不添加CRC校验，返回byte[6]
        /// </summary>
        /// <param name="address">从机地址</param>
        /// <param name="type">功能号</param>
        /// <param name="start">寄存器地址，十进制整数</param>
        /// <param name="registers">寄存器数据，十进制整数</param>
        public byte[] BuildMessage(byte address, byte type, ushort start, ushort registers)
        {
            byte[] message = new byte[6];
            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;
            return message;
        }

        /// <summary>
        /// 构造发送Message，不添加CRC校验，返回byte[6]
        /// </summary>
        /// <param name="str">字符串，不包含CRC校验</param>
        /// <returns></returns>
        public byte[] BuildMessage(string str)
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

            byte[] result = new byte[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list[i];
            }
            return result;            
        }

#endregion

        /// <summary>
        /// 打包发送数据，不需要校验位
        /// </summary>
        /// <param name="values">byte[]数组，不需要校验位</param>
        /// <returns></returns>
        public bool SendModbusData(ref byte[] values)
        {
            if (!this.Port.IsOpen)
            {
                Msg = "端口没有打开";
                return false;
            }

            if (this.DIR == 2)
            {
                Msg = "端口处于工作状态，不能发送";
                return false;
            }

            this.DIR = 2;


            //清除 in/out buffers:
            Port.DiscardOutBuffer();
            Port.DiscardInBuffer();

            //打包带有 CRC 验证的modbus 数据包:
            byte[] response = new byte[values.Length + 2];
            Array.Copy(values, response, values.Length);
            byte[] CRC = GetCRC(values);
            response[response.Length - 2] = CRC[0];
            response[response.Length - 1] = CRC[1];

            //返回带有 CRC 验证的modbus 数据包
            values = response; 

            try
            {
                // 发送指令
                Port.Write(response, 0, response.Length);
                // 读取超时处理
                this.Tm = new System.Timers.Timer();
                this.Tm.AutoReset = false;
                this.Tm.Enabled = true;
                this.Tm.Interval = 50;
                this.Tm.Elapsed += new System.Timers.ElapsedEventHandler(this.TimeOutEvent);
                Thread.Sleep(Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(this.CharTime * response.Length))));
                return true;
            }
            catch (Exception err)
            {
                Msg = "数据写入错误: " + err.Message;
                return false;
            }
            finally
            {
                this.DIR = 1;
            }
        }
    }
}