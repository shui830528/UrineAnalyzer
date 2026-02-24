using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  public class DBQCInfo
  {
    private DateTime qcDate = DateTime.Now;
    public DateTime QCDate
    {
      get { return qcDate; }
      set { qcDate = value; }
    }

    private string operation;
    public string Operation
    {
      get { return operation; }
      set { operation = value; }
    }
    public List<DBQCResultInfo> Items
    {
      get;
      set;
    }
  }

  public class DBQCResultInfo : INotifyPropertyChanged
  {
    private DateTime qcDate = DateTime.Now;
    public DateTime QCDate
    {
      get { return qcDate; }
      set { qcDate = value; }
    }

    private bool isNeg = false;
    public bool IsNeg
    {
      get { return isNeg; }
      set { isNeg = value; }
    }

    private EM_CHEMICAL_ITEMS itemID;
    public EM_CHEMICAL_ITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    private int itemIndex;
    public int ItemIndex
    {
      get { return itemIndex; }
      set { itemIndex = value; }
    }

    private string abnormal;
    public string Abnormal
    {
      get { return abnormal; }
      set { abnormal = value; }
    }

    private string name;
    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    public string Caption
    {
      get;
      set;
    }

    private string result;
    public string Result
    {
      get { return result; }
      set { result = value; }
    }

    private string unit;
    public string Unit
    {
      get { return unit; }
      set { unit = value; }
    }

    private string range;
    public string Range
    {
      get { return range; }
      set { range = value; }
    }

    private string operation;
    public string Operation
    {
      get { return operation; }
      set { operation = value; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }

  public class QCResultModel
  {
    private DB db = new DB();

    private void CopyResultInfo(DBQCResultInfo info, IDataReader objReader)
    {
      info.QCDate = objReader["QCDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["QCDate"]);
      info.IsNeg = objReader["IsNeg"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsNeg"]);
      info.ItemID = (EM_CHEMICAL_ITEMS)(objReader["ItemID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ItemID"]));
      info.ItemIndex = objReader["ItemIndex"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ItemIndex"]);
      info.Abnormal = objReader["ItemAbnormal"].ToString();
      info.Name = objReader["ItemName"].ToString();
      info.Result = objReader["ItemResult"].ToString();
      info.Unit = objReader["ItemUnit"].ToString();
      info.Range = objReader["ItemRange"].ToString();
      info.Operation = objReader["Operator"].ToString();

    }
    public void GetResult(string sql,IList<DBQCResultInfo> resultList)
    {
      try
      {
        resultList.Clear();
        IDataReader objReader = null;

        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          DBQCResultInfo info = new DBQCResultInfo();

          CopyResultInfo(info, objReader);

          resultList.Add(info);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " QCResultModel.GetResult Error " + sql);
      }
    }

    public int Delete(DBQCResultInfo resultInfo)
    {
      string sql = "Delete From Tab_QCResult Where QCDate = #" +
        resultInfo.QCDate.ToString(Const.DateTimeFormat) + "# and IsNeg = " + (resultInfo.IsNeg ? "true" : "false");
      return db.Execute(sql);
    }

    public int Insert(IList<DBQCResultInfo> resultList)
    {
      List<string> sqlList = new List<string>();

      foreach(DBQCResultInfo item in resultList)
      {
        string sql = "Insert Into Tab_QCResult(QCDate,IsNeg,ItemID,ItemIndex,ItemAbnormal,ItemName,ItemResult,ItemUnit,ItemRange,Operator) values("+
          "#" + item.QCDate.ToString(Const.DateTimeFormat) + "#," +
          (item.IsNeg ? "true":"false") + "," +
          (int)item.ItemID + "," +
          item.ItemIndex + "," +
          "'" + item.Abnormal + "'," +
          "'" + item.Name + "'," +
          "'" + item.Result + "'," +
          "'" + item.Unit + "'," +
          "'" + item.Range + "'," +
          "'" + item.Operation + "'"
          +")";
        sqlList.Add(sql);

      }

      return db.BatchExecute(sqlList);
    }

  }
}
