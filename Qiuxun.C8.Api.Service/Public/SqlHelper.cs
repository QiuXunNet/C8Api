using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Public
{
    public static class SqlHelper
    {
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static readonly string connStr2 = ConfigurationManager.ConnectionStrings["connStr2"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr2"].ConnectionString;
        public static readonly string connStr3 = ConfigurationManager.ConnectionStrings["connStr3"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr3"].ConnectionString;
        public static readonly string connStr4 = ConfigurationManager.ConnectionStrings["connStr4"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr4"].ConnectionString;
        public static readonly string connStr5 = ConfigurationManager.ConnectionStrings["connStr5"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr5"].ConnectionString;
        public static readonly string connStr6 = ConfigurationManager.ConnectionStrings["connStr6"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr6"].ConnectionString;

        public static readonly string connStr11 = ConfigurationManager.ConnectionStrings["connStr11"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr11"].ConnectionString;
        public static readonly string connStr22 = ConfigurationManager.ConnectionStrings["connStr22"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr22"].ConnectionString;
        public static readonly string connStr33 = ConfigurationManager.ConnectionStrings["connStr33"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr33"].ConnectionString;
        public static readonly string connStr44 = ConfigurationManager.ConnectionStrings["connStr44"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr44"].ConnectionString;
        public static readonly string connStr55 = ConfigurationManager.ConnectionStrings["connStr55"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr55"].ConnectionString;
        public static readonly string connStr66 = ConfigurationManager.ConnectionStrings["connStr66"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr66"].ConnectionString;

        public static readonly string connStr77 = ConfigurationManager.ConnectionStrings["connStr77"] == null ? "" : ConfigurationManager.ConnectionStrings["connStr77"].ConnectionString;

        private static string fenzhanStr = "";

        private static readonly string connSupportStr = ConfigurationManager.ConnectionStrings["connSupportStr"].ConnectionString;

        #region 普通

        //执行insert/delete/update的方法
        public static int ExecuteNonQuery(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        //执行ExecuteScalar的方法
        public static object ExecuteScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }

                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
        }


        //执行ExecuteReader的方法
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (pms != null) cmd.Parameters.AddRange(pms);
                try
                {
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return sdr;
                }
                catch (Exception ex)
                {
                    conn.Dispose();
                }
            }
            return null;
        }


        //SqlDataAdapter 返回DataSet数据集
        public static DataSet ExecutedDataAdapter(string sql, params SqlParameter[] pms)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                if (pms != null) adapter.SelectCommand.Parameters.AddRange(pms);
                adapter.Fill(ds);
            }
            return ds;
        }


        public static void ExecuteTransaction(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    try
                    {

                        if (pms != null) cmd.Parameters.AddRange(pms);
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
                        tran.Rollback();
                    }

                }
            }
        }

        #endregion

        #region 分站使用

        public static void HandConnStr(int fenzhan)
        {
            if (fenzhan == 1)
            {
                fenzhanStr = connStr;
            }
            else if (fenzhan == 2)
            {
                fenzhanStr = connStr2;
            }
            else if (fenzhan == 3)
            {
                fenzhanStr = connStr3;
            }
            else if (fenzhan == 4)
            {
                fenzhanStr = connStr4;
            }
            else if (fenzhan == 5)
            {
                fenzhanStr = connStr5;
            }
            else if (fenzhan == 6)
            {
                fenzhanStr = connStr6;
            }
            else if (fenzhan == 11)
            {
                fenzhanStr = connStr11;
            }
            else if (fenzhan == 22)
            {
                fenzhanStr = connStr22;
            }
            else if (fenzhan == 33)
            {
                fenzhanStr = connStr33;
            }
            else if (fenzhan == 44)
            {
                fenzhanStr = connStr44;
            }
            else if (fenzhan == 55)
            {
                fenzhanStr = connStr55;
            }
            else if (fenzhan == 66)
            {
                fenzhanStr = connStr66;
            }
            else if (fenzhan == 77)
            {
                fenzhanStr = connStr77;
            }
        }


        //执行insert/delete/update的方法
        public static int ExecuteNonQueryForFenZhan(int fenzhan, string sql, params SqlParameter[] pms)
        {
            //处理连接字符串
            HandConnStr(fenzhan);

            using (SqlConnection conn = new SqlConnection(fenzhanStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        //执行ExecuteScalar的方法
        public static object ExecuteScalarForFenZhan(int fenzhan, string sql, params SqlParameter[] pms)
        {
            //处理连接字符串
            HandConnStr(fenzhan);

            using (SqlConnection conn = new SqlConnection(fenzhanStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
        }


        //执行ExecuteReader的方法
        public static SqlDataReader ExecuteReaderForFenZhan(int fenzhan, string sql, params SqlParameter[] pms)
        {

            //处理连接字符串
            HandConnStr(fenzhan);

            SqlConnection conn = new SqlConnection(fenzhanStr);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (pms != null) cmd.Parameters.AddRange(pms);
                try
                {
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return sdr;
                }
                catch (Exception ex)
                {
                    conn.Dispose();
                }
            }
            return null;
        }


        //事务执行
        public static void ExecuteTransactionForFenZhan(int fenzhan, string sql, params SqlParameter[] pms)
        {
            //处理连接字符串
            HandConnStr(fenzhan);

            using (SqlConnection conn = new SqlConnection(fenzhanStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    try
                    {

                        if (pms != null) cmd.Parameters.AddRange(pms);
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(fenzhan + "---->" + sql + "------>" + ex.Message);
                        tran.Rollback();
                    }

                }
            }
        }


        #endregion

        #region Support使用


        //执行insert/delete/update的方法
        public static int ExecuteNonQueryForSupport(string sql, params SqlParameter[] pms)
        {

            using (SqlConnection conn = new SqlConnection(connSupportStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (pms != null) cmd.Parameters.AddRange(pms);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

    }
}
