using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
  public class SampleInfoViewModel
  {
    private ObservableCollection<DBSampleInfo> sampleInfoList = new ObservableCollection<DBSampleInfo>();
    private SampleInfoModel sampleInfoModel = new SampleInfoModel();
    private bool isSampleInfoEdit = false;
    private DBSampleInfo curSampleInfoEditObj = null;
    private bool isSampleAdd = false;
    private DateTime startDate = DateTime.Now;
    private DateTime endDate = DateTime.Now;


    public DateTime StartDate
    {
      get { return startDate; }
      set { startDate = value; }
    }

    public DateTime EndDate
    {
      get { return endDate; }
      set { endDate = value; }
    }
    /// <summary>
    /// 是否为样本添加
    /// </summary>
    public bool IsSampleAdd
    {
      get { return isSampleAdd; }
      set { isSampleAdd = value; }
    }
    /// <summary>
    /// 样本信息编辑状态
    /// </summary>
    public bool IsSampleInfoEdit
    {
      get { return isSampleInfoEdit; }
      set { isSampleInfoEdit = value; }
    }
    /// <summary>
    /// 当前样本编辑对象
    /// </summary>
    public DBSampleInfo CurSampleInfoEditObj
    {
      get { return curSampleInfoEditObj; }
      set { curSampleInfoEditObj = value; }
    }
    /// <summary>
    /// 样本信息列表
    /// </summary>
    public IList<DBSampleInfo> SampleInfoList
    {
      get { return sampleInfoList; }
    }

    public SampleInfoViewModel()
    {
      sampleInfoModel.Query(sampleInfoList);
    }

    public void UpdateSampleInfoList()
    {
      sampleInfoModel.Query(startDate,endDate,sampleInfoList);
    }

    public void UpdateSampleInfoList(DateTime? beginDate, DateTime? endDate, string otherCondition)
    {
      UIExecute.RunAsync(()=> {
        sampleInfoModel.Query(beginDate, endDate, otherCondition, sampleInfoList);
      });
      
    }
    /// <summary>
    /// 从样本列表中获取指令记录
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public DBSampleInfo GetSampleInfoFromList(DBSampleInfo info)
    {
      foreach (DBSampleInfo item in sampleInfoList)
      {
        if (info.SampleID == item.SampleID && info.RegistDate == item.RegistDate)
        {
          return item;
        }
      }
      return null;
    }
    /// <summary>
    /// 样本在数据库中是否存在
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool IsExistsInDB(DBSampleInfo info)
    {
      return sampleInfoModel.IsExists(info);
    }
    /// <summary>
    /// 样本保存
    /// </summary>
    /// <returns></returns>
    public bool SampleInfoSave()
    {
      IsSampleAdd = false;
      return sampleInfoModel.Update(CurSampleInfoEditObj) > 0 ? true : false;
    }
    /// <summary>
    /// 样本审核
    /// </summary>
    /// <param name="IsAuditor"></param>
    /// <returns></returns>
    public bool SampleInfoAuditor(DBSampleInfo info,bool IsAuditor)
    {
      info.IsAuditor = IsAuditor;
      if (IsAuditor)
      {
        info.Auditor = "admin";
      }
      else
      {
        info.Auditor = "";
      }
      return sampleInfoModel.Update(info) > 0 ? true : false;
    }

    public int GetMaxSampleID()
    {
      return sampleInfoModel.GetMaxSampleID();
    }

    public bool SampleInfoDelete(DBSampleInfo info)
    {
      return sampleInfoModel.Delete(info) > 0;
    }

    public bool SampleInfoBatchDelete(DBSampleInfo StartSampleInfo,DBSampleInfo EndSampleInfo)
    {
      return sampleInfoModel.BatchDelete(StartSampleInfo,EndSampleInfo) > 0;
    }
  }
}
