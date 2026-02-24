using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Themes
{
  /// <summary>
  /// 主题更新接口
  /// </summary>
  public interface IThemeUpdate
  {
    /// <summary>
    /// 主题更新方法
    /// </summary>
    /// <param name="obj"></param>
    void ThemeUpdateHander(SkinInfo obj);
  }
  /// <summary>
  /// 皮肤信息
  /// </summary>
  public class SkinInfo
  {
    public string TitleColor = "#3D3D3D";//"#FFA7CDCD";
    public string Name = "Theme1";
  }
  /// <summary>
  /// 皮夫列表控制
  /// </summary>
  public class SkinManage
  {
    private List<SkinInfo> skinList = new List<SkinInfo>();
    private List<IThemeUpdate> updateList = new List<IThemeUpdate>();
    private SkinInfo CurSkinInfo = null;

    public List<SkinInfo> SkinItems
    {
      get { return skinList; }
    }
    public List<IThemeUpdate> UpdateItems
    {
      get { return updateList; }
    }
    protected void InitSkinInfo()
    {
      skinList.Add(new SkinInfo());

      CurSkinInfo = skinList[0];
    }
    public SkinManage()
    {
      InitSkinInfo();
    }
    /// <summary>
    /// 不为空更新指定接口对象，否则更新所有接口对象
    /// </summary>
    /// <param name="obj"></param>
    public void UpdateTheme(IThemeUpdate obj)
    {
      foreach(IThemeUpdate item in updateList)
      {
        if (obj == null || obj == item)
        {
          if (item != null)
          {
            item.ThemeUpdateHander(CurSkinInfo);
          }
          
        }
      }
    }
    /// <summary>
    /// 添加主题更新事件接口
    /// </summary>
    /// <param name="obj"></param>
    public void AddUpdateEvent(IThemeUpdate obj)
    {
      if (updateList.Where(O=>O == obj).ToList().Count == 0)
      {
        updateList.Add(obj);
      }
    }
    public void SetSkin(SkinInfo obj)
    {
      CurSkinInfo = obj;
    }
    public void SetSkin(string Name)
    {
      foreach(SkinInfo item in skinList)
      {
        if (item.Name == Name)
        {
          SetSkin(item);
          break;
        }
      }
    }
  }
  public static class Skin
  {
    private static SkinManage skinControl = new SkinManage();

    public static SkinManage SkinManage
    {
      get { return skinControl; }
    }
  }
}
