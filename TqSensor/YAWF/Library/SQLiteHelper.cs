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
        /// ���ٵ��ú������ڲ������ַ���
        /// </summary>
        public static string ConnectionString = "Data Source = " + System.Environment.CurrentDirectory + "\\Data\\" + "mydata.db";


        /// <summary>
        /// ����Ĭ�Ͼ�̬���ӵ������ַ���
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
        /// �ڲ�����connection����
        /// </summary>
        /// <returns>һ�����õ�����</returns>
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
        /// ׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
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
        /// ׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
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


        #region ���ݷ�����ض������
        

        /// <summary>
        /// ����һ��strConn����������
        /// </summary>
        /// <param name="strConn">�����ַ���</param>
        /// <returns>����һ��strConn����������</returns>
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
        /// ����strSQL�Զ����ɲ����飬�����ء�
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <returns>���ز�����</returns>
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
        /// ����һ�����úõ�SQLiteDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// ����һ�����úõ�SQLiteDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// ����һ�����úõ�SQLiteDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// �ͷ�ָ����SQLiteDataReader��
        /// </summary>
        /// <param name="sdr">ָ����SQLiteDataReader</param>
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
        /// ����һ�����úõ�SQLiteCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// ����һ�����úõ�SQLiteCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// ����һ�����úõ�SQLiteCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// �ͷ�ָ����SQLiteCommand����ʹ�õ�SQLiteConnection��
        /// </summary>
        /// <param name="comm">ָ����SQLiteCommand</param>
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


        #region ִ��SQL���


        /// <summary>
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����, By RainMan
        /// </summary>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="cols">����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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


        #region ʹ��SQL��䷵������


        /// <summary>
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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


        #region ��������


        /// <summary>
        /// ��������DataTable
        /// </summary>
        /// <param name="strSelectSQL">����ѡ���dt��¼��ͬ��SQL���</param>
        /// <param name="dt">Ҫ�������µ�DataTable</param>
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


        #region ִ�д洢�������


        /// <summary>
        /// StoredProcedure׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
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
        /// StoredProcedureִ����command�󽫲�����ֵ���ظ�command����
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="parms">command����</param>
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
        /// StoredProcedure��������һ�����úõ�SQLiteDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SQLiteDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SQLiteCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SQLiteCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SQLiteDataReader</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// StoredProcedure���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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
        /// StoredProcedure���ָ��DataSet
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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


        #region �Զ��庯��

        /// <summary>
        /// �ַ���ƴ�ӷ�ʽ����SQL���
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="parms">SQLiteParameter����</param>
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
        /// StoredProcedureִ��һ����ѯ����,����DataSet,DataSet.Tables������0��ʼ����
        /// </summary>
        /// <param name="strSpName">�洢������</param>
        /// <param name="parms">SQLiteParameter����</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, params SQLiteParameter[] parms)
        {
            return SpGetDataSet(strSpName, null, parms);
        }

        /// <summary>
        /// StoredProcedureִ��һ����ѯ����,����DataSet,DataSet.Tables��strTableName����
        /// </summary>
        /// <param name="strSpName">�洢������</param>
        /// <param name="strTableName">DataSet��������</param>
        /// <param name="parms">SQLiteParameter����</param>
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
