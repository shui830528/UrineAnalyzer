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
using ReportPrintSet;
using System.Diagnostics;
using System.Threading;
using Tool;

namespace UrineAnalyzer
{
  /// <summary>
  /// PrintSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class PrintSettingPage : Page
  {
    private ReportsPrintSet reportsPrintSet = null;
    public PrintSettingPage()
    {
      InitializeComponent();
      frame.Dispatcher.Invoke(new Action(() => {reportsPrintSet = new ReportsPrintSet(App.DBConnectionParamName, App.DBConnectionString); frame.Content = reportsPrintSet;}));
    }
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      reportsPrintSet.IsSelected = Const.user == "admin";
    }
  }
}
