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
using System.Windows.Threading;

namespace UrineAnalyzer
{
  /// <summary>
  /// BatchPrintProgressWindow.xaml 的交互逻辑
  /// </summary>
  public partial class BatchPrintProgressWindow : Window
  {
    DispatcherTimer dtimer;
    public BatchPrintProgressWindow()
    {
      InitializeComponent();
    }
    private void dtimer_Tick(object sender, EventArgs e)
    {
      if(progressBar.Value == progressBar.Maximum)
      {
        dtimer.Stop();
        Close();
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      dtimer = new DispatcherTimer();
      dtimer.Interval = TimeSpan.FromSeconds(1);
      dtimer.Tick += dtimer_Tick;
      dtimer.Start();
    }
  }
}
