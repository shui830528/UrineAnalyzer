using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  public class DBInputDefaultItem
  {
    private HOSPINFOITEMS itemId;
    public HOSPINFOITEMS ItemID
    {
      get { return itemId; }
      set { itemId = value; }
    }

    private string caption;
    public string Caption
    {
      get { return caption; }
      set { caption = value; }
    }

    public string TypeName
    {
      get
      {
        switch (itemId)
        {

          case HOSPINFOITEMS.HI_PATIEN_TTYPE:
            return "患者类型";

          case HOSPINFOITEMS.HI_SENDCHECK_OFFICE:
            return "送检科室";

          case HOSPINFOITEMS.HI_SENDCHECK_DOCTOR:
            return "送检医师";

          case HOSPINFOITEMS.HI_CLINICAL_DIAGNOSIS:
            return "临床诊断";

          case HOSPINFOITEMS.HI_PATIENT_SEX:
            return "患者性别";

          case HOSPINFOITEMS.HI_AGEUNIT_TYPE:
            return "性别类型";

          case HOSPINFOITEMS.HI_SAMPLE_TYPE:
            return "样本类型";
        }
        return "";
      }
    }

    public bool IsModify
    {
      get;
      set;
    }
  }

  public class InputDefaultModel
  {
    private DB db = new DB();
    

    public void GetInputDefault(IList<DBInputDefaultItem> list)
    {
      list.Clear();
      string sql = "Select * From Tab_DefaultInput";
      try
      {
        IDataReader objReader = null;

        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          DBInputDefaultItem info = new DBInputDefaultItem();

          info.Caption = objReader["Caption"].ToString();
          info.ItemID =  (HOSPINFOITEMS)Convert.ToInt32(objReader["TypeID"]);

          list.Add(info);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " InputDefaultModel.GetInputDefault Error " + sql);
      }
    }

    public bool IsExists(HOSPINFOITEMS id)
    {
      string sql = "Select * From Tab_DefaultInput Where TypeID = "+(int)id;
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
        Log.WriteLog(ex.Message + " InputDefaultModel.IsExists Error " + sql);
      }
      return false;
    }

    public int Save(IList<DBInputDefaultItem> list)
    {
      int nCount = 0;

      List<string> sqlList = new List<string>();

      foreach(DBInputDefaultItem item in list)
      {
        if (item.IsModify)
        {
          string sql = "";
          if (IsExists(item.ItemID))
          {
            sql = "Update Tab_DefaultInput Set Caption = '"+item.Caption+"' Where TypeID = "+(int) item.ItemID;
          }
          else
          {
            sql = "Insert Into Tab_DefaultInput(TypeID,Caption) Values("+(int)item.ItemID+",'"+item.Caption+"')";
          }

          sqlList.Add(sql);
        }
      }

      nCount = db.BatchExecute(sqlList);

      foreach (DBInputDefaultItem item in list)
      {
        if (item.IsModify)
        {
          item.IsModify = false;
        }
      }

      return nCount;
    }

    public int Delete(HOSPINFOITEMS id)
    {
      string sql = "Delete From Tab_DefaultInput Where TypeID = "+(int)id;
      return db.Execute(sql);
    }
  }
}
