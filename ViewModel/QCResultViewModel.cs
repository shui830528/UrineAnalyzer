using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
  public class QCResultViewModel
  {
    private QCResultModel model = new QCResultModel();
    private ObservableCollection<DBQCInfo> groupItems = new ObservableCollection<DBQCInfo>();
    private ItemConfigViewModel itemConfigViewModel = new ItemConfigViewModel();
    private ObservableCollection<DBQCResultInfo> items = new ObservableCollection<DBQCResultInfo>();

    private int nYear;
    public int Year
    {
      get { return nYear; }
      set { nYear = value; }
    }

    private int nMonth;
    public int Month
    {
      get { return nMonth; }
      set { nMonth = value; }
    }

    private bool isNeg;
    public bool IsNeg
    {
      get { return isNeg; }
      set { isNeg = value; }
    }

    public ObservableCollection<DBQCInfo> GroupItems
    {
      get { return groupItems; }
    }

    public ObservableCollection<DBQCResultInfo> Items
    {
      get { return items; }
    }

    public void UpdateGroupInfo()
    {
      List<DBQCResultInfo> resultList = new List<DBQCResultInfo>();
      string sql = "Select * From Tab_QCResult Where Year(QCDate) = "+
        Year + " and Month(QCDate) = " + Month 
        + " and IsNeg = " + (IsNeg ? "true" : "false") + " Order by QCDate";
      model.GetResult(sql,resultList);

      List<DBQCInfo> GroupList = resultList.GroupBy(o => new{ o.QCDate, o.Operation }).Select(g=>new DBQCInfo{ QCDate=g.Key.QCDate, Operation =g.Key.Operation,Items=g.ToList() }).ToList();
      GroupItems.Clear();
      foreach (DBQCInfo item in GroupList)
      {
        GroupItems.Add(item);
      }
      UpdateItmesInfo(0);

    }

    public void UpdateItmesInfo(int nIndex)
    {
      items.Clear();
      if (groupItems.Count >= (nIndex + 1))
      {
        foreach (DBQCResultInfo item in groupItems[nIndex].Items)
        {
          item.Caption = itemConfigViewModel.GetChemicalItem(item.ItemID).Caption;
          items.Add(item);
        }
      }


    }

    public void DeleteQCResult(DBQCInfo QCInfo,bool IsNeg)
    {
      model.Delete(new DBQCResultInfo() { QCDate = QCInfo.QCDate, IsNeg = IsNeg });
    }
  }
}
