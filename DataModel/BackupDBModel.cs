using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
  public class BackupDBModel
  {
    private DB db = new DB();
    public void ClearTestData()
    {
      List<string> sqlList = new List<string>();
      sqlList.Add("delete from Tab_SampleAnalyzerResult");
      sqlList.Add("delete from Tab_SampleManualResult");
      sqlList.Add("delete from Tab_SampleInfo");
      db.BatchExecute(sqlList);
    }
  }
}
