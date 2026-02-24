using System;
using System.IO;
using System.Threading;
using System.Xml;

namespace Tool
{
  public class XML
  {
    static public string DIR_CONFIG = "Configure";
    static public string DIR_REPORT = "Reports";

    private static Mutex m_SignalMutex = new Mutex();
    protected XmlDocument xmlDoc = new XmlDocument();
    protected XmlElement xmlRoot = null;
    private string xmlFileName = null;
    private static string filePath = System.AppDomain.CurrentDomain.BaseDirectory;

    public XmlElement Root
    {
      get { return xmlRoot; }
    }

    public static Mutex SignalMutex
    {
      get { return m_SignalMutex; }
      set { m_SignalMutex = value; }
    }
    protected XML()
    {

    }
    public XML(string DirName, string FileName)
    {
      filePath = "";
      xmlFileName = FileName;
      filePath += DirName;
      //文件不存在，则创建新文件
      if (!Directory.Exists(filePath))
      {
        try
        {
          //按照路径创建目录
          Directory.CreateDirectory(filePath);
        }
        catch (System.Exception e)
        {
          throw new System.Exception(e + "创建目录失败！");
        }
      }

      if (!File.Exists(GetXmlFileName()))
      {
        CreateXmlFile();
      }
      else
      {
        ReadXmlFile();
      }
    }
    public bool XmlExists
    {
      get { return File.Exists(GetXmlFileName()); }
    }
    /// <summary>
    /// XML文件名
    /// </summary>
    /// <returns></returns>
    virtual public string GetXmlFileName()
    {
      return filePath + "//" + xmlFileName;
    }
    /// <summary>
    /// 创建根节点
    /// </summary>
    private void CreateRoot()
    {
      xmlRoot = xmlDoc.CreateElement("ROOT");
      xmlDoc.AppendChild(xmlRoot);
    }
    /// <summary>
    /// 创建XML文件
    /// </summary>
    /// <returns></returns>
    protected bool CreateXmlFile()
    {
      bool bResult = false;
      SignalMutex.WaitOne();
      try
      {
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
        CreateRoot();
        bResult = true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + xmlFileName + "创建失败");
      }
      finally
      {
        SignalMutex.ReleaseMutex();
      }


      return bResult;
    }
    /// <summary>
    /// 读取XML文件
    /// </summary>
    /// <returns></returns>
    protected bool ReadXmlFile()
    {
      bool bResult = false;
      SignalMutex.WaitOne();
      try
      {
        xmlDoc.Load(GetXmlFileName());
        xmlRoot = (XmlElement)xmlDoc.SelectSingleNode("ROOT");
        if (xmlRoot == null)
        {
          CreateRoot();
        }
        bResult = true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + xmlFileName + "加载失败");
      }
      finally
      {
        SignalMutex.ReleaseMutex();
      }

