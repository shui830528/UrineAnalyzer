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

namespace UrineAnalyzer
{
  /// <summary>
  /// HelpWindow.xaml 的交互逻辑
  /// </summary>
  public partial class HelpWindow : ModernWindow
  {
    public HelpWindow()
    {
      InitializeComponent();
    }

    private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.Max.Visibility = Visibility.Visible;
    }

    private void button_Colose_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
