using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tool;

namespace DataModel
{
  public class User
  {
    public string UserName
    {
      get;
      set;
    }
    public int UserID
    {
      get;
      set;
    }
    public string PWD
    {
      get;
      set;
    }
    public int AuthorityID
    {
      get;
      set;
    }
    public string AuthorityName
    {
      get;
      set;
    }
  }
  public class ModuleFunctionItem
  {
    public int FunctionID
    {
      get;
      set;
    }
    public string FunctionName
    {
      get;
      set;
    }
    public bool IsChecked
    {
      get;
      set;
    }
  }
  public class Module
  {
    public Module()
    {
      moduleFunctionList.Clear();
      ModuleFunctionItem func1 = new ModuleFunctionItem();
      func1.FunctionID = 1;
      func1.FunctionName = "修改";
      moduleFunctionList.Add(func1);
      ModuleFunctionItem func2 = new ModuleFunctionItem();
      func2.FunctionID = 2;
      func2.FunctionName = "查询";
      moduleFunctionList.Add(func2);
      ModuleFunctionItem func3 = new ModuleFunctionItem();
      func3.FunctionID = 3;
      func3.FunctionName = "打印";
      moduleFunctionList.Add(func3);
    }
    public int ModuleID
    {
      get;
      set;
    }
    public string ModuleName
    {
      get;
      set;
    }
    public bool IsChecked
    {
      get;
      set;
    }
    private ObservableCollection<ModuleFunctionItem> moduleFunctionList = new ObservableCollection<ModuleFunctionItem>();
    public ObservableCollection<ModuleFunctionItem> ModuleFunctionList
    {
      get
      {
        return moduleFunctionList;
      }
    }
  }
  public class Authority
  {
    public Authority()
    {
      moduleList.Clear();
      Module m1 = new Module();
      m1.ModuleID = 1;
      m1.ModuleName = "样本管理";
      moduleList.Add(m1);
      Module m2 = new Module();
      m2.ModuleID = 2;
      m2.ModuleName = "质控管理";
      moduleList.Add(m2);
      Module m3 = new Module();
      m3.ModuleID = 3;
      m3.ModuleName = "工作量统计";
      moduleList.Add(m3);
      Module m4 = new Module();
      m4.ModuleID = 4;
      m4.ModuleName = "系统设置";
      moduleList.Add(m4);
      Module m5 = new Module();
      m5.ModuleID = 5;
      m5.ModuleName = "用户管理";
      moduleList.Add(m5);
      Module m6 = new Module();
      m6.ModuleID = 6;
      m6.ModuleName = "接收日志";
      moduleList.Add(m6);
    }
    public int AuthorityID
    {
      get;
      set;
    }
    public string AuthorityName
    {
      get;
      set;
    }
    public string AuthorityContent
    {
      get { return GetAuthorityContent(); }
    }
    private ObservableCollection<Module> moduleList = new ObservableCollection<Module>();
    public ObservableCollection<Module> ModuleList
    {
      get
      {
        return moduleList;
      }
    }
    private string GetAuthorityContent()
    {
      string authorityContent = "";
      for (int i = 0; i < moduleList.Count; i++)
      {
        for(int j=0;j<moduleList[i].ModuleFunctionList.Count;j++)
        {
          if (moduleList[i].ModuleFunctionList[j].IsChecked)
          {
            authorityContent = authorityContent + "," + moduleList[i].ModuleID.ToString() + moduleList[i].ModuleFunctionList[j].FunctionID;
          }
        }
      }
      return authorityContent.Substring(1);
    }
    //functionID: 模块ID1位数字+功能ID1位数字,需要在功能按钮的Tag属性里设置两位数字来标识该功能是否可用
    public bool IsHavePermissions(string functionID)
    {
      if (functionID.Length != 2)
      {
        return true;
      }
      else
      {
        string mID = functionID.Substring(0, 1);
        string fID = functionID.Substring(1, 1);
        for (int i = 0; i < moduleList.Count; i++)
        {
          for(int j=0;j<moduleList[i].ModuleFunctionList.Count;j++)
          {
            if (moduleList[i].ModuleID.ToString() == mID && moduleList[i].ModuleFunctionList[j].FunctionID.ToString() == fID && moduleList[i].ModuleFunctionList[j].IsChecked)
            {
              return true;
            }
          }
        }
      }
      return false;
    }

