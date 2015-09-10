using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace YAWF
{
    public class Modbus
    {
        private SerialPort Port = new SerialPort();
        public string Status;

        // 串口状态
        public bool IsOpen { get { return this.Port.IsOpen; } }

        #region Constructor / Deconstructor
        public Modbus()
        {
        }
        ~Modbus()
        {
        }
        #endregion

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits, int readTimeOut, int WriteTimeOut)
        {
            //Ensure port isn't already opened:
            if (!Port.IsOpen)
            {
                //Assign desired settings to the serial port:
                Port.PortName = portName;
                Port.BaudRate = baudRate;
                Port.DataBits = databits;
                Port.Parity = parity;
                Port.StopBits = stopBits;
                //These timeouts are default and cannot be editted through the class at this point:
                Port.ReadTimeout = readTimeOut;
                Port.WriteTimeout = WriteTimeOut;

                try
                {
                    Port.Open();
                }
                catch (Exception err)
                {
                    Status = "串口" + portName + "打开失败: " + err.Message;
                    return false;
                }
                Status = "串口" + portName + "打开成功";
                return true;
            }
            else
            {
                Status = "串口" + portName + "已经打开过了";
                return false;
            }
        }

        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            return this.Open(portName, baudRate, databits, parity, stopBits, 10, 20);
        }

        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (Port.IsOpen)
            {
                try
                {
                    Port.Close();
                }
                catch (Exception err)
                {
                    Status = "串口" + Port.PortName + "关闭错误: " + err.Message;
                    return false;
                }
                Status = "串口" + Port.PortName + "关闭成功";
                return true;
            }
            else
            {
                Status = "串口" + Port.PortName + "已经关闭过了";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
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
        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (int i = 0; i < response.Length; i++)
            {
                response[i] = (byte)(Port.ReadByte());
            }
        }
        #endregion

        #region Function 16 - Write Multiple Registers
        public bool SendFc16(byte address, ushort start, ushort registers, short[] values)
        {
            //Ensure port is open:
            if (Port.IsOpen)
            {
                //Clear in/out buffers:
                Port.DiscardOutBuffer();
                Port.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[9 + 2 * registers];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = (byte)(values[i] >> 8);
                    message[8 + 2 * i] = (byte)(values[i]);
                }
                //Build outgoing message:
                BuildMessage(address, (byte)16, start, registers, ref message);
                
                //Send Modbus message to Serial Port:
                try
                {
                    Port.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    Status = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    Status = "Write successful";
                    return true;
                }
                else
                {
                    Status = "CRC error";
                    return false;
                }
            }
            else
            {
                Status = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 3 - Read Registers
        public bool SendFc03(ref byte[] message, ref short[] values)
        {
            byte[] response = null;
            if (this.SendFc03(ref message, ref response))
            {
                //Return requested register values:
                for (int i = 0; i < (response.Length - 5) / 2; i++)
                {
                    values[i] = response[2 * i + 3];
                    values[i] <<= 8;
                    values[i] += response[2 * i + 4];
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SendFc03(ref byte[] message, ref byte[] response)
        {
            //Ensure port is open:
            if (Port.IsOpen)
            {
                // 创建address、start、registers变量
                byte address = (byte)message[0];
                ushort start = (ushort)Convert.ToInt32(message[2].ToString("X2") + message[3].ToString("X2"), 16);        // 寄存器地址
                ushort registers = (ushort)Convert.ToInt32(message[4].ToString("X2") + message[5].ToString("X2"), 16);    // 数据字节数

                //Clear in/out buffers:
                Port.DiscardOutBuffer();
                Port.DiscardInBuffer();
                //Function 3 request is always 8 bytes:
                byte[] message02 = new byte[8];
                //Function 3 response buffer:
                byte[] response02 = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)3, start, registers, ref message02);
                // 返回message02
                message = message02;

                //Send modbus message to Serial Port:
                try
                {
                    Port.Write(message02, 0, message02.Length);
                    GetResponse(ref response02);
                }
                catch (Exception err)
                {
                    Status = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response02))
                {
                    // 返回response02
                    response = response02;
                    Status = "Read successful";
                    return true;
                }
                else
                {
                    Status = "CRC error";
                    return false;
                }
            }
            else
            {
                Status = "Serial port not open";
                return false;
            }

        }
        #endregion

    }
}
