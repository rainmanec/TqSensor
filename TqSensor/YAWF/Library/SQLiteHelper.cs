using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Collections;
using System.Collections.Generic;

using System.Configuration;

namespace YAWF
{

    /// <summary>
    /// SqlHelper
    /// </summary>
    public static class SQLiteHelper
    {

        /// <summary>
        /// 快速调用函数的内部连接字符串
        /// </summary>
        public static string ConnectionString = "Data Source = " + System.Environment.CurrentDirectory + "\\Data\\" + "mydata.db";


        /// <summary>
        /// 设置默认静态连接的连接字符串
        /// </summary>
        public static string DefaultConnectionString
        {
            set
            {
                ConnectionString = value;
            }
            get
            {
                return ConnectionString;
            }
        }


        /// <summary>
        /// 内部建立connection函数
        /// </summary>
        /// <returns>一个可用的连接</returns>
        private static SQLiteConnection PrepareConnetcion(string strConn)
        {            
            try
            {
                SQLiteConnection conn = new SQLiteConnection(strConn);
                return conn;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 准备连接command
        /// </summary>
        /// <param name="comm">连接command</param>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        private static void PrepareCommand(SQLiteCommand comm, SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                comm.Connection = conn;

                comm.CommandText = strSQL;

                if (parms != null)
                {
                    foreach (SQLiteParameter parm in parms)
                    {
                        if (parm != null)
                        {
                            comm.Parameters.Add(parm.ParameterName, parm.DbType);
                            comm.Parameters[parm.ParameterName].Value = parm.Value;
                        }
                    }
                }

                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (SQLiteException e)
            {
                if (conn.State != ConnectionState.Open)
                    conn.Close();
                throw e;
            }
        }

        /// <summary>
        /// 准备连接command
        /// </summary>
        /// <param name="comm">连接command</param>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        private static void PrepareCommand(SQLiteCommand comm, SQLiteConnection conn, string strSQL)
        {
            try
            {
                comm.Connection = conn;

                comm.CommandText = strSQL;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

            }
            catch (SQLiteException e)
            {
                if (conn.State != ConnectionState.Open)
                    conn.Close();
                throw e;
            }
        }


        #region 数据访问相关对象相关
        

        /// <summary>
        /// 返回一个strConn的数据连接
        /// </summary>
        /// <param name="strConn">连接字符串</param>
        /// <returns>返回一个strConn的数据连接</returns>
        public static SQLiteConnection CreatConnection(string strConn)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(strConn);
                return conn;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据strSQL自动生成参数组，并返回。
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <returns>返回参数组</returns>
        public static SQLiteParameter[] PrepareSQLiteParameters(string strSQL)
        {
            try
            {
                char[] splt = { ',', ')', ' ' };
                string[] names = strSQL.Split(splt);
                string name = "";
                ArrayList list = new ArrayList();
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].Contains("@"))
                    {
                        name = names[i].Substring(names[i].LastIndexOf('@') + 1).Trim();
                        if (!list.Contains(name))
                        {
                            list.Add(name);
                        }
                    }
                }

