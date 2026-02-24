using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  public class DBSampleResultInfo : INotifyPropertyChanged
  {
    private int sampleID;
    public int SampleID
    {
      get { return sampleID; }
      set { sampleID = value; }
    }

    private DateTime registDate;
    public DateTime RegistDate
    {
      get { return registDate; }
      set { registDate = value; }
    }

    private EM_CHEMICAL_ITEMS itemID;
    /// <summary>
    /// 项目ID
    /// </summary>
    public EM_CHEMICAL_ITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    private string abnormal;
    public string Abnormal
    {
      get { return abnormal; }
      set { abnormal = value; OnPropertyChanged("Abnormal"); }
    }

    private string caption;
    public string Caption
    {
      get { return caption; }
      set { caption = value; OnPropertyChanged("Caption"); }
    }

    private string name;
    public string Name
    {
      get { return name; }
      set { name = value; OnPropertyChanged("Name"); }
    }

    private string result;
    public string Result
    {
      get { return result; }
      set { result = value; OnPropertyChanged("Result"); }
    }
    private string unit;
    public string Unit
    {
      get { return unit; }
      set { unit = value; OnPropertyChanged("Unit"); }
    }

    private string range;
    public string Range
    {
      get { return range; }
      set { range = value; OnPropertyChanged("Range"); }
    }
    public int ResultIndex { get; set; }
    private bool isModify;
    public bool IsModify
    {
      get { return isModify; }
      set { isModify = value; }
    }

    public List<DBChemicalItemInfo> ChemicalItemInfoList
    {
      get;
      set;
    }

    private ObservableCollection<DBChemicalResultInfo> resultInfoList = new ObservableCollection<DBChemicalResultInfo>();
    public IList<DBChemicalResultInfo> ResultInfoList
    {
      get { return resultInfoList; }
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
  public class SampleResultModel
  {
    private DB db = new DB();

    private void CopyResultInfo(DBSampleResultInfo info, IDataReader objReader)
    {

      info.SampleID = objReader["SampleID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["SampleID"]);
      info.RegistDate = objReader["RegistDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["RegistDate"]);
      info.ItemID = (EM_CHEMICAL_ITEMS)(objReader["ItemID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ItemID"]));
      info.Abnormal = objReader["ItemAbnormal"].ToString();
      info.Name = objReader["ItemName"].ToString();
      info.Result = objReader["ItemResult"].ToString();
      info.Unit = objReader["ItemUnit"].ToString();
      info.Range = objReader["ItemRange"].ToString();
      info.ResultIndex = int.Parse(objReader["ResultIndex"].ToString());

    }
    protected void GetResult(string sql, IList<DBSampleResultInfo> resultList)
    {
      try
      {
        resultList.Clear();
        IDataReader objReader = null;

        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          DBSampleResultInfo info = new DBSampleResultInfo();

          CopyResultInfo(info, objReader);

          resultList.Add(info);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " SampleResultModel.GetResult Error " + sql);
      }
    }
    public void GetAnalyzerResult(DBSampleInfo info,IList<DBSampleResultInfo> resultList)
    {
      string sql = "Select * From Tab_SampleAnalyzerResult Where SampleID = " + info.SampleID + " and RegistDate = #" + info.RegistDate.ToString(Const.DateTimeFormat) + "#";
      GetResult(sql,resultList);
    }

    public void GetManualResult(DBSampleInfo info, IList<DBSampleResultInfo> resultList)
    {
      string sql = "Select * From Tab_SampleManualResult Where SampleID = " + info.SampleID + " and RegistDate = #" + info.RegistDate.ToString(Const.DateTimeFormat) + "#";
      GetResult(sql, resultList);
    }

    public bool IsAnalyzerResultExists(DBSampleResultInfo resultInfo)
    {
      string sql = "Select * From Tab_SampleAnalyzerResult Where SampleID = " + resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" + 
        " and ItemID = " + (int)resultInfo.ItemID;
      try
      {
        IDataReader objReader = null;
        db.Query(sql, out objReader);
        while (objReader != null && objReader.Read())
        {
          return true;
        }

      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " SampleResultModel.IsExists " + sql);
      }
      return false;
    }

    public bool IsManualResultExists(DBSampleResultInfo resultInfo)
    {
      string sql = "Select * From Tab_SampleManualResult Where SampleID = " + resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" + 
        " and ItemID = " + (int)resultInfo.ItemID;

      try
      {
        IDataReader objReader = null;
        db.Query(sql, out objReader);
        while (objReader != null && objReader.Read())
        {
          return true;
        }

      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " SampleResultModel.IsExists " + sql);
      }
      return false;
    }

    public int InsertAnalyzerResult(IList<DBSampleResultInfo> resultList)
    {
      int nCount = 0;
      List<string> sqlList = new List<string>();
      foreach (DBSampleResultInfo item in resultList)
      {
        //string sql = "Insert Into Tab_SampleAnalyzerResult(SampleID,RegistDate,ItemID,ItemAbnormal,ItemName,ItemResult,ItemUnit,ItemRange) Values(" +
        //  item.SampleID + "," +
        //  "#" + item.RegistDate.ToString(Const.DateFormatting + " " + Const.TimeFormatting) + "#," +
        //  "" + (int)item.ItemID + "," +
        //  "'" + item.Abnormal + "'," +
        //  "'" + item.Name + "'," +
        //  "'" + item.Result + "'," +
        //  "'" + item.Unit + "'," +
        //  "'" + item.Range + "'" +
        //  ")";
        string sql = GetUpdateAnalyzerResultSQL(item);
        sqlList.Add(sql);
      }
      nCount = db.BatchExecute(sqlList);
      return nCount;
    }
    private string GetUpdateAnalyzerResultSQL(DBSampleResultInfo resultInfo)
    {
      string sql = "";
      if (IsAnalyzerResultExists(resultInfo))
      {
        sql = "Update Tab_SampleAnalyzerResult Set ItemAbnormal = '" + resultInfo.Abnormal + "',ItemResult = '" + resultInfo.Result + "'," +
          "ItemName = '" + resultInfo.Name + "',ItemUnit = '" + resultInfo.Unit + "',ItemRange = '" + resultInfo.Range + "' Where SampleID = " +
          resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" +
        " and ItemID = " + (int)resultInfo.ItemID+" and ResultIndex = "+resultInfo.ResultIndex;
      }
      else
      {
        sql = "Insert Into Tab_SampleAnalyzerResult(SampleID,RegistDate,ItemID,ItemAbnormal,ItemName,ItemResult,ItemUnit,ItemRange,ResultIndex) Values(" +
          resultInfo.SampleID + "," +
          "#" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#," +
          "" + (int)resultInfo.ItemID + "," +
          "'" + resultInfo.Abnormal + "'," +
          "'" + resultInfo.Name + "'," +
          "'" + resultInfo.Result + "'," +
          "'" + resultInfo.Unit + "'," +
          "'" + resultInfo.Range + "'," +
          resultInfo.ResultIndex+
          ")";
      }
      return sql;
    }
    public int UpdateAnalyzerResult(DBSampleResultInfo resultInfo)
    {
      int nCount = 0;
      string sql = "";
      if (IsAnalyzerResultExists(resultInfo))
      {
        sql = "Update Tab_SampleAnalyzerResult Set ItemAbnormal = '" + resultInfo.Abnormal + "',ItemResult = '" + resultInfo.Result + "',"+
          "ItemName = '" + resultInfo.Name + "',ItemUnit = '" + resultInfo.Unit + "',ItemRange = '" + resultInfo.Range + "' Where SampleID = " + 
          resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" +
        " and ItemID = " + (int)resultInfo.ItemID+" and ResultIndex = "+resultInfo.ResultIndex;
      }
      else
      {
        sql = "Insert Into Tab_SampleAnalyzerResult(SampleID,RegistDate,ItemID,ItemAbnormal,ItemName,ItemResult,ItemUnit,ItemRange,ResultIndex) Values(" + 
          resultInfo.SampleID + "," +
          "#" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#," +
          "" + (int)resultInfo.ItemID + "," + 
          "'" + resultInfo.Abnormal + "'," + 
          "'" + resultInfo.Name + "'," + 
          "'" + resultInfo.Result + "'," +
          "'" + resultInfo.Unit + "'," +
          "'" + resultInfo.Range + "'," +
          resultInfo.ResultIndex+
          ")";
      }

      nCount = db.Execute(sql);

      return nCount;
    }

    public int UpdateManualResult(DBSampleResultInfo resultInfo)
    {
      int nCount = 0;
      string sql = "";
      if (IsManualResultExists(resultInfo))
      {
        sql = "Update Tab_SampleManualResult Set ItemAbnormal = '" + resultInfo.Abnormal + "',ItemResult = '" + resultInfo.Result + "'," +
          "ItemName = '" + resultInfo.Name + "',ItemUnit = '" + resultInfo.Unit + "',ItemRange = '" + resultInfo.Range + "' Where SampleID = " +
          resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" +
        " and ItemID = " + (int)resultInfo.ItemID+" and ResultIndex = "+resultInfo.ResultIndex;
      }
      else
      {
        sql = "Insert Into Tab_SampleManualResult(SampleID,RegistDate,ItemID,ItemAbnormal,ItemName,ItemResult,ItemUnit,ItemRange, ResultIndex) Values(" +
          resultInfo.SampleID + "," +
          "#" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#," +
          "" + (int)resultInfo.ItemID + "," +
          "'" + resultInfo.Abnormal + "'," +
          "'" + resultInfo.Name + "'," +
          "'" + resultInfo.Result + "'," +
          "'" + resultInfo.Unit + "'," +
          "'" + resultInfo.Range + "'," +
          resultInfo.ResultIndex+
          ")";
      }

      nCount = db.Execute(sql);

      return nCount;
    }

    public int DeleteAnalyzerResult(DBSampleResultInfo resultInfo)
    {
      string sql = "Delete From Tab_SampleAnalyzerResult Where SampleID = " +
          resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" +
        " and ItemID = " + (int)resultInfo.ItemID;
      return db.Execute(sql);
    }

    public int DeleteAllAnalyzerResult(int nSampleNo,DateTime RegistDate)
    {
      string sql = "Delete From Tab_SampleAnalyzerResult Where SampleID = " +
        nSampleNo + " and RegistDate = #" + RegistDate.ToString(Const.DateFormat) + "#";
      return db.Execute(sql);
    }

    public int DeleteManualResult(DBSampleResultInfo resultInfo)
    {
      string sql = "Delete From Tab_SampleManualResult Where SampleID = " +
          resultInfo.SampleID + " and RegistDate = #" + resultInfo.RegistDate.ToString(Const.DateTimeFormat) + "#" +
        " and ItemID = " + (int)resultInfo.ItemID;
      return db.Execute(sql);
    }
  }
}
