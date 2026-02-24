using DataModel;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tool;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// ResultSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class ResultSettingPage : Page
  {
    private ItemConfigViewModel itemConfigViewModel = null;
    public ResultSettingPage()
    {
      InitializeComponent();
      itemConfigViewModel = new ItemConfigViewModel();
    }

    private void ItemInfoSave()
    {
      if (cbUnitTypeList.SelectedItem != null)
      {
        if (!itemConfigViewModel.SaveItemConfig(cbUnitTypeList.SelectedItem as DBChemicalItemUnitType))
        {
          MessageBoxEx.ShowEx(App.Current.MainWindow,"保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
          MessageBoxEx.ShowEx(App.Current.MainWindow,"保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"没有需要保存的数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void ResultAdd()
    {
      if (dgItemInfo.SelectedItem == null)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"请选择添加结果的项目！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      DBChemicalItemInfo ItemInfo = (dgItemInfo.SelectedItem as DBChemicalItemInfo);
      DBChemicalResultInfo ResultInfo = new DBChemicalResultInfo() {UnitType = DBItemConfigModel.CurUnitType, ItemID = ItemInfo.ItemID, IsModify = true };
      if (dgItemResult.SelectedIndex < 0)
      {
        ItemInfo.Items.Add(ResultInfo);
      }
      else
      {
        ItemInfo.Items.Insert(dgItemResult.SelectedIndex, ResultInfo);
      }
      int nIndex = 0;
      foreach (DBChemicalResultInfo item in ItemInfo.Items)
      {
        item.Index = nIndex++;
      }
    }

    private bool HasNullValueItemResult()
    {
      foreach(DBChemicalResultInfo item in (dgItemInfo.SelectedItem as DBChemicalItemInfo).Items)
      {
        if (string.IsNullOrEmpty(item.Result))
        {
          return true;
        }
      }
      return false;
    }
    private void ItemResultSave()
    {
      if (HasNullValueItemResult())
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"结果不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (!itemConfigViewModel.SaveItemResult(dgItemInfo.SelectedItem as DBChemicalItemInfo))
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        dgItemInfo.IsEnabled = true;
        MessageBoxEx.ShowEx(App.Current.MainWindow, "保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void ItemResultDelete()
    {
      if (dgItemResult.SelectedItem == null)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"请选择要删除的记录！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (MessageBoxEx.ShowEx(App.Current.MainWindow,"是否要删除选中记录！","提示", MessageBoxButton.YesNo,MessageBoxImage.Question) != MessageBoxResult.Yes)
      {
        return;
      }

      if (!itemConfigViewModel.ItemResultDelete(dgItemInfo.SelectedItem as DBChemicalItemInfo, dgItemResult.SelectedItem as DBChemicalResultInfo))
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      cbUnitTypeList.ItemsSource = itemConfigViewModel.GetUnitType();
      cbUnitTypeList.SelectedItem = itemConfigViewModel.GetCurrentUnitType();
      dgItemInfo.ItemsSource = itemConfigViewModel.GetUnitType().Single(ut=>ut.UnitTypeID == DBItemConfigModel.CurUnitType).Items;
      if (dgItemInfo.Items.Count > 0)
      {
        dgItemInfo.SelectedIndex = 0;
      }
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {

    }

    private void btnItemInfoSave_Click(object sender, RoutedEventArgs e)
    {
      ItemInfoSave();
    }

    private void btnResultAdd_Click(object sender, RoutedEventArgs e)
    {
      ResultAdd();
    }

    private void dgItemResult_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if ((e.EditingElement as TextBox).Text != (e.Row.Item as DBChemicalResultInfo).Result)
      {
        (e.Row.Item as DBChemicalResultInfo).IsModify = true;
        //dgItemInfo.IsEnabled = false;
      }
      
    }

    private void btnResultSave_Click(object sender, RoutedEventArgs e)
    {
      ItemResultSave();
      //DB db = new DB();
      //List<DBChemicalItemUnitType> dbUnitTypeList = new List<DBChemicalItemUnitType>();
      //string sql = "select * from Tab_ItemConfig order by UnitType, UnitID";
      //DataSet dataSet = new DataSet();
      //db.Query(sql, out dataSet);
      //EM_ITEM_UNITTYPE unitType = EM_ITEM_UNITTYPE.EM_ITEM_UNITTYPE_CUSTOM;
      //DBChemicalItemUnitType uType = null;
      //foreach (DataRow dr in  dataSet.Tables[0].Rows)
      //{
      //  if(unitType != (EM_ITEM_UNITTYPE)(dr["UnitType"]))
      //  {
      //    uType = new DBChemicalItemUnitType();
      //    dbUnitTypeList.Add(uType);
      //    unitType = (EM_ITEM_UNITTYPE)(dr["UnitType"]);
      //  }
      //  uType.UnitTypeID = unitType;
      //  DBChemicalItemInfo item = new DBChemicalItemInfo();
      //  item.ItemID = (EM_CHEMICAL_ITEMS)(dr["ID"]);
      //  item.Unit = (string)(dr["ItemUnit"]);
      //  item.ItemOrder = (int)(dr["ItemOrd"]);
      //  item.Name = (string)(dr["ItemName"]);
      //  item.Range = (string)(dr["ItemRange"]);
      //  uType.Items.Add(item);
      //}
      //IList <DBChemicalItemUnitType> unitTypeList = itemConfigViewModel.GetUnitType();
      //foreach(DBChemicalItemUnitType iut in unitTypeList)
      //{
      //  foreach(DBChemicalItemInfo ii in iut.Items)
      //  {
      //    foreach(DBChemicalResultInfo ri in ii.Items)
      //    {
      //      sql = "insert into Tab_ItemResultConfig(UnitType,ItemID,ResultIndex,Result) values(" + (int)iut.UnitTypeID + "," + (int)ii.ItemID + "," + ri.Index + ",'" + ri.Result + "')";
      //      db.Execute(sql);
      //    }
      //  }
      //}
    }

    private void dgItemResult_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if ((e.Source as DataGrid).CurrentCell.Column is DataGridCheckBoxColumn)
      {
        DataGridCheckBoxColumn checkColumn = (DataGridCheckBoxColumn)(e.Source as DataGrid).CurrentCell.Column;
        if ((checkColumn.Binding as Binding).Path.Path == "IsAbnormal")
        {
          ((e.Source as DataGrid).CurrentCell.Item as DBChemicalResultInfo).IsAbnormal =
            !((e.Source as DataGrid).CurrentCell.Item as DBChemicalResultInfo).IsAbnormal;
          ((e.Source as DataGrid).CurrentCell.Item as DBChemicalResultInfo).IsModify = true;
          //dgItemInfo.IsEnabled = false;
        }
      }
    }

    private void dgItemInfo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      if (((e.Column as DataGridTextColumn).Binding as Binding).Path.Path == "Unit" &&  (e.EditingElement as TextBox).Text != (e.Row.Item as DBChemicalItemInfo).Unit)
      {
        (e.Row.Item as DBChemicalItemInfo).IsModify = true;
      }
      if (((e.Column as DataGridTextColumn).Binding as Binding).Path.Path == "Range" && (e.EditingElement as TextBox).Text != (e.Row.Item as DBChemicalItemInfo).Range)
      {
        (e.Row.Item as DBChemicalItemInfo).IsModify = true;
      }

    }

    private void btnResultDelete_Click(object sender, RoutedEventArgs e)
    {
      ItemResultDelete();
    }

    private void cbUnitTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (cbUnitTypeList.SelectedItem != null)
      {
        itemConfigViewModel.SetCurrentUnitType(cbUnitTypeList.SelectedItem as DBChemicalItemUnitType);
        dgItemInfo.ItemsSource = itemConfigViewModel.GetUnitType().Single(ut => ut.UnitTypeID == DBItemConfigModel.CurUnitType).Items;
        if (dgItemInfo.Items.Count > 0)
        {
          dgItemInfo.SelectedIndex = 0;
        }
      }
    }

    private void dgItemInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if(dgItemInfo.SelectedItem != null)
      {
        dgItemResult.ItemsSource = (dgItemInfo.SelectedItem as DBChemicalItemInfo).Items;
      }
    }
  }
}
