using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
  public class InputDefaultViewModel
  {
    private InputDefaultModel model = new InputDefaultModel();
    private ObservableCollection<DBInputDefaultItem> items = new ObservableCollection<DBInputDefaultItem>();

    public IList<DBInputDefaultItem> Items
    {
      get { return items; }
    }

    public InputDefaultViewModel()
    {
      Update();
      if (items.Count == 0)
      {
        foreach (HOSPINFOITEMS id in Enum.GetValues(typeof(HOSPINFOITEMS)))
        {
          DBInputDefaultItem itemType = new DBInputDefaultItem() { ItemID = id,IsModify = true };
          items.Add(itemType);
        }

        model.Save(items);
      }
    }


    public void Update()
    {
      model.GetInputDefault(items);
    }

    public bool Save()
    {
      return model.Save(items) > 0;
    }

    public bool Delete(DBInputDefaultItem item)
    {
      return model.Delete(item.ItemID) > 0;
    }

  }
}
