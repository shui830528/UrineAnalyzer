using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{

  public class AuthoritySettingViewModel
  {
    private AuthoritySettingModel authoritySettingModel = new AuthoritySettingModel();
    private ObservableCollection<Authority> authorityList = new ObservableCollection<Authority>();
    public ObservableCollection<Authority> AuthorityList
    {
      get { return authorityList; }
    }
    private ObservableCollection<User> userList = new ObservableCollection<User>();
    public ObservableCollection<User> UserList
    {
      get { return userList; }
    }
    private ObservableCollection<User> normalUserList = new ObservableCollection<User>();
    public ObservableCollection<User> NormalUserList
    {
      get { return normalUserList; }
    }
    public AuthoritySettingViewModel()
    {
      authorityList.Clear();
      Authority authority1 = new Authority();
      authority1.AuthorityID = 1;
      authority1.AuthorityName = "系统管理员";
      authorityList.Add(authority1);
      Authority authority2 = new Authority();
      authority2.AuthorityID = 2;
      authority2.AuthorityName = "管理员";
      authorityList.Add(authority2);
      Authority authority3 = new Authority();
      authority3.AuthorityID = 3;
      authority3.AuthorityName = "用户";
      authorityList.Add(authority3);
      GetAuthoritys();
    }
    private void GetAuthoritys()
    {
      authoritySettingModel.GetAuthoritySetting(ref authorityList);
    }
    public void GetUsers()
    {
      authoritySettingModel.GetUsers(ref userList);
    }
    public void GetNormalUsers()
    {
      authoritySettingModel.GetNormalUsers(ref normalUserList);
    }
    public bool SaveAuthoritys()
    {
      return authoritySettingModel.SaveAuthoritySetting(authorityList) > 0;
    }
    public bool SaveUser(User user)
    {
      return authoritySettingModel.SaveUser(user) > 0;
    }
    public Authority GetAuthority(int authorityID)
    {
      for(int i=0;i<authorityList.Count;i++)
      {
        if(authorityList[i].AuthorityID == authorityID)
        {
          return authorityList[i];
        }
      }
      return null;
    }
    public void DeleteUser(int userID)
    {
      authoritySettingModel.DeleteUser(userID);
    }
  }
}
