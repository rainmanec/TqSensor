﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using System.Web.Script.Serialization;

namespace YAWF
{
    partial class Util
    {

        #region 项目函数库
        /// <summary>
        /// String字符串转换成byte[6]，不添加CRC校验
        /// </summary>
        /// <param name="str">字符串，不包含CRC校验</param>
        /// <returns></returns>
        public static byte[] BuildCode(string str)
        {            
            string[] code = str.Trim().Split(' ');
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

            int length = list.Count > 6 ? 6 : list.Count;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeHash(string str)
        {
            str = AppData.SECRET_KEY + str + AppData.SECRET_KEY;
            return Util.GetMd5(Util.GetMd5(str));
        }

        #endregion

        #region 据库操作-常用表
        public static string GetTbConfig(string key)
        {
            SQLiteParameter p_key = Util.NewSQLiteParameter("@p_key", DbType.String, key);
            string sql = "SELECT Value FROM Config WHERE KEY = @p_key LIMIT 1";
            string value = SQLiteHelper.GetFirstCellStringBySQL(sql, p_key);
            return value;
        }
        public static bool SetTbConfig(string key, string value)
        {
            SQLiteParameter p_key = Util.NewSQLiteParameter("@p_key", DbType.String, key);
            SQLiteParameter p_value = Util.NewSQLiteParameter("@p_value", DbType.String, value);
            string sql = "UPDATE Config SET Value = @p_value WHERE Key = @p_key";
            int affect = SQLiteHelper.ExecuteNonQuery(sql, p_key, p_value);
            return affect == 1;
        }


        #endregion

        #region XXX

        #endregion


        /// <summary>
        /// 获取App.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppConfig(string key)
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                return null;
            }
            System.Configuration.KeyValueConfigurationElement k = config.AppSettings.Settings[key];
            if (k == null)
            {
                return null;
            }
            else
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
        }

        /// <summary>
        /// 设置App.config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetAppConfig(string key, string value)
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                return false;   // throw new ArgumentException("程序配置文件缺失！");
            }
            System.Configuration.KeyValueConfigurationElement k = config.AppSettings.Settings[key];
            if (k == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            return true;
        }

        /// <summary>
        /// 百分之一英寸单位，转化为厘米
        /// </summary>
        /// <param name="inch"></param>
        /// <returns></returns>
        public static string PrintMgToInch(string length)
        {
            decimal d = Util.DecimalTryParse(length);
            return Util.DecimalToString(d / 100);
        }

        /// <summary>
        /// 将CM转化为百分之一英寸单位
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public static int InchToPrintMg(string length)
        {
            decimal d = Util.DecimalTryParse(length);
            return Convert.ToInt32(d * 100);
        }


        /// <summary>
        /// Decimal转化为String
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n">位数</param>
        /// <returns></returns>
        public static string DecimalToString(decimal d, int n)
        {
            return Math.Round(d, n).ToString();
        }

        /// <summary>
        /// Decimal转化为String
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string DecimalToString(decimal d)
        {
            return Math.Round(d, 2).ToString();
        }

        /// <summary>
        /// 根据配置文件重新设置PrintDocument对象
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SetPrintDocumentByConfig(System.Drawing.Printing.PrintDocument pd, string key)
        {
            string setting = Util.GetAppConfig(key);
            if (setting == null)
            {
                setting = "";
            }
            Hashtable hash = new Hashtable();
            hash = new JavaScriptSerializer().Deserialize<Hashtable>(setting);
            if (hash != null)
            {
                //// 边距
                //int top = (hash["MarginTop"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString());
                //int right = (hash["MarginRight"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString());
                //int bottom = (hash["MarginBottom"] == null) ? 0 : Util.CmToPrintMg(hash["MarginBottom"].ToString());
                //int left = (hash["MarginRight"] == null) ? 0 : Util.CmToPrintMg(hash["MarginRight"].ToString()) ;

                int left = (hash["MarginLeft"] == null) ? 0 : Util.InchToPrintMg(hash["MarginLeft"].ToString());
                int right = (hash["MarginRight"] == null) ? 0 : Util.InchToPrintMg(hash["MarginRight"].ToString());
                int top = (hash["MarginTop"] == null) ? 0 : Util.InchToPrintMg(hash["MarginTop"].ToString());
                int bottom = (hash["MarginBottom"] == null) ? 0 : Util.InchToPrintMg(hash["MarginBottom"].ToString());

                System.Drawing.Printing.Margins mg = new System.Drawing.Printing.Margins(left, right, top, bottom);
                pd.DefaultPageSettings.Margins = mg;

                // 打印机和纸张
                if (hash["Printer"] != null)
                {
                    foreach (String printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (hash["Printer"].ToString() == printer)
                        {
                            pd.PrinterSettings.PrinterName = printer;
                            if (hash["Paper"] != null)
                            {
                                foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
                                {
                                    if (ps.PaperName == hash["Paper"].ToString())
                                    {
                                        pd.PrinterSettings.DefaultPageSettings.PaperSize = ps;
                                        pd.DefaultPageSettings.PaperSize = ps;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }


                // 横纵向
                if (hash["LandScape"] != null)
                {
                    pd.DefaultPageSettings.Landscape = (hash["LandScape"].ToString().ToUpper() == "H") ? true : false;
                }

            }
            return true;
        }

    }
}