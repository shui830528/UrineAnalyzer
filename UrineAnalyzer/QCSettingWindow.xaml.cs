using DataModel;
using System;
using System.Collections.Generic;
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
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// QCSettingWindow.xaml 的交互逻辑
  /// </summary>
  public partial class QCSettingWindow : ModernWindow
  {
    private QCSettingViewModel ViewModel = new QCSettingViewModel();
    private ItemConfigViewModel ConfigModel = new ItemConfigViewModel();
    private DBChemicalItemInfo SelectedItemInfo = null;

    public QCSettingWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ViewModel.Update();
      dgItems.ItemsSource = ViewModel.Items;
      if (dgItems.Items.Count > 0)
      {
        dgItems.SelectedIndex = 0;
      }
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      ViewModel.Save();
      MessageBoxEx.ShowEx(this,"保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      DialogResult = true;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void Binding(DBQCSettingItem item)
    {

      DBChemicalItemUnitType UnitType = ConfigModel.GetCurrentUnitType();
      List<DBChemicalItemInfo> ItemList = UnitType.Items.Where(o => o.ItemID == item.ItemID).ToList();
      if (ItemList.Count > 0)
      {
        SelectedItemInfo = ItemList[0];
      }
      else
      {
        SelectedItemInfo = null;
      }

      if (SelectedItemInfo != null)
      {
        cbNegUpperLimit.ItemsSource = SelectedItemInfo.Items;
        cbNegLowerLimit.ItemsSource = SelectedItemInfo.Items;
        cbPosUpperLimit.ItemsSource = SelectedItemInfo.Items;
        cbPosLowerLimit.ItemsSource = SelectedItemInfo.Items;


      }

      cbNegNoEnable.DataContext = item;
      cbNegUpperLimit.DataContext = item;
      cbNegLowerLimit.DataContext = item;

      cbPosNoEnable.DataContext = item;
      cbPosUpperLimit.DataContext = item;
      cbPosLowerLimit.DataContext = item;

  
    }

    private void dgItems_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (dgItems.SelectedItem != null)
      {
        DBQCSettingItem Item = dgItems.SelectedItem as DBQCSettingItem;
        Binding(Item);
      }
      
    }

    private void tbNegNoEnable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      cbNegNoEnable.IsChecked = !cbNegNoEnable.IsChecked;
    }

    private void tbPosNoEnable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      cbPosNoEnable.IsChecked = !cbPosNoEnable.IsChecked;
    }

    private void cbNegNoEnable_CheckChange(object sender, RoutedEventArgs e)
    {
      cbNegUpperLimit.IsEnabled = !cbNegNoEnable.IsChecked == true;
      cbNegLowerLimit.IsEnabled = !cbNegNoEnable.IsChecked == true;
    }
    private void cbPosNoEnable_CheckChange(object sender, RoutedEventArgs e)
    {
      cbPosUpperLimit.IsEnabled = !cbPosNoEnable.IsChecked == true;
      cbPosLowerLimit.IsEnabled = !cbPosNoEnable.IsChecked == true;
    }
  }
}
