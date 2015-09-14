using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Management;
using System.Data.SQLite;

namespace YAWF
{
    partial class Util
    {

        public static string[] CHARS = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string[] NUMBS = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        #region 通用函数-数据库相关
        /// <summary>
        /// 生成SqlParameter
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SQLiteParameter NewSQLiteParameter(string pname, DbType dbType, object value)
        {
            SQLiteParameter parm = new SQLiteParameter(pname, dbType);
            if (dbType == DbType.DateTime)
            {
                //parm.Value = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.fff001");
                parm.Value = value;
            }
            else if (dbType == DbType.Decimal)
            {
                decimal d;
                if (Decimal.TryParse(value.ToString().Trim(), out d))
                {
                    parm.Value = d;
                }
                else
                {
                    if (value.ToString().Trim() == "")
                    {
                        parm.Value = DBNull.Value;
                    }
                    else
                    {
                        parm.Value = (decimal)0;
                    }
                }
            }
            else
            {
                parm.Value = value;
            }
            return parm;
        }
        /// <summary>
        /// 生成SqlParameter
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlParameter NewSqlParameter(string pname, SqlDbType dbType, object value)
        {
            SqlParameter parm = new SqlParameter(pname, dbType);
            if (dbType == SqlDbType.DateTime)
            {
                //parm.Value = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.fff001");
                parm.Value = value;
            }
            else if (dbType == SqlDbType.Decimal)
            {
                decimal d;
                if (Decimal.TryParse(value.ToString().Trim(), out d))
                {
                    parm.Value = d;
                }
                else
                {
                    if (value.ToString().Trim() == "")
                    {
                        parm.Value = DBNull.Value;
                    }
                    else
                    {
                        parm.Value = (decimal)0;
                    }
                }
            }
            else
            {
                parm.Value = value;
            }
            return parm;
        }

        /// <summary>
        /// 生成SqlParameter
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SqlParameter NewSqlParameter(string pname, SqlDbType dbType, object value, int size)
        {
            SqlParameter parm = new SqlParameter(pname, dbType, size);
            if (dbType == SqlDbType.VarChar)
            {
                parm.Value = value.ToString().Trim();
            }
            else if (dbType == SqlDbType.Date)
            {
                parm.SqlDbType = SqlDbType.VarChar;
                parm.Value = Util.DateTryParse(value.ToString().Trim());
            }
            else
            {
                parm.Value = value;
            }
            return parm;
        }


        /// <summary>
        /// SQL SERVER 2008 分页功能
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="col">ROW_NUMBER()列的名称，一般为“序号”</param>
        /// <param name="cur">当前页</param>
        /// <param name="per">每页数量</param>
        /// <returns></returns>
        public static string PaginatorSQL(string sql, string col, int cur, int per)
        {
            int begin = (cur - 1) * per + 1;
            int end = cur * per;
            return string.Format("SELECT * FROM ({0}) AS TTBB WHERE {1} BETWEEN {2} and {3}", sql, col, begin.ToString(), end.ToString());
        }

        /// <summary>
        /// SQL SERVER 2008 分页功能
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="cur">当前页</param>
        /// <param name="per">每页数量</param>
        /// <returns></returns>
        public static string PaginatorSQL(string sql, int cur, int per)
        {
            return Util.PaginatorSQL(sql, "序号", cur, per);
        }

        #endregion


        #region 通用函数-类型转换
        /// <summary>
        /// 将String转换为Decimal，若str不合法则返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal DecimalTryParse(string str)
        {
            decimal d;
            Decimal.TryParse(str, out d);
            return d;
        }

        /// <summary>
        /// 将String转换为DateTime，若str不合法则返回"0001/1/1 0:00:00"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime DateTimeTryParse(string str)
        {
            DateTime d;
            DateTime.TryParse(str, out d);
            return d;
        }

        /// <summary>
        /// 将“日期”字符（如20140101）串转换为"2014-01-01"格式，若str不合法则返回""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DateTryParse(string str)
        {
            DateTime d;
            if (str.Length == 8)
            {
                str = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            }
            if (DateTime.TryParse(str, out d))
            {
                return d.ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将String准换为Int，若str不合法则返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int IntTryParse(string str)
        {
            int i;
            int.TryParse(str.Trim(), out i);
            return i;
        }

        /// <summary>
        /// 返回DateTime对应的星期几
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetWeekDay(DateTime time)
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(time.DayOfWeek)];
            return week;
        }

        /// <summary>
        /// 提取字符串中的Int部分，若str不合法则返回0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseInt(string str)
        {
            int index = str.IndexOf('.');
            if (index < 0)
            {
                index = str.Length;
            }
            return Util.IntTryParse(str.Substring(0, index));
        }

