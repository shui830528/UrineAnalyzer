using DataModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ViewModel
{


  public class LinkParamViewModel
  {
    private LinkParamModel model = new LinkParamModel();
    private DBLinkParam linkParam = new DBLinkParam();
    private List<string> portNameList = new List<string>();
    private List<int> baudRateList = new List<int>();

    public DBLinkParam LinkParam
    {
      get { return linkParam; }
    }

    public IList<string> PortNameList
    {
      get { return portNameList; }
    }

    public IList<int> BaudRateList
    {
      get { return baudRateList; }
    }

    public LinkParamViewModel()
    {
      Update();
      foreach(string item in SerialPort.GetPortNames())
      {
        portNameList.Add(item);
      }

      int[] baudarray = { 1200,2400,4800,9600,14400,19200,38400,57600,115200 };
      foreach(int item in baudarray)
      {
        baudRateList.Add(item);
      }

    }
    public void Update()
    {
      model.Load(linkParam);
    }
    public bool Save()
    {
      return model.Save(linkParam);
    }
  }
}
