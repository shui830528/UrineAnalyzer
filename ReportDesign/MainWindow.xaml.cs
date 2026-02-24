using FastReport;
using FastReport.Design.StandardDesigner;
using ReportPrint;
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

namespace ReportDesign
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    private string reportDesignTilte = "报表设计";
    private string reportPreviewTitle = "报表预览";
    private string fileName = "";
    private DesignerControl designerControl = new DesignerControl();
    private EnvironmentSettings environmentSettings = new EnvironmentSettings();
    public MainWindow()
    {
      FastReport.Utils.Res.LoadLocale(System.AppDomain.CurrentDomain.BaseDirectory +"\\Localization\\Chinese (Simplified).frl");
      InitializeComponent();
    }

    public MainWindow(string reportFileName, string reportTempletDir)
    {
      FastReport.Utils.Res.LoadLocale(System.AppDomain.CurrentDomain.BaseDirectory + "\\Localization\\Chinese (Simplified).frl");
      InitializeComponent();
      fileName = System.AppDomain.CurrentDomain.BaseDirectory + reportTempletDir + reportFileName + ".frx";
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      designerControl.UIStyle = FastReport.Utils.UIStyle.VisualStudio2005;
      FormHost.Child = designerControl;
      Report report = new Report();
      if (fileName != "")
      {
        this.Title = reportDesignTilte + "-" + System.IO.Path.GetFileNameWithoutExtension(fileName);
        report.Load(fileName);
      }
      designerControl.Report = report;
      designerControl.RefreshLayout();
      environmentSettings.CustomPreviewReport += EnvironmentSettings_CustomPreviewReport;
      environmentSettings.CustomSaveReport += EnvironmentSettings_CustomSaveReport;
      environmentSettings.CustomOpenReport += EnvironmentSettings_CustomOpenReport;
      designerControl.MainMenu.Items[0].SubItems[designerControl.MainMenu.Items[0].SubItems.Count - 1].Click += MainWindow_Click;
    }

    private void EnvironmentSettings_CustomOpenReport(object sender, FastReport.Design.OpenSaveReportEventArgs e)
    {
      this.Title = reportDesignTilte+"-" + System.IO.Path.GetFileNameWithoutExtension(e.FileName);
      designerControl.Report.Load(e.FileName);
    }

    private void EnvironmentSettings_CustomSaveReport(object sender, FastReport.Design.OpenSaveReportEventArgs e)
    {
      e.Report.Save(e.FileName);
      fileName = e.FileName;
      this.Title = reportDesignTilte+"-" + System.IO.Path.GetFileNameWithoutExtension(e.FileName);
      designerControl.Modified = false;
    }

    private void MainWindow_Click(object sender, EventArgs e)
    {
      if(designerControl.Modified)
      {
        MessageBoxResult question = MessageBox.Show("报表已更改，是否保存？", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        if (question == MessageBoxResult.Yes)
        {
          if(fileName == "")
          {
            designerControl.cmdSave.Invoke();
          }
          else
          {
            designerControl.Report.Save(fileName);
          }
        }
        else if(question == MessageBoxResult.Cancel)
        {
          return;
        }
        else
        {
          ;
        }
      }
      Close();
    }

    private void EnvironmentSettings_CustomPreviewReport(object sender, EventArgs e)
    {
      ReportPreview.MainWindow reportPreviewWindow = new ReportPreview.MainWindow(); ;
      designerControl.Report.Preview = reportPreviewWindow.MyPreviewReport;
      designerControl.Report.Show();
      if(designerControl.Report.FileName == "")
      {
        reportPreviewWindow.Title = reportPreviewTitle;
      }
      else
      {
        reportPreviewWindow.Title = reportDesignTilte+"-" + System.IO.Path.GetFileNameWithoutExtension(designerControl.Report.FileName);
      }
      reportPreviewWindow.ShowDialog();
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
      environmentSettings.CustomPreviewReport -= EnvironmentSettings_CustomPreviewReport;
      environmentSettings.CustomSaveReport -= EnvironmentSettings_CustomSaveReport;
      environmentSettings.CustomOpenReport -= EnvironmentSettings_CustomOpenReport;
      designerControl.MainMenu.Items[0].SubItems[designerControl.MainMenu.Items[0].SubItems.Count - 1].Click -= MainWindow_Click;
    }
  }
}
