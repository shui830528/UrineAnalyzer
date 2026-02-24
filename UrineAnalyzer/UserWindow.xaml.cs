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
using DataModel;
using ViewModel;
using Tool;

namespace UrineAnalyzer
{
  /// <summary>
  /// UserWindow.xaml 的交互逻辑
  /// </summary>
  public partial class UserWindow : ModernWindow
  {
    private User user = new User();
    public UserWindow()
    {
      InitializeComponent();
    }
    private AuthoritySettingViewModel authoritySettingViewModel = null;
    public AuthoritySettingViewModel AuthorityUsers
    {
      get { return authoritySettingViewModel; }
      set { authoritySettingViewModel = value; }
    }
    private void button_Ok_Click(object sender, RoutedEventArgs e)
    {
      if(textBox_User.Text.Trim() == "")
      {
        MessageBoxEx.ShowEx(this,"用户名称不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      //if(comboBox.SelectedItem == null)
      //{
      //  MessageBox.Show(this,"请选择用户所属角色！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      //  return;
      //}
      user.UserName = textBox_User.Text;
      user.PWD = passwordBox.Password;
      user.AuthorityID = 3;
      //user.AuthorityID = ((Authority)comboBox.SelectedItem).AuthorityID;
      authoritySettingViewModel.SaveUser(user);
      //Const.user = user.UserName;
      //Const.authority = ((Authority)comboBox.SelectedItem).AuthorityName;
      DialogResult = true;
    }

    private void button_Cancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //comboBox.ItemsSource = authoritySettingViewModel.AuthorityList;
      //comboBox.DisplayMemberPath = "AuthorityName";
      //comboBox.SelectedValuePath = "AuthorityID";
    }
  }
}
