using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
  /// <summary>
  /// 联机仪器类型
  /// </summary>
  public enum EM_INSTRUMENT_TYPE
  {
    INSTRUMENT_TYPE_SEMI_AUTOMATIC,
    INSTRUMENT_TYPE_FULL_AUTOMATIC,

  }
  /// <summary>
  /// 干化学单位类型
  /// </summary>
  public enum EM_ITEM_UNITTYPE
  {
    EM_ITEM_UNITTYPE_CUSTOM,
    /// <summary>
    /// 常规-加号开
    /// </summary>
    EM_ITEM_UNITTYPE_CONVENTION_PLUS_ON,
    /// <summary>
    /// 常规-加号关
    /// </summary>
    EM_ITEM_UNITTYPE_CONVENTION_PLUS_OFF,
    /// <summary>
    /// 国际-加号开
    /// </summary>
    EM_ITEM_UNITTYPE_INTERNATIONAL_PLUS_ON,
    /// <summary>
    /// 国际-加号关
    /// </summary>
    EM_ITEM_UNITTYPE_INTERNATIONAL_PLUS_OFF,
    /// <summary>
    /// 加号系统
    /// </summary>
    EM_ITEM_UNITTYPE_PLUS,
  }
  /// <summary>
  /// 干化学项目
  /// </summary>
  public enum EM_CHEMICAL_ITEMS
  {
    EM_CHEMICAL_ITEM_UBG,
    EM_CHEMICAL_ITEM_BIL,
    EM_CHEMICAL_ITEM_KET,
    EM_CHEMICAL_ITEM_CRE,
    EM_CHEMICAL_ITEM_BLD,
    EM_CHEMICAL_ITEM_PRO,
    EM_CHEMICAL_ITEM_MAL,
    EM_CHEMICAL_ITEM_NIT,
    EM_CHEMICAL_ITEM_LEU,
    EM_CHEMICAL_ITEM_GLU,
    EM_CHEMICAL_ITEM_SG,
    EM_CHEMICAL_ITEM_PH,
    EM_CHEMICAL_ITEM_VC,
    EM_CHEMICAL_ITEM_AC,
    EM_CHEMICAL_ITEM_CA,
    EM_CHEMICAL_ITEM_RESULT,
    EM_CHEMICAL_ITEM_COLOR,
    EM_CHEMICAL_ITEM_CLARITY,
    EM_CHEMICAL_ITEM_TURB
  }

  public interface IDataReceiveParser
  {
    void OnDataReceive(string InsStr);
  }

  public static class Const
  {
    /// <summary>
    /// 日期时间格式
    /// </summary>
    public static string DateFormat = "yyyy-MM-dd";
    public static string TimeFormat = "HH:mm:ss";
    public static string DateTimeFormat =  DateFormat+" "+TimeFormat;
    public static char[] OnLine = new char[5] { (char)(0x02), '$' , '0', '#', (char)(0x03) };
    public static bool Exit = false;
    public static string user = "admin";
    public static string Version = "V1.00";
    public static string authority = "Administrator";
    public static string ImageFileName = string.Empty;
  }
}
