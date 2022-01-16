using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using LuaInterface;
using System;
using System.Collections.Generic;

namespace lgu3d
{

  public static class LuaHelpTools
  {

    /// <summary>
    /// 模块管理器——管理模块
    /// </summary>
    public static Manager_ManagerModel GetManager_ManagerModel
    {
      get { return Manager_ManagerModel.Instance; }
    }
    /// <summary>
    /// Lua管理器——Lua管理模块
    /// </summary>
    public static LuaManagerModule GetLuaManagerModule
    {
      get
      {
        Debug.Log("获取 LuaManagerModule");
        return LuaManagerModule.Instance;
      }
    }

    /// <summary>
    /// 加载App资源文件
    /// </summary>
    public static void LoadAppAssetInfo()
    {
      ResourceModule.Instance.LoadAppAssetInfo();
    }

    /// <summary>
    /// 加载模块资源文件
    /// </summary>
    /// <param name="ModuleName"></param>
    public static void LoadModuleAssetInfo(string ModuleName)
    {
      ResourceModule.Instance.LoadModuleAssetInfo(ModuleName);
    }

    #region Transform

    public static void SetPositionX(this Transform t, float newX)
    {
      t.position = new Vector3(newX, t.position.y, t.position.z);
    }

    public static void SetPositionY(this Transform t, float newY)
    {
      t.position = new Vector3(t.position.x, newY, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float newZ)
    {
      t.position = new Vector3(t.position.x, t.position.y, newZ);
    }

    public static void SetLocalPositionX(this Transform t, float newX)
    {
      t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
    }

    public static void SetLocalPositionY(this Transform t, float newY)
    {
      t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
    }

    public static void SetLocalPositionZ(this Transform t, float newZ)
    {
      t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZ);
    }

    public static float GetPositionX(this Transform t)
    {
      return t.position.x;
    }

    public static float GetPositionY(this Transform t)
    {
      return t.position.y;
    }

    public static float GetPositionZ(this Transform t)
    {
      return t.position.z;
    }

    /// <summary>
    /// 自动 脚本 控制
    /// Gets or add a component. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    public static T GetOrAddComponent<T>(this Component child) where T : Component
    {
      T result = child.GetComponent<T>();
      if (result == null)
      {
        try
        {
          result = child.gameObject.AddComponent<T>();
        }
        catch (Exception exception)
        {
          Debug.LogWarning("GetOrAddComponent=" + exception);
        }
      }
      return result;
    }

    /// <summary>
    /// 转换成屏幕坐标
    /// </summary>
    /// <param name="myPos"></param>
    /// <returns></returns>
    public static Vector3 ToScreenPoint(this Vector3 myPos)
    {
      return Camera.main.WorldToScreenPoint(myPos);
    }

    public static Vector3 ToViewPortPoint(this Vector3 myPos)
    {
      return Camera.main.WorldToViewportPoint(myPos);
    }

    /// <summary>
    /// 由于层次的关系，世界z轴和spibling不匹配，改成更具z轴排序spibling
    /// </summary>
    /// <param name="rectTr"></param>
    public static void SortSpiblingByZ(this Transform rectTr)
    {
      List<Transform> tempChildren = new List<Transform>();

      foreach (Transform childTr in rectTr)
      {
        tempChildren.Add(childTr);
      }

      tempChildren.Sort(new Comparison<Transform>((transform1, transform2) =>
      {
        if (!transform1) return -1;
        if (!transform2) return 1;

        if (transform1.position.z > transform2.position.z)
        {
          return 1;
        }
        if (transform1.position.z == transform2.position.z)
        {
          return 0;
        }
        return -1;
      }));
      //z轴大的放在最后面，所以翻转一下
      tempChildren.Reverse();
      int i = 0;
      foreach (var tempChild in tempChildren)
      {
        tempChild.SetSiblingIndex(i++);
      }
    }

