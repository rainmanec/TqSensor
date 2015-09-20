using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;

namespace YAWF
{
    public partial class FormDetect : Form
    {
        public FormDetect()
        {
            InitializeComponent();
        }

        public int date = 0;
        public bool IsFirstPoint = false;   // 默认Point;

        public int max_nm = 0;
        public int min_nm = 0;
        public int max_rad = 0;
        public int min_rad = 0;
        public int max_kw = 0;
        public int min_kw = 0;
        public List<double> list_nm = new List<double>();
        public List<double> list_rad = new List<double>();
        public List<double> list_kw = new List<double>();

        private void Form2_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = this.max_nm;
            chart1.ChartAreas[0].AxisY.Minimum = this.min_nm;
            chart1.ChartAreas[0].AxisY.Title = "扭矩";
            chart1.ChartAreas[0].AxisY2.Maximum = this.max_rad;
            chart1.ChartAreas[0].AxisY2.Minimum = this.min_rad;
            chart1.ChartAreas[0].AxisY2.Title = "转速";

            CreateYAxis(chart1, chart1.Series["kw"], 5000, 1000, 12, "功率");
            try
            {
                chart1.Series[0].Points.AddXY(0, 0);
                this.IsFirstPoint = true;
            }
            catch
            {
                this.IsFirstPoint = false;
            }
        }

        #region 辅助函数

        private void InitConfig()
        {
            this.min_nm = Util.IntTryParse(Util.GetTbConfig("Nm_Range_Begin"));
            this.max_nm = Util.IntTryParse(Util.GetTbConfig("Nm_Range_End"));
            this.min_nm = Util.IntTryParse(Util.GetTbConfig("Rad_Range_Begin"));
            this.max_nm = Util.IntTryParse(Util.GetTbConfig("Rad_Range_End"));
            this.min_kw = Util.IntTryParse(Util.GetTbConfig("Kw_Range_Begin"));
            this.max_kw = Util.IntTryParse(Util.GetTbConfig("Kw_Range_End"));
        }

        public double[] AnalyzeDataList(List<double> list)
        {
            double min = 0, max = 0, avg = 0, sum = 0, now = 0;
            foreach (double v in list)
            {
                if (v < min) { min = v; }
                if (v > max) { max = v; }
                sum += v;
            }
            if (list.Count > 0)
            {
                avg = sum / list.Count;
                now = list[list.Count - 1];
            }
            return new double[] {max, min, avg, now};
        }

        /// <summary>
        /// 创建Y轴坐标
        /// </summary>
        /// <param name="chart">原始图标Chart</param>
        /// <param name="series">原始数据线Series</param>
        /// <param name="offsetLeft">向右侧的偏移值</param>
        /// <param name="title">坐标轴标题</param>
        public void CreateYAxis(Chart chart, Series series, double maxinum, double minimun, float offsetLeft, string title)
        {
            // .1 创建新的ChartArea
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());
            areaAxis.AxisY2.Enabled = AxisEnabled.True;

            // 1.2 创建一份数据线series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            // 1.3 将复制的数据线series隐藏掉
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;   // 数据线绑定到新建的chartArea上
            // 1.4 设置Y轴的最大和最小值，如果max=min则不设定
            if (maxinum != minimun)
            {
                areaAxis.AxisY2.Maximum = maxinum;
                areaAxis.AxisY2.Minimum = minimun;
            }
            // 1.5 位CharArea填充数据
            if (series.Points.Count > 0)
            {
                foreach (DataPoint point in series.Points)
                {
                    seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                }
            }
            else
            {
                seriesCopy.Points.AddXY(chart.ChartAreas[series.ChartArea].AxisX.Minimum, maxinum);
            }

            // 2.4 隐藏掉网格线
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.LineWidth = 0;
            areaAxis.AxisY.LabelStyle.Enabled = false;
            areaAxis.AxisY.MinorGrid.Enabled = false;
            areaAxis.AxisY.MinorTickMark.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.MajorTickMark.Enabled = false;
            areaAxis.AxisY2.MinorGrid.Enabled = false;
            areaAxis.AxisY2.MinorTickMark.Enabled = true;
            areaAxis.AxisY2.MajorGrid.Enabled = false;
            areaAxis.AxisY2.MajorTickMark.Enabled = true;
            areaAxis.AxisY2.LabelStyle.Enabled = true;
            if (title.Trim() != "")
            {
                areaAxis.AxisY2.Title = title;
            }

