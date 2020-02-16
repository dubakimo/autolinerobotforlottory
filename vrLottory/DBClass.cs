using ErikEJ.SqlCe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLottory
{
    public class DBClass
    {
        //static string DBpath;
        //public static string SqlSrvIP { get; set; }  //sql server ip & port ,ex: 192.168.0.1:1433
        //public static string DBname { get; set; }   //sql server DB 名稱 
        //public static string user { get; set; }     //使用者名稱ur-
        //public static string pass { get; set; }      //密碼
        public static string SqlConnectionString { get; set; }      //Sql Connection String
        public static string DBType { get; set; }
        //FileInfo fio = new FileInfo(DBpath);


        //對就有資料庫新增FWVersion欄位
        public static bool upDBTableField()
        {
            bool ret = false;

            //string sqlsel = "SELECT  *  FROM              Patient_Basic ";
            string sqlsel = "SELECT  *  FROM              tb_Patient ";

            DataSet ds = GetDataSet(sqlsel);

            if (ds.Tables[0].Columns.Count <= 30)   //欄位數量<=30,表示就資料庫,沒有FWVersion欄位 ==>新增此欄位
            {
                //string sqlScript = " ALTER TABLE [Patient_Basic] ADD [FWversion] int NULL ";
                string sqlScript = " ALTER TABLE [tb_Patient] ADD [FWversion] int NULL ";
                ret = Sqlce_ExecuteNonQuery(sqlScript);
            }

            return ret;
        }

        /// <summary>
        /// 建立回傳認證資料
        /// </summary>
        /// <param name="i">i=0,表示token認證失敗,i=1,表示token認證成功,i=-1,表示密碼修改驗證失敗</param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        public static DataTable RcTable(int i, string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RC");
            dt.Columns.Add("RM");
            dt.TableName = "verify";
            dt.AcceptChanges();


            //表示認證不成功
            DataRow dr = dt.NewRow();
            dr["RC"] = i;
            dr["RM"] = message;
            dt.Rows.Add(dr);
            dt.AcceptChanges();


            return dt;
        }

        private static bool Sqlce_ExecuteNonQuery(string Sqlstr)
        {

            SqlCeConnection Conn = null;
            SqlCeCommand Dc = null;
            int Return_val;
            bool Return_bool = true;

            Conn = GetSQLCeConnection();
            try
            {
                if (Conn != null)
                {
                    Dc = new SqlCeCommand(Sqlstr, Conn);
                    Return_val = Dc.ExecuteNonQuery();

                    if (Return_val < 0)
                    {
                        Return_bool = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.Sqlce_ExecuteNonQuery: " + ex.Message.ToString() + "SQL Command:" + Sqlstr);
                Return_bool = false;
            }
            finally
            {
                Conn.Close();
            }

            return Return_bool;
        }

        public static bool Sqlce_ExecuteNonQuery(string Sqlstr, List<SqlCeParameter> param)
        {

            SqlCeConnection Conn = null;
            SqlCeCommand Dc = null;
            int Return_val;
            bool Return_bool = true;

            Conn = GetSQLCeConnection();
            try
            {
                if (Conn != null)
                {
                    Dc = new SqlCeCommand(Sqlstr, Conn);
                    Dc.Parameters.AddRange(param.ToArray());
                    Return_val = Dc.ExecuteNonQuery();

                    if (Return_val < 0)
                    {
                        Return_bool = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.Sqlce_ExecuteNonQuery: " + ex.Message.ToString() + "SQL Command:" + Sqlstr);
                Return_bool = false;
            }
            finally
            {
                Conn.Close();
            }

            return Return_bool;
        }

        private static bool Sql_ExecuteNonQuery(string Sqlstr)
        {
            SqlConnection Conn = null;
            SqlCommand Dc = null;
            int Return_val;
            bool Return_bool = true;

            Conn = GetSQLConnection();
            try
            {
                if (Conn != null)
                {
                    Dc = new SqlCommand(Sqlstr, Conn);
                    Return_val = Dc.ExecuteNonQuery();

                    if (Return_val < 0)
                    {
                        Return_bool = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.SQL_ExecuteNonQuery: " + ex.Message.ToString() + "SQL Command:" + Sqlstr);
                Return_bool = false;
            }
            finally
            {
                Conn.Close(); Conn.Dispose();
            }

            return Return_bool;
        }

        public static bool Sql_ExecuteNonQuery(string Sqlstr, List<SqlParameter> param)
        {
            SqlConnection Conn = null;
            SqlCommand Dc = null;
            int Return_val;
            bool Return_bool = true;

            Conn = GetSQLConnection();
            try
            {
                if (Conn != null)
                {
                    Dc = new SqlCommand(Sqlstr, Conn);
                    Dc.Parameters.AddRange(param.ToArray());
                    Return_val = Dc.ExecuteNonQuery();

                    if (Return_val < 0)
                    {
                        Return_bool = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.SQL_ExecuteNonQuery: " + ex.Message.ToString() + "SQL Command:" + Sqlstr);
                Return_bool = false;
            }
            finally
            {
                Conn.Close(); Conn.Dispose();
            }

            return Return_bool;
        }

        public static bool ExecuteNonQuery(string sqlstr)
        {
            bool flag;
            string dbType = ConfigurationManager.AppSettings["dbType"].ToString();
            //string dbType = Resource.dbType;

            if (dbType == "sdf")
                flag = Sqlce_ExecuteNonQuery(sqlstr);
            else
                flag = Sql_ExecuteNonQuery(sqlstr);

            return flag;
        }




        public static SqlCeConnection GetSQLCeConnection()
        {
            SqlCeConnection Conn_SQL = null;
            string ConnectionString = "";

            //ConnectionString = EasyCompliance.Properties.Settings.Default.EasyCompConnectionString;
            //ConnectionString = @"Data Source=C:\APEX\EZDBce40.sdf;Max Database Size=4091";

            //ConnectionString = DotNetEZLib.Resource.sdfConnectionString;
            //ConnectionString = DBClass.SqlConnectionString;
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            try
            {
                Conn_SQL = new SqlCeConnection();
                Conn_SQL.ConnectionString = ConnectionString;
                Conn_SQL.Open();
                return Conn_SQL;
            }
            catch (Exception ex)
            {
                //LogAction logs = new LogAction();
                //logs.Debug(DateTime.Now.ToString("yyyy/MM/dd mm:ss") + " LoginID:" + MainWindow.LsInfo.LoginId + ", ShareFunc=>GetSQLCeConnection Exception:" + ex.ToString() + "\n");
                return null;
            }
        }


        /// <summary>
        /// 得到SQLConnection
        /// </summary>
        /// <returns></returns>        
        public static SqlConnection GetSQLConnection()
        {
            //2018/1/14 add azure key


            SqlConnection Conn_SQL = null;
            string ConnectionString = "";

            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
            //.IntegratedSecurity = true;
            //connStringBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;

            try
            {
                Conn_SQL = new SqlConnection();
                //add by arthur 2019/01/14
                Conn_SQL.ConnectionString = connStringBuilder.ConnectionString;

                if (Conn_SQL.State == ConnectionState.Closed)
                    Conn_SQL.Open();
                return Conn_SQL;
            }
            catch (Exception ex)
            {
                //LogAction logs = new LogAction();
                //logs.Debug(DateTime.Now.ToString("yyyy/MM/dd mm:ss") + " LoginID:" + MainWindow.LsInfo.LoginId + ", ShareFunc=>GetSQLConnection Exception:" + ex.ToString() + "\n");
                return null;
            }
        }

        /// <summary>
        /// 通用式GetDatsSet
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sqlstr)
        {

            DataSet ds = new DataSet();
            string dbType = ConfigurationManager.AppSettings["dbType"].ToString();
            //string dbType = DotNetEZLib.Resource.dbType;

            if (dbType == "sdf")
                ds = sqlce_GetDataSet(sqlstr);
            else
                ds = sql_GetDataSet(sqlstr);

            return ds;
        }

        public static DataSet sql_GetDataSet(string sqlstr)
        {
            SqlConnection Conn = null;
            SqlDataAdapter Dr = null;
            DataSet Return_DS = new DataSet();

            Conn = DBClass.GetSQLConnection();
            try
            {
                if (Conn != null)
                {
                    SqlCommand comd = new SqlCommand(sqlstr, Conn);
                    comd.CommandTimeout = 0;
                    //Dr = new SqlDataAdapter(sqlstr, Conn);
                    Dr = new SqlDataAdapter(comd);

                    Dr.Fill(Return_DS);
                    Dr.Dispose();
                    Conn.Close(); Conn.Dispose();
                }

            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.sql_GetDataSet: " + ex.Message.ToString());
                Return_DS = null;
            }
            finally
            {

            }

            return Return_DS;
        }

        public static DataSet sql_GetDataSet(string sqlstr, List<SqlParameter> param)
        {
            SqlConnection Conn = null;
            SqlDataAdapter Dr = null;
            DataSet Return_DS = new DataSet();

            Conn = DBClass.GetSQLConnection();
            try
            {
                if (Conn != null)
                {
                    SqlCommand comd = new SqlCommand(sqlstr, Conn);
                    comd.CommandTimeout = 0;
                    comd.Parameters.AddRange(param.ToArray());
                    //Dr = new SqlDataAdapter(sqlstr, Conn);
                    Dr = new SqlDataAdapter(comd);

                    Dr.Fill(Return_DS);
                    Dr.Dispose();
                    Conn.Close(); Conn.Dispose();
                }

            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.sql_GetDataSet: " + ex.Message.ToString());
                Return_DS = null;
            }
            finally
            {

            }

            return Return_DS;
        }

        private static DataSet sqlce_GetDataSet(string sqlstr)
        {
            SqlCeConnection Conn = null;
            SqlCeDataAdapter Dr = null;
            DataSet Return_DS = new DataSet();

            Conn = GetSQLCeConnection();
            try
            {
                if (Conn != null)
                {
                    Dr = new SqlCeDataAdapter(sqlstr, Conn);

                    Dr.Fill(Return_DS);
                    Dr.Dispose();
                    Conn.Close(); Conn.Dispose();
                }

            }
            catch (Exception ex)
            {
                //LogAction log = new LogAction();
                //log.Debug(DateTime.Now + " SF.sqlce_GetDataSet: " + ex.Message.ToString());
                Return_DS = null;
            }
            finally
            {

            }

            return Return_DS;
        }


        /// <summary>
        /// 匯入資料到資料庫 Table
        /// </summary>
        /// <param name="maplist"> 對應資料庫欄位名稱</param>
        /// <param name="TableName">資料庫Table Name</param>
        /// <param name="dt">Data Table</param>
        /// <returns>true or false</returns>
        private static bool SqlceBulkCopyImport(IList<string> maplist, string TableName, DataTable dt)
        {
            using (SqlCeConnection connection = GetSQLCeConnection())
            {
                //connection.Open();
                using (SqlCeBulkCopy bulkCopy = new SqlCeBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = TableName;
                    foreach (string a in maplist)
                    {
                        bulkCopy.ColumnMappings.Add(a, a);
                    }
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        bulkCopy.Dispose();
                        //return true;
                    }
                    catch (Exception e)
                    {
                        connection.Close(); connection.Dispose();
                        return false;
                        //throw e;
                    }
                }

                connection.Close(); connection.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 匯入資料到資料庫 Table
        /// </summary>
        /// <param name="maplist"> 對應資料庫欄位名稱</param>
        /// <param name="TableName">資料庫Table Name</param>
        /// <param name="dt">Data Table</param>
        /// <returns>true or false</returns>
        private static bool SqlBulkCopyImport(IList<string> maplist, string TableName, DataTable dt)
        {

            using (SqlConnection connection = GetSQLConnection())
            {
                //connection.Open();
                //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = TableName;
                    foreach (string a in maplist)
                    {
                        bulkCopy.ColumnMappings.Add(a, a);
                    }
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        bulkCopy.Close();

                    }
                    catch (Exception e)
                    {
                        bulkCopy.Close();
                        connection.Close(); connection.Dispose();
                        //throw e;
                    }
                }
                connection.Close(); connection.Dispose();
            }

            return true;
        }

        public static bool BulkCopyImport(IList<string> maplist, string TableName, DataTable dt)
        {
            string dbType = ConfigurationManager.AppSettings["dbType"].ToString();
            //string dbType = DotNetEZLib.Resource.dbType;

            if (dbType == "sdf")
                return SqlceBulkCopyImport(maplist, TableName, dt);
            else
                return SqlBulkCopyImport(maplist, TableName, dt);
        }

    }
}
