using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Tool;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// App.xaml 的交互逻辑
  /// </summary>
  public partial class App : Application
  {
    public static Dictionary<string, object> UserInterfaces = new Dictionary<string, object>();
    public static List<string> UserInterfaceFileList = new List<string>();
    public static string cWindow = "-w";
    public static string cPage = "-p";
    public static string DBConnectionParamName = "conn";
    public static string DBConnectionString;
    
    public App()
    {
      Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
      Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
      Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "hh:MM:ss";
      Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
      Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "hh:MM:ss";

      Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
    }
    //DataGrid中复制数据时有时会引发异常，数据复制失败，加此异常处理后数据复制成功
    void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      var comException = e.Exception as System.Runtime.InteropServices.COMException;
      if (comException != null && comException.ErrorCode == -2147221040)
        e.Handled = true;
    }
    private void CreateUserInterfaceFileList()
    {
      UserInterfaceFileList.Add("SamplePage.xaml"+cPage);
      UserInterfaceFileList.Add("QCPage.xaml" + cPage);
      UserInterfaceFileList.Add("LinkParamPage.xaml" + cPage);
      UserInterfaceFileList.Add("BaseParamPage.xaml" + cPage);
      UserInterfaceFileList.Add("ResultSettingPage.xaml" + cPage);
      UserInterfaceFileList.Add("HospInfoSettingPage.xaml" + cPage);
      UserInterfaceFileList.Add("InputSettingPage.xaml" + cPage);
      UserInterfaceFileList.Add("PrintSettingPage.xaml" + cPage);
      UserInterfaceFileList.Add("LogSettingPage.xaml" + cPage);
      //UserInterfaceFileList.Add("AuthoritySettingPage.xaml"+cPage);
      UserInterfaceFileList.Add("UserSettingPage.xaml" + cPage);
      UserInterfaceFileList.Add("PasswordSettingPage.xaml" + cPage);
    }
    public static void GetUserInterface(string fileName, ref ModernWindow userInterfaceW, ref Page userInterfaceP, ref string userInterfaceT)
    {
      foreach (var u in App.UserInterfaces)
      {
        string userInterfaceName = u.Key.Substring(0, u.Key.Length - 2);
        string userInterfaceType = u.Key.Substring(u.Key.Length - 2).Trim();
        if (userInterfaceName == fileName)
        {
          userInterfaceT = userInterfaceType;
          if (userInterfaceType == cPage)
          {
            userInterfaceP = (Page)(u.Value);
            userInterfaceW = null;
          }
          else
          {
            userInterfaceP = null;
            userInterfaceW = (ModernWindow)(u.Value);
          }
          break;
        }
      }
    }
    protected override void OnStartup(StartupEventArgs e)
    {
      UIExecute.Initialize();
      LoginPage loginWindow = new LoginPage();
      if (loginWindow.ShowDialog() == true)
      {
        CreateUserInterfaceFileList();
        ProgressPage progressWindow = new ProgressPage();
        progressWindow.ShowDialog();
        base.OnStartup(e);
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
      }
      else
      {
        this.Shutdown();
      }
    }
  }
}
