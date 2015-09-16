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

        }

        private string GetConfig(string key)
        {
            SQLiteParameter p_key = Util.NewSQLiteParameter("@p_key", DbType.String, );


        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            this.InitInterface();
        }
    }
}
