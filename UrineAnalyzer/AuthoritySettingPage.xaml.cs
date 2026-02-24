using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace UrineAnalyzer
{
  /// <summary>
  /// AuthoritySettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class AuthoritySettingPage : Page
  {
    private AuthoritySettingViewModel authoritySettingViewModel = new AuthoritySettingViewModel();
    public AuthoritySettingPage()
    {

    }

    private void authority_Loaded(object sender, RoutedEventArgs e)
    {
      authoritys.ItemsSource = authoritySettingViewModel.AuthorityList;
      authoritys.DisplayMemberPath = "AuthorityName";
      authoritys.SelectedValuePath = "AuthorityID";
      authoritys.SelectedIndex = 0;
      module.SelectedIndex = 0;
    }

    private void button_Save_Click(object sender, RoutedEventArgs e)
    {
      if(authoritySettingViewModel.SaveAuthoritys())
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"角色权限设置保存完成。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow, "角色权限设置保存失败。", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void module_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (module.SelectedItem == null)
      {
        return;
      }
      Module md = (Module)module.SelectedItem;
      function.ItemsSource = md.ModuleFunctionList;
      function.SelectedIndex = 0;
    }

    private void authoritys_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (authoritys.SelectedItem == null)
      {
        return;
      }
      Authority authority = (Authority)authoritys.SelectedItem;
      module.ItemsSource = authority.ModuleList;
      module.SelectedIndex = 0;
      module.DisplayMemberPath = "ModuleName";
      module.SelectedValuePath = "ModuleID";
    }
  }
}
