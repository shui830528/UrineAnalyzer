using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  /// <summary>
  /// 数据库连接对象
  /// </summary>
  public class ConnectionObj
  {
    private static ConcurrentDictionary<int, OleDbConnection> connectionList = new ConcurrentDictionary<int, OleDbConnection>();
    private int nThreadID = 0;
    public static string DBName;

    public ConnectionObj()
    {
      nThreadID = Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    /// <returns></returns>
    public static string GetConnectionString()
    {
      return string.Format("Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = {0}", DBName);
    }
    /// <summary>
    /// 新建数据库连接
    /// </summary>
    /// <returns></returns>
    protected OleDbConnection NewConnection()
    {
      OleDbConnection connection = null;
      try
      {
        connection = new OleDbConnection(GetConnectionString());
        connection.Open();
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message);
      }
      return connection;
    }
    static OleDbConnection connection = null;
    public OleDbConnection GetConnection()
    {
      if (connection == null)
      {
        connection = NewConnection();
      }
      
      return connection;
      //OleDbConnection connection = null;
      //if (connectionList.ContainsKey(nThreadID))
      //{
      //  connectionList.TryGetValue(nThreadID,out connection);
      //}
      //else
      //{
      //  connection = NewConnection();
      //  connectionList.TryAdd(nThreadID, connection);
      //  Log.WriteLog(string.Format("Connection Thread ID {0} Count : {1}", nThreadID, connectionList.Count));
      //}
      //return connection;
    }
  }
  /// <summary>
  /// 数据库操作
  /// </summary>
  public class DB
  {
    /// <summary>
    /// 数据查询
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dataSet"></param>
    public void Query(string sql, out DataSet dataSet)
    {
      dataSet = new DataSet();
      OleDbDataAdapter adapter = new OleDbDataAdapter();

      try
      {
        OleDbCommand command = new OleDbCommand(sql, new ConnectionObj().GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(dataSet);
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + " : DB.Query执行错误 : " + sql);
      }

    }
    public void Query(string sql,out IDataReader dataReader)
    {
      DataSet dataSet = new DataSet();
      OleDbDataAdapter adapter = new OleDbDataAdapter();
      dataReader = null;
      try
      {
        OleDbCommand command = new OleDbCommand(sql, new ConnectionObj().GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(dataSet);
        dataReader = dataSet.Tables[0].CreateDataReader();
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " : DB.Query执行错误 : " + sql);
      }

    }
    /// <summary>
    /// 执行SQL
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public int Execute(string sql)
    {
      int nCount = 0;
      OleDbConnection connection = new ConnectionObj().GetConnection();
      OleDbCommand command = new OleDbCommand(sql, connection);
      try
      {
        command.Transaction = connection.BeginTransaction();
        nCount = command.ExecuteNonQuery();
        command.Transaction.Commit();
      }
      catch (Exception ex)
      {
        nCount = 0;
        command.Transaction.Rollback();
        Log.WriteLog(ex.Message + " : DB.Execute执行错误 : " + sql);
      }

      return nCount;
    }

    public int BatchExecute(List<string> sqlList)
    {
      int nCount = 0;
      OleDbConnection connection = new ConnectionObj().GetConnection();
      OleDbTransaction trans = connection.BeginTransaction();
      string cursql = "";
      try
      {

        OleDbCommand command = new OleDbCommand();
        command.Connection = connection;
        command.Transaction = trans;

        foreach (string sql in sqlList)
        {
          cursql = sql;

          command.CommandText = sql;
          nCount += command.ExecuteNonQuery();
        }


        trans.Commit();
       
      }
      catch (Exception ex)
      {
        nCount = 0;
        trans.Rollback();
        Log.WriteLog(ex.Message + " : DB.Execute执行错误 : " + cursql);
      }
      finally
      {
        trans.Dispose();
      }
      return nCount;
    }
  }
}
