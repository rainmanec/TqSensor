namespace YAWF
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_logout = new System.Windows.Forms.ToolStripMenuItem();
            this.modbus调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.串口调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_logout,
            this.modbus调试ToolStripMenuItem,
            this.串口调试ToolStripMenuItem,
            this.设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(894, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_logout
            // 
            this.tsmi_logout.Name = "tsmi_logout";
            this.tsmi_logout.Size = new System.Drawing.Size(12, 21);
            // 
            // modbus调试ToolStripMenuItem
            // 
            this.modbus调试ToolStripMenuItem.Name = "modbus调试ToolStripMenuItem";
            this.modbus调试ToolStripMenuItem.Size = new System.Drawing.Size(93, 21);
            this.modbus调试ToolStripMenuItem.Text = "Modbus调试";
            this.modbus调试ToolStripMenuItem.Click += new System.EventHandler(this.modbus调试ToolStripMenuItem_Click);
            // 
            // 串口调试ToolStripMenuItem
            // 
            this.串口调试ToolStripMenuItem.Name = "串口调试ToolStripMenuItem";
            this.串口调试ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.串口调试ToolStripMenuItem.Text = "串口调试";
            this.串口调试ToolStripMenuItem.Click += new System.EventHandler(this.串口调试ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 363);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "{0}管理系统";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_logout;
        private System.Windows.Forms.ToolStripMenuItem modbus调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 串口调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;

    }
}

