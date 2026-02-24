using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{

  public class DBQCSettingItem
  {
    public bool IsCheck
    {
      get;
      set;
    }

    public EM_CHEMICAL_ITEMS ItemID
    {
      get;
      set;
    }

    public string Name
    {
      get;
      set;
    }

    public string Caption
    {
      get;
      set;
    }

    public bool NegNoEnable
    {
      get;
      set;
    }

    public bool PosNoEnable
    {
      get;
      set;
    }

    public int NegUpperLimit
    {
      get;
      set;
    }

    public int NegLowerLimit
    {
      get;
      set;
    }

    public int PosUpperLimit
    {
      get;
      set;
    }

    public int PosLowerLimit
    {
      get;
      set;
    }

  }

  public class QCSettingModel
  {
    private static XML xml = null;
    private DBItemConfigModel ConfigModel = new DBItemConfigModel();

    public QCSettingModel()
    {
      if (xml == null)
      {
        xml = new XML("Config","QCSetting.xml");
        if (xml.Root.ChildNodes.Count == 0)
        {
          
          DBChemicalItemUnitType UnitType = ConfigModel.GetUnitType(DBItemConfigModel.CurUnitType);
          foreach(DBChemicalItemInfo item in UnitType.Items)
          {
            string key = "ID" +((int)item.ItemID).ToString();
            xml.WriteBool(key,"IsCheck",false);
            xml.WriteString(key,"Name",item.Name);
            xml.WriteBool(key,"NegNoEnable",false);
            xml.WriteBool(key,"PosNoEnable",false);
            xml.WriteInt(key,"NegUpperLimit",-1);
            xml.WriteInt(key, "NegLowerLimit", -1);
            xml.WriteInt(key, "PosUpperLimit", -1);
            xml.WriteInt(key, "PosLowerLimit", -1);
          }
          xml.SaveXml();
        }
      }
    }

    public void Load(IList<DBQCSettingItem> lstItems)
    {
      lstItems.Clear();
      DBChemicalItemUnitType UnitType = ConfigModel.GetUnitType(DBItemConfigModel.CurUnitType);
      foreach (DBChemicalItemInfo item in UnitType.Items)
      {
        DBQCSettingItem qcItem = new DBQCSettingItem();

        string key = "ID"+((int)item.ItemID).ToString();

        qcItem.IsCheck = xml.ReadBool(key, "IsCheck", false);
        qcItem.Name = xml.ReadString(key, "Name", item.Name);
        qcItem.NegNoEnable = xml.ReadBool(key, "NegNoEnable", false);
        qcItem.PosNoEnable = xml.ReadBool(key, "PosNoEnable", false);
        qcItem.NegUpperLimit = xml.ReadInt(key, "NegUpperLimit", -1);
        qcItem.NegLowerLimit = xml.ReadInt(key, "NegLowerLimit", -1);
        qcItem.PosUpperLimit = xml.ReadInt(key, "PosUpperLimit", -1);
        qcItem.PosLowerLimit = xml.ReadInt(key, "PosLowerLimit", -1);
        qcItem.ItemID = item.ItemID;

        lstItems.Add(qcItem);
      }
    }

    public void Save(IList<DBQCSettingItem> lstItems)
    {
      xml.Root.RemoveAll();
      foreach (DBQCSettingItem item in lstItems)
      {
        string key = "ID"+((int)item.ItemID).ToString();
        xml.WriteBool(key, "IsCheck", item.IsCheck);
        xml.WriteString(key, "Name", item.Name);
        xml.WriteBool(key, "NegNoEnable", item.NegNoEnable);
        xml.WriteBool(key, "PosNoEnable", item.NegNoEnable);
        xml.WriteInt(key, "NegUpperLimit", item.NegUpperLimit);
        xml.WriteInt(key, "NegLowerLimit", item.NegLowerLimit);
        xml.WriteInt(key, "PosUpperLimit", item.PosUpperLimit);
        xml.WriteInt(key, "PosLowerLimit", item.PosLowerLimit);
      }

      xml.SaveXml();
    }
  }
}
