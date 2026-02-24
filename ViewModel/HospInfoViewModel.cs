using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{

  public class HospInfoViewModel
  {
    private HospInfoModel model = new HospInfoModel();
    private List<DBHospInfoItemType> typeItems = new List<DBHospInfoItemType>();
    public IList<DBHospInfoItemType> TypeItems
    {
      get { return typeItems; }
    }


    public HospInfoViewModel()
    {
      foreach(HOSPINFOITEMS id in Enum.GetValues(typeof(HOSPINFOITEMS)))
      {
        DBHospInfoItemType itemType = new DBHospInfoItemType() { ItemID = id };
        LoadTypeItem(itemType);
        typeItems.Add(itemType);
      }

      
    }


    public void LoadTypeItem(DBHospInfoItemType item)
    {
      model.LoadTypeItem(item);
    }

    public bool Save(DBHospInfoItemType item)
    {
      foreach (DBHospInfoItem info in item.Items)
      {
        if (info.IsModify && (model.IsExistsFromName(item.ItemID,info) || model.IsExistsFromCode(item.ItemID,info)))
        {
          return false;
        }
      }

      

      return model.Save(item) > 0;
    }

    public bool Delete(HOSPINFOITEMS id, DBHospInfoItem item)
    {
      return model.Delete(id,item) > 0;
    }
  }
}
