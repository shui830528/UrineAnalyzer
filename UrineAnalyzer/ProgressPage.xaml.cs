using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.ObjectModel;
using ViewModel;
using DataModel;
using Tool;
using System.ComponentModel;
using System.Threading;

namespace UrineAnalyzer
{
  /// <summary>
  /// LoginPage.xaml 的交互逻辑
  /// </summary>
  /// 
  public partial class ProgressPage : Window
  {

    private void CreateUserInterfaces()
    {
      for (int i = 0; i < App.UserInterfaceFileList.Count; i++)
      {
        this.Dispatcher.Invoke(
          new Action(() =>
          {
            string userInterface = App.UserInterfaceFileList[i];
            Uri uri = new Uri(userInterface.Substring(0, userInterface.Length - 2), UriKind.Relative);
            try
            {
              App.UserInterfaces.Add(userInterface, Application.LoadComponent(uri));
            }
            catch (Exception ex)
            {
              Log.WriteLog("文件无效：" + ex.Message);
            }
            progress.Value = i + 1;
          }));
        Thread.Sleep(50);
      }
      this.Dispatcher.Invoke(new Action(() => { Close(); }));
    }
    public ProgressPage()
    {
      InitializeComponent();
      progress.Maximum = App.UserInterfaceFileList.Count;
      progress.Minimum = 0;
      progress.Value = 0;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      new Thread(new ThreadStart(CreateUserInterfaces)).Start();
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
      //if (Mouse.LeftButton == MouseButtonState.Pressed)
      //{
      //  Window window = (Window)this;
      //  window.DragMove();
      //}
    }
  }
}
