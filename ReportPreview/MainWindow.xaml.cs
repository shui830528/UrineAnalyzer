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
using FastReport.Preview;
using FastReport;

namespace ReportPreview
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  /// 
  public partial class MainWindow : Window
  {
    private string reportPreviewTitle = "报表预览";
    private bool bDesign = false;
    private PreviewControl previewReport;
    public PreviewControl MyPreviewReport
    {
      get { return previewReport; }
    }
    private ReportPrint.ReportPrint printReport;
    public ReportPrint.ReportPrint MyPrintReport
    {
      get { return printReport; }
    }
    private void SetReportPreviewInfo()
    {
      previewReport.UIStyle = FastReport.Utils.UIStyle.VisualStudio2005;
      previewReport.ToolbarVisible = true;
      previewReport.StatusbarVisible = true;
      previewReport.OutlineVisible = true;
    }
    public MainWindow()
    {
      FastReport.Utils.Res.LoadLocale(System.AppDomain.CurrentDomain.BaseDirectory + "\\Localization\\Chinese (Simplified).frl");
      InitializeComponent();
      previewReport = new PreviewControl();
      SetReportPreviewInfo();
      bDesign = true;
    }
    public MainWindow(string reportFileName)
    {
      FastReport.Utils.Res.LoadLocale(System.AppDomain.CurrentDomain.BaseDirectory +"\\Localization\\Chinese (Simplified).frl");  
      InitializeComponent();
      printReport = new ReportPrint.ReportPrint(reportFileName);
      previewReport = new PreviewControl();
      SetReportPreviewInfo();
      this.Title = reportPreviewTitle+"-" + System.IO.Path.GetFileNameWithoutExtension(reportFileName);
      bDesign = false;
    }
    private void OnCloseClick(object sender, EventArgs e)
    {
      Close();
    }
    private void Window_Loaded(object sender, RoutedEventArgs e) 
    {
      previewReport.ToolBar.Items[16].Click += OnCloseClick;
      FormHost.Child = previewReport;
      if (!bDesign)
      {
        printReport.MyReport.Preview = previewReport;
        printReport.MyReport.Show();
      }
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
      previewReport.ToolBar.Items[16].Click -= OnCloseClick;
    }
  }
}
