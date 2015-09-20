using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace YAWF
{
    public partial class FrmConfig : Form
    {

        public FrmConfig()
        {
            InitializeComponent();
        }

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

            // 文本框的值
            this.tb_Kw_Range_Begin.Text = Util.GetTbConfig("Kw_Range_Begin");
            this.tb_Kw_Range_End.Text = Util.GetTbConfig("Kw_Range_End");
            this.tb_Nm_Range_Begin.Text = Util.GetTbConfig("Nm_Range_Begin");
            this.tb_Nm_Range_End.Text = Util.GetTbConfig("Nm_Range_End");
            this.tb_Rad_Range_Begin.Text = Util.GetTbConfig("Rad_Range_Begin");
            this.tb_Rad_Range_End.Text = Util.GetTbConfig("Rad_Range_End");
            this.tb_Nm_Modbus_Code.Text = Util.GetTbConfig("Nm_Modbus_Code");
            this.tb_Rad_Modbus_Code.Text = Util.GetTbConfig("Rad_Modbus_Code");
            this.tb_ReadSpeed.Text = Util.GetTbConfig("ReadSpeed");

            //ComboBox的值
            this.cb_PortName.Text = Util.GetTbConfig("PortName");
            this.cb_Parity.Text = Util.GetTbConfig("Parity");
            this.cb_BaudRate.Text = Util.GetTbConfig("BaudRate");
            this.cb_DataBits.Text = Util.GetTbConfig("DataBits");
            this.cb_StopBits.Text = Util.GetTbConfig("StopBits");
        }


        private void FrmConfig_Load(object sender, EventArgs e)
        {
            this.InitInterface();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            // Modbus检测
            if (this.tb_Nm_Modbus_Code.Text.Trim() != "")
            {
                byte[] code = Util.BuildCode(this.tb_Nm_Modbus_Code.Text);
                if (code.Length != 6)
                {
                    MessageBox.Show("扭矩Modbus指令不正确！");
                    return;
                }
                else
                {
                    this.tb_Nm_Modbus_Code.Text = Util.ByteToStr16(code);
                }
            }
            if (this.tb_Rad_Modbus_Code.Text.Trim() != "")
            {
                byte[] code = Util.BuildCode(this.tb_Rad_Modbus_Code.Text);
                if (code.Length != 6)
                {
                    MessageBox.Show("转速Modbus指令不正确！");
                    return;
                }
                else
                {
                    this.tb_Rad_Modbus_Code.Text = Util.ByteToStr16(code);
                }
            }


            // 文本框的值
            this.tb_Nm_Range_Begin.Text = Util.IntTryParse(this.tb_Nm_Range_Begin.Text).ToString();
            this.tb_Nm_Range_End.Text = Util.IntTryParse(this.tb_Nm_Range_End.Text).ToString();
            this.tb_Rad_Range_Begin.Text = Util.IntTryParse(this.tb_Rad_Range_Begin.Text).ToString();
            this.tb_Rad_Range_End.Text = Util.IntTryParse(this.tb_Rad_Range_End.Text).ToString();
            this.tb_Kw_Range_Begin.Text = Util.IntTryParse(this.tb_Kw_Range_Begin.Text).ToString();
            this.tb_Kw_Range_End.Text = Util.IntTryParse(this.tb_Kw_Range_End.Text).ToString();
            try
            {
                Util.SetTbConfig("Nm_Range_Begin", this.tb_Nm_Range_Begin.Text);
                Util.SetTbConfig("Nm_Range_End", this.tb_Nm_Range_End.Text);
                Util.SetTbConfig("Rad_Range_Begin", this.tb_Rad_Range_Begin.Text);
                Util.SetTbConfig("Rad_Range_End", this.tb_Rad_Range_End.Text);
                Util.SetTbConfig("Kw_Range_Begin", this.tb_Kw_Range_Begin.Text);
                Util.SetTbConfig("Kw_Range_End", this.tb_Kw_Range_End.Text);
                Util.SetTbConfig("Nm_Modbus_Code", this.tb_Nm_Modbus_Code.Text);
                Util.SetTbConfig("Rad_Modbus_Code", this.tb_Rad_Modbus_Code.Text);

                //ComboBox的值
                Util.SetTbConfig("PortName", this.cb_PortName.Text);
                Util.SetTbConfig("Parity", this.cb_Parity.Text);
                Util.SetTbConfig("BaudRate", this.cb_BaudRate.Text);
                Util.SetTbConfig("DataBits", this.cb_DataBits.Text);
                Util.SetTbConfig("StopBits", this.cb_StopBits.Text);
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("保存失败");
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
