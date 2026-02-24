using DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using Tool;
using ViewModel;

namespace UrineAnalyzer
{
    /// <summary>
    /// SamplePage.xaml 的交互逻辑
    /// </summary>
    public partial class SamplePage : Page
    {
        private ReportPrintViewModel reportPrintViewModel;
        private SampleInfoViewModel sampleInfoViewModel = new SampleInfoViewModel();
        private SampleResultViewModel sampleResultViewModel = new SampleResultViewModel();
        private HospInfoViewModel hospInfoViewModel = new HospInfoViewModel();
        private DataSet batchSampleDataSet = null;
        private Dictionary<int, Dictionary<string, string>> types = new Dictionary<int, Dictionary<string, string>>();
        //private Timer refreshDataTimer;

        private DataUpdate dataUpdate = new DataUpdate("SampleManage");

        public SamplePage()
        {
            InitializeComponent();
            reportPrintViewModel = new ReportPrintViewModel(App.DBConnectionParamName, App.DBConnectionString);

            dataUpdate.SetAction((param01, param02) =>
            {

                //Task.Run(()=> { });
                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    UIExecute.RunAsync(() =>
                    {
                        RefreshData(null);
                    });
                }).Start();

            });
        }
        private void RefreshData(object state)
        {
            if (!sampleInfoViewModel.IsSampleInfoEdit)
            {
                sampleInfoViewModel.UpdateSampleInfoList(DateTime.Now, DateTime.Now, string.Empty);
            }

        }
        private void EditSampleButtonEnable(bool bEnable)
        {
            btnUp.IsEnabled = bEnable;
            btnDown.IsEnabled = bEnable;
            btnAdd.IsEnabled = bEnable;
            btnModify.IsEnabled = bEnable;
            btnDelete.IsEnabled = bEnable;
            btnBatchDelete.IsEnabled = bEnable;
            btnAuditor.IsEnabled = bEnable;
            btnUnAuditor.IsEnabled = bEnable;
            btnPrint.IsEnabled = bEnable;
            btnQuery.IsEnabled = bEnable;
        }


        /// <summary>
        /// 编辑可用
        /// </summary>
        /// <param name="IsEnabled"></param>
        private void EnableEdit(bool IsEnabled)
        {
            tbSampleID.IsEnabled = IsEnabled;
            cbEmergency.IsEnabled = IsEnabled;
            dpCheckDate.IsEnabled = IsEnabled;
            tbBarCode.IsEnabled = IsEnabled;
            tbName.IsEnabled = IsEnabled;
            tbAge.IsEnabled = IsEnabled;
            cbAgeUnit.IsEnabled = IsEnabled;
            cbSex.IsEnabled = IsEnabled;
            tbCaseNo.IsEnabled = IsEnabled;
            tbBedNo.IsEnabled = IsEnabled;
            cbPatientType.IsEnabled = IsEnabled;
            cbSendCheckOffice.IsEnabled = IsEnabled;
            cbSampleType.IsEnabled = IsEnabled;
            dpSendCheckDate.IsEnabled = IsEnabled;
            cbSendCheckDoctor.IsEnabled = IsEnabled;
            tbCheckDoctor.IsEnabled = IsEnabled;
            tbAuditor.IsEnabled = IsEnabled;
            cbClinical.IsEnabled = IsEnabled;
            tbRemakes.IsEnabled = IsEnabled;
            cbTest.IsEnabled = IsEnabled;
        }
        /// <summary>
        /// 样本添加
        /// </summary>
        private void SampleInfoAdd()
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已在编辑状态，不能再执行添加操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DGSampleInfo.IsEnabled = false;
            sampleInfoViewModel.IsSampleAdd = true;
            sampleInfoViewModel.IsSampleInfoEdit = true;
            EnableEdit(true);
            sampleInfoViewModel.CurSampleInfoEditObj = new DBSampleInfo() { SampleID = sampleInfoViewModel.GetMaxSampleID() };

            SampleInfoEditBinding();
        }
        /// <summary>
        /// 样本编辑
        /// </summary>
        private void SampleInfoModify()
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已在编辑状态，不能再执行修改操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (DGSampleInfo.SelectedItem == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要编辑的样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            sampleInfoViewModel.IsSampleInfoEdit = true;
            DGSampleInfo.IsEnabled = false;
            EnableEdit(true);
            btnSaveNew.IsEnabled = false;
            tbSampleID.IsEnabled = false;
            sampleInfoViewModel.CurSampleInfoEditObj = new DBSampleInfo();
            (DGSampleInfo.SelectedItem as DBSampleInfo).CopyToTarget(sampleInfoViewModel.CurSampleInfoEditObj);
            SampleInfoEditBinding();
        }

        /// <summary>
        /// 样本保存
        /// </summary>
        private bool SampleInfoSave()
        {
            if (!sampleInfoViewModel.IsSampleInfoEdit)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "没有需要保存的信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            btnSave.Focus();

            if (sampleInfoViewModel.CurSampleInfoEditObj.SampleID <= 0)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本号不能小于1！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (sampleInfoViewModel.IsSampleAdd && sampleInfoViewModel.IsExistsInDB(sampleInfoViewModel.CurSampleInfoEditObj))
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已存在不能保存添加！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (sampleInfoViewModel.SampleInfoSave())
            {
                DBSampleInfo editInfo = sampleInfoViewModel.GetSampleInfoFromList(sampleInfoViewModel.CurSampleInfoEditObj);
                if (editInfo == null)
                {
                    sampleInfoViewModel.SampleInfoList.Add(sampleInfoViewModel.CurSampleInfoEditObj);
                }
                else
                {
                    sampleInfoViewModel.CurSampleInfoEditObj.CopyToTarget(editInfo);
                    DGSampleInfo.SelectedItem = editInfo;
                }

                sampleResultViewModel.SaveAnalyzerResult();
                sampleResultViewModel.SaveManualResult();

                sampleInfoViewModel.IsSampleInfoEdit = false;
                RefreshData(null);
                EnableEdit(false);
                btnSaveNew.IsEnabled = true;
                DGSampleInfo.IsEnabled = true;
                //MessageBox.Show("保存成功！");
                return true;
            }
            else
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return false;
        }

        private void SampleInfoSaveAndNew()
        {
            if (SampleInfoSave())
            {
                SampleInfoAdd();
            }
        }
        /// <summary>
        /// 取消保存
        /// </summary>
        private void CancelSave()
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                sampleInfoViewModel.IsSampleInfoEdit = false;

                if (!sampleInfoViewModel.IsSampleAdd)
                {
                    DBSampleInfo editInfo = sampleInfoViewModel.GetSampleInfoFromList(sampleInfoViewModel.CurSampleInfoEditObj);
                    sampleInfoViewModel.CurSampleInfoEditObj = new DBSampleInfo();
                    editInfo.CopyToTarget(sampleInfoViewModel.CurSampleInfoEditObj);
                    DGSampleInfo.SelectedItem = editInfo;
                }
                else
                {
                    sampleInfoViewModel.CurSampleInfoEditObj = null;
                }
                sampleInfoViewModel.IsSampleAdd = false;
                RefreshData(null);
                SampleInfoEditBinding();
                EnableEdit(false);
                DGSampleInfo.IsEnabled = true;
                btnSaveNew.IsEnabled = true;
            }
        }
        /// <summary>
        /// 样本删除
        /// </summary>
        private void SampleInfoDelete()
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已在编辑状态，不能再执行删除操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (DGSampleInfo.SelectedItem == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要删除的样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if ((DGSampleInfo.SelectedItem as DBSampleInfo).IsAuditor)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "已审核的样本不能删除！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBoxEx.ShowEx(App.Current.MainWindow, "是否要删除选中样本？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            if (sampleInfoViewModel.SampleInfoDelete(DGSampleInfo.SelectedItem as DBSampleInfo))
            {
                sampleInfoViewModel.SampleInfoList.Remove(DGSampleInfo.SelectedItem as DBSampleInfo);
            }
            else
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// 样本批量删除
        /// </summary>
        private void SampleInfoBatchDelete()
        {
            SampleInfoBatchDeleteWindow deleteWindow = new SampleInfoBatchDeleteWindow();

            if (DGSampleInfo.SelectedItem != null)
            {
                deleteWindow.StartSampleInfo.SampleID = (DGSampleInfo.SelectedItem as DBSampleInfo).SampleID;
                deleteWindow.EndSampleInfo.SampleID = (DGSampleInfo.SelectedItem as DBSampleInfo).SampleID;

                deleteWindow.StartSampleInfo.RegistDate = (DGSampleInfo.SelectedItem as DBSampleInfo).RegistDate;
                deleteWindow.EndSampleInfo.RegistDate = (DGSampleInfo.SelectedItem as DBSampleInfo).RegistDate;
            }

            deleteWindow.Owner = App.Current.MainWindow;
            if (deleteWindow.ShowDialog() == true)
            {
                if (sampleInfoViewModel.SampleInfoBatchDelete(deleteWindow.StartSampleInfo, deleteWindow.EndSampleInfo))
                {
                    sampleInfoViewModel.UpdateSampleInfoList();
                }
                else
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "样本批量删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        /// <summary>
        /// 样本审核
        /// </summary>
        /// <param name="IsAuditor"></param>
        private void SampleInfoAuditor(bool IsAuditor)
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                if (IsAuditor)
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已在编辑状态，不能再执行审核操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "样本已在编辑状态，不能再执行取消审核操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return;
            }

            if (DGSampleInfo.SelectedItems == null)
            {
                if (IsAuditor)
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要审核的样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要取消审核的样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return;
            }

            if (IsAuditor)
            {
                if (MessageBoxEx.ShowEx(App.Current.MainWindow, "是否要执行审核操作？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            else
            {
                if (MessageBoxEx.ShowEx(App.Current.MainWindow, "是否要执行取消审核操作？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            foreach (var s in DGSampleInfo.SelectedItems)
            {
                DBSampleInfo info = new DBSampleInfo();
                (s as DBSampleInfo).CopyToTarget(info);
                if (IsAuditor)
                {
                    if (sampleInfoViewModel.SampleInfoAuditor(info, IsAuditor))
                    {
                        info.CopyToTarget(s as DBSampleInfo);
                    }
                    else
                    {
                        MessageBoxEx.ShowEx(App.Current.MainWindow, "审核失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (sampleInfoViewModel.SampleInfoAuditor(info, IsAuditor))
                    {
                        info.CopyToTarget(s as DBSampleInfo);
                    }
                    else
                    {
                        MessageBoxEx.ShowEx(App.Current.MainWindow, "取消审核失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        /// <summary>
        /// 编辑控件绑定
        /// </summary>
        private void SampleInfoEditBinding()
        {
            tbSampleID.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbEmergency.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            dpCheckDate.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbBarCode.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbName.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbAge.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbAgeUnit.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbSex.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbCaseNo.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbBedNo.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbPatientType.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbSendCheckOffice.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbSampleType.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            dpSendCheckDate.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbSendCheckDoctor.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbCheckDoctor.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbAuditor.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbClinical.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            tbRemakes.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
            cbTest.DataContext = sampleInfoViewModel.CurSampleInfoEditObj;
        }
        private void ClearSampleInfoBinding()
        {
            tbSampleID.DataContext = null;
            cbEmergency.DataContext = null;
            dpCheckDate.DataContext = null;
            tbBarCode.DataContext = null;
            tbName.DataContext = null;
            tbAge.DataContext = null;
            cbAgeUnit.DataContext = null;
            cbSex.DataContext = null;
            tbCaseNo.DataContext = null;
            tbBedNo.DataContext = null;
            cbPatientType.DataContext = null;
            cbSendCheckOffice.DataContext = null;
            cbSampleType.DataContext = null;
            dpSendCheckDate.DataContext = null;
            cbSendCheckDoctor.DataContext = null;
            tbCheckDoctor.DataContext = null;
            tbAuditor.DataContext = null;
            cbClinical.DataContext = null;
            tbRemakes.DataContext = null;
            cbTest.DataContext = null;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            sampleInfoViewModel.UpdateSampleInfoList();
            DGSampleInfo.ItemsSource = sampleInfoViewModel.SampleInfoList;
            dgAnalyzerResult.ItemsSource = sampleResultViewModel.ResultAnalyzerList;
            dgManualResult.ItemsSource = sampleResultViewModel.ResultManualList;

            //sampleInfoViewModel.UpdateSampleInfoList();
            LoadTypeItems();
            EnableEdit(false);
            //refreshDataTimer = new Timer(RefreshData, null, 10000, 10000);
            RefreshData(null);
        }
        private void SetDBSampleResultInfo(ref DBSampleResultInfo resultInfo, ItemsInfos item, DBSampleInfo info)
        {
            resultInfo.Caption = item.Caption;
            resultInfo.ItemID = item.ItemID;
            resultInfo.Name = item.Name;
            resultInfo.Range = item.Range;
            resultInfo.SampleID = info.SampleID;
            resultInfo.RegistDate = info.RegistDate;
            resultInfo.Result = item.Result;
            resultInfo.Unit = item.Unit;
            resultInfo.Abnormal = item.Abnormal;
            resultInfo.ResultIndex = item.ResultIndex;
        }
        private bool AnalyzerResultAdd()
        {
            if (DGSampleInfo.SelectedItem == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要添加结果的样本信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            DBSampleInfo info = DGSampleInfo.SelectedItem as DBSampleInfo;
            AddItem addItem = new AddItem();
            addItem.Owner = App.Current.MainWindow;
            addItem.UseItemList = sampleResultViewModel.ResultAnalyzerList;
            if (addItem.ShowDialog() == true)
            {
                sampleResultViewModel.ResultAnalyzerList.Clear();
                foreach (var item in addItem.SelectItemInfoList)
                {
                    DBSampleResultInfo resultInfo = new DBSampleResultInfo();
                    SetDBSampleResultInfo(ref resultInfo, item, info);
                    sampleResultViewModel.ResultAnalyzerList.Add(resultInfo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ManualResultAdd()
        {
            if (DGSampleInfo.SelectedItem == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择要添加结果的样本信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            DBSampleInfo info = DGSampleInfo.SelectedItem as DBSampleInfo;
            AddItem addItem = new AddItem();
            addItem.Owner = App.Current.MainWindow;
            addItem.UseItemList = sampleResultViewModel.ResultManualList;
            if (addItem.ShowDialog() == true)
            {
                sampleResultViewModel.ResultManualList.Clear();
                foreach (var item in addItem.SelectItemInfoList)
                {
                    DBSampleResultInfo resultInfo = new DBSampleResultInfo();
                    SetDBSampleResultInfo(ref resultInfo, item, info);
                    sampleResultViewModel.ResultManualList.Add(resultInfo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                EditSampleButtonEnable(false);

                SampleInfoAdd();
            }
            catch (Exception ex)
            {
                EditSampleButtonEnable(true);
                MessageBoxEx.ShowEx(App.Current.MainWindow, "样本添加出错", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sampleInfoViewModel.IsSampleInfoEdit)
            {
                if (MessageBoxEx.ShowEx(App.Current.MainWindow, "样本在编辑状态，是否需要保存?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SampleInfoSave();
                }
                else
                {
                    EnableEdit(false);
                    sampleInfoViewModel.IsSampleInfoEdit = false;
                }
            }
            //refreshDataTimer.Dispose();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SampleInfoSave();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                EditSampleButtonEnable(true);
            }


        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoModify();
        }
        /// <summary>
        /// 取消保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CancelSave();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                EditSampleButtonEnable(true);
            }

        }

        private void DGSampleInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sampleInfoViewModel.IsSampleInfoEdit && DGSampleInfo.SelectedItem != null)
            {
                sampleInfoViewModel.CurSampleInfoEditObj = (DBSampleInfo)DGSampleInfo.SelectedItem;
                SampleInfoEditBinding();
            }
            else
            {
                sampleInfoViewModel.CurSampleInfoEditObj = null;
            }

            sampleResultViewModel.UpdateAnalyzerResult(sampleInfoViewModel.CurSampleInfoEditObj);
            sampleResultViewModel.UpdateManualResult(sampleInfoViewModel.CurSampleInfoEditObj);
        }

        private void btnSaveNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoSaveAndNew();
        }

        private void btnUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DGSampleInfo.SelectedIndex > 0)
            {
                DGSampleInfo.SelectedIndex -= 1;
            }

        }

        private void btnDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DGSampleInfo.SelectedIndex < sampleInfoViewModel.SampleInfoList.Count)
            {
                DGSampleInfo.SelectedIndex += 1;
            }
        }

        private void btnDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoDelete();
        }

        private void btnBatchDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoBatchDelete();
        }

        private void btnAuditor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoAuditor(true);
        }

        private void btnUnAuditor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SampleInfoAuditor(false);
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox cbAnalyzerName = sender as ComboBox;
            List<DBChemicalItemInfo> comlist = new List<DBChemicalItemInfo>();
            //sampleResultViewModel.GetItemNameList(comlist);
            cbAnalyzerName.ItemsSource = comlist;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbAnalyzerName = sender as ComboBox;
            (dgAnalyzerResult.SelectedItem as DBSampleResultInfo).Name = (cbAnalyzerName.SelectedItem as DBChemicalItemInfo).Name;
            (dgAnalyzerResult.SelectedItem as DBSampleResultInfo).Unit = (cbAnalyzerName.SelectedItem as DBChemicalItemInfo).Unit;
            (dgAnalyzerResult.SelectedItem as DBSampleResultInfo).Range = (cbAnalyzerName.SelectedItem as DBChemicalItemInfo).Range;
            (dgAnalyzerResult.SelectedItem as DBSampleResultInfo).IsModify = true;
        }

        private void btnAnalyzerResultAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AnalyzerResultAdd())
            {
                sampleResultViewModel.SaveAnalyzerResult();
            }
        }

        private void btnManualResultAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ManualResultAdd())
            {
                sampleResultViewModel.SaveManualResult();
            }
        }

        private void btnAnalyzerResultDelete_Click(object sender, RoutedEventArgs e)
        {
            DBSampleResultInfo result = dgAnalyzerResult.SelectedItem as DBSampleResultInfo;
            if (result == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择需要删除的结果信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBoxEx.ShowEx(App.Current.MainWindow, "是否要删除'" + result.Name + "'项目的结果信息？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            if (!sampleResultViewModel.DeleteAnalyzerResult(result))
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "结果删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                sampleResultViewModel.ResultAnalyzerList.Remove(result);
            }

        }

        private void btnManualResultDelete_Click(object sender, RoutedEventArgs e)
        {
            DBSampleResultInfo result = dgManualResult.SelectedItem as DBSampleResultInfo;
            if (result == null)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择需要删除的结果信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBoxEx.ShowEx(App.Current.MainWindow, "是否要删除'" + result.Name + "'项目的结果信息？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            if (!sampleResultViewModel.DeleteManualResult(result))
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "结果删除失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                sampleResultViewModel.ResultManualList.Remove(result);
            }
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

            DBHospInfoItemType SampleTypeitem = new DBHospInfoItemType();
            SampleTypeitem.ItemID = HOSPINFOITEMS.HI_SAMPLE_TYPE;
            cbSampleType.Tag = HOSPINFOITEMS.HI_SAMPLE_TYPE;
            hospInfoViewModel.LoadTypeItem(SampleTypeitem);
            cbSampleType.ItemsSource = SampleTypeitem.Items;
            cbSampleType.DisplayMemberPath = "Caption";
            cbSampleType.SelectedValuePath = "Caption";

            DBHospInfoItemType SendCheckDoctoritem = new DBHospInfoItemType();
            SendCheckDoctoritem.ItemID = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            cbSendCheckDoctor.Tag = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            hospInfoViewModel.LoadTypeItem(SendCheckDoctoritem);
            cbSendCheckDoctor.ItemsSource = SendCheckDoctoritem.Items;
            cbSendCheckDoctor.DisplayMemberPath = "Caption";
            cbSendCheckDoctor.SelectedValuePath = "Caption";

            DBHospInfoItemType Clinicalitem = new DBHospInfoItemType();
            Clinicalitem.ItemID = HOSPINFOITEMS.HI_CLINICAL_DIAGNOSIS;
            cbClinical.Tag = HOSPINFOITEMS.HI_CLINICAL_DIAGNOSIS;
            hospInfoViewModel.LoadTypeItem(Clinicalitem);
            cbClinical.ItemsSource = Clinicalitem.Items;
            cbClinical.DisplayMemberPath = "Caption";
            cbClinical.SelectedValuePath = "Caption";



            DBHospInfoItemType CheckDoctoritem = new DBHospInfoItemType();
            CheckDoctoritem.ItemID = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            tbCheckDoctor.Tag = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            hospInfoViewModel.LoadTypeItem(CheckDoctoritem);
            tbCheckDoctor.ItemsSource = CheckDoctoritem.Items;
            tbCheckDoctor.DisplayMemberPath = "Caption";
            tbCheckDoctor.SelectedValuePath = "Caption";



            DBHospInfoItemType Auditoritem = new DBHospInfoItemType();
            Auditoritem.ItemID = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            tbAuditor.Tag = HOSPINFOITEMS.HI_SENDCHECK_DOCTOR;
            hospInfoViewModel.LoadTypeItem(Auditoritem);
            tbAuditor.ItemsSource = Auditoritem.Items;
            tbAuditor.DisplayMemberPath = "Caption";
            tbAuditor.SelectedValuePath = "Caption";
        }

        private void btnQuery_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SampleQueryWindow sampleQueryWindow = new SampleQueryWindow();
            sampleQueryWindow.Owner = App.Current.MainWindow;
            if (sampleQueryWindow.ShowDialog() == true)
            {
                //refreshDataTimer.Dispose();
                sampleInfoViewModel.UpdateSampleInfoList(sampleQueryWindow.BeginDate, sampleQueryWindow.EndDate, sampleQueryWindow.OtherCondition);
                if (sampleResultViewModel != null)
                {
                    sampleResultViewModel.ResultAnalyzerList.Clear();
                    sampleResultViewModel.ResultManualList.Clear();
                }
                ClearSampleInfoBinding();
            }
        }

        private void btnPreview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DGSampleInfo.SelectedItem != null)
            {
                reportPrintViewModel.ReportPreview(((DBSampleInfo)DGSampleInfo.SelectedItem).SampleID, ((DBSampleInfo)DGSampleInfo.SelectedItem).RegistDate, App.Current.MainWindow);
            }
            else
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btnPrint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DGSampleInfo.SelectedItem != null)
            {
                reportPrintViewModel.PrintReport(((DBSampleInfo)DGSampleInfo.SelectedItem).SampleID, ((DBSampleInfo)DGSampleInfo.SelectedItem).CheckDate);
            }
            else
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择样本！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BatchPrint(object obj)
        {
            reportPrintViewModel.PrintReport(batchSampleDataSet, (BatchPrintProgressWindow)obj, ((BatchPrintProgressWindow)obj).info, ((BatchPrintProgressWindow)obj).progressBar);
        }
        private void btnBatchPrint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BatchPrintSetWindow batchPrintSetWindow = new BatchPrintSetWindow();
            batchPrintSetWindow.Owner = App.Current.MainWindow;
            if (batchPrintSetWindow.ShowDialog() == true)
            {
                batchSampleDataSet = reportPrintViewModel.GetBatchSample(batchPrintSetWindow.BeginNo, batchPrintSetWindow.EndNo, batchPrintSetWindow.TestDate, batchPrintSetWindow.IsTest, batchPrintSetWindow.IsAuditor);
                if (batchSampleDataSet.Tables[0].Rows.Count == 0)
                {
                    MessageBoxEx.ShowEx(App.Current.MainWindow, "没有可打印的报告单！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    BatchPrintProgressWindow batchPrintProgressWindow = new BatchPrintProgressWindow();
                    batchPrintProgressWindow.Owner = App.Current.MainWindow;
                    batchPrintProgressWindow.progressBar.Minimum = 0;
                    batchPrintProgressWindow.progressBar.Maximum = batchSampleDataSet.Tables[0].Rows.Count;
                    Thread batchPrintThread = new Thread(new ParameterizedThreadStart(BatchPrint));
                    batchPrintThread.Start(batchPrintProgressWindow);
                    batchPrintProgressWindow.ShowDialog();
                }
            }
        }

        private void btnQueryImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DGSampleInfo.SelectedIndex < 0)
            {
                MessageBoxEx.ShowEx(App.Current.MainWindow, "请选择样本后再查看试纸条图像！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SampleImageWindow sampleImageWindow = new SampleImageWindow();
            sampleImageWindow.Owner = App.Current.MainWindow;
            sampleImageWindow.SampleID = ((DBSampleInfo)DGSampleInfo.SelectedItem).SampleID;
            sampleImageWindow.ImageFile = ((DBSampleInfo)DGSampleInfo.SelectedItem).ImageFile;
            sampleImageWindow.ShowDialog();
        }

        private void btnTodayQuery_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //refreshDataTimer = new Timer(RefreshData, null, 0, 10000);
            RefreshData(null);
        }
    }
}
