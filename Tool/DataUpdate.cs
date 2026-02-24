using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
  public class DataUpdate
  {
    private string Key = "";

    Action<object, object> Value = null;

    private static ConcurrentDictionary<string, Action<object, object>> _UpdateList = new ConcurrentDictionary<string, Action<object, object>>();
    public static ConcurrentDictionary<string, Action<object, object>> UpdateList
    {
      get => _UpdateList;
    }

    public DataUpdate()
    {

    }

    public DataUpdate(string ID)
    {
      Key = ID;
    }

    public void SetAction(Action<object, object> action)
    {
      if (!UpdateList.ContainsKey(Key))
      {
        UpdateList.TryAdd(Key, action);
      }
      else
      {
        UpdateList.TryRemove(Key, out Action<object, object> reAction);
        UpdateList.TryAdd(Key, action);
      }
      Value = action;
    }

    public static void DoUpdate(string Key, object param01 = null, object param02 = null)
    {
      if (UpdateList.ContainsKey(Key))
      {
        UpdateList[Key](param01, param02);
      }
    }


    ~DataUpdate()
    {
      if (UpdateList.ContainsKey(Key))
      {
        if (UpdateList[Key] == Value)
        {
          UpdateList.TryRemove(Key, out Action<object, object> reAction);
        }

      }
    }


  }
}
