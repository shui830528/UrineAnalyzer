using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using ReportPrintSet;
using System.Collections.ObjectModel;

namespace DataModel
{
  public class PrintSetItem
  {
    public ObservableCollection<TitleFooterInfo> ReportTitle
    {
      get;
      set;
    }

    public ObservableCollection<TitleFooterInfo> ReportFooter
    {
      get;
      set;
    }

    public string ReportName
    {
      get;
      set;
    }
    
    public string ReportManualName
    {
      get;
      set;
    }
  }

  public class PrintSetModel
  {
    private string QCReport = "质控";
    private string SampleReportNoManual = "无手工镜检";
    private string SampleReportManual = "有手工镜检";
    public PrintSetModel()
    {
    }

    public void LoadSamplePrintParam(PrintSetItem samplePrintParam)
    {
      try
      {
        ObservableCollection<TitleFooterInfo> titles1 = new ObservableCollection<TitleFooterInfo>();
        ObservableCollection<TitleFooterInfo> footers1 = new ObservableCollection<TitleFooterInfo>();
        string reportFile = "";
        ReportSetModel.GetReportInfo(ReportSetModel.GetDefaultReport(SampleReportNoManual), ref reportFile, ref titles1, ref footers1);
        samplePrintParam.ReportName = reportFile;
        samplePrintParam.ReportTitle = titles1;
        samplePrintParam.ReportFooter = footers1;
        ObservableCollection<TitleFooterInfo> titles2 = new ObservableCollection<TitleFooterInfo>();
        ObservableCollection<TitleFooterInfo> footers2 = new ObservableCollection<TitleFooterInfo>();
        reportFile = "";
        ReportSetModel.GetReportInfo(ReportSetModel.GetDefaultReport(SampleReportManual), ref reportFile, ref titles2, ref footers2);
        samplePrintParam.ReportManualName = reportFile;
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + "PrintSetModel.LoadSamplePrintParam Error");
      }
    }

    public void LoadQCPrintParam(PrintSetItem qcPrintParam)
    {
      try
      {
        string reportFile = "";
        ObservableCollection<TitleFooterInfo> titles = new ObservableCollection<TitleFooterInfo>();
        ObservableCollection<TitleFooterInfo> footers = new ObservableCollection<TitleFooterInfo>();
        ReportSetModel.GetReportInfo(ReportSetModel.GetDefaultReport(QCReport), ref reportFile, ref titles, ref footers);
        qcPrintParam.ReportName = reportFile;
        qcPrintParam.ReportTitle = titles;
        qcPrintParam.ReportFooter = footers;
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + "PrintSetModel.LoadQCPrintParam Error");
      }
    }
  }
}