    public void SetPermissions(string functionID)
    {
      string mID = functionID.Substring(0, 1);
      string fID = functionID.Substring(1, 1);
      for (int i = 0; i < moduleList.Count; i++)
      {
        for (int j = 0; j < moduleList[i].ModuleFunctionList.Count; j++)
        {
          if (moduleList[i].ModuleID.ToString() == mID && moduleList[i].ModuleFunctionList[j].FunctionID.ToString() == fID)
          {
            moduleList[i].ModuleFunctionList[j].IsChecked = true;
            return;
          }
        }
      }
    }
  }
  public class AuthoritySettingModel
  {
    private DB db = new DB();
    public int SaveAuthoritySetting(ObservableCollection<Authority> authorityList)
    {
      int nCount = 0;
      string sql = "";
      try
      {
        List<string> sqlList = new List<string>();
        foreach (Authority authority in authorityList)
        {
          DataSet dataSet = null;
          string existsSQL = "select * from Tab_Authority where ID = "+authority.AuthorityID;
          db.Query(existsSQL, out dataSet);
          if (dataSet.Tables[0].Rows.Count > 0)
          {
            sql = "update Tab_Authority set AuthorityName = '"+ authority.AuthorityName + "',AuthorityText = '"+ authority.AuthorityContent + "' where ID =" + authority.AuthorityID;
          }
          else
          {
            sql = "Insert Into Tab_Authority(ID,AuthorityName,AuthorityText) Values(" + authority.AuthorityID + ",'" + authority.AuthorityName + "','" + authority.AuthorityContent + "')";
          }
          sqlList.Add(sql);
        }
        nCount = db.BatchExecute(sqlList);
      }
      catch(Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.SaveAuthoritySetting Error " + sql);
      }
      return nCount;
    }
    public void GetAuthoritySetting(ref ObservableCollection<Authority> authorityList)
    {
      string sql = "select * from Tab_Authority";
      try
      {
        IDataReader objReader = null;
        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          Authority authority = null;
          int authorityID = objReader["ID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ID"]);
          for(int i = 0;i<authorityList.Count;i++)
          {
            if(authorityList[i].AuthorityID == authorityID)
            {
              authority = authorityList[i];
              break;
            }
          }
          if(authority == null)
          {
            authority = new Authority();
            authorityList.Add(authority);
            authority.AuthorityID = objReader["ID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ID"]);
          }
          authority.AuthorityName = objReader["AuthorityName"].ToString();
          string authorityContent = objReader["AuthorityText"].ToString();
          while(authorityContent.IndexOf(",") >= 0)
          {
            string function = authorityContent.Substring(0, authorityContent.IndexOf(",") + 1);
            authorityContent = authorityContent.Substring(authorityContent.IndexOf(",") + 1);
            authority.SetPermissions(function.Substring(0,2));
          }
          if(authorityContent != "")
          {
            authority.SetPermissions(authorityContent);
          }
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.GetAuthoritySetting Error " + sql);
      }
    }
    public void GetUsers(ref ObservableCollection<User> userList)
    {
      string sql = "select Tab_User.*,Tab_Authority.AuthorityName from Tab_User left join Tab_Authority on Tab_User.AuthorityID = Tab_Authority.ID";
      try
      {
        userList.Clear();
        IDataReader objReader = null;
        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          User user = new User();
          user.UserID = objReader["ID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ID"]);
          user.UserName = objReader["UserName"].ToString();
          user.AuthorityName = objReader["AuthorityName"].ToString();
          user.PWD = objReader["PWD"].ToString();
          user.AuthorityID = objReader["AuthorityID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["AuthorityID"]);
          userList.Add(user);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.GetUsers Error " + sql);
      }
    }
    public void GetNormalUsers(ref ObservableCollection<User> userList)
    {
      string sql = "select Tab_User.*,Tab_Authority.AuthorityName from Tab_User left join Tab_Authority on Tab_User.AuthorityID = Tab_Authority.ID where AuthorityID = 3";
      try
      {
        userList.Clear();
        IDataReader objReader = null;
        db.Query(sql, out objReader);

        while (objReader != null && objReader.Read())
        {
          User user = new User();
          user.UserID = objReader["ID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["ID"]);
          user.UserName = objReader["UserName"].ToString();
          user.AuthorityName = objReader["AuthorityName"].ToString();
          user.PWD = objReader["PWD"].ToString();
          user.AuthorityID = objReader["AuthorityID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["AuthorityID"]);
          userList.Add(user);
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.GetUsers Error " + sql);
      }
    }
    public int SaveUser(User user)
    {
      int nCount = 0;
      string sql = "";
      try
      {
        DataSet dataSet = null;
        string existsSQL = "select * from Tab_User where UserName = '" + user.UserName+"'";
        db.Query(existsSQL, out dataSet);
        if (dataSet.Tables[0].Rows.Count > 0)
        {
          sql = "update Tab_User set AuthorityID = " + user.AuthorityID + ",PWD = '"+user.PWD+ "' where UserName ='" + user.UserName+"'";
        }
        else
        {
          sql = "Insert Into Tab_User(UserName,PWD,AuthorityID) Values('" + user.UserName + "','" + user.PWD + "'," + user.AuthorityID + ")";
        }
        nCount = db.Execute(sql);
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.SaveUser Error " + sql);
      }
      return nCount;
    }
    public void DeleteUser(int userID)
    {
      string sql = "";
      try
      {
        sql = "Delete from Tab_User where ID =" + userID;
        db.Execute(sql);
      }
      catch (Exception ex)
      {
        Log.WriteLog(ex.Message + " AuthoritySettingModel.DeleteUser Error " + sql);
      }
    }
  }
}
