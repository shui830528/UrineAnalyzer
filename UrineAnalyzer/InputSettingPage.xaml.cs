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
  /// InputSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class InputSettingPage : Page
  {
    private InputDefaultViewModel viewModel = new InputDefaultViewModel();

    public InputSettingPage()
    {
      InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      viewModel.Update();
      dgInputDefault.ItemsSource = viewModel.Items;
    }

    private bool IsModify()
    {
      foreach (DBInputDefaultItem item in viewModel.Items)
      {
        if (item.IsModify)
        {
          return true;
        }
      }

      return false;
    }

    private void dgInputDefault_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      (e.Row.Item as DBInputDefaultItem).IsModify = true;
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (IsModify())
      {
        if (!viewModel.Save())
        {
          MessageBoxEx.ShowEx(App.Current.MainWindow,"数据保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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

    private void btnUnSave_Click(object sender, RoutedEventArgs e)
    {
      if (IsModify())
      {
        viewModel.Update();
      }
    }
  }
}
