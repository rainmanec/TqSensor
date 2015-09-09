using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MYUI
{

    public partial class Paginator: UserControl
    {
        /// <summary>
        /// 定义PageChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="total"></param>
        /// <param name="cur"></param>
        /// <param name="per"></param>
        public delegate void PageChangeHandle(object sender, int total, int cur, int per);
        public event PageChangeHandle PageChanged;

        /// <summary>
        /// 定义ExportExcel事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="total"></param>
        /// <param name="cur"></param>
        /// <param name="per"></param>
        public delegate void ExportExcelHandle(object sender, int total, int cur, int per);
        public event ExportExcelHandle ExportExcel;

        // 分页的总数量
        private int total = 0;

        /// <summary>
        /// 当前页，只读
        /// </summary>
        private int cur = 1;
        public int Cur
        {
            get
            {
                return this.cur;
            }
        }

        /// <summary>
        /// 总记录数，只读
        /// </summary>
        private int sum = 0;
        public int Sum
        {
            get
            {
                return this.sum;
            }
        }

        /// <summary>
        /// 每页显示的数量，只读
        /// </summary>
        private int per = 20;
        public int Per
        {
            get
            {
                return this.per;
            }
        }

        /// <summary>
        /// 初始化分页控件
        /// </summary>
        /// <param name="sum">总记录数</param>
        /// <param name="cur">当前页</param>
        /// <param name="per">每页数量</param>
        public void Init(int sum, int cur, int per, bool isExport)
        {
            this.per = (per < 1) ? 1 : per;
            this.sum = (sum < 1) ? 0 : sum;
            this.total = (int)Math.Ceiling((decimal)((this.sum + 0.0) / this.per));
            this.Jump(cur);
            this.btnExport.Visible = isExport;
        }

        /// <summary>
        /// 初始化分页控件
        /// </summary>
        /// <param name="sum">总记录数</param>
        /// <param name="per">每页数量</param>
        public void Init(int sum, int per)
        {
            this.Init(sum, 1, per, true);
        }

        /// <summary>
        /// 刷新当前页
        /// </summary>
        public void Refresh()
        {
            this.Jump(this.cur);
        }

        public Paginator()
        {
            InitializeComponent();
            this.Init(0, 1, 20, true);
        }

        /// <summary>
        /// 刷新按钮的状态
        /// </summary>
        private void InitInterface()
        {
            if (this.cur <= 1)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
            }
            else
            {
                this.btnFirst.Enabled = true;
                this.btnPrev.Enabled = true;
            }
            if (this.cur >= this.total)
            {
                this.btnLast.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else
            {
                this.btnLast.Enabled = true;
                this.btnNext.Enabled = true;
            }
            this.tbCur.Text = this.cur.ToString();
            this.lbInfo.Text = "共" + this.total.ToString() + "页";
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="index">需要跳转的页面</param>
        public void Jump(int index)
        {
            if (index > this.total)
            {
                index = this.total;
            }
            if (index < 1)
            {
                index = 1;
            }
            this.cur = index;
            this.InitInterface();
            if (this.PageChanged != null)
            {
                this.PageChanged(this, this.total, this.cur, this.per);
            }
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            int i;
            int.TryParse(this.tbCur.Text, out i);
            this.Jump(i);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            this.Jump(1);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.Jump(this.total);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.Jump(this.cur - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Jump(this.cur + 1);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.ExportExcel != null)
            {
                this.ExportExcel(this, this.total, this.cur, this.per);
            }
        }

        private void tbCur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int p;
                int.TryParse(tbCur.Text.Trim(), out p);
                this.Jump(p);
            }
        }


    }


}