            areaAxis.AxisY2.LabelStyle.Font = chart.ChartAreas[series.ChartArea].AxisY.LabelStyle.Font;
            areaAxis.Position.Width = chart.ChartAreas[series.ChartArea].Position.Width + offsetLeft;
        }

        /// <summary>
        /// 创建Y轴坐标
        /// </summary>
        /// <param name="chart">原始图标Chart</param>
        /// <param name="series">原始数据线Series</param>
        /// <param name="axisOffset">向右侧的偏移值</param>
        /// <param name="title">坐标轴标题</param>
        public void CreateYAxis(Chart chart, Series series, float offsetLeft, string title)
        {
            this.CreateYAxis(chart, series, 0, 0, offsetLeft, title);
        }

        /// <summary>
        /// 创建Y轴坐标
        /// </summary>
        /// <param name="chart">原始图标Chart</param>
        /// <param name="series">原始数据线Series</param>
        /// <param name="axisOffset">向右侧的偏移值</param>
        public void CreateYAxis(Chart chart, Series series, float offsetLeft)
        {
            this.CreateYAxis(chart, series, 0, 0, offsetLeft, "");
        }

        /// <summary>
        /// 多Y轴数值转换
        /// </summary>
        /// <param name="v">值</param>
        /// <param name="max">该值所在范围的最大值</param>
        /// <param name="min">该值所在范围的最小值</param>
        /// <param name="max2">目标范围的最大值</param>
        /// <param name="min2">目标范围的最小值</param>
        /// <returns></returns>
        public double ConvertY2YAxis(double v, double max, double min, double max2, double min2)
        {
            if (max < min)
            {
                double tmp;
                tmp = max;
                max = min;
                min = tmp;
            }
            if (max2 < min2)
            {
                double tmp;
                tmp = max2;
                max2 = min2;
                min2 = tmp;
            }

            if (max == min)
            {
                return (max2 + min2) / 2;
            }
            else
            {
                return (max2 - min2) * (v - min) / (max - min) + min2;
            }
        }
        #endregion

        private double[,] GetSensorValue()
        {
            double[,] table = new double[3, 2];
            Random rd = new Random();
            table[0, 0] = date;
            table[1, 0] = date;
            table[2, 0] = date;
            table[0, 1] = rd.Next(8000, 8200);
            table[1, 1] = rd.Next(5, 100);
            table[2, 1] = rd.Next(1000, 5000);
            date++;
            return table;
        }
        private void caculate()
        {
            double[] aly_nm = this.AnalyzeDataList(this.list_nm);
            this.tb_nm_max.Text = aly_nm[0].ToString(("#0.00"));
            this.tb_nm_min.Text = aly_nm[1].ToString(("#0.00"));
            this.tb_nm_avg.Text = aly_nm[2].ToString(("#0.00"));
            this.tb_nm_rt.Text = aly_nm[3].ToString(("#0.00"));
            double[] aly_rad = this.AnalyzeDataList(this.list_nm);
            this.tb_rad_max.Text = aly_rad[0].ToString(("#0.00"));
            this.tb_rad_min.Text = aly_rad[1].ToString(("#0.00"));
            this.tb_rad_avg.Text = aly_rad[2].ToString(("#0.00"));
            this.tb_rad_rt.Text = aly_rad[3].ToString(("#0.00"));
            double[] aly_kw = this.AnalyzeDataList(this.list_nm);
            this.tb_kw_max.Text = aly_kw[0].ToString(("#0.00"));
            this.tb_kw_min.Text = aly_kw[1].ToString(("#0.00"));
            this.tb_kw_avg.Text = aly_kw[2].ToString(("#0.00"));
            this.tb_kw_rt.Text = aly_kw[3].ToString(("#0.00"));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.IsFirstPoint == true)
            {
                chart1.Series[0].Points.RemoveAt(0);
                this.IsFirstPoint = false;
            }

            double[,] tb = this.GetSensorValue();
            
            // 绘图
            chart1.Series["nm"].Points.AddXY(tb[0, 0], tb[0, 1]);
            chart1.Series["rad"].Points.AddXY(tb[1, 0], this.ConvertY2YAxis(tb[1, 1], max_rad, min_rad, max_nm, min_nm));
            chart1.Series["kw"].Points.AddXY(tb[2, 0], this.ConvertY2YAxis(tb[2, 1], max_kw, min_kw, max_nm, min_nm));

            // 添加
            this.list_nm.Add(tb[0, 1]);
            this.list_rad.Add(tb[1, 1]);
            this.list_kw.Add(tb[2, 1]);

            // 统计
            this.caculate();

            if (date > 40)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Position = date - 40;
            }

        }

        private void btn_begin_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void btn_cal_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            foreach (Series s in chart1.Series)
            {
                s.Points.Clear();
            }
            try
            {
                chart1.Series[0].Points.AddXY(0, 0);
                this.IsFirstPoint = true;
            }
            catch
            {
                this.IsFirstPoint = false;
            }
            finally
            {
                date = 0;
                this.list_nm = new List<double>();
                this.list_rad = new List<double>();
                this.list_kw = new List<double>();
                chart1.ChartAreas[0].AxisX.ScaleView.Position = 0;
                this.caculate();
            }
        }

    }
}
