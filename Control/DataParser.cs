using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using DataModel;

namespace Control
{
  public enum TestType
  {
    ttSample = 0,
    ttQC = 1
  }
  public class ParserResult
  {
    public int SampleID
    {
      get;
      set;
    }
    public DateTime TestDate
    {
      get;
      set;
    }
    public string Abnormal
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
    public EM_CHEMICAL_ITEMS ItemID
    {
      get;
      set;
    }
    public string Result
    {
      get;
      set;
    }
    public string Unit
    {
      get;
      set;
    }
    public int ResultIndex
    {
      get;
      set;
    }
  }
  public class DataParser
  {
    private DBItemConfigModel itemConfig;
    //private Heart heart = null;
    private Snapshot snapshot = null;
    public DataParser()
    {
      itemConfig = new DBItemConfigModel();
      //heart = new Heart();
      snapshot = new Snapshot();
      //heart.Start();
      snapshot.Connect(0);
    }
    private string sHeart = "1";
    private string sSnapshot = "2";
    public void OnDataReceive(string InsStr)
    {
      List<ParserResult> resultList = new List<ParserResult>();
      char dataGroupSeparator = ',';
      char dataSeparator = '|';
      char resultSeparator = ';';
      InsStr = InsStr.Replace("\r", "");
      InsStr = InsStr.Replace("\n", "");
      List<string> InsLine = InsStr.Split(dataGroupSeparator).ToList<string>();
      if (InsLine.Count == 1)
      {
        string command = InsLine[0].Replace("$", "");
        command = command.Replace("#", "");
        //心跳
        if(command == sHeart)
        {
          //heart.HeartEvent.Set();
        }
        //拍照
        if(command == sSnapshot)
        {
          try
          {
            Const.ImageFileName = snapshot.CaptureImage();
          }
          catch
          {
            Const.ImageFileName = string.Empty;
          }
        }
      }
      else
      {
        //第一条数据是测试类型（样本、质控）
        bool IsNeg = false;
        TestType testType = TestType.ttSample;

        if (InsLine.Count < 3 || InsLine[1] != "23" || InsLine[2] != "3")
        {
          return;
        }

        List<string> dataList = InsLine[10].Split(dataSeparator).ToList<string>();
        if (dataList.Count < 8)
        {
          return;
        }
        try
        {
          
          string strType = InsLine[3];
          if (strType == "00")
          {
            testType = TestType.ttSample;
          }
          else if (strType == "10")
          {
            testType = TestType.ttQC;
            IsNeg = true;
          }
          else if (strType == "11")
          {
            testType = TestType.ttQC;
            IsNeg = false;
          }


          //testType = (TestType)Int32.Parse(dataList[0]);
          //if (testType == TestType.ttQC)
          //{
          //  IsNeg = Int32.Parse(dataList[1]) == 1;
          //}


        }
        catch (Exception ex)
        {
          Log.WriteLog(ex.Message +"  "+ InsLine[3] + "数据解析测试类型出错");
        }

        //第二条数据是日期时间
        DateTime testDate = Convert.ToDateTime("1990-01-01 00:00:00");
        try
        {
          //testDate = Convert.ToDateTime(InsLine[1]);
          testDate = Convert.ToDateTime(InsLine[4]);
        }
        catch (Exception ex)
        {
          Log.WriteLog(ex.Message + "数据解析日期出错");
        }

        //第三条数据是样本号
        int nSampleNo = 0;
        try
        {
          //nSampleNo = Int32.Parse(InsLine[2]);
          nSampleNo = Int32.Parse(InsLine[5]);
        }
        catch (Exception ex)
        {
          Log.WriteLog(ex.Message + "  " + InsLine[5] + "数据解析编号出错");
        }
        //第四条数据是架号、管号、架条码、管条码
        //List<string> sampleInfoList = InsLine[3].Split(dataSeparator).ToList<string>();
        string sampleBarCode = InsLine[9];
        
        //第五条数据及以后是测试数据
        try
        {
          for (int i = 0; i < dataList.Count; i++)
          {
            string str = dataList[i];
            if (str != string.Empty)
            {
              var ResultList = str.Split(resultSeparator).ToList<string>();
              ParserResult resultInfo = new ParserResult();
              resultInfo.SampleID = nSampleNo;
              resultInfo.TestDate = testDate;
              resultInfo.Abnormal = ResultList[0].Trim();
              resultInfo.Name = ResultList[1].Trim();
              resultInfo.ItemID = (EM_CHEMICAL_ITEMS)itemConfig.ItemNos[resultInfo.Name];
              resultInfo.Unit = ResultList[3].Trim();
              resultInfo.Result = ResultList[2].Trim();
              resultInfo.ResultIndex = int.Parse(ResultList[4].Trim());
              resultList.Add(resultInfo);
            }
          }
        }
        catch (Exception ex)
        {
          Log.WriteLog(ex.Message + "项目解析出错");
        }

        try
        {
          if (testType == TestType.ttSample)
          {
            DBSampleInfo sampleInfo = new DBSampleInfo();
            sampleInfo.SampleID = nSampleNo;
            sampleInfo.BarCode = sampleBarCode;
            sampleInfo.RegistDate = testDate;
            sampleInfo.IsTest = true;
            sampleInfo.ImportDate = testDate;
            sampleInfo.ImageFile = Const.ImageFileName;
            Const.ImageFileName = string.Empty;

            SampleInfoModel dbSampleModel = new SampleInfoModel();
            dbSampleModel.Update(sampleInfo);
            DataUpdate.DoUpdate("SampleManage",null,null);

            SampleResultModel dbResultModel = new SampleResultModel();
            dbResultModel.DeleteAllAnalyzerResult(nSampleNo, testDate);
            List<DBSampleResultInfo> SampleResultList = new List<DBSampleResultInfo>();

            foreach (ParserResult item in resultList)
            {
              DBSampleResultInfo SampleResult = new DBSampleResultInfo();

              SampleResult.SampleID = item.SampleID;
              SampleResult.RegistDate = item.TestDate;
              SampleResult.Abnormal = item.Abnormal;
              SampleResult.Name = item.Name;
              SampleResult.ItemID = item.ItemID;
              SampleResult.Result = item.Result;
              SampleResult.Unit = item.Unit;
              SampleResult.ResultIndex = item.ResultIndex;

              SampleResultList.Add(SampleResult);
            }

            dbResultModel.InsertAnalyzerResult(SampleResultList);
          }
          else
          {
            QCResultModel qcResultModel = new QCResultModel();
            List<DBQCResultInfo> QCResultList = new List<DBQCResultInfo>();

            foreach (ParserResult item in resultList)
            {
              DBQCResultInfo QCResult = new DBQCResultInfo();

              QCResult.IsNeg = IsNeg;
              QCResult.QCDate = item.TestDate;
              QCResult.Abnormal = item.Abnormal;
              QCResult.Name = item.Name;
              QCResult.ItemID = item.ItemID;
              QCResult.Result = item.Result;
              QCResult.Unit = item.Unit;
              QCResult.Operation = Const.user;
              QCResult.ItemIndex = item.ResultIndex;

              QCResultList.Add(QCResult);
            }
            qcResultModel.Delete(new DBQCResultInfo() { QCDate = testDate, IsNeg = IsNeg });
            qcResultModel.Insert(QCResultList);

            DataUpdate.DoUpdate("QCPage", null, null);
          }
        }
        catch (Exception ex)
        {
          Log.WriteLog(ex.Message + "数据插入失败");
        }
      }
    }
    public void Stop()
    {
      snapshot.Disconnect();
      //heart.Stop();
    }
  }
}
