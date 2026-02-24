using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ViewModel
{
  public class ItemsInfos : INotifyPropertyChanged
  {
    private EM_CHEMICAL_ITEMS itemID;
    public EM_CHEMICAL_ITEMS ItemID
    {
      get { return itemID; }
      set { itemID = value; }
    }
    private string abnormal;
    public string Abnormal
    {
      get { return abnormal; }
      set { abnormal = value; NotifyPropertyChanged("Abnormal"); }
    }
    private bool isChecked;
    public bool IsChecked
    {
      get { return isChecked; }
      set { isChecked = value; NotifyPropertyChanged("IsChecked"); }
    }
    private string name;
    public string Name
    {
      get { return name; }
      set { name = value; NotifyPropertyChanged("Name"); }
    }
    private string caption;
    public string Caption
    {
      get { return caption; }
      set { caption = value; NotifyPropertyChanged("Caption"); }
    }
    private string unit;
    public string Unit
    {
      get { return unit; }
      set { unit = value; NotifyPropertyChanged("Unit"); }
    }
    public int ResultIndex { get; set; }
    private string result = "";
    public string Result
    {
      get { return result; }
      set { result = value; NotifyPropertyChanged("Result"); }
    }
    private ObservableCollection<string> resultList = new ObservableCollection<string>();
    public ObservableCollection<string> ResultList
    {
      get { return resultList; }
    }
    private string range;
    public string Range
    {
      get { return range; }
      set { range = value; NotifyPropertyChanged("Range"); }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String info)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(info));
      }
    }
  }
  public class AddItemViewModel
  {
    private ItemConfigViewModel itemConfigViewModel = new ItemConfigViewModel();

    private ObservableCollection<ItemsInfos> itemInfoList = new ObservableCollection<ItemsInfos>();
    public ObservableCollection<ItemsInfos> ItemInfoList
    {
      get { return itemInfoList; }
    }

    private IList<DBChemicalItemInfo> itemNameList = null;

    public IList<DBChemicalItemUnitType> unitInfoList = null;
    public IList<DBChemicalItemUnitType> UnitInfoList
    {
      get { return unitInfoList; }
    }

    public AddItemViewModel()
    {
      unitInfoList = itemConfigViewModel.GetUnitType();
      itemNameList = itemConfigViewModel.GetUnitType().Single(ut=>ut.UnitTypeID == DBItemConfigModel.CurUnitType).Items;
    }
    private string GetItemCaption(string itemName)
    {
      string itemCaption = "";
      for(int i = 0;i<itemNameList.Count;i++)
      {
        if(itemNameList[i].Name == itemName)
        {
          itemCaption = itemNameList[i].Caption;
        }
      }
      return itemCaption;
    }
    public void GetItemInfo(EM_ITEM_UNITTYPE unitTypeID)
    {
      itemInfoList.Clear();
      for (int i = 0;i < unitInfoList.Count;i++)
      {
        if(unitInfoList[i].UnitTypeID == unitTypeID)
        {
          for(int j = 0;j< unitInfoList[i].Items.Count;j++)
          {
            ItemsInfos itemsInfos = new ItemsInfos();
            itemsInfos.Caption = GetItemCaption(unitInfoList[i].Items[j].Name);
            itemsInfos.Name = unitInfoList[i].Items[j].Name;
            itemsInfos.Unit = unitInfoList[i].Items[j].Unit;
            itemsInfos.Range = unitInfoList[i].Items[j].Range;
            itemsInfos.ItemID = unitInfoList[i].Items[j].ItemID;
            string result = "";
            for(int k = 0;k< unitInfoList[i].Items[j].Items.Count;k++)
            {
              itemsInfos.ResultList.Add(unitInfoList[i].Items[j].Items[k].Result);
              if(k == 0)
              {
                result = unitInfoList[i].Items[j].Items[k].Result;
              }
            }
            itemsInfos.Result = result;
            itemInfoList.Add(itemsInfos);
          }
          break;
        }
      }
    }
  }
}