    /// <summary>
    /// ugui 重新按照z轴排序sibling，
    /// </summary>
    /// <param name="rectTr"></param>
    /// <param name="addChild"></param>
    public static void AddChildSiblingByZ(this Transform rectTr, Transform addChild)
    {
      addChild.SetParent(rectTr);
      rectTr.SortSpiblingByZ();
    }

    /// <summary>
    /// 新生成一个子RectTransform控件，ugui
    /// </summary>
    /// <param name="parentObj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject AddRectChild(this Transform parentObj, string name = "gameObject")
    {
      GameObject go = new GameObject(name);
      go.AddComponent<RectTransform>();
      go.transform.SetParent(parentObj.transform);
      go.transform.ResetTr();
      return go;
    }

    /// <summary>
    /// 新生成一个子控件
    /// </summary>
    /// <param name="parentObj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject AddChild(this Transform parentObj, string name = "gameObject")
    {
      GameObject go = new GameObject(name);
      go.transform.SetParent(parentObj);
      go.transform.ResetTr();
      return go;
    }

    public static GameObject AddChild(this Transform parentObj, GameObject childObj)
    {
      childObj.transform.SetParent(parentObj);
      childObj.transform.ResetTr();
      return childObj;
    }

    /// <summary>
    /// 自动生成一个添加
    /// </summary>
    /// <param name="parentObj"></param>
    /// <param name="prefabObj"></param>
    /// <returns></returns>
    public static GameObject AddChildPrefab(this Transform parentObj, GameObject prefabObj)
    {
      GameObject go = UnityEngine.Object.Instantiate(prefabObj, parentObj) as GameObject;
      go.transform.ResetTr();
      go.SetActive(true);
      return go;
    }

    /// <summary>
    /// 创建游戏对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_Object"></param>
    /// <param name="Parent"></param>
    /// <returns></returns>
    public static GameObject CreateToParnt(this UnityEngine.Object _Object, string objName, GameObject Parent)
    {
      GameObject obj = GameObject.Instantiate(_Object) as GameObject;
      if (obj != null && Parent != null)
      {
        obj.name = objName;
        obj.SetParent(Parent);
      }
      return obj;
    }

    /// <summary>
    /// 快速删除全部子空间
    /// </summary>
    /// <param name="parentObj"></param>
    public static void RemoveAllChild(this Transform tr)
    {
      List<GameObject> tempList = new List<GameObject>();

      for (int i = 0; i < tr.transform.childCount; i++)
      {
        tempList.Add(tr.GetChild(i).gameObject);
      }
      tr.DetachChildren();
      foreach (GameObject obj in tempList)
      {
        UnityEngine.Object.Destroy(obj);
      }
    }

    /// <summary>
    /// 快速删除全部子空间
    /// </summary>
    /// <param name="parentObj"></param>
    public static void RemoveAllChildExcept(this Transform tr, string exceptObjName)
    {
      List<GameObject> tempList = new List<GameObject>();

      for (int i = 0; i < tr.transform.childCount; i++)
      {
        Transform obj = tr.GetChild(i);
        if (obj.name != exceptObjName)
          tempList.Add(obj.gameObject);
      }
      //tr.DetachChildren();
      foreach (GameObject obj in tempList)
      {
        UnityEngine.Object.Destroy(obj);
      }
    }

    /// <summary>
    /// 重新设置值为默认
    /// </summary>
    /// <param name="trans"></param>
    public static void ResetTr(this Transform trans)
    {
      trans.localPosition = Vector3.zero;
      trans.localRotation = new Quaternion(0, 0, 0, 0);
      trans.localScale = Vector3.one;
    }

    /// <summary>
    /// 根据z轴排序depth
    /// </summary>
    /// <param name="myTr"></param>
    /// <param name="parentTr"></param>
    public static void SetParentSiblingByZ(this Transform myTr, Transform parentTr)
    {
      myTr.SetParent(parentTr);
      parentTr.SortSpiblingByZ();
    }

