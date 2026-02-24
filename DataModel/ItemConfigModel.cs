using System;
using System.Collections.Concurrent;
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
  /// <summary>
  /// 干化学单位类型
  /// </summary>
  public class DBChemicalItemUnitType
  {
    private EM_ITEM_UNITTYPE unitTypeID;
    /// <summary>
    /// 单位类型
    /// </summary>
    public EM_ITEM_UNITTYPE UnitTypeID
    {
      get { return unitTypeID; }
      set { unitTypeID = value; }
    }

    private string caption;
    public string Caption
    {
      get { return caption; }
      set { caption = value; }
    }

    private ObservableCollection<DBChemicalItemInfo> items = new ObservableCollection<DBChemicalItemInfo>();
    public IList<DBChemicalItemInfo> Items
    {
      get { return items; }
    }
  }

  /// <summary>
  /// 干化学项
  /// </summary>
  public class DBChemicalItemInfo : INotifyPropertyChanged
  {
    private EM_CHEMICAL_ITEMS itemID;
    /// <summary>
    /// 项目ID
    /// </summary>
    public EM_CHEMICAL_ITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    private string name;
    /// <summary>
    /// 项目名称
    /// </summary>
    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    private string caption;
    /// <summary>
    /// 显示名称
    /// </summary>
    public string Caption
    {
      get { return caption; }
      set { caption = value; OnPropertyChanged("Caption"); }
    }

    private int itemOrder;
    /// <summary>
    /// 显示顺序
    /// </summary>
    public int ItemOrder
    {
      get { return itemOrder; }
      set { itemOrder = value; OnPropertyChanged("ItemOrder"); }
    }

    private string unit;
    /// <summary>
    /// 单位
    /// </summary>
    public string Unit
    {
      get { return unit; }
      set { unit = value; OnPropertyChanged("Unit"); }
    }
    private string range;
    /// <summary>
    /// 范围
    /// </summary>
    public string Range
    {
      get { return range; }
      set { range = value; OnPropertyChanged("Range"); }
    }

    private bool isVisable;
    public bool IsVisable
    {
      get { return isVisable; }
      set { isVisable = value; }
    }

    private bool isModify;
    public bool IsModify
    {
      get { return isModify; }
      set { isModify = value; }
    }

    private ObservableCollection<DBChemicalResultInfo> items = new ObservableCollection<DBChemicalResultInfo>();
    public IList<DBChemicalResultInfo> Items
    {
      get { return items; }
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

  public class DBChemicalResultInfo : INotifyPropertyChanged
  {
    private EM_ITEM_UNITTYPE unitType;
    public EM_ITEM_UNITTYPE UnitType { get { return unitType; } set { unitType = value; } }
    private EM_CHEMICAL_ITEMS itemID;
    /// <summary>
    /// 项目ID
    /// </summary>
    public EM_CHEMICAL_ITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }

    private int index;
    public int Index
    {
      get { return index; }
      set { index = value; OnPropertyChanged("Index"); }
    }

    private string result;
    public string Result
    {
      get { return result; }
      set { result = value; OnPropertyChanged("Result"); }
    }

    private bool isAbnormal;
    public bool IsAbnormal
    {
      get { return isAbnormal; }
      set { isAbnormal = value; OnPropertyChanged("IsAbnormal"); }
    }

    private bool isModify;
    public bool IsModify
    {
      get { return isModify; }
      set { isModify = value; }
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
  public class DBItemConfigModel
  {
    private static DB db = new DB();
    private static ObservableCollection<DBChemicalItemUnitType> items = new ObservableCollection<DBChemicalItemUnitType>();
    private Dictionary<string, int> itemNos = new Dictionary<string, int>();
    private static EM_ITEM_UNITTYPE curUnitType = EM_ITEM_UNITTYPE.EM_ITEM_UNITTYPE_INTERNATIONAL_PLUS_ON;

    public static EM_ITEM_UNITTYPE CurUnitType
    {
      get { return curUnitType; }
      set { curUnitType = value; }
    }
    public IList<DBChemicalItemUnitType> Items
    {
      get { return items; }
    }
    public Dictionary<string,int> ItemNos
    {
      get { return itemNos; }
    }
    private void GetCurrentUnitType()
    {
      string sql = "select * from Tab_Unit where Used = true";
      DataSet dataSet = new DataSet();
      db.Query(sql, out dataSet);
      if (dataSet.Tables[0].Rows.Count > 0)
      {
        curUnitType = (EM_ITEM_UNITTYPE)(dataSet.Tables[0].Rows[0]["UnitID"]);
      }
    }
    public void SetCurrentUnitType(EM_ITEM_UNITTYPE unitType)
    {
      List<string> sqlList = new List<string>();
      sqlList.Add("update Tab_Unit set Used = 0 ");
      sqlList.Add("update Tab_Unit set Used = 1 where UnitID = "+(int)unitType);
      db.BatchExecute(sqlList);
    }
    public void AddItemResult(DBChemicalResultInfo resultInfo)
    {
      List<string> sqlList = new List<string>();
      foreach(DBChemicalItemUnitType iut in items)
      {
        sqlList.Add("insert into Tab_ItemResultConfig(UnitType,ItemID,ResultIndex,ResultIndex) values(" + (int)iut.UnitTypeID + "," + (int)resultInfo.ItemID + "," + resultInfo.Index + ",'" + resultInfo.Result + "')");
      }
      db.BatchExecute(sqlList);
    }
    public void DeleteItemResult(DBChemicalResultInfo resultInfo)
    {
      string sql = "delete from Tab_ItemResultConfig where UnitType = "+(int)resultInfo.UnitType+" and ItemID = "+(int)resultInfo.ItemID+ " and ResultIndex = "+resultInfo.Index;
      db.Execute(sql);
    }
    public bool SaveItemResultConfig(DBChemicalItemInfo item)
    {
      List<string> sqlList = new List<string>();
      sqlList.Add("delete from Tab_ItemResultConfig where UnitType = "+ (int)DBItemConfigModel.CurUnitType+" and ItemID = "+ (int)item.ItemID);
      foreach (DBChemicalResultInfo ri in item.Items)
      {
        sqlList.Add("insert into Tab_ItemResultConfig(UnitType,ItemID,ResultIndex,Result) values(" + (int)ri.UnitType + "," + (int)ri.ItemID + "," + ri.Index + ",'" + ri.Result + "')");
      }
      return db.BatchExecute(sqlList) > 0;
    }
    public bool SaveUnitTypeItemConfig(DBChemicalItemUnitType unitType)
    {
      List<string> sqlList = new List<string>();
      foreach (DBChemicalItemInfo ii in unitType.Items)
      {
        sqlList.Add("update Tab_ItemConfig set ItemUnit = '"+ii.Unit+"', ItemRange = '"+ii.Range+"' where UnitType = "+(int)unitType.UnitTypeID+" and ID = "+(int)ii.ItemID);
      }
      return db.BatchExecute(sqlList) > 0;
    }
    private void GetItemConfig()
    {
      string sql = "select * from Tab_Item";
      DataSet itemNoDataSet = new DataSet();
      db.Query(sql, out itemNoDataSet);
      foreach(DataRow dr in itemNoDataSet.Tables[0].Rows)
      {
        itemNos.Add((string)(dr["ItemID"]), (int)(dr["ItemNo"]));
      }

      sql = "select * from Tab_Unit";
      DataSet unitDataSet = new DataSet();
      db.Query(sql, out unitDataSet);
      List<DBChemicalItemUnitType> unitTypeList = new List<DBChemicalItemUnitType>();
      foreach(DataRow dr in unitDataSet.Tables[0].Rows)
      {
        unitTypeList.Add(new DBChemicalItemUnitType() { UnitTypeID = (EM_ITEM_UNITTYPE)(dr["UnitID"]), Caption = (string)(dr["UnitName"]) });
      }
      sql = "select Tab_ItemConfig.*,ItemNameCN from Tab_ItemConfig left join Tab_Item on Tab_ItemConfig.ItemName = Tab_Item.ItemID order by UnitType, ID";
      DataSet dataSet = new DataSet();
      db.Query(sql, out dataSet);
      sql = "select * from Tab_ItemResultConfig order by UnitType,ItemID,ResultIndex";
      DataSet resultDataSet = new DataSet();
      db.Query(sql, out resultDataSet);
      List<DBChemicalResultInfo> resutlInfos = new List<DBChemicalResultInfo>();
      foreach(DataRow dr in resultDataSet.Tables[0].Rows)
      {
        DBChemicalResultInfo ri = new DBChemicalResultInfo();
        ri.UnitType = (EM_ITEM_UNITTYPE)(dr["UnitType"]);
        ri.ItemID = (EM_CHEMICAL_ITEMS)(dr["ItemID"]);
        ri.Index = (int)(dr["ResultIndex"]);
        ri.Result = (string)(dr["Result"]);
        ri.IsAbnormal = (bool)(dr["IsAbnormal"]);
        resutlInfos.Add(ri);
      }
      EM_ITEM_UNITTYPE unitType = EM_ITEM_UNITTYPE.EM_ITEM_UNITTYPE_CUSTOM;
      DBChemicalItemUnitType uType = null;
      items.Clear();
      foreach (DataRow dr in dataSet.Tables[0].Rows)
      {
        if (unitType != (EM_ITEM_UNITTYPE)(dr["UnitType"]))
        {
          unitType = (EM_ITEM_UNITTYPE)(dr["UnitType"]);
          uType = new DBChemicalItemUnitType();
          uType.UnitTypeID = unitType;
          uType.Caption = unitTypeList.Single(ut => ut.UnitTypeID == unitType).Caption;
          items.Add(uType);
        }
        DBChemicalItemInfo item = new DBChemicalItemInfo();
        item.ItemID = (EM_CHEMICAL_ITEMS)(dr["ID"]);
        item.Name = (string)(dr["ItemName"]);
        item.Caption = dr["ItemNameCN"] is DBNull ? string.Empty: (string)(dr["ItemNameCN"]);
        item.Unit = dr["ItemUnit"] is DBNull ? string.Empty : (string)(dr["ItemUnit"]);
        item.ItemOrder = (int)(dr["ItemOrd"]);
        item.Range = dr["ItemRange"] is DBNull ? string.Empty : (string)(dr["ItemRange"]);
        IEnumerable<DBChemicalResultInfo> itemReuslt =  resutlInfos.Where(r => r.UnitType == unitType && r.ItemID == item.ItemID);
        foreach(DBChemicalResultInfo ri in itemReuslt)
        {
          item.Items.Add(new DBChemicalResultInfo() { UnitType = ri.UnitType, ItemID = ri.ItemID, Index = ri.Index, Result = ri.Result, IsAbnormal = ri.IsAbnormal });
        }
        uType.Items.Add(item);
      }
    }
    public DBItemConfigModel()
    {
      GetCurrentUnitType();
      GetItemConfig();
    }
    public DBChemicalItemUnitType GetUnitType(EM_ITEM_UNITTYPE nUntiType)
    {
      foreach (DBChemicalItemUnitType item in items)
      {
        if (item.UnitTypeID == nUntiType)
        {
          return item;
        }
      }
      return null;
    }
  }
}
