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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// HospInfoSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class HospInfoSettingPage : Page
  {
    private HospInfoViewModel hospInfoViewModel = new HospInfoViewModel();


    public HospInfoSettingPage()
    {
      InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      lbHospInfoType.ItemsSource = hospInfoViewModel.TypeItems;
      
    }

    private void lbHospInfoType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DBHospInfoItemType item = lbHospInfoType.SelectedItem as DBHospInfoItemType;
      if (item != null)
      {
        gbHospInfo.Header = item.Caption;
        columnName.Header = item.Caption;
        dgHospInfo.ItemsSource = item.Items;
      }
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      btnAdd.IsEnabled = false;
      dgHospInfo.IsReadOnly = false;
      DBHospInfoItemType item = lbHospInfoType.SelectedItem as DBHospInfoItemType;
      if (item != null)
      {
        item.Items.Add(new DBHospInfoItem() { IsModify = true });
       
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"请选择信息类别！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
      }
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      DBHospInfoItemType itemType = lbHospInfoType.SelectedItem as DBHospInfoItemType;
      DBHospInfoItem item = dgHospInfo.SelectedItem as DBHospInfoItem;

      if (itemType == null || item == null)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"请选择要删除的项目！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
        return;
      }

      if (hospInfoViewModel.Delete(itemType.ItemID,item))
      {
        itemType.Items.Remove(item);
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow, "删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }

    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      DBHospInfoItemType item = lbHospInfoType.SelectedItem as DBHospInfoItemType;
      if (item == null)
      {
        return;
      }
      if (hospInfoViewModel.Save(item))
      {
        dgHospInfo.IsReadOnly = true;
        btnAdd.IsEnabled = true;
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"保存失败!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void btnUnSave_Click(object sender, RoutedEventArgs e)
    {
      DBHospInfoItemType item = lbHospInfoType.SelectedItem as DBHospInfoItemType;
      if (item != null)
      {
        hospInfoViewModel.LoadTypeItem(item);
      }
        
      dgHospInfo.IsReadOnly = true;
      btnAdd.IsEnabled = true;
    }

    private void dgHospInfo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      DBHospInfoItem item = dgHospInfo.SelectedItem as DBHospInfoItem;
      if (item != null)
      {
        if (!item.IsModify)
        {
          if (e.Column == columnName)
          {
            (e.EditingElement as TextBox).Text = item.Caption;
          }
          else if (e.Column == columnCode)
          {
            (e.EditingElement as TextBox).Text = item.Code;
          }

        }
      }
    }
  }
}