    /// <summary>
    /// Deep search the heirarchy of the specified transform for the name. Uses width-first search.
    /// 深度搜索子控件
    /// </summary>
    /// <param name="t"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    static public Transform FindChildDeepSearch(this Transform t, string name)
    {
      Transform dt = t.Find(name);
      if (dt != null)
      {
        return dt;
      }

      foreach (Transform child in t)
      {
        dt = child.FindChildDeepSearch(name);
        if (dt != null)
          return dt;
      }
      return null;
    }

    /// <summary>
    /// 针对lua进行的优化，避免gameObject.transform 操作过多
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Transform GetTransform(this Component obj)
    {
      if (obj == null) return null;
      return obj.transform;
    }

    public static Transform GetTransform(this GameObject obj)
    {
      if (obj == null) return null;
      return obj.transform;
    }

    public static void SetPosition(this GameObject obj, float x, float y, float z)
    {
      if (obj)
      {
        obj.transform.position = new Vector3(x, y, z);
      }
      else
      {
        Debug.LogError("null obj!");
      }
    }

    public static void SetLocalPosition(this GameObject obj, float x, float y, float z)
    {
      if (obj)
      {
        obj.transform.localPosition = new Vector3(x, y, z);
      }
      else
      {
        Debug.LogError("null obj!");
      }
    }

    public static void SetPosition(this Component obj, float x, float y, float z)
    {
      if (obj)
      {
        obj.transform.position = new Vector3(x, y, z);
      }
      else
      {
        Debug.LogError("null obj!");
      }
    }

    public static void SetLocalPosition(this Component obj, float x, float y, float z)
    {
      if (obj)
      {
        obj.transform.localPosition = new Vector3(x, y, z);
      }
      else
      {
        Debug.LogError("null obj!");
      }
    }
    #endregion Transform

    #region GameObject 

    public static GameObject Find(this GameObject Target, string target)
    {
      Transform targetran = Target.transform.Find(target);
      if (targetran != null)
      {
        return targetran.gameObject;
      }
      else
      {
        return null;
      }
    }

    public static void SetGameObjectPos(GameObject Target, float x, float y, float z)
    {
      Target.transform.position = new Vector3(x, y, z);
    }

    public static void SetGameObjectSize(GameObject Target, float x, float y, float z)
    {

      Target.transform.localScale = new Vector3(x, y, z);
    }

    public static void SetGameObjectRotation(GameObject Target, float x, float y, float z, float w)
    {
      Target.transform.rotation = new Quaternion(x, y, z, w);
    }

    public static void SetGameObjectRotate(GameObject Target, float x, float y, float z)
    {
      Target.transform.Rotate(new Vector3(x, y, z));
    }

    public static void SetWorldTransfrom(this GameObject Target, Vector3 pos, Quaternion rot)
    {
      Target.transform.position = pos;
      Target.transform.rotation = rot;
      //Target.transform.localScale = size;
    }


    public static void SetTransfrom(this GameObject Target, Vector3 pos, Vector3 rot, Vector3 size)
    {
      Target.transform.localPosition = pos;
      Target.transform.localRotation = Quaternion.Euler(rot);
      Target.transform.localScale = size;
    }

    public static Component OnSubmit(this GameObject Target, string target, string type)
    {
      Transform obj = Target.transform.Find(target);
      if (obj != null)
      {
        Component comp = obj.GetComponent(type);
        return comp;
      }
      return null;
    }

    public static void AddClick(this GameObject Target, string target, LuaFunction fun)
    {
      GameObject targetobj = null;
      if (target != "")
      {
        targetobj = Target.transform.Find(target).gameObject;
      }
      else
      {
        targetobj = Target;
      }

      if (targetobj == null) return;
      if (targetobj.GetComponent<Button>() != null)
      {
        targetobj.GetComponent<Button>().onClick.AddListener(
            delegate ()
            {
              fun.Call(targetobj);
            }
        );
        return;
      }
      if (targetobj.GetComponent<Toggle>() != null)
      {
        targetobj.GetComponent<Toggle>().onValueChanged.AddListener(
            delegate (bool isOn)
            {
              fun.Call(isOn);
            }
        );
        return;
      }
      if (targetobj.GetComponent<InputField>() != null)
      {
        targetobj.GetComponent<InputField>().onValueChanged.AddListener(
            delegate (string Value)
            {
              fun.Call(Value);
            }
        );
        return;
      }
    }

