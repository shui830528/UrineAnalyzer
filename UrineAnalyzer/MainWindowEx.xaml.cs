using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.IO;
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
      set { 
        runState = value; 
        OnPropertyChanged("RunState"); }
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
    private Dictionary<string, Button> moduleList = new Dictionary<string, Button>();
    private Dictionary<string, Dictionary<string, Button>> childModuleList = new Dictionary<string, Dictionary<string, Button>>();
    private Dictionary<string, ToolBar> childModulePanelList = new Dictionary<string, ToolBar>();
    private Dictionary<string, double> initWidths = new Dictionary<string, double>();
    private bool isDefaultPage;

    private DataReceive dataReceiveManage = null;
    private delegate void OpenDevice();
    private Thread thread = null;
    private Timer dateTimeShowTimer;
    private List<System.Windows.Controls.Control> functionsList = new List<System.Windows.Controls.Control>();
    private MainWindowDataDisplayModel mainWindowDataDisplayModel = new MainWindowDataDisplayModel();
    public MainWindowEx()
    {
      InitializeComponent();

      label_Com.DataContext = mainWindowDataDisplayModel;
      label_DateTime.DataContext = mainWindowDataDisplayModel;
      dateTimeShowTimer = new Timer(ShowCurTime,null,0,1000);

      InitModuleList();
      InitChildModuleList();
      InitChildModulePanelList();
    }
    private void InitModuleList()
    {
      moduleList.Add(button_Sample.Name, button_Sample);
      moduleList.Add(button_QC.Name, button_QC);
      moduleList.Add(button_Set.Name, button_Set);
      moduleList.Add(button_Log.Name, button_Log);
      moduleList.Add(button_User.Name, button_User);
    }
    private void InitChildModuleList()
    {
      childModuleList.Add(button_Sample.Name, new Dictionary<string, Button>());
      childModuleList[button_Sample.Name].Add(button_Sample.Name, button_Sample);
      childModuleList.Add(button_QC.Name, new Dictionary<string, Button>());
      childModuleList[button_QC.Name].Add(button_QC.Name, button_QC);
      childModuleList.Add(button_Set.Name, new Dictionary<string, Button>());
      childModuleList[button_Set.Name].Add(menuItem_Com.Name, menuItem_Com);
      childModuleList[button_Set.Name].Add(menuItem_Param.Name, menuItem_Param);
      childModuleList[button_Set.Name].Add(menuItem_LIS.Name, menuItem_LIS);
      childModuleList[button_Set.Name].Add(menuItem_Print.Name, menuItem_Print);
      childModuleList[button_Set.Name].Add(menuItem_SedimentItem.Name, menuItem_SedimentItem);
      childModuleList[button_Set.Name].Add(menuItem_UrineAnalyzerItem.Name, menuItem_UrineAnalyzerItem);
      childModuleList.Add(button_User.Name, new Dictionary<string, Button>());
      childModuleList[button_User.Name].Add(button_User.Name, button_User);
    }
    private void InitChildModulePanelList()
    {
      childModulePanelList.Add(stackPanel_Set.Name, stackPanel_Set);
      childModulePanelList.Add(stackPanel_User.Name, stackPanel_User);
    }

    private void ShowCurTime(object state)
    {
      mainWindowDataDisplayModel.DateTimeString = DateTime.Now.ToString(Const.DateTimeFormat);
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

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      string errorInfo = string.Empty;
      label_Version.Content = Const.Version;
      label_UserName.Content = Const.user;
      //SetFunctions();
      thread = new Thread(new ThreadStart(OpenDeviceandStartThread));
      thread.Start();
      module_Click(button_Sample, null);

      this.Height = SystemParameters.WorkArea.Height;
      this.Width = SystemParameters.WorkArea.Width;
      Left = 0.0;
      Top = 0.0;
    }

    private void button_Help_Click(object sender, RoutedEventArgs e)
    {
      //System.Diagnostics.Process.Start("help.chm");
      HelpWindow helpWindow = new HelpWindow();
      helpWindow.Owner = this;
      helpWindow.ShowDialog();
    }
    private void GetInitWidths()
    {
      initWidths.Clear();
      initWidths.Add(button_Sample.Name, button_Sample.Width);
      initWidths.Add(button_QC.Name, button_QC.Width);
      initWidths.Add(button_Set.Name, button_Set.Width);
      initWidths.Add(menuItem_Com.Name, menuItem_Com.Width);
      initWidths.Add(menuItem_Param.Name, menuItem_Param.Width);
      initWidths.Add(menuItem_LIS.Name, menuItem_LIS.Width);
      initWidths.Add(menuItem_SedimentItem.Name, menuItem_SedimentItem.Width);
      initWidths.Add(menuItem_UrineAnalyzerItem.Name, menuItem_UrineAnalyzerItem.Width);
      initWidths.Add(menuItem_Print.Name, menuItem_Print.Width);
      initWidths.Add(button_User.Name, button_User.Width);
      initWidths.Add(menuItem_User.Name, menuItem_User.Width);
      initWidths.Add(menuItem_PWD.Name, menuItem_PWD.Width);
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

    //private void SetFunction(DataItem di)
    //{
    //  if (di.EName != string.Empty)
    //  {
    //    System.Windows.Controls.Control mc = functionsList.Single<System.Windows.Controls.Control>(f => f.Name == di.EName);
    //    if (mc != null)
    //    {
    //      //mc.IsEnabled = di.IsSelected;
    //      mc.Visibility = di.IsSelected ? Visibility.Visible : Visibility.Hidden;
    //      mc.Width = di.IsSelected ? initWidths[mc.Name] : 0;
    //      if (di.IsSelected && di.childDataItems.Count == 0)
    //      {
    //        if (!isDefaultPage)
    //        {
    //          //button_LoadPage(mc, null);
    //          string moduleName = GetModuleName(mc as Button);
    //          GetModuleSelected(moduleName);
    //          LoadPage(mc);
    //          isDefaultPage = true;
    //        }
    //      }
    //    }
    //  }
    //  foreach (DataItem cdi in di.ChildDataItems)
    //  {
    //    if (cdi.IsSelected && !isDefaultPage)
    //    {
    //      System.Windows.Controls.Control mc = functionsList.Single<System.Windows.Controls.Control>(f => f.Name == cdi.EName);
    //      if (mc != null)
    //      {
    //        //button_LoadPage(mc, null);
    //        string moduleName = GetModuleName(mc as Button);
    //        GetModuleSelected(moduleName);
    //        LoadPage(mc);
    //        isDefaultPage = true;
    //      }
    //    }
    //    SetFunction(cdi);
    //  }
    //}
    //private void SetFunctions()
    //{
    //  GetFunctions();
    //  foreach (DataItem pdi in App.User.PrivilegeObject.Items)
    //  {
    //    SetFunction(pdi);
    //  }
    //}
    private void SetModuleSelected(Button button)
    {
      foreach (var b in moduleList)
      {
        if (b.Key == button.Name)
        {
          b.Value.Style = (Style)this.FindResource("ButtonStyle_SelectedModule");
          if (b.Value.Tag == null || Convert.ToString(b.Value.Tag) == string.Empty)
          {
            frame_LoadPage.Content = null;
            SetModuleToolBarVisible("stackPanel" + button.Name.Substring(button.Name.IndexOf("_")));
          }
          else
          {
            LoadPage(b.Value);
            SetModuleToolBarVisible("stackPanel" + button.Name.Substring(button.Name.IndexOf("_")));
          }
        }
        else
        {
          b.Value.Style = (Style)this.FindResource("ButtonStyle_UnSelectedModule");
        }
      }
    }
    private void GetModuleSelected(string moduleName)
    {
      foreach (var b in moduleList)
      {
        if (b.Key == moduleName)
        {
          b.Value.Style = (Style)this.FindResource("ButtonStyle_SelectedModule");
          SetModuleToolBarVisible("stackPanel" + b.Value.Name.Substring(b.Value.Name.IndexOf("_")));
        }
        else
        {
          b.Value.Style = (Style)this.FindResource("ButtonStyle_UnSelectedModule");
        }
      }
    }
    private void SetModuleToolBarVisible(string toolBarName)
    {
      if (childModulePanelList.ContainsKey(toolBarName))
      {
        toolBar.Height = new GridLength(40);
        foreach (var t in childModulePanelList)
        {
          if (t.Key == toolBarName)
          {
            t.Value.Visibility = Visibility.Visible;
          }
          else
          {
            t.Value.Visibility = Visibility.Hidden;
          }
        }
      }
      else
      {
        toolBar.Height = new GridLength(0);
      }
    }
    //private void SetChildModuleSelected(Button button)
    //{
    //  foreach (var m in childModuleList)
    //  {
    //    foreach (var b in m.Value)
    //    {
    //      if (b.Key == button.Name)
    //      {
    //        b.Value.Style = (Style)this.FindResource("ButtonStyle_SelectedChildModule");
    //      }
    //      else
    //      {
    //        b.Value.Style = (Style)this.FindResource("ButtonStyle_UnSelectedChildModule");
    //      }
    //    }
    //  }
    //}
    private string GetModuleName(Button button)
    {
      string moduleName = string.Empty;
      foreach (var m in childModuleList)
      {
        foreach (var b in m.Value)
        {
          if (b.Key == button.Name)
          {
            return m.Key;
          }
        }
      }
      return moduleName;
    }
    private void LoadPage(object sender)
    {
      Button button = sender as Button;
      if (button != null)
      {
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
          else if (userInterfaceType == App.cWindow && modernWindow != null)
          {
            modernWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            modernWindow.Owner = this;
            modernWindow.ShowDialog();
          }
          else
          {
            MessageBoxEx.ShowEx(this, interfaceFile + "文件无效！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
          }
        }
      }
    }
    private void module_Click(object sender, RoutedEventArgs e)
    {
      SetModuleSelected(sender as Button);
    }

    private void childModule_Click(object sender, RoutedEventArgs e)
    {
      LoadPage(sender);
    }
    private void button_ChangeUser_Click(object sender, RoutedEventArgs e)
    {
      LoginPage loginWindow = new LoginPage();
      if (loginWindow.ShowDialog() == true)
      {
        Const.user = loginWindow.cbbUserName.Text;
        label_UserName.Content = Const.user;
        module_Click(button_Sample, null);
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
        Heart.HeartState = label_Com;
        Snapshot.VideoState = label_Video;
        dataReceiveManage = new DataReceive();
        if (!dataReceiveManage.Open(linkParamModel.LinkParam.PortName, linkParamModel.LinkParam.BaudRate))
        {
          //MessageBoxEx.ShowEx(this, "串口打开失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
          mainWindowDataDisplayModel.RunState = "尿仪连接失败";
        }
        else
        {
          mainWindowDataDisplayModel.RunState = "尿仪连接成功";
        }
      }
    }
  }
}
