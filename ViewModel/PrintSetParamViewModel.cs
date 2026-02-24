using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
  public class PrintSetParamViewModel
  {
    private PrintSetModel printSetModel = new PrintSetModel();
    public void LoadSampleReportSet(PrintSetItem samplePrintParam)
    {
      printSetModel.LoadSamplePrintParam(samplePrintParam);
    }
    public void LoadQCReportSet(PrintSetItem qcPrintParam)
    {
      printSetModel.LoadQCPrintParam(qcPrintParam);
    }
  }
}
