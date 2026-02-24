using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataModel
{

  public class DBInstrumentType
  {
    private EM_INSTRUMENT_TYPE id;
    public EM_INSTRUMENT_TYPE ID
    {
      get { return id; }
      set { id = value; }
    }

    public string Caption
    {
      get;
      set;
    }
  }

  public class DBLinkParam : INotifyPropertyChanged
  {
    public string DBName { get; set; }
    private string portName;
    public string PortName
    {
      get { return portName; }
      set { portName = value; OnPropertyChanged("PortName"); }
    }

    private int baudRate;
    public int BaudRate
    {
      get { return baudRate; }
      set { baudRate = value; OnPropertyChanged("BaudRate"); }
    }

    private DBInstrumentType instrumentType;
    public DBInstrumentType InstrumentType
    {
      get { return instrumentType; }
      set { instrumentType = value; OnPropertyChanged("InstrumentType"); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }

  public class InstrumentTypeList
  {
    private static List<DBInstrumentType> items = null;
    public IList<DBInstrumentType> Items
    {
      get { return items; }
    }

    public InstrumentTypeList()
    {
      if (items == null)
      {
        items = new List<DBInstrumentType>();
        items.Add(new DBInstrumentType() { ID = EM_INSTRUMENT_TYPE.INSTRUMENT_TYPE_SEMI_AUTOMATIC, Caption = "半自动尿仪" });
        items.Add(new DBInstrumentType() { ID = EM_INSTRUMENT_TYPE.INSTRUMENT_TYPE_FULL_AUTOMATIC, Caption = "全自动尿仪" });
      }

    }

    public DBInstrumentType GetInstrumentType(EM_INSTRUMENT_TYPE id)
    {
      foreach (DBInstrumentType item in items)
      {
        if (id == item.ID)
        {
          return item;
        }
      }
      return null;
    }

  }

  public class LinkParamModel
  {
    private static XML xml = null;
    private InstrumentTypeList instrumentType = new InstrumentTypeList();

    public LinkParamModel()
    {
      if (xml == null)
      {
        xml = new XML("Config","LinkParam.xml");
      }
    }

    public void Load(DBLinkParam linkParam)
    {
      try
      {
        linkParam.DBName = xml.ReadString("DB", "DBName", System.AppDomain.CurrentDomain.BaseDirectory + "Database.mdb");
        linkParam.PortName = xml.ReadString("PortConfig", "PortName", "");
        linkParam.BaudRate = xml.ReadInt("PortConfig", "BaudRate", 19200);

        linkParam.InstrumentType = instrumentType.GetInstrumentType((EM_INSTRUMENT_TYPE)xml.ReadInt("InstrumentConfig", "InstrumentType", 0));
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + "LinkParamModel.Load Error");
      }

    }

    public bool Save(DBLinkParam linkParam)
    {
      try
      {
        xml.WriteString("DB", "DBName", linkParam.DBName);
        xml.WriteString("PortConfig", "PortName", linkParam.PortName);
        xml.WriteInt("PortConfig", "BaudRate", linkParam.BaudRate);

        xml.WriteInt("InstrumentConfig", "InstrumentType", (int)linkParam.InstrumentType.ID);
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + "LinkParamModel.Save Error");
      }

      return xml.SaveXml();
    }
  }
}
