using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using FastReport.Utils;
using System.Data;
using FastReport.MSChart;

namespace ReportPrint
{

  public class ReportPrint
  {
    private Report report;
    public Report MyReport
    {
      get { return report; }
    }
    private string fileName;
    private Dictionary<string, string> parameterList = new Dictionary<string, string>();
    public Dictionary<string,string> ParameterList
    {
      get { return parameterList; }
      set { parameterList = value; }
    }
    public void LoadReportData(List<string> reportDataSourceName, List<System.Data.DataSet> reportDataSourceDataSet, string reportTempletDir, Dictionary<string,string> reportDataSourcePicture)
    {
      LoadReportData(reportDataSourceName, reportDataSourceDataSet, reportTempletDir);
      if (reportDataSourcePicture != null)
      {
        foreach (var pic in reportDataSourcePicture)
        {
          PictureObject picture = (PictureObject)report.FindObject(pic.Key);
          if (picture != null)
          {
            picture.ImageLocation = pic.Value;
          }
        }
      }
    }
    public void LoadReportData(List<string> reportDataSourceName,List<System.Data.DataSet> reportDataSourceDataSet, string reportTempletDir)
    {
      try
      {
        report.Load(System.AppDomain.CurrentDomain.BaseDirectory + reportTempletDir + fileName + ".frx");
        if (reportDataSourceDataSet != null && reportDataSourceName != null)
        {
          for (int i = 0;i<reportDataSourceDataSet.Count;i++)
          {
            if (i < reportDataSourceName.Count)
            {
              reportDataSourceDataSet[i].Tables[0].TableName = reportDataSourceName[i];
            }
            report.RegisterData(reportDataSourceDataSet[i]);
            if (i < reportDataSourceName.Count)
            {
              report.GetDataSource(reportDataSourceName[i]).Enabled = true;
            }
          }
        }
        if (parameterList != null)
        {
          foreach (var para in parameterList)
          {
            report.SetParameterValue(para.Key, para.Value);
          }
        }
      }
      catch(Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
    public void Print()
    {
      report.PrintSettings.ShowDialog = false;
      report.Print();
    }
    public ReportPrint(string reportFileName)
    {
      report = new Report();
      fileName = reportFileName;
      Config.ReportSettings.ShowProgress = false;
    }
  }
}
