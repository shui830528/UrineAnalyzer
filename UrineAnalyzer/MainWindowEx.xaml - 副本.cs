using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.IO;
using Themes;
using Control;
using System.ComponentModel;
using Tool;
using ViewModel;
using System.Windows.Input;

namespace UrineAnalyzer
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public class MainWindowDataDisplayModel : INotifyPropertyChanged
  {
    private string runState;

    public string RunState
    {
      get { return runState; }
      set { runState = value; OnPropertyChanged("RunState"); }
    }

    private string dateTimeString;
    public string DateTimeString
    {
      get { return dateTimeString; }
      set { dateTimeString = value; OnPropertyChanged("DateTimeString"); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler temp = PropertyChanged;
      if (temp != null)
      {
        temp(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
  public partial class MainWindowEx : Window
  {
    private DataReceive dataReceiveManage = null;
    private delegate void OpenDevice();
    private Thread thread = null;
    private List<ContextMenu> contextMenus = new List<ContextMenu>();
    private Timer dateTimeShowTimer;
    private List<System.Windows.Controls.Control> functionsList = new List<System.Windows.Controls.Control>();
    private string DateFormat = "yyyy-MM-dd";
    private string TimeFormat = "HH:mm:ss";
    private MainWindowDataDisplayModel mainWindowDataDisplayModel = new MainWindowDataDisplayModel();
    public MainWindowEx()
    {
      InitializeComponent();

      label_Status.DataContext = mainWindowDataDisplayModel;
      label_DateTime.DataContext = mainWindowDataDisplayModel;
      dateTimeShowTimer = new Timer(ShowCurTime,null,0,1000);
    }

    private void ShowCurTime(object state)
    {
      mainWindowDataDisplayModel.DateTimeString = DateTime.Now.ToString(DateFormat) + " " + DateTime.Now.ToString(TimeFormat);
    }
    private void button_Quit_Click(object sender, RoutedEventArgs e)
    {
      if (MessageBoxEx.ShowEx(this, "是否退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        Const.Exit = true;
        Close();
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (dataReceiveManage != null)
      {
        dataReceiveManage.Close();
      }
      dateTimeShowTimer.Dispose();
    }
    //弹出菜单的名称采用对应的按钮名称+“_ContextMenu”来命名，这样便于统一化处理
    private void button_LoadPage(object sender, RoutedEventArgs e)
    {
      Button button = sender as Button;
      if (button != null)
      {
        ContextMenu contextMenu = GetContextMenu(button.Name + "_ContextMenu");
        if (contextMenu != null)
        {
          contextMenu.PlacementTarget = button;
          Point RelativePos = Mouse.GetPosition(e.Source as FrameworkElement); //鼠标点在控件内的相对位置
          Point AbsolutePos = Mouse.GetPosition(null); //鼠标点在屏幕内的绝对位置
          contextMenu.HorizontalOffset = AbsolutePos.X - RelativePos.X + 155;
          contextMenu.VerticalOffset = AbsolutePos.Y - RelativePos.Y;
          contextMenu.Placement = PlacementMode.Absolute;
          contextMenu.IsOpen = true;
        }
        string interfaceFile = button.Tag == null ? string.Empty : button.Tag as string;
        if (interfaceFile != string.Empty)
        {
          ModernWindow modernWindow = null;
          Page page = null;
          string userInterfaceType = string.Empty;
          App.GetUserInterface(interfaceFile, ref modernWindow, ref page, ref userInterfaceType);
          if (userInterfaceType == App.cPage && page != null)
          {
            frame_LoadPage.Navigate(page);
          }
          else if(userInterfaceType == App.cWindow && modernWindow != null)
          {
            modernWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            modernWindow.Owner = this;
            modernWindow.ShowDialog();
          }
          else
          {
            MessageBoxEx.ShowEx(this, interfaceFile+"文件无效！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
          }
        }
      }
      else
      {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem != null)
        {
          string interfaceFile = menuItem.Tag == null ? string.Empty : menuItem.Tag as string;
          if (interfaceFile != string.Empty)
          {
            ModernWindow modernWindow = null;
            Page page = null;
            string userInterfaceType = string.Empty;
            App.GetUserInterface(interfaceFile, ref modernWindow, ref page, ref userInterfaceType);
            if (userInterfaceType == App.cPage && page != null)
            {
              frame_LoadPage.Navigate(page);
            }
            else if(userInterfaceType == App.cWindow && modernWindow != null)
            {
              modernWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
              modernWindow.Owner = this;
              modernWindow.ShowDialog();
            }
            else
            {
              MessageBoxEx.ShowEx(this, interfaceFile+"文件无效！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
          }
        }
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      string errorInfo = string.Empty;
      label_Version.Content = Const.Version;
      label_UserName.Content = Const.user;
      //SetFunctions();
      thread = new Thread(new ThreadStart(OpenDeviceandStartThread));
      thread.Start();
      button_LoadPage(button_Sample, null);
    }

    private ContextMenu GetContextMenu(string contextMenuName)
    {
      ContextMenu contextMenu = null;
      foreach (var m in contextMenus)
      {
        if (m != null && m.Name == contextMenuName)
        {
          contextMenu = m;
          break;
        }
      }
      return contextMenu;
    }
    private void button_Initialized(object sender, EventArgs e)
    {
      Button button = sender as Button;
      if (button != null && button.ContextMenu != null)
      {
        contextMenus.Add(button.ContextMenu);
        button.ContextMenu = null;
      }
    }
    private void button_Help_Click(object sender, RoutedEventArgs e)
    {
      HelpWindow helpWindow = new HelpWindow();
      helpWindow.Owner = this;
      helpWindow.ShowDialog();
    }
    private void GetFunctions()
    {
      functionsList.Clear();
      functionsList.Add(button_Sample);
      functionsList.Add(button_QC);
      functionsList.Add(button_Set);
      functionsList.Add(menuItem_Com);
      functionsList.Add(menuItem_SedimentItem);
      functionsList.Add(menuItem_UrineAnalyzerItem);
      functionsList.Add(menuItem_Param);
      functionsList.Add(menuItem_LIS);
      functionsList.Add(menuItem_Print);
      functionsList.Add(button_Log);
      functionsList.Add(button_User);
      functionsList.Add(menuItem_User);
      functionsList.Add(menuItem_PWD);
    }

    private void SetFunctions()
    {
      //GetFunctions();
      //foreach (DataItem pdi in App.User.PrivilegeObject.Items)
      //{
      //  if (pdi.EName != string.Empty)
      //  {
      //    (functionsList.Single<System.Windows.Controls.Control>(f => f.Name == pdi.EName) as Button).IsEnabled = pdi.IsSelected;
      //  }
      //  foreach (DataItem di in pdi.ChildDataItems)
      //  {
      //    System.Windows.Controls.Control mc = functionsList.Single<System.Windows.Controls.Control>(f => f.Name == di.EName);
      //    mc.IsEnabled = di.IsSelected;
      //    if (di.IsSelected && !isDefaultPage)
      //    {
      //      button_LoadPage(mc, null);
      //      isDefaultPage = true;
      //    }
      //  }
      //}
    }

    private void button_ChangeUser_Click(object sender, RoutedEventArgs e)
    {
      LoginPage loginWindow = new LoginPage();
      if (loginWindow.ShowDialog() == true)
      {
        Const.user = loginWindow.cbbUserName.Text;
        label_UserName.Content = Const.user;
        button_LoadPage(button_Sample, null);
      }
    }
    private void OpenDeviceandStartThread()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
      {
        this.Dispatcher.Invoke(new OpenDevice(this.OpenDeviceandStartThread));
      }
      else
      {
        LinkParamViewModel linkParamModel = new LinkParamViewModel();
        linkParamModel.Update();
        Heart.HeartState = label_Status;
        Snapshot.VideoState = label_Video;
        dataReceiveManage = new DataReceive();
        label_Com.Content = linkParamModel.LinkParam.PortName;
        if (!dataReceiveManage.Open(linkParamModel.LinkParam.PortName, linkParamModel.LinkParam.BaudRate))
        {
          MessageBoxEx.ShowEx(this, "串口打开失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }
  }
}