                if (list.Count == 0)
                    return null;
                else
                {
                    SQLiteParameter[] prams = new SQLiteParameter[list.Count];
                    for (int i = 0; i < list.Count; i++)
                    {
                        SQLiteParameter pram = new SQLiteParameter(list[i].ToString(), null);
                        prams[i] = pram;
                    }
                    return prams;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteDataReader CreatReader(string strSQL)
        {
            try
            {
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL);
                SQLiteDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteDataReader CreatReader(string strSQL, params SQLiteParameter[] parms)
        {
            try
            {                
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                SQLiteDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteDataReader CreatReader(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                SQLiteDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 释放指定的SQLiteDataReader。
        /// </summary>
        /// <param name="sdr">指定的SQLiteDataReader</param>
        public static void DisposeReader(SQLiteDataReader sdr)
        {
            try
            {
                if (sdr != null)
                {
                    sdr.Close();
                    sdr.Dispose();
                }
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteCommand CreatCommand(string strSQL)
        {
            try
            {
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL);
                return comm;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteCommand CreatCommand(string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SQLiteCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteCommand CreatCommand(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteCommand comm = new SQLiteCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 释放指定的SQLiteCommand和其使用的SQLiteConnection。
        /// </summary>
        /// <param name="comm">指定的SQLiteCommand</param>
        public static void DisposeCommand(SQLiteCommand comm)
        {
            try
            {
                if (comm != null)
                {
                    comm.Connection.Close();
                    comm.Connection.Dispose();
                    comm.Dispose();
                }
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }



        #endregion


        #region 执行SQL语句


        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteNonQuery(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteNonQuery(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteNonQuery(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteScalar命令,返回第一行第一列,如果为空则返回null;
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回第一行第一列</returns>
        public static object ExecuteScalar(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteScalar命令,返回第一行第一列,如果为空则返回null;
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回第一行第一列</returns>
        public static object ExecuteScalar(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteScalar命令,返回第一行第一列,如果为空则返回null;
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回第一行第一列</returns>
        public static object ExecuteScalar(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">获取自动id表名</param>
        /// <returns>返回影响行数</returns>
        public static int InsertAndReturnID(string strSQL, string strTableNameInDb)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL);
                    if (comm.ExecuteNonQuery() > 0)
                    {
                        comm.CommandText = "SELECT IDENT_CURRENT('" + strTableNameInDb + "')";
                        object i = comm.ExecuteScalar();
                        return int.Parse(i.ToString());
                    }
                    else
                        return 0;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">获取自动id表名</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int InsertAndReturnID(string strSQL, string strTableNameInDb, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    if (comm.ExecuteNonQuery() > 0)
                    {
                        comm.CommandText = "SELECT IDENT_CURRENT('" + strTableNameInDb + "')";
                        object i = comm.ExecuteScalar();
                        return int.Parse(i.ToString());
                    }
                    else
                        return 0;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                    comm.Dispose();
                }
            }
        }
        
        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行, By RainMan
        /// </summary>
        /// <param name="strTableName">获取自动id表名</param>
        /// <param name="cols">列名</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int InsertAndReturnID(string strTableNameInDb, List<string> cols,  List<SQLiteParameter> parms)
        {
            StringBuilder sb_cols = new StringBuilder();
            StringBuilder sb_vals = new StringBuilder();
            for (int i = 0, len = cols.Count; i < len; i++)
            {
                sb_cols.Append(cols[i]);
                sb_vals.Append("@" + cols[i]);
                if(i != len-1)
                {
                    sb_cols.Append(",");
                    sb_vals.Append(",");
                }
            }
            SQLiteParameter[] arr_parms = new  SQLiteParameter[parms.Count];
            for (int i = 0, len = parms.Count; i < len; i++)
            {
                arr_parms[i] = parms[i];
            }
            string strSQL = String.Format("INSERT INTO {0}({1}) VALUES ({2});", strTableNameInDb, sb_cols.ToString(), sb_vals.ToString());

            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, arr_parms);
                    if (comm.ExecuteNonQuery() > 0)
                    {
                        comm.CommandText = "SELECT IDENT_CURRENT('" + strTableNameInDb + "')";
                        object i = comm.ExecuteScalar();
                        return int.Parse(i.ToString());
                    }
                    else
                        return 0;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">获取自动id表名</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int InsertAndReturnID(SQLiteConnection conn, string strSQL, string strTableNameInDb, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    if (comm.ExecuteNonQuery() > 0)
                    {
                        comm.CommandText = "SELECT IDENT_CURRENT('" + strTableNameInDb + "')";
                        object i = comm.ExecuteScalar();
                        return int.Parse(i.ToString());
                    }
                    else
                        return 0;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                    comm.Dispose();
                }
            }
        }


        #endregion


        #region 使用SQL语句返回数据


        /// <summary>
        /// 执行一个查询命令,返回string,如果查询不到则返回string.Empty。
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回string</returns>
        public static string GetFirstCellStringBySQL(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL);
                    SQLiteDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (mrd.Read())
                    {
                        obj = mrd.GetValue(0).ToString();
                    }
                    else
                    {
                        obj = string.Empty;
                    }
                    mrd.Close();
                    conn.Close();
                    mrd.Dispose();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {

                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回string,如果查询不到则返回string.Empty。
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回string</returns>
        public static string GetFirstCellStringBySQL(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL, parms);
                    SQLiteDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (mrd.Read())
                    {
                        obj = mrd.GetValue(0).ToString();
                    }
                    else
                    {
                        obj = string.Empty;
                    }
                    mrd.Close();
                    conn.Close();
                    mrd.Dispose();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {

                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回string,如果查询不到则返回string.Empty。
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回string</returns>
        public static string GetFirstCellStringBySQL(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL, parms);
                    SQLiteDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (mrd.Read())
                    {
                        obj = mrd.GetValue(0).ToString();
                    }
                    else
                    {
                        obj = string.Empty;
                    }
                    mrd.Close();
                    conn.Close();
                    mrd.Dispose();
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataRow
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回DataRow</returns>
        public static DataRow GetFirstRowBySQL(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataRow
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataRow</returns>
        public static DataRow GetFirstRowBySQL(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataRow
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataRow</returns>
        public static DataRow GetFirstRowBySQL(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataTable
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetDataTableBySQL(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataTable
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetDataTableBySQL(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataTable
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetDataTableBySQL(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    dt = null;
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(string strSQL)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">要填充的表名</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(string strSQL, string strTableName)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">要填充的表名</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(string strSQL, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataSet ds = new DataSet();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    ds = null;
                }
            }
        }

        /// <summary>
        /// 执行一个查询命令,返回DataSet
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="strTableName">要填充的表名</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSetBySQL(SQLiteConnection conn, string strSQL, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataSet ds = new DataSet();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    ds = null;
                }
            }
        }

        /// <summary>
        /// 填充指定DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">指定DataSet,缺省系统将自动创建</param>
        /// <param name="strTableName">要添加的表名</param>
        public static void FillDataSet(string strSQL, DataSet ds, string strTableName)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 填充指定DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">指定DataSet,缺省系统将自动创建</param>
        /// <param name="strTableName">要添加的表名</param>
        /// <param name="parms">命令参数</param>
        public static void FillDataSet(string strSQL, DataSet ds, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// 填充指定DataSet
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">指定DataSet,缺省系统将自动创建</param>
        /// <param name="strTableName">要添加的表名</param>
        /// <param name="parms">命令参数</param>
        public static void FillDataSet(SQLiteConnection conn, string strSQL, DataSet ds, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                try
                {
                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }
                    sda.SelectCommand = new SQLiteCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }


        #endregion


        #region 批量更新


        /// <summary>
        /// 批量更新DataTable
        /// </summary>
        /// <param name="strSelectSQL">可以选择和dt记录相同的SQL语句</param>
        /// <param name="dt">要批量更新的DataTable</param>
        public static void UpdateTable(string strSelectSQL, DataTable dt)
        {
            try
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(strSelectSQL, ConnectionString))
                {
                    SQLiteCommandBuilder scb = new SQLiteCommandBuilder(sda);
                    sda.Update(dt);
                    scb.Dispose();
                }
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        public static void UpdateDataset(SQLiteConnection conn, string strInsertSQL, string strDeleteSQL, string strUpdateSQL, DataSet ds, string strTableName)
        {
            if (strInsertSQL == null) throw new ArgumentNullException("strInsertSQL");
            if (strDeleteSQL == null) throw new ArgumentNullException("strDeleteSQL");
            if (strUpdateSQL == null) throw new ArgumentNullException("strUpdateSQL");
            if (strTableName == "") throw new ArgumentNullException("tableName");
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                try
                {
                    sda.UpdateCommand = CreatCommand(conn, strUpdateSQL, PrepareSQLiteParameters(strUpdateSQL));
                    sda.InsertCommand = CreatCommand(conn, strInsertSQL, PrepareSQLiteParameters(strInsertSQL));
                    sda.DeleteCommand = CreatCommand(conn, strDeleteSQL, PrepareSQLiteParameters(strDeleteSQL));

                    sda.Update(ds, strTableName);
                    ds.Tables[strTableName].AcceptChanges();
                    DisposeCommand(sda.UpdateCommand);
                    DisposeCommand(sda.InsertCommand);
                    DisposeCommand(sda.DeleteCommand);
                    sda.Dispose();
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        public static void UpdateDataset(string strInsertSQL, string strDeleteSQL, string strUpdateSQL, DataSet ds, string strTableName)
        {
            if (strInsertSQL == null) throw new ArgumentNullException("strInsertSQL");
            if (strDeleteSQL == null) throw new ArgumentNullException("strDeleteSQL");
            if (strUpdateSQL == null) throw new ArgumentNullException("strUpdateSQL");
            if (strTableName == "") throw new ArgumentNullException("tableName");
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.UpdateCommand = CreatCommand(strUpdateSQL, PrepareSQLiteParameters(strUpdateSQL));
                    sda.InsertCommand = CreatCommand(strInsertSQL, PrepareSQLiteParameters(strInsertSQL));
                    sda.DeleteCommand = CreatCommand(strDeleteSQL, PrepareSQLiteParameters(strDeleteSQL));

                    sda.Update(ds, strTableName);
                    ds.Tables[strTableName].AcceptChanges();
                    DisposeCommand(sda.UpdateCommand);
                    DisposeCommand(sda.InsertCommand);
                    DisposeCommand(sda.DeleteCommand);
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        #endregion


        #region 执行存储过程相关


        /// <summary>
        /// StoredProcedure准备连接command
        /// </summary>
        /// <param name="comm">连接command</param>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        private static void SpPrepareCommand(SQLiteCommand comm, SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                comm.Connection = conn;
                comm.CommandText = strSQL;
                comm.CommandType = CommandType.StoredProcedure;
                if (parms != null)
                {
                    foreach (SQLiteParameter parm in parms)
                    {
                        if (parm != null)
                        {
                            comm.Parameters.Add(parm.ParameterName, parm.DbType);
                            comm.Parameters[parm.ParameterName].Value = parm.Value;
                            comm.Parameters[parm.ParameterName].Direction = parm.Direction;
                            comm.Parameters[parm.ParameterName].Size = parm.Size;
                        }
                    }
                }

                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (SQLiteException e)
            {
                if (conn.State != ConnectionState.Open)
                    conn.Close();
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure执行完command后将参数的值返回给command参数
        /// </summary>
        /// <param name="comm">连接command</param>
        /// <param name="parms">command参数</param>
        private static void SpOutputParameter(SQLiteCommand comm, params SQLiteParameter[] parms)
        {
            if (parms != null)
            {
                foreach (SQLiteParameter parm in parms)
                {
                    if (parm != null)
                    {
                        parm.Value = comm.Parameters[parm.ParameterName].Value;
                    }
                }
            }
        }

        /// <summary>
        /// StoredProcedure方法返回一个配置好的SQLiteDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteDataReader SpCreatReader(string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                SQLiteDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SQLiteDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteDataReader SpCreatReader(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteCommand comm = new SQLiteCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                SQLiteDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SQLiteCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteCommand SpCreatCommand(string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteConnection conn = PrepareConnetcion(ConnectionString);
                SQLiteCommand comm = new SQLiteCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SQLiteCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SQLiteDataReader</returns>
        public static SQLiteCommand SpCreatCommand(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            try
            {
                SQLiteCommand comm = new SQLiteCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int SpExecuteNonQuery(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return i;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个comm.ExecuteNonQuery命令,返回影响行
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回影响行数</returns>
        public static int SpExecuteNonQuery(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return i;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个comm.ExecuteScalar命令,返回第一行第一列,如果为空则返回null;
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回第一行第一列</returns>
        public static object SpExecuteScalar(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个comm.ExecuteScalar命令,返回第一行第一列,如果为空则返回null;
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回第一行第一列</returns>
        public static object SpExecuteScalar(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回string,如果查询不到则返回string.Empty。
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回string</returns>
        public static string SpGetFirstCellString(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteCommand comm = new SQLiteCommand();
                try
                {
                    string obj;
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    SQLiteDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (mrd.Read())
                    {
                        obj = mrd.GetValue(0).ToString();
                    }
                    else
                    {
                        obj = string.Empty;
                    }
                    mrd.Close();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {

                    comm.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回string,如果查询不到则返回string.Empty。
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回string</returns>
        public static string SpGetFirstCellString(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteCommand comm = new SQLiteCommand())
            {
                try
                {
                    string obj;
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    SQLiteDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (mrd.Read())
                    {
                        obj = mrd.GetValue(0).ToString();
                    }
                    else
                    {
                        obj = string.Empty;
                    }
                    mrd.Close();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataRow
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataRow</returns>
        public static DataRow SpGetFirstRow(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataRow
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataRow</returns>
        public static DataRow SpGetFirstRow(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    dt = null;
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataTable
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable SpGetDataTable(string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    DataTable dt = new DataTable();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    return dt;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataTable
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">命令参数</param>
        /// <returns>返回DataTable</returns>
        public static DataTable SpGetDataTable(SQLiteConnection conn, string strSQL, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SQLiteCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    return dt;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    dt = null;
                }
            }
        }

        /// <summary>
        /// StoredProcedure填充指定DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">指定DataSet,缺省系统将自动创建</param>
        /// <param name="strTableName">要添加的表名</param>
        /// <param name="parms">命令参数</param>
        public static void SpFillDataSet(string strSQL, DataSet ds, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SQLiteCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    sda.Dispose();
                }
            }
        }

        /// <summary>
        /// StoredProcedure填充指定DataSet
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">指定DataSet,缺省系统将自动创建</param>
        /// <param name="strTableName">要添加的表名</param>
        /// <param name="parms">命令参数</param>
        public static void SpFillDataSet(SQLiteConnection conn, string strSQL, DataSet ds, string strTableName, params SQLiteParameter[] parms)
        {
            using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
            {
                try
                {
                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }
                    sda.SelectCommand = new SQLiteCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
            }
        }

        #endregion


        #region 自定义函数

        /// <summary>
        /// 字符串拼接方式生成SQL语句
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="parms">SQLiteParameter参数</param>
        /// <returns></returns>
        public static string PrepareSQL(string strSQL, params SQLiteParameter[] parms)
        {
            foreach(SQLiteParameter parm in parms)
            {
                string pname = parm.ParameterName;
                string pvalue = parm.Value.ToString();
                if (parm.DbType == DbType.AnsiString || parm.DbType == DbType.AnsiStringFixedLength || parm.DbType == DbType.String)
                {
                    pvalue = "'" + pvalue.Replace("'", "''") + "'";
                }
                strSQL = strSQL.Replace(pname, pvalue);         
            }
            return strSQL;
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataSet,DataSet.Tables从数字0开始索引
        /// </summary>
        /// <param name="strSpName">存储过程名</param>
        /// <param name="parms">SQLiteParameter参数</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, params SQLiteParameter[] parms)
        {
            return SpGetDataSet(strSpName, null, parms);
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataSet,DataSet.Tables以strTableName索引
        /// </summary>
        /// <param name="strSpName">存储过程名</param>
        /// <param name="strTableName">DataSet索引表名</param>
        /// <param name="parms">SQLiteParameter参数</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, string strTableName, params SQLiteParameter[] parms)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection conn = PrepareConnetcion(ConnectionString))
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter();
                try
                {

                    sda.SelectCommand = new SQLiteCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSpName, parms);
                    if (strTableName == "" || strTableName == null)
                    {
                        sda.Fill(ds);
                    }
                    else
                    {
                        sda.Fill(ds, strTableName);
                    }
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    return ds;
                }
                catch (SQLiteException e)
                {
                    throw e;
                }
                finally
                {
                    ds = null;
                }
            }
        }
        #endregion

    }
}
