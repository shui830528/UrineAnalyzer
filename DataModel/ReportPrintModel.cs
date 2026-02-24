using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  public class ReportPrintModel
  {
    private DB db = new DB();

    public DataSet GetSampleInfo(int SampleID,DateTime dateTime)
    {
      DataSet dataSet = null;
      string sql = "select * from Tab_SampleInfo where SampleID = " + SampleID + " and RegistDate = #" + dateTime.ToString(Const.DateTimeFormat) + "#";
      db.Query(sql, out dataSet);
      return dataSet;
    }
    public DataSet GetSampleInfos(int beginSampleID,int endSampleID, string otherCondition)
    {
      DataSet dataSet = null;
      string sql = "select * from Tab_SampleInfo where (SampleID >= " + beginSampleID+ " and SampleID <= "+endSampleID+") and "+otherCondition;
      db.Query(sql, out dataSet);
      return dataSet;
    }
    public DataSet GetSampleAnalyzerResult(int SampleID,DateTime dateTime)
    {
      DataSet dataSet = new DataSet();
      string sql = "select "+
                      "si.ItemName,item.ItemUnit,item.ItemRange,rc.Result as ItemResult,it.ItemNameCN "+
                   "from "+
                      "(((Tab_SampleAnalyzerResult si inner join Tab_Item it on si.ItemName = it.ItemID) "+
                   "inner join Tab_ItemConfig item on si.ItemName = item.ItemName) "+
                   "inner join Tab_Unit unit on item.UnitType = unit.UnitID) "+
                   "inner join Tab_ItemResultConfig rc on rc.UnitType = unit.UnitID and rc.ItemID = si.ItemID and rc.ResultIndex = si.ResultIndex "+
                   "where unit.Used = true and si.SampleID = "+ SampleID + " and si.RegistDate = #" + dateTime.ToString(Const.DateTimeFormat) + "#";
      db.Query(sql, out dataSet);
      return dataSet;
    }
    public DataSet GetSampleManualResult(int SampleID, DateTime dateTime)
    {
      DataSet dataSet = new DataSet();
      string sql = "select "+
                      "si.ItemName,item.ItemUnit,item.ItemRange,rc.Result as ItemResult,it.ItemNameCN "+
                   "from "+
                      "(((Tab_SampleManualResult si inner join Tab_Item it on si.ItemName = it.ItemID) "+
                   "inner join Tab_ItemConfig item on si.ItemName = item.ItemName) "+
                   "inner join Tab_Unit unit on item.UnitType = unit.UnitID) "+
                   "inner join Tab_ItemResultConfig rc on rc.UnitType = unit.UnitID and rc.ItemID = si.ItemID and rc.ResultIndex = si.ResultIndex "+
                   "where unit.Used = true and si.SampleID = " + SampleID + " and si.RegistDate = #" + dateTime.ToString(Const.DateTimeFormat) + "#";
      db.Query(sql, out dataSet);
      return dataSet;
    }
    public DataSet GetQCData(string ItemName,int iYear,int iMonth)
    {
      DataSet dataSet = new DataSet();
      string sql = "select * from Tab_QCResult where ItemName = '"+ItemName+"' and QCDate in (select Max(QCDate) as maxDate from Tab_QCResult "+
                   "where ItemName = '" + ItemName + "' and Year(QCDate) = " + iYear + " and Month(QCDate) = " + iMonth+" group by ItemID,Day(QCDate))";
      db.Query(sql, out dataSet);
      return dataSet;
    }
    public DataSet GetQCData(string ItemName, int iYear, int iMonth, string otherCondition)
    {
      DataSet dataSet = new DataSet();
      string sql = "select * from Tab_QCResult where ItemName = '" + ItemName + "' and QCDate in (select Max(QCDate) as maxDate from Tab_QCResult "+
                  "where ItemName = '" + ItemName + "' and Year(QCDate) = " + iYear + " and Month(QCDate) = " + iMonth + " and " + otherCondition +" group by ItemID,Day(QCDate))";
      db.Query(sql, out dataSet);
      return dataSet;
    }
  }
}
