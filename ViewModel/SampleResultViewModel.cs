using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ViewModel
{
  //public class SampleResultNameViewObj
  //{
  //  private EM_CHEMICAL_ITEMS itemID;
  //  /// <summary>
  //  /// 项目ID
  //  /// </summary>
  //  public EM_CHEMICAL_ITEMS ItemID
  //  {
  //    get { return itemID; }
  //    set { itemID = value; }
  //  }

  //  private string caption;
  //  public string Caption
  //  {
  //    get { return caption; }
  //    set { caption = value; }
  //  }

  //  private string name;
  //  public string Name
  //  {
  //    get { return name; }
  //    set { name = value; }
  //  }

  //}

  public class SampleResultViewModel
  {
    private SampleResultModel sampleResultModel = new SampleResultModel();
    private ItemConfigViewModel itemconfigviewmodel = new ItemConfigViewModel();
    private ObservableCollection<DBSampleResultInfo> resultAnalyzerList = new ObservableCollection<DBSampleResultInfo>();
    private ObservableCollection<DBSampleResultInfo> resultManualList = new ObservableCollection<DBSampleResultInfo>();
    private List<DBChemicalItemInfo> itemNameList = new List<DBChemicalItemInfo>();

    public SampleResultViewModel()
    {
      
    }
  
    public IList<DBSampleResultInfo> ResultAnalyzerList
    {
      get { return resultAnalyzerList; }
    }

    public IList<DBSampleResultInfo> ResultManualList
    {
      get { return resultManualList; }
    }

    public List<DBChemicalItemInfo> ItemNameList
    {
      get { return itemNameList; }
    }
      
    public void UpdateAnalyzerResult(DBSampleInfo info)
    {
      if (info != null)
      {
        sampleResultModel.GetAnalyzerResult(info, resultAnalyzerList);
        foreach (DBSampleResultInfo item in resultAnalyzerList)
        {
          DBChemicalItemInfo itemInfo = itemconfigviewmodel.GetChemicalItem(item.ItemID);
          item.ChemicalItemInfoList = ItemNameList;
          item.Caption = itemInfo.Caption;
          item.Range = itemInfo.Range;
          item.Unit = itemInfo.Unit;
          DBChemicalResultInfo result = itemInfo.Items.SingleOrDefault(ri => ri.Index == item.ResultIndex);
          if (result != null)
          {
            item.Result = result.Result;
          }
        }
      }
      else
      {
        resultAnalyzerList.Clear();
      }
    }

    public void UpdateManualResult(DBSampleInfo info)
    {
      if (info != null)
      {
        sampleResultModel.GetManualResult(info, resultManualList);
        foreach (DBSampleResultInfo item in resultManualList)
        {
          DBChemicalItemInfo itemInfo = itemconfigviewmodel.GetChemicalItem(item.ItemID);
          item.ChemicalItemInfoList = ItemNameList;
          item.Caption = itemInfo.Caption;
          item.Range = itemInfo.Range;
          item.Unit = itemInfo.Unit;
          DBChemicalResultInfo result = itemInfo.Items.SingleOrDefault(ri => ri.Index == item.ResultIndex);
          if (result != null)
          {
            item.Result = result.Result;
          }
        }
      }
      else
      {
        resultManualList.Clear();
      }
    }

    public bool SaveAnalyzerResult()
    {
      foreach(DBSampleResultInfo item in resultAnalyzerList)
      {
        if (sampleResultModel.UpdateAnalyzerResult(item) == 0)
        {
          return false;
        }
        else
        {
          item.IsModify = false;
        }
      }
      return true;
    }

    public bool SaveManualResult()
    {
      foreach (DBSampleResultInfo item in resultManualList)
      {
        if (sampleResultModel.UpdateManualResult(item) == 0)
        {
          return false;
        }
        else
        {
          item.IsModify = false;
        }
      }
      return true;
    }

    public bool DeleteAnalyzerResult(DBSampleResultInfo resultInfo)
    {
      return sampleResultModel.DeleteAnalyzerResult(resultInfo) > 0;
    }

    public bool DeleteManualResult(DBSampleResultInfo resultInfo)
    {
      return sampleResultModel.DeleteManualResult(resultInfo) > 0;
    }
  }
}
