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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UrineAnalyzer
{
  /// <summary>
  /// LogSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class LogSettingPage : Page
  {
    private LogModel logModel = new LogModel();
    private ObservableCollection<string> usersList = new ObservableCollection<string>();
    private ObservableCollection<DBLogInfo> logsList = new ObservableCollection<DBLogInfo>();
    public LogSettingPage()
    {
      InitializeComponent();
    }

    private void btn_Query_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      logModel.Query(datePicker_Date.Text, comboBox_User.Text, ref logsList);
      if(logsList.Count == 0)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow, "没有查询到日志信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      logModel.GetUserNames(ref usersList);
      comboBox_User.ItemsSource = usersList;
      dataGrid_Log.ItemsSource = logsList;
    }
  }
}