      return bResult;
    }
    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="NodeName">节点名称(分类名)</param>
    /// <param name="PropertyName">属性名</param>
    /// <param name="PropertyValue">属性值</param>
    /// <returns></returns>
    protected bool SetNodeValue(string NodeName, string PropertyName, string PropertyValue)
    {
      SignalMutex.WaitOne();
      bool bResult = false;
      try
      {
        XmlElement xmlNode = null;
        xmlNode = (XmlElement)xmlRoot.SelectSingleNode(NodeName);
        if (xmlNode == null)
        {
          xmlNode = xmlDoc.CreateElement(NodeName);
        }

        xmlNode.SetAttribute(PropertyName, PropertyValue);
        xmlRoot.AppendChild(xmlNode);
        bResult = true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + xmlFileName + "写数据失败");
      }
      finally
      {
        SignalMutex.ReleaseMutex();
      }

      return bResult;
    }
    /// <summary>
    /// 读取节点值
    /// </summary>
    /// <param name="NodeName">节点名称(分类名)</param>
    /// <param name="PropertyName">属性名</param>
    /// <param name="PropertyDefaultValue">属性默认值</param>
    /// <returns></returns>
    protected string GetNodeValue(string NodeName, string PropertyName, string PropertyDefaultValue)
    {
      string Result = null;
      SignalMutex.WaitOne();
      try
      {
        XmlElement xmlNode = null;
        xmlNode = (XmlElement)xmlRoot.SelectSingleNode(NodeName);
        if (xmlNode == null)
        {
          SetNodeValue(NodeName, PropertyName, PropertyDefaultValue);
          xmlNode = (XmlElement)xmlRoot.SelectSingleNode(NodeName);
        }
        if (xmlNode.GetAttributeNode(PropertyName) == null)
        {
          xmlNode.SetAttribute(PropertyName, PropertyDefaultValue);
        }

        Result = xmlNode.GetAttribute(PropertyName);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + xmlFileName + "读数据失败");
      }
      finally
      {
        SignalMutex.ReleaseMutex();
      }


      return Result;
    }
    /// <summary>
    /// 保存XML文件
    /// </summary>
    /// <returns></returns>
    public bool SaveXml()
    {
      bool bResult = false;
      SignalMutex.WaitOne();
      try
      {
        xmlDoc.Save(GetXmlFileName());
        bResult = true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message + xmlFileName + "保存失败");
      }
      finally
      {
        SignalMutex.ReleaseMutex();
      }
      return bResult;
    }
    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Default"></param>
    /// <returns></returns>
    public string ReadString(string Title, string Name, string Default)
    {
      string value = GetNodeValue(Title, Name, Default);
      if (value != null)
        return value;
      return "";
    }
    /// <summary>
    /// 写字符串值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public bool WriteString(string Title, string Name, string Value)
    {
      return SetNodeValue(Title, Name, Value);
    }
    /// <summary>
    /// 读整型值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Default"></param>
    /// <returns></returns>
    public int ReadInt(string Title, string Name, int Default)
    {
      return Convert.ToInt32(ReadString(Title, Name, Default.ToString()));
    }
    /// <summary>
    /// 写整型值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public bool WriteInt(string Title, string Name, int Value)
    {
      return WriteString(Title, Name, Value.ToString());
    }
    /// <summary>
    /// 读取单精度符点型值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Defalut"></param>
    /// <returns></returns>
    public float ReadFloat(string Title, string Name, float Defalut)
    {
      return Convert.ToSingle(ReadString(Title, Name, Defalut.ToString()));
    }
    /// <summary>
    /// 写单精度符点值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public bool WriteFloat(string Title, string Name, float Value)
    {
      return WriteString(Title, Name, Value.ToString());
    }
    /// <summary>
    /// 读取布尔值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Default"></param>
    /// <returns></returns>
    public bool ReadBool(string Title, string Name, bool Default)
    {
      return Convert.ToBoolean(ReadString(Title, Name, Default.ToString()));
    }
    /// <summary>
    /// 写布尔值
    /// </summary>
    /// <param name="Title"></param>
    /// <param name="Name"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public bool WriteBool(string Title, string Name, bool Value)
    {
      return WriteString(Title, Name, Value.ToString());
    }
    public DateTime ReadDateTime(string Title, string Name, DateTime Default)
    {
      return Convert.ToDateTime(ReadString(Title, Name, Default.ToString("yyyy-MM-dd hh:mm:ss")));
    }
    public bool WriteDateTime(string Title, string Name, DateTime Value)
    {
      return WriteString(Title, Name, Value.ToString("yyyy-MM-dd hh:mm:ss"));
    }
    public char ReadChar(string Title, string Name, char Default)
    {
      return ReadString(Title, Name, new string(new char[] { Default })).ToCharArray()[0];
    }
    public bool WriteChar(string Title, string Name, char Value)
    {
      return WriteString(Title, Name, new string(new char[] { Value }));
    }
  }
}