        /// <summary>
        /// 判断String能否转换为Decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimal(string str)
        {
            decimal d;
            return Decimal.TryParse(str.Trim(), out d);
        }

        /// <summary>
        /// 将字符串转化为，人民币形式，如¥2.12
        /// </summary>
        /// <param name="rmb"></param>
        /// <returns></returns>
        public static string StrToRMB(string rmb)
        {
            if (rmb.Trim() == "")
            {
                return "";
            }
            else
            {
                return Util.DecimalTryParse(rmb).ToString("c");
            }
        }

        /// <summary>
        /// 将字符串转化为，货币形式，如2.12，不包含¥
        /// </summary>
        /// <param name="hb"></param>
        /// <returns></returns>
        public static string StrToHB(string hb)
        {
            string tmp = Util.StrToRMB(hb);
            if (tmp == "")
            {
                return tmp;
            }
            else
            {
                return tmp.Substring(1);
            }
        }

        /// <summary>
        /// 将十进制byte[]转换为16进制字符串
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        public static string ByteToStr16(byte[] bt)
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

        #endregion


        #region 通用函数

        /// <summary>
        /// 获取1970-01-01至dt的毫秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public long GetTime(DateTime dt)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dt.Ticks - dt1970.Ticks) / 1000;
        }

        /// <summary>
        /// 根据毫秒数计算日期
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public DateTime NewDate(long ms)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = dt1970.Ticks + ms * 1000;
            return new DateTime(t);
        }
        /// <summary>
        /// Md5加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(str));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            str = tmp.ToString().ToUpper();
            return str;
        }

        /// <summary>
        /// 获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetMacAddress()
        {
            
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            List<string> list = new List<string>();
            foreach (ManagementObject mo in moc)
            {
                if (mo["MacAddress"] != null)
                {
                    list.Add(mo["MacAddress"].ToString().Replace('-', ':').ToUpper());
                }
            }
            return Util.ListToArray(list);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string encode(string str)
        {
            string htext = "";

            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + 10 - 1 * 2);
            }
            return htext;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string decode(string str)
        {
            string dtext = "";
            if (str != "")
            {
                for (int i = 0; i < str.Length; i++)
                {
                    dtext = dtext + (char)(str[i] - 10 + 1 * 2);
                }
            }
            return dtext;
        }

        /// <summary>
        /// 返回TextBox.Value去除空白的值
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string TextBoxValue(TextBox tb)
        {
            return tb.Text.Trim();
        }

        /// <summary>
        /// 返回TextBox.Value转化为日期形式（“1988-01-01”）后的值；不合法返回“1900-00-00”
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static string TextBoxToDateString(TextBox tb)
        {
            return Util.DateTryParse(tb.Text.Trim());
        }

        /// <summary>
        /// 返回TextBox.Value转化为Decimal后的值，不合法返回0
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static decimal TextBoxValueToDecimal(TextBox tb)
        {
            return Util.DecimalTryParse(tb.Text.Trim());
        }


        /// <summary>
        /// 获取字符串的全拼
        /// </summary>
        /// <param name="text">获取文本框数值</param>
        /// <param name="only">只返回汉字部分</param>  
        /// <returns>返回全拼</returns>
        public static string GetFullPinYin(string text, bool only)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char ch in text)
            {
                HanZi hzi = Chinese.GetHanZi(ch);
                if (hzi == null)
                {
                    if(only == false) sb.Append(ch);
                }
                else
                {
                    sb.Append(hzi.PinYin);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的全拼，排除非中文字符
        /// </summary>
        /// <param name="text">获取文本框数值</param>
        /// <returns>返回全拼</returns>
        public static string GetFullPinYin(string text)
        {
            return Util.GetFullPinYin(text, true);
        }

        /// <summary>
        /// 获取汉字首字母
        /// </summary>
        /// <param name="text">获取文本框数值</param>
        /// <param name="only">只返回汉字部分</param>  
        /// <returns>返回每个汉字首字母</returns>
        public static string GetFirstPinYin(string text, bool only)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char ch in text)
            {
                HanZi hz = Chinese.GetHanZi(ch);
                if (hz == null)
                {
                    if (only == false) sb.Append(ch);
                }
                else
                {
                    sb.Append(hz.FirstPinYin);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取汉字首字母，排除非汉字字符
        /// </summary>
        /// <param name="text">获取文本框数值</param>
        /// <returns>返回每个汉字首字母</returns>
        public static string GetFirstPinYin(string text)
        {
            return Util.GetFirstPinYin(text, false);
        }

        /// <summary>
        /// 模糊搜索的值，用于Like
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetLikeValue(params string[] strs)
        {
            for(int i = 0; i < strs.Length; i++)
            {
                string str = strs[i].ToUpper();
                string str_spell = Util.GetFirstPinYin(str);
                string str_char = "";
                string str_numb = "";
                for (int j = 0; j < str.Length; j++)
                {
                    string cp = str[j].ToString();
                    if (Util.CHARS.Contains(cp))
                    {
                        str_char += cp;
                    }
                    else if (Util.NUMBS.Contains(cp))
                    {
                        str_char += cp;
                        str_numb += cp;
                    }
                }
                strs[i] = str + ";" + str_spell + ";" + str_char + ";" + str_numb;
            }
            return Util.ArrayJoin(";", strs);
        }

        /// <summary>
        /// List拼接成字符串
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ListJoin(string sp, List<string> arr) 
        {
            string result = "";
            for (int i = 0, length = arr.Count; i < length; i++)
            {
                result += arr[i];
                if (i != length - 1)
                {
                    result += sp;
                }
            }
            return result;
        }

        /// <summary>
        /// Array拼接成字符串
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string ArrayJoin(string sp, string[] arr)
        {
            string result = "";
            for (int i = 0, length = arr.Length; i < length; i++)
            {
                result += arr[i];
                if (i != length - 1)
                {
                    result += sp;
                }
            }
            return result;
        }

        /// <summary>
        /// List转换为Array
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static SqlParameter[] ListToArray(List<SqlParameter> list)
        {
            SqlParameter[] result = new SqlParameter[list.Count];
            for (int i = 0, length = list.Count; i < length; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

        /// <summary>
        /// List转换为Array
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string[] ListToArray(List<string> list)
        {
            string[] result = new string[list.Count];
            for (int i = 0, length = list.Count; i < length; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

        /// <summary>
        /// 清空DataGridView所有的行和列
        /// </summary>
        /// <param name="dgv">DataGridView对象</param>
        public static void DgvClear(DataGridView dgv)
        {
            for (int i = dgv.Columns.Count - 1; i >= 0; i--)
            {
                dgv.Columns.RemoveAt(i);
            }
        }

        /// <summary>
        /// 删除DataTable的col列
        /// </summary>
        /// <param name="table">DataTable对象</param>
        /// <param name="col">列名</param>
        public static void DataTableRemoveCol(DataTable table, string col)
        {
            table.Columns.Remove(table.Columns[col]);
        }

        /// <summary>
        /// 将DataTable，导出为Excel
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="path">存储路径</param>
        /// <param name="sheet">Sheet名称</param>
        /// <returns></returns>
        public static bool ExportExcelWithAspose(System.Data.DataTable dt, string path, string sheet)
        {
            bool succeed = false;
            if (dt != null)
            {
                try
                {
                    // Excel
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
                    cellSheet.Name = sheet;

                    // 开始行、结束行；开始列、结束列
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;

                    // 列标题
                    for (int i = 0; i < colCount; i++)
                    {
                        Aspose.Cells.Style s = workbook.Styles[workbook.Styles.Add()];
                        s.Font.IsBold = true;
                        cellSheet.Cells[0, i].SetStyle(s);
                        cellSheet.Cells[0, i].PutValue(dt.Columns[i].ColumnName);
                    }

                    // 单元格数据
                    for (int i = 1; i <= rowCount; i++)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            cellSheet.Cells[i, j].PutValue(dt.Rows[i - 1][j].ToString());
                        }
                    }

                    cellSheet.AutoFitColumns();
                    path = System.IO.Path.GetFullPath(path);
                    workbook.Save(path);
                    succeed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    succeed = false;
                }
            }
            return succeed;
        }

        /// <summary>
        /// 将DataTable，导出为Excel
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="path">存储路径</param>
        /// <returns></returns>
        public static bool ExportExcelWithAspose(System.Data.DataTable dt, string path)
        {
            return Util.ExportExcelWithAspose(dt, path, "Sheet1");
        }

        /// <summary>
        /// 为DataGridView列指定宽度
        /// </summary>
        /// <param name="dgv">DataGridView对象</param>
        /// <param name="col">列名</param>
        /// <param name="width">宽度</param>
        public static void DgvColWidth(DataGridView dgv, string col, int width)
        {
            dgv.Columns[col].Width = width;
        }

        /// <summary>
        /// 隐藏DataGridView某列
        /// </summary>
        /// <param name="dgv">DataGridView对象</param>
        /// <param name="col">列名</param>
        public static void DgvColHide(DataGridView dgv, string col)
        {
            dgv.Columns[col].Visible = false;
        }

        /// <summary>
        /// 返回DateTime当天的开始时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime DateTimeToDayBegin(DateTime time)
        {
            DateTime t = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            return t;
        }

        /// <summary>
        /// 返回DateTime当天的结束时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime DateTimeToDayEnd(DateTime time)
        {
            DateTime t = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
            return t;
        }
        #endregion

    }
}
