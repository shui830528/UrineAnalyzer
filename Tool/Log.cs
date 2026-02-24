using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tool
{
  public class Log
  {
    private static Mutex m_SignalMutex = new Mutex();
    public static List<string> LogList = new List<string>();

    public static Mutex SignalMutex
    {
      get { return Log.m_SignalMutex; }
      set { Log.m_SignalMutex = value; }
    }
    //日志保存路径，不包括文件名
    private static string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "log";
    //日志完整路径，包括文件名
    private static string logFileName = filePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

    /// <summary>
    /// 创建文件夹和日志文件
    /// </summary>
    private static void CreateLogFile()
    {
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
      if (!File.Exists(logFileName))
      {
        FileStream filestream = null;
        try
        {
          filestream = File.Create(logFileName);
          /*创建日志头部*/
          filestream.Dispose();
          filestream.Close();
          CreateLogHead();
        }
        catch (System.Exception ex)
        {
          throw new System.Exception(ex + "创建日志文件失败");
        }
      }
    }

    public static void SaveLog()
    {
      //创建一个文件流，用以写入或者创建一个StreamWriter 
      FileStream fs = new FileStream(Log.logFileName, FileMode.OpenOrCreate, FileAccess.Write);
      StreamWriter sw = new StreamWriter(fs);
      sw.Flush();
      // 使用StreamWriter来往文件中写入内容 
      sw.BaseStream.Seek(0, SeekOrigin.Begin);
      for (int i = 0; i < LogList.Count; i++) sw.WriteLine(LogList[i]);
      //关闭此文件t 
      sw.Flush();
      sw.Close();
      fs.Close();
    }

    /// <summary>
    /// 创建日志头部
    /// </summary>
    private static void CreateLogHead()
    {
      System.IO.StreamWriter sw = null;
      try
      {
        sw = new System.IO.StreamWriter(logFileName, true, System.Text.Encoding.UTF8);
        sw.WriteLine();
        sw.WriteLine("【日志创建时间***********】【日志内容*****************************************************】");
        sw.WriteLine();
      }
      catch { }
      finally
      {
        sw.Flush();
        sw.Dispose();
        sw.Close();
      }
    }

    /// <summary>
    ///写入日志，
    ///自动创建文件夹和文件，
    ///日志文件创建到系统启动项根目录的Log文件夹下。
    /// </summary>
    /// <param name="logText">参数，日志内容</param>
    /// <returns>日志写入成功返回true,失败返回false</returns>
    public static void WriteLog(string logText)
    {
      SignalMutex.WaitOne();
      CreateLogFile();
      //true 如果日志文件存在则继续追加日志 
      System.IO.StreamWriter sw = null;
      try
      {
        sw = new System.IO.StreamWriter(logFileName, true, System.Text.Encoding.UTF8);
        sw.WriteLine("【  " + System.DateTime.Now.ToString() + "  】" + "【  " + logText + "  】");
      }
      catch (System.Exception ex)
      {
        throw new System.Exception(ex + "写入日志失败，检查！");
      }
      finally
      {
        sw.Flush();
        sw.Dispose();
        sw.Close();
        SignalMutex.ReleaseMutex();
      }
    }

    public static void WriteLogToFileA(string logText)
    {
      LogList.Add("【  " + System.DateTime.Now.ToString() + "  】" + "【  " + logText + "  】");
    }
  }
}
