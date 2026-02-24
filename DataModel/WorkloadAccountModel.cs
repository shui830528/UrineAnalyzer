using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  public enum WorkloadType
  {
    wtSendCheckOffice,
    wtSendCheckDoctor,
    wtCheckPersion
  }
  
  public class WorkloadItem
  {
    private WorkloadType workloadType;
    public WorkloadType WorkloadType
    {
      get { return workloadType; }
      set { workloadType = value; }
    }

    private string typeCaption;
    public string TypeCaption
    {
      get { return typeCaption; }
      set { typeCaption = value; }
    }

    private string itemName;
    public string ItemName
    {
      get { return itemName; }
      set { itemName = value; }
    }

    private int count;
    public int Count
    {
      get { return count; }
      set { count = value; }
    }
  }

  public class WorkloadAccountModel
  {
    private DB db = new DB();

    private IList<WorkloadItem> GetWorkloadTypeList(string StartDate, string EndDate, WorkloadType WorkType )
    {
      List<WorkloadItem> tempList = new List<WorkloadItem>();

      string sql = "";
      string strWhere = " Where RegistDate >= #"+ StartDate +"# and RegistDate <= #"+ EndDate +"# ";
      if (WorkType == WorkloadType.wtSendCheckOffice)
      {
        sql = "Select SendCheckOffice as ItemName,Count(SendCheckOffice) as Num  From Tab_SampleInfo "+ strWhere + " Group By SendCheckOffice";
      }
      else if (WorkType == WorkloadType.wtSendCheckDoctor)
      {
        sql = "Select SendCheckDoctor as ItemName,Count(SendCheckDoctor) as Num  From Tab_SampleInfo " + strWhere + " Group By SendCheckDoctor";
      }
      else
      {
        sql = "Select CheckDoctor as ItemName,Count(CheckDoctor) as Num  From Tab_SampleInfo " + strWhere + " Group By CheckDoctor";
      }

      try
      {
        IDataReader objReader = null;

        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          WorkloadItem info = new WorkloadItem();

          info.WorkloadType = WorkType;
          info.ItemName = objReader["ItemName"].ToString();
          info.Count = objReader["Num"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["Num"]);

          tempList.Add(info);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " WorkloadAccountModel.GetWorkloadTypeList Error " + sql);
      }

      return tempList;
    }
    public void GetWorkloadList(string StartDate,string EndDate, IList<WorkloadItem> WorkloadList)
    {
      WorkloadList.Clear();
      IList<WorkloadItem> tempList = null;
      tempList = GetWorkloadTypeList(StartDate,EndDate, WorkloadType.wtSendCheckOffice);
      foreach(WorkloadItem item in tempList)
      {
        if (item == tempList.First())
        {
          item.TypeCaption = "送检科室";
        }
        if (string.IsNullOrEmpty(item.ItemName))
          item.ItemName = "空";
        WorkloadList.Add(item);
      }
      tempList = GetWorkloadTypeList(StartDate, EndDate, WorkloadType.wtSendCheckDoctor);
      foreach (WorkloadItem item in tempList)
      {
        if (item == tempList.First())
        {
          item.TypeCaption = "送检医师";

        }
        if (string.IsNullOrEmpty(item.ItemName))
          item.ItemName = "空";
        WorkloadList.Add(item);
      }
      tempList = GetWorkloadTypeList(StartDate, EndDate, WorkloadType.wtCheckPersion);
      foreach (WorkloadItem item in tempList)
      {
        if (item == tempList.First())
        {
          item.TypeCaption = "检验人";
        }
        if (string.IsNullOrEmpty(item.ItemName))
          item.ItemName = "空";
        WorkloadList.Add(item);
      }

    }

  }
}
