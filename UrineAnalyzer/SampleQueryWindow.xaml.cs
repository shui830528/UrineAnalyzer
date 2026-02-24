using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// SampleQueryWindow.xaml 的交互逻辑
  /// </summary>
  public partial class SampleQueryWindow : Window
  {
    private string otherCondition = "";
    private HospInfoViewModel hospInfoViewModel = new HospInfoViewModel();
    private Dictionary<int, Dictionary<string, string>> types = new Dictionary<int, Dictionary<string, string>>();
    public string OtherCondition
    {
      get { return otherCondition; }
    }

    private DateTime? beginDate = null;
    public DateTime? BeginDate
    {
      get { return beginDate; }
    }

    private DateTime? endDate = null;
    public DateTime? EndDate
    {
      get { return endDate; }
    }
    public SampleQueryWindow()
    {
      InitializeComponent();
    }

    private void LoadTypeItems()
    {
      HospInfoModel hospInfoModel = new HospInfoModel();
      hospInfoModel.LoadTypes(types);

      DBHospInfoItemType AgeUnititem = new DBHospInfoItemType();
      AgeUnititem.ItemID = HOSPINFOITEMS.HI_AGEUNIT_TYPE;
      cbAgeUnit.Tag = HOSPINFOITEMS.HI_AGEUNIT_TYPE;
      hospInfoViewModel.LoadTypeItem(AgeUnititem);
      cbAgeUnit.ItemsSource = AgeUnititem.Items;
      cbAgeUnit.DisplayMemberPath = "Caption";
      cbAgeUnit.SelectedValuePath = "Caption";

      DBHospInfoItemType Sexitem = new DBHospInfoItemType();
      Sexitem.ItemID = HOSPINFOITEMS.HI_PATIENT_SEX;
      cbSex.Tag = HOSPINFOITEMS.HI_PATIENT_SEX;
      hospInfoViewModel.LoadTypeItem(Sexitem);
      cbSex.ItemsSource = Sexitem.Items;
      cbSex.DisplayMemberPath = "Caption";
      cbSex.SelectedValuePath = "Caption";

      DBHospInfoItemType PatientTypeitem = new DBHospInfoItemType();
      PatientTypeitem.ItemID = HOSPINFOITEMS.HI_PATIEN_TTYPE;
      cbPatientType.Tag = HOSPINFOITEMS.HI_PATIEN_TTYPE;
      hospInfoViewModel.LoadTypeItem(PatientTypeitem);
      cbPatientType.ItemsSource = PatientTypeitem.Items;
      cbPatientType.DisplayMemberPath = "Caption";
      cbPatientType.SelectedValuePath = "Caption";

      DBHospInfoItemType SendCheckOfficeitem = new DBHospInfoItemType();
      SendCheckOfficeitem.ItemID = HOSPINFOITEMS.HI_SENDCHECK_OFFICE;
      cbSendCheckOffice.Tag = HOSPINFOITEMS.HI_SENDCHECK_OFFICE;
      hospInfoViewModel.LoadTypeItem(SendCheckOfficeitem);
      cbSendCheckOffice.ItemsSource = SendCheckOfficeitem.Items;
      cbSendCheckOffice.DisplayMemberPath = "Caption";
      cbSendCheckOffice.SelectedValuePath = "Caption";

      DBHospInfoItemType SendCheckDoctoritem = new DBHospInfoItemType();
      SendCheckDoctoritem.ItemID = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
      cbSendCheckDoctor.Tag = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
      hospInfoViewModel.LoadTypeItem(SendCheckDoctoritem);
      cbSendCheckDoctor.ItemsSource = SendCheckDoctoritem.Items;
      cbSendCheckDoctor.DisplayMemberPath = "Caption";
      cbSendCheckDoctor.SelectedValuePath = "Caption";

      cbAgeOperate.Items.Clear();
      cbAgeOperate.Items.Add("=");
      cbAgeOperate.Items.Add(">");
      cbAgeOperate.Items.Add(">=");
      cbAgeOperate.Items.Add("<");
      cbAgeOperate.Items.Add("<=");
    }
    private string GetSampleInfoCondition()
    {
      string sampleInfoCondition = "";
      if (tbSampleID.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and SampleID = " + tbSampleID.Text;
      }
      if (tbBedNo.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and BedNo = '" + tbBedNo.Text + "'";
      }
      if (tbBarCode.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and BarCode = '" + tbBarCode.Text + "'";
      }
      if (cbPatientType.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and PatientType = '" + cbPatientType.Text + "'";
      }
      if (tbName.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and PatientName = '" + tbName.Text + "'";
      }
      if (cbSendCheckOffice.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and SendCheckOffice = '" + cbSendCheckOffice.Text + "'";
      }
      if (cbAgeOperate.Text != "" && tbAge.Text != "" && cbAgeUnit.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and (PatientAge " + cbAgeOperate.Text + tbAge.Text + " and PatientAgeUnit = '" + cbAgeUnit.Text + "')";
      }
      if (cbSendCheckDoctor.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and SendCheckDoctor = '" + cbSendCheckDoctor.Text + "'";
      }
      if (cbSex.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and PatientSex = '" + cbSex.Text + "'";
      }
      if (tbCheckDoctor.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and CheckDoctor = '" + tbCheckDoctor.Text + "'";
      }
      if (tbCaseNo.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and CaseNo = '" + tbCaseNo.Text + "'";
      }
      if (tbAuditor.Text != "")
      {
        sampleInfoCondition = sampleInfoCondition + " and Auditor = '" + tbAuditor.Text + "'";
      }
      if (sampleInfoCondition == "")
      {
        return sampleInfoCondition = "1=1";
      }
      else
      {
        return sampleInfoCondition.Trim().Substring(3);
      }
    }
    protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
      ComboBox cb = e.Source as ComboBox;
      if (cb != null)
      {
        int typeID = -1;
        try
        {
          if (cb.Tag != null)
          {
            typeID = Convert.ToInt32(cb.Tag);
          }
        }
        catch
        {

        }
        if (typeID != -1)
        {
          if (types.ContainsKey(typeID) && cb.Text != null && types[typeID].ContainsKey(cb.Text))
          {
            cb.Text = types[typeID][cb.Text];
          }
          else if (types.ContainsKey(typeID) && cb.Text != null && types[typeID].Values.Where(tn => tn == cb.Text).Count() == 0)
          {
            cb.Text = string.Empty;
          }
          else
          {
            ;
          }
        }
      }
      TextBox tb = e.Source as TextBox;
      if (tb != null)
      {
        int typeID = -1;
        try
        {
          if (tb.Tag != null)
          {
            typeID = Convert.ToInt32(tb.Tag);
          }
        }
        catch
        {

        }
        if (typeID != -1)
        {
          if (types.ContainsKey(typeID) && tb.Text != null && types[typeID].ContainsKey(tb.Text))
          {
            tb.Text = types[typeID][tb.Text];
          }
          else if (types.ContainsKey(typeID) && tb.Text != null && types[typeID].Values.Where(tn => tn == tb.Text).Count() == 0)
          {
            tb.Text = string.Empty;
          }
          else
          {
            ;
          }
        }
      }
      base.OnPreviewLostKeyboardFocus(e);
    }
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        Button bt = e.Source as Button;
        if (bt != null && bt.IsDefault == true)
        {
          e.Handled = false;
        }
        else
        {
          TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
          UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
          if (elementWithFocus != null)
          {
            elementWithFocus.MoveFocus(request);
          }
          e.Handled = true;
        }
      }
      base.OnPreviewKeyDown(e);
    }

    private string GetSampleRangeCondition()
    {
      string sampleRangeCondition = "";
      if (rb_All.IsChecked == true)
      {
        sampleRangeCondition = "";
      }
      else if (rb_unTest.IsChecked == true)
      {
        sampleRangeCondition = "IsTest = 0";
      }
      else if (rb_Test.IsChecked == true)
      {
        sampleRangeCondition = "IsTest = 1";
      }
      else if (rb_Auditor.IsChecked == true)
      {
        sampleRangeCondition = "IsAuditor = 1";
      }
      else
      {
        sampleRangeCondition = "ImportDate = RegistDate";
      }
      return sampleRangeCondition;
    }
    private string GetOtherCondition()
    {
      string strSampleInfoCondition = GetSampleInfoCondition();
      string strSampleRangeCondition = GetSampleRangeCondition();
      string strCondition = "";
      if (strSampleInfoCondition != "")
      {
        strCondition = strCondition + " and (" + strSampleInfoCondition + ") ";
      }
      if (strSampleRangeCondition != "")
      {
        strCondition = strCondition + " and (" + strSampleRangeCondition + ")";
      }
      if (strCondition == "")
      {
        return strCondition = "1=1";
      }
      else
      {
        return strCondition.Trim().Substring(3);
      }
    }
    private void btnQuery_Click(object sender, RoutedEventArgs e)
    {
      otherCondition = GetOtherCondition();
      beginDate = datePicker_BeginDate.SelectedDate;
      endDate = datePicker_EndDate.SelectedDate;
      DialogResult = true;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      datePicker_BeginDate.SelectedDate = DateTime.Now;
      datePicker_EndDate.SelectedDate = DateTime.Now;
      LoadTypeItems();
    }

    private void tbAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      Regex re = new Regex("[^0-9]+");
      e.Handled = re.IsMatch(e.Text);
    }

    private void Border_MouseMove(object sender, MouseEventArgs e)
    {
      if (Mouse.LeftButton == MouseButtonState.Pressed)
      {
        Window window = (Window)this;
        window.DragMove();
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }
  }
}
