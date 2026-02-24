using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
  public class DBLogInfo
  {
    public int No { get; set; }
    public string DateTime { get; set; }
    public string Content { get; set; }
    public string User { get; set; }
  }
  public class LogModel
  {
    private DB db = new DB();
    public void GetUserNames(ref ObservableCollection<string> userNameList)
    {
      string sql = "select * from Tab_User where UserName <> 'admin'";
      userNameList.Clear();
      IDataReader objReader = null;
      db.Query(sql, out objReader);
      userNameList.Add(string.Empty);
      while (objReader != null && objReader.Read())
      {
        userNameList.Add(objReader["UserName"].ToString());
      }
    }
    public void Query(string Date, string user, ref ObservableCollection<DBLogInfo> logList)
    {
      string sql;
      if (user.Trim() == string.Empty)
      {
        sql = string.Format("SELECT * FROM Tab_Log Where LogDate >= #{0} 00:00:00# and LogDate <= #{0} 23:59:59#", Date);
      }
      else
      {
        sql = string.Format("SELECT * FROM Tab_Log Where LogDate >= #{0} 00:00:00# and LogDate <= #{0} 23:59:59# and Operator = '{1}'", Date, user);
      }
      
      logList.Clear();
      IDataReader objReader = null;

      db.Query(sql, out objReader);
      int i = 1;
      while (objReader != null && objReader.Read())
      {
        DBLogInfo info = new DBLogInfo();
        info.No = i;
        info.Content = objReader["LogText"].Equals(DBNull.Value) ? "" : (objReader["LogText"].ToString());
        info.User = objReader["Operator"].Equals(DBNull.Value) ? "" : (objReader["Operator"].ToString());
        info.DateTime = objReader["LogDate"].Equals(DBNull.Value) ? string.Empty : (Convert.ToDateTime(objReader["LogDate"]).ToString());
        i++;
        logList.Add(info);
      }
    }
  }
}
