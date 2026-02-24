using Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Themes;
using Tool;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window,IThemeUpdate
  {
    private DataReceive dataReceiveManage = null;
    private delegate void OpenDevice();
    private Thread thread = null;
    public MainWindow()
    {
      InitializeComponent();

      Skin.SkinManage.AddUpdateEvent(this);
      Skin.SkinManage.UpdateTheme(this);
    }

    public void ThemeUpdateHander(SkinInfo obj)
    {
      MainTitle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(obj.TitleColor));
    }

    private void btnExit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      Const.Exit = true;
      Close();
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
        Heart.HeartState = textBlock_State;
        Snapshot.VideoState = textBlock_Video;
        dataReceiveManage = new DataReceive();
        textBlock_Com.Text = "端口：" + linkParamModel.LinkParam.PortName;
        if (!dataReceiveManage.Open(linkParamModel.LinkParam.PortName, linkParamModel.LinkParam.BaudRate))
        {
          MessageBox.Show(this, "串口打开失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      textBlock_Administrator.Text = Const.user;
      textBlock_User.Text = Const.authority;
      thread = new Thread(new ThreadStart(OpenDeviceandStartThread));
      thread.Start();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (dataReceiveManage != null)
      {
        dataReceiveManage.Close();
      }
    }
  }
}
