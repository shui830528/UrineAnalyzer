using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tool;

namespace Control
{
  public class DataReceive
  {
    public static SerialPort commPort = new SerialPort();
    private ConcurrentQueue<string> queueList = new ConcurrentQueue<string>();
    private DataParser ReceiveParser = null;

    public DataReceive()
    {
      ReceiveParser = new DataParser();
    }

    private void ReceiveThread()
    {
      try
      {
        string InsStr = "";
        while(commPort.IsOpen && !Const.Exit)
        {
          int bytesCount = commPort.BytesToRead;
          if (bytesCount > 0)
          {
            char[] buffer = new char[bytesCount];
            int nCount = commPort.Read(buffer, 0, bytesCount);
            for (int i = 0; i < nCount; i++)
            {
              if (buffer[i] == '$'/*0x02*/)
              {
                InsStr = "";
              }
              else if (buffer[i] == '#'/*0x03*/)
              {
                queueList.Enqueue(InsStr);
                Log.WriteLog("接收一包数据:\r\n" + InsStr);
              }
              else
              {
                InsStr += buffer[i];
              }
            }
          }
          else
          {
            Thread.Sleep(50);
          }
        }
      }
      catch(Exception ex)
      {

      }
    }

    private void DispatchThread()
    {
      try
      {
        while (!Const.Exit)
        {
          string InsStr = "";
          if (queueList.Count > 0)
          {
            if (queueList.TryDequeue(out InsStr))
            {
              try
              {
                ReceiveParser.OnDataReceive(InsStr);
              }
              catch(Exception)
              {

              }
            }
          }
          else
          {
            Thread.Sleep(50);
          }
        }
      }
      catch(Exception ex)
      {

      }
    }

    public bool Open(string PortName, int nBaudRate)
    {
      if (commPort.IsOpen)
      {
        return true;
      }
      try
      {
        commPort.PortName = PortName;
        commPort.BaudRate = nBaudRate;
        commPort.Open();
        new Thread(ReceiveThread).Start();
        new Thread(DispatchThread).Start();
        //Heart.SendHeart();
        return commPort.IsOpen;
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + "SerialPort.Open Error");
      }
      

      return false;
    }

    public bool IsOpen()
    {
      return commPort.IsOpen;
    }

    public void Close()
    {
      ReceiveParser.Stop();
      if (commPort.IsOpen)
      {
        commPort.Close();
      }
    }
  }
}
