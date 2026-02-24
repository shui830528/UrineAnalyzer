using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tool;

namespace Control
{
  public class Heart
  {
    private Thread thread = null;
    private AutoResetEvent heartEvent = new AutoResetEvent(false);
    public AutoResetEvent HeartEvent { get { return heartEvent; } }
    private AutoResetEvent exitEvent = new AutoResetEvent(false);
    private AutoResetEvent[] events;
    private int iTimeOut = 20000;
    private static Label heartState = null;
    public static Label HeartState { get { return heartState; } set { heartState = value; } }
    private delegate void DisplayHeartStateInfo(string stateInfo);
    public Heart()
    {
      events = new AutoResetEvent[2];
      events[0] = heartEvent;
      events[1] = exitEvent;
    }
    public static void SendHeart()
    {
      if (DataReceive.commPort.IsOpen)
      {
        DataReceive.commPort.Write(Const.OnLine, 0, Const.OnLine.Length);
      }
    }
    private void SetHeartStateInfo(string stateInfo)
    {
      if (heartState != null)
      {
        if (heartState.Dispatcher.Thread != Thread.CurrentThread)
        {
          heartState.Dispatcher.Invoke(new DisplayHeartStateInfo(this.SetHeartStateInfo), stateInfo);
        }
        else
        {
          heartState.Content = stateInfo;
        }
      }
    }
    private void OnHeart()
    {
      while(!Const.Exit)
      {
        if (WaitHandle.WaitAny(events, iTimeOut) == WaitHandle.WaitTimeout)
        {
          SetHeartStateInfo("未连接尿液分析仪");
          SendHeart();
        }
        else
        {
          SetHeartStateInfo("已连接尿液分析仪");
        }
      }
    }
    public void Start()
    {
      thread = new Thread(new ThreadStart(OnHeart));
      thread.Start();
    }
    public void Stop()
    {
      exitEvent.Set();
    }
  }
}
