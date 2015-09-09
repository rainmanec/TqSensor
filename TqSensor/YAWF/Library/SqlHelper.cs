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
        /// ���ٵ��ú������ڲ������ַ���
        /// </summary>
        public static string ConnectionString = "";


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
        /// ׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
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
        /// ׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
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


        #region ���ݷ�����ض������
        

        /// <summary>
        /// ����һ��strConn����������
        /// </summary>
        /// <param name="strConn">�����ַ���</param>
        /// <returns>����һ��strConn����������</returns>
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
        /// ����strSQL�Զ����ɲ����飬�����ء�
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <returns>���ز�����</returns>
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
        /// ����һ�����úõ�SqlDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// ����һ�����úõ�SqlDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// ����һ�����úõ�SqlDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// �ͷ�ָ����SqlDataReader��
        /// </summary>
        /// <param name="sdr">ָ����SqlDataReader</param>
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
        /// ����һ�����úõ�SqlCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// ����һ�����úõ�SqlCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// ����һ�����úõ�SqlCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// �ͷ�ָ����SqlCommand����ʹ�õ�SqlConnection��
        /// </summary>
        /// <param name="comm">ָ����SqlCommand</param>
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


        #region ִ��SQL���


        /// <summary>
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����, By RainMan
        /// </summary>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="cols">����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// ִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">��ȡ�Զ�id����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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


        #region ʹ��SQL��䷵������


        /// <summary>
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ִ��һ����ѯ����,����DataSet
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="strTableName">Ҫ���ı���</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataSet</returns>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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
        /// ���ָ��DataSet
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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


        #region ִ�д洢�������


        /// <summary>
        /// StoredProcedure׼������command
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
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
        /// StoredProcedureִ����command�󽫲�����ֵ���ظ�command����
        /// </summary>
        /// <param name="comm">����command</param>
        /// <param name="parms">command����</param>
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
        /// StoredProcedure��������һ�����úõ�SqlDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SqlDataReader�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SqlCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// StoredProcedure����һ�����úõ�SqlCommand�������Ѿ��򿪣�����ֱ��ʹ�ã�����Ҫ�ǵ��ͷ�
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">command����</param>
        /// <returns>���úõ�SqlDataReader</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteNonQuery����,����Ӱ����
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����Ӱ������</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// StoredProcedureִ��һ��comm.ExecuteScalar����,���ص�һ�е�һ��,���Ϊ���򷵻�null;
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>���ص�һ�е�һ��</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����string,�����ѯ�����򷵻�string.Empty��
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����string</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataRow
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataRow</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// StoredProcedureִ��һ����ѯ����,����DataTable
        /// </summary>
        /// <param name="conn">��������</param>
        /// <param name="strSQL">strSQL����</param>
        /// <param name="parms">�������</param>
        /// <returns>����DataTable</returns>
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
        /// StoredProcedure���ָ��DataSet
        /// </summary>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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
        /// StoredProcedure���ָ��DataSet
        /// </summary>
        /// <param name="conn">�Զ�������</param>
        /// <param name="strSQL">strSQL</param>
        /// <param name="ds">ָ��DataSet,ȱʡϵͳ���Զ�����</param>
        /// <param name="strTableName">Ҫ��ӵı���</param>
        /// <param name="parms">�������</param>
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


        #region �Զ��庯��

        /// <summary>
        /// �ַ���ƴ�ӷ�ʽ����SQL���
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="parms">SqlParameter����</param>
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
        /// StoredProcedureִ��һ����ѯ����,����DataSet,DataSet.Tables������0��ʼ����
        /// </summary>
        /// <param name="strSpName">�洢������</param>
        /// <param name="parms">SqlParameter����</param>
        /// <returns></returns>
        public static DataSet SpGetDataSet(string strSpName, params SqlParameter[] parms)
        {
            return SpGetDataSet(strSpName, null, parms);
        }

        /// <summary>
        /// StoredProcedureִ��һ����ѯ����,����DataSet,DataSet.Tables��strTableName����
        /// </summary>
        /// <param name="strSpName">�洢������</param>
        /// <param name="strTableName">DataSet��������</param>
        /// <param name="parms">SqlParameter����</param>
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
