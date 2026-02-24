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
using System.Collections;
using System.Collections.ObjectModel;
using ViewModel;
using DataModel;
using Tool;
using System.ComponentModel;

namespace UrineAnalyzer
{
  /// <summary>
  /// LoginPage.xaml 的交互逻辑
  /// </summary>
  /// 
  public partial class LoginPage : Window
  {
    private AuthoritySettingViewModel authoritySettingViewModel;
    private LinkParamModel linkParam = new LinkParamModel();


    #region UI更新接口
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = null)
    {
      if (PropertyChanged != null)
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion


    public LoginPage()
    {
      InitializeComponent();
    }


    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }
    private bool CheckUserPWD()
    {
      bool exists = false;
      if ((cbbUserName.Text == "admin" && txtUserPWD.Password == "admin"))
      {
        exists = true;
      }
      else
      {
        for (int i = 0; i < authoritySettingViewModel.UserList.Count; i++)
        {
          if (authoritySettingViewModel.UserList[i].UserName == cbbUserName.Text && authoritySettingViewModel.UserList[i].PWD == txtUserPWD.Password)
          {
            Const.authority = authoritySettingViewModel.UserList[i].AuthorityName;
            Const.user = authoritySettingViewModel.UserList[i].UserName;
            exists = true;
            break;
          }
        }
      }
      return exists;
    }
    private int loginCnt = 0;
    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      loginCnt = loginCnt + 1;
      if (loginCnt >= 3)
      {
        MessageBoxEx.ShowEx(this,"登录错误次数达到3次，系统直接退出！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
        return;
      }
      if (CheckUserPWD())
      {
        App.DBConnectionString = ConnectionObj.GetConnectionString();
        DialogResult = true;
      }
      else
      {
        MessageBoxEx.ShowEx(this,"用户名或密码错误，请重新输入。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      DBLinkParam dbLinkParam = new DBLinkParam();
      linkParam.Load(dbLinkParam);
      ConnectionObj.DBName = dbLinkParam.DBName;
      authoritySettingViewModel = new AuthoritySettingViewModel();
      authoritySettingViewModel.GetUsers();
      cbbUserName.Focus();
    }

    //private void txtUserPWD_KeyUp(object sender, KeyEventArgs e)
    //{
    //  if (e.Key.Equals(Key.Return))
    //  {
    //    btnLogin.Focus();
    //  }
    //}

    //private void cbbUserName_KeyUp(object sender, KeyEventArgs e)
    //{
    //  if(e.Key.Equals(Key.Return))
    //  {
    //    txtUserPWD.Focus();
    //  }
    //}
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

    private void Login_Window_MouseMove(object sender, MouseEventArgs e)
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
