using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{
  /// <summary>
  /// 医院信息列表
  /// </summary>
  public enum HOSPINFOITEMS
  {
    /// <summary>
    /// 患者类型
    /// </summary>
    HI_PATIEN_TTYPE,
    /// <summary>
    /// 送检科室
    /// </summary>
    HI_SENDCHECK_OFFICE,
    /// <summary>
    /// 磅检医师
    /// </summary>
    HI_SENDCHECK_DOCTOR,
    /// <summary>
    /// 临床诊断
    /// </summary>
    HI_CLINICAL_DIAGNOSIS,
    /// <summary>
    /// 患者性别
    /// </summary>
    HI_PATIENT_SEX,
    /// <summary>
    /// 年龄类型
    /// </summary>
    HI_AGEUNIT_TYPE,
    /// <summary>
    /// 样本类型
    /// </summary>
    HI_SAMPLE_TYPE
  }

  public class DBHospInfoItem
  {
    private string caption;
    public string Caption
    {
      get { return caption; }
      set { caption = value; }
    }

    private string code;
    public string Code
    {
      get { return code; }
      set { code = value; }
    }

    public bool IsModify
    {
      get;
      set;
    }
  }

  public class DBHospInfoItemType
  {
    private HOSPINFOITEMS itemID;
    public HOSPINFOITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    public string Caption
    {
      get
      {
        switch (itemID)
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
            return "年龄类型";

          case HOSPINFOITEMS.HI_SAMPLE_TYPE:
            return "样本类型";
        }
        return "";
      }
    }

    private ObservableCollection<DBHospInfoItem> items = new ObservableCollection<DBHospInfoItem>();
    public IList<DBHospInfoItem> Items
    {
      get { return items; }
    }
  }

  public class HospInfoModel
  {
    private DB db = new DB();
    public void LoadTypes(Dictionary<int, Dictionary<string,string>>  types)
    {
      types.Clear();
      string sql = "Select * From Tab_OptionNames Order by TypeID";
      try
      {
        IDataReader objReader = null;

        db.Query(sql, out objReader);
        int typeID = -1;
        while (objReader != null && objReader.Read())
        {
          if(typeID != Convert.ToInt32(objReader["TypeID"]))
          {
            typeID = Convert.ToInt32(objReader["TypeID"]);
            Dictionary<string, string> type = new Dictionary<string, string>();
            type.Add(objReader["Code"].ToString(), objReader["TypeName"].ToString());
            types.Add(typeID, type);
          }
          else
          {
            types[typeID].Add(objReader["Code"].ToString(), objReader["TypeName"].ToString());
          }
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " HospInfoModel.LoadTypes Error " + sql);
      }
    }
    public void LoadTypeItem(DBHospInfoItemType item)
    {
      item.Items.Clear();
      string sql = "Select * From Tab_OptionNames Where TypeID = " + (int)item.ItemID;
      try
      {
        IDataReader objReader = null;

        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          DBHospInfoItem info = new DBHospInfoItem();

          info.Caption = objReader["TypeName"].ToString();
          info.Code = objReader["Code"].ToString();

          item.Items.Add(info);
        }
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + " HospInfoModel.LoadTypeItem Error " + sql);
      }
    }
    public bool IsExistsFromName(HOSPINFOITEMS id,DBHospInfoItem item)
    {
      string sql = "Select * From Tab_OptionNames Where TypeID = "+(int)id + " and TypeName = '"+ item.Caption.Trim() +"'";
      IDataReader objReader = null;

      db.Query(sql, out objReader);

      while (objReader != null && objReader.Read())
      {
        return true;
      }
        return false;
    }

    public bool IsExistsFromCode(HOSPINFOITEMS id, DBHospInfoItem item)
    {
      string sql = "Select * From Tab_OptionNames Where TypeID = " + (int)id + " and Code = '" + item.Code.Trim() + "'";
      IDataReader objReader = null;

      db.Query(sql, out objReader);

      while (objReader != null && objReader.Read())
      {
        return true;
      }
      return false;
    }

    public int Save(DBHospInfoItemType item)
    {
      int nCount = 0;
      List<string> sqlList = new List<string>();
      foreach(DBHospInfoItem info in item.Items)
      {
        if (info.IsModify)
        {
          string str = "Insert Into Tab_OptionNames(TypeID,TypeName,Code) values("
            + (int)item.ItemID +",'"+ info.Caption +"','"+ info.Code +"')";

          sqlList.Add(str);
        }
      }

      nCount = db.BatchExecute(sqlList);
      if (nCount > 0)
      {
        foreach (DBHospInfoItem info in item.Items)
        {
          if (info.IsModify)
          {
            info.IsModify = false;
          }
        }
      }
      return nCount;
    }

    public int Delete(HOSPINFOITEMS id,DBHospInfoItem item)
    {
      string str = "Delete From Tab_OptionNames Where TypeID = " + (int)id + " and TypeName = '"+ item.Caption +"' and Code = '"+ item.Code +"'";
      return db.Execute(str);
    }

  }
}
