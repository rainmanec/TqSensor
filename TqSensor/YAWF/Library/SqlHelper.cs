using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using System.Configuration;

namespace YAMF
{

    /// <summary>
    /// SqlHelper
    /// </summary>
    public static class SqlHelper
    {

        /// <summary>
        /// 快速调用函数的内部连接字符串
        /// </summary>
        public static string ConnectionString = "";


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
        private static SqlConnection PrepareConnetcion(string strConn)
        {
            try
            {
                SqlConnection conn = new SqlConnection(strConn);
                return conn;
            }
            catch (SqlException e)
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
        private static void PrepareCommand(SqlCommand comm, SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                comm.Connection = conn;

                comm.CommandText = strSQL;

                if (parms != null)
                {
                    foreach (SqlParameter parm in parms)
                    {
                        if (parm != null)
                        {
                            comm.Parameters.Add(parm.ParameterName, parm.SqlDbType);
                            comm.Parameters[parm.ParameterName].Value = parm.Value;
                        }
                    }
                }

                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (SqlException e)
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
        private static void PrepareCommand(SqlCommand comm, SqlConnection conn, string strSQL)
        {
            try
            {
                comm.Connection = conn;

                comm.CommandText = strSQL;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

            }
            catch (SqlException e)
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
        public static SqlConnection CreatConnection(string strConn)
        {
            try
            {
                SqlConnection conn = new SqlConnection(strConn);
                return conn;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据strSQL自动生成参数组，并返回。
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <returns>返回参数组</returns>
        public static SqlParameter[] PrepareSqlParameters(string strSQL)
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
                    SqlParameter[] prams = new SqlParameter[list.Count];
                    for (int i = 0; i < list.Count; i++)
                    {
                        SqlParameter pram = new SqlParameter(list[i].ToString(), null);
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
        /// 返回一个配置好的SqlDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlDataReader CreatReader(string strSQL)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL);
                SqlDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SqlDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlDataReader CreatReader(string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                SqlDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SqlDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlDataReader CreatReader(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                SqlDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 释放指定的SqlDataReader。
        /// </summary>
        /// <param name="sdr">指定的SqlDataReader</param>
        public static void DisposeReader(SqlDataReader sdr)
        {
            try
            {
                if (sdr != null)
                {
                    sdr.Close();
                    sdr.Dispose();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SqlCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlCommand CreatCommand(string strSQL)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL);
                return comm;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SqlCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlCommand CreatCommand(string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回一个配置好的SqlCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlCommand CreatCommand(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                PrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 释放指定的SqlCommand和其使用的SqlConnection。
        /// </summary>
        /// <param name="comm">指定的SqlCommand</param>
        public static void DisposeCommand(SqlCommand comm)
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
            catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SqlException e)
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
        public static int ExecuteNonQuery(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SqlException e)
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
        public static int ExecuteNonQuery(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    return i;
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SqlException e)
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
        public static object ExecuteScalar(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SqlException e)
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
        public static object ExecuteScalar(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    PrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
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
                catch (SqlException e)
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
        public static int InsertAndReturnID(string strSQL, string strTableNameInDb, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
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
                catch (SqlException e)
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
        public static int InsertAndReturnID(string strTableNameInDb, List<string> cols,  List<SqlParameter> parms)
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
            SqlParameter[] arr_parms = new  SqlParameter[parms.Count];
            for (int i = 0, len = parms.Count; i < len; i++)
            {
                arr_parms[i] = parms[i];
            }
            string strSQL = String.Format("INSERT INTO {0}({1}) VALUES ({2});", strTableNameInDb, sb_cols.ToString(), sb_vals.ToString());

            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
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
                catch (SqlException e)
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
        public static int InsertAndReturnID(SqlConnection conn, string strSQL, string strTableNameInDb, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
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
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL);
                    SqlDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
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
                catch (SqlException e)
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
        public static string GetFirstCellStringBySQL(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL, parms);
                    SqlDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
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
                catch (SqlException e)
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
        public static string GetFirstCellStringBySQL(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    string obj;
                    PrepareCommand(comm, conn, strSQL, parms);
                    SqlDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
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
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SqlException e)
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
        public static DataRow GetFirstRowBySQL(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SqlException e)
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
        public static DataRow GetFirstRowBySQL(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SqlException e)
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
        public static DataTable GetDataTableBySQL(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataTable dt = new DataTable();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SqlException e)
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
        public static DataTable GetDataTableBySQL(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
        public static DataSet GetDataSetBySQL(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
        public static DataSet GetDataSetBySQL(string strSQL, string strTableName, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataSet ds = new DataSet();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
        public static DataSet GetDataSetBySQL(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataSet ds = new DataSet();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
        public static DataSet GetDataSetBySQL(SqlConnection conn, string strSQL, string strTableName, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataSet ds = new DataSet();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds, strTableName);
                    conn.Close();
                    return ds;
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SqlException e)
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
        public static void FillDataSet(string strSQL, DataSet ds, string strTableName, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SqlException e)
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
        public static void FillDataSet(SqlConnection conn, string strSQL, DataSet ds, string strTableName, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                try
                {
                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }
                    sda.SelectCommand = new SqlCommand();
                    PrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                }
                catch (SqlException e)
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
                using (SqlDataAdapter sda = new SqlDataAdapter(strSelectSQL, ConnectionString))
                {
                    SqlCommandBuilder scb = new SqlCommandBuilder(sda);
                    sda.Update(dt);
                    scb.Dispose();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public static void UpdateDataset(SqlConnection conn, string strInsertSQL, string strDeleteSQL, string strUpdateSQL, DataSet ds, string strTableName)
        {
            if (strInsertSQL == null) throw new ArgumentNullException("strInsertSQL");
            if (strDeleteSQL == null) throw new ArgumentNullException("strDeleteSQL");
            if (strUpdateSQL == null) throw new ArgumentNullException("strUpdateSQL");
            if (strTableName == "") throw new ArgumentNullException("tableName");
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                try
                {
                    sda.UpdateCommand = CreatCommand(conn, strUpdateSQL, PrepareSqlParameters(strUpdateSQL));
                    sda.InsertCommand = CreatCommand(conn, strInsertSQL, PrepareSqlParameters(strInsertSQL));
                    sda.DeleteCommand = CreatCommand(conn, strDeleteSQL, PrepareSqlParameters(strDeleteSQL));

                    sda.Update(ds, strTableName);
                    ds.Tables[strTableName].AcceptChanges();
                    DisposeCommand(sda.UpdateCommand);
                    DisposeCommand(sda.InsertCommand);
                    DisposeCommand(sda.DeleteCommand);
                    sda.Dispose();
                }
                catch (SqlException e)
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
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.UpdateCommand = CreatCommand(strUpdateSQL, PrepareSqlParameters(strUpdateSQL));
                    sda.InsertCommand = CreatCommand(strInsertSQL, PrepareSqlParameters(strInsertSQL));
                    sda.DeleteCommand = CreatCommand(strDeleteSQL, PrepareSqlParameters(strDeleteSQL));

                    sda.Update(ds, strTableName);
                    ds.Tables[strTableName].AcceptChanges();
                    DisposeCommand(sda.UpdateCommand);
                    DisposeCommand(sda.InsertCommand);
                    DisposeCommand(sda.DeleteCommand);
                }
                catch (SqlException e)
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
        private static void SpPrepareCommand(SqlCommand comm, SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                comm.Connection = conn;
                comm.CommandText = strSQL;
                comm.CommandType = CommandType.StoredProcedure;
                if (parms != null)
                {
                    foreach (SqlParameter parm in parms)
                    {
                        if (parm != null)
                        {
                            comm.Parameters.Add(parm.ParameterName, parm.SqlDbType);
                            comm.Parameters[parm.ParameterName].Value = parm.Value;
                            comm.Parameters[parm.ParameterName].Direction = parm.Direction;
                            comm.Parameters[parm.ParameterName].Size = parm.Size;
                        }
                    }
                }

                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (SqlException e)
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
        private static void SpOutputParameter(SqlCommand comm, params SqlParameter[] parms)
        {
            if (parms != null)
            {
                foreach (SqlParameter parm in parms)
                {
                    if (parm != null)
                    {
                        parm.Value = comm.Parameters[parm.ParameterName].Value;
                    }
                }
            }
        }

        /// <summary>
        /// StoredProcedure方法返回一个配置好的SqlDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlDataReader SpCreatReader(string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                SqlDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SqlDataReader，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlDataReader SpCreatReader(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                SqlDataReader sdr = comm.ExecuteReader();
                return sdr;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SqlCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlCommand SpCreatCommand(string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlConnection conn = PrepareConnetcion(ConnectionString);
                SqlCommand comm = new SqlCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// StoredProcedure返回一个配置好的SqlCommand，连接已经打开，可以直接使用，用完要记得释放
        /// </summary>
        /// <param name="conn">自定义连接</param>
        /// <param name="strSQL">strSQL命令</param>
        /// <param name="parms">command参数</param>
        /// <returns>配置好的SqlDataReader</returns>
        public static SqlCommand SpCreatCommand(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                SpPrepareCommand(comm, conn, strSQL, parms);
                return comm;
            }
            catch (SqlException e)
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
        public static int SpExecuteNonQuery(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return i;
                }
                catch (SqlException e)
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
        public static int SpExecuteNonQuery(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    int i = comm.ExecuteNonQuery();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return i;
                }
                catch (SqlException e)
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
        public static object SpExecuteScalar(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SqlException e)
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
        public static object SpExecuteScalar(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    object obj = comm.ExecuteScalar();
                    conn.Close();
                    SpOutputParameter(comm, parms);
                    return obj;
                }
                catch (SqlException e)
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
        public static string SpGetFirstCellString(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                try
                {
                    string obj;
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    SqlDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
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
                catch (SqlException e)
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
        public static string SpGetFirstCellString(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlCommand comm = new SqlCommand())
            {
                try
                {
                    string obj;
                    SpPrepareCommand(comm, conn, strSQL, parms);
                    SqlDataReader mrd = comm.ExecuteReader(CommandBehavior.CloseConnection);
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
                catch (SqlException e)
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
        public static DataRow SpGetFirstRow(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
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
                catch (SqlException e)
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
        public static DataRow SpGetFirstRow(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    if (dt.Rows.Count == 0)
                        return null;
                    else
                        return dt.Rows[0];
                }
                catch (SqlException e)
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
        public static DataTable SpGetDataTable(string strSQL, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    DataTable dt = new DataTable();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    return dt;
                }
                catch (SqlException e)
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
        public static DataTable SpGetDataTable(SqlConnection conn, string strSQL, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataTable dt = new DataTable();
                try
                {
                    sda.SelectCommand = new SqlCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(dt);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                    return dt;
                }
                catch (SqlException e)
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
        public static void SpFillDataSet(string strSQL, DataSet ds, string strTableName, params SqlParameter[] parms)
        {
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {

                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }

                    sda.SelectCommand = new SqlCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                }
                catch (SqlException e)
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
        public static void SpFillDataSet(SqlConnection conn, string strSQL, DataSet ds, string strTableName, params SqlParameter[] parms)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                try
                {
                    if (ds == null)
                    {
                        ds = new DataSet();
                        ds.Tables.Add(strTableName);
                    }
                    sda.SelectCommand = new SqlCommand();
                    SpPrepareCommand(sda.SelectCommand, conn, strSQL, parms);
                    sda.Fill(ds.Tables[strTableName]);
                    conn.Close();
                    SpOutputParameter(sda.SelectCommand, parms);
                }
                catch (SqlException e)
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
        /// <param name="parms">SqlParameter参数</param>
        /// <returns></returns>
        public static string PrepareSQL(string strSQL, params SqlParameter[] parms)
        {
            foreach(SqlParameter parm in parms)
            {
                string pname = parm.ParameterName;
                string pvalue = parm.Value.ToString();
                if (parm.SqlDbType == SqlDbType.VarChar || parm.SqlDbType == SqlDbType.Char || parm.SqlDbType == SqlDbType.Text)
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
        /// <param name="parms">SqlParameter参数</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, params SqlParameter[] parms)
        {
            return SpGetDataSet(strSpName, null, parms);
        }

        /// <summary>
        /// StoredProcedure执行一个查询命令,返回DataSet,DataSet.Tables以strTableName索引
        /// </summary>
        /// <param name="strSpName">存储过程名</param>
        /// <param name="strTableName">DataSet索引表名</param>
        /// <param name="parms">SqlParameter参数</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, string strTableName, params SqlParameter[] parms)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = PrepareConnetcion(ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                try
                {

                    sda.SelectCommand = new SqlCommand();
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
                catch (SqlException e)
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
