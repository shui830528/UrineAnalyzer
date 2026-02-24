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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;
using DataModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// BaseParamPage.xaml 的交互逻辑
  /// </summary>
  public partial class BaseParamPage : Page
  {
    private BackupDBViewModel backupDBViewModel = new BackupDBViewModel();
    private LinkParamModel linkParamModel = new LinkParamModel();
    private DBLinkParam dbLinkParam = new DBLinkParam();
    public BaseParamPage()
    {
      InitializeComponent();
    }

    private void button_BackUp_Click(object sender, RoutedEventArgs e)
    {
      bool isBackupFinish = backupDBViewModel.BackUpDB();
      if(!isBackupFinish)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow, "数据库备份失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      if (checkBox_Set.IsChecked == true && isBackupFinish)
      {
        backupDBViewModel.ClearTestData();
      }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      List<BackupDBObject> dbs = backupDBViewModel.GetBackupDBs();
      comboBox_DBName.ItemsSource = dbs;
      comboBox_DBName.DisplayMemberPath = "Caption";
      comboBox_DBName.SelectedValue = dbs.SingleOrDefault(bdb=>bdb.DBName == ConnectionObj.DBName);
      linkParamModel.Load(dbLinkParam);
    }

    private void button_Save_Click(object sender, RoutedEventArgs e)
    {
      BackupDBObject dbObject = comboBox_DBName.SelectedItem as BackupDBObject;
      if (dbObject != null && dbLinkParam.DBName != dbObject.DBName)
      {
        dbLinkParam.DBName = dbObject.DBName;
        if(linkParamModel.Save(dbLinkParam))
        {
          MessageBoxEx.ShowEx(App.Current.MainWindow, "保存完成。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
          MessageBoxEx.ShowEx(App.Current.MainWindow, "保存失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }
  }
}
