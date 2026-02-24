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
  /// LinkParamPage.xaml 的交互逻辑
  /// </summary>
  public partial class LinkParamPage : Page
  {
    private InstrumentTypeList instrumentType = new InstrumentTypeList();
    private LinkParamViewModel model = new LinkParamViewModel();
    public LinkParamPage()
    {
      InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      cbInstrumentType.DataContext = model.LinkParam;
      cbPortName.DataContext = model.LinkParam;
      cbBaudRate.DataContext = model.LinkParam;

      cbInstrumentType.ItemsSource = instrumentType.Items;
      cbInstrumentType.DisplayMemberPath = "Caption";

      cbPortName.ItemsSource = model.PortNameList;


      cbBaudRate.ItemsSource = model.BaudRateList;



    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
      if (cbInstrumentType.SelectedItem == null)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"仪器类型不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (cbPortName.SelectedItem == null)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"串口名称不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      if (model.Save())
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void btnUnSave_Click(object sender, RoutedEventArgs e)
    {
      model.Update();
    }
  }
}
