using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
  public class QCSettingViewModel
  {
    private QCSettingModel model = new QCSettingModel();
    private ItemConfigViewModel itemConfigViewModel = new ItemConfigViewModel();
    private ObservableCollection<DBQCSettingItem> items = new ObservableCollection<DBQCSettingItem>();

    public IList<DBQCSettingItem> Items
    {
      get { return items; }
    }
    public void Update()
    {
      model.Load(items);
      foreach(DBQCSettingItem item in items)
      {
        item.Caption = itemConfigViewModel.GetChemicalItem(item.ItemID).Caption;
      }
    }
    public void GetChartItems(IList<DBQCSettingItem> itemList)
    {
      itemList.Clear();
      Update();
      IList<DBQCSettingItem> tempList = items.Where(o => o.IsCheck).ToList();
      foreach(DBQCSettingItem item in tempList)
      {
        itemList.Add(item);
      }
    }
    public void Save()
    {
      model.Save(items);
    }
  }
}
