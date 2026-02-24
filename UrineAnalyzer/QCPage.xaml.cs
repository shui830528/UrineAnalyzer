using DataModel;
using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
  /// QCPage.xaml 的交互逻辑
  /// </summary>
  public partial class QCPage : Page
  {
    private QCResultViewModel viewModel = new QCResultViewModel();
    private DateTime CurDate = DateTime.Now;
    private QCSettingViewModel SettingViewModel = new QCSettingViewModel();
    private ObservableCollection<DBQCSettingItem> chartItems = new ObservableCollection<DBQCSettingItem>();
    private ItemConfigViewModel ConfigModel = new ItemConfigViewModel();
    private ObservableCollection<DBQCResultInfo> chartResultItems = new ObservableCollection<DBQCResultInfo>();
    private ReportPrintViewModel reportPrintViewModel;

    private string qCDate;
    private string qCItem;
    private string qCType;
    private string qCItemRange;

    private DataUpdate dataUpdate = new DataUpdate("QCPage"); 

    public QCPage()
    {
      InitializeComponent();
      viewModel.Year = CurDate.Year;
      viewModel.Month = CurDate.Month;
      
      rbNeg.IsChecked = true;
      reportPrintViewModel = new ReportPrintViewModel(App.DBConnectionParamName, App.DBConnectionString);


      dataUpdate.SetAction((param01,param02)=> {

        new Thread(() =>
        {
          Thread.Sleep(1000);
          UIExecute.RunAsync(() => {
            viewModel.UpdateGroupInfo();
            UpdateChart();
          });
        }).Start();



      });
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      dgGroupItems.ItemsSource = viewModel.GroupItems;
      dgItems.ItemsSource = viewModel.Items;
      viewModel.UpdateGroupInfo();

      dgChartItems.ItemsSource = chartItems;
      SettingViewModel.GetChartItems(chartItems);

      dgChartResult.ItemsSource = chartResultItems;

      if (dgChartItems.Items.Count > 0)
        dgChartItems.SelectedIndex = 0;
      tbDate.Text = string.Format("{0}-{1:D2}", viewModel.Year, viewModel.Month);
      SetChartMonthTitle(tbDate.Text);

      //ClearCustomLables();
      //ClearConstantLines();
      //ClearSeriesPoints();

      //int nIndex = 1;
      //AddCustomLabel(nIndex++, "Neg");
      //AddCustomLabel(nIndex++, "+-");
      //AddCustomLabel(nIndex++, "1+");
      //AddCustomLabel(nIndex++, "2+");
      //AddCustomLabel(nIndex++, "3+");


      //nIndex = 1;
      //AddConstantLine(nIndex++, Brushes.Black, true);
      //AddConstantLine(nIndex++, Brushes.Red, false);
      //AddConstantLine(nIndex++, Brushes.Black, true);
      //AddConstantLine(nIndex++, Brushes.Red, false);
      //AddConstantLine(nIndex++, Brushes.Black, true);


      //GetSeriesPoints().Add(new SeriesPoint() { Argument = "1", Value = 5 });
      //GetSeriesPoints().Add(new SeriesPoint() { Argument = "2", Value = 3 });
      //GetSeriesPoints().Add(new SeriesPoint() { Argument = "3", Value = 1 });
      //GetSeriesPoints().Add(new SeriesPoint() { Argument = "4", Value = 4 });

      (qcChart.Diagram as XYDiagram2D).AxisY.WholeRange.MinValue = 0;

      int iDays = DateTime.DaysInMonth(viewModel.Year, viewModel.Month);
      (qcChart.Diagram as XYDiagram2D).AxisX.WholeRange.MinValue = 1;
      (qcChart.Diagram as XYDiagram2D).AxisX.WholeRange.MaxValue = iDays;


    }
    private void SetChartTitle(string strTitle)
    {
      qcChart.Titles[0].Content = strTitle;
    }
    private void SetChartMonthTitle(string strTitle)
    {
      qcChart.Titles[1].Content = strTitle;
    }

    private void AddConstantLine(int Value,Brush Brush,bool IsDashes)
    {
      ConstantLine line = new ConstantLine() { Value = Value, Brush = Brush };
      if (IsDashes)
      {
        line.LineStyle = new LineStyle() { DashStyle = new DashStyle(new List<double>() { 5 },0) };
      }
      GetConstantLines().Add(line);
    }

    private void AddCustomLabel(int Value,string Caption)
    {
      GetCustomLables().Add(new CustomAxisLabel() { Value = Value, Content = Caption });
    }
    private CustomAxisLabelCollection GetCustomLables()
    {
      return (qcChart.Diagram as XYDiagram2D).AxisY.CustomLabels;
    }
    private ConstantLineCollection GetConstantLines()
    {
      return (qcChart.Diagram as XYDiagram2D).AxisY.ConstantLinesBehind;
    }
    private SeriesPointCollection GetSeriesPoints()
    {
      return ((qcChart.Diagram as XYDiagram2D).Series[0] as LineSeries2D).Points;
    }
    private void ClearCustomLables()
    {
      GetCustomLables().Clear();
    }
    private void ClearConstantLines()
    {
      GetConstantLines().Clear();
    }
    private void ClearSeriesPoints()
    {
      GetSeriesPoints().Clear();
    }
    private void dgGroupItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (dgGroupItems.SelectedItem != null)
      {
        viewModel.UpdateItmesInfo(dgGroupItems.SelectedIndex);
      }
    }
    private void UpdateChart()
    {
      if (dgChartItems.SelectedItem != null)
      {
        DBQCSettingItem SettingItem = dgChartItems.SelectedItem as DBQCSettingItem;
        DBChemicalItemUnitType UnitType = ConfigModel.GetCurrentUnitType();
        List<DBChemicalItemInfo> ItemList = UnitType.Items.Where(o => o.ItemID == SettingItem.ItemID).ToList();

        SetChartTitle((viewModel.IsNeg ? "阴性质控：" : "阳性质控：") + SettingItem.Caption+"("+SettingItem.Name+")");

        qCType = viewModel.IsNeg ? "阴性质控" : "阳性质控";
        qCItem = SettingItem.Name;
        qCItemRange = ConfigModel.GetChemicalItem(SettingItem.ItemID).Range;

        ClearCustomLables();
        ClearConstantLines();
        ClearSeriesPoints();
        foreach(DBChemicalResultInfo item in ItemList[0].Items)
        {
          AddCustomLabel(item.Index,item.Result);
          Brush Brush = Brushes.Black;
          bool IsDashes = true;
          if (viewModel.IsNeg)
          {
            if (!SettingItem.NegNoEnable && (SettingItem.NegLowerLimit == item.Index || SettingItem.NegUpperLimit == item.Index))
            {
              Brush = Brushes.Red;
              IsDashes = false;
            }
          }
          else
          {
            if (!SettingItem.PosNoEnable && (SettingItem.PosLowerLimit == item.Index || SettingItem.PosUpperLimit == item.Index))
            {
              Brush = Brushes.Red;
              IsDashes = false;
            }
          }


          AddConstantLine(item.Index, Brush, IsDashes);
        }

        (qcChart.Diagram as XYDiagram2D).AxisY.WholeRange.MaxValue = ItemList[0].Items.Count - 1;
        int iDays = DateTime.DaysInMonth(viewModel.Year, viewModel.Month);
        (qcChart.Diagram as XYDiagram2D).AxisX.WholeRange.MinValue = 1;
        (qcChart.Diagram as XYDiagram2D).AxisX.WholeRange.MaxValue = iDays;
      }

      UpdateChartResult();

      foreach(DBQCResultInfo item in chartResultItems)
      {
        GetSeriesPoints().Add(new SeriesPoint() { Argument = item.QCDate.Day.ToString(),Value = item.ItemIndex });
      }
    }

    private int FindMaxQCInfo(DBQCInfo info , IList<DBQCInfo> tempList)
    {
      for (int i = 0;i < tempList.Count;i++)
      {
        DBQCInfo item = tempList[i];
        if (info.QCDate.Year == item.QCDate.Year && 
          info.QCDate.Month == item.QCDate.Month && 
          info.QCDate.Day == item.QCDate.Day && 
          info.QCDate > item.QCDate)
        {
          return i;
        }
      }
      return -1;
    }

    private void UpdateChartResult()
    {
      if (dgChartItems.SelectedItem != null)
      {
        DBQCSettingItem SettingItem = dgChartItems.SelectedItem as DBQCSettingItem;
        chartResultItems.Clear();

        List<DBQCInfo> tempQCInfoList = new List<DBQCInfo>();
        foreach (DBQCInfo info in viewModel.GroupItems)
        {
          int nIndex = FindMaxQCInfo(info,tempQCInfoList);
          if (nIndex != -1)
          {
            tempQCInfoList[nIndex] = info;
          }
          else
          {
            tempQCInfoList.Add(info);
          }
        }

        foreach (DBQCInfo info in tempQCInfoList)
        {
          List<DBQCResultInfo> tempList = info.Items.Where(o => o.ItemID == SettingItem.ItemID).ToList();

          foreach (DBQCResultInfo item in tempList)
          {
            chartResultItems.Add(item);
          }
        }


      }
    }
    private void rbNeg_Checked(object sender, RoutedEventArgs e)
    {
      viewModel.IsNeg = true;
      viewModel.UpdateGroupInfo();
      UpdateChart();
    }

    private void rbPos_Checked(object sender, RoutedEventArgs e)
    {
      viewModel.IsNeg = false;
      viewModel.UpdateGroupInfo();
      UpdateChart();
    }

    private void btnPreYear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      CurDate = CurDate.AddYears(-1);
      viewModel.Year = CurDate.Year;
      viewModel.UpdateGroupInfo();
      tbDate.Text = string.Format("{0}-{1:D2}",viewModel.Year,viewModel.Month);
      SetChartMonthTitle(tbDate.Text);
      UpdateChart();
    }

    private void btnNextYear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      CurDate = CurDate.AddYears(1);
      viewModel.Year = CurDate.Year;
      viewModel.UpdateGroupInfo();
      tbDate.Text = string.Format("{0}-{1:D2}", viewModel.Year, viewModel.Month);
      SetChartMonthTitle(tbDate.Text);
      UpdateChart();
    }

    private void btnPreMonth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      CurDate = CurDate.AddMonths(-1);
      viewModel.Month = CurDate.Month;
      viewModel.UpdateGroupInfo();
      tbDate.Text = string.Format("{0}-{1:D2}", viewModel.Year, viewModel.Month);
      SetChartMonthTitle(tbDate.Text);
      UpdateChart();
    }

    private void btnNextMonth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      CurDate = CurDate.AddMonths(1);
      viewModel.Month = CurDate.Month;
      viewModel.UpdateGroupInfo();
      tbDate.Text = string.Format("{0}-{1:D2}", viewModel.Year, viewModel.Month);
      SetChartMonthTitle(tbDate.Text);
      UpdateChart();
    }

    private void btnSetting_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      QCSettingWindow settingWindow = new QCSettingWindow();
      settingWindow.Owner = Application.Current.MainWindow;
      settingWindow.ShowDialog();
    }

    private void dgChartItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      UpdateChart();
    }

    private void QCTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (QCTabControl.SelectedIndex == 0)
      {
        btnDelete.IsEnabled = true;
      }
      else
      {
        btnDelete.IsEnabled = false;
      }
    }

    private void qcChart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
    {
      if (dgChartItems.SelectedItem != null)
      {
        DBQCSettingItem SettingItem = dgChartItems.SelectedItem as DBQCSettingItem;

        if (viewModel.IsNeg)
        {
          if (!SettingItem.NegNoEnable && (SettingItem.NegLowerLimit >= e.SeriesPoint.Value || SettingItem.NegUpperLimit <= e.SeriesPoint.Value))
          {
            e.DrawOptions.Color = Colors.Red;
          }
        }
        else
        {
          if (!SettingItem.PosNoEnable && (SettingItem.PosLowerLimit >= e.SeriesPoint.Value || SettingItem.PosUpperLimit <= e.SeriesPoint.Value))
          {
            e.DrawOptions.Color = Colors.Red;
          }
        }

      }
    }

    private void btnPreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      reportPrintViewModel.ReportPreview(qCItem, qCType, qCItemRange, viewModel.Year, viewModel.Month, qcChart,App.Current.MainWindow);
    }

    private void btnPrint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      reportPrintViewModel.PrintReport(qCItem, qCType, qCItemRange, viewModel.Year, viewModel.Month, qcChart, App.Current.MainWindow);
    }

    private void btnDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (dgGroupItems.SelectedItem != null)
      {
        viewModel.DeleteQCResult(dgGroupItems.SelectedItem as DBQCInfo, viewModel.IsNeg);
        viewModel.UpdateGroupInfo();
        UpdateChart();
      }
    }
  }
}
