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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// PasswordSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class PasswordSettingPage : Page
  {
    public PasswordSettingPage()
    {
      InitializeComponent();
    }

    private void button_Save_Click(object sender, RoutedEventArgs e)
    {
      if(textBox_UserName.Text.Trim() == "")
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"用户名称不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      if(passwordBox_NewPWD2.Password != passwordBox_NewPWD1.Password)
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"新密码与确认密码不相同，请重新输入密码！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }
      User user = new User();
      user.UserName = textBox_UserName.Text.Trim();
      user.PWD = passwordBox_NewPWD2.Password;
      AuthoritySettingViewModel authoritySettingViewModel = new AuthoritySettingViewModel();
      authoritySettingViewModel.GetUsers();
      user.AuthorityID = authoritySettingViewModel.UserList.Single(u => u.UserName == user.UserName).AuthorityID;
      if (authoritySettingViewModel.SaveUser(user))
      {
        MessageBoxEx.ShowEx(App.Current.MainWindow,"密码修改成功。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
  }
}
