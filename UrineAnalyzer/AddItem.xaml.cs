using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tool;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// AddItem.xaml 的交互逻辑
  /// </summary>
  public partial class AddItem : ModernWindow
  {
    private AddItemViewModel addItemViewModel = null;
    private ObservableCollection<ItemsInfos> selectItemInfoList = new ObservableCollection<ItemsInfos>();
    public ObservableCollection<ItemsInfos> SelectItemInfoList
    {
      get { return selectItemInfoList; }
    }

    private IList<DBSampleResultInfo> useItemList = null;
    public IList<DBSampleResultInfo> UseItemList
    {
      get { return useItemList; }
      set { useItemList = value; }
    }
    public AddItem()
    {
      InitializeComponent();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      selectItemInfoList.Clear();
      foreach(var item in addItemViewModel.ItemInfoList)
      {
        if(item.IsChecked)
        {
          foreach(var unit in addItemViewModel.UnitInfoList)
          {
            if(unit.UnitTypeID == DBItemConfigModel.CurUnitType)
            {
              foreach(var unitItem in unit.Items)
              {
                if(unitItem.Name == item.Name)
                {
                  foreach(var result in unitItem.Items)
                  {
                    if(item.Result == result.Result)
                    {
                      item.ResultIndex = result.Index;
                      item.Abnormal = result.IsAbnormal ? "*" : "";
                      break;
                    }
                  }
                }
              }
            }
          }
          selectItemInfoList.Add(item);
        }
      }
      DialogResult = true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void SetItemInfoSource(EM_ITEM_UNITTYPE unitTypeID)
    {
      addItemViewModel.GetItemInfo(DBItemConfigModel.CurUnitType);
      foreach(var item in addItemViewModel.ItemInfoList)
      {
        foreach(var useItem in useItemList)
        {
          if(item.Name == useItem.Name)
          {
            item.Result = useItem.Result;
            item.ResultIndex = useItem.ResultIndex;
            item.IsChecked = true;
            break;
          }
        }
      }
      dgItemInfo.ItemsSource = addItemViewModel.ItemInfoList;
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      addItemViewModel = new AddItemViewModel();
      SetItemInfoSource(DBItemConfigModel.CurUnitType);
    }
  }
}