    /// <summary>
    /// 对象添加事件
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="EventType"></param>
    /// <param name="fun"></param>
    public static void AddEvent(this GameObject Target, string target, EventTriggerType EventType, LuaFunction fun)
    {
      GameObject targetobj = null;
      if (target != "")
      {
        targetobj = Target.transform.Find(target).gameObject;
      }
      else
      {
        targetobj = Target;
      }
      if (targetobj == null) return;
      EventTrigger Trigger = targetobj.GetComponent<EventTrigger>();
      if (Trigger != null)
      {
        UnityAction<BaseEventData> _BackCall = new UnityAction<BaseEventData>(delegate (BaseEventData data) { fun.Call((PointerEventData)data); });
        EventTrigger.Entry MyEvent = new EventTrigger.Entry();
        MyEvent.eventID = EventType;
        MyEvent.callback.AddListener(_BackCall);
        Trigger.triggers.Add(MyEvent);
      }
    }

    #endregion

    #region UGUIEventListener

    /// <summary>
    /// 主要在ui界面注册点击事件,并只能点击一次
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetOnClick(this Component obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj.gameObject).onClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          //LuaHelper.GetSoundManager().PlaySound("Shared", "anniu1", ViewHelper.Instance.UICamera.transform.position, 1);
          callback.Call(obj, evt, go, args);
          //callback = null;
        }
      };
    }

    /// <summary>
    /// 主要用于UI界面的双击事件，并只有效一次回调
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetOnDoubleClick(this Component obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj.gameObject).onDoubleClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }

    /// <summary>
    /// 主要在ui界面注册点击事件,并只能点击一次
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetOnClick(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);

        }
      };
    }

    public static void SetOnHover(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onHover = (evt, go, state, my_args) =>
      {
        if (callback != null)
        {
          if (state)
          {
          }
          callback.Call(obj, evt, go, state, args);
        }
      };
    }

    public static void SetOnPress(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onPress = (evt, go, state, my_args) =>
      {
        if (callback != null)
        {
          if (state)
          {

          }
          callback.Call(obj, evt, go, state, args);

        }
      };
    }

    /// <summary>
    /// 主要在ui界面注册点击事件,并只能点击一次
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void Set3DOnClick(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }


    public static void SetOnDragStart(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onDragStart = (evt, go, data, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, data, args);
        }
      };
    }

    public static void SetOnDarg(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onDrag = (evt, go, data, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, data, args);
        }
      };
    }

    public static void SetOnDragEnd(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onDragEnd = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }

    /// <summary>
    /// 长按2秒事件
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetLongPress(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onLongPress = (evt, go, isPress, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }

    public static void SetLongPress(this Component obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj.gameObject).onLongPress = (evt, go, isPress, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }

    /// <summary>
    /// 添加一次点击事件
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetOneClick(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
          callback = null;
        }
      };
    }

    /// <summary>
    /// 主要用于UI界面的双击事件，并只有效一次回调
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void SetOnDoubleClick(this GameObject obj, LuaFunction callback, object args = null)
    {
      UGUIEventListener.Get(obj).onDoubleClick = (evt, go, my_args) =>
      {
        if (callback != null)
        {
          callback.Call(obj, evt, go, args);
        }
      };
    }

    #endregion GameObject

    #region ZipTool
    public static IEnumerator UnzipFile(byte[] _fileBytes, string _outputPath, string _password = null, LuaFunction fun = null)
    {
      return ZipTools.UnzipFile(_fileBytes, _outputPath, _password, (Describe, Progress) =>
      {
        if (fun != null)
        {
          fun.Call(Describe, Progress);
        }
      });
    }

    #endregion
  }

}


