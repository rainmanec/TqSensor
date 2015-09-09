using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YAWF
{
    public partial class FrmMain : Form
    {

        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行登录和版本检测
        /// </summary>
        private void FrmMain_Load(object sender, EventArgs e)
        {
                this.WindowState = FormWindowState.Maximized;
        }

        private void modbus调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmModbus frm = new FrmModbus();
            //frm.WindowState = FormWindowState.Maximized;
            frm.MdiParent = this;
            frm.Show();
        }

        private void 串口调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPort frm = new FrmPort();
            //frm.WindowState = FormWindowState.Maximized;
            frm.MdiParent = this;
            frm.Show();
        }

    
    }
}
