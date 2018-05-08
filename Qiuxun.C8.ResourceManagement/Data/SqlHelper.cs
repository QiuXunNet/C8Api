using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.ResourceManagement.Data
{
    public static class SqlHelper
    {
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        

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


    }
}
