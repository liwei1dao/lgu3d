using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace lgu3d
{

  public static class GameObjectExtend
  {
    public static string GetPath(this Transform current)
    {
      if (current.parent == null)
        return "/" + current.name;
      return current.parent.GetPath() + "/" + current.name;
    }

    /// <summary>
    /// 查找添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T AddMissingComponent<T>(this GameObject go) where T : Component
    {
      T comp = go.GetComponent<T>();
      if (comp == null)
      {
        comp = go.AddComponent<T>();
      }
      return comp;
    }

    /// <summary>
    /// 查找添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static Component AddMissingComponent(this GameObject go, Type componentType)
    {
      Component comp = go.GetComponent(componentType);
      if (comp == null)
      {
        comp = go.AddComponent(componentType);
      }
      return comp;
    }

    /// <summary>
    /// 查找添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T AddMissingComponent<T>(this Transform go) where T : Component
    {
      T comp = go.GetComponent<T>();
      if (comp == null)
      {
        comp = go.gameObject.AddComponent<T>();
      }
      return comp;
    }


    /// <summary>
    /// 创建游戏对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_object"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject CreateToParnt(this UnityEngine.Object _object, GameObject parent)
    {
      GameObject obj = GameObject.Instantiate(_object) as GameObject;
      if (obj != null && parent != null)
      {
        obj.SetParent(parent);
      }
      return obj;
    }
    /// <summary>
    /// 创建子对象
    /// </summary>
    /// <param name="Parent"></param>
    public static GameObject CreateChild(this GameObject parent, string name, params Type[] components)
    {
      GameObject child = new GameObject(name, components);
      child.SetParent(parent);
      return child;
    }


    /// <summary>
    ///  清理子对象
    /// </summary>
    /// <param name="Target"></param>
    public static void ClearChilds(this GameObject Target)
    {
      for (int i = 0; i < Target.transform.childCount; i++)
      {
        Transform child = Target.transform.GetChild(i);
        GameObject.Destroy(child.gameObject);
      }
    }


    /// <summary>
    /// 設置父物体对象
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="Parent"></param>
    public static void SetParent(this GameObject Target, GameObject Parent)
    {
      Target.transform.SetParent(Parent.transform);
      Target.transform.localPosition = Vector3.zero;
      Target.transform.localScale = Vector3.one;
      Target.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 設置父物体对象
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="Parent"></param>
    public static void SetParent(this GameObject Target, GameObject Parent, Vector3 Pos, Vector3 Scale, Quaternion Rotation)
    {
      Target.transform.SetParent(Parent.transform);
      Target.transform.localPosition = Pos;
      Target.transform.localScale = Scale;
      Target.transform.localRotation = Rotation;
    }


    /// <summary>
    /// 设置对象以及子对象层
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="layer"></param>
    public static void SetLayer(this GameObject Target, string layer)
    {
      Target.layer = LayerMask.NameToLayer(layer);
      for (int i = 0; i < Target.transform.childCount; i++)
      {
        Target.transform.GetChild(i).gameObject.SetLayer(layer);
      }
    }
    /// <summary>
    /// 设置对象以及子对象层
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="layer"></param>
    public static void SetLayer(this GameObject Target, LayerMask layer)
    {
      Target.layer = layer;
      for (int i = 0; i < Target.transform.childCount; i++)
      {
        Target.transform.GetChild(i).gameObject.SetLayer(layer);
      }
    }

    /// <summary>
    /// 设置对象trans
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="Parent"></param>
    public static void SetTrans(this GameObject Target, Transform Parent)
    {
      Target.transform.position = Parent.position;
      Target.transform.localScale = Vector3.one;
      Target.transform.rotation = Parent.rotation;
    }



    /// <summary>
    /// 找到子节点的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Target"></param>
    /// <param name="Childpath"></param>
    /// <returns></returns>
    public static T OnSubmit<T>(this GameObject Target, string Childpath) where T : Component
    {
      if (Childpath != "")
      {
        Transform obj = Target.transform.Find(Childpath);
        if (obj != null)
        {
          return obj.GetComponent<T>();
        }
        return null;
      }
      else
      {
        return Target.GetComponent<T>();
      }
    }

    public static void OnAddClick(this GameObject Target, string Childpath, UnityAction call)
    {
      Button butt = Target.OnSubmit<Button>(Childpath);
      butt.onClick.AddListener(call);
    }
  }
}
