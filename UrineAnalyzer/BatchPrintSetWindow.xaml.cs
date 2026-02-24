using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
  /// BatchPrintSetWindow.xaml 的交互逻辑
  /// </summary>
  public partial class BatchPrintSetWindow : ModernWindow
  {
    private int beginNo;
    public int BeginNo
    {
      get { return beginNo; }
    }
    private int endNo;
    public int EndNo
    {
      get { return endNo; }
    }
    private DateTime testDate;
    public DateTime TestDate
    {
      get { return testDate; }
    }
    private bool isTest;
    public bool IsTest
    {
      get { return isTest; }
    }
    private bool isAuditor;
    public bool IsAuditor
    {
      get { return isAuditor; }
    }
    public BatchPrintSetWindow()
    {
      InitializeComponent();
    }

    private void button_OK_Click(object sender, RoutedEventArgs e)
    {
      if(textBox_BeginNo.Text == "")
      {
        MessageBoxEx.ShowEx(this,"起始样本编号不能为空！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
        return;
      }
      if(textBox_EndNo.Text == "")
      {
        MessageBoxEx.ShowEx(this, "结束样本编号不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      if(datePicker_TestDate.SelectedDate == null)
      {
        MessageBoxEx.ShowEx(this, "请选择测试日期！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      if(Convert.ToInt32(textBox_BeginNo.Text) > Convert.ToInt32(textBox_EndNo.Text))
      {
        MessageBoxEx.ShowEx(this, "起始样本编号不能大于结束样本编号！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      beginNo = Convert.ToInt32(textBox_BeginNo.Text);
      endNo = Convert.ToInt32(textBox_EndNo.Text);
      testDate = (DateTime)datePicker_TestDate.SelectedDate;
      isTest = (bool)checkBox_Test.IsChecked;
      isAuditor = (bool)checkBox_Auditor.IsChecked;
      DialogResult = true;
    }

    private void button_Cancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void textBox_BeginNo_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      Regex re = new Regex("[^0-9]+");
      e.Handled = re.IsMatch(e.Text);
    }
  }
}
