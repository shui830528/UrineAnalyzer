using DataModel;
using DevExpress.Xpf.Charts;
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
using Tool;

namespace UrineAnalyzer
{
  /// <summary>
  /// WorkloadAccountPage.xaml 的交互逻辑
  /// </summary>
  public partial class WorkloadAccountPage : Page
  {
    private WorkloadAccountModel model = new WorkloadAccountModel();
    private ObservableCollection<WorkloadItem> items = new ObservableCollection<WorkloadItem>();
    private string StartDate;
    private string EndDate;

    public WorkloadAccountPage()
    {
      InitializeComponent();

      //StartDate = DateTime.Now.ToString(Const.DateFormatting) + " 00:00:00";
      //EndDate = DateTime.Now.ToString(Const.DateFormatting) + " 23:59:59";
      dpStart.SelectedDate = DateTime.Now;
      dpEnd.SelectedDate = DateTime.Now;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      model.GetWorkloadList(StartDate,EndDate, items);
      dgWorkload.ItemsSource = items;
      UpdateChart();
    }

    private void UpdateChart()
    {
      GetChartSeriesPoints(WorkloadType.wtSendCheckOffice).Clear();
      GetChartSeriesPoints(WorkloadType.wtSendCheckDoctor).Clear();
      GetChartSeriesPoints(WorkloadType.wtCheckPersion).Clear();

      foreach(WorkloadItem item in items)
      {
        GetChartSeriesPoints(item.WorkloadType).Add(new SeriesPoint() { Argument = item.ItemName,Value = item.Count });
      }
    }

    private SeriesPointCollection GetChartSeriesPoints(WorkloadType WorkType)
    {
      if (WorkType == WorkloadType.wtCheckPersion)
        return Series2DPerson.Points;
      else if (WorkType == WorkloadType.wtSendCheckDoctor)
        return Series2DCheckPersion.Points;
      return Series2DOffice.Points;
    }

    private void Update()
    {
      StartDate = ((DateTime)dpStart.SelectedDate).ToString(Const.DateFormat) + " 00:00:00";
      EndDate = ((DateTime)dpEnd.SelectedDate).ToString(Const.DateFormat) + " 23:59:59";
      model.GetWorkloadList(StartDate, EndDate, items);
      UpdateChart();
    }

    private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
        return;
      if (dpStart.SelectedDate > dpEnd.SelectedDate)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"开始日期不能大于结束日期", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        Update();
      }
    }

    private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
        return;
      if (dpStart.SelectedDate > dpEnd.SelectedDate)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"开始日期不能大于结束日期", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        Update();
      }
    }

    private void btnStatistics_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if(items.Count == 0)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow, "没有查询到统计数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      Update();
    }

    private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      string strFileName = @"E:\生化上位机软件\尿仪\UrineAnalyzer\UrineAnalyzer\bin\Debug\image\20181010092352.jpg";
      StringBuilder imageFile = new StringBuilder(strFileName);
      Control.Snapshot.ClippingPictures(imageFile);
    }
  }
}
