using Control;
using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ViewModel
{
  public class ItemConfigViewModel
  {
    private DBItemConfigModel ConfigModel = null;

    public IList<DBChemicalItemUnitType> GetUnitType()
    {
      return ConfigModel.Items;
    }

    public DBChemicalItemUnitType GetCurrentUnitType()
    {
      return ConfigModel.GetUnitType(DBItemConfigModel.CurUnitType);
    }

    public void SetCurrentUnitType(DBChemicalItemUnitType UnitType)
    {
      DBItemConfigModel.CurUnitType = UnitType.UnitTypeID;
      ConfigModel.SetCurrentUnitType(UnitType.UnitTypeID);
    }

    public ItemConfigViewModel()
    {
      ConfigModel = new DBItemConfigModel();
    }
    public DBChemicalItemInfo GetChemicalItem(EM_CHEMICAL_ITEMS nID)
    {
      return ConfigModel.Items.Single(ut => ut.UnitTypeID == DBItemConfigModel.CurUnitType).Items.Single(ii => ii.ItemID == nID);
    }

    public bool SaveItemResult(DBChemicalItemInfo ItemInfo)
    {
      return ConfigModel.SaveItemResultConfig(ItemInfo);
    }
    public bool SaveItemConfig(DBChemicalItemUnitType unitType)
    {
      return ConfigModel.SaveUnitTypeItemConfig(unitType);
    }
    public bool ItemResultDelete(DBChemicalItemInfo Item, DBChemicalResultInfo ItemResult)
    {
      Item.Items.Remove(ItemResult);
      int nIndex = 0;
      foreach (DBChemicalResultInfo item in Item.Items)
      {
        item.Index = nIndex++;
      }
      return ConfigModel.SaveItemResultConfig(Item);
    }

  }
}
