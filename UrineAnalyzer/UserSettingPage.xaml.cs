using DataModel;
using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// UserSettingPage.xaml 的交互逻辑
  /// </summary>
  public partial class UserSettingPage : Page
  {
    private AuthoritySettingViewModel authoritySettingViewModel = new AuthoritySettingViewModel();
    public UserSettingPage()
    {
      InitializeComponent();
    }

    private void button_Add_Click(object sender, RoutedEventArgs e)
    {
      UserWindow userWindow = new UserWindow();
      userWindow.AuthorityUsers = authoritySettingViewModel;
      userWindow.Owner = App.Current.MainWindow;
      if(userWindow.ShowDialog() == true)
      {
        authoritySettingViewModel.GetNormalUsers();
      }
    }

    private void button_Delete_Click(object sender, RoutedEventArgs e)
    {
      User user = (User)DataGrid_User.SelectedItem;
      if ( user!= null)
      {
        authoritySettingViewModel.DeleteUser(user.UserID);
        authoritySettingViewModel.GetNormalUsers();
      }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
      authoritySettingViewModel.GetNormalUsers();
      DataGrid_User.ItemsSource = authoritySettingViewModel.NormalUserList;
    }
  }
}
