using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UrineAnalyzer
{
  public class ModernWindow : Window
  {
    private Button CloseButton;
    private Button MaxButton;
    public Button Max
    {
      get { return MaxButton; }
    }
    //private Button MinButton;
    //public Button Min
    //{
    //  get { return MinButton; }
    //}
    private TextBlock WindowTitleTbl;

    public ModernWindow()
    {
      this.Activated += ModernWindow_Activated;
    }

    private void ModernWindow_Activated(object sender, EventArgs e)
    {
      // 查找窗体模板
      ControlTemplate metroWindowTemplate = App.Current.Resources["MetroWindowTemplate"] as ControlTemplate;

      if (metroWindowTemplate != null)
      {
        CloseButton = metroWindowTemplate.FindName("CloseWinButton", this) as Button;
        MaxButton = metroWindowTemplate.FindName("MaxWinButton", this) as Button;
        //MinButton = metroWindowTemplate.FindName("MinWinButton", this) as Button;
        CloseButton.Click += CloseButton_Click;
        MaxButton.Click += MaxButton_Click;
        MaxButton.Visibility = Visibility.Hidden;
        //MinButton.Click += MinButton_Click;

        WindowTitleTbl = metroWindowTemplate.FindName("WindowTitleTbl", this) as TextBlock;
      }
    }
    private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      Close();
    }
    private void MaxButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      if (this.WindowState == System.Windows.WindowState.Normal)
      {
        this.WindowState = System.Windows.WindowState.Maximized;
      }
      else
      {
        this.WindowState = System.Windows.WindowState.Normal;
      }
    }
    private void MinButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      this.WindowState = System.Windows.WindowState.Minimized;
    }

    /// <summary>
    /// 实现窗体移动
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      DragMove();

      base.OnMouseLeftButtonDown(e);
    }
  }
}
