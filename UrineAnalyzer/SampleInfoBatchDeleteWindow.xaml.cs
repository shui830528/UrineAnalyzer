using DataModel;
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
  /// SampleInfoBatchDeleteWindow.xaml 的交互逻辑
  /// </summary>
  public partial class SampleInfoBatchDeleteWindow : ModernWindow
  {

    private DBSampleInfo startSampleInfo = new DBSampleInfo();
    private DBSampleInfo endSampleInfo = new DBSampleInfo();

    public DBSampleInfo StartSampleInfo
    {
      get { return startSampleInfo; }
    }

    public DBSampleInfo EndSampleInfo
    {
      get { return endSampleInfo; }
    }

    public SampleInfoBatchDeleteWindow()
    {
      InitializeComponent();

      startSampleInfo.SampleID = 1;
      endSampleInfo.SampleID = 1;

      startSampleInfo.RegistDate = DateTime.Now;
      endSampleInfo.RegistDate = DateTime.Now;

      tbStartSampleNo.DataContext = startSampleInfo;
      tbEndSampleNo.DataContext = endSampleInfo;

      dpStartDate.DataContext = startSampleInfo;
      dpEndDate.DataContext = endSampleInfo;

    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
        if (elementWithFocus != null)
        {
          elementWithFocus.MoveFocus(request);
        }
        e.Handled = true;
      }
      base.OnPreviewKeyDown(e);
    }
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }
  }
}
