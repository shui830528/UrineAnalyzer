using DataModel;
using DevExpress.Xpf.Charts;
using ReportPrint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using Tool;
using ReportPreviewer;
using System.IO;

namespace ViewModel
{
  public enum enumReportType
  {
    QC = 1,
    Sample = 2
  }
  public class ReportPrintViewModel
  {
    private ReportPrint.ReportPrint reportPrint;
    private PreviewerWindow reportPreview;
    private ReportPrintModel reportPrintModel = new ReportPrintModel();
    private PrintSetParamViewModel printSetParamViewModel = new PrintSetParamViewModel();
    private string dbConnectionParamName;
    private string dbConnectionString;
    private delegate void SetPosInfo(System.Windows.Window window, System.Windows.Controls.Label showInfo, System.Windows.Controls.ProgressBar progress, int pos, string info);

    private Dictionary<string, string> parameterList = new Dictionary<string, string>();
    public ReportPrintViewModel(string dbConnectionParamName,string dbConnectionString)
    {
      this.dbConnectionParamName = dbConnectionParamName;
      this.dbConnectionString = dbConnectionString;
    }
    private void SetTextandPosMesssage(System.Windows.Window window, System.Windows.Controls.Label showInfo, System.Windows.Controls.ProgressBar progress, int pos, string info)
    {
      if (window.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
      {
        window.Dispatcher.Invoke(new SetPosInfo(this.SetTextandPosMesssage), window,showInfo, progress,pos,info);
      }
      else
      {
        showInfo.Content = info;
        progress.Value = pos;
      }
    }
    private void PrepareReportData(string itemName, int iyear, int imonth, out List<System.Data.DataSet> reportDataSourceDataSet,out List<string> reportDataSourceName)
    {
      reportDataSourceDataSet = new List<DataSet>();
      reportDataSourceName = new List<string>();
      DataSet qcData = reportPrintModel.GetQCData(itemName, iyear, imonth);
      DateTime beginDateTime = DateTime.Now, endDateTime= DateTime.Now;
      int Count = 1;
      for(int i=1;i<=qcData.Tables[0].Rows.Count;i++)
      {
        if((i-1)%8 == 0)
        {
          beginDateTime = Convert.ToDateTime(qcData.Tables[0].Rows[i - 1]["QCDate"]);
        }
        if(i%8 == 0)
        {
          endDateTime = Convert.ToDateTime(qcData.Tables[0].Rows[i - 1]["QCDate"]);
          string otherCondition = "(QCDate>=" + beginDateTime.ToString() + " and QCDate <=" + endDateTime.ToString() + ")";
          DataSet data = reportPrintModel.GetQCData(itemName, iyear, imonth, otherCondition);
          reportDataSourceName.Add("Query" + Count.ToString());
          reportDataSourceDataSet.Add(data); 
          Count++;
        }
      }
      if(qcData.Tables[0].Rows.Count < 8 && qcData.Tables[0].Rows.Count > 0)
      {
        endDateTime = Convert.ToDateTime(qcData.Tables[0].Rows[qcData.Tables[0].Rows.Count-1]["QCDate"]);
        string otherCondition = "(QCDate>=#" + beginDateTime.ToString() + "# and QCDate <=#" + endDateTime.ToString() + "#)";
        DataSet data = reportPrintModel.GetQCData(itemName, iyear, imonth, otherCondition);
        reportDataSourceName.Add("Query" + Count.ToString());
        reportDataSourceDataSet.Add(data);
      }
    }

    private void PrepareReportData(int sampleID, DateTime dateTime, out List<System.Data.DataSet> reportDataSourceDataSet, out List<string> reportDataSourceName)
    {
      reportDataSourceDataSet = new List<System.Data.DataSet>();
      reportDataSourceName = new List<string>();
      DataSet patientDataSet = reportPrintModel.GetSampleInfo(sampleID, dateTime);
      reportDataSourceName.Add("Query");
      reportDataSourceDataSet.Add(patientDataSet);

      DataSet analyzerDataSet = reportPrintModel.GetSampleAnalyzerResult(sampleID, dateTime);
      reportDataSourceName.Add("AnalyzerQuery");
      reportDataSourceDataSet.Add(analyzerDataSet);

      DataSet manualDataSet = reportPrintModel.GetSampleManualResult(sampleID, dateTime);
      if (manualDataSet.Tables.Count > 0 && manualDataSet.Tables[0].Rows.Count > 0)
      {
        reportDataSourceName.Add("ManualQuery");
        reportDataSourceDataSet.Add(manualDataSet);
      }
    }
    public void PrintReport(int sampleID,DateTime dateTime)
    {
      List<DataSet> datas;
      List<string> names;
      PrepareReportData(sampleID, dateTime, out datas, out names);
      PrintSetItem samplePrintParam = new PrintSetItem();
      printSetParamViewModel.LoadSampleReportSet(samplePrintParam);
      if (datas.Count > 2)
      {
        reportPrint = new ReportPrint.ReportPrint(samplePrintParam.ReportManualName, dbConnectionParamName, dbConnectionString);
      }
      else
      {
        reportPrint = new ReportPrint.ReportPrint(samplePrintParam.ReportName, dbConnectionParamName, dbConnectionString);
      }
      parameterList.Clear();
      parameterList.Add("SampleTitle", samplePrintParam.ReportTitle[0].InfoValue);
      parameterList.Add("HospitalTitle", samplePrintParam.ReportFooter[0].InfoValue);
      reportPrint.LoadReportData(names, datas, parameterList);
      reportPrint.Print();
    }
    public DataSet GetBatchSample(int beginSampleID, int endSampleID, DateTime testDate, bool isTest, bool isAuditor)
    {
      string otherCondition = "";
      string strDateTimeCondition = string.Format("(CheckDate >= #{0} 00:00:00# and CheckDate <= #{0} 23:59:59#) ", testDate.ToString(Const.DateFormat));
      if (isTest && isAuditor)
      {
        otherCondition = "IsAuditor = 1 and IsTest = 1 and "+ strDateTimeCondition;
      }
      else if (isTest)
      {
        otherCondition = "IsTest = 1 and " + strDateTimeCondition;
      }
      else if (isAuditor)
      {
        otherCondition = "IsAuditor = 1 and " + strDateTimeCondition;
      }
      else
      {
        otherCondition = strDateTimeCondition;
      }
      return reportPrintModel.GetSampleInfos(beginSampleID, endSampleID, otherCondition);
    }
    public void PrintReport(DataSet batchSampleDataSet, System.Windows.Window window, System.Windows.Controls.Label showInfo, System.Windows.Controls.ProgressBar progress)
    {
      if (batchSampleDataSet.Tables[0].Rows.Count == 0)
      {
        return;
      }
      int i = 1;
      foreach (DataRow row in batchSampleDataSet.Tables[0].Rows)
      {
        int currentSampleID = Convert.ToInt32(row["SampleID"]);
        DateTime registDate = Convert.ToDateTime(row["RegistDate"]);
        SetTextandPosMesssage(window, showInfo, progress, i, "正在打印"+ currentSampleID+"号报告单");
        PrintReport(currentSampleID, registDate);
        i++;
      }
    }
    public void PrintReport(string itemName, string qcType, string qcItemRange, int iyear, int imonth, ChartControl chart, Window owner)
    {
      List<DataSet> datas;
      List<string> names;
      PrepareReportData(itemName, iyear, imonth, out datas, out names);
      PrintSetItem qcPrintParam = new PrintSetItem();
      printSetParamViewModel.LoadQCReportSet(qcPrintParam);
      reportPrint = new ReportPrint.ReportPrint(qcPrintParam.ReportName, dbConnectionParamName, dbConnectionString);
      parameterList.Clear();
      parameterList.Add("QCTitle", qcPrintParam.ReportTitle[0].InfoValue);
      parameterList.Add("QCItem", itemName);
      parameterList.Add("QCType", qcType);
      parameterList.Add("QCDate", iyear + "年" + imonth + "月");
      parameterList.Add("QCRange", qcItemRange);

      string exportFileName = Path.GetFullPath(qcPrintParam.ReportName) + "QCChart.bmp";
      chart.ExportToImage(exportFileName);
      Dictionary<string, string> reportPicutureData = new Dictionary<string, string>();
      reportPicutureData.Add("Picture1", exportFileName);

      reportPrint.LoadReportData(names, datas, reportPicutureData, parameterList);
      reportPrint.Print();
    }
    public void ReportPreview(string itemName,string qcType,string qcItemRange,int iyear, int imonth,ChartControl chart, Window owner)
    {
      List<DataSet> datas;
      List<string> names;
      PrepareReportData(itemName,iyear,imonth, out datas, out names);
      PrintSetItem qcPrintParam = new PrintSetItem();
      printSetParamViewModel.LoadQCReportSet(qcPrintParam);
      reportPreview = new PreviewerWindow(qcPrintParam.ReportName, dbConnectionParamName, dbConnectionString);
      //参数
      parameterList.Clear();
      parameterList.Add("QCTitle", qcPrintParam.ReportTitle[0].InfoValue);
      parameterList.Add("QCItem", itemName);
      parameterList.Add("QCType", qcType);
      parameterList.Add("QCDate", iyear + "年" + imonth + "月");
      parameterList.Add("QCRange", qcItemRange);

      string exportFileName = Path.GetFullPath(qcPrintParam.ReportName) + "QCChart.bmp";
      chart.ExportToImage(exportFileName);
      Dictionary<string, string> reportPicutureData = new Dictionary<string, string>();
      reportPicutureData.Add("Picture1", exportFileName);

      reportPreview.MyPrintReport.LoadReportData(names, datas, reportPicutureData, parameterList);
      reportPreview.Owner = owner;
      reportPreview.ShowDialog();
    }

    public void ReportPreview(int sampleID, DateTime dateTime, Window owner)
    {
      List<DataSet> datas;
      List<string> names;
      PrepareReportData(sampleID, dateTime, out datas, out names);
      PrintSetItem samplePrintParam = new PrintSetItem();
      printSetParamViewModel.LoadSampleReportSet(samplePrintParam);
      if (datas.Count > 2)
      {
        reportPreview = new PreviewerWindow(samplePrintParam.ReportManualName, dbConnectionParamName, dbConnectionString);
      }
      else
      {
        reportPreview = new PreviewerWindow(samplePrintParam.ReportName, dbConnectionParamName, dbConnectionString);
      }
      parameterList.Clear();
      parameterList.Add("SampleTitle", samplePrintParam.ReportTitle[0].InfoValue);
      parameterList.Add("HospitalTitle", samplePrintParam.ReportFooter[0].InfoValue);
      reportPreview.MyPrintReport.LoadReportData(names, datas, parameterList);
      reportPreview.Owner = owner;
      reportPreview.ShowDialog();
    }

    //public void ReportPreview(Window owner,string reportFileName, enumReportType reportType)
    //{
    //  PrintSetItem printParam = new PrintSetItem();
    //  if(reportType == enumReportType.Sample)
    //  {
    //    printSetParamViewModel.LoadSampleReportSet(printParam);
    //  }
    //  else
    //  {
    //    printSetParamViewModel.LoadQCReportSet(printParam);
    //  }
    //  reportPreview = new PreviewerWindow(reportFileName);
    //  parameterList.Clear();
    //  parameterList.Add("SampleTitle", printParam.ReportTitle);
    //  parameterList.Add("HospitalTitle", printParam.ReportFooter);
    //  reportPreview.MyPrintReport.LoadReportData(null,null, parameterList);
    //  reportPreview.Owner = owner;
    //  reportPreview.ShowDialog();
    //}
  }
}
